using SolCodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class HexStringExtensions
    {
        public static ReadOnlyMemory<byte> HexToReadOnlyMemory(this string hexString)
        {
            return HexConverter.HexToMemory(hexString);
        }

        public static byte[] HexToBytes(this string hexString)
        {
            return HexConverter.HexToBytes(hexString);
        }

        public static Span<byte> HexToSpan(this string hexString)
        {
            return HexConverter.HexToBytes(hexString);
        }

        public static ReadOnlySpan<byte> HexToReadOnlySpan(this string hexString)
        {
            return HexConverter.HexToBytes(hexString);
        }

        public static string ToHexString(this ReadOnlySpan<byte> bytes, bool hexPrefix = true)
        {
            return HexConverter.GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string ToHexString(this Span<byte> bytes, bool hexPrefix = true)
        {
            return HexConverter.GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string ToHexString(this byte[] bytes, bool hexPrefix = true)
        {
            return HexConverter.GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string ToHexString(this IEnumerable<byte> bytes, bool hexPrefix = true)
        {
            return HexConverter.GetHexFromBytes(bytes.ToArray(), hexPrefix: hexPrefix);
        }
    }
}
