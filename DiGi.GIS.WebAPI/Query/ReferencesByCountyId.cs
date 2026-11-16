using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Groups the references of building references by the county partition they sit in.
        /// <para>A reference is unique only within its county partition, so the key is the county and the same reference may appear under two counties. The groups come out in the order a county was first met. References without a county identifier or without a reference, and a reference already seen in the same county, are skipped.</para>
        /// </summary>
        /// <param name="building2DReferences">The building references to group.</param>
        /// <returns>The references keyed by county identifier; empty when there is nothing to group.</returns>
        public static Dictionary<int, List<string>> ReferencesByCountyId(this IEnumerable<Building2DReference>? building2DReferences)
        {
            Dictionary<int, List<string>> result = [];
            if (building2DReferences is null)
            {
                return result;
            }

            Dictionary<int, HashSet<string>> references_ByCountyId = [];
            foreach (Building2DReference? building2DReference in building2DReferences)
            {
                if (building2DReference?.CountyId is not int countyId || string.IsNullOrWhiteSpace(building2DReference.Reference))
                {
                    continue;
                }

                string reference = building2DReference.Reference!;

                if (!references_ByCountyId.TryGetValue(countyId, out HashSet<string>? references_Seen))
                {
                    references_Seen = [];
                    references_ByCountyId[countyId] = references_Seen;
                    result[countyId] = [];
                }

                if (references_Seen.Add(reference))
                {
                    result[countyId].Add(reference);
                }
            }

            return result;
        }
    }
}
