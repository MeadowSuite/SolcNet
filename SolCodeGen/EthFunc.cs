using SolCodeGen.JsonRpc;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SolCodeGen
{
    public class EthFunc<TReturn>
    {

        BaseContract _contract;
        string _callData;
        Func<byte[], TReturn> _call;

        public EthFunc(BaseContract contract, string callData, Func<byte[], TReturn> call)
        {
            _contract = contract;
            _callData = callData;
            _call = call;
        }

        public TaskAwaiter<TransactionReceipt> GetAwaiter()
        {
            return SendTransaction(null).GetAwaiter();
        }

        public async Task<TReturn> Call(SendParams sendParams = null)
        {
            var callResult = await _contract.JsonRpcClient.Call(_callData, _contract.GetSendParams(sendParams));
            var data = HexConverter.HexToBytes(callResult);
            return _call(data);
        }

        public async Task<TransactionReceipt> SendTransaction(SendParams sendParams = null)
        {
            var transactionHash = await _contract.JsonRpcClient.SendTransaction(_callData, _contract.GetSendParams(sendParams));
            var receipt = await _contract.JsonRpcClient.GetTransactionReceipt(transactionHash);
            return receipt;
        }

    }

}
