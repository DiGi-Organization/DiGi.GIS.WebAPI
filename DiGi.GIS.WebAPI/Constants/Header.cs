namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the names of the HTTP headers the GIS Web API sets on its responses.
    /// </summary>
    public static class Header
    {
        /// <summary>
        /// The response header carrying the cursor of the next page of a physical-order building data read. Absent when the county part is exhausted, and absent when the endpoint answered in reference order instead.
        /// </summary>
        public const string NextCursor = "DiGi-Next-Cursor";

        /// <summary>
        /// The response header of <c>gis/epwfile/item</c> carrying the id of the <c>epw_file</c> row whose file the body holds, as an invariant integer. A client stores it as the weather source id of a solar radiation summary; the body is unchanged.
        /// </summary>
        public const string EPWFileId = "DiGi-EPWFile-Id";
    }
}
