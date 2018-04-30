using Newtonsoft.Json;
using System;

namespace SolcNet.DataDescription.Parsing
{
    public class NamedStringTokenConverterv : IEquatable<NamedStringTokenConverterv>
    {
        public virtual string Value { get; set; }

        public bool Equals(NamedStringTokenConverterv other) => other?.Value == Value;
        public override bool Equals(object obj) => obj is NamedStringTokenConverterv other ? other?.Value == Value : false;
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(NamedStringTokenConverterv a, NamedStringTokenConverterv b) => a?.Value == b?.Value;
        public static bool operator !=(NamedStringTokenConverterv a, NamedStringTokenConverterv b) => !(a == b);

        public override string ToString() => Value;
    }

    class NamedStringTokenConverter<TToken> : JsonConverter<TToken> where TToken : NamedStringTokenConverterv, new()
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
