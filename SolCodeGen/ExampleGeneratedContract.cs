using SolCodeGen.AbiEncoding;
using SolCodeGen.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static ExampleGeneratedContract At(Uri server, Address address, Address defaultFromAccount)
        {
            return new ExampleGeneratedContract(server, address, defaultFromAccount);
        }

        public static Task<ExampleGeneratedContract> New(
            Uri server, 
            (string tokenName, uint decimals) args,
            SendParams sendParams = null)
        {

            throw new NotImplementedException();
        }
        

        public async Task<(TransactionReceipt Receipt, FilterLogObject[] EventLogs, bool? _result)> ExampleFunction(
            Address _to, UInt256 _amount, IEnumerable<ulong> _extraVals, IEnumerable<byte> _rawData,
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            // get function selector
            var funcHash = MethodID.GetMethodID("transfer(address,uint256,uint64[])");

            // get encoders for parameters
            var encoders = new IAbiTypeEncoder[] {
                EncoderFactory.LoadEncoder("address", _to),
                EncoderFactory.LoadEncoder("uint256", _amount),
                EncoderFactory.LoadEncoder("uint64[]", _extraVals, EncoderFactory.LoadEncoder("uint64", default(ulong))),
                EncoderFactory.LoadEncoder("bytes", _rawData)
            };

            // encoded parameters
            var paramHex = encoders.ToEncodedHex();

            // rpc eth_sendTransaction
            if (callType == CallType.Transaction)
            {
                var transactionHash = await JsonRpcClient.SendTransaction(paramHex, sendParams);
                // rpc eth_getTransactionReceipt
                var receipt = await JsonRpcClient.GetTransactionReceipt(transactionHash);
                // TODO: ...
                // parse log data into C# objects...
                return (receipt, receipt.Logs, null);
            }
            else if (callType == CallType.Call)
            {
                var resultData = await JsonRpcClient.Call(paramHex, sendParams);
                // TODO: ...
                // parse return values
                return (null, null, null);
            }
            else
            {
                throw new ArgumentException($"Unsupported call type: {callType}");
            }

        }

        public override string AbiJson { get; } = "{ TODO: ... }";
        public override string DevDocJson { get; } = "{ TODO: ... }";
        public override string UserDocJson { get; } = "{ TODO: ... }";

    }
}
