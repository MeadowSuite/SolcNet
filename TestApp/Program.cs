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

            const string OPEN_ZEP_DIR = "OpenZeppelin";

            var solcLib = new SolcLib(OPEN_ZEP_DIR);

            var ver = solcLib.VersionDescription;
            var license = solcLib.License;

            var outputSelection = new[] {
                    OutputType.Abi,
                    OutputType.Ast,
                    OutputType.EvmMethodIdentifiers,
                    OutputType.EvmAssembly,
                    OutputType.EvmBytecode,
                    OutputType.IR,
                    OutputType.DevDoc,
                    OutputType.UserDoc
                };

            var srcs = new[] {
                "contracts/AddressUtils.sol",
                "contracts/Bounty.sol",
                "contracts/DayLimit.sol",
                "contracts/ECRecovery.sol",
                "contracts/LimitBalance.sol",
                "contracts/MerkleProof.sol",
                "contracts/ReentrancyGuard.sol",
                "contracts/access/SignatureBouncer.sol",
                "contracts/crowdsale/Crowdsale.sol",
                "contracts/examples/SampleCrowdsale.sol",
                "contracts/examples/SimpleSavingsWallet.sol",
                "contracts/examples/SimpleToken.sol",
                "contracts/token/ERC20/StandardBurnableToken.sol",
                "contracts/token/ERC721/ERC721Token.sol",
                "contracts/token/ERC827/ERC827Token.sol"
            };
            
            int runs = 0;
            while (true)
            {
                var compiled = solcLib.Compile(srcs, outputSelection, CompileErrorHandling.ThrowOnError);
                //GC.Collect(GC.MaxGeneration);
                runs++;
                Console.WriteLine(runs);
            }
        }


    }
}
