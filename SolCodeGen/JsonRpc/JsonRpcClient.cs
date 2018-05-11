using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolCodeGen.JsonRpc.RequestMessages;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// net_version - Returns the current network id.
        /// </summary>
        /// <returns>
        /// String - The current network id. Examples:
        /// "1": Ethereum Mainnet
        /// "2": Morden Testnet(deprecated)
        /// "3": Ropsten Testnet
        /// "4": Rinkeby Testnet
        /// "42": Kovan Testnet
        /// </returns>
        public async Task<string> Version()
        {
            
            var request = new JsonRpcRequest("net_version");
            var (error, result) = await InvokeRpcMethod(request);
            return result.Value<string>();
        }

        /// <summary>
        /// eth_protocolVersion - Returns the current ethereum protocol version
        /// </summary>
        /// <returns>String - The current ethereum protocol version</returns>
        public async Task<string> ProtocolVersion()
        {
            var request = new JsonRpcRequest("eth_protocolVersion");
            var (error, result) = await InvokeRpcMethod(request);
            return result.Value<string>();
        }

        /// <summary>
        /// evm_mine - 
        /// Special non-standard ganache client methods (not included within the original RPC specification).
        /// Force a block to be mined. Takes no parameters. Mines a block independent of whether or not mining is started or stopped.
        /// </summary>
        public async Task EvmMine()
        {
            var request = new JsonRpcRequest("evm_mine");
            var (error, result) = await InvokeRpcMethod(request);
        }

        /// <summary>
        /// eth_accounts - 
        /// Returns a list of addresses owned by client.
        /// </summary>
        /// <returns>Array of DATA, 20 Bytes - addresses owned by the client</returns>
        public async Task<Address[]> Accounts()
        {
            var request = new JsonRpcRequest("eth_accounts");
            var (error, result) = await InvokeRpcMethod(request);
            return result.ToObject<Address[]>();
        }

        /// <summary>
        /// eth_getBalance - 
        /// Returns the balance of the account of given address.
        /// </summary>
        /// <param name="account">20 Bytes - address to check for balance</param>
        /// <param name="blockTag">Defaults to "latest" block</param>
        /// <param name="blockNumber">integer block number (only if blockTag is not specified)</param>
        /// <returns>Eth balance in wei</returns>
        public async Task<UInt256> GetBalance(Address account, BlockTagParameter? blockTag = null, long? blockNumber = null)
        {
            var blockTagParam = GetBlockTagParams(blockTag, blockNumber);
            var methodParams = new object[]
            {
                account.GetHexString(hexPrefix: true),
                blockTagParam
            };
            
            var request = new JsonRpcRequest("eth_getBalance", methodParams);
            var (error, result) = await InvokeRpcMethod(request);
            return HexConverter.HexToInteger<UInt256>(result.Value<string>());
        }

        /// <summary>
        /// eth_blockNumber - 
        /// Returns the number of most recent block
        /// </summary>
        public async Task<long> BlockNumber()
        {
            var request = new JsonRpcRequest("eth_blockNumber");
            var (error, result) = await InvokeRpcMethod(request);
            return HexConverter.HexToInteger<long>(result.Value<string>());
        }

        /// <summary>
        /// eth_getBlockByHash - 
        /// Returns information about a block by hash
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_getblockbyhash"/>
        /// </summary>
        /// <param name="hash">32 Bytes - Hash of a block</param>
        /// <param name="getFullTransactionObjects">
        /// If true it returns the full transaction objects, if false only the hashes of the transactions.
        /// </param>
        /// <returns>A block object, or null when no block was found</returns>
        public async Task<Block> GetBlockByHash(Hash hash, bool getFullTransactionObjects = false)
        {
            var request = new JsonRpcRequest("eth_getBlockByHash", hash.GetHexString());
            var (error, result) = await InvokeRpcMethod(request);
            return result.ToObject<Block>();
        }

        /// <summary>
        /// eth_getBlockByNumber - 
        /// Returns information about a block by block number
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_getblockbynumber"/>
        /// </summary>
        /// <param name="getFullTransactionObjects">
        /// If true it returns the full transaction objects, if false only the hashes of the transactions.
        /// </param>
        /// <param name="blockTag">Defaults to "latest" block</param>
        /// <param name="blockNumber">integer block number (only if blockTag is not specified)</param>
        public async Task<Block> GetBlockByNumber(bool getFullTransactionObjects = false, BlockTagParameter ? blockTag = null, long? blockNumber = null)
        {
            var blockTagParam = GetBlockTagParams(blockTag, blockNumber);
            var methodParams = new object[]
            {
                blockTagParam,
                getFullTransactionObjects
            };

            var request = new JsonRpcRequest("eth_getBlockByNumber", methodParams);
            var (error, result) = await InvokeRpcMethod(request);
            return result.ToObject<Block>();
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
            var request = new JsonRpcRequest("eth_sendRawTransaction", signedHexData);
            var (error, result) = await InvokeRpcMethod(request);
            return result.Value<string>();
        }

        /// <summary>
        /// eth_sendTransaction - 
        /// Creates new message call transaction or a contract creation, if the data field contains code
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_sendtransaction"/>
        /// </summary>
        /// <param name="encodedHexParams">
        /// Hex string of the compiled code of a contract OR hash of the invoked 
        /// method signature and encoded parameters
        /// </param>
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
            var request = new JsonRpcRequest("eth_sendTransaction", requestData);

            var (error, result) = await InvokeRpcMethod(request);
            var hashHexStr = result.Value<string>();
            var hash = HexConverter.HexToValue<Hash>(hashHexStr);
            return hash;
        }


        /// <summary>
        /// eth_call - 
        /// Executes a new message call immediately without creating a transaction on the block chain
        /// <see href="https://github.com/ethereum/wiki/wiki/JSON-RPC#eth_call"/>
        /// </summary>
        /// <param name="encodedHexParams">Hash of the method signature and encoded parameters</param>
        /// <param name="sendParams">Specify to override the default</param>
        /// <param name="blockTag">Defaults to "latest" block</param>
        /// <param name="blockNumber">integer block number (only if blockTag is not specified)</param>
        /// <returns>the return value of executed contract</returns>
        public async Task<string> Call(string encodedHexParams, SendParams sendParams = null, BlockTagParameter? blockTag = null, long? blockNumber = null)
        {
            var blockTagParam = GetBlockTagParams(blockTag, blockNumber);

            var requestData = new EthCallParam
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = encodedHexParams
            };

            var request = new JsonRpcRequest("eth_call", requestData, blockTagParam);

            var (error, result) = await InvokeRpcMethod(request);
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
            var requestData = new JsonRpcRequest("eth_getTransactionReceipt", transactionHash.GetHexString());
            var (error, result) = await InvokeRpcMethod(requestData);
            var receipt = result.ToObject<TransactionReceipt>();
            return receipt;
        }

        public Task<(JsonRpcError Error, JToken Result)> InvokeRpcMethod(JsonRpcRequest request, bool throwOnError = true)
        {
            var jsonStr = JsonConvert.SerializeObject(request);
            return InvokeRpcMethod(jsonStr, throwOnError);
        }

        /// <param name="msgJson">Full message data as json string to POST</param>
        public async Task<(JsonRpcError Error, JToken Result)> InvokeRpcMethod(string msgJson, bool throwOnError = true)
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

        string GetBlockTagParams(BlockTagParameter? blockTag = null, long? blockNumber = null)
        {
            if (blockTag != null && blockTag != null)
            {
                throw new ArgumentException($"Both parameters '{nameof(blockNumber)}' and '{nameof(blockTag)}' must not be specified");
            }

            if (blockTag == null && blockNumber == null)
            {
                blockTag = BlockTagParameter.Latest;
            }

            if (blockTag.HasValue)
            {
                return JToken.FromObject(blockTag.Value).Value<string>();
            }
            else
            {
                return HexConverter.GetHexFromInteger(blockNumber.Value, hexPrefix: true);
            }
        }


    }
}
