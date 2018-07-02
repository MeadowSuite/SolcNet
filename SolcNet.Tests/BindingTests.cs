using SolcNet.NativeLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SolcNet.Tests
{
    public class NativeLibProvider : TheoryData<IInteropLibProvider>
    {
        public NativeLibProvider()
        {
            Add(new InteropLibProvider<SolcLibDefaultProvider>());
            Add(new InteropLibProvider<NativeLibraryLoader.SolcLibDynamicProvider>());
            Add(new InteropLibProvider<AdvDL.SolcLibAdvDLProvider>());
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

    public class BindingTests
    {
        const string CONTRACT_SRC_DIR = "OpenZeppelin";

        public BindingTests()
        {

        }

        [Theory]
        [ClassData(typeof(NativeLibProvider))]
        public void VersionTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var version = solcLib.Version;
            Assert.Equal(Version.Parse("0.4.24"), version);
        }

        [Theory]
        [ClassData(typeof(NativeLibProvider))]
        public void LicenseTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            var license = solcLib.License;
            Assert.StartsWith("Most of the code is licensed under GPLv3", license);
        }

        [Theory]
        [ClassData(typeof(NativeLibProvider))]
        public void CompileCallbackTest(IInteropLibProvider nativeSolcLib)
        {
            var solcLib = new SolcLib(nativeSolcLib.InteropLib, CONTRACT_SRC_DIR);
            CompileSeveralContracts(solcLib);
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
