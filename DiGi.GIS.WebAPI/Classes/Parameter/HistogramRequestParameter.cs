using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for generating histograms.
    /// </summary>
    public class HistogramRequestParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramRequestParameter"/> class.
        /// </summary>
        public HistogramRequestParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HistogramRequestParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public HistogramRequestParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the target partition identifier (County ID). If null, histogram is generated across all partitions.
        /// </summary>
        /// <example>10365</example>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the column unique identifier to calculate value distributions for.
        /// </summary>
        /// <example>"column_unique_id_123"</example>
        [Required]
        public string ColumnUniqueId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total number of histogram buckets. Defaults to 10.
        /// <para>Capped because the bucket count is width_bucket's divisor rather than a page size: the server builds and returns one row per bucket whatever the table holds, so an unbounded value turns a cheap aggregate into an arbitrarily large response.</para>
        /// </summary>
        /// <example>20</example>
        [Range(1, 1000)]
        public int BucketCount { get; set; } = 10;

        /// <summary>
        /// Gets or sets the bucketing rule. Defaults to <see cref="HistogramBucketing.EqualWidth"/> (<c>width_bucket</c> over the scope's [min, max], the maximum in the overflow bucket <see cref="BucketCount"/> + 1); <see cref="HistogramBucketing.EqualCount"/> (<c>ntile</c>) gives every bucket the same number of rows, so a skewed column keeps its resolution where its rows are - what a consumer placing quantiles needs (DiGi-Organization/DiGi.PostgreSQL#7).
        /// <para>Sent as the integer value; an omitted property keeps equal width, so clients written against the earlier contract are unaffected.</para>
        /// </summary>
        /// <example>1</example>
        public HistogramBucketing HistogramBucketing { get; set; } = HistogramBucketing.EqualWidth;

        /// <summary>
        /// Gets or sets the optional dynamic hierarchical filters to apply prior to generating the histogram.
        /// </summary>
        public FilterGroup? FilterGroup { get; set; }
    }
}