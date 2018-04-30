using Newtonsoft.Json;
using SolcNet.DataDescription.Input;
using SolcNet.DataDescription.Output;
using SolcNet.NativeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet
{
    public class SolcLib
    {
        protected ISolcNativeLib _native;

        public string VersionDescription => _native.version();
        public Version Version => Version.Parse(VersionDescription.Split(new [] { "-" }, 2, StringSplitOptions.RemoveEmptyEntries)[0]);

        public string License => _native.license();

        string _solSourceRoot = null;
        string _lastSourceDir = null;

        public SolcLib(string solSourceRoot = null)
        {
            _native = NativeLibFactory.Create();
            _solSourceRoot = solSourceRoot;
        }

        public OutputDescription CompileJson(string jsonInput)
        {
            var res = _native.compileStandard(jsonInput, ReadSolSourceFileManaged);
            var output = OutputDescription.FromJsonString(res);
            return output;
        }

        public OutputDescription Compile(InputDescription input)
        {
            var jsonStr = input.ToJsonString();
            return CompileJson(jsonStr);
        }

        public OutputDescription Compile(string contractFilePath, params OutputType[] outputSelection)
        {
            var fileName = Path.GetFileName(contractFilePath);
            var inputDesc = new InputDescription();
            inputDesc.Settings.OutputSelection["*"] = new Dictionary<string, List<OutputType>>
            {
                ["*"] = new List<OutputType>(outputSelection)
            };
            var source = new Source { Urls = new List<string> { contractFilePath } };
            inputDesc.Sources.Add(fileName, source);
            return Compile(inputDesc);
        }

        void ReadSolSourceFileManaged(string path, ref string contents, ref string error)
        {
            try
            {
                string sourceFilePath = path;
                // if given path is relative and a root is provided, combine them
                if (!Uri.TryCreate(path, UriKind.Absolute, out _) && _solSourceRoot != null)
                {
                    sourceFilePath = Path.Combine(_solSourceRoot, path);
                }
                if (!File.Exists(sourceFilePath) && _lastSourceDir != null)
                {
                    sourceFilePath = Path.Combine(_lastSourceDir, path);
                }
                if (File.Exists(sourceFilePath))
                {
                    _lastSourceDir = Path.GetDirectoryName(sourceFilePath);
                    contents = File.ReadAllText(sourceFilePath);
                }
                else
                {
                    error = "Source file not found: " + path;
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }

    }
}
