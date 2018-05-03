using NativeLibraryLoader;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    public class SolcLibDynamicProvider : INativeSolcLib
    {
        public const string LIB_FILE = "solc";

        static Lazy<NativeLibrary> _native = new Lazy<NativeLibrary>(() => new NativeLibrary(
            LIB_FILE, 
            LibraryLoader.GetPlatformDefaultLoader(), 
            new CustomResolver())
        );
        
        class CustomResolver : PathResolver
        {
            public override IEnumerable<string> EnumeratePossibleLibraryLoadTargets(string name)
            {
                yield return LibPathResolver.Resolve(name);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeReadFileCallback(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string contents,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string error);

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string CompileStandardDelegate(string input, NativeReadFileCallback readCallback);
        Lazy<CompileStandardDelegate> _compileStandard = LoadFunction<CompileStandardDelegate>("compileStandard");

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string CompileJsonDelegate(string input, bool optimize);
        Lazy<CompileJsonDelegate> _compileJson = LoadFunction<CompileJsonDelegate>("compileJSON");

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string CompileJsonMultiDelegate(string input, bool optimize);
        Lazy<CompileJsonMultiDelegate> _compileJsonMulti = LoadFunction<CompileJsonMultiDelegate>("compileJSONMulti");

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string CompileJsonCallbackDelegate(string input, bool optimize, NativeReadFileCallback readCallback);
        Lazy<CompileJsonCallbackDelegate> _compileJsonCallback = LoadFunction<CompileJsonCallbackDelegate>("compileJSONCallback");

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string LicenseDelegate();
        Lazy<LicenseDelegate> _license = LoadFunction<LicenseDelegate>("license");

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
        delegate string VersionDelegate();
        Lazy<VersionDelegate> _version = LoadFunction<VersionDelegate>("version");

        static Lazy<TDelegate> LoadFunction<TDelegate>(string name)
        {
            return new Lazy<TDelegate>(() => _native.Value.LoadFunction<TDelegate>(name));
        }

        public SolcLibDynamicProvider()
        {

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

    }
}
