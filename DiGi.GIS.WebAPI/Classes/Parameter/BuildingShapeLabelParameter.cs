using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes.Parameter
{
    /// <summary>
    /// The body of the <c>gis/buildingshape/label</c> write: one signed-in annotator's building shape label of one sample item (DiGi.GIS.WebAPI#80).
    /// <para>Every member is bound nullable so an omitted field is representable and can be rejected explicitly. <see cref="Shape"/> is the integer value of <see cref="GIS.Enums.BuildingShape"/>, where <c>0</c> (<see cref="GIS.Enums.BuildingShape.Undefined"/>) records that the footprint could not be classified. The interpreter and the timestamp are never taken from this body: they are stamped server-side from the bearer token and the clock.</para>
    /// </summary>
    public class BuildingShapeLabelParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingShapeLabelParameter"/> class.
        /// </summary>
        public BuildingShapeLabelParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingShapeLabelParameter"/> class using the provided JSON object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="JsonObject"/> containing the data used to initialize the parameter.</param>
        public BuildingShapeLabelParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingShapeLabelParameter"/> class by copying the values from an existing instance.
        /// </summary>
        /// <param name="buildingShapeLabelParameter">The source instance from which to copy the values.</param>
        public BuildingShapeLabelParameter(BuildingShapeLabelParameter buildingShapeLabelParameter)
            : base(buildingShapeLabelParameter)
        {
            if (buildingShapeLabelParameter is not null)
            {
                SampleKey = buildingShapeLabelParameter.SampleKey;
                CountyId = buildingShapeLabelParameter.CountyId;
                Reference = buildingShapeLabelParameter.Reference;
                Shape = buildingShapeLabelParameter.Shape;
                Note = buildingShapeLabelParameter.Note;
            }
        }

        /// <summary>
        /// Gets or sets the identity of the sample the labelled item belongs to; a blank value is rejected.
        /// </summary>
        public string? SampleKey { get; set; }

        /// <summary>
        /// Gets or sets the county id (not code) the item is filed under; <c>null</c> (an omitted field) is rejected.
        /// </summary>
        public int? CountyId { get; set; }

        /// <summary>
        /// Gets or sets the building reference of the item; a blank value is rejected.
        /// </summary>
        public string? Reference { get; set; }

        /// <summary>
        /// Gets or sets the label as the integer value of <see cref="GIS.Enums.BuildingShape"/>; <c>null</c> and values outside the enumeration are rejected.
        /// </summary>
        public short? Shape { get; set; }

        /// <summary>
        /// Gets or sets an optional free-text note.
        /// </summary>
        public string? Note { get; set; }
    }
}
