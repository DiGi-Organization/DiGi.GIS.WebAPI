using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes.Parameter
{
    /// <summary>
    /// The body of the <c>gis/ortoarea/label</c> write: one interpreter's reading of one epoch of one object in a full-interpretation test area.
    /// <para>Every member is bound nullable so an omitted field is representable and can be rejected explicitly - the object identity is exactly one of <see cref="Reference"/> (a footprint) or <see cref="ObjectId"/> (a drawn object), with <see cref="CountyId"/> only an optional narrowing hint, and <see cref="State"/> is checked against 0/1/2 rather than compared to a sentinel. The interpreter identity and the observation dates are never taken from this body: they are resolved server-side from the bearer token, the stored object geometry and the orthophoto sheet index.</para>
    /// </summary>
    public class OrtoAreaLabelParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaLabelParameter"/> class.
        /// </summary>
        public OrtoAreaLabelParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaLabelParameter"/> class using the provided JSON object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="JsonObject"/> containing the data used to initialize the parameter.</param>
        public OrtoAreaLabelParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrtoAreaLabelParameter"/> class by copying the values from an existing instance.
        /// </summary>
        /// <param name="ortoAreaLabelParameter">The source instance from which to copy the values.</param>
        public OrtoAreaLabelParameter(OrtoAreaLabelParameter ortoAreaLabelParameter)
            : base(ortoAreaLabelParameter)
        {
            if (ortoAreaLabelParameter is not null)
            {
                X_1 = ortoAreaLabelParameter.X_1;
                Y_1 = ortoAreaLabelParameter.Y_1;
                X_2 = ortoAreaLabelParameter.X_2;
                Y_2 = ortoAreaLabelParameter.Y_2;
                CountyId = ortoAreaLabelParameter.CountyId;
                Reference = ortoAreaLabelParameter.Reference;
                ObjectId = ortoAreaLabelParameter.ObjectId;
                EpochYear = ortoAreaLabelParameter.EpochYear;
                State = ortoAreaLabelParameter.State;
                Note = ortoAreaLabelParameter.Note;
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
        /// Gets or sets the county part of the footprint identity as an optional narrowing hint; resolved server-side from <see cref="Reference"/> when omitted, and the resolved part is what files the label row.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the building reference of the footprint identity; exactly one of this or <see cref="ObjectId"/> must be given, so the reference alone identifies the footprint.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the drawn object the label was filed under; exactly one of this or <see cref="Reference"/> must be given. The stored polygon of that object resolves the observation dates - the body carries none.
        /// </summary>
        public string? ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the imagery epoch the label interprets; <c>null</c> (an omitted value) is rejected.
        /// </summary>
        public short? EpochYear { get; set; }

        /// <summary>
        /// Gets or sets what the interpreter saw, as 0 (present), 1 (absent) or 2 (indeterminate); <c>null</c> or any other value is rejected.
        /// </summary>
        public short? State { get; set; }

        /// <summary>
        /// Gets or sets the free-text note of the interpreter, or <c>null</c> when unnoted.
        /// </summary>
        public string? Note { get; set; }
    }
}
