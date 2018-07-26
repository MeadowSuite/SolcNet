using NativeLibraryLoader;
using SolcNet.NativeLib;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLibraryLoader
{

    public class SolcLibDynamicProvider : INativeSolcLib
    {
        public const string LIB_FILE = "solc";

        public string NativeLibFilePath { get; private set; }

        NativeLibrary _native;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeReadFileCallback(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string contents,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string error);

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string CompileStandardDelegate(string input, NativeReadFileCallback readCallback);
        Lazy<CompileStandardDelegate> _compileStandard;

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string CompileJsonDelegate(string input, bool optimize);
        Lazy<CompileJsonDelegate> _compileJson;

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string CompileJsonMultiDelegate(string input, bool optimize);
        Lazy<CompileJsonMultiDelegate> _compileJsonMulti;

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string CompileJsonCallbackDelegate(string input, bool optimize, NativeReadFileCallback readCallback);
        Lazy<CompileJsonCallbackDelegate> _compileJsonCallback;

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string LicenseDelegate();
        Lazy<LicenseDelegate> _license;

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        delegate string VersionDelegate();
        Lazy<VersionDelegate> _version;

        Lazy<TDelegate> LoadFunction<TDelegate>(string name)
        {
            return new Lazy<TDelegate>(() => _native.LoadFunction<TDelegate>(name));
        }

        public SolcLibDynamicProvider()
        {
            NativeLibFilePath = LibPathResolver.Resolve(LIB_FILE);
            var libPathResolver = new CustomResolver(NativeLibFilePath);
            _native = new NativeLibrary(LIB_FILE, LibraryLoader.GetPlatformDefaultLoader(), libPathResolver);

            _compileStandard = LoadFunction<CompileStandardDelegate>("compileStandard");
            _compileJson = LoadFunction<CompileJsonDelegate>("compileJSON");
            _compileJsonMulti = LoadFunction<CompileJsonMultiDelegate>("compileJSONMulti");
            _compileJsonCallback = LoadFunction<CompileJsonCallbackDelegate>("compileJSONCallback");
            _license = LoadFunction<LicenseDelegate>("license");
            _version = LoadFunction<VersionDelegate>("version");
        }

        public string GetLicense() => _license.Value();

        public string GetVersion() => _version.Value();

        public string Compile(string input, ReadFileCallback readCallback)
        {
            return _compileStandard.Value(input, new NativeReadFileCallback(readCallback));
        }

        public string CompileLegacyJson(string input, bool optimize)
        {
            return _compileJson.Value(input, optimize);
        }

        public string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback)
        {
            return _compileJsonCallback.Value(input, optimize, new NativeReadFileCallback(readCallback));
        }

        public string CompileLegacyJsonMutli(string input, bool optimize)
        {
            return _compileJsonMulti.Value(input, optimize);
        }

        public void Dispose()
        {
            _native.Dispose();
        }
    }
}
