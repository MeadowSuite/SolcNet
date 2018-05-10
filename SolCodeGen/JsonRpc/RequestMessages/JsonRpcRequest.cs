using Newtonsoft.Json;
using System;
using System.Threading;

namespace SolCodeGen.JsonRpc.RequestMessages
{
    public class JsonRpcRequest
    {
        static long RPC_MSG_ID = 1;

        [JsonProperty("jsonrpc")]
        public string JsonRpc = "2.0";

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("id")]
        public long ID = Interlocked.Increment(ref RPC_MSG_ID);

        [JsonProperty("params")]
        public object[] Params { get; set; }

        public JsonRpcRequest()
        {

        }

        public JsonRpcRequest(string method, params object[] args)
        {
            Method = method;
            Params = args;
        }
    }


}
