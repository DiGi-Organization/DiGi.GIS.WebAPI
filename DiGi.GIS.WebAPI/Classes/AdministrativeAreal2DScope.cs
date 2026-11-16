using DiGi.PostgreSQL.Table.Classes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// One county partition of <c>building_data</c> and the filter group that confines a read of it to the buildings of an administrative area.
    /// <para>An administrative area resolves to one scope per county partition its buildings sit in; see <see cref="Query.AdministrativeAreal2DScopesAsync"/>.</para>
    /// </summary>
    public class AdministrativeAreal2DScope
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdministrativeAreal2DScope"/> class.
        /// </summary>
        /// <param name="countyId">The identifier of the county partition to read.</param>
        /// <param name="filterGroup">The filter group that confines the read to the area, with the caller's own filters nested inside; null reads the whole partition.</param>
        public AdministrativeAreal2DScope(int countyId, FilterGroup? filterGroup)
        {
            CountyId = countyId;
            FilterGroup = filterGroup;
        }

        /// <summary>
        /// Gets the identifier of the county partition to read.
        /// </summary>
        public int CountyId { get; }

        /// <summary>
        /// Gets the filter group that confines the read to the area, with the caller's own filters nested inside; null reads the whole partition.
        /// </summary>
        public FilterGroup? FilterGroup { get; }
    }
}
