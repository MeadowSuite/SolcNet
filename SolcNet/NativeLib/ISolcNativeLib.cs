using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet.NativeLib
{

    public interface ISolcNativeLib
    {
        string license();
        string version();
        string compileJSON(string input, bool optimize);
        string compileJSONMulti(string input, bool optimze);
        string compileJSONCallback(string input, bool optimize, IntPtr readCallback);
        string compileStandard(string input, bool optimize, IntPtr readCallback);
    }
}
