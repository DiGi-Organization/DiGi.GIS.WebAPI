using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.ComponentModel.DataAnnotations;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for multi-value database aggregation queries.
    /// </summary>
    public class MultivalueAggregateRequestParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultivalueAggregateRequestParameter"/> class.
        /// </summary>
        public MultivalueAggregateRequestParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivalueAggregateRequestParameter"/> class by copying another instance.
        /// <para>The filter group is shared, not cloned: <see cref="DiGi.PostgreSQL.Table.Classes.FilterGroup"/> is not a serializable object.</para>
        /// </summary>
        /// <param name="multivalueAggregateRequestParameter">The instance to copy; null leaves every member at its default.</param>
        public MultivalueAggregateRequestParameter(MultivalueAggregateRequestParameter multivalueAggregateRequestParameter)
            : base(multivalueAggregateRequestParameter)
        {
            if (multivalueAggregateRequestParameter is not null)
            {
                CountyId = multivalueAggregateRequestParameter.CountyId;
                AdministrativeAreal2DId = multivalueAggregateRequestParameter.AdministrativeAreal2DId;
                ColumnUniqueId = multivalueAggregateRequestParameter.ColumnUniqueId;
                MultivalueAggregateFunction = multivalueAggregateRequestParameter.MultivalueAggregateFunction;
                Separator = multivalueAggregateRequestParameter.Separator;
                MaxGroups = multivalueAggregateRequestParameter.MaxGroups;
                FilterGroup = multivalueAggregateRequestParameter.FilterGroup;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivalueAggregateRequestParameter"/> class using a <see cref="System.Text.Json.Nodes.JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public MultivalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject)
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
        /// Gets or sets the multi-value aggregation function to perform.
        /// </summary>
        /// <example>SplitDistinctCount</example>
        [Required]
        public MultivalueAggregateFunction MultivalueAggregateFunction { get; set; }

        /// <summary>
        /// Gets or sets the optional string separator; defaults to null triggering dynamic auto-detection.
        /// </summary>
        /// <example>","</example>
        public string? Separator { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of distinct-value groups a <see cref="MultivalueAggregateFunction.SplitValueDistribution"/> may return. Defaults to 1000.
        /// <para>For <see cref="MultivalueAggregateFunction.SplitValueDistribution"/> only: the result carries at most this many groups plus one trailing truncation marker, so a result longer than this means the distribution was cut and the caller drops the last element. Ignored by <see cref="MultivalueAggregateFunction.SplitDistinctCount"/>.</para>
        /// <para>Capped because the group count is a <c>GROUP BY</c> divisor rather than a page size: the server builds and returns one row per distinct value whatever the table holds, so an unbounded value turns a cheap aggregate into an arbitrarily large response.</para>
        /// </summary>
        /// <example>20</example>
        [Range(1, 1000)]
        public int MaxGroups { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to aggregation.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}