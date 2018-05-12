using SolcNet.DataDescription.Output;
using SolCodeGen.JsonRpc;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        public readonly Address ContractAddress;
        public readonly Address DefaultFromAccount;

        public abstract string BytecodeHex { get; }
        public abstract string AbiJson { get; }
        public abstract string DevDocJson { get; }
        public abstract string UserDocJson { get; }


        protected Abi _abi;
        public Abi Abi => _abi ?? (_abi = AbiJson);

        protected Doc _devDoc;
        public Doc DevDoc => _devDoc ?? (_devDoc = DevDocJson);

        protected Doc _userDoc;
        public Doc UserDoc => _userDoc ?? (_userDoc = UserDocJson);

        protected ReadOnlyMemory<byte>? _bytecode;
        public ReadOnlyMemory<byte> Bytecode => (_bytecode ?? (_bytecode = BytecodeHex.HexToReadOnlyMemory())).Value;

        public JsonRpcClient JsonRpcClient { get; protected set; }

        public BaseContract(Uri server, Address contractAddress, Address defaultFromAccount)
        {
            Server = server;
            ContractAddress = contractAddress;
            DefaultFromAccount = defaultFromAccount;
            JsonRpcClient = new JsonRpcClient(Server, ContractAddress, DefaultFromAccount);
        }

        /// <summary>
        /// Deploys a contract that has no constructor arguments
        /// </summary>
        /// <param name="abiEncodedConstructorParams">ABI encoded function selector and constructor parameters</param>
        protected async Task Deploy(ReadOnlyMemory<byte> abiEncodedConstructorArgs, SendParams sendParams = null)
        { 
            var deploymentHex = HexConverter.GetHexFromBytes(hexPrefix: true, Bytecode, abiEncodedConstructorArgs);
            var transHash = await JsonRpcClient.SendTransaction(deploymentHex, sendParams: sendParams);
            var receipt = await JsonRpcClient.GetTransactionReceipt(transHash);

            //ContractAddress = receipt....
        }

        /// <summary>
        /// Deploys a contract that has no constructor arguments
        /// </summary>
        protected async Task Deploy(SendParams sendParams = null)
        {
            var deploymentHex = HexConverter.GetHexFromBytes(hexPrefix: true, Bytecode);
            var transHash = await JsonRpcClient.SendTransaction(deploymentHex, sendParams: sendParams);
            var receipt = await JsonRpcClient.GetTransactionReceipt(transHash);
            throw new NotImplementedException();
        }

        protected async Task SetFromAddress(Address addr)
        {
            //ContractAddress =  addr;
            throw new NotImplementedException();
        }
    }

    public class ContractConstructParams
    {
        public Uri Server { get; set; }

        public Address DefaultFromAccount { get; set; }
    }
}
