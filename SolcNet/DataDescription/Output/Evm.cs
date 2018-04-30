using Newtonsoft.Json;
using System.Collections.Generic;

namespace SolcNet.DataDescription.Output
{
    public class Evm
    {
        /// <summary>
        /// Assembly (string)
        /// </summary>
        [JsonProperty("assembly")]
        public string Assembly { get; set; }

        /// <summary>
        /// Old-style assembly (object)
        /// </summary>
        [JsonProperty("legacyAssembly")]
        public LegacyAssembly LegacyAssembly { get; set; }

        /// <summary>
        /// Bytecode and related details.
        /// </summary>
        [JsonProperty("bytecode")]
        public Bytecode Bytecode { get; set; }

        [JsonProperty("deployedBytecode")]
        public Bytecode DeployedBytecode { get; set; }

        /// <summary>
        /// The list of function hashes
        /// </summary>
        [JsonProperty("methodIdentifiers")]
        public Dictionary<string/*function definition*/, string/*function hash*/> MethodIdentifiers { get; set; }

        /// <summary>
        /// Function gas estimates
        /// </summary>
        [JsonProperty("gasEstimates")]
        public GasEstimates GasEstimates { get; set; }
    }

    public class LegacyAssembly
    {
        [JsonProperty(".code")]
        public IList<Code> Code { get; set; }

        [JsonProperty(".data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("0")]
        public Zero Zero { get; set; }
    }

    public class Zero
    {
        [JsonProperty(".auxdata")]
        public string Auxdata { get; set; }

        [JsonProperty(".code")]
        public IList<Code> Code { get; set; }
    }

    public class Code
    {
        [JsonProperty("begin")]
        public int Begin { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Bytecode
    {
        /// <summary>
        /// The bytecode as a hex string.
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Opcodes list (string)
        /// </summary>
        [JsonProperty("opcodes")]
        public string Opcodes { get; set; }

        /// <summary>
        /// The source mapping as a string. See the source mapping definition.
        /// </summary>
        [JsonProperty("sourceMap")]
        public string SourceMap { get; set; }

        /// <summary>
        /// If given, this is an unlinked object.
        /// </summary>
        [JsonProperty("linkReferences")]
        public Dictionary<string /*sol file*/, Dictionary<string/*contract name*/, List<LinkReference>>> LinkReferences { get; set; }
    }

    public class LinkReference
    {
        /// <summary>
        /// Byte offsets into the bytecode. Linking replaces the 20 bytes located there.
        /// </summary>
        [JsonProperty("start", Required = Required.Always)]
        public uint Start { get; set; }

        [JsonProperty("length", Required = Required.Always)]
        public uint Length { get; set; }
    }

    public class GasEstimates
    {
        [JsonProperty("creation")]
        public Creation Creation { get; set; }

        [JsonProperty("external")]
        public Dictionary<string/*function def*/, string> External { get; set; }

        [JsonProperty("internal")]
        public Dictionary<string/*function def*/, string> Internal { get; set; }
    }

    public class Creation
    {
        [JsonProperty("codeDepositCost")]
        public string CodeDepositCost { get; set; }

        [JsonProperty("executionCost")]
        public string ExecutionCost { get; set; }

        [JsonProperty("totalCost")]
        public string TotalCost { get; set; }
    }

}
