using SolCodeGen.SolidityTypeEncoding;
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
        

        public Task<(TransactionReceipt Receipt, EventLog[] EventLogs, bool _result)> ExampleFunction(
            Address _to, UInt256 _amount, IEnumerable<ulong> _extraVals,
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var funcHash = GetFunctionHash("transfer(address,uint256,uint64[])");

            // get encoders for parameters
            var encoders = new ISolidityTypeEncoder[] {
                EncoderFactory.LoadEncoder("address", _to),
                EncoderFactory.LoadEncoder("uint256", _amount),
                EncoderFactory.LoadEncoder("uint64[]", _extraVals, EncoderFactory.LoadEncoder("uint64", default(ulong)))
            };

            // get length of all encoded params
            var len = encoders.Sum(e => e.GetEncodedSize());

            // create buffer to write encoded params into
            Span<byte> buffer = stackalloc byte[len];

            // encode transaction arguments
            var cursor = buffer;
            foreach(var encoder in encoders)
            {
                cursor = encoder.Encode(cursor);
            }

            // start event log filter with eth_newFilter

            // rpc eth_sendTransaction

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
