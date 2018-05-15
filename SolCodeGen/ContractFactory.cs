using SolCodeGen.JsonRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SolCodeGen
{
    public static class ContractFactory
    {

        /// <summary>
        /// Deploys a contract that has no constructor arguments
        /// </summary>
        /// <param name="abiEncodedConstructorParams">ABI encoded function selector and constructor parameters</param>
        public static async Task<Address> Deploy(
            JsonRpcClient rpcClient,
            ReadOnlyMemory<byte> bytecode, 
            ReadOnlyMemory<byte> abiEncodedConstructorArgs, 
            SendParams sendParams = null)
        {
            var deploymentHex = HexConverter.GetHexFromBytes(hexPrefix: true, bytecode, abiEncodedConstructorArgs);
            var transHash = await rpcClient.SendTransaction(deploymentHex, sendParams: sendParams);
            var receipt = await rpcClient.GetTransactionReceipt(transHash);
            return receipt.ContractAddress.Value;
        }

        /// <summary>
        /// Deploys a contract that has no constructor arguments
        /// </summary>
        public static async Task<Address> Deploy(
            JsonRpcClient rpcClient,
            ReadOnlyMemory<byte> bytecode,
            SendParams sendParams)
        {
            var deploymentHex = HexConverter.GetHexFromBytes(hexPrefix: true, bytecode);
            var transHash = await rpcClient.SendTransaction(deploymentHex, sendParams: sendParams);
            var receipt = await rpcClient.GetTransactionReceipt(transHash);
            return receipt.ContractAddress.Value;
        }
    }
}
