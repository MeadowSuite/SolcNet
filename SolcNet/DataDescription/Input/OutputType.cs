using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq;

namespace SolcNet.DataDescription.Input
{
    [JsonConverter(typeof(NamedStringTokenConverter<OutputType>))]
    public class OutputType : INamedStringToken
    {
        public string Value { get; set; }

        public static implicit operator OutputType(string value) => new OutputType { Value = value };
        public static implicit operator string(OutputType o) => o.Value;
        public override string ToString() => Value;

        public static OutputType[] All => _all.Value;
        static Lazy<OutputType[]> _all = new Lazy<OutputType[]>(() =>
        {
            var props = typeof(OutputType).GetFields(BindingFlags.Public | BindingFlags.Static);
            var vals = props.Select(p => p.GetValue(null)).OfType<OutputType>();
            return vals.ToArray();
        });

        /// <summary>ABI</summary>
        public static readonly OutputType Abi = "abi";

        /// <summary>AST of all source files</summary>
        public static readonly OutputType Ast = "ast";

        /// <summary>legacy AST of all source files</summary>
        public static readonly OutputType LegacyAst = "legacyAST";

        /// <summary>Developer documentation (natspec)</summary>
        public static readonly OutputType DevDoc = "devdoc";

        /// <summary>User documentation (natspec)</summary>
        public static readonly OutputType UserDoc = "userdoc";

        /// <summary>Metadata</summary>
        public static readonly OutputType Metadata = "metadata";

        /// <summary>New assembly format before desugaring</summary>
        public static readonly OutputType IR = "ir";

        /// <summary>All evm related targets</summary>
        public static readonly OutputType Evm = "evm";

        /// <summary>New assembly format after desugaring</summary>
        public static readonly OutputType EvmAssembly = "evm.assembly";

        /// <summary>Old-style assembly format in JSON</summary>
        public static readonly OutputType EvmLegacyAssembly = "evm.legacyAssembly";

        /// <summary>All bytecode related targets</summary>
        public static readonly OutputType EvmBytecode = "evm.bytecode";

        /// <summary>Bytecode object</summary>
        public static readonly OutputType EvmBytecodeObject = "evm.bytecode.object";

        /// <summary>Opcodes list</summary>
        public static readonly OutputType EvmBytecodeOpcodes = "evm.bytecode.opcodes";

        /// <summary>Source mapping (useful for debugging)</summary>
        public static readonly OutputType EvmBytecodeSourceMap = "evm.bytecode.sourceMap";

        /// <summary>Link references (if unlinked object)</summary>
        public static readonly OutputType EvmBytecodeLinkReferences = "evm.bytecode.linkReferences";

        /// <summary>Deployed bytecode (has the same options as evm.bytecode)</summary>
        public static readonly OutputType EvmDeployedBytecode = "evm.deployedBytecode*";

        /// <summary>The list of function hashes</summary>
        public static readonly OutputType EvmMethodIdentifiers = "evm.methodIdentifiers";

        /// <summary>Function gas estimates</summary>
        public static readonly OutputType EvmGasEstimates = "evm.gasEstimates";

        /// <summary>All eWASM related targets</summary>
        public static readonly OutputType Ewasm = "ewasm";

        /// <summary>eWASM S-expressions format (not supported atm)</summary>
        public static readonly OutputType EwasmWast = "ewasm.wast";

        /// <summary>eWASM binary format (not supported atm)</summary>
        public static readonly OutputType EwasmWasm = "ewasm.wasm";
    }



}
