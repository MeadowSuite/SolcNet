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

        protected SendParams GetSendParams(SendParams optional)
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


        protected T1 Decode<T1>(ReadOnlyMemory<byte> data,
            string n1, DecodeDelegate<T1> d1)
        {
            var buffer = data.Span;
            buffer = d1(n1, buffer, out var i1);
            return i1;
        }

        protected ValueTuple<T1, T2> Decode<T1, T2>(ReadOnlyMemory<byte> data,
            string n1, DecodeDelegate<T1> d1,
            string n2, DecodeDelegate<T2> d2)
        {
            var buffer = data.Span;
            return (
                DecodeWithDelegate(n1, d1, ref buffer),
                DecodeWithDelegate(n2, d2, ref buffer));
        }

        protected ValueTuple<T1, T2, T3> Decode<T1, T2, T3>(ReadOnlyMemory<byte> data,
            string n1, DecodeDelegate<T1> d1,
            string n2, DecodeDelegate<T2> d2,
            string n3, DecodeDelegate<T3> d3)
        {
            var buffer = data.Span;
            return (
                DecodeWithDelegate(n1, d1, ref buffer),
                DecodeWithDelegate(n2, d2, ref buffer),
                DecodeWithDelegate(n3, d3, ref buffer));
        }

        protected ValueTuple<T1, T2, T3, T4> Decode<T1, T2, T3, T4>(ReadOnlyMemory<byte> data,
            string n1, DecodeDelegate<T1> d1,
            string n2, DecodeDelegate<T2> d2,
            string n3, DecodeDelegate<T3> d3,
            string n4, DecodeDelegate<T4> d4)
        {
            var buffer = data.Span;
            return (
                DecodeWithDelegate(n1, d1, ref buffer),
                DecodeWithDelegate(n2, d2, ref buffer),
                DecodeWithDelegate(n3, d3, ref buffer),
                DecodeWithDelegate(n4, d4, ref buffer));
        }

        protected ValueTuple<T1, T2, T3, T4, T5> Decode<T1, T2, T3, T4, T5>(ReadOnlyMemory<byte> data,
            string n1, DecodeDelegate<T1> d1,
            string n2, DecodeDelegate<T2> d2,
            string n3, DecodeDelegate<T3> d3,
            string n4, DecodeDelegate<T4> d4,
            string n5, DecodeDelegate<T5> d5)
        {
            var buffer = data.Span;
            return (
                DecodeWithDelegate(n1, d1, ref buffer),
                DecodeWithDelegate(n2, d2, ref buffer),
                DecodeWithDelegate(n3, d3, ref buffer),
                DecodeWithDelegate(n4, d4, ref buffer),
                DecodeWithDelegate(n5, d5, ref buffer));
        }

        protected T DecodeWithDelegate<T>(string solType, DecodeDelegate<T> del, ref ReadOnlySpan<byte> buffer)
        {
            buffer = del(solType, buffer, out T val);
            return val;
        }
    }

    public class ContractConstructParams
    {
        public Uri Server { get; set; }

        public Address DefaultFromAccount { get; set; }
    }
}
