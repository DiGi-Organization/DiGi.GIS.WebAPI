using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for a batch of single-value database aggregation queries: several measures over one shared partition and filter, served in one request (DiGi-Organization/DiGi.GIS.WebAPI#56).
    /// </summary>
    public class AggregateSummariesRequestParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateSummariesRequestParameter"/> class.
        /// </summary>
        public AggregateSummariesRequestParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateSummariesRequestParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object.</para>
        /// </summary>
        /// <param name="aggregateSummariesRequestParameter">The instance to copy; null leaves every member at its default.</param>
        public AggregateSummariesRequestParameter(AggregateSummariesRequestParameter aggregateSummariesRequestParameter)
            : base(aggregateSummariesRequestParameter)
        {
            if (aggregateSummariesRequestParameter is not null)
            {
                CountyId = aggregateSummariesRequestParameter.CountyId;
                AdministrativeAreal2DId = aggregateSummariesRequestParameter.AdministrativeAreal2DId;
                FilterGroup = aggregateSummariesRequestParameter.FilterGroup;
                Measures = aggregateSummariesRequestParameter.Measures is null ? [] : [.. aggregateSummariesRequestParameter.Measures];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateSummariesRequestParameter"/> class using a <see cref="System.Text.Json.Nodes.JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public AggregateSummariesRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject)
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
        /// Gets or sets the optional dynamic hierarchical filters applied to every measure.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }

        /// <summary>
        /// Gets or sets the single-value aggregate measures to compute, each pairing a target column with its function.
        /// <para>The answers arrive in this order, one value per measure; a SQL <c>NULL</c> aggregate (for example <c>Avg</c> over zero rows) is preserved as JSON <c>null</c>. Multi-value measures and histograms are not accepted here. An empty list, or a list above the cap, is refused with 400 (DiGi-Organization/DiGi.GIS.WebAPI#56).</para>
        /// </summary>
        /// <example>[{ "ColumnUniqueId": "floor_area", "SinglevalueAggregateFunction": "Avg" }]</example>
        [Required]
        public List<AggregateMeasure> Measures { get; set; } = [];
    }
}
