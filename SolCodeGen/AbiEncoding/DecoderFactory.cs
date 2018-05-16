using SolCodeGen.AbiEncoding.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SolCodeGen.AbiEncoding
{
    public static class DecoderFactory
    {
        public static ReadOnlySpan<byte> Decode<TItem>(string solidityType, ReadOnlySpan<byte> buffer, out TItem[] val, IAbiTypeEncoder<TItem> itemEncoder)
        {
            var buff = Decode(solidityType, buffer, out IEnumerable<TItem> items, itemEncoder);
            val = items is TItem[] arr ? arr : items.ToArray();
            return buff;
        }

        public static ReadOnlySpan<byte> Decode<TItem>(string solidityType, ReadOnlySpan<byte> buffer, out IEnumerable<TItem> val, IAbiTypeEncoder<TItem> itemEncoder)
        {
            var info = AbiTypeMap.GetSolidityTypeInfo(solidityType);
            AbiTypeEncoder<IEnumerable<TItem>> encoder;
            switch (info.Category)
            {
                case SolidityTypeCategory.DynamicArray:
                    encoder = new DynamicArrayEncoder<TItem>(itemEncoder);
                    break;
                case SolidityTypeCategory.FixedArray:
                    encoder = new FixedArrayEncoder<TItem>(itemEncoder);
                    break;
                default:
                    throw new ArgumentException($"Encoder factory for array types was called with a type '{info.Category}'");

            }
            encoder.SetTypeInfo(info);
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out byte[] val)
        {
            var buff = Decode(solidityType, buffer, out IEnumerable<byte> bytes);
            val = bytes is byte[] arr ? arr : bytes.ToArray();
            return buff;
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out IEnumerable<byte> val)
        {
            var into = AbiTypeMap.GetSolidityTypeInfo(solidityType);
            switch (into.Category)
            {
                case SolidityTypeCategory.Bytes:
                    {
                        var encoder = new BytesEncoder();
                        encoder.SetTypeInfo(into);
                        return encoder.Decode(buffer, out val);
                    }
                case SolidityTypeCategory.BytesM:
                    {
                        var encoder = new BytesMEncoder();
                        encoder.SetTypeInfo(into);
                        return encoder.Decode(buffer, out val);
                    }
                case SolidityTypeCategory.DynamicArray:
                    {
                        var encoder = new DynamicArrayEncoder<byte>(new UInt8Encoder());
                        encoder.SetTypeInfo(into);
                        return encoder.Decode(buffer, out val);
                    }
                case SolidityTypeCategory.FixedArray:
                    {
                        var encoder = new FixedArrayEncoder<byte>(new UInt8Encoder());
                        encoder.SetTypeInfo(into);
                        return encoder.Decode(buffer, out val);
                    }
                default:
                    throw new ArgumentException($"Encoder factor method for byte arrays called with type '{into.Category}'");
            }
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out string val)
        {
            var encoder = new StringEncoder();
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out Address val)
        {
            var encoder = new AddressEncoder();
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out bool val)
        {
            var encoder = new BoolEncoder();
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out sbyte val)
        {
            var encoder = new Int8Encoder();
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out byte val)
        {
            var encoder = new UInt8Encoder();
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out short val)
        {
            var encoder = new Int16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out ushort val)
        {
            var encoder = new UInt16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out int val)
        {
            var encoder = new Int32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out uint val)
        {
            var encoder = new UInt32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out long val)
        {
            var encoder = new Int64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out ulong val)
        {
            var encoder = new UInt64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out BigInteger val)
        {
            var encoder = new Int256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public static ReadOnlySpan<byte> Decode(string solidityType, ReadOnlySpan<byte> buffer, out UInt256 val)
        {
            NumberEncoder<UInt256> encoder = new UInt256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            return encoder.Decode(buffer, out val);
        }

        public delegate ReadOnlySpan<byte> DecodeDelegate<TOut>(ReadOnlySpan<byte> buffer, out TOut result);

    }
}
