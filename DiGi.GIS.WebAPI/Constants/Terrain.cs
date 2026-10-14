namespace DiGi.GIS.WebAPI.Constants
{
    /// <summary>
    /// Provides the limits applied to terrain queries served by the GIS PostgreSQL Web API.
    /// </summary>
    public static class Terrain
    {
        /// <summary>
        /// The largest search radius, in model units, a terrain request may ask for.
        /// <para>The terrain endpoints are unauthenticated reads with no natural ceiling of their own: the radius alone decides how many stored points are gathered, triangulated and serialised, and the store is partitioned by county rather than capped by extent. Without this a single request can ask for the whole country.</para>
        /// <para>The cap is a half-extent, so the largest search area is 4 km by 4 km. Counties are sampled onto a lattice between 10 m and 100 m, which puts the worst case at roughly 160 000 points. Raising this is safe only if the finest lattice is never queried at the new size - at 5 000 m the same lattice yields about a million points.</para>
        /// <para><c>const</c> rather than <c>static readonly</c> so the terrain mesh endpoints can carry it as a <see cref="System.ComponentModel.DataAnnotations.RangeAttribute"/> maximum, which has to be a compile-time constant.</para>
        /// </summary>
        public const double MaximumRadius = 2000;

        /// <summary>
        /// How many times its own vertex spacing a terrain mesh triangle's longest edge may reach before it is treated as bridging no data.
        /// <para>A Delaunay triangulation covers the convex hull of its sites, so without a limit the mesh bridges county edges, no-data gaps and concave outlines with a skirt of long thin triangles that look like terrain and are not.</para>
        /// <para>Expressed as a multiple of local spacing rather than as a distance, because the stored lattice is not the only sampling this has to survive: points added where the ground moves sharply make one area denser than another, and any fixed distance is then simultaneously too tight for the coarse part and too loose for the fine part. A factor measures each triangle against its own vertices and needs no retuning when the sampling changes.</para>
        /// <para>The consequence to know when reading a served mesh: an empty area entirely enclosed by terrain - a lake, or a pocket the elevation service has no data for - is now spanned flat rather than left open, because nothing over it reaches the boundary to be removed. That is acceptable for a surface that is rendered; it is not a licence to treat the mesh as measured ground everywhere it has vertices.</para>
        /// <para>On the present 100 m lattice this behaves like a limit of about 250 m. It only governs the outer edge: <see cref="DiGi.Geometry.PointCloud.Core.Query.ErodedIndexes"/> removes triangles from the boundary inwards, so a point missing from the interior keeps the triangles around it and reads as a flat spot rather than a hole.</para>
        /// </summary>
        public static readonly double EdgeLengthFactor = 2.5;

        /// <summary>
        /// The finest lattice, in model units, a coverage or gap request may be measured against.
        /// <para>The work of those endpoints rises with the square of how fine the lattice is: they generate every node of a county and decide each one against its outlines. A county of 1 000 square kilometres is 100 000 nodes at 100 m and 10 million at 10 m, and below that the request stops being a diagnostic and becomes a denial of service that anyone can send.</para>
        /// <para><c>const</c> rather than <c>static readonly</c> so the lattice endpoints can carry it as the floor of a <see cref="DiGi.GIS.WebAPI.Classes.MinimumAttribute"/>, whose argument has to be a compile-time constant, the way <see cref="MaximumRadius"/> became one for <see cref="System.ComponentModel.DataAnnotations.RangeAttribute"/> (DiGi-Organization/DiGi.GIS.WebAPI#47).</para>
        /// </summary>
        public const double MinimumGridSize = 10;

        /// <summary>
        /// The largest number of lattice nodes a single coverage or gap request may generate.
        /// <para>Checked against the requested area before any node is built, so an area and a lattice that would together exceed it are refused rather than started. This is the cap that holds when a large county meets a fine lattice, both of which are individually allowed.</para>
        /// </summary>
        public static readonly long MaximumNodeCount = 5000000;

        /// <summary>
        /// The longest side, in model units, a gap request may ask for.
        /// <para>Larger than the mesh endpoints admit, because a gap request returns coordinates rather than a triangulated surface and is meant for looking over a region at once. It is still bounded by <see cref="MaximumNodeCount"/> at any given lattice.</para>
        /// </summary>
        public static readonly double MaximumGapExtent = 50000;

        /// <summary>
        /// The largest number of counties a single density request may name.
        /// <para>A density costs the reading of every subdivision outline of the county it is asked for, so this is what keeps one request from pulling the administrative geometry of the whole country. Sweeping every county means several requests, which is also what makes the sweep interruptible.</para>
        /// </summary>
        public static readonly int MaximumDensityCountyCount = 50;
    }
}
