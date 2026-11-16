using System.Collections.Generic;

namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to building data queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class BuildingData
    {
        /// <summary>
        /// The smallest accepted value of <c>limit</c> on an address search or on <c>byaddress</c>.
        /// <para>A <see langword="null"/> limit is refused by both endpoints; a set limit outside the bounds of <see cref="ByAddressLimit_Maximum"/> is refused with 400, never clamped. The two endpoints share this pair so a caller cannot pick one county answer set on <c>buildingsbyaddress</c> and a different one on <c>byaddress</c> (DiGi-Organization/DiGi.GIS.WebAPI#85).</para>
        /// </summary>
        public const int ByAddressLimit_Minimum = 1;

        /// <summary>
        /// The largest number of address matches <c>byaddress</c> answers in one request.
        /// <para>The ceiling of 100 matches the one <c>addresspoint/buildingsbyaddress</c> accepts, because byaddress ranks and caps its matches the same way; each match costs one row of building data read (DiGi-Organization/DiGi.GIS.WebAPI#85).</para>
        /// </summary>
        public const int ByAddressLimit_Maximum = 100;

        /// <summary>
        /// The projection <c>byaddress</c> reads with when the caller sends no <c>columns</c>.
        /// <para>Chosen for one question - what the building is, how big it is, when it was built, where it stands - and small enough that a handful of matches stays a few kilobytes instead of the 93 809 bytes an unprojected <c>tablebyreference</c> answered for one building (measured 2026-11-16; DiGi-Organization/DiGi.GIS.WebAPI#85). Every id was checked against the 246-column catalogue on that date. A default column the stored table no longer holds is dropped with a warning when read, never refused.</para>
        /// </summary>
        public static readonly List<string> ByAddressDefaultColumnUniqueIds =
        [
            "storeys",
            "building_general_function",
            "building_specific_functions",
            "floor_area",
            "total_area",
            "calculated_year_built",
            "subdivision_name"
        ];

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

        /// <summary>
        /// The smallest accepted value of <c>BuildingDataByFilterGroupParameter.Limit</c> on a filter-group table read.
        /// <para>A <see langword="null"/> limit reads every matching row; a set limit outside these bounds is refused with 400 before any connection is opened - refused, never clamped (DiGi-Organization/DiGi.GIS.WebAPI#84).</para>
        /// </summary>
        public static readonly int MinimumLimit = 1;

        /// <summary>
        /// The largest accepted value of <c>BuildingDataByFilterGroupParameter.Limit</c> on a filter-group table read.
        /// <para>The ceiling bounds the rows one request may return and the sort work behind them; it is far above any realistic top-N question (DiGi-Organization/DiGi.GIS.WebAPI#84).</para>
        /// </summary>
        public static readonly int MaximumLimit = 1000;
    }
}
