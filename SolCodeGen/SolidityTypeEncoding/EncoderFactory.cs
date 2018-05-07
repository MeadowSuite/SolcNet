using System;
using System.Collections.Generic;
using System.Numerics;

namespace SolCodeGen.SolidityTypeEncoding
{
    /// <summary>
    /// Generic-overloaded methods for easy access to typed encoders from generated contracts.
    /// Trade off of: Lots of code here for less code in the generated contracts, and less 
    /// dynamic runtime type checking.
    /// </summary>
    public static class EncoderFactory
    {
        // TODO: if we use the t4 generated UInt<M> types, use a t4 generator to create the corresponding LoadEncoder methods here...

        public static ISolidityTypeEncoder LoadEncoder<TItem>(string solidityType, in IEnumerable<TItem> val, ISolidityTypeEncoder<TItem> itemEncoder)
        {
            var encoder = new ArrayEncoder<TItem>(itemEncoder);
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<string> LoadEncoder(string solidityType, in string val)
        {
            var encoder = new StringEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<Address> LoadEncoder(string solidityType, in Address val)
        {
            var encoder = new AddressEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<bool> LoadEncoder(string solidityType, in bool val)
        {
            var encoder = new BoolEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<sbyte> LoadEncoder(string solidityType, in sbyte val)
        {
            var encoder = new Int8Encoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<byte> LoadEncoder(string solidityType, in byte val)
        {
            var encoder = new UInt8Encoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<short> LoadEncoder(string solidityType, in short val)
        {
            var encoder = new Int16Encoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<ushort> LoadEncoder(string solidityType, in ushort val)
        {
            var encoder = new UInt16Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<int> LoadEncoder(string solidityType, in int val)
        {
            var encoder = new Int32Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<uint> LoadEncoder(string solidityType, in uint val)
        {
            var encoder = new UInt32Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<long> LoadEncoder(string solidityType, in long val)
        {
            var encoder = new Int64Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<ulong> LoadEncoder(string solidityType, in ulong val)
        {
            var encoder = new UInt64Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<BigInteger> LoadEncoder(string solidityType, in BigInteger val)
        {
            var encoder = new Int256Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static ISolidityTypeEncoder<UInt256> LoadEncoder(string solidityType, in UInt256 val)
        {
            var encoder = new UInt256Encoder();
            encoder.SetTypeInfo(SolidityTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }
    }

}
