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
        /// The largest number of building references a Subdivision's polygon may resolve to when a building data read is confined to that Subdivision.
        /// <para>The references travel into one <c>reference = ANY(@array)</c> parameter per county partition and are built server-side, so this is not the 10 000 cap on client-supplied references; Bemowo (55413) resolves to 6 435 (measured 2026-11-16). The ceiling of 200 000 bounds the statement against a pathological polygon (DiGi-Organization/DiGi.GIS.WebAPI#83).</para>
        /// </summary>
        public static readonly int MaximumAdministrativeAreal2DReferenceCount = 200000;

        /// <summary>
        /// The largest number of single-value aggregate measures one <c>aggregatesummary/batch</c> request may carry.
        /// <para>The measures share one filter, one connection and one SQL command, so the cost follows the measure count. The ceiling of 100 is a guardrail against pathological requests - far above any realistic client - and stays at the converter's own cap so the 400 pre-check and the converter's refusal agree (DiGi-Organization/DiGi.GIS.WebAPI#56).</para>
        /// </summary>
        public static readonly int MaximumAggregateSummariesMeasureCount = 100;
    }
}
