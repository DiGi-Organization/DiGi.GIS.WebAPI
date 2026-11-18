using DiGi.GIS.PostgreSQL.Classes;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI
{
    public static partial class Convert
    {
        /// <summary>
        /// Converts building centroids to the compact, columnar answer of <c>gis/Building2D/centroidsbyadministrativeareal2Did</c>: <c>{"References":[…],"CountyIds":[…],"X":[…],"Y":[…]}</c>, the i-th entry of every array describing the same building.
        /// <para>The full <see cref="Building2DCentroid"/> answer of <c>point2dsbyadministrativeareal2Did</c> repeats a <c>_type</c> discriminator and four property names per building. For county 1465 (155 307 buildings) that relays as 15.9 MB and takes 2.1-2.6 s, and the client rebuilds every row through <c>Core.Convert.ToDiGi</c> only to keep four values (DiGi.GIS.WebAPI.UI#29). This shape carries the values alone.</para>
        /// <para>Coordinates are rounded to 0.01 m (away from zero on a midpoint), which also drops the binary noise of values such as <c>474823.57999999996</c>. A centroid without a reference (null, empty or whitespace - <c>building_2d.reference</c> is <c>NOT NULL</c> but may be empty) or a county identifier cannot be joined by the client and is skipped, as the full answer's consumers and the solar solve skip it, and as <see cref="ToDiGi_Building2DCentroids(JsonObject?)"/> requires - one blank reference would otherwise make the client refuse the whole answer; the arrays therefore always share one length. The input order is kept.</para>
        /// </summary>
        /// <param name="building2DCentroids">The centroids to convert. This value can be null.</param>
        /// <returns>The columnar object, with four empty arrays for an empty input, or <see langword="null"/> when <paramref name="building2DCentroids"/> is null.</returns>
        public static JsonObject? ToSystem_JsonObject(this IEnumerable<Building2DCentroid>? building2DCentroids)
        {
            if (building2DCentroids is null)
            {
                return null;
            }

            JsonArray references = [];
            JsonArray countyIds = [];
            JsonArray xs = [];
            JsonArray ys = [];

            foreach (Building2DCentroid? building2DCentroid in building2DCentroids)
            {
                if (building2DCentroid?.Reference is not string reference || string.IsNullOrWhiteSpace(reference) || building2DCentroid.CountyId is not int countyId)
                {
                    continue;
                }

                references.Add(reference);
                countyIds.Add(countyId);
                xs.Add(Math.Round(building2DCentroid.X, 2, MidpointRounding.AwayFromZero));
                ys.Add(Math.Round(building2DCentroid.Y, 2, MidpointRounding.AwayFromZero));
            }

            return new JsonObject()
            {
                ["References"] = references,
                ["CountyIds"] = countyIds,
                ["X"] = xs,
                ["Y"] = ys
            };
        }
    }
}
