using Newtonsoft.Json;
using SolcNet.DataDescription.Output;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SolcNet.DataDescription.Parsing
{
    class SourceMapParser : JsonConverter<SourceMapEntry[]>
    {
        public override SourceMapEntry[] ReadJson(JsonReader reader, Type objectType, SourceMapEntry[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var val = reader.Value as string;
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            var valParts = val.Split(';');
            var entries = new SourceMapEntry[valParts.Length];
            for (var i = 0; i < entries.Length; i++)
            {
                if (valParts[i] == string.Empty)
                {
                    entries[i] = entries[i - 1];
                }
                else
                {
                    var entryParts = valParts[i].Split(':');
                    entries[i].Offset = entryParts[0] != string.Empty 
                        ? int.Parse(entryParts[0], CultureInfo.InvariantCulture)
                        : entries[i - 1].Offset;
                    entries[i].Length = entryParts.Length > 1 && entryParts[1] != string.Empty
                        ? int.Parse(entryParts[1], CultureInfo.InvariantCulture)
                        : entries[i - 1].Length;
                    entries[i].Index = entryParts.Length > 2 && entryParts[2] != string.Empty
                        ? int.Parse(entryParts[2], CultureInfo.InvariantCulture)
                        : entries[i - 1].Index;
                    entries[i].Jump = entryParts.Length > 3 && entryParts[3] != string.Empty
                        ? (JumpInstruction)(byte)entryParts[3][0]
                        : entries[i - 1].Jump;
                }
            }
            return entries;
        }

        public override void WriteJson(JsonWriter writer, SourceMapEntry[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
