using SolcNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.NetFramework
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
            var compiled = solcLib.Compile("OpenZeppelin/token/ERC20/StandardToken.sol");
        }
    }
}
