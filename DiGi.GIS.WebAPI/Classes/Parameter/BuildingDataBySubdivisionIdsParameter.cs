using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Represents a parameter containing subdivision ids for querying building data.
    /// </summary>
    public class BuildingDataBySubdivisionIdsParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataBySubdivisionIdsParameter"/> class.
        /// </summary>
        public BuildingDataBySubdivisionIdsParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataBySubdivisionIdsParameter"/> class using an existing <see cref="BuildingDataBySubdivisionIdsParameter"/> object.
        /// </summary>
        /// <param name="buildingDataBySubdivisionIdsParameter">The parameter object to copy data from.</param>
        public BuildingDataBySubdivisionIdsParameter(BuildingDataBySubdivisionIdsParameter buildingDataBySubdivisionIdsParameter)
            : base(buildingDataBySubdivisionIdsParameter)
        {
            if (buildingDataBySubdivisionIdsParameter is not null)
            {
                SubdivisionIds = buildingDataBySubdivisionIdsParameter.SubdivisionIds;
                ColumnUniqueIds = buildingDataBySubdivisionIdsParameter.ColumnUniqueIds;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataBySubdivisionIdsParameter"/> class using a JSON object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing parameter data.</param>
        public BuildingDataBySubdivisionIdsParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the unique identifiers of the columns (Column.UniqueId). All columns will be returned if the collection is null or empty.
        /// Required if performance is a concern and the column unique identifiers are available; otherwise, the column unique identifiers will be determined by the building data PostgreSQL converter.
        /// </summary>
        public IEnumerable<string>? ColumnUniqueIds { get; set; }

        /// <summary>
        /// Gets or sets the subdivision ids for the building data
        /// </summary>
        [Required]
        public IEnumerable<int> SubdivisionIds { get; set; } = [];
    }
}