using DiGi.Geometry.Planar.Classes;

namespace DiGi.GIS.WebAPI
{
    public static partial class Create
    {
        /// <summary>
        /// Validates four bounding-box corners and builds their box, applied to every <c>gis/ortoarea</c> route and to the open building match summary.
        /// <para>Refuses non-finite values, a box whose minimum corner is not below its maximum, and any side longer than <see cref="Constants.OrtoArea.MaximumAreaSide"/>; each refusal is answered 400 by the caller. A box degenerate to a point or a line is accepted - it simply holds no grid cell and the WMS answers it.</para>
        /// </summary>
        /// <param name="x_1">The easting of the first corner, or <c>null</c> when omitted.</param>
        /// <param name="y_1">The northing of the first corner, or <c>null</c> when omitted.</param>
        /// <param name="x_2">The easting of the second corner, or <c>null</c> when omitted.</param>
        /// <param name="y_2">The northing of the second corner, or <c>null</c> when omitted.</param>
        /// <returns>The bounding box, or null when a corner is missing or non-finite, the corners are not ordered, or a side exceeds the maximum.</returns>
        public static global::DiGi.Geometry.Planar.Classes.BoundingBox2D? BoundingBox2D(double? x_1, double? y_1, double? x_2, double? y_2)
        {
            if (x_1 is not double x_1_Value || y_1 is not double y_1_Value || x_2 is not double x_2_Value || y_2 is not double y_2_Value)
            {
                return null;
            }

            if (!double.IsFinite(x_1_Value) || !double.IsFinite(y_1_Value) || !double.IsFinite(x_2_Value) || !double.IsFinite(y_2_Value))
            {
                return null;
            }

            if (x_1_Value > x_2_Value || y_1_Value > y_2_Value)
            {
                return null;
            }

            if (x_2_Value - x_1_Value > Constants.OrtoArea.MaximumAreaSide || y_2_Value - y_1_Value > Constants.OrtoArea.MaximumAreaSide)
            {
                return null;
            }

            return new global::DiGi.Geometry.Planar.Classes.BoundingBox2D(new Point2D(x_1_Value, y_1_Value), new Point2D(x_2_Value, y_2_Value));
        }
    }
}
