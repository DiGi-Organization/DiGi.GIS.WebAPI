using System;

namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the defaults of the full-interpretation test areas served by the GIS PostgreSQL Web API (<c>gis/ortoarea</c>).
    /// <para>Every value here is protocol-tunable: the wire contract names them as defaults rather than fixed limits, so a host may retune them by changing this file - the routes, parameter names and status contract stay the same.</para>
    /// </summary>
    public static class OrtoArea
    {
        /// <summary>
        /// The largest side of a test area bounding box, in metres, applied to every <c>gis/ortoarea</c> route.
        /// <para>Two things are sized by it. The WMS tile fetched per area and epoch is scaled against this side so the tile stays inside the orthophoto services' scale limit (<see cref="DiGi.GIS.Constants.WebService.OrtoMaxScaleDenominator"/>) whatever shape the box has; and the <c>orto_sheet</c> reads behind the epoch list and the label observation dates fan out over the box, so an unbounded box would turn one request into a whole-index scan. A box wider than this answers 400.</para>
        /// </summary>
        public const double MaximumAreaSide = 2000;

        /// <summary>
        /// The edge length of one sampling-grid cell, in metres.
        /// <para>The grid is aligned to multiples of this value over the area box (the floor/ceiling index idiom of <see cref="DiGi.GIS.Create.OrtoAvailabilityMatrix"/>), so the same box always yields the same grid.</para>
        /// </summary>
        public const double CellSize = 100;

        /// <summary>
        /// The fraction of the grid cells drawn as the sampled cells of a new area.
        /// <para>The effective count is <c>clamp(round(total cells &#215; SamplingFraction), CellCountMinimum, CellCountMaximum)</c> and never exceeds the total - a tiny grid samples every cell it has.</para>
        /// </summary>
        public const double SamplingFraction = 0.2;

        /// <summary>
        /// The smallest number of sampled cells a new area draws, when the grid holds that many.
        /// </summary>
        public const int CellCountMinimum = 5;

        /// <summary>
        /// The largest number of sampled cells a new area draws.
        /// </summary>
        public const int CellCountMaximum = 50;

        /// <summary>
        /// The smallest tile width, in pixels, an area image is fetched at.
        /// <para>The services answer requests below a certain size poorly, and the floor keeps the scale of a very small box predictable; the scale is then verified against <see cref="DiGi.GIS.Constants.WebService.OrtoMaxScaleDenominator"/> before the fetch.</para>
        /// </summary>
        public const int MinimumImagePixelSize = 64;

        /// <summary>
        /// The tile width and height cap, in pixels, of an area image.
        /// <para>The width is <c>clamp(ceil(box.Width &#215; MaximumImagePixelSize &#247; MaximumAreaSide), MinimumImagePixelSize, MaximumImagePixelSize)</c>, which fixes the ground scale of the fetch near one metre per pixel whatever the box, and the height follows the box aspect ratio capped at this value.</para>
        /// </summary>
        public const int MaximumImagePixelSize = 2048;

        /// <summary>
        /// The number of times a failed WMS fetch is retried before the request answers 503.
        /// <para>A fetch that still fails is a transport failure (image status 2): it is never stored, so the next request tries again.</para>
        /// </summary>
        public const int ImageFetchRetryCount = 2;

        /// <summary>
        /// The delay before the first WMS fetch retry, doubling after each attempt.
        /// </summary>
        public static readonly TimeSpan ImageFetchRetryDelay = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The number of times the WMS is asked again, after <see cref="ImageBlankRefetchDelay"/>, when it answers a blank (uniform) tile (DiGi.GIS.WebAPI#76).
        /// <para>Blanks are flaky and independent: on 2026-11-05, 6 of 20 identical requests for a covered square in central Warsaw (<c>TIME=2024</c>) came back blank, in no run longer than two. Three refetches leave about 0.3^4 ≈ 1 % of first loads blank; one that stays blank is not stored and answers 503, so the next request tries again.</para>
        /// </summary>
        public const int ImageBlankRefetchCount = 3;

        /// <summary>
        /// The delay before each blank refetch (<see cref="ImageBlankRefetchCount"/>). Short, because a blank is an answer, not a transport failure that needs back-off.
        /// </summary>
        public static readonly TimeSpan ImageBlankRefetchDelay = TimeSpan.FromSeconds(1);
    }
}
