using Newtonsoft.Json;
using SolcNet;
using SolcNet.InputData;
using SolcNet.NativeLib;
using System;
using System.IO;
using System.Linq;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var solcLib = new SolcLib();
            var verDesc = solcLib.VersionDescription;
            var version = solcLib.Version;

            var license = solcLib.License;

            var inputString = File.ReadAllText("test-input.json");
            var inputSol = File.ReadAllText("Ownable.sol");
            var inputDesc = InputDescription.FromJsonString(inputString);
            inputDesc.Sources["Ownable.sol"] = new Source { Content = inputSol };
            var compiled = solcLib.CompileStandard(inputDesc);
        }


    }
}
