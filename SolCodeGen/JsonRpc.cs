using Newtonsoft.Json;
using System;
using System.Threading;

namespace SolCodeGen
{
    public class JsonRpcRequest<TParams>
    {
        static long RPC_MSG_ID = 1;

        [JsonProperty("jsonrpc")]
        public string JsonRpc = "2.0";

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public TParams[] Params { get; set; }

        [JsonProperty("id")]
        public long ID = Interlocked.Increment(ref RPC_MSG_ID);

        public JsonRpcRequest() { }


        public JsonRpcRequest(string method, TParams methodParams)
        {
            Method = method;
            Params = new[] { methodParams };
        }
    }

    public class EthSendTransactionRequest : JsonRpcRequest<EthSendTransactionRequestData>
    {
        public EthSendTransactionRequest(EthSendTransactionRequestData data) : base("eth_sendTransaction", data)
        {

        }
    }

    public class EthSendTransactionRequestData
    {
        /// <summary>
        /// DATA, 20 Bytes - The address the transaction is send from.
        /// </summary>
        [JsonProperty("from", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address From { get; set; }

        /// <summary>
        /// DATA, 20 Bytes - (optional when creating new contract) The address the transaction is directed to.
        /// </summary>
        [JsonProperty("to", ItemConverterType = typeof(JsonRpcHexConverter))]
        public Address To { get; set; }

        /// <summary>
        /// QUANTITY - (optional, default: 90000) Integer of the gas provided for the transaction execution. It will return unused gas.
        /// </summary>
        [JsonProperty("gas", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256 Gas { get; set; }

        /// <summary>
        /// QUANTITY - (optional, default: To-Be-Determined) Integer of the gasPrice used for each paid gas
        /// </summary>
        [JsonProperty("gasPrice", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256 GasPrice { get; set; }

        /// <summary>
        /// QUANTITY - (optional) Integer of the value sent with this transaction
        /// </summary>
        [JsonProperty("value", ItemConverterType = typeof(JsonRpcHexConverter))]
        public UInt256 Value { get; set; }

        [JsonProperty("data", ItemConverterType = typeof(JsonRpcHexConverter))]
        public byte[] Data { get; set; }
    }


    class JsonRpcHexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string hex)
            {
                return HexConverter.HexToObject(objectType, hex);
            }
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string val = HexConverter.GetHexFromObject(value, hexPrefix: true);
            writer.WriteToken(JsonToken.String, val);
        }


    }
}
