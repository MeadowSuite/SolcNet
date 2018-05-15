using SolcNet.DataDescription.Output;
using SolCodeGen.AbiEncoding;
using SolCodeGen.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SolCodeGen
{
    public class ExampleGeneratedContract : BaseContract
    {
        public class TransferEvent : EventLog<(Address from, Address to, UInt256 value)>
        {
            public TransferEvent((Address from, Address to, UInt256 value) logData, EventLog eventLog) 
                : base(logData, eventLog)
            {

            }
        }

        private ExampleGeneratedContract(Uri server, Address address, Address defaultFromAccount) 
            : base(server, address, defaultFromAccount)
        {

        }

        private ExampleGeneratedContract(JsonRpcClient rpcClient, Address address, Address defaultFromAccount)
            : base(rpcClient, address, defaultFromAccount)
        {

        }

        public static ExampleGeneratedContract At(Uri server, Address address, Address defaultFromAccount)
        {
            return new ExampleGeneratedContract(server, address, defaultFromAccount);
        }

        public static async Task<ExampleGeneratedContract> New(
            JsonRpcClient rpcClient, 
            (UInt256 _name, bool _enableThing, UInt256 _last) args,
            SendParams sendParams,
            Address defaultFromAccount)
        {
            var encodedParams = EncoderUtil.GetBytes(
                EncoderFactory.LoadEncoder("uint256", args._name),
                EncoderFactory.LoadEncoder("bool", args._enableThing),
                EncoderFactory.LoadEncoder("uint256", args._last)
            );

            var contractAddr = await ContractFactory.Deploy(rpcClient, BYTECODE.Value, encodedParams, sendParams);
            return new ExampleGeneratedContract(rpcClient, contractAddr, defaultFromAccount);
        }

        public static async Task<ExampleGeneratedContract> New(
            JsonRpcClient rpcClient,
            SendParams sendParams,
            Address defaultFromAccount)
        {
            var contractAddr = await ContractFactory.Deploy(rpcClient, BYTECODE.Value, sendParams);
            return new ExampleGeneratedContract(rpcClient, contractAddr, defaultFromAccount);
        }

        public async Task<(TransactionReceipt Receipt, FilterLogObject[] EventLogs, UInt256? result)> givenName(
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var funcHash = MethodID.GetMethodID("givenName()");
            var dataHex = HexConverter.GetHexFromBytes(hexPrefix: true, funcHash);
            var resultData = await JsonRpcClient.Call(dataHex, GetSendParams(sendParams));
            return (null, null, HexConverter.HexToInteger<UInt256>(resultData));
        }

        public class EthFunc<TReturn> 
        {
            private Func<SendParams, Task<TReturn>> _call;
            public Task<TReturn> Call(SendParams sendParam = null) => _call(sendParam);

            private Func<SendParams, Task<TransactionReceipt>> _sendTransaction;
            public Task<TransactionReceipt> SendTransaction(SendParams sendParam = null) => _sendTransaction(sendParam);

            public EthFunc(Func<SendParams, Task<TReturn>> call, Func<SendParams, Task<TransactionReceipt>> sendTransaction)
            {
                _call = call;
                _sendTransaction = sendTransaction;
            }

            public TaskAwaiter<TransactionReceipt> GetAwaiter()
            {
                return _sendTransaction(null).GetAwaiter();
            }
        }

        public EthFunc<string> echoString(string str)
        {
            var callData = GetCallData("echoString(string)",
                EncoderFactory.LoadEncoder("string", str));

            return new EthFunc<string>(
                async (sendParams) =>
                {
                    var callResult = await JsonRpcClient.Call(callData, GetSendParams(sendParams));
                    // TODO: decode
                    throw new NotImplementedException();
                },
                async (sendParams) =>
                {
                    var transactionHash = await JsonRpcClient.SendTransaction(callData, GetSendParams(sendParams));
                    var receipt = await JsonRpcClient.GetTransactionReceipt(transactionHash);
                    return receipt;
                });
        }

        public EthFunc<(Address addr, UInt256 num, string str)> echoMany(Address addr, UInt256 num, string str)
        {
            var callData = GetCallData("echoMany(address,uint256,string)",
                EncoderFactory.LoadEncoder("address", addr),
                EncoderFactory.LoadEncoder("uint256", num),
                EncoderFactory.LoadEncoder("string", str));

            return new EthFunc<(Address addr, UInt256 num, string str)>(
                async (sendParams) =>
                {
                    var callResult = await JsonRpcClient.Call(callData, GetSendParams(sendParams));
                    // TODO: decode
                    throw new NotImplementedException();
                },
                async (sendParams) =>
                {
                    var transactionHash = await JsonRpcClient.SendTransaction(callData, GetSendParams(sendParams));
                    var receipt = await JsonRpcClient.GetTransactionReceipt(transactionHash);
                    return receipt;
                });
        }


        public async Task<(TransactionReceipt Receipt, FilterLogObject[] EventLogs, bool? isNine)> myFunc(
            UInt256 _num, SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var callData = GetCallData("myFunc(uint256)",
                EncoderFactory.LoadEncoder("uint256", _num));

            // rpc eth_sendTransaction
            if (callType == CallType.Transaction)
            {
                var transactionHash = await JsonRpcClient.SendTransaction(callData, GetSendParams(sendParams));
                // rpc eth_getTransactionReceipt
                var receipt = await JsonRpcClient.GetTransactionReceipt(transactionHash);
                // TODO: ...
                // parse log data into C# objects...
                return (receipt, receipt.Logs, null);
            }
            else if (callType == CallType.Call)
            {
                var resultData = await JsonRpcClient.Call(callData, GetSendParams(sendParams));
                // TODO: ...
                // parse return values
                return (null, null, HexConverter.HexToInteger<UInt256>(resultData) == 1);
            }
            else
            {
                throw new ArgumentException($"Unsupported call type: {callType}");
            }
        }

        public async Task<(TransactionReceipt Receipt, FilterLogObject[] EventLogs, bool? _result)> ExampleFunction(
            Address _to, UInt256 _amount, IEnumerable<ulong> _extraVals, IEnumerable<byte> _rawData,
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var callData = GetCallData("transfer(address,uint256,uint64[])",
                EncoderFactory.LoadEncoder("address", _to),
                EncoderFactory.LoadEncoder("uint256", _amount),
                EncoderFactory.LoadEncoder("uint64[]", _extraVals, EncoderFactory.LoadEncoder("uint64", default(ulong))),
                EncoderFactory.LoadEncoder("bytes", _rawData));

            // rpc eth_sendTransaction
            if (callType == CallType.Transaction)
            {
                var transactionHash = await JsonRpcClient.SendTransaction(callData, GetSendParams(sendParams));
                // rpc eth_getTransactionReceipt
                var receipt = await JsonRpcClient.GetTransactionReceipt(transactionHash);
                // TODO: ...
                // parse log data into C# objects...
                return (receipt, receipt.Logs, null);
            }
            else if (callType == CallType.Call)
            {
                var resultData = await JsonRpcClient.Call(callData, GetSendParams(sendParams));
                // TODO: ...
                // parse return values
                return (null, null, null);
            }
            else
            {
                throw new ArgumentException($"Unsupported call type: {callType}");
            }

        }

        public override ReadOnlyMemory<byte> Bytecode => BYTECODE.Value;
        public override Abi Abi => ABI.Value;
        public override Doc DevDoc => DEV_DOC.Value;
        public override Doc UserDoc => USER_DOC.Value;

        public static readonly Lazy<ReadOnlyMemory<byte>> BYTECODE = new Lazy<ReadOnlyMemory<byte>>(() => BYTECODE_HEX.HexToReadOnlyMemory());
        public static readonly Lazy<Abi> ABI = new Lazy<Abi>(() => ABI_JSON);
        public static readonly Lazy<Doc> DEV_DOC = new Lazy<Doc>(() => DEV_DOC_JSON);
        public static readonly Lazy<Doc> USER_DOC = new Lazy<Doc>(() => USER_DOC_JSON);

        public static /*readonly*/ string BYTECODE_HEX = "TODO...";
        public static /*readonly*/ string ABI_JSON = "TODO";
        public static readonly string DEV_DOC_JSON = "TODO";
        public static readonly string USER_DOC_JSON = "TODO";


    }
}
