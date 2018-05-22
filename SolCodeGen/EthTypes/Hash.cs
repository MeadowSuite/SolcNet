using HoshoEthUtil;
using Newtonsoft.Json;
using SolCodeGen.JsonRpc;
using SolCodeGen.Utils;
using System;
using System.Runtime.InteropServices;

namespace SolCodeGen
{
    [JsonConverter(typeof(JsonRpcHexConverter))]
    [StructLayout(LayoutKind.Sequential)]
    public struct Hash : IEquatable<Hash>
    {
        public const int SIZE = 32;

        public static readonly Hash Zero = new Hash();

        // parts are big-endian
        readonly ulong P1;
        readonly ulong P2;
        readonly ulong P3;
        readonly ulong P4;

        public byte[] GetBytes() => MemoryMarshal.AsBytes(new Span<ulong>(new[] { P1, P2, P3, P4 })).ToArray();
        public string GetHexString(bool hexPrefix = true) => HexConverter.GetHex<Hash>(this, hexPrefix: hexPrefix);

        public Hash(string hexString)
        {
            if (hexString.Length == SIZE * 2 + 2)
            {
                if (hexString.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    hexString = hexString.Substring(2);
                }
            }
            if (hexString.Length != SIZE * 2)
            {
                throw new ArgumentException("Hash hex string should be 40 chars long, or 42 with a 0x prefix, was given " + hexString.Length, nameof(hexString));
            }

            Span<byte> bytes = HexUtil.HexToBytes(hexString);
            var uintView = MemoryMarshal.Cast<byte, ulong>(bytes);
            P1 = uintView[0];
            P2 = uintView[1];
            P3 = uintView[2];
            P4 = uintView[3];
        }

        public Hash(byte[] bytes)
        {
            if (bytes.Length != SIZE)
            {
                throw new ArgumentException("Byte arrays for hash should be 32 bytes long, was given " + bytes.Length, nameof(bytes));
            }
            var uintView = MemoryMarshal.Cast<byte, ulong>(bytes);
            P1 = uintView[0];
            P2 = uintView[1];
            P3 = uintView[2];
            P4 = uintView[3];
        }

        public string ToString(bool hexPrefix = true) => GetHexString(hexPrefix);
        public override string ToString() => GetHexString();

        public override int GetHashCode() => (P1, P2, P3, P4).GetHashCode();

        public override bool Equals(object obj) => obj is Hash addr ? Equals(addr) : false;

        public bool Equals(Hash other)
        {
            return other.P1 == P1 && other.P2 == P2 && other.P3 == P3 && other.P4 == P4;
        }

        public static bool operator ==(Hash a, Hash b) => a.Equals(b);
        public static bool operator !=(Hash a, Hash b) => !a.Equals(b);

        public static implicit operator Hash(string value) => HexConverter.HexToValue<Hash>(value);
        public static implicit operator string(Hash value) => value.GetHexString();
    }



}
