using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Groups the identifiers of subdivision references by the county partition they sit in.
        /// <para>The groups come out in the order a county was first met, so a caller that caps or iterates them does so in a stable, documented order. References without a county identifier, and identifiers already seen, are skipped.</para>
        /// </summary>
        /// <param name="administrativeAreal2DReferences">The subdivision references to group.</param>
        /// <returns>The subdivision identifiers keyed by county identifier; empty when there is nothing to group.</returns>
        public static Dictionary<int, List<int>> SubdivisionIdsByCountyId(this IEnumerable<AdministrativeAreal2DReference>? administrativeAreal2DReferences)
        {
            Dictionary<int, List<int>> result = [];
            if (administrativeAreal2DReferences is null)
            {
                return result;
            }

            HashSet<int> ids = [];
            foreach (AdministrativeAreal2DReference administrativeAreal2DReference in administrativeAreal2DReferences)
            {
                if (administrativeAreal2DReference?.CountyId is not int countyId)
                {
                    continue;
                }

                if (!ids.Add(administrativeAreal2DReference.Id))
                {
                    continue;
                }

                if (!result.TryGetValue(countyId, out List<int>? subdivisionIds))
                {
                    subdivisionIds = [];
                    result[countyId] = subdivisionIds;
                }

                subdivisionIds.Add(administrativeAreal2DReference.Id);
            }

            return result;
        }
    }
}
