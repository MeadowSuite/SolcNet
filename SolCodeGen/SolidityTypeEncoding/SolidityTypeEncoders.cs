using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SolCodeGen.SolidityTypeEncoding
{
    public static class SolidityTypeEncoders
    {
        public static void Encode(ref Span<byte> buffer, in Address val, string solidityType)
            => AddressEncoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in bool val, string solidityType)
            => BoolEncoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in sbyte val, string solidityType)
            => Int8Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in byte val, string solidityType)
            => UInt8Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in int val, string solidityType)
            => Int32Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in uint val, string solidityType)
            => UInt32Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in long val, string solidityType)
            => Int64Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in ulong val, string solidityType)
            => UInt64Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in BigInteger val, string solidityType)
            => Int256Encoder.Instance.Encode(ref buffer, val, solidityType);

        public static void Encode(ref Span<byte> buffer, in UInt256 val, string solidityType)
            => UInt256Encoder.Instance.Encode(ref buffer, val, solidityType);
    }

    public abstract class SolidityTypeEncoder<TVal> where TVal : struct
    {
        public string[] SolidityTypes;

        public SolidityTypeEncoder(params string[] solidityTypes)
        {
            SolidityTypes = solidityTypes;
        }

        public abstract int GetEncodedSize(in TVal val, string solidityType);
        public abstract void Encode(ref Span<byte> buffer, in TVal val, string solidityType);

    }

    public class BoolEncoder : SolidityTypeEncoder<bool>
    {
        public static readonly BoolEncoder Instance = new BoolEncoder();

        public BoolEncoder() : base("bool") { }

        public override void Encode(ref Span<byte> buffer, in bool val, string solidityType)
        {
            buffer[31] = val ? (byte)1 : (byte)0;
            buffer = buffer.Slice(32);
        }

        public override int GetEncodedSize(in bool val, string solidityType) => 32;
    }

    public class AddressEncoder : SolidityTypeEncoder<Address>
    {
        public static readonly AddressEncoder Instance = new AddressEncoder();

        public AddressEncoder() : base("address") { }

        public override void Encode(ref Span<byte> buffer, in Address val, string solidityType)
        {
            var addr = val;
            MemoryMarshal.Write(buffer.Slice(12), ref addr);
            buffer = buffer.Slice(32);
        }

        public override int GetEncodedSize(in Address val, string solidityType) => 32;
    }

}
