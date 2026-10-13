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
        /// Initializes a new instance of the <see cref="BuildingDataByFilterGroupParameter"/> class.
        /// </summary>
        public BuildingDataByFilterGroupParameter()
        {
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