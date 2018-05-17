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
using SolCodeGen.AbiEncoding;

namespace SolCodeGen
{

    public class SendParams
    {
        public Address? From { get; set; }
        public Address? To { get; set; }
        /// <summary>
        /// Value in wei
        /// </summary>
        public UInt256? Value { get; set; }

        public UInt256? Gas { get; set; }
        public UInt256? GasPrice { get; set; }
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
        public readonly Address ContractAddress;
        public readonly Address DefaultFromAccount;

        public abstract Abi Abi { get; }
        public abstract Doc DevDoc { get; }
        public abstract Doc UserDoc { get; }
        public abstract ReadOnlyMemory<byte> Bytecode { get; }

        public JsonRpcClient JsonRpcClient { get; protected set; }

        public BaseContract(Uri server, Address contractAddress, Address defaultFromAccount)
        {
            ContractAddress = contractAddress;
            DefaultFromAccount = defaultFromAccount;
            JsonRpcClient = new JsonRpcClient(server);
        }

        public BaseContract(JsonRpcClient rpcClient, Address contractAddress, Address defaultFromAccount)
        {
            ContractAddress = contractAddress;
            DefaultFromAccount = defaultFromAccount;
            JsonRpcClient = rpcClient;
        }

        public SendParams GetSendParams(SendParams optional)
        {
            return new SendParams
            {
                From = optional?.From ?? DefaultFromAccount,
                To = optional?.To ?? ContractAddress,
                Value = optional?.Value,
                Gas = optional?.Gas,
                GasPrice = optional?.GasPrice
            };
        }

        protected string GetCallData(string funcSignature, params IAbiTypeEncoder[] encoders)
        {
            var funcHash = MethodID.GetMethodID(funcSignature);
            var paramBytes = EncoderUtil.GetBytes(encoders);
            var dataHex = HexConverter.GetHexFromBytes(hexPrefix: true, funcHash, paramBytes);
            return dataHex;
        }
    }

    public class ContractConstructParams
    {
        public Uri Server { get; set; }

        public Address DefaultFromAccount { get; set; }
    }
}
