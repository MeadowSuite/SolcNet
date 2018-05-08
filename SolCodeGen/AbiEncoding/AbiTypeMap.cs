using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;

namespace SolCodeGen.AbiEncoding
{

    public static class AbiTypeMap
    {
        /// <summary>
        /// Map of all finite solidity type names and their corresponding C# type.
        /// Includes all the static / elementary types, as well as the explicit dynamic
        /// types 'string' and 'bytes'.
        /// 
        /// Does not include the array or tuple types; i.e.: <type>[M], <type>[], or (T1,T2,...,Tn)
        /// 
        /// Note: All possible values of the C# types do not neccessarily fit into the corresponding
        /// solidity types. For example a UInt32 C# type is used for a UInt24 solidity type.
        /// Integer over/underflows are checked at runtime during encoding.
        /// 
        /// ByteSize is zero for dynamic types.
        /// </summary>
        static readonly ReadOnlyDictionary<string, AbiTypeInfo> _finiteTypes;

        /// <summary>
        /// Cache of solidity types parsed during runtime; eg: arrays, tuples
        /// </summary>
        static readonly Dictionary<string, AbiTypeInfo> _cachedTypes = new Dictionary<string, AbiTypeInfo>();

        static AbiTypeMap()
        {
            // elementary types
            var dict = new Dictionary<string, AbiTypeInfo>
            {
                // equivalent to uint8 restricted to the values 0 and 1
                ["bool"] = new AbiTypeInfo("bool", typeof(bool), 1),
                
                // 20 bytes
                ["address"] = new AbiTypeInfo("address", typeof(Address), 20),
                
                // dynamic sized unicode string assumed to be UTF - 8 encoded.
                ["string"] = new AbiTypeInfo("string", typeof(string), 1, SolidityTypeCategory.String),
                
                // dynamic sized byte sequence
                ["bytes"] = new AbiTypeInfo("bytes", typeof(IEnumerable<byte>), 1, SolidityTypeCategory.Bytes)
            };

            // fixed sized bytes elementary types
            for (var i = 1; i <= 32; i++)
            {
                dict["bytes" + i] = new AbiTypeInfo("bytes" + i, 
                    typeof(IEnumerable<byte>), 
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
                    dict.Add("int" + bits, new AbiTypeInfo("int" + bits, typeof(TIntType), i));
                    dict.Add("uint" + bits, new AbiTypeInfo("uint" + bits, typeof(TUIntType), i));
                }
            }

            _finiteTypes = new ReadOnlyDictionary<string, AbiTypeInfo>(dict);
        }

        public static string SolidityTypeToClrTypeString(string name)
        {
            var info = GetSolidityTypeInfo(name);
            return info.ClrTypeName;
        }

        // TODO: return array size data from here...
        public static AbiTypeInfo GetSolidityTypeInfo(string name)
        {
            var arrayBracket = name.IndexOf('[');
            if (arrayBracket > 0)
            {
                if (_cachedTypes.TryGetValue(name, out var t))
                {
                    return t;
                }
                var bracketPart = name.Substring(arrayBracket);
                int arraySize = 0;
                var typeCategory = SolidityTypeCategory.DynamicArray;

                // if a fixed array length has been set, ex: uint64[10]
                if (bracketPart.Length > 2)
                {
                    // parse the number within the square brackets
                    var sizeStr = bracketPart.Substring(1, bracketPart.Length - 2);
                    arraySize = int.Parse(sizeStr, CultureInfo.InvariantCulture);
                    typeCategory = SolidityTypeCategory.FixedArray;
                }

                var baseName = name.Substring(0, arrayBracket);
                if (_finiteTypes.TryGetValue(baseName, out var baseInfo))
                {
                    var arrayType = typeof(IEnumerable<>).MakeGenericType(baseInfo.ClrType);
                    var info = new AbiTypeInfo(name, arrayType, baseInfo.BaseTypeByteSize, typeCategory, arraySize);
                    _cachedTypes[name] = info;
                    return info;
                }
            }
            else if (_finiteTypes.TryGetValue(name, out var t))
            {
                return t;
            }
    
            throw new ArgumentException("Unexpected solidity ABI type: " + name, nameof(name));

        }

    

    }




}
