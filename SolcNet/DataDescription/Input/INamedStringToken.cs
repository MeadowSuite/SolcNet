using Newtonsoft.Json;
using System;

namespace SolcNet.DataDescription.Input
{
    interface INamedStringToken
    {
        string Value { get; set; }
    }

    class NamedStringTokenConverter<TToken> : JsonConverter<TToken> where TToken : INamedStringToken, new()
    {
        public override TToken ReadJson(JsonReader reader, Type objectType, TToken existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return new TToken { Value = (string)reader.Value };
        }

        public override void WriteJson(JsonWriter writer, TToken value, JsonSerializer serializer)
        {
            writer.WriteToken(JsonToken.String, value.Value);
        }
    }
}
