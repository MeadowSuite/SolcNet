using SolcNet.NativeLib;
using System;
using Xunit;

namespace SolcNet.Tests
{
    public class BindingTests
    {
        ISolcNativeLib solcLib;

        public BindingTests()
        {
            solcLib = NativeLibFactory.Create();
        }

        [Fact]
        public void VersionTest()
        {
            var version = solcLib.version();
        }
    }
}
