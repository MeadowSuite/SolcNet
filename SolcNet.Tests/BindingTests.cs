using SolcNet.NativeLib;
using System;
using Xunit;

namespace SolcNet.Tests
{
    public class BindingTests
    {
        ISolcLib solcLib;

        public BindingTests()
        {
            solcLib = new SolcLibPInvoke();
        }

        [Fact]
        public void VersionTest()
        {
            var version = solcLib.GetVersion();
            Assert.Equal("0.4.23-develop.2018.4.28+commit.124ca40d.mod.Windows.msvc", version);
        }

        [Fact]
        public void LicenseTest()
        {
            var license = solcLib.GetLicense();
            Assert.StartsWith("Most of the code is licensed under GPLv3", license);
        }
    }
}
