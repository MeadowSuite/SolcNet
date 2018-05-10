using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolCodeGen.JsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SolCodeGen.Tests
{
    public class RpcSerialization
    {
        [Fact]
        public void BlockObject()
        {
            var blockJson = File.ReadAllText("TestData/rpc_block_object.json");
            var block = JsonConvert.DeserializeObject<Block>(blockJson, new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error });
            var toJson = JsonConvert.SerializeObject(block, Formatting.Indented);

            var jdp = new JsonDiffPatch();
            var diff = JObject.Parse(jdp.Diff(blockJson, toJson));
            var diffStr = diff.ToString(Formatting.Indented);
            foreach(var item in diff)
            {
                var vals = item.Value.Values();
                var b1 = HexConverter.HexToBytes(vals[0].ToString()).GetHexFromBytes(hexPrefix: true);
                var b2 = HexConverter.HexToBytes(vals[1].ToString()).GetHexFromBytes(hexPrefix: true);
                Assert.Equal(b1, b2);
            }
        }

    }
}
