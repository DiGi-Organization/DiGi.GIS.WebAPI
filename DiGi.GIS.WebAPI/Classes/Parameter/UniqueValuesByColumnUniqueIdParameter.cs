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
        /// Initializes a new instance of the <see cref="UniqueValuesByColumnUniqueIdParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object.</para>
        /// </summary>
        /// <param name="uniqueValuesByColumnUniqueIdParameter">The instance to copy; null leaves every member at its default.</param>
        public UniqueValuesByColumnUniqueIdParameter(UniqueValuesByColumnUniqueIdParameter uniqueValuesByColumnUniqueIdParameter)
            : base(uniqueValuesByColumnUniqueIdParameter)
        {
            if (uniqueValuesByColumnUniqueIdParameter is not null)
            {
                ColumnUniqueId = uniqueValuesByColumnUniqueIdParameter.ColumnUniqueId;
                CountyId = uniqueValuesByColumnUniqueIdParameter.CountyId;
                AdministrativeAreal2DId = uniqueValuesByColumnUniqueIdParameter.AdministrativeAreal2DId;
                FilterGroup = uniqueValuesByColumnUniqueIdParameter.FilterGroup;
            }
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
        /// Gets or sets the optional administrative area the read is confined to, instead of <see cref="CountyId"/>; the two are mutually exclusive.
        /// <para>A Subdivision is resolved through its polygon (every building whose bounding-box centre lies inside it, whatever subdivision it is filed under), a County as its own partition, a higher level through its Subdivision children. The area must resolve to one county partition (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        /// <example>55413</example>
        public int? AdministrativeAreal2DId { get; set; }

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to retrieving unique values.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}