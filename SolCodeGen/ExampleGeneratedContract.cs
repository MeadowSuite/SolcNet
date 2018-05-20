using SolcNet.DataDescription.Output;
using SolCodeGen.AbiEncoding;
using SolCodeGen.AbiEncoding.Encoders;
using SolCodeGen.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolCodeGen
{
    public partial class ExampleGeneratedContract : BaseContract
    {
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
            (string _name, bool _enableThing, UInt256 _last) args,
            SendParams sendParams,
            Address defaultFromAccount)
        {
            var encodedParams = EncoderUtil.GetBytes(
                EncoderFactory.LoadEncoder("string", args._name),
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

        public EthFunc<string> givenName(
            SendParams sendParams = null, CallType callType = CallType.Transaction)
        {
            var callData = GetCallData("givenName()");

            return EthFunc.Create<string>(this, callData, "string", DecoderFactory.Decode);
        }

        public EthFunc<string> echoString(string str)
        {
            var callData = GetCallData("echoString(string)",
                EncoderFactory.LoadEncoder("string", str));

            return EthFunc.Create<string>(this, callData, "string", DecoderFactory.Decode);
        }

        public EthFunc<(Address addr, UInt256 num, string str)> echoMany(Address addr, UInt256 num, string str)
        {
            var callData = GetCallData("echoMany(address,uint256,string)",
                EncoderFactory.LoadEncoder("address", addr),
                EncoderFactory.LoadEncoder("uint256", num),
                EncoderFactory.LoadEncoder("string", str));

            return EthFunc.Create<Address, UInt256, string>(this, callData, 
                "address", DecoderFactory.Decode, 
                "uint256", DecoderFactory.Decode, 
                "string", DecoderFactory.Decode);
        }

        public EthFunc<short[]> getArrayStatic()
        {
            var callData = GetCallData("getArrayStatic()");

            return EthFunc.Create<short[]>(this, callData, 
                "int16[4]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("int16", default(short))));
        }

        public EthFunc<short[]> getArrayDynamic()
        {
            var callData = GetCallData("getArrayDynamic()");

            return EthFunc.Create<short[]>(this, callData,
                "int16[]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("int16", default(short))));
        }

        public EthFunc<uint[]> echoArrayStatic(uint[] input)
        {
            var callData = GetCallData("echoArrayStatic(uint24[5])",
                EncoderFactory.LoadEncoder("uint24[5]", input, EncoderFactory.LoadEncoder("uint24", default(uint))));

            return EthFunc.Create<uint[]>(this, callData,
                "uint24[5]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("uint24", default(uint))));
        }

        public EthFunc<uint[]> echoArrayDynamic(uint[] input)
        {
            var callData = GetCallData("echoArrayDynamic(uint24[])",
                EncoderFactory.LoadEncoder("uint24[]", input, EncoderFactory.LoadEncoder("uint24", default(uint))));

            return EthFunc.Create<uint[]>(this, callData,
                "uint24[]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("uint24", default(uint))));
        }

        public EthFunc<(bool p1, string p2, long p3, Address[] p4, byte p5, ulong[] p6)> boat(
            bool p1, string p2, long p3, Address[] p4, byte p5, ulong[] p6)
        {
            var callData = GetCallData("boat(bool,string,int56,address[],uint8,uint64[3])",
                EncoderFactory.LoadEncoder("bool", p1),
                EncoderFactory.LoadEncoder("string", p2),
                EncoderFactory.LoadEncoder("int56", p3),
                EncoderFactory.LoadEncoder("address[]", p4, EncoderFactory.LoadEncoder("address", default(Address))),
                EncoderFactory.LoadEncoder("uint8", p5),
                EncoderFactory.LoadEncoder("uint64[3]", p6, EncoderFactory.LoadEncoder("uint64", default(ulong))));

            return EthFunc.Create<bool, string, long, Address[], byte, ulong[]>(
                this, callData,
                "bool", DecoderFactory.Decode,
                "string", DecoderFactory.Decode,
                "int56", DecoderFactory.Decode,
                "address[]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("address", default(Address))),
                "uint8", DecoderFactory.Decode,
                "uint64[3]", DecoderFactory.GetArrayDecoder(EncoderFactory.LoadEncoder("uint64", default(ulong))));
        }

        public EthFunc<(uint p1, bool p2, Address p3)> echoMultipleStatic(uint r1, bool r2, Address r3)
        {
            var callData = GetCallData("echoMultipleStatic(uint32,bool,address)",
                EncoderFactory.LoadEncoder("uint32", r1),
                EncoderFactory.LoadEncoder("bool", r2),
                EncoderFactory.LoadEncoder("address", r3));

            return EthFunc.Create<uint, bool, Address>(
                this, callData,
                "uint32", DecoderFactory.Decode,
                "bool", DecoderFactory.Decode,
                "address", DecoderFactory.Decode);
        }

        public EthFunc<(string p1, string p2, string p3)> echoMultipleDynamic(string r1, string r2, string r3)
        {
            var callData = GetCallData("echoMultipleDynamic(string,string,string)",
                EncoderFactory.LoadEncoder("string", r1),
                EncoderFactory.LoadEncoder("string", r2),
                EncoderFactory.LoadEncoder("string", r3));
            //callData = "0x14d6b8fa000000000000000000000000000000000000000000000000000000000000006000000000000000000000000000000000000000000000000000000000000000a000000000000000000000000000000000000000000000000000000000000000e0000000000000000000000000000000000000000000000000000000000000000c666972737420737472696e670000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000461736466000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000028757466383b20342062797465733a20f0a0beb43b20332062797465733a20e28fb020776f726b7321000000000000000000000000000000000000000000000000";
            return EthFunc.Create<string, string, string>(this, callData,
                "string", DecoderFactory.Decode,
                "string", DecoderFactory.Decode,
                "string", DecoderFactory.Decode);
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
