using Newtonsoft.Json;
using SolcNet;
using SolcNet.CompileErrors;
using SolcNet.DataDescription.Input;
using SolcNet.NativeLib;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Init");

            var solcLib = new SolcLib("OpenZeppelin");

            var outputSelection = new[] {
                    OutputType.Abi,
                    OutputType.Ast,
                    OutputType.EvmBytecode,
                    OutputType.Metadata,
                    OutputType.IR,
                    OutputType.DevDoc,
                    OutputType.UserDoc
                };
            outputSelection = OutputType.All;
            var compiled = solcLib.Compile("token/ERC20/StandardToken.sol", outputSelection, CompileErrorHandling.ThrowOnWarning);

        }


    }
}
