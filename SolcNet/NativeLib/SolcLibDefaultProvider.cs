using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    public class SolcLibDefaultProvider : INativeSolcLib
    {
        public const string LIB_FILE = "solc";


        readonly Lazy<CompileStandardDelegate> _compileStandard;
        readonly Lazy<CompileJsonDelegate> _compileJson;
        readonly Lazy<CompileJsonMultiDelegate> _compileJsonMulti;
        readonly Lazy<CompileJsonCallbackDelegate> _compileJsonCallback;
        readonly Lazy<LicenseDelegate> _license;
        readonly Lazy<VersionDelegate> _version;

        public string NativeLibFilePath => LibPath;
        public readonly string LibPath;
        IntPtr _libHandle;

        public SolcLibDefaultProvider(string libPath)
        {
            LibPath = libPath;
            _libHandle = PlatformNativeLibInterop.LoadLib(LibPath);

            _compileStandard = LazyLoad<CompileStandardDelegate>("compileStandard");
            _compileJson = LazyLoad<CompileJsonDelegate>("compileJSON");
            _compileJsonMulti = LazyLoad<CompileJsonMultiDelegate>("compileJSONMulti");
            _compileJsonCallback = LazyLoad<CompileJsonCallbackDelegate>("compileJSONCallback");
            _license = LazyLoad<LicenseDelegate>("license");
            _version = LazyLoad<VersionDelegate>("version");
        }

        public SolcLibDefaultProvider() : this(LibPathResolver.Resolve(LIB_FILE))
        {
        }

        Lazy<TDelegate> LazyLoad<TDelegate>(string symbol)
        {
            return new Lazy<TDelegate>(() => LoadFunction<TDelegate>(symbol), isThreadSafe: false);
        }

        TDelegate LoadFunction<TDelegate>(string symbol)
        {
            return PlatformNativeLibInterop.GetDelegate<TDelegate>(_libHandle, symbol);
        }

        public string GetLicense() => _license.Value();
        public string GetVersion() => _version.Value();

        public string Compile(string input, ReadFileCallback readCallback)
        {
            return _compileStandard.Value(input, new NativeReadFileCallbackDelegate(readCallback));
        }

        public string CompileLegacyJson(string input, bool optimize)
        {
            return _compileJson.Value(input, optimize);
        }

        public string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback)
        {
            return _compileJsonCallback.Value(input, optimize, new NativeReadFileCallbackDelegate(readCallback));
        }

        public string CompileLegacyJsonMutli(string input, bool optimize)
        {
            return _compileJsonMulti.Value(input, optimize);
        }

        public void Dispose()
        {
            if (_libHandle != IntPtr.Zero)
            {
                PlatformNativeLibInterop.FreeLibrary(_libHandle);
            }
            _libHandle = IntPtr.Zero;
        }
    }
}
