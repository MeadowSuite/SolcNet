using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;

namespace SolCodeGen.SolidityTypeEncoding
{

    public static class SolidityTypeMap
    {
        /// <summary>
        /// Map of all the elementary solidity type names and their corresponding C# type.
        /// All the static types and only the 'string' and 'bytes' dynamic types.
        /// Does not include the generic/meta types: <type>[M], <type>[], or (T1,T2,...,Tn)
        /// 
        /// Note: All possible values of the C# types do not neccessarily fit into the corresponding
        /// solidity types. For example a UInt32 C# type is used for a UInt24 solidity type.
        /// Integer over/underflows are checked at runtime during encoding.
        /// 
        /// ByteSize is zero for dynamic types.
        /// </summary>
        static readonly ReadOnlyDictionary<string, SolidityTypeInfo> _types;

        static SolidityTypeMap()
        {
            // elementary types
            var dict = new Dictionary<string, SolidityTypeInfo>
            {
                // equivalent to uint8 restricted to the values 0 and 1
                ["bool"] = new SolidityTypeInfo("bool", typeof(bool), 1),
                
                // 20 bytes
                ["address"] = new SolidityTypeInfo("address", typeof(Address), 20),
                
                // dynamic sized unicode string assumed to be UTF - 8 encoded.
                ["string"] = new SolidityTypeInfo("string", typeof(string), 1, SolidityTypeCategory.String),
                
                // dynamic sized byte sequence
                ["bytes"] = new SolidityTypeInfo("bytes", typeof(Span<byte>), 1, SolidityTypeCategory.Bytes)
            };

            // fixed sized bytes elementary types
            for (var i = 1; i <= 32; i++)
            {
                dict["bytes" + i] = new SolidityTypeInfo("bytes" + i, 
                    typeof(Span<byte>), 
                    baseTypeByteSize: 1, 
                    SolidityTypeCategory.BytesM, 
                    arrayTypeLength: i);
            }

            // signed and unsigned integer type of M bits, 0 < M <= 256, M % 8 == 0
            AddIntRange<sbyte, byte>(1, 1);
            AddIntRange<short, ushort>(2, 2);
            AddIntRange<int, uint>(3, 4);
            AddIntRange<long, ulong>(5, 8);
            AddIntRange<BigInteger, UInt256>(9, 32);

            void AddIntRange<TIntType, TUIntType>(int byteStart, int byteEnd)
            {
                for (var i = byteStart; i <= byteEnd; i++)
                {
                    var bits = i * 8;
                    dict.Add("int" + bits, new SolidityTypeInfo("uint" + bits, typeof(TIntType), i));
                    dict.Add("uint" + bits, new SolidityTypeInfo("uint" + bits, typeof(TUIntType), i));
                }
            }

            _types = new ReadOnlyDictionary<string, SolidityTypeInfo>(dict);
        }

        public static string SolidityTypeToCSharpString(string name)
        {
            var info = GetSolidityTypeInfo(name);
            return info.ClrTypeName;
        }

        // TODO: return array size data from here...
        public static SolidityTypeInfo GetSolidityTypeInfo(string name)
        {
            var arrayBracket = name.IndexOf('[');
            if (arrayBracket > 0)
            {
                var bracketPart = name.Substring(arrayBracket);
                int arraySize = 0;
                var typeCategory = SolidityTypeCategory.DynamicArray;
                if (bracketPart == "[]")
                {
                    var sizeStr = bracketPart.Substring(0, bracketPart.Length - 1);
                    arraySize = int.Parse(sizeStr, CultureInfo.InvariantCulture);
                    typeCategory = SolidityTypeCategory.FixedArray;
                }

                var baseName = name.Substring(0, arrayBracket);
                if (_types.TryGetValue(baseName, out var baseInfo))
                {
                    var arrayType = typeof(IEnumerable<>).MakeGenericType(baseInfo.ClrType);
                    var info = new SolidityTypeInfo(name, arrayType, baseInfo.BaseTypeByteSize, typeCategory, arraySize);
                    return info;
                }
            }
            else
            {
                if (_types.TryGetValue(name, out var t))
                {
                    return t;
                }
            }

            throw new ArgumentException("Unexpected solidity ABI type: " + name, nameof(name));

        }

    

    }




}
