namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to building 2D queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class Building2D
    {
        /// <summary>
        /// The largest number of county parts a single estimated count request may name.
        /// <para>An estimate is read from the catalogue rather than the table, so the whole country - 406 county parts - costs one query and is well inside the cap. The cap only stops a request from naming an unbounded list.</para>
        /// </summary>
        public static readonly int MaximumEstimatedCountCountyCount = 1000;
    }
}
