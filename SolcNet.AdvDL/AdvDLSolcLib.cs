using AdvancedDLSupport;
using SolcNet.NativeLib;
using System;
using System.IO;

namespace SolcNet.AdvDL
{
    public class SolcLibAdvDLProvider : INativeSolcLib
    {
        public const string LIB_FILE = "solc";
        IAdvDLSolcLib _native;

        public string NativeLibFilePath { get; private set; }

        public SolcLibAdvDLProvider()
        {
            var config = ImplementationOptions.UseLazyBinding;
            NativeLibFilePath = LibPathResolver.Resolve(LIB_FILE);
            var builder = new NativeLibraryBuilder(config);
            _native = builder.ActivateInterface<IAdvDLSolcLib>(NativeLibFilePath);
        }

        public string Compile(string input, ReadFileCallback readCallback)
        {
            return _native.compileStandard(input, new AvdDLReadFileCallback(readCallback));
        }

        public string CompileLegacyJson(string input, bool optimize)
        {
            return _native.compileJSON(input, optimize);
        }

        public string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback)
        {
            return _native.compileJSONCallback(input, optimize, new AvdDLReadFileCallback(readCallback));
        }

        public string CompileLegacyJsonMutli(string input, bool optimize)
        {
            return _native.compileJSONMulti(input, optimize);
        }

        public string GetLicense()
        {
            return _native.license();
        }

        public string GetVersion()
        {
            return _native.version();
        }

        public void Dispose()
        {
            if (_native is NativeLibraryBase lib)
            {
                lib.Dispose();
            }
        }
    }
}
