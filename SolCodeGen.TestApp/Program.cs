using Newtonsoft.Json;
using SolcNet;
using SolcNet.DataDescription.Input;
using SolcNet.DataDescription.Output;
using SolCodeGen.JsonRpc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SolCodeGen.TestApp
{
    class Program
    {
        static async Task Rpc()
        {
            Uri server = new Uri("http://127.0.0.1:7545");
            var rpcClient = new JsonRpc.JsonRpcClient(server);
            var ver = await rpcClient.Version();
            var protoVer = await rpcClient.ProtocolVersion();

            var accounts = await rpcClient.Accounts();
            var balance = await rpcClient.GetBalance(accounts[0]);

            await rpcClient.EvmMine();
            var blockNum = await rpcClient.BlockNumber();
            var block = await rpcClient.GetBlockByNumber(blockNumber: blockNum);
        }

        static async Task Main(string[] args)
        {
            var compiledExample = new SolcLib("TestData").Compile("ExampleContract.sol", 
                OutputType.Abi | OutputType.EvmBytecodeObject | OutputType.DevDoc | OutputType.UserDoc);

            var bytecode = compiledExample.Contracts.Values.First().Values.First().Evm.Bytecode.Object.ToHexString();
            var abi = compiledExample.Contracts.Values.First().Values.First().Abi;
            var abiJson = JsonConvert.SerializeObject(abi, Formatting.None);

            ExampleGeneratedContract.ABI_JSON = abiJson;
            ExampleGeneratedContract.BYTECODE_HEX = bytecode;

            Uri server = new Uri("http://127.0.0.1:7545");
            var rpcClient = new JsonRpcClient(server);
            var accounts = await rpcClient.Accounts();
            var exContract = await ExampleGeneratedContract.New(rpcClient, (99999999999999, true, 22222), new SendParams
            {
                From = accounts[2],
                Gas = 5_000_000,
            }, accounts[3]);

            var echoStringResult = await exContract.echoString("hello world").Call();

            var echoManyResult = await exContract.echoMany(accounts[9], 12345, "asdf").Call();

            return;

            //await Rpc();

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
