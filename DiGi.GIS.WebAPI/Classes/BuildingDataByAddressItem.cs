using DiGi.Core.Classes;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// One row of the <c>GET gis/buildingdata/byaddress</c> answer: the address point matched to a building, together with the projected building data of that building (DiGi-Organization/DiGi.GIS.WebAPI#85).
    /// <para><see cref="AddressPointMatch"/> is the same object <c>addresspoint/buildingsbyaddress</c> answers, its county part included, so the caller never has to ask which partition the building data was read from. <see cref="BuildingData"/> carries one key per projected column, named by the stored column unique id, plus <c>reference</c> and - when the read was confined to one partition - <c>county_id</c>.</para>
    /// <para>A key whose value is null means the stored row holds no value for that column; a building with no <c>building_data</c> row answers every projected key as null, which reads the same way. The answer does not separate the two.</para>
    /// </summary>
    public class BuildingDataByAddressItem : SerializableObject
    {
        [JsonInclude, JsonPropertyName(nameof(AddressPointMatch))]
        private readonly GIS.Classes.AddressPointMatch? addressPointMatch;

        // The bag is held as SerializableObjectWrapper rather than JsonObject: a JsonObject member is written by the
        // serializer as a Key/Value pair array and reads back empty, while the wrapper passes the raw JSON through
        // unchanged, so numbers stay numbers and nested arrays stay arrays across a round trip (DiGi.GIS.IO.Classes.Footprint holds its attribute bag for the same reason).
        [JsonInclude, JsonPropertyName(nameof(BuildingData))]
        private readonly SerializableObjectWrapper? buildingData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByAddressItem"/> class.
        /// </summary>
        /// <param name="addressPointMatch">The matched address point. It is cloned.</param>
        /// <param name="buildingData">The projected building data of the matched building, keyed by stored column unique id, or null when there is none.</param>
        public BuildingDataByAddressItem(GIS.Classes.AddressPointMatch? addressPointMatch, JsonObject? buildingData)
        {
            this.addressPointMatch = Core.Query.Clone(addressPointMatch);
            this.buildingData = buildingData is null ? null : new SerializableObjectWrapper(buildingData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByAddressItem"/> class by copying an existing instance.
        /// </summary>
        /// <param name="buildingDataByAddressItem">The source item to copy from.</param>
        public BuildingDataByAddressItem(BuildingDataByAddressItem? buildingDataByAddressItem)
            : base(buildingDataByAddressItem)
        {
            if (buildingDataByAddressItem != null)
            {
                addressPointMatch = Core.Query.Clone(buildingDataByAddressItem.addressPointMatch);
                buildingData = buildingDataByAddressItem.buildingData is null ? null : new SerializableObjectWrapper(buildingDataByAddressItem.buildingData);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingDataByAddressItem"/> class from a JSON object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing the item data.</param>
        public BuildingDataByAddressItem(JsonObject? jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets a clone of the matched address point: the address, how it was matched, the building it points at and the county part the data was read from.
        /// </summary>
        [JsonIgnore]
        public GIS.Classes.AddressPointMatch? AddressPointMatch
        {
            get
            {
                return Core.Query.Clone(addressPointMatch);
            }
        }

        /// <summary>
        /// Gets a deep copy of the projected building data of the matched building, keyed by stored column unique id, or null when there is none.
        /// </summary>
        [JsonIgnore]
        public JsonObject? BuildingData
        {
            get
            {
                return buildingData?.ToJsonObject();
            }
        }
    }
}
