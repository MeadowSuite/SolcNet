using HoshoEthUtil;
using Newtonsoft.Json;
using System;

namespace SolcNet.DataDescription.Parsing
{
    class HexStringConverter : JsonConverter<byte[]>
    {
        public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var hexStr = (string)reader.Value;
            return HoshoEthUtil.HexUtil.HexToBytes(hexStr);
        }

        public override void WriteJson(JsonWriter writer, byte[] value, JsonSerializer serializer)
        {
            var hexString = HoshoEthUtil.HexUtil.GetHexFromBytes(value);
            writer.WriteToken(JsonToken.String, hexString);
        }
    }
}
