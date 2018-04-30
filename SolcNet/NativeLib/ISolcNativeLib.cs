using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    public delegate void ReadFileCallback(
        [MarshalAs(UnmanagedType.LPStr)] string path,
        [MarshalAs(UnmanagedType.LPStr)] ref string contents,
        [MarshalAs(UnmanagedType.LPStr)] ref string error);


    public interface ISolcNativeLib
    {
        [return: MarshalAs(UnmanagedType.LPStr)]
        string license();

        [return: MarshalAs(UnmanagedType.LPStr)]
        string version();

        [Obsolete]
        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSON(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimize);

        [Obsolete]
        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSONMulti(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimze);

        [Obsolete]
        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSONCallback(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimize,
            ReadFileCallback readCallback);

        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileStandard(
            [MarshalAs(UnmanagedType.LPStr)] string input,
            ReadFileCallback readCallback);
    }
}
