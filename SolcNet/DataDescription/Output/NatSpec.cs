using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolcNet.DataDescription.Output
{
    public class Doc
    {
        [JsonProperty("methods")]
        public Dictionary<string /*function signature*/, MethodDoc> Methods { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class MethodDoc
    {
        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("params")]
        public Dictionary<string /*param name*/, string /*description*/> Params { get; set; }

        [JsonProperty("return")]
        public string Return { get; set; }
    }

}
