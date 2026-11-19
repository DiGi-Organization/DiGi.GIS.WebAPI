using System;
using System.Collections.Generic;
using System.Globalization;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Gets every building data column that holds a bounded choice list, together with the complete set of values it can hold (DiGi-Organization/DiGi.GIS.WebAPI#102).
        /// <para>The answer is built from the code alone - the enumerations of <c>DiGi.GIS</c> the columns are written from - so no connection is opened, a value no stored row holds yet is still listed, and the answer costs the same on the largest partition as on the smallest. Each value is what the writer stores: the description text for the columns <c>Modify/Update_Building2D.cs</c> writes through <c>Description()</c>, the member name for <c>cardinal_direction</c>, which the enum-to-string conversion stores instead of the description, the underlying integer for <c>solar_weather_source_type</c>, and <c>false</c> then <c>true</c> for the two boolean columns.</para>
        /// <para>Values come out in enum declaration order and never repeat; the integer column comes out ascending, so its documented <c>-1, 0, 1</c> reads in that order instead of the unsigned order <see cref="Enum.GetValues{TEnum}"/> yields. <c>building_specific_functions</c> carries its functions one by one - one value per <c>BuildingSpecificFunction</c> member - never the comma-joined combination a stored row holds, because a caller filters on one function at a time.</para>
        /// </summary>
        /// <returns>The value sets keyed by column unique id (the normalized name of the matching <c>DiGi.GIS.IO.Constants.Column</c> member), one entry per limited-choice column; never null and never empty.</returns>
        public static Dictionary<string, List<object?>> ColumnValues()
        {
            Dictionary<string, List<object?>> result = new(StringComparer.Ordinal)
            {
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.BuildingGeneralFunction)!, Descriptions<DiGi.GIS.Enums.BuildingGeneralFunction>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.BuildingSpecificFunctions)!, Descriptions<DiGi.GIS.Enums.BuildingSpecificFunction>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.BuildingPhase)!, Descriptions<DiGi.GIS.Enums.BuildingPhase>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.CardinalDirection)!, Names<DiGi.GIS.Enums.CardinalDirection>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.SettlementType)!, Descriptions<DiGi.GIS.Enums.SettlementType>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.CalculatedBuildingShape)!, Descriptions<DiGi.GIS.Enums.BuildingShape>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.PredictedBuildingShape)!, Descriptions<DiGi.GIS.Enums.BuildingShape>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.UserBuildingShape)!, Descriptions<DiGi.GIS.Enums.BuildingShape>() },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.IsResidential)!, [false, true] },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.IsOccupied)!, [false, true] },
                { DiGi.PostgreSQL.Table.Query.UniqueId(DiGi.GIS.IO.Constants.Column.SolarWeatherSourceType)!, Integers<DiGi.GIS.Enums.WeatherSourceType>() },
            };

            return result;

            // The description is the text the writer stores, falling back to the member name when an enum carries no
            // Description attribute, which is the same fallback DiGi.Core.Query.Description takes on the write path.
            static List<object?> Descriptions<T>() where T : struct, Enum
            {
                List<object?> values = [];
                foreach (T value in Enum.GetValues<T>())
                {
                    values.Add(Core.Query.Description(value));
                }

                return values;
            }

            // The member name, not the description: cardinal_direction is stored by the enum-to-string conversion
            // (SetValue -> TryGetValidValue -> TryConvert_String -> ToString()), so "NorthEast" travels, not "North East".
            static List<object?> Names<T>() where T : struct, Enum
            {
                List<object?> values = [];
                foreach (T value in Enum.GetValues<T>())
                {
                    values.Add(Enum.GetName(value));
                }

                return values;
            }

            // The underlying integer of each member: solar_weather_source_type is an int column whose domain the
            // enumeration declares as -1 undefined, 0 EPW, 1 live. Sorted ascending, because Enum.GetValues orders by
            // the unsigned bit pattern and would put the negative Undefined member after 0 and 1.
            static List<object?> Integers<T>() where T : struct, Enum
            {
                List<int> integers = [];
                foreach (T value in Enum.GetValues<T>())
                {
                    integers.Add(System.Convert.ToInt32(value, CultureInfo.InvariantCulture));
                }

                integers.Sort();

                List<object?> values = [];
                foreach (int integer in integers)
                {
                    values.Add(integer);
                }

                return values;
            }
        }
    }
}
