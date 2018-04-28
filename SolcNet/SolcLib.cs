using Newtonsoft.Json;
using SolcNet.InputData;
using SolcNet.NativeLib;
using System;
using System.Collections.Generic;
using System.IO;
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

        public string CompileJson(string jsonInput, string solSourceRoot = null)
        {
            //solSourceRoot = solSourceRoot ?? Directory.GetCurrentDirectory();
            var res = _native.compileStandard(jsonInput, (string path, ref string contents, ref string err) => 
            {
                try
                {
                    // if given path is relative and a root is provided, combine them
                    if (!Uri.TryCreate(path, UriKind.Absolute, out _) && solSourceRoot != null)
                    {
                        path = Path.Combine(solSourceRoot, path);
                    }
                    if (File.Exists(path))
                    {
                        contents = File.ReadAllText(path);
                    }
                    else
                    {
                        err = "Source file not found: " + path;
                    }
                }
                catch (Exception ex)
                {
                    err = ex.ToString();
                }
            });
            return res;
        }

        public string Compile(InputDescription input, string solSourceRoot = null)
        {
            var jsonStr = input.ToJsonString();
            return CompileJson(jsonStr, solSourceRoot);
        }

        public string Compile(string contractFilePath)
        {
            var fileName = Path.GetFileName(contractFilePath);
            var sourceRoot = fileName == contractFilePath ? null : Path.GetDirectoryName(contractFilePath);
            var inputDesc = new InputDescription();
            inputDesc.Sources.Add(fileName, new Source(fileName));
            return Compile(inputDesc, sourceRoot);
        }

    }
}
