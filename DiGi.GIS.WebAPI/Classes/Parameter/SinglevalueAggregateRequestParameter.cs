using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.ComponentModel.DataAnnotations;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for single-value database aggregation queries.
    /// </summary>
    public class SinglevalueAggregateRequestParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SinglevalueAggregateRequestParameter"/> class.
        /// </summary>
        public SinglevalueAggregateRequestParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglevalueAggregateRequestParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object.</para>
        /// </summary>
        /// <param name="singlevalueAggregateRequestParameter">The instance to copy; null leaves every member at its default.</param>
        public SinglevalueAggregateRequestParameter(SinglevalueAggregateRequestParameter singlevalueAggregateRequestParameter)
            : base(singlevalueAggregateRequestParameter)
        {
            if (singlevalueAggregateRequestParameter is not null)
            {
                CountyId = singlevalueAggregateRequestParameter.CountyId;
                AdministrativeAreal2DId = singlevalueAggregateRequestParameter.AdministrativeAreal2DId;
                ColumnUniqueId = singlevalueAggregateRequestParameter.ColumnUniqueId;
                SinglevalueAggregateFunction = singlevalueAggregateRequestParameter.SinglevalueAggregateFunction;
                FilterGroup = singlevalueAggregateRequestParameter.FilterGroup;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SinglevalueAggregateRequestParameter"/> class using a <see cref="System.Text.Json.Nodes.JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public SinglevalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the target partition identifier (County ID). If null, aggregation is performed across all partitions.
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
        /// Gets or sets the column unique identifier to calculate statistics for.
        /// </summary>
        /// <example>"col_unique_id_123"</example>
        [Required]
        public string ColumnUniqueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the single-value aggregation function to perform.
        /// </summary>
        /// <example>Sum</example>
        [Required]
        public SinglevalueAggregateFunction SinglevalueAggregateFunction { get; set; }

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to aggregation.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}