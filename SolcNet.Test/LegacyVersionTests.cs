using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolcNet.NativeLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SolcNet.Test
{

    public class LegacyLibVersionsAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[] { Legacy.SolcVersion.v0_4_20 };
            yield return new object[] { Legacy.SolcVersion.v0_4_21 };
            yield return new object[] { Legacy.SolcVersion.v0_4_22 };
            yield return new object[] { Legacy.SolcVersion.v0_4_23 };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo.Name} ({data[0].ToString()})";
        }
    }

    [TestClass]
    public class LegacyVersionTests
    {
        [DataTestMethod]
        [LegacyLibVersions]
        public void ReportedVersion(Legacy.SolcVersion solcVersion)
        {
            var libPath = Legacy.LibPath.GetLibPath(solcVersion);
            var libProvider = new SolcLibDefaultProvider(libPath);
            var solc = new SolcLib(libProvider);
            var ver = solc.Version;
            var verString = solcVersion.ToString().Replace('_', '.').TrimStart('v');
            Assert.AreEqual(verString, ver.ToString());
        }

        [DataTestMethod]
        [LegacyLibVersions]
        public void IsReleaseVersion(Legacy.SolcVersion solcVersion)
        {
            var libPath = Legacy.LibPath.GetLibPath(solcVersion);
            var libProvider = new SolcLibDefaultProvider(libPath);
            var solcLib = new SolcLib(libProvider);
            var version = solcLib.VersionDescription;

            // Prerelease/develop compiled libs have a version that looks like: 0.4.24-develop.2018.7.5+commit.346b21c8
            // Release compiled libs have a version that looks like: 0.4.24+commit.8025b766
            if (version.Contains("develop") || version.Contains("night"))
            {
                Assert.Fail("Lib version is not develop / nightly: " + version);
            }
        }

        [DataTestMethod]
        [LegacyLibVersions]
        public void Compile(Legacy.SolcVersion solcVersion)
        {
            const string CONTRACT_SRC_DIR = "LegacyContracts";
            var libPath = Legacy.LibPath.GetLibPath(solcVersion);
            var libProvider = new SolcLibDefaultProvider(libPath);
            var solcLib = new SolcLib(libProvider, CONTRACT_SRC_DIR);
            var srcs = new[] {
                "token/ERC20/StandardToken.sol"
            };
            solcLib.Compile(srcs);
        }

    }
}
