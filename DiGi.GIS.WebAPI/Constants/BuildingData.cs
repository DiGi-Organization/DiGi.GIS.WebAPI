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
    }
}
