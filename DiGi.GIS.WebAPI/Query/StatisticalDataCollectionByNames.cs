using DiGi.GIS.Classes;
using DiGi.GIS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Narrows a statistical data collection to the requested variables, keeping its identifier and unit code.
        /// <para>A requested value selects a variable when it equals, ordinally, the variable's stored name (<c>population [person/km2]</c>) or its BDL variable identifier (<see cref="IStatisticalData.Reference"/>, <c>60559</c>): callers working from BDL metadata hold the identifier, not the name. Variables keep the collection's order and appear once, whichever and however many values selected them.</para>
        /// </summary>
        /// <param name="statisticalDataCollection">The collection to narrow.</param>
        /// <param name="names">The requested names or BDL variable identifiers; blank values are ignored.</param>
        /// <returns>The original collection when no non-blank value is requested, a new collection holding the selected variables (possibly none) otherwise, or null when the collection is null.</returns>
        public static StatisticalDataCollection? StatisticalDataCollectionByNames(this StatisticalDataCollection? statisticalDataCollection, IEnumerable<string>? names)
        {
            if (statisticalDataCollection is null)
            {
                return null;
            }

            HashSet<string> names_Set = names is null ? [] : new(names.Where(x => !string.IsNullOrWhiteSpace(x)), StringComparer.Ordinal);
            if (names_Set.Count == 0)
            {
                return statisticalDataCollection;
            }

            List<IStatisticalData> statisticalDatas = [];
            foreach (IStatisticalData statisticalData in statisticalDataCollection.StatisticalDatas ?? [])
            {
                if ((statisticalData.Name is string name && names_Set.Contains(name)) || (statisticalData.Reference is string reference && names_Set.Contains(reference)))
                {
                    statisticalDatas.Add(statisticalData);
                }
            }

            return new StatisticalDataCollection(statisticalDataCollection.Guid, GIS.Create.UnitCode(statisticalDataCollection.Code), statisticalDatas);
        }
    }
}
