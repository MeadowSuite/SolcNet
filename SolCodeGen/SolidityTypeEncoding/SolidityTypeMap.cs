using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;

namespace SolCodeGen.SolidityTypeEncoding
{
    public static class SolidityTypeMap
    {
        static readonly ReadOnlyDictionary<string, (Type Type, string TypeName)> _types;

        static SolidityTypeMap()
        {
            var dict = new Dictionary<string, Type>
            {
                ["bool"] = typeof(bool),
                ["address"] = typeof(Address),
                ["string"] = typeof(string),
                ["bytes"] = typeof(Span<byte>)
            };

            void AddIntRange<TIntType, TUIntType>(int byteStart, int byteEnd)
            {
                for (var i = byteStart; i <= byteEnd; i++)
                {
                    var bits = i * 8;
                    dict.Add("int" + bits, typeof(TIntType));
                    dict.Add("uint" + bits, typeof(TUIntType));
                }
            }

            AddIntRange<sbyte, byte>(1, 1);
            AddIntRange<short, ushort>(2, 2);
            AddIntRange<int, uint>(3, 4);
            AddIntRange<long, ulong>(5, 8);
            AddIntRange<BigInteger, UInt256>(9, 32);

            var mapped = new Dictionary<string, (Type, string)>(dict.Count);
            foreach(var item in dict)
            {
                mapped.Add(item.Key, (item.Value, item.Value.FullName));
            }

            _types = new ReadOnlyDictionary<string, (Type, string)>(mapped);
        }

        public static string SolidityTypeToCSharpString(string name)
        {
            var netType = SolidityTypeToCSharp(name);
            return netType.FullName;
        }

        // TODO: return array size data from here...
        public static Type SolidityTypeToCSharp(string name)
        {
            Type Find(string n) => 
                _types.TryGetValue(n, out (Type Type, string Name) t)
                    ? t.Type
                    : throw new ArgumentException("Unexpected solidity ABI type: " + name, nameof(name));
            
            var arrayBracket = name.IndexOf('[');
            if (arrayBracket > 0)
            {
                var bracketPart = name.Substring(arrayBracket);
                int arraySize = bracketPart == "[]"
                    ? 0
                    : int.Parse(bracketPart.Substring(0, bracketPart.Length - 1), CultureInfo.InvariantCulture);

                var baseName = name.Substring(0, arrayBracket);
                var baseType = Find(baseName);
                return typeof(IEnumerable<>).MakeGenericType(baseType);
            }

            return Find(name);

        }

    

    }




}
