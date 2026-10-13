namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to building data queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class BuildingData
    {
        /// <summary>
        /// The largest number of distinct counties one area count may iterate.
        /// <para>An area above the subdivision level is counted one county partition at a time, sequentially, so the cost follows the counties its subdivisions sit in. The ceiling of 500 allows a whole-country count across all 406 county parts, while protecting the database connection pool against unbounded input.</para>
        /// </summary>
        public static readonly int MaximumAdministrativeAreal2DCountyCount = 500;

        /// <summary>
        /// The largest number of single-value aggregate measures one <c>aggregatesummary/batch</c> request may carry.
        /// <para>The measures share one filter, one connection and one SQL command, so the cost follows the measure count. The ceiling of 100 is a guardrail against pathological requests - far above any realistic client - and stays at the converter's own cap so the 400 pre-check and the converter's refusal agree (ZiolkowskiJakub/DiGi.GIS.WebAPI#56).</para>
        /// </summary>
        public static readonly int MaximumAggregateSummariesMeasureCount = 100;
    }
}
