using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
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
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in ushort s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(bytes, s);
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in int s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(bytes, s);
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in uint s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(bytes, s);
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in long s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(bytes, s);
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromInteger(in ulong s, bool hexPrefix = false)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(bytes, s);
            StripLeadingZeros(ref bytes);
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
            StripLeadingZeros(ref bytes);
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        static void StripLeadingZeros(ref Span<byte> span)
        {
            int startIndex = 0;
            for (; startIndex < span.Length && span[startIndex] == 0x0; startIndex++) { }
            if (startIndex != 0)
            {
                span = span.Slice(startIndex);
            }
        }

        public static string GetHex<T>(in T s, bool hexPrefix = false) where T : struct
        {
            Span<byte> bytes = stackalloc byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write(bytes, ref Unsafe.AsRef(s));
            return GetHexFromBytes(bytes, hexPrefix: hexPrefix);
        }

        public static string GetHexFromBytes(this Span<byte> bytes, bool hexPrefix = false)
        {
            if (hexPrefix && bytes.Length == 0)
            {
                return "0x00";
            }
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
                if (_bigEndianCheckTypes.Contains(targetType))
                {
                    methodInfo = typeof(HexConverter).GetMethod(nameof(HexToInteger), BindingFlags.Static | BindingFlags.Public);
                    methodInfo = methodInfo.MakeGenericMethod(targetType);
                }
                else
                {
                    methodInfo = typeof(HexConverter).GetMethod(nameof(HexToValue), BindingFlags.Static | BindingFlags.Public);
                    methodInfo = methodInfo.MakeGenericMethod(targetType);
                }
                ParseHexGenericCache[targetType] = methodInfo;
            }

            return methodInfo.Invoke(null, new object[] { str });
        }

        public static TInt HexToInteger<TInt>(string str) where TInt : struct
        {
            ReadOnlySpan<char> strSpan = str.AsSpan();
            StripHexPrefix(ref strSpan);
            int byteLen = strSpan.Length / 2;
            Span<byte> bytes = stackalloc byte[byteLen];
            HexToSpan(strSpan, bytes);
            int typeSize = Unsafe.SizeOf<TInt>();

            if (typeSize < byteLen)
            {
                throw new ArgumentException($"Target type '{typeof(TInt)}' is {Unsafe.SizeOf<TInt>()} bytes but was given {bytes.Length} bytes of input");
            }

            if (typeSize > byteLen)
            {
                Span<byte> resized = stackalloc byte[typeSize];
                bytes.CopyTo(resized.Slice(typeSize - byteLen));
                bytes = resized;
            }

            switch (bytes.Length)
            {
                case 1:
                    return Unsafe.As<byte, TInt>(ref bytes[0]);
                case 2:
                    return Unsafe.As<ushort, TInt>(ref Unsafe.AsRef(BinaryPrimitives.ReadUInt16BigEndian(bytes)));
                case 4:
                    return Unsafe.As<uint, TInt>(ref Unsafe.AsRef(BinaryPrimitives.ReadUInt32BigEndian(bytes)));
                case 8:
                    return Unsafe.As<ulong, TInt>(ref Unsafe.AsRef(BinaryPrimitives.ReadUInt64BigEndian(bytes)));
                case 32:
                    bytes.Reverse();
                    return MemoryMarshal.Read<TInt>(bytes);
                default:
                    throw new ArgumentException($"Unexpected input length {bytes.Length}");
            }
        }

        public static void StripHexPrefix(ref string str)
        {
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(2);
            }
        }

        public static void StripHexPrefix(ref ReadOnlySpan<char> str)
        {
            if (str[0] == '0' && (str[1] == 'x' || str[1] == 'X'))
            {
                str = str.Slice(2);
            }
        }

        public static T HexToValue<T>(string str) where T : struct
        {
            if (_bigEndianCheckTypes.Contains(typeof(T)))
            {
                throw new ArgumentException($"Integer types should be parsed with {nameof(HexToInteger)}");
            }
            StripHexPrefix(ref str);

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
            //bool endianSwap = _bigEndianCheckTypes.Contains(typeof(T)) && BitConverter.IsLittleEndian;
            for (int i = 0; i < resultBytes.Length; i++)
            {
                var index = /*endianSwap ? resultBytes.Length - i - 1 :*/ i;
                resultBytes[index] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return MemoryMarshal.Read<T>(resultBytes);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HexCharToByte(in char c)
        {
            var b = c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0');
            return (byte)b;
        }

        /// <summary>
        /// Expected str to already be stripped of hex prefix, 
        /// and expects bytes to already be allocated to the correct size
        /// </summary>
        public static void HexToSpan(ReadOnlySpan<char> str, in Span<byte> bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(HexCharToByte(str[i * 2]) << 4 | HexCharToByte(str[i * 2 + 1]));
            }
        }

        public static byte[] HexToBytes(string str)
        {
            StripHexPrefix(ref str);
            ReadOnlySpan<char> strSpan = str.AsSpan();
            var byteArr = new byte[strSpan.Length / 2];
            Span<byte> bytes = byteArr;
            HexToSpan(strSpan, bytes);
            return byteArr;
        }

    }
}
