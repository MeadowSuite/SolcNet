using SolCodeGen.AbiEncoding;
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
        

        public async Task<(object Receipt, EventLog[] EventLogs, bool _result)> ExampleFunction(
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

            // start event log filter with eth_newFilter
            // TODO:...

            string resultData;

            // rpc eth_sendTransaction
            if (callType == CallType.Transaction)
            {
                var transactionHash = await JsonRpcClient.SendTransaction(paramHex, sendParams);
            }
            else if (callType == CallType.Call)
            {
                resultData = await JsonRpcClient.Call(paramHex, sendParams);
            }
            else
            {
                throw new ArgumentException($"Unsupported call type: {callType}");
            }

            // rpc eth_getTransactionReceipt

            // parse return values

            // get event logs with eth_getFilterChanges

            // parse event logs

            throw new NotImplementedException();

        }

        public override string AbiJson { get; } = "{ TODO: ... }";
        public override string DevDocJson { get; } = "{ TODO: ... }";
        public override string UserDocJson { get; } = "{ TODO: ... }";

        /*
        public Invoker<(Address _to, UInt256 _amount), (Receipt Receipt, bool _result)> Transfer
        {
            get
            {
                return new Invoker<
                    (Address _to, UInt256 _amount),
                    (Receipt Receipt, bool _result)>
                (
                    Server,
                    new string[] { "address", "uint" },
                    new string[] { "bool" },
                    DefaultFromAccount
                );
            }
        }
        */
    }
}
