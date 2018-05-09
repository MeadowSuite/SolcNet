using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SolCodeGen.JsonRpc
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BlockTagParameter
    {
        [EnumMember(Value = "earliest")]
        Earliest,

        [EnumMember(Value = "latest")]
        Latest,

        [EnumMember(Value = "pending")]
        Pending
    }
}
