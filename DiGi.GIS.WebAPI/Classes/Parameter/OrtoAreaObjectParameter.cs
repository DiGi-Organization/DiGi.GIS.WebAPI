using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes.Parameter
{
    /// <summary>
    /// The body of the <c>gis/ortoarea/objects</c> write: one polygon drawn in a full-interpretation test area.
    /// <para>Every member is bound nullable so an omitted field is representable and can be rejected explicitly. The bounding box names the area with the same component names the read routes take as query parameters, and <see cref="Polygon"/> carries the drawn outline as serialized JSON - its shape is resolved server-side (an array of points, or a point array under <c>point2Ds</c>/<c>points</c>, or a serialized DiGi <c>Polygon2D</c>) and must hold at least three vertices.</para>
    /// </summary>
    public class OrtoAreaObjectParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaObjectParameter"/> class.
        /// </summary>
        public OrtoAreaObjectParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaObjectParameter"/> class using the provided JSON object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="JsonObject"/> containing the data used to initialize the parameter.</param>
        public OrtoAreaObjectParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaObjectParameter"/> class by copying the values from an existing instance.
        /// </summary>
        /// <param name="ortoAreaObjectParameter">The source instance from which to copy the values.</param>
        public OrtoAreaObjectParameter(OrtoAreaObjectParameter ortoAreaObjectParameter)
            : base(ortoAreaObjectParameter)
        {
            if (ortoAreaObjectParameter is not null)
            {
                X_1 = ortoAreaObjectParameter.X_1;
                Y_1 = ortoAreaObjectParameter.Y_1;
                X_2 = ortoAreaObjectParameter.X_2;
                Y_2 = ortoAreaObjectParameter.Y_2;
                Polygon = ortoAreaObjectParameter.Polygon?.DeepClone();
            }
        }

        /// <summary>
        /// Gets or sets the easting of the first corner of the area bounding box, in EPSG:2180 metres; <c>null</c> (an omitted field) is rejected.
        /// </summary>
        public double? X_1 { get; set; }

        /// <summary>
        /// Gets or sets the northing of the first corner of the area bounding box, in EPSG:2180 metres; <c>null</c> (an omitted field) is rejected.
        /// </summary>
        public double? Y_1 { get; set; }

        /// <summary>
        /// Gets or sets the easting of the second corner of the area bounding box, in EPSG:2180 metres; <c>null</c> (an omitted field) is rejected.
        /// </summary>
        public double? X_2 { get; set; }

        /// <summary>
        /// Gets or sets the northing of the second corner of the area bounding box, in EPSG:2180 metres; <c>null</c> (an omitted field) is rejected.
        /// </summary>
        public double? Y_2 { get; set; }

        /// <summary>
        /// Gets or sets the drawn polygon as serialized JSON; missing or unreadable, or holding fewer than three vertices, is rejected.
        /// </summary>
        public JsonNode? Polygon { get; set; }
    }
}
