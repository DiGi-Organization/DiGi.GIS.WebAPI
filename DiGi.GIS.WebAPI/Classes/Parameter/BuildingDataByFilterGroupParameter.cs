using DiGi.PostgreSQL.Table.Classes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for building data queries using dynamic hierarchical filters.
    /// </summary>
    public class BuildingDataByFilterGroupParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Gets or sets the dynamic hierarchical filter group to apply to the database query.
        /// </summary>
        [Required]
        public FilterGroup FilterGroup { get; set; } = null!;

        /// <summary>
        /// Gets or sets the optional list of column unique identifiers to project in the result.
        /// </summary>
        public List<string>? ColumnUniqueIds { get; set; }

        /// <summary>
        /// Gets or sets the county partition to read; <c>null</c> reads across all partitions.
        /// <para>The table is partitioned by <c>county_id</c>. Omitting this (and not filtering <c>county_id</c> in <see cref="FilterGroup"/>) makes the read span every partition of the national table; supply it unless a cross-partition read is intended.</para>
        /// </summary>
        /// <example>10365</example>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the optional administrative area the read is confined to, instead of <see cref="CountyId"/>; the two are mutually exclusive.
        /// <para>A Subdivision is resolved through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <example>55413</example>
        public int? AdministrativeAreal2DId { get; set; }

        /// <summary>
        /// Gets or sets the optional sort order of the returned rows, applied in list order.
        /// <para>Each entry names a column the stored table must hold (validated like a filter column) and its direction; null or empty reads unordered. Null values sort last in both directions, and equal sort values are tie-broken deterministically by <c>reference</c> unless the list already sorts by it. A column the table does not hold is refused with 400 (DiGi-Organization/DiGi.GIS.WebAPI#84).</para>
        /// </summary>
        public List<SortCondition>? SortConditions { get; set; }

        /// <summary>
        /// Gets or sets the optional maximum number of rows to return, for top-N questions ("the tallest building", "the largest five").
        /// <para>Accepted values are 1 to 1000; outside that range the request is refused with 400, never clamped. Null reads every matching row. A limit without <see cref="SortConditions"/> returns that many rows ordered by <c>reference</c> (DiGi-Organization/DiGi.GIS.WebAPI#84).</para>
        /// </summary>
        /// <example>1</example>
        public int? Limit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class.
        /// </summary>
        public BuildingDataByFilterGroupParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object. The sort conditions are copied into a new list, the items shared.</para>
        /// </summary>
        /// <param name="buildingDataByFilterGroupParameter">The instance to copy; null leaves every member at its default.</param>
        public BuildingDataByFilterGroupParameter(BuildingDataByFilterGroupParameter buildingDataByFilterGroupParameter)
            : base(buildingDataByFilterGroupParameter)
        {
            if (buildingDataByFilterGroupParameter is not null)
            {
                FilterGroup = buildingDataByFilterGroupParameter.FilterGroup;
                ColumnUniqueIds = buildingDataByFilterGroupParameter.ColumnUniqueIds is null ? null : [.. buildingDataByFilterGroupParameter.ColumnUniqueIds];
                CountyId = buildingDataByFilterGroupParameter.CountyId;
                AdministrativeAreal2DId = buildingDataByFilterGroupParameter.AdministrativeAreal2DId;
                SortConditions = buildingDataByFilterGroupParameter.SortConditions is null ? null : [.. buildingDataByFilterGroupParameter.SortConditions];
                Limit = buildingDataByFilterGroupParameter.Limit;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public BuildingDataByFilterGroupParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }
    }
}