using SolcNet.DataDescription.Output;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolCodeGen
{

    public class SendParams
    {
        public Address? From { get; set; }
        public Address? To { get; set; }
        public UInt256? Value { get; set; }
    }

    public enum CallType
    {
        /// <summary>
        /// Creates new message call transaction on the block chain.
        /// </summary>
        Transaction,
        /// <summary>
        /// Executes a new message call immediately without creating a transaction on the block chain.
        /// </summary>
        Call
    }

    public abstract class BaseContract
    {
        public readonly Uri Server;
        public readonly Address Address;
        public readonly Address DefaultFromAccount;

        public abstract string AbiJson { get; }
        public abstract string DevDocJson { get; }
        public abstract string UserDocJson { get; }

        protected Abi _abi;
        public Abi Abi => _abi ?? (_abi = AbiJson);

        protected Doc _devDoc;
        public Doc DevDoc => _devDoc ?? (_devDoc = DevDocJson);

        protected Doc _userDoc;
        public Doc UserDoc => _userDoc ?? (_userDoc = UserDocJson);

        public BaseContract(Uri server, Address address, Address defaultFromAccount)
        {
            Server = server;
            Address = address;
            DefaultFromAccount = defaultFromAccount;
        }

        public string GetFunctionHash(string functionSignature)
        {
            var bytes = Encoding.UTF8.GetBytes("transfer(address,uint256)");
            var hash = Keccak.ComputeHash(bytes).Slice(0, 4);
            string funcSignature = HexConverter.BytesToHex(hash, bigEndian: false, hexPrefix: false);
            return funcSignature;
        }

    }

    public class ContractConstructParams
    {
        public Uri Server { get; set; }

        public Address DefaultFromAccount { get; set; }
    }

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
        
        public Task<(TransactionReceipt Receipt, EventLog[] EventLogs, bool _result)> Transfer(
            Address _to, UInt256 _amount, 
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var funcHash = GetFunctionHash("transfer(address,uint256)");

            // encode transaction arguments

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
