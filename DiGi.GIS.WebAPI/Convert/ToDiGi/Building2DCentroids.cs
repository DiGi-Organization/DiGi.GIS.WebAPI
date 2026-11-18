using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI
{
    public static partial class Convert
    {
        /// <summary>
        /// Converts the compact, columnar answer of <c>gis/Building2D/centroidsbycountyid</c> and <c>centroidsbyadministrativeareal2Did</c> - <c>{"References":[…],"CountyIds":[…],"X":[…],"Y":[…]}</c>, the i-th entry of every array describing the same building - back into building centroids; the inverse of <see cref="ToSystem_JsonObject(IEnumerable{Building2DCentroid}?)"/>.
        /// <para>The arrays must be present and of one length, and every entry must hold its type - a reference string, an integer county id and finite coordinates. Anything else is a broken answer and gives null rather than a shorter list, so a caller never reads a damaged answer as fewer buildings. The order is kept.</para>
        /// </summary>
        /// <param name="jsonObject">The columnar object. This value can be null.</param>
        /// <returns>The centroids (empty for four empty arrays), or <see langword="null"/> when <paramref name="jsonObject"/> is null or not a well-formed columnar answer.</returns>
        public static List<Building2DCentroid>? ToDiGi_Building2DCentroids(this JsonObject? jsonObject)
        {
            if (jsonObject is null)
            {
                return null;
            }

            if (jsonObject["References"] is not JsonArray references || jsonObject["CountyIds"] is not JsonArray countyIds || jsonObject["X"] is not JsonArray xs || jsonObject["Y"] is not JsonArray ys)
            {
                return null;
            }

            int count = references.Count;
            if (countyIds.Count != count || xs.Count != count || ys.Count != count)
            {
                return null;
            }

            List<Building2DCentroid> result = new(count);
            for (int i = 0; i < count; i++)
            {
                if (references[i] is not JsonValue jsonValue_Reference || !jsonValue_Reference.TryGetValue(out string? reference) || string.IsNullOrWhiteSpace(reference))
                {
                    return null;
                }

                if (countyIds[i] is not JsonValue jsonValue_CountyId || !jsonValue_CountyId.TryGetValue(out int countyId))
                {
                    return null;
                }

                if (xs[i] is not JsonValue jsonValue_X || !jsonValue_X.TryGetValue(out double x) || !double.IsFinite(x))
                {
                    return null;
                }

                if (ys[i] is not JsonValue jsonValue_Y || !jsonValue_Y.TryGetValue(out double y) || !double.IsFinite(y))
                {
                    return null;
                }

                result.Add(new Building2DCentroid() { Reference = reference, CountyId = countyId, X = x, Y = y });
            }

            return result;
        }
    }
}
