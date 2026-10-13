using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
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
        /// Asynchronously counts the building data rows of an administrative area, optionally narrowed by a filter group.
        /// <para>An area above the subdivision level is resolved to its Subdivision children, which are grouped by the county partition they sit in; each county is then counted with <c>subdivision_id IN (that county's subdivisions)</c> plus the caller's filters, and the counts are summed. A Subdivision is counted by membership of its own id, so a nested subdivision counts only the buildings filed under that exact id (DiGi.GIS.PostgreSQL#75).</para>
        /// <para>Every step is awaited in turn - the reads are never fanned out, because concurrent requests exhaust the Npgsql pool. The three steps are taken as delegates so the orchestration is separate from the converters that perform them.</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The identifier of the administrative area.</param>
        /// <param name="filterGroup">The optional filters applied on top of the area.</param>
        /// <param name="readReferenceAsync">Reads the area reference by identifier; returns null when there is none.</param>
        /// <param name="readSubdivisionsAsync">Reads the Subdivision references below the area, given the area identifier.</param>
        /// <param name="countAsync">Counts the rows of one county partition matching a filter group, given the filter group and the county identifier; returns null when the count could not be made.</param>
        /// <param name="maximumCountyCount">The largest number of counties that may be iterated; exceeding it counts nothing.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The result carries how the count ended and, for <see cref="AdministrativeAreal2DCountStatus.Counted"/>, the summed count.</returns>
        public static async Task<(AdministrativeAreal2DCountStatus Status, long Count)> CountByAdministrativeAreal2DIdAsync(int administrativeAreal2DId, FilterGroup? filterGroup, Func<int, CancellationToken, Task<AdministrativeAreal2DReference?>> readReferenceAsync, Func<int, CancellationToken, Task<List<AdministrativeAreal2DReference>?>> readSubdivisionsAsync, Func<FilterGroup, int, CancellationToken, Task<long?>> countAsync, int maximumCountyCount, CancellationToken cancellationToken = default)
        {
            AdministrativeAreal2DReference? administrativeAreal2DReference = await readReferenceAsync(administrativeAreal2DId, cancellationToken);
            if (administrativeAreal2DReference is null)
            {
                return (AdministrativeAreal2DCountStatus.AreaNotFound, 0);
            }

            Dictionary<int, List<int>> subdivisionIds_ByCountyId;
            if (administrativeAreal2DReference.AdministrativeArealType == AdministrativeArealType.Subdivision)
            {
                subdivisionIds_ByCountyId = SubdivisionIdsByCountyId([administrativeAreal2DReference]);
            }
            else
            {
                List<AdministrativeAreal2DReference>? administrativeAreal2DReferences = await readSubdivisionsAsync(administrativeAreal2DId, cancellationToken);
                subdivisionIds_ByCountyId = SubdivisionIdsByCountyId(administrativeAreal2DReferences);
            }

            if (subdivisionIds_ByCountyId.Count == 0)
            {
                return (AdministrativeAreal2DCountStatus.NoSubdivisions, 0);
            }

            if (subdivisionIds_ByCountyId.Count > maximumCountyCount)
            {
                return (AdministrativeAreal2DCountStatus.TooManyCounties, 0);
            }

            long count = 0;
            foreach (KeyValuePair<int, List<int>> keyValuePair in subdivisionIds_ByCountyId)
            {
                FilterGroup? filterGroup_County = Create.SubdivisionFilterGroup(filterGroup, keyValuePair.Value);
                if (filterGroup_County is null)
                {
                    continue;
                }

                long? count_County = await countAsync(filterGroup_County, keyValuePair.Key, cancellationToken);
                if (count_County is null)
                {
                    return (AdministrativeAreal2DCountStatus.NotResolved, 0);
                }

                count += count_County.Value;
            }

            return (AdministrativeAreal2DCountStatus.Counted, count);
        }
    }
}
