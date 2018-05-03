using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.AdvDL
{
    public delegate void AvdDLReadFileCallback(
        [MarshalAs(UnmanagedType.LPStr)] string path,
        [MarshalAs(UnmanagedType.LPStr)] ref string contents,
        [MarshalAs(UnmanagedType.LPStr)] ref string error);


    public interface IAdvDLSolcLib
    {
        [return: MarshalAs(UnmanagedType.LPStr)]
        string license();

        [return: MarshalAs(UnmanagedType.LPStr)]
        string version();

        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSON(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimize);

        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSONMulti(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimze);

        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileJSONCallback(
            [MarshalAs(UnmanagedType.LPStr)] string input, 
            bool optimize,
            AvdDLReadFileCallback readCallback);

        [return: MarshalAs(UnmanagedType.LPStr)]
        string compileStandard(
            [MarshalAs(UnmanagedType.LPStr)] string input,
            AvdDLReadFileCallback readCallback);
    }
}
