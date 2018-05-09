using System;
using System.Collections.Generic;
using System.Text;

namespace SolCodeGen.JsonRpc.RequestMessages
{
    public class EthGetTransactionReceipt : JsonRpcRequest<Hash>
    {
        public EthGetTransactionReceipt(Hash data) : base("eth_getTransactionReceipt", data)
        {
           
        }
    }
}
