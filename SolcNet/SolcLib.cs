using Newtonsoft.Json;
using SolcNet.CompileErrors;
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
        ISolcLib _native;

        public string VersionDescription => _native.GetVersion();
        public Version Version => Version.Parse(VersionDescription.Split(new [] { "-" }, 2, StringSplitOptions.RemoveEmptyEntries)[0]);

        public string License => _native.GetLicense();

        string _solSourceRoot = null;
        string _lastSourceDir = null;

        public SolcLib(string solSourceRoot = null)
        {
            _native = new SolcLibPInvoke();
            _solSourceRoot = solSourceRoot;
        }

        private OutputDescription CompileInputDescriptionJson(string jsonInput, 
            CompileErrorHandling errorHandling = CompileErrorHandling.ThrowOnError)
        {
            var res = _native.Compile(jsonInput, ReadSolSourceFileManaged);
            var output = OutputDescription.FromJsonString(res);
            var compilerException = CompilerException.GetCompilerExceptions(output.Errors, errorHandling);
            if (compilerException != null)
            {
                throw compilerException;
            }
            return output;
        }

        public OutputDescription Compile(InputDescription input, 
            CompileErrorHandling errorHandling = CompileErrorHandling.ThrowOnError)
        {
            var jsonStr = input.ToJsonString();
            return CompileInputDescriptionJson(jsonStr, errorHandling);
        }

        /// <param name="contractFilePath"></param>
        /// <param name="outputSelection">Defaults to all output types if not specified</param>
        /// <param name="errorHandling"></param>
        /// <returns></returns>
        public OutputDescription Compile(string contractFilePath, 
            OutputType[] outputSelection = null, 
            CompileErrorHandling errorHandling = CompileErrorHandling.ThrowOnError)
        {
            outputSelection = outputSelection ?? OutputType.All;

            var fileName = Path.GetFileName(contractFilePath);
            var inputDesc = new InputDescription();
            inputDesc.Settings.OutputSelection["*"] = new Dictionary<string, List<OutputType>>
            {
                ["*"] = new List<OutputType>(outputSelection)
            };
            var source = new Source { Urls = new List<string> { contractFilePath } };
            inputDesc.Sources.Add(fileName, source);
            return Compile(inputDesc, errorHandling);
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
                    contents = File.ReadAllText(sourceFilePath, Encoding.UTF8);
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
