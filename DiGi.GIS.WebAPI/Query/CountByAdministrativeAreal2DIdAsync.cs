using DiGi.GIS.PostgreSQL.Classes;
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
        /// Asynchronously counts the building data rows of an administrative area, optionally narrowed by a filter group.
        /// <para>The area is resolved to its county partitions by <see cref="AdministrativeAreal2DScopesAsync"/> - a Subdivision through its polygon, a County as its own partition, a higher level through its Subdivision children (DiGi-Organization/DiGi.GIS.WebAPI#83) - and each partition is counted with its scope's filter group, the counts summed.</para>
        /// <para>Every step is awaited in turn - the reads are never fanned out, because concurrent requests exhaust the Npgsql pool. The steps are taken as delegates so the orchestration is separate from the converters that perform them.</para>
        /// </summary>
        /// <param name="administrativeAreal2DId">The identifier of the administrative area.</param>
        /// <param name="filterGroup">The optional filters applied on top of the area.</param>
        /// <param name="readReferenceAsync">Reads the area reference by identifier; returns null when there is none.</param>
        /// <param name="readSubdivisionsAsync">Reads the Subdivision references below the area, given the area identifier.</param>
        /// <param name="readBuilding2DReferencesAsync">Reads the building references inside a Subdivision's polygon, given the Subdivision identifier; returns null when the read could not be made.</param>
        /// <param name="countAsync">Counts the rows of one county partition matching a filter group, given the filter group (null counts the whole partition) and the county identifier; returns null when the count could not be made.</param>
        /// <param name="maximumCountyCount">The largest number of county partitions that may be counted; exceeding it counts nothing.</param>
        /// <param name="maximumReferenceCount">The largest number of building references a Subdivision's polygon may resolve to; exceeding it counts nothing.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The result carries how the count ended and, for <see cref="AdministrativeAreal2DCountStatus.Counted"/>, the summed count.</returns>
        public static async Task<(AdministrativeAreal2DCountStatus Status, long Count)> CountByAdministrativeAreal2DIdAsync(
            int administrativeAreal2DId,
            FilterGroup? filterGroup,
            Func<int, CancellationToken, Task<AdministrativeAreal2DReference?>> readReferenceAsync,
            Func<int, CancellationToken, Task<List<AdministrativeAreal2DReference>?>> readSubdivisionsAsync,
            Func<int, CancellationToken, Task<List<Building2DReference>?>> readBuilding2DReferencesAsync,
            Func<FilterGroup?, int, CancellationToken, Task<long?>> countAsync,
            int maximumCountyCount,
            int maximumReferenceCount,
            CancellationToken cancellationToken = default)
        {
            (AdministrativeAreal2DCountStatus Status, List<AdministrativeAreal2DScope> AdministrativeAreal2DScopes) scopes = await AdministrativeAreal2DScopesAsync(
                administrativeAreal2DId,
                filterGroup,
                readReferenceAsync,
                readSubdivisionsAsync,
                readBuilding2DReferencesAsync,
                maximumCountyCount,
                maximumReferenceCount,
                cancellationToken);

            if (scopes.Status != AdministrativeAreal2DCountStatus.Counted)
            {
                return (scopes.Status, 0);
            }

            long count = 0;
            foreach (AdministrativeAreal2DScope administrativeAreal2DScope in scopes.AdministrativeAreal2DScopes)
            {
                long? count_County = await countAsync(administrativeAreal2DScope.FilterGroup, administrativeAreal2DScope.CountyId, cancellationToken);
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
