using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SolCodeGen
{


    public class Invoker
    {
        //public delegate string[] ConvertToJsonRpcParams(TInput input);
        //public delegate TOutput ConvertFromJsonRpcResult(string[] input);


        // Note: HttpClient is designed to be used as a singleton
        private static readonly HttpClient _httpClient = new HttpClient();

        readonly Uri _server;
        readonly string[] _inputTypes;
        readonly string[] _outputTypes;
        readonly Address _contractAddress;
        readonly Address _defaultFrom;


        public Invoker(Uri server, 
            string[] inputTypes, 
            string[] outputTypes, 
            Address contractAddress,
            Address defaultFrom)
        {
            _server = server;
            _inputTypes = inputTypes;
            _outputTypes = outputTypes;
            _contractAddress = contractAddress;
            _defaultFrom = defaultFrom;
        }

        public async Task<TOutput> Send<TInput, TOutput>(TInput input, SendParams sendParams = null)
        {
            var transactionData = new EthSendTransactionRequestData
            {
                From = sendParams?.From ?? _defaultFrom,
                To = sendParams?.To ?? _contractAddress,
                Value = sendParams?.Value ?? 0,
                Data = false ? (byte[])null : throw new NotImplementedException()
            };
            var transactionRequest = new EthSendTransactionRequest(transactionData);

            var jsonStr = JsonConvert.SerializeObject(transactionRequest);

            var payload = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, _server);
            requestMsg.Content = payload;
            var response = await _httpClient.SendAsync(requestMsg);
            var responseBody = await response.Content.ReadAsStringAsync();

            return default;
        }

    }
}
