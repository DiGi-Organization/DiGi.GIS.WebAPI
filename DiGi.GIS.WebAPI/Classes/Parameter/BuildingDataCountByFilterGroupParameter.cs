using DiGi.PostgreSQL.Table.Classes;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for counting building data rows matching an optional dynamic hierarchical filter group.
    /// </summary>
    public class BuildingDataCountByFilterGroupParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataCountByFilterGroupParameter"/> class.
        /// </summary>
        public BuildingDataCountByFilterGroupParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataCountByFilterGroupParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public BuildingDataCountByFilterGroupParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the optional partition (county) to count; <c>null</c> counts across all partitions.
        /// </summary>
        /// <example>10365</example>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to counting; <c>null</c> (or an empty group) counts the whole partition scope.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}