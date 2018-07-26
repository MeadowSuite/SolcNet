using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{
    public delegate void ReadFileCallback(string path, ref string contents, ref string error);

    public interface INativeSolcLib : IDisposable
    {
        string GetLicense();
        string GetVersion();
        string Compile(string input, ReadFileCallback readCallback);
        [Obsolete]
        string CompileLegacyJson(string input, bool optimize);
        [Obsolete]
        string CompileLegacyJsonMutli(string input, bool optimize);
        [Obsolete]
        string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback);

        string NativeLibFilePath { get; }

    }
}
