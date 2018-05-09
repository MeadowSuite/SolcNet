using Newtonsoft.Json;
using System;

namespace SolCodeGen.JsonRpc.RequestMessages
{
    public class EthSendTransactionParam
    {
        /// <summary>
        /// DATA, 20 Bytes - The address the transaction is send from.
        /// </summary>
        [JsonProperty("from", Required = Required.Always, ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address? From { get; set; }

        /// <summary>
        /// DATA, 20 Bytes - (optional when creating new contract) The address the transaction is directed to.
        /// </summary>
        [JsonProperty("to", Required = Required.Always, ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address? To { get; set; }

        /// <summary>
        /// QUANTITY - (optional, default: 90000) Integer of the gas provided for the transaction execution. It will return unused gas.
        /// </summary>
        [JsonProperty("gas", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256? Gas { get; set; }

        /// <summary>
        /// QUANTITY - (optional, default: To-Be-Determined) Integer of the gasPrice used for each paid gas
        /// </summary>
        [JsonProperty("gasPrice", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256? GasPrice { get; set; }

        /// <summary>
        /// QUANTITY - (optional) Integer of the value sent with this transaction
        /// </summary>
        [JsonProperty("value", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256? Value { get; set; }

        /// <summary>
        /// Hex string of the compiled code of a contract OR hash of the invoked method signature and encoded parameters
        /// </summary>
        [JsonProperty("data", Required = Required.Always)]
        public string Data { get; set; }

        /// <summary>
        /// QUANTITY - (optional) Integer of a nonce. This allows to overwrite your own pending transactions that use the same nonce.
        /// </summary>
        [JsonProperty("nonce")]
        public long? Nonce { get; set; }
    }
}
