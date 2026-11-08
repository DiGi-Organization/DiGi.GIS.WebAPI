using DiGi.PostgreSQL.Table.Classes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Represents a parameter containing column unique id for querying unique values.
    /// </summary>
    public class UniqueValuesByColumnUniqueIdParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueValuesByColumnUniqueIdParameter"/> class.
        /// </summary>
        public UniqueValuesByColumnUniqueIdParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueValuesByColumnUniqueIdParameter"/> class with the specified column unique id and county id.
        /// </summary>
        /// <param name="columnUniqueId">The unique identifier of the column.</param>
        /// <param name="countyId">The optional unique identifier of the county.</param>
        public UniqueValuesByColumnUniqueIdParameter(string columnUniqueId, int? countyId)
        {
            ColumnUniqueId = columnUniqueId;
            CountyId = countyId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueValuesByColumnUniqueIdParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="System.Text.Json.Nodes.JsonObject" /> containing the data used to initialize the parameter.</param>
        public UniqueValuesByColumnUniqueIdParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the unique identifier of the column (Column.UniqueId).
        /// </summary>
        [Required]
        public string ColumnUniqueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the county identifier.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to retrieving unique values.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}