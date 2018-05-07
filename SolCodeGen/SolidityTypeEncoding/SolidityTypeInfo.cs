using System;

namespace SolCodeGen.SolidityTypeEncoding
{
    public class SolidityTypeInfo : IEquatable<SolidityTypeInfo>
    {
        /// <summary>
        /// Original type string from the json ABI.
        /// </summary>
        public readonly string SolidityName;

        /// <summary>
        /// The corresponding C# type. For solidiy array types this is an IEnumerable<TBaseType>.
        /// </summary>
        public readonly Type ClrType;

        /// <summary>
        /// The ClrType.FullName (for caching).
        /// </summary>
        public readonly string ClrTypeName;

        /// <summary>
        /// For static sized types this is the size of the entire type, 
        /// otherwise is the size of the base type for an array/dynamic type.
        /// The solidity types 'string' and 'bytes' are considered dynamic arrays
        /// of bytes where this base size is 1.
        /// </summary>
        public readonly int BaseTypeByteSize;

        /// <summary>
        /// The length of static sized array types, for dynamic arrays this is zero.
        /// For non-array types this is zero.
        /// </summary>
        public readonly int ArrayLength;

        public readonly SolidityTypeCategory Category;

        /// <summary>
        /// The elementary/base value of an array time. Null for non-array types.
        /// </summary>
        public readonly SolidityTypeInfo ArrayItemInfo;


        public SolidityTypeInfo(string solidityName, Type clrType, int baseTypeByteSize, 
            SolidityTypeCategory category = SolidityTypeCategory.Elementary, 
            int arrayTypeLength = 0, SolidityTypeInfo arrayItemInfo = null)
        {
            SolidityName = solidityName;
            ClrType = clrType;
            ClrTypeName = ClrType.FullName;
            BaseTypeByteSize = baseTypeByteSize;
            Category = category;
            ArrayLength = arrayTypeLength;
            ArrayItemInfo = arrayItemInfo;
        }

        public bool Equals(SolidityTypeInfo other)
        {
            return other.SolidityName == SolidityName;
        }

        public override bool Equals(object obj)
        {
            return obj is SolidityTypeInfo info ? Equals(info) : false;
        }

        public override int GetHashCode()
        {
            return SolidityName.GetHashCode();
        }

        public override string ToString()
        {
            return SolidityName;
        }
    }

    public enum SolidityTypeCategory
    {
        /// <summary>
        /// Static / base / primitive type, eg: uint16, address, bool, etc..
        /// </summary>
        Elementary,

        /// <summary>
        /// An static/fixed sized array type
        /// </summary>
        FixedArray,

        /// <summary>
        /// A dynamic/variably sized array type
        /// </summary>
        DynamicArray,

        /// <summary>
        /// Special encoded dynamic length string type
        /// </summary>
        String,

        /// <summary>
        /// Special encoded dynamic length bytes type
        /// </summary>
        Bytes,
        BytesM
    }



}
