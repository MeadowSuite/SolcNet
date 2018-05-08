using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolCodeGen.JsonRpc.RequestMessages
{
    public class EthGetTransactionReceipt : JsonRpcRequest<Hash>
    {
        public EthGetTransactionReceipt(Hash data) : base("eth_getTransactionReceipt", data)
        {
           
        }
    }

    public class EthGetTransactionReceiptResponse
    {
        /// <summary>
        /// DATA, 32 Bytes - hash of the transaction.
        /// </summary>
        [JsonProperty("transactionHash", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Hash TransactionHash { get; set; }

        /// <summary>
        /// QUANTITY - integer of the transactions index position in the block.
        /// </summary>
        [JsonProperty("transactionIndex")]
        public long TransactionIndex { get; set; }

        /// <summary>
        /// DATA, 32 Bytes - hash of the block where this transaction was in.
        /// </summary>
        [JsonProperty("blockHash", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Hash BlockHash { get; set; }

        /// <summary>
        /// QUANTITY - block number where this transaction was in.
        /// </summary>
        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        /// <summary>
        /// QUANTITY - The total amount of gas used when this transaction was executed in the block.
        /// </summary>
        [JsonProperty("cumulativeGasUsed")]
        public long CumulativeGasUsed { get; set; }

        /// <summary>
        /// QUANTITY - The amount of gas used by this specific transaction alone.
        /// </summary>
        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        /// <summary>
        /// DATA, 20 Bytes - The contract address created, if the transaction was a contract creation, otherwise null.
        /// </summary>
        [JsonProperty("contractAddress", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address? ContractAddress { get; set; }

        /// <summary>
        /// Array - Array of log objects, which this transaction generated.
        /// </summary>
        [JsonProperty("logs")]
        public FilterLogObject[] Logs { get; set; }

        /// <summary>
        /// DATA, 256 Bytes - Bloom filter for light clients to quickly retrieve related logs.
        /// </summary>
        [JsonProperty("logsBloom")]
        public string LogsBloom { get; set; }

        /// <summary>
        /// QUANTITY either 1 (success) or 0 (failure)
        /// </summary>
        [JsonProperty("status")]
        public long Status { get; set; }

    }
}
