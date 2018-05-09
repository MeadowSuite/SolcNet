using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SolCodeGen
{
    public static class HexConverter
    {

        public static string GetHexFromObject(object val, bool hexPrefix = false)
        {
            switch (val)
            {
                case byte v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case sbyte v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case short v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case ushort v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case int v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case uint v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case long v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case ulong v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case UInt256 v: return GetHexFromInteger(v, hexPrefix: hexPrefix);
                case Address v: return GetHex<Address>(v, hexPrefix: hexPrefix);
                case Hash v: return GetHex<Hash>(v, hexPrefix: hexPrefix);
                case byte[] v: return GetHexFromBytes(v, hexPrefix: hexPrefix);
                default:
                    throw new Exception($"Converting type '{val.GetType()}' to hex is not supported");
            }
        }

        public static string GetHexFromInteger(in byte s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(byte)];
            bytes[0] = s;
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in sbyte s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(sbyte)];
            bytes[0] = unchecked((byte)s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in short s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in ushort s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in int s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in uint s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in long s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in ulong s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(bytes, s);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in UInt256 s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[UInt256.SIZE];
            MemoryMarshal.Write(bytes, ref Unsafe.AsRef(s));
            if (BitConverter.IsLittleEndian)
            {
                bytes.Reverse();
            }
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHex<T>(in T s, bool hexPrefix = false) where T : struct
        {
            Span<byte> bytes = stackalloc byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write(bytes, ref Unsafe.AsRef(s));
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromBytes(this Span<byte> bytes, bool hexPrefix = false)
        {
            Span<char> charArr = stackalloc char[bytes.Length * 2 + (hexPrefix ? 2 : 0)];
            Span<char> c = charArr;
            if (hexPrefix)
            {
                c[0] = '0';
                c[1] = 'x';
                c = c.Slice(2);
            }

            for (int i = 0; i < bytes.Length; ++i)
            {
                ref byte index = ref bytes[i];
                c[i * 2] = GetHexCharFromByte((byte)(index >> 4));
                c[i * 2 + 1] = GetHexCharFromByte((byte)(index & 0xF));
            }

            return charArr.ToString();
        }

        /// <summary>
        /// Returns single lowercase hex character for given byte
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char GetHexCharFromByte(in byte b)
        {
            return (char)(b > 9 ? b + 0x57 : b + 0x30);
        }

        readonly static Dictionary<Type, MethodInfo> ParseHexGenericCache = new Dictionary<Type, MethodInfo>();

        static readonly HashSet<Type> _bigEndianCheckTypes = new HashSet<Type>
        {
            typeof(ushort), typeof(short),
            typeof(uint), typeof(int),
            typeof(ulong), typeof(long),
            typeof(UInt256)
        };

        public static object HexToObject(Type targetType, string str)
        {

            if (!targetType.IsValueType)
            {
                throw new ArgumentException($"Type '{targetType}' is not a struct", nameof(targetType));
            }

            MethodInfo methodInfo;

            if (!ParseHexGenericCache.TryGetValue(targetType, out methodInfo))
            {
                methodInfo = typeof(HexConverter).GetMethod(nameof(HexToValue), BindingFlags.Static | BindingFlags.Public);
                methodInfo = methodInfo.MakeGenericMethod(targetType);
                ParseHexGenericCache[targetType] = methodInfo;
            }

            return methodInfo.Invoke(null, new object[] { str });
        }

        public static T HexToValue<T>(string str) where T : struct
        {
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(2);
            }

            var byteLen = str.Length / 2;
            var tSize = Marshal.SizeOf<T>();
            if (byteLen > tSize)
            {
                var overSize = (byteLen - tSize) * 2;
                if (str.Substring(0, overSize) != new string('0', overSize))
                {
                    throw new ArgumentException($"Cannot fit {byteLen} bytes from hex string into {tSize} byte long type {typeof(T)}");
                }
                str = str.Substring(overSize);
            }
            else if (byteLen < tSize)
            {
                var underSize = (tSize - byteLen) * 2;
                str = new string('0', underSize) + str;
            }
            Span<byte> resultBytes = stackalloc byte[Marshal.SizeOf<T>()];
            bool endianSwap = _bigEndianCheckTypes.Contains(typeof(T)) && BitConverter.IsLittleEndian;
            for (int i = 0; i < resultBytes.Length; i++)
            {
                var index = endianSwap ? resultBytes.Length - i - 1 : i;
                resultBytes[index] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return MemoryMarshal.Read<T>(resultBytes);
        }


        public static byte[] HexToBytes(string str)
        {
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(2);
            }
            
            var outputLength = str.Length / 2;
            var output = new byte[outputLength];
            for (var i = 0; i < outputLength; i++)
            {
                output[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return output;
        }

    }
}
