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
        public static DecodeDelegate<TItem[]> GetArrayDecoder<TItem>(IAbiTypeEncoder<TItem> itemEncoder)
        {
            void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out TItem[] val)
            {
                DecoderFactory.Decode(solidityType, ref buffer, out val, itemEncoder);
            }
            return Decode;
        }

        public static void Decode<TItem>(string solidityType, ref ReadOnlySpan<byte> buffer, out TItem[] val, IAbiTypeEncoder<TItem> itemEncoder)
        {
            Decode(solidityType, ref buffer, out IEnumerable<TItem> items, itemEncoder);
            val = items is TItem[] arr ? arr : items.ToArray();
        }

        public static void Decode<TItem>(string solidityType, ref ReadOnlySpan<byte> buffer, out IEnumerable<TItem> val, IAbiTypeEncoder<TItem> itemEncoder)
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
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out byte[] val)
        {
            Decode(solidityType, ref buffer, out IEnumerable<byte> bytes);
            val = bytes is byte[] arr ? arr : bytes.ToArray();
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out IEnumerable<byte> val)
        {
            var into = AbiTypeMap.GetSolidityTypeInfo(solidityType);
            switch (into.Category)
            {
                case SolidityTypeCategory.Bytes:
                    {
                        var encoder = new BytesEncoder();
                        encoder.SetTypeInfo(into);
                        buffer = encoder.Decode(buffer, out val);
                        break;
                    }
                case SolidityTypeCategory.BytesM:
                    {
                        var encoder = new BytesMEncoder();
                        encoder.SetTypeInfo(into);
                        buffer = encoder.Decode(buffer, out val);
                        break;
                    }
                case SolidityTypeCategory.DynamicArray:
                    {
                        var encoder = new DynamicArrayEncoder<byte>(new UInt8Encoder());
                        encoder.SetTypeInfo(into);
                        buffer = encoder.Decode(buffer, out val);
                        break;
                    }
                case SolidityTypeCategory.FixedArray:
                    {
                        var encoder = new FixedArrayEncoder<byte>(new UInt8Encoder());
                        encoder.SetTypeInfo(into);
                        buffer = encoder.Decode(buffer, out val);
                        break;
                    }
                default:
                    throw new ArgumentException($"Encoder factor method for byte arrays called with type '{into.Category}'");
            }
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out string val)
        {
            var encoder = new StringEncoder();
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out Address val)
        {
            var encoder = new AddressEncoder();
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out bool val)
        {
            var encoder = new BoolEncoder();
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out sbyte val)
        {
            var encoder = new Int8Encoder();
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out byte val)
        {
            var encoder = new UInt8Encoder();
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out short val)
        {
            var encoder = new Int16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out ushort val)
        {
            var encoder = new UInt16Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out int val)
        {
            var encoder = new Int32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out uint val)
        {
            var encoder = new UInt32Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out long val)
        {
            var encoder = new Int64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out ulong val)
        {
            var encoder = new UInt64Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out BigInteger val)
        {
            var encoder = new Int256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

        public static void Decode(string solidityType, ref ReadOnlySpan<byte> buffer, out UInt256 val)
        {
            NumberEncoder<UInt256> encoder = new UInt256Encoder();
            encoder.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo(solidityType));
            buffer = encoder.Decode(buffer, out val);
        }

    }

    public delegate void DecodeDelegate<TOut>(string solidityType, ref ReadOnlySpan<byte> buffer, out TOut result);

}
