namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the constants of the stored solar radiation endpoints.
    /// </summary>
    public static class SolarRadiation
    {
        /// <summary>
        /// The number of hours of the solar reference year, and so the length of every surface's shading factors a stored result must carry: 365 days of 24 hours, 29 February excluded.
        /// </summary>
        public const int HourCount = 8760;

        /// <summary>
        /// The largest number of results one request to <c>gis/buildingsolarradiation/setsolarradiations</c> may carry.
        /// <para>Every result of a county part goes into one upsert of 13 parameters per result, and Npgsql allows 65 535 parameters per statement, so a part could take 5 041; the cap stays well under that. A client splits a larger batch by memory first (<c>SerializableObjectsPostOptions.BatchMemorySize</c>), which at 8 760 bytes of factors per surface keeps a request far below it in practice.</para>
        /// </summary>
        public const int MaximumBatchCount = 1000;
    }
}
