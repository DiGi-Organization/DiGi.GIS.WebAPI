using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using DiGi.GIS.WebAPI.Classes;
using DiGi.GIS.WebAPI.Enums;
using DiGi.PostgreSQL.Table.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously resolves an administrative area to the <c>building_data</c> county partitions its buildings sit in, each with the filter group that confines a read to those buildings and the caller's filters nested inside.
        /// <para>Resolution by level (DiGi-Organization/DiGi.GIS.WebAPI#83):</para>
        /// <para>- A <b>Subdivision</b> resolves through its <b>polygon</b>: the buildings whose bounding-box centre lies strictly inside it, grouped by county partition, each partition confined by <c>reference IN (…)</c>. The Subdivision layer nests and <c>subdivision_id</c> files a building under its smallest containing subdivision only, so membership cannot name the buildings of a district; a subdivision that only partly overlaps the polygon contributes exactly the buildings inside it.</para>
        /// <para>- A <b>County</b> resolves to its own partition, unconfined: the partition is the county.</para>
        /// <para>- A <b>Municipality</b>, <b>Voivodeship</b> or <b>Country</b> resolves to its Subdivision children, grouped by county partition, each confined by <c>subdivision_id IN (…)</c>. Rows with a NULL <c>subdivision_id</c> are not reached on this path.</para>
        /// <para>The reads are awaited in turn, never fanned out, because concurrent requests exhaust the Npgsql pool. They are taken as delegates so the resolution is separate from the converters that perform them; the area reads run against the main database and the scopes are then applied to the storage database, because <c>building_2d</c> and <c>building_data</c> never share a statement.</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The identifier of the administrative area.</param>
        /// <param name="filterGroup">The optional filters of the caller, nested inside every scope.</param>
        /// <param name="readReferenceAsync">Reads the area reference by identifier; returns null when there is none.</param>
        /// <param name="readSubdivisionsAsync">Reads the Subdivision references below the area, given the area identifier.</param>
        /// <param name="readBuilding2DReferencesAsync">Reads the building references inside a Subdivision's polygon, given the Subdivision identifier; returns null when the read could not be made.</param>
        /// <param name="maximumCountyCount">The largest number of county partitions the area may resolve to.</param>
        /// <param name="maximumReferenceCount">The largest number of building references a Subdivision's polygon may resolve to.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The result carries how the resolution ended and, for <see cref="AdministrativeAreal2DCountStatus.Counted"/>, the scopes in the order a county partition was first met; on any other status the list is empty.</returns>
        public static async Task<(AdministrativeAreal2DCountStatus Status, List<AdministrativeAreal2DScope> AdministrativeAreal2DScopes)> AdministrativeAreal2DScopesAsync(
            int administrativeAreal2DId,
            FilterGroup? filterGroup,
            Func<int, CancellationToken, Task<AdministrativeAreal2DReference?>> readReferenceAsync,
            Func<int, CancellationToken, Task<List<AdministrativeAreal2DReference>?>> readSubdivisionsAsync,
            Func<int, CancellationToken, Task<List<Building2DReference>?>> readBuilding2DReferencesAsync,
            int maximumCountyCount,
            int maximumReferenceCount,
            CancellationToken cancellationToken = default)
        {
            List<AdministrativeAreal2DScope> result = [];

            AdministrativeAreal2DReference? administrativeAreal2DReference = await readReferenceAsync(administrativeAreal2DId, cancellationToken);
            if (administrativeAreal2DReference is null)
            {
                return (AdministrativeAreal2DCountStatus.AreaNotFound, result);
            }

            switch (administrativeAreal2DReference.AdministrativeArealType)
            {
                case AdministrativeArealType.County:
                    result.Add(new AdministrativeAreal2DScope(administrativeAreal2DReference.Id, filterGroup));
                    return (AdministrativeAreal2DCountStatus.Counted, result);

                case AdministrativeArealType.Subdivision:
                    List<Building2DReference>? building2DReferences = await readBuilding2DReferencesAsync(administrativeAreal2DReference.Id, cancellationToken);
                    if (building2DReferences is null)
                    {
                        return (AdministrativeAreal2DCountStatus.NotResolved, result);
                    }

                    Dictionary<int, List<string>> references_ByCountyId = ReferencesByCountyId(building2DReferences);
                    if (references_ByCountyId.Count == 0)
                    {
                        return (AdministrativeAreal2DCountStatus.NoBuildings, result);
                    }

                    if (references_ByCountyId.Count > maximumCountyCount)
                    {
                        return (AdministrativeAreal2DCountStatus.TooManyCounties, result);
                    }

                    int referenceCount = 0;
                    foreach (List<string> references in references_ByCountyId.Values)
                    {
                        referenceCount += references.Count;
                    }

                    if (referenceCount > maximumReferenceCount)
                    {
                        return (AdministrativeAreal2DCountStatus.TooManyReferences, result);
                    }

                    foreach (KeyValuePair<int, List<string>> keyValuePair in references_ByCountyId)
                    {
                        FilterGroup? filterGroup_County = Create.ReferenceFilterGroup(filterGroup, keyValuePair.Value);
                        if (filterGroup_County is not null)
                        {
                            result.Add(new AdministrativeAreal2DScope(keyValuePair.Key, filterGroup_County));
                        }
                    }

                    return (AdministrativeAreal2DCountStatus.Counted, result);

                default:
                    List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await readSubdivisionsAsync(administrativeAreal2DReference.Id, cancellationToken);
                    Dictionary<int, List<int>> subdivisionIds_ByCountyId = SubdivisionIdsByCountyId(administrativeAreal2DReferences);
                    if (subdivisionIds_ByCountyId.Count == 0)
                    {
                        return (AdministrativeAreal2DCountStatus.NoSubdivisions, result);
                    }

                    if (subdivisionIds_ByCountyId.Count > maximumCountyCount)
                    {
                        return (AdministrativeAreal2DCountStatus.TooManyCounties, result);
                    }

                    foreach (KeyValuePair<int, List<int>> keyValuePair in subdivisionIds_ByCountyId)
                    {
                        FilterGroup? filterGroup_County = Create.SubdivisionFilterGroup(filterGroup, keyValuePair.Value);
                        if (filterGroup_County is not null)
                        {
                            result.Add(new AdministrativeAreal2DScope(keyValuePair.Key, filterGroup_County));
                        }
                    }

                    return (AdministrativeAreal2DCountStatus.Counted, result);
            }
        }
    }
}
