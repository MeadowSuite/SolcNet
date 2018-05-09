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
        /// <summary>
        /// eth_getBalance - 
        /// Returns the balance of the account of given address.
        /// </summary>
        /// <param name="account">20 Bytes - address to check for balance</param>
        /// <param name="blockTag">integer block number, or the string "latest", "earliest" or "pending", see the default block parameter</param>
        /// <returns></returns>
        public async Task<UInt256> GetBalance(Address account, BlockTagParameter blockTag)
        {
            var request = new JsonRpcRequest { Method = "eth_blockNumber" };
            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return HexConverter.HexToValue<long>(result.Value<string>());
        }

        /// <summary>
        /// eth_blockNumber - 
        /// Returns the number of most recent block
        /// </summary>
        public async Task<long> BlockNumber()
        {
            var request = new JsonRpcRequest { Method = "eth_blockNumber" };
            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return HexConverter.HexToValue<long>(result.Value<string>());
        }

        /// <summary>
        /// eth_getBlockByHash - 
        /// Returns information about a block by hash
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_getblockbyhash"/>
        /// </summary>
        /// <param name="hash">32 Bytes - Hash of a block</param>
        /// <param name="getFullTransactionObjects">If true it returns the full transaction objects, if false only the hashes of the transactions.</param>
        /// <returns>A block object, or null when no block was found</returns>
        public async Task<Block> GetBlockByHash(Hash hash, bool getFullTransactionObjects = false)
        {
            var request = new JsonRpcRequest<string>("eth_getBlockByHash", hash.GetHexString());
            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return result.Value<Block>();
        }

        /// <summary>
        /// eth_getBlockByNumber - 
        /// Returns information about a block by block number
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_getblockbynumber"/>
        /// </summary>
        /// <param name="blockNumber">integer of a block number</param>
        /// <param name="blockTag">"earliest", "latest" or "pending", as in the default block parameter.</param>
        /// <param name="getFullTransactionObjects">If true it returns the full transaction objects, if false only the hashes of the transactions.</param>
        public async Task<Block> GetBlockByNumber(long? blockNumber = null, BlockTagParameter? blockTag = null, bool getFullTransactionObjects = false)
        {
            if (blockTag == null && blockTag == null)
            {
                throw new ArgumentException($"A parameter of '{nameof(blockNumber)}' or '{nameof(blockTag)}' must be specified");
            }
            if (blockTag != null && blockTag != null)
            {
                throw new ArgumentException($"Both parameters '{nameof(blockNumber)}' and '{nameof(blockTag)}' must not be specified");
            }

            object[] methodParams = new object[2];
            methodParams[1] = getFullTransactionObjects;

            if (blockNumber.HasValue)
            {
                methodParams[0] = HexConverter.GetHexFromInteger(blockNumber.Value, hexPrefix: true);
            }
            else
            {
                methodParams[0] = JToken.FromObject(blockTag.Value).Value<string>();
            }

            var request = new JsonRpcRequest<object[]>("eth_getBlockByNumber", methodParams);
            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return result.Value<Block>();
        }

        /// <summary>
        /// eth_sendRawTransaction - 
        /// Creates new message call transaction or a contract creation for signed transactions.
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_sendrawtransaction"/>
        /// </summary>
        /// <param name="signedHexData"></param>
        /// <returns>the transaction hash, or the zero hash if the transaction is not yet available</returns>
        public async Task<Hash> SendRawTransaction(string signedHexData)
        {
            var request = new JsonRpcRequest<string>("eth_sendRawTransaction", signedHexData);
            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return result.Value<string>();
        }

        /// <summary>
        /// eth_sendTransaction - 
        /// Creates new message call transaction or a contract creation, if the data field contains code
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_sendtransaction"/>
        /// </summary>
        /// <param name="encodedHexParams">Hex string of the compiled code of a contract OR hash of the invoked method signature and encoded parameters</param>
        /// <returns>The transaction hash</returns>
        public async Task<Hash> SendTransaction(string encodedHexParams, SendParams sendParams = null)
        {
            var requestData = new EthSendTransactionParam
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = encodedHexParams
            };
            var request = new JsonRpcRequest<EthSendTransactionParam>("eth_sendTransaction", requestData);

            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            var hashHexStr = result.Value<string>();
            var hash = HexConverter.HexToValue<Hash>(hashHexStr);
            return hash;
        }

        /// <summary>
        /// eth_call - 
        /// Executes a new message call immediately without creating a transaction on the block chain
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_call"/>
        /// </summary>
        /// <returns>the return value of executed contract</returns>
        public async Task<string> Call(string encodedHexParams, SendParams sendParams = null)
        {
            var requestData = new EthCallParam
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = encodedHexParams
            };
            var request = new JsonRpcRequest<EthCallParam>("eth_call", requestData);

            var jsonStr = JsonConvert.SerializeObject(request);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            return result.Value<string>();
        }

        /// <summary>
        /// eth_getTransactionReceipt - 
        /// Returns the receipt of a transaction by transaction hash
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_gettransactionreceipt"/>
        /// </summary>
        /// <param name="transactionHash"></param>
        /// <returns></returns>
        public async Task<TransactionReceipt> GetTransactionReceipt(Hash transactionHash)
        {
            var requestData = new JsonRpcRequest<string>("eth_getTransactionReceipt", transactionHash.GetHexString());
            var jsonStr = JsonConvert.SerializeObject(requestData);
            var (error, result) = await InvokeRpcMethod(jsonStr);
            var receipt = result.ToObject<TransactionReceipt>();
            return receipt;
        }

        /// <param name="msgJson">Full message data as json string to POST</param>
        async Task<(JsonRpcError Error, JToken Result)> InvokeRpcMethod(string msgJson, bool throwOnError = true)
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
                if (throwOnError)
                {
                    throw jsonRpcError.ToException();
                }
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
