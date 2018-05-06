using System;
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

        public static string GetHexFromObject(object val)
        {
            switch (val)
            {
                case byte v: return GetHex(v);
                case sbyte v: return GetHex(v);
                case short v: return GetHex((int)v);
                case ushort v: return GetHex((int)v);
                case int v: return GetHex(v);
                case uint v: return GetHex(v);
                case long v: return GetHex(v);
                case ulong v: return GetHex(v);
                case UInt256 v: return GetHex(v);
                case Address v: return GetHex(v, bigEndian: false);
                case byte[] v: return BytesToHex(v, bigEndian: false, hexPrefix: true);
                default:
                    throw new Exception($"Converting type '{val.GetType()}' to hex is not supported");
            }
        }

        public static string GetHex<T>(T s, bool bigEndian = true, bool hexPrefix = true) where T : struct
        {
            //var bytes = MemoryMarshal.Cast<T, byte>(new Span<T>(new[] { s }));
            //Span<byte> bytes = stackalloc byte[Unsafe.SizeOf<T>()];
            Span<byte> bytes = stackalloc byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write(bytes, ref s);
            return BytesToHex(bytes, bigEndian, hexPrefix);
        }

        public static string BytesToHex(Span<byte> bytes, bool bigEndian, bool hexPrefix)
        {
            var charArr = new char[bytes.Length * 2 + (hexPrefix ? 2 : 0)];
            Span<char> c = charArr;
            if (hexPrefix)
            {
                c[0] = '0';
                c[1] = 'x';
                c = c.Slice(2);
            }
            byte b;
            for (int i = 0; i < bytes.Length; ++i)
            {
                var index = bigEndian ? bytes.Length - i - 1 : i;
                b = ((byte)(bytes[index] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x57 : b + 0x30);
                b = ((byte)(bytes[index] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x57 : b + 0x30);
            }
            return new string(charArr);
        }

        readonly static Dictionary<Type, MethodInfo> ParseHexGenericCache = new Dictionary<Type, MethodInfo>();

        public static object HexToObject(Type targetType, string str, bool bigEndian = true)
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

            return methodInfo.Invoke(null, new object[] { str, bigEndian });
        }

        public static T HexToValue<T>(string str, bool bigEndian = true) where T : struct
        {
            if (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(2);
            }

            var byteLen = str.Length / 2;
            //var tSize = Unsafe.SizeOf<T>();
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

            Span<T> result = new Span<T>(new T[1]);
            var resultBytes = MemoryMarshal.AsBytes(result);
            for (int i = 0; i < resultBytes.Length; i++)
            {
                var index = bigEndian ? resultBytes.Length - i - 1 : i;
                resultBytes[index] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return result[0];
        }

        /*
        public static TTo LooseStructConvert<TFrom, TTo>(TFrom val) 
            where TFrom : struct
            where TTo : struct
        {
            var fromSize = Unsafe.SizeOf<TFrom>();
            var toSize = Unsafe.SizeOf<TTo>();
            if (fromSize > toSize)
            {
                throw new ArgumentException($"From type '{typeof(TFrom)}' is larger than target type '{typeof(TTo)}', {fromSize} vs {toSize} bytes");
            }

        }
        */

    }
}
