using SolcNet.DataDescription.Output;
using SolCodeGen.AbiEncoding;
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

            return new EthFunc<string>(this, callData,
                data => Decode<string>(data, "string", DecoderFactory.Decode));
        }

        public EthFunc<string> echoString(string str)
        {
            var callData = GetCallData("echoString(string)",
                EncoderFactory.LoadEncoder("string", str));

            return new EthFunc<string>(this, callData,
                data => Decode<string>(data, "string", DecoderFactory.Decode));
        }

        public EthFunc<(Address addr, UInt256 num, string str)> echoMany(Address addr, UInt256 num, string str)
        {
            var callData = GetCallData("echoMany(address,uint256,string)",
                EncoderFactory.LoadEncoder("address", addr),
                EncoderFactory.LoadEncoder("uint256", num),
                EncoderFactory.LoadEncoder("string", str));

            return new EthFunc<(Address addr, UInt256 num, string str)>(this, callData,
                data => Decode<Address, UInt256, string>(data, 
                        "address", DecoderFactory.Decode, 
                        "uint256", DecoderFactory.Decode, 
                        "string", DecoderFactory.Decode));
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
