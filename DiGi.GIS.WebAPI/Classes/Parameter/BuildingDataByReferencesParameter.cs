using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Represents a parameter containing references for querying building data.
    /// </summary>
    public class BuildingDataByReferencesParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByReferencesParameter"/> class.
        /// </summary>
        public BuildingDataByReferencesParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByReferencesParameter"/> class using an existing <see cref="BuildingDataByReferencesParameter"/> object.
        /// </summary>
        /// <param name="buildingDataByReferencesParameter">The parameter object to copy data from.</param>
        public BuildingDataByReferencesParameter(BuildingDataByReferencesParameter buildingDataByReferencesParameter)
            : base(buildingDataByReferencesParameter)
        {
            if (buildingDataByReferencesParameter is not null)
            {
                References = buildingDataByReferencesParameter.References;
                CountyId = buildingDataByReferencesParameter.CountyId;
                ColumnUniqueIds = buildingDataByReferencesParameter.ColumnUniqueIds;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByReferencesParameter"/> class using a JSON object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing parameter data.</param>
        public BuildingDataByReferencesParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the unique identifiers of the columns (Column.UniqueId). All columns will be returned if the collection is null or empty.
        /// Required if performance is a concern and the column unique identifiers are available; otherwise, the column unique identifiers will be determined by the building data PostgreSQL converter.
        /// </summary>
        public IEnumerable<string>? ColumnUniqueIds { get; set; }

        /// <summary>
        /// Gets or sets the county identifier.
        /// Required if performance is a concern and the county identifier is available; otherwise, the county identifier will be determined by the building data PostgreSQL converter.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the references for the building data parameter.
        /// </summary>
        [Required]
        public IEnumerable<string> References { get; set; } = [];
    }
}