﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolcNet.DataDescription.Output
{
    public class OutputDescription
    {
        public static OutputDescription FromJsonString(string jsonStr)
        {
            return JsonConvert.DeserializeObject<OutputDescription>(jsonStr, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
        }

 
        /// <summary>
        /// Optional: not present if no errors/warnings were encountered
        /// </summary>
        [JsonProperty("errors")]
        public IList<Error> Errors { get; set; }

        /// <summary>
        /// This contains the file-level outputs. In can be limited/filtered by the outputSelection settings.
        /// </summary>
        [JsonProperty("sources")]
        public Dictionary<string/*sol file name*/, SourceFileOutput> Sources { get; set; }

        /// <summary>
        /// This contains the contract-level outputs. It can be limited/filtered by the outputSelection settings.
        /// </summary>
        [JsonProperty("contracts")]
        public Dictionary<string/*sol file name*/, Dictionary<string/*contract name*/, Contract>> Contracts { get; set; }
    }

    public class SourceFileOutput
    {
        /// <summary>Identifier (used in source maps)</summary>
        [JsonProperty("id")]
        public uint? ID { get; set; }

        /// <summary>The AST object</summary>
        [JsonProperty("ast")]
        public Dictionary<string, object> Ast { get; set; }

        /// <summary>The legacy AST object</summary>
        [JsonProperty("legacyAST")]
        public Dictionary<string, object> LegacyAst { get; set; }
    }

    public class Contract
    {
        /// <summary>
        /// The Ethereum Contract ABI. If empty, it is represented as an empty array.
        /// See https://github.com/ethereum/wiki/wiki/Ethereum-Contract-ABI
        /// </summary>
        [JsonProperty("abi")]
        public IList<Abi> Abi { get; set; }

        [JsonProperty("metadata")]
        public string Metadata { get; set; }

        /// <summary>
        /// User documentation (natspec)
        /// </summary>
        [JsonProperty("userdoc")]
        public Doc Userdoc { get; set; }

        /// <summary>
        /// Developer documentation (natspec)
        /// </summary>
        [JsonProperty("devdoc")]
        public Doc Devdoc { get; set; }

        /// <summary>
        /// Intermediate representation (string)
        /// </summary>
        [JsonProperty("ir")]
        public object IR { get; set; }

        /// <summary>
        /// EVM-related outputs
        /// </summary>
        [JsonProperty("evm")]
        public Evm Evm { get; set; }
    }

}