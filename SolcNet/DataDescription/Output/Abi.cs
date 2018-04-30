using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolcNet.DataDescription.Output
{

    public class Abi
    {
        /// <summary>
        /// "function", "constructor", or "fallback" (the unnamed "default" function);
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The name of the function
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inputs")]
        public IList<Input> Inputs { get; set; }

        [JsonProperty("outputs")]
        public IList<Output> Outputs { get; set; }

        /// <summary>
        /// true if function accepts ether, defaults to false
        /// </summary>
        [JsonProperty("payable")]
        public bool? Payable { get; set; }

        /// <summary>
        /// a string with one of the following values: pure (specified to not read blockchain state), view (specified to not modify the blockchain state), nonpayable and payable (same as payable above).
        /// </summary>
        [JsonProperty("stateMutability")]
        public string StateMutability { get; set; }

        /// <summary>
        /// true if function is either pure or view
        /// </summary>
        [JsonProperty("constant")]
        public bool? Constant { get; set; }

        /// <summary>
        /// true if the event was declared as anonymous
        /// </summary>
        [JsonProperty("anonymous")]
        public bool? Anonymous { get; set; }
    }

    public class Input
    {
        /// <summary>
        /// the name of the parameter
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// the canonical type of the parameter (more below).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// used for tuple types
        /// </summary>
        [JsonProperty("components")]
        public List<Component> Components { get; set; }

        /// <summary>
        /// if the field is part of the log’s topics, false if it one of the log’s data segment.
        /// </summary>
        [JsonProperty("indexed")]
        public bool? Indexed { get; set; }
    }

    /// <summary>
    /// used for tuple types
    /// </summary>
    public class Component
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("components")]
        public List<Component> Components { get; set; }
    }

    public class Output
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("components")]
        public List<Component> Components { get; set; }
    }


}
