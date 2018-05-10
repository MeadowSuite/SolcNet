using Newtonsoft.Json;
using SolcNet;
using SolcNet.DataDescription.Input;
using SolcNet.DataDescription.Output;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolCodeGen.TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Uri server = new Uri("http://127.0.0.1:7545");
            var rpcClient = new JsonRpc.JsonRpcClient(server, "0x0", "0x0");
            var accounts = await rpcClient.Accounts();
            var balance = await rpcClient.GetBalance(accounts[0]);

            await rpcClient.EvmMine();
            var blockNum = await rpcClient.BlockNumber();
            var block = await rpcClient.GetBlockByNumber(blockNumber: blockNum);



            var compiledExample = new SolcLib("TestData").Compile("ExampleContract.sol");
            var abi = compiledExample.Contracts.Values.First().Values.First().Abi;
            var abiJson = JsonConvert.SerializeObject(abi, Formatting.Indented);

            //var ex = await ExampleGeneratedContract.New("My Token", 8);
            //var transferResult = await ex.Transfer.Send(("0x1234", 1000));

            const string OPEN_ZEP_DIR = "OpenZeppelin";
            var solcLib = SolcLib.Create(OPEN_ZEP_DIR);
            var srcs = new[] {
                "contracts/AddressUtils.sol",
                "contracts/Bounty.sol",
                "contracts/DayLimit.sol",
                "contracts/ECRecovery.sol",
                "contracts/LimitBalance.sol",
                "contracts/MerkleProof.sol",
                "contracts/ReentrancyGuard.sol",
                "contracts/crowdsale/Crowdsale.sol",
                "contracts/examples/SampleCrowdsale.sol",
                "contracts/token/ERC20/StandardBurnableToken.sol",
            };
            var solcOutput = solcLib.Compile(srcs);

            var projDirectory = Directory.GetParent(typeof(Program).Assembly.Location);
            bool projDirFound = false;
            for (var i = 0; i < 5; i++)
            {
                if (projDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).Any())
                {
                    projDirFound = true;
                    break;
                }
                projDirectory = projDirectory.Parent;
            }

            if (!projDirFound)
            {
                throw new Exception("Could not find project directory");
            }

            var generatedContractsDir = Path.Combine(projDirectory.FullName, "GeneratedContracts");
            if (Directory.Exists(generatedContractsDir))
            {
                Directory.Delete(generatedContractsDir, true);
            }
            Directory.CreateDirectory(generatedContractsDir);


            foreach (var entry in solcOutput.Contracts.SelectMany(c => c.Value))
            {
                var generator = new ContractGenerator(entry.Key, entry.Value);
                var generatedContractCode = generator.GenerateCodeFile();

                File.WriteAllText(Path.Combine(generatedContractsDir, entry.Key + ".cs"), generatedContractCode);
            }

            await Task.Yield();

        }
    }
}
