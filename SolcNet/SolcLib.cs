using Newtonsoft.Json;
using SolcNet.InputData;
using SolcNet.NativeLib;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static SolcNet.NativeLib.PInvokeLib;

namespace SolcNet
{
    public class SolcLib
    {
        protected ISolcNativeLib _native;

        public string VersionDescription => _native.version();
        public Version Version => Version.Parse(VersionDescription.Split(new [] { "-" }, 2, StringSplitOptions.RemoveEmptyEntries)[0]);

        public string License => _native.license();

        public SolcLib()
        {
            _native = NativeLibFactory.Create();
        }

        [Obsolete("Use CompileStandard")]
        public string CompileJson(string input, bool optimize = false)
        {
            input = EncodingUtils.RemoveBom(input);
            return _native.compileJSON(input, optimize);
        }

        public string CompileStandard(string jsonInput, bool optimize = false)
        {
            var res = _native.compileStandard(jsonInput, optimize, IntPtr.Zero);
            return res;
        }

        public string CompileStandard(InputDescription input, bool optimize = false)
        {
            var jsonStr = input.ToJsonString();
            return CompileStandard(jsonStr);
        }

    }
}
