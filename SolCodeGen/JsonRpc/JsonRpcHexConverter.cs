using Newtonsoft.Json;
using System;

namespace SolCodeGen.JsonRpc
{
    class JsonRpcHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string hex)
            {
                if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    objectType = Nullable.GetUnderlyingType(objectType);
                }
                return HexConverter.HexToObject(objectType, hex);
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string val = HexConverter.GetHexFromObject(value, hexPrefix: true);
            writer.WriteToken(JsonToken.String, val);
        }


    }
}
