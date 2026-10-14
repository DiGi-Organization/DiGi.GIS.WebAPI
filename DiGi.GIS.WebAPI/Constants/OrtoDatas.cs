namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to orthophoto queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class OrtoDatas
    {
        /// <summary>
        /// The largest number of counties a summary or queue request may name at once.
        /// <para>Each county costs a grouped aggregate over one partition, which is cheap, but the request is unauthenticated and naming every county in the country in one call is not what these are for. Omitting the identifiers entirely is still allowed and answers every partition in a single grouped statement, which is one query rather than many.</para>
        /// </summary>
        public static readonly int MaximumSummaryCountyCount = 100;

        /// <summary>
        /// The largest number of references a subdivision comparison may name back per category.
        /// <para>The comparison itself walks a whole county, and the counts it returns are exact whatever this is set to. The samples exist to make a disagreement actionable without returning a hundred thousand strings, so the ceiling bounds the response rather than the work.</para>
        /// <para><c>const</c> rather than <c>static readonly</c> so <c>subdivisionlinksbycountyid</c> can carry it as the maximum of a <see cref="System.ComponentModel.DataAnnotations.RangeAttribute"/>, whose argument has to be a compile-time constant (DiGi-Organization/DiGi.GIS.WebAPI#47).</para>
        /// </summary>
        public const int MaximumSampleCount = 1000;

        /// <summary>
        /// The largest number of distinct counties one coverage request may have measured exactly.
        /// <para>A subdivision or a municipality has no partition of its own, so its coverage is counted rather than estimated - one read per side of the county it sits in, measured at 0.10 s for a county of 33 000 buildings and 0.63 s for the largest in the country. Every subdivision and municipality of one county is served from that single pass, so the cost follows the counties named and not the identifiers.</para>
        /// <para>The ceiling of 500 allows full-country listings covering all 380 Polish counties at once, while protecting the database connection pool against unbounded input.</para>
        /// </summary>
        public static readonly int MaximumCoverageCountyCount = 500;
    }
}
