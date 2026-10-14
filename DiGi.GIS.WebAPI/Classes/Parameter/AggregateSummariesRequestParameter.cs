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
