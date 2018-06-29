using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    public class SolcLibPInvokeProvider : INativeSolcLib
    {
        public const string LIB_FILE = "solc";

        [DllImport("Kernel32.dll")]
        static extern IntPtr LoadLibrary(string path);

        [DllImport("libdl")]
        static extern IntPtr dlopen(string path, int flags);

        [DllImport("libdl")]
        static extern IntPtr dlerror();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void NativeReadFileCallback(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            string path,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string contents,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshaler))]
            ref string error);

        [DllImport(LIB_FILE, EntryPoint = "license", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeLicense();

        [DllImport(LIB_FILE, EntryPoint = "version", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeVersion();

        [DllImport(LIB_FILE, EntryPoint = "compileStandard", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeCompileStandard(string input, NativeReadFileCallback readCallback);

        [DllImport(LIB_FILE, EntryPoint = "compileJSON", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeCompileLegacyJson(string input, bool optimize);

        [DllImport(LIB_FILE, EntryPoint = "compileJSONMulti", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeCompileLegacyJsonMutli(string input, bool optimize);

        [DllImport(LIB_FILE, EntryPoint = "compileJSONCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8StringMarshalerNoCleanup))]
        static extern string NativeCompileLegacyJson(string input, bool optimize, NativeReadFileCallback readCallback);


        static SolcLibPInvokeProvider()
        {
            var libPath = LibPathResolver.Resolve(LIB_FILE);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var loadResult = LoadLibrary(libPath);
                if (loadResult == IntPtr.Zero)
                {
                    throw new Exception($"Library loading failed, file: {libPath}", new Win32Exception(Marshal.GetLastWin32Error()));
                }
            }
            else
            {
                const int RTLD_NOW = 0x002;
                var loadResult = dlopen(libPath, RTLD_NOW);
                if (loadResult == IntPtr.Zero)
                {
                    var errorPtr = dlerror();
                    if (errorPtr == IntPtr.Zero)
                    {
                        throw new Exception($"Library could not be loaded, and error information from dl library could not be found, file: {libPath}");
                    }
                    throw new Exception($"Library could not be loaded: {Marshal.PtrToStringAnsi(errorPtr)}, file: {libPath}");
                }
            }
        }

        public string GetLicense() => NativeLicense();
        public string GetVersion() => NativeVersion();

        public string Compile(string input, ReadFileCallback readCallback)
        {
            return NativeCompileStandard(input, new NativeReadFileCallback(readCallback));
        }

        public string CompileLegacyJson(string input, bool optimize)
        {
            return NativeCompileLegacyJson(input, optimize);
        }

        public string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback)
        {
            return NativeCompileLegacyJson(input, optimize, new NativeReadFileCallback(readCallback));
        }

        public string CompileLegacyJsonMutli(string input, bool optimize)
        {
            return NativeCompileLegacyJsonMutli(input, optimize);
        }

    }
}
