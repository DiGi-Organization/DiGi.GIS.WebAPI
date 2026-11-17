namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to building model queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class BuildingModel
    {
        /// <summary>
        /// The default number of building models one page of <c>gis/buildingmodel/itemsbycountyid</c> holds - the batch size of the server-side solar solve.
        /// </summary>
        public const int DefaultItemsByCountyIdLimit = 100;

        /// <summary>
        /// The largest number of building models one page of <c>gis/buildingmodel/itemsbycountyid</c> may hold.
        /// <para>Every model travels with its full geometry, so a page is bounded to keep one response, and the server's copies of it, within a few tens of megabytes. A client walking a county part asks for more pages, not larger ones.</para>
        /// </summary>
        public const int MaximumItemsByCountyIdLimit = 1000;
    }
}
