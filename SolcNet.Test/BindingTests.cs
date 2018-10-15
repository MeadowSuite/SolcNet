using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolcNet.DataDescription.Input;
using SolcNet.NativeLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SolcNet.Test
{

    public class NativeLibProvider : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            yield return new object[] { new InteropLibProvider<SolcLibDefaultProvider>() };
            yield return new object[] { new InteropLibProvider<NativeLibraryLoader.SolcLibDynamicProvider>() };
            yield return new object[] { new InteropLibProvider<AdvDL.SolcLibAdvDLProvider>() };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
            {
                return $"{methodInfo.Name} ({string.Join(",", data)})";
            }
            return null;
        }
    }

    public interface IInteropLibProvider
    {
        INativeSolcLib InteropLib { get; }
    }

    public class InteropLibProvider<T> : IInteropLibProvider where T : INativeSolcLib, new()
    {
        public INativeSolcLib InteropLib => new T();
    }

    [TestClass]
    public class BindingTests
    {
        const string CONTRACT_SRC_DIR = "OpenZeppelin";

        public BindingTests()
        {

        }

        [DataTestMethod]
        [NativeLibProvider]
        public void NativeLibPathTests(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var nativeLibPath = solcLib.NativeLibFilePath;
            Assert.IsTrue(!string.IsNullOrEmpty(nativeLibPath));
            Assert.IsTrue(File.Exists(nativeLibPath));
        }

        [DataTestMethod]
        [NativeLibProvider]
        public void VersionTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var version = solcLib.Version;
            Assert.AreEqual(Version.Parse("0.4.25"), version);
        }

        [DataTestMethod]
        [NativeLibProvider]
        public void IsReleaseVersion(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var version = solcLib.VersionDescription;

            // Prerelease/develop compiled libs have a version that looks like: 0.4.24-develop.2018.7.5+commit.346b21c8
            // Release compiled libs have a version that looks like: 0.4.24+commit.8025b766
            if (version.Contains("develop") || version.Contains("night"))
            {
                Assert.Fail("Lib version is not develop / nightly: " + version);
            }
        }

        [DataTestMethod]
        [NativeLibProvider]
        public void LicenseTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var license = solcLib.License;
            StringAssert.StartsWith(license, "Most of the code is licensed under GPLv3");
        }

        [DataTestMethod]
        [NativeLibProvider]
        public void CompileCallbackTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            CompileSeveralContracts(solcLib);
        }

        [DataTestMethod]
        [NativeLibProvider]
        public void CompileInMemorySources(IInteropLibProvider nativeSolcLib)
        {
            var contract1Source = @"
                pragma solidity ^0.4.24;
                import ""./interfaces/IHelloWorld.sol"";
                contract HelloWorld {
                    event HelloEvent(string _message, address _sender);
                    function renderHelloWorld () public returns (string) {
                        emit HelloEvent(""Hello world"", msg.sender);
                        return ""Hello world"";
                    }
                }";

            var contract2Source = @"
                pragma solidity ^0.4.24;
                contract IHelloWorld {
                    event HelloEvent(string _message, address _sender);
                    function renderHelloWorld () public returns (string);
                }";

            var solcLib = new SolcLib(nativeSolcLib.InteropLib);
            var result = solcLib.Compile(new InputDescription
            {
                Sources = 
                {
                    ["HelloWorld.sol"] = new Source { Content = contract1Source },
                    ["interfaces/IHelloWorld.sol"] = new Source { Content = contract2Source }
                }
            });
        }


        static void CompileSeveralContracts(SolcLib solcLib)
        {
            var srcs = new[] {
                "contracts/crowdsale/validation/WhitelistedCrowdsale.sol",
                "contracts/token/ERC20/StandardBurnableToken.sol"
            };

            solcLib.Compile(srcs);
        }

    }
}
