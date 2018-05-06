using Newtonsoft.Json;
using SolcNet.DataDescription.Parsing;
using System.Collections.Generic;

namespace SolcNet.DataDescription.Output
{
    public class Bytecode
    {
        /// <summary>
        /// The bytecode as a hex string.
        /// </summary>
        [JsonProperty("object"), JsonConverter(typeof(HexStringConverter))]
        public byte[] Object { get; set; }

        /// <summary>
        /// Opcodes list (string)
        /// </summary>
        [JsonProperty("opcodes")]
        public string Opcodes { get; set; }

        /// <summary>
        /// The source mapping as a string. See the source mapping definition.
        /// </summary>
        [JsonProperty("sourceMap"), JsonConverter(typeof(SourceMapParser))]
        public SourceMapEntry[] SourceMap { get; set; }

        /// <summary>
        /// If given, this is an unlinked object.
        /// </summary>
        [JsonProperty("linkReferences")]
        public Dictionary<string /*sol file*/, Dictionary<string/*contract name*/, LinkReference[]>> LinkReferences { get; set; }
    }

    public struct SourceMapEntry
    {
        /*
        Where s is the byte-offset to the start of the range in the source file, 
        l is the length of the source range in bytes and f is the source index 
        mentioned above. The encoding in the source mapping for the bytecode is 
        more complicated: It is a list of s:l:f:j separated by ;. Each of these 
        elements corresponds to an instruction, i.e. you cannot use the byte offset 
        but have to use the instruction offset (push instructions are longer than 
        a single byte). The fields s, l and f are as above and j can be either i, o 
        or - signifying whether a jump instruction goes into a function, returns 
        from a function or is a regular jump as part of e.g. a loop. In order to 
        compress these source mappings especially for bytecode, the following rules 
        are used:
            If a field is empty, the value of the preceding element is used.
            If a : is missing, all following fields are considered empty.
        */

        /// <summary>the byte-offset to the start of the range in the source file</summary>
        public int Offset;
        /// <summary>the length of the source range in bytes</summary>
        public int Length;
        /// <summary>integer indentifier to refer to source file</summary>
        public int Index;
        /// <summary></summary>
        public JumpInstruction Jump;

        public string Raw;
    }

    public enum JumpInstruction : byte
    {
        /// <summary>jump instruction goes into a function</summary>
        Function = (byte)'i',
        /// <summary>returns from a function</summary>
        Return = (byte)'o',
        /// <summary>regular jump as part of e.g. a loop</summary>
        Regular = (byte)'-'
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

}
