﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SolCodeGen.JsonRpc
{
    public class FilterLogObject
    {
        /// <summary>
        /// TAG - true when the log was removed, due to a chain reorganization. false if its a valid log
        /// </summary>
        [JsonProperty("removed")]
        public bool Removed { get; set; }

        /// <summary>
        /// QUANTITY - integer of the log index position in the block. null when its pending log.
        /// </summary>
        [JsonProperty("logIndex", ItemConverterType = typeof(JsonRpcHexConverter))]
        public ulong LogIndex { get; set; }

        /// <summary>
        /// QUANTITY - integer of the transactions index position log was created from. null when its pending log.
        /// </summary>
        [JsonProperty("transactionIndex", ItemConverterType = typeof(JsonRpcHexConverter))]
        public ulong TransactionIndex { get; set; }

        /// <summary>
        /// DATA, 32 Bytes - hash of the transactions this log was created from. null when its pending log.
        /// </summary>
        [JsonProperty("transactionHash", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Hash TransactionHash { get; set; }

        /// <summary>
        /// DATA, 32 Bytes - hash of the block where this log was in. null when its pending. null when its pending log.
        /// </summary>
        [JsonProperty("blockHash", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Hash BlockHash { get; set; }

        /// <summary>
        /// QUANTITY - the block number where this log was in. null when its pending. null when its pending log.
        /// </summary>
        [JsonProperty("blockNumber", ItemConverterType = typeof(JsonRpcHexConverter))]
        public ulong BlockNumber { get; set; }

        /// <summary>
        /// DATA, 20 Bytes - address from which this log originated.
        /// </summary>
        [JsonProperty("address", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address Address { get; set; }

        /// <summary>
        /// DATA - contains one or more 32 Bytes non-indexed arguments of the log.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Array of DATA - Array of 0 to 4 32 Bytes DATA of indexed log arguments. 
        /// (In solidity: The first topic is the hash of the signature of the event 
        /// (e.g. Deposit(address,bytes32,uint256)), except you declared the event 
        /// with the anonymous specifier.)
        /// </summary>
        [JsonProperty("topics")]
        public string[] Topics { get; set; }


        /// <summary>
        /// Properties not deserialized any members
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> ExtraFields { get; set; }
    }
}
