using System;
using System.Runtime.InteropServices;

namespace SolcNet.NativeLib.DynamicLinking
{
    static class DynamicLinkingWindows
    {
        const string KERNEL32 = "kernel32";

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern IntPtr LoadLibrary(string path);

        [DllImport(KERNEL32, SetLastError = true)]
        public static extern int FreeLibrary(IntPtr module);

        [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr module, string procName);
    }
}
