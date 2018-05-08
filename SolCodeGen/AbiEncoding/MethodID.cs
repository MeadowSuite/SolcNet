using System;
using System.Collections.Generic;
using System.Text;

namespace SolCodeGen.AbiEncoding
{
    /// <summary>
    /// The first four bytes of the call data for a function call specifies the function to be called. 
    /// It is the first (left, high-order in big-endian) four bytes of the Keccak (SHA-3) hash of the 
    /// signature of the function. The signature is defined as the canonical expression of the basic 
    /// prototype, i.e. the function name with the parenthesised list of parameter types. Parameter 
    /// types are split by a single comma - no spaces are used.
    /// <see href="https://solidity.readthedocs.io/en/v0.4.23/abi-spec.html#function-selector"/>
    /// </summary>
    public static class MethodID
    {
        /// <summary>
        /// Creates the 4 byte function selector from a function signature string
        /// </summary>
        /// <param name="functionSignature">Function signature, ex: "baz(uint32,bool)"</param>
        /// <param name="hexPrefix">True to prepend the hex string with "0x"</param>
        /// <returns>8 character lowercase hex string (from first 4 bytes of the sha3 hash of utf8 encoded function signature)</returns>
        public static string GetMethodID(string functionSignature, bool hexPrefix = false)
        {
            var bytes = Encoding.UTF8.GetBytes(functionSignature);
            var hash = Keccak.ComputeHash(bytes).Slice(0, 4);
            string funcSignature = HexConverter.BytesToHex(hash, hexPrefix: hexPrefix, checkEndian: false);
            return funcSignature;
        }
    }
}
