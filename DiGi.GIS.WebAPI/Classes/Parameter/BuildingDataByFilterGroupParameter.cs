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
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class.
        /// </summary>
        public BuildingDataByFilterGroupParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object.</para>
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