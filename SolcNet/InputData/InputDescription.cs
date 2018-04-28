using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace SolcNet.InputData
{
    public class InputDescription
    {
        public static InputDescription FromJsonString(string jsonStr)
        {
            return JsonConvert.DeserializeObject<InputDescription>(jsonStr, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
        }

        public override string ToString() => ToJsonString();

        /// <summary>
        /// Required: Source code language, such as "Solidity", "serpent", "lll", "assembly", etc.
        /// </summary>
        [JsonProperty("language", Required = Required.DisallowNull)]
        public Language Language { get; set; } = Language.Solidity;

        /// <summary>
        /// The keys here are the "global" names of the source files
        /// </summary>
        [JsonProperty("sources", Required = Required.DisallowNull)]
        public Dictionary<string, Source> Sources { get; set; } = new Dictionary<string, Source>();

        /// <summary>
        /// Optional
        /// </summary>
        [JsonProperty("settings", NullValueHandling = NullValueHandling.Ignore)]
        public Settings Settings { get; set; } = new Settings();
    }

    /// <summary>
    /// Source code language
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Language
    {
        [EnumMember(Value = "Solidity")] Solidity,
        [EnumMember(Value = "serpent")] Serpent,
        [EnumMember(Value = "lll")] Lll,
        [EnumMember(Value = "assembly")] Assembly
    }

    public class Source
    {
        /// <summary>
        /// Optional: keccak256 hash of the source file. It is used to verify the source.
        /// </summary>
        [JsonProperty("keccak256", NullValueHandling = NullValueHandling.Ignore)]
        public string Keccak256 { get; set; }

        /// <summary>
        /// Required (unless "urls" is used): literal contents of the source file
        /// </summary>
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content
        {
            get => _content;
            set => _content = value;
        }
        string _content;

        /// <summary>
        /// Required (unless "content" is used, see below): URL(s) to the source file.
        /// URL(s) should be imported in this order and the result checked against the
        /// keccak256 hash (if available). If the hash doesn't match or none of the
        /// URL(s) result in success, an error should be raised.
        /// Examples:
        ///     "bzzr://56ab...",
        ///     "ipfs://Qma...",
        ///     "file:///tmp/path/to/file.sol"
        /// </summary>
        [JsonProperty("urls", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Urls { get; set; }

        public Source() { }

        public Source(params string[] urls)
        {
            Urls = new List<string>(urls);
        }
    }


    public class Settings
    {
        /// <summary>
        /// Optional: Sorted list of remappings
        /// </summary>
        [JsonProperty("remappings", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Remappings { get; set; } = new List<string>();

        /// <summary>
        /// Optional: Optimizer settings (enabled defaults to false)
        /// </summary>
        [JsonProperty("optimizer", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Optimizer Optimizer { get; set; } = new Optimizer();

        /// <summary>
        /// Version of the EVM to compile for. Affects type checking and code generation.
        /// </summary>
        [JsonProperty("evmVersion", Required = Required.DisallowNull)]
        public EvmVersion EvmVersion { get; set; } = EvmVersion.Byzantium;

        /// <summary>
        /// Metadata settings (optional)
        /// </summary>
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Metadata Metadata { get; set; } = new Metadata();

        /// <summary>
        /// Addresses of the libraries. If not all libraries are given here, it can result in unlinked objects whose output data is different.
        /// The top level key is the the name of the source file where the library is used.
        /// If remappings are used, this source file should match the global path after remappings were applied.
        /// If this key is an empty string, that refers to a global level.
        /// </summary>
        [JsonProperty("libraries", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, string>> Libraries { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// The following can be used to select desired outputs.
        /// If this field is omitted, then the compiler loads and does type checking, but will not generate any outputs apart from errors.
        /// The first level key is the file name and the second is the contract name, where empty contract name refers to the file itself,
        /// while the star refers to all of the contracts.
        /// Note that using a using `evm`, `evm.bytecode`, `ewasm`, etc. will select every
        /// target part of that output. Additionally, `*` can be used as a wildcard to request everything.
        /// </summary>
        [JsonProperty("outputSelection", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, List<OutputType>>> OutputSelection { get; set; } = new Dictionary<string, Dictionary<string, List<OutputType>>>();
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OutputType
    {
        /// <summary>ABI</summary>
        [EnumMember(Value = "abi")] Abi,
        /// <summary>AST of all source files</summary>
        [EnumMember(Value = "ast")] Ast,
        /// <summary>legacy AST of all source files</summary>
        [EnumMember(Value = "legacyAST")] LegacyAst,
        /// <summary>Developer documentation (natspec)</summary>
        [EnumMember(Value = "devdoc")] DevDoc,
        /// <summary>User documentation (natspec)</summary>
        [EnumMember(Value = "userdoc")] UserDoc,
        /// <summary>Metadata</summary>
        [EnumMember(Value = "metadata")] Metadata,
        /// <summary>New assembly format before desugaring</summary>
        [EnumMember(Value = "ir")] IR,
        /// <summary>All evm related targets</summary>
        [EnumMember(Value = "evm")] Evm,
        /// <summary>New assembly format after desugaring</summary>
        [EnumMember(Value = "evm.assembly")] EvmAssembly,
        /// <summary>Old-style assembly format in JSON</summary>
        [EnumMember(Value = "evm.legacyAssembly")] EvmLegacyAssembly,
        /// <summary>All bytecode related targets</summary>
        [EnumMember(Value = "evm.bytecode")] EvmBytecode,
        /// <summary>Bytecode object</summary>
        [EnumMember(Value = "evm.bytecode.object")] EvmBytecodeObject,
        /// <summary>Opcodes list</summary>
        [EnumMember(Value = "evm.bytecode.opcodes")] EvmBytecodeOpcodes,
        /// <summary>Source mapping (useful for debugging)</summary>
        [EnumMember(Value = "evm.bytecode.sourceMap")] EvmBytecodeSourceMap,
        /// <summary>Link references (if unlinked object)</summary>
        [EnumMember(Value = "evm.bytecode.linkReferences")] EvmBytecodeLinkReferences,
        /// <summary>Deployed bytecode (has the same options as evm.bytecode)</summary>
        [EnumMember(Value = "evm.deployedBytecode*")] EvmDeployedBytecode,
        /// <summary>The list of function hashes</summary>
        [EnumMember(Value = "evm.methodIdentifiers")] EvmMethodIdentifiers,
        /// <summary>Function gas estimates</summary>
        [EnumMember(Value = "evm.gasEstimates")] EvmGasEstimates,
        /// <summary>All eWASM related targets</summary>
        [EnumMember(Value = "ewasm")] Ewasm,
        /// <summary>eWASM S-expressions format (not supported atm)</summary>
        [EnumMember(Value = "ewasm.wast")] EwasmWast,
        /// <summary>eWASM binary format (not supported atm)</summary>
        [EnumMember(Value = "ewasm.wasm")] EwasmWasm
    }

    /// <summary>
    /// Version of the EVM to compile for. Affects type checking and code generation.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EvmVersion
    {
        [EnumMember(Value = "homestead")] Homestead,
        [EnumMember(Value = "tangerineWhistle")] TangerineWhistle,
        [EnumMember(Value = "spuriousDragon")] SpuriousDragon,
        [EnumMember(Value = "byzantium")] Byzantium,
        [EnumMember(Value = "constantinople")] Constantinople
    }

    /// <summary>
    /// Optional: Optimizer settings (enabled defaults to false)
    /// </summary>
    public class Optimizer
    {
        [JsonProperty("enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Enabled { get; set; }

        [JsonProperty("runs", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? Runs { get; set; }
    }

    /// <summary>
    /// Metadata settings (optional)
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Use only literal content and not URLs (false by default)
        /// </summary>
        [JsonProperty("useLiteralContent", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? UseLiteralContent { get; set; }
    }



}
