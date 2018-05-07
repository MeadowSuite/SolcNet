using SolcNet.DataDescription.Output;
using System;
using System.Runtime.InteropServices;
using System.Text;

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
}
