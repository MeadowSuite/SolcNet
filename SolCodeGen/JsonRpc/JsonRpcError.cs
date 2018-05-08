﻿using Newtonsoft.Json;
using System;

namespace SolCodeGen.JsonRpc
{
    public class JsonRpcError
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public JsonRpcErrorException ToException() => new JsonRpcErrorException(this);
    }

    public class JsonRpcErrorException : Exception
    {
        public long Code => Error.Code;
        public readonly JsonRpcError Error;

        public JsonRpcErrorException(JsonRpcError err):base(err.Message)
        {
            Error = err;
        }
    }
}