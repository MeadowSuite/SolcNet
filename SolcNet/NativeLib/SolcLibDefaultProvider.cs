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


        readonly Lazy<CompileDelegate> _compile;
        readonly Lazy<LicenseDelegate> _license;
        readonly Lazy<VersionDelegate> _version;

        public string NativeLibFilePath => LibPath;
        public readonly string LibPath;
        IntPtr _libHandle;

        readonly bool _isOld_API;

        public SolcLibDefaultProvider(string libPath)
        {
            LibPath = libPath;
            _libHandle = PlatformNativeLibInterop.LoadLib(LibPath);

            _isOld_API = !PlatformNativeLibInterop.GetFunctionPointer(_libHandle, "solidity_version", out _);

            if (_isOld_API)
            {
                _compile = LazyLoad<CompileDelegate>("compileStandard");
                _license = LazyLoad<LicenseDelegate>("license");
                _version = LazyLoad<VersionDelegate>("version");
            }
            else
            {
                _compile = LazyLoad<CompileDelegate>("solidity_compile");
                _license = LazyLoad<LicenseDelegate>("solidity_license");
                _version = LazyLoad<VersionDelegate>("solidity_version");
            }
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
            return _compile.Value(input, new NativeReadFileCallbackDelegate(readCallback));
        }

        public void Dispose()
        {
            if (_libHandle != IntPtr.Zero)
            {
                PlatformNativeLibInterop.CloseLibrary(_libHandle);
            }
            _libHandle = IntPtr.Zero;
        }
    }
}
