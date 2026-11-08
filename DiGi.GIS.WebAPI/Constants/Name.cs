namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides a collection of constant name identifiers used within the GIS PostgreSQL Web API.
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Provides the identifiers for the named HTTP clients created by the Web API.
        /// </summary>
        public static class Client
        {
            /// <summary>
            /// The GIS client identifier.
            /// </summary>
            public static readonly string GIS = "GIS";

            /// <summary>
            /// The Geoportal client identifier.
            /// </summary>
            public static readonly string Geoportal = "Geoportal";

            /// <summary>
            /// The GUGiK (Główny Urząd Geodezji i Kartografii) client identifier.
            /// </summary>
            public static readonly string GUGiK = "GUGiK";
        }
    }
}