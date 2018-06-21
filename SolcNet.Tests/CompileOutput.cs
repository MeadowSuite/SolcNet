using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolcNet.DataDescription.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace SolcNet.Tests
{
    public class CompileOutput
    {

        SolcLib _lib;

        public CompileOutput()
        {
            _lib = new SolcLib();
        }

        [Fact]
        public void ExpectedOutput()
        {
            var exampleContract = "TestContracts/ExampleContract.sol";
            var output = _lib.Compile(exampleContract/*, OutputType.Ast | OutputType.LegacyAst*/);
            var originalOutput = JObject.Parse(output.RawJsonOutput).ToString(Formatting.Indented);
            //var expectedOutput = JObject.Parse(File.ReadAllText("TestOutput/ExampleContract.json"));
            var serializedOutput = JsonConvert.SerializeObject(output, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var jdp = new JsonDiffPatch();
            var diffStr = jdp.Diff(originalOutput, serializedOutput);
            if (!string.IsNullOrEmpty(diffStr))
            {
                var diff = JObject.Parse(diffStr).ToString(Formatting.Indented);
            }
        }

        [Fact]
        public void SourceMapParsing()
        {
            var exampleContract = "TestContracts/ExampleContract.sol";
            var output = _lib.Compile(exampleContract, OutputType.EvmBytecodeSourceMap);
            var sourceMaps = output.Contracts.Values.First().Values.First().Evm.Bytecode.SourceMap;
            var parsed = sourceMaps.Entries;
            Assert.True(parsed.Length > 0);
        }

    }
}
