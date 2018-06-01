using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolcNet.DataDescription.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
            var diff = JObject.Parse(jdp.Diff(originalOutput, serializedOutput)).ToString(Formatting.Indented);
        }

    }
}
