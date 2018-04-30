using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    [Obsolete]
    public static class PInvokeLib
    {
        [DllImport("Kernel32.dll")]
        public static extern IntPtr LoadLibrary(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ReadFileCallback(IntPtr path, IntPtr contents, IntPtr error);

        [DllImport("solc", CallingConvention = CallingConvention.Cdecl)]
        public static extern string compileStandard(string input, bool optimize, ref ReadFileCallback readCallback);

    }
}
