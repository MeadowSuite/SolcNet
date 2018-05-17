using SolCodeGen.AbiEncoding;
using SolCodeGen.JsonRpc;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SolCodeGen
{
    public class EthFunc<TReturn>
    {
        protected BaseContract _contract;
        protected string _callData;
        protected ParseResponseDelegate _parseResponse;

        public delegate TReturn ParseResponseDelegate(ReadOnlyMemory<byte> data);

        public EthFunc(BaseContract contract, string callData, ParseResponseDelegate parseResponse)
        {
            _contract = contract;
            _callData = callData;
            _parseResponse = parseResponse;
        }

        public TaskAwaiter<TransactionReceipt> GetAwaiter()
        {
            return SendTransaction(null).GetAwaiter();
        }

        public async Task<TReturn> Call(SendParams sendParams = null)
        {
            var callResult = await _contract.JsonRpcClient.Call(_callData, _contract.GetSendParams(sendParams));
            var data = HexConverter.HexToBytes(callResult);
            var result = _parseResponse(data);
            return result;
        }

        public async Task<TransactionReceipt> SendTransaction(SendParams sendParams = null)
        {
            var transactionHash = await _contract.JsonRpcClient.SendTransaction(_callData, _contract.GetSendParams(sendParams));
            var receipt = await _contract.JsonRpcClient.GetTransactionReceipt(transactionHash);
            return receipt;
        }

        /* 
         * Preface: lots of code duplication follows..
         * This is the idiotmatic method for pseudo-variadic generics in C#.
         * The C# language designers wanted to avoid the C# generic syntax from
         * becoming a Turing complete nightmare like the C++ template syntax.
         * 
         * Simplicity at the expense of minor code duplication.
         * 
         * An example of Microsoft doing the same thing for Tuple<T1... Tn>:
         *  https://referencesource.microsoft.com/#mscorlib/system/tuple.cs,83
         * And Action<T1... Tn>:
         *  https://referencesource.microsoft.com/#mscorlib/system/action.cs,29
         */

        public static EthFunc<T1> Create<T1>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1)
        {
            T1 parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                return i1;
            }
            return new EthFunc<T1>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2)> Create<T1, T2>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2)
        {
            (T1, T2) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                return (i1, i2);
            }
            return new EthFunc<(T1, T2)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3)> Create<T1, T2, T3>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3)
        {
            (T1, T2, T3) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                return (i1, i2, i3);
            }
            return new EthFunc<(T1, T2, T3)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3, T4)> Create<T1, T2, T3, T4>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3,
            string s4, DecodeDelegate<T4> d4)
        {
            (T1, T2, T3, T4) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                d4(s4, ref buffer, out var i4);
                return (i1, i2, i3, i4);
            }
            return new EthFunc<(T1, T2, T3, T4)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3, T4, T5)> Create<T1, T2, T3, T4, T5>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3,
            string s4, DecodeDelegate<T4> d4,
            string s5, DecodeDelegate<T5> d5)
        {
            (T1, T2, T3, T4, T5) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                d4(s4, ref buffer, out var i4);
                d5(s5, ref buffer, out var i5);
                return (i1, i2, i3, i4, i5);
            }
            return new EthFunc<(T1, T2, T3, T4, T5)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3, T4, T5, T6)> Create<T1, T2, T3, T4, T5, T6>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3,
            string s4, DecodeDelegate<T4> d4,
            string s5, DecodeDelegate<T5> d5,
            string s6, DecodeDelegate<T6> d6)
        {
            (T1, T2, T3, T4, T5, T6) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                d4(s4, ref buffer, out var i4);
                d5(s5, ref buffer, out var i5);
                d6(s6, ref buffer, out var i6);
                return (i1, i2, i3, i4, i5, i6);
            }
            return new EthFunc<(T1, T2, T3, T4, T5, T6)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3, T4, T5, T6, T7)> Create<T1, T2, T3, T4, T5, T6, T7>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3,
            string s4, DecodeDelegate<T4> d4,
            string s5, DecodeDelegate<T5> d5,
            string s6, DecodeDelegate<T6> d6,
            string s7, DecodeDelegate<T7> d7)
        {
            (T1, T2, T3, T4, T5, T6, T7) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                d4(s4, ref buffer, out var i4);
                d5(s5, ref buffer, out var i5);
                d6(s6, ref buffer, out var i6);
                d7(s7, ref buffer, out var i7);
                return (i1, i2, i3, i4, i5, i6, i7);
            }
            return new EthFunc<(T1, T2, T3, T4, T5, T6, T7)>(contract, callData, parse);
        }

        public static EthFunc<(T1, T2, T3, T4, T5, T6, T7, T8)> Create<T1, T2, T3, T4, T5, T6, T7, T8>(
            BaseContract contract, string callData,
            string s1, DecodeDelegate<T1> d1,
            string s2, DecodeDelegate<T2> d2,
            string s3, DecodeDelegate<T3> d3,
            string s4, DecodeDelegate<T4> d4,
            string s5, DecodeDelegate<T5> d5,
            string s6, DecodeDelegate<T6> d6,
            string s7, DecodeDelegate<T7> d7,
            string s8, DecodeDelegate<T8> d8)
        {
            (T1, T2, T3, T4, T5, T6, T7, T8) parse(ReadOnlyMemory<byte> data)
            {
                var buffer = data.Span;
                d1(s1, ref buffer, out var i1);
                d2(s2, ref buffer, out var i2);
                d3(s3, ref buffer, out var i3);
                d4(s4, ref buffer, out var i4);
                d5(s5, ref buffer, out var i5);
                d6(s6, ref buffer, out var i6);
                d7(s7, ref buffer, out var i7);
                d8(s8, ref buffer, out var i8);
                return (i1, i2, i3, i4, i5, i6, i7, i8);
            }
            return new EthFunc<(T1, T2, T3, T4, T5, T6, T7, T8)>(contract, callData, parse);
        }
    }

}
