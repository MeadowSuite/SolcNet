using SolCodeGen.AbiEncoding.Encoders;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SolCodeGen.AbiEncoding
{
    /// <summary>
    /// Generic-overloaded methods for easy access to typed encoders from generated contracts.
    /// Trade off of: Lots of code here for less code in the generated contracts, and less 
    /// dynamic runtime type checking.
    /// </summary>
    public static class EncoderFactory
    {
        // TODO: if we use the t4 generated UInt<M> types, use a t4 generator to create the corresponding LoadEncoder methods here...

        public static IAbiTypeEncoder<IEnumerable<TItem>> LoadEncoder<TItem>(string solidityType, in IEnumerable<TItem> val, IAbiTypeEncoder<TItem> itemEncoder)
        {
            var info = AbiTypeMap.GetSolidityTypeInfo(solidityType);
            IAbiTypeEncoder<IEnumerable<TItem>> encoder;
            switch(info.Category)
            {
                case SolidityTypeCategory.FixedArray:
                    encoder = new FixedArrayEncoder<TItem>(itemEncoder);
                    break;
                case SolidityTypeCategory.DynamicArray:
                    encoder = new DynamicArrayEncoder<TItem>(itemEncoder);
                    break;
                default:
                    throw new ArgumentException($"Encoder factory for array types was called with a type '{info.Category}'");
            }
            encoder.SetTypeInfo(info);
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<IEnumerable<byte>> LoadEncoder(string solidityType, in IEnumerable<byte> val)
        {
            var into = AbiTypeMap.GetSolidityTypeInfo(solidityType);
            switch(into.Category)
            {
                case SolidityTypeCategory.Bytes:
                    {
                        var encoder = new BytesEncoder();
                        encoder.SetTypeInfo(into);
                        encoder.SetValue(val);
                        return encoder;
                    }
                case SolidityTypeCategory.BytesM:
                    {
                        var encoder = new BytesMEncoder();
                        encoder.SetTypeInfo(into);
                        encoder.SetValue(val);
                        return encoder;
                    }
                case SolidityTypeCategory.DynamicArray:
                case SolidityTypeCategory.FixedArray:
                    {
                        var encoder = new FixedArrayEncoder<byte>(new UInt8Encoder());
                        encoder.SetTypeInfo(into);
                        encoder.SetValue(val);
                        return encoder;
                    }
                default:
                    throw new ArgumentException($"Encoder factor method for byte arrays called with type '{into.Category}'");
            }

        }

        public static IAbiTypeEncoder<string> LoadEncoder(string solidityType, in string val)
        {
            var encoder = new StringEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<Address> LoadEncoder(string solidityType, in Address val)
        {
            var encoder = new AddressEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<bool> LoadEncoder(string solidityType, in bool val)
        {
            var encoder = new BoolEncoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<sbyte> LoadEncoder(string solidityType, in sbyte val)
        {
            var encoder = new Int8Encoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<byte> LoadEncoder(string solidityType, in byte val)
        {
            var encoder = new UInt8Encoder();
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<short> LoadEncoder(string solidityType, in short val)
        {
            var encoder = new Int16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<ushort> LoadEncoder(string solidityType, in ushort val)
        {
            var encoder = new UInt16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<int> LoadEncoder(string solidityType, in int val)
        {
            var encoder = new Int32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<uint> LoadEncoder(string solidityType, in uint val)
        {
            var encoder = new UInt32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<long> LoadEncoder(string solidityType, in long val)
        {
            var encoder = new Int64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<ulong> LoadEncoder(string solidityType, in ulong val)
        {
            var encoder = new UInt64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<BigInteger> LoadEncoder(string solidityType, in BigInteger val)
        {
            var encoder = new Int256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }

        public static IAbiTypeEncoder<UInt256> LoadEncoder(string solidityType, in UInt256 val)
        {
            var encoder = new UInt256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            encoder.SetValue(val);
            return encoder;
        }
    }

}
