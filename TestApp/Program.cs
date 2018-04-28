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
            Console.WriteLine("Init");

            var solcLib = new SolcLib();
            var verDesc = solcLib.VersionDescription;
            var version = solcLib.Version;
            var license = solcLib.License;

            //var inputString = File.ReadAllText("TestResources/test-input.json");
            //var inputDesc = InputDescription.FromJsonString(inputString);
            //var inputSol = File.ReadAllText("TestResources/Ownable.sol");
            //inputDesc.Sources["Ownable.sol"] = new Source { Content = inputSol };
            //var compiled = solcLib.Compile(inputDesc, "TestResources/ZeppelinErc20");
            var compiled = solcLib.Compile("OpenZeppelin/token/ERC20/StandardToken.sol");
        }


    }
}
