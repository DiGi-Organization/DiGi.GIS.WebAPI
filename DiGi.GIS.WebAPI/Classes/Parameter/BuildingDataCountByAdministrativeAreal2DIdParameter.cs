using DiGi.PostgreSQL.Table.Classes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for counting the building data rows of an administrative area matching an optional dynamic hierarchical filter group.
    /// </summary>
    public class BuildingDataCountByAdministrativeAreal2DIdParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataCountByAdministrativeAreal2DIdParameter"/> class.
        /// </summary>
        public BuildingDataCountByAdministrativeAreal2DIdParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataCountByAdministrativeAreal2DIdParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public BuildingDataCountByAdministrativeAreal2DIdParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the identifier of the administrative area whose buildings are counted.
        /// <para>Nullable so that an omitted property is distinguishable from a legitimate value and can be refused with HTTP 400 rather than silently meaning <c>0</c>.</para>
        /// </summary>
        /// <example>53469</example>
        [Required]
        public int? AdministrativeAreal2DId { get; set; }

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters applied on top of the area; <c>null</c> (or an empty group) counts every building the area resolves to.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}
