using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolCodeGen.JsonRpc.RequestMessages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SolCodeGen.JsonRpc
{
    public class JsonRpcClient
    {
        // Note: HttpClient is designed to be used as a singleton
        private static readonly HttpClient _httpClient = new HttpClient();

        readonly Uri _server;
        readonly Address _contractAddress;
        readonly Address _defaultFrom;

        public JsonRpcClient(
            Uri server, 
            Address contractAddress,
            Address defaultFrom)
        {
            _server = server;
            _contractAddress = contractAddress;
            _defaultFrom = defaultFrom;
        }

        /// <summary>eth_sendTransaction</summary>
        /// <param name="encodedHexParams">Hex string of the compiled code of a contract OR hash of the invoked method signature and encoded parameters</param>
        /// <returns>The transaction hash</returns>
        public async Task<Hash> SendTransaction(string encodedHexParams, SendParams sendParams = null)
        {
            var transactionData = new EthSendTransactionParam
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = encodedHexParams
            };
            var transactionRequest = new EthSendTransaction(transactionData);

            var jsonStr = JsonConvert.SerializeObject(transactionRequest);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            if (error != null)
            {
                throw error.ToException();
            }

            var hashHexStr = result.Value<string>();
            var hash = HexConverter.HexToValue<Hash>(hashHexStr);
            return hash;
        }

        public async Task<string> Call(string encodedHexParams, SendParams sendParams = null)
        {
            var callData = new EthCallParam
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = encodedHexParams
            };
            var transactionRequest = new EthCall(callData);

            var jsonStr = JsonConvert.SerializeObject(transactionRequest);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            if (error != null)
            {
                throw error.ToException();
            }

            return result.Value<string>();
        }

        public async Task<EthGetTransactionReceiptResponse> GetTransactionReceipt(Hash transactionHash)
        {
            var requestData = new EthGetTransactionReceipt(transactionHash);
            var jsonStr = JsonConvert.SerializeObject(requestData);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            if (error != null)
            {
                throw error.ToException();
            }
            var receipt = result.ToObject<EthGetTransactionReceiptResponse>();
            return receipt;
        }

        /// <param name="msgJson">Full message data as json string to POST</param>
        /// <returns></returns>
        async Task<(JsonRpcError Error, JToken Result)> InvokeRpcMethod(string msgJson)
        {
            var payload = new StringContent(msgJson, Encoding.UTF8, "application/json");
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, _server);
            requestMsg.Content = payload;
            var response = await _httpClient.SendAsync(requestMsg);
            var responseBody = await response.Content.ReadAsStringAsync();
            var jObj = JObject.Parse(responseBody);
            if (jObj.TryGetValue("error", out var errorToken))
            {
                var jsonRpcError = errorToken.ToObject<JsonRpcError>();
                return (jsonRpcError, null);
            }
            else if (jObj.TryGetValue("result", out var resultToken))
            {
                return (null, resultToken);
            }
            else
            {
                throw new Exception("Unexpected JSON-RPC response: " + responseBody);
            }
        }

    }
}
