using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SolCodeGen.SolidityTypeEncoding
{

    public abstract class SolidityNumberEncoder<TInt> : SolidityTypeEncoder<TInt> where TInt : struct
    {
        protected static Dictionary<string, (int ByteSize, BigInteger MaxValue)> _unsignedTypeSizes
            = new Dictionary<string, (int ByteSize, BigInteger MaxValue)>(32);

        protected static Dictionary<string, (int ByteSize, BigInteger MaxValue, BigInteger MinValue)> _signedTypeSizes
            = new Dictionary<string, (int ByteSize, BigInteger MaxValue, BigInteger MinValue)>(32);

        static SolidityNumberEncoder()
        {
            for (var i = 1; i <= 32; i++)
            {
                var bitSize = i * 8;
                var maxIntValue = BigInteger.Pow(2, bitSize);
                _unsignedTypeSizes.Add("uint" + bitSize, (i, maxIntValue));
                _signedTypeSizes.Add("int" + bitSize, (i, maxIntValue / 2 + 1, -maxIntValue / 2));
            }
        }

        public SolidityNumberEncoder(bool signed, int byteRangeStart, int byteRangeEnd)
            : base(Enumerable
                  .Range(byteRangeStart, byteRangeEnd - byteRangeStart)
                  .Select(b => (signed ? "int" : "uint") + (b * 8))
                  .ToArray())
        {
            
        }

        public override int GetEncodedSize(in TInt val, string solidityType) => 32;

        protected void Encode(ref Span<byte> buffer, TInt val, string solidityType, in int byteSize)
        {
            var bufferOffset = 31 - byteSize;
            //var valBytes = new ReadOnlySpan<byte>(Unsafe.AsPointer(ref val), byteSize);
            var valBytes = MemoryMarshal.Cast<TInt, byte>(new[] { val });
            
            // TODO: implement support for bigendian systems
            for (var i = 0; i < byteSize; i++)
            {
                buffer[31 - i] = valBytes[i];
            }
            buffer = buffer.Slice(32);
        }

        protected Exception IntOverflow(in TInt val, string solidityType, BigInteger maxSize)
        {
            return new ArgumentException($"Max value for type '{solidityType}' is {maxSize}, was given {val}");
        }

        protected Exception IntUnderflow(in TInt val, string solidityType, BigInteger minSize)
        {
            return new ArgumentException($"Min value for type '{solidityType}' is {minSize}, was given {val}");
        }
    }


    public class Int8Encoder : SolidityTypeEncoder<sbyte>
    {
        public static readonly Int8Encoder Instance = new Int8Encoder();

        public Int8Encoder() : base("int8") { }

        public override int GetEncodedSize(in sbyte val, string solidityType) => 32;

        public override void Encode(ref Span<byte> buffer, in sbyte val, string solidityType)
        {
            buffer[31] = unchecked((byte)val);
            buffer = buffer.Slice(32);
        }
    }

    public class UInt8Encoder : SolidityTypeEncoder<byte>
    {
        public static readonly UInt8Encoder Instance = new UInt8Encoder();

        public UInt8Encoder() : base("uint8") { }

        public override int GetEncodedSize(in byte val, string solidityType) => 32;

        public override void Encode(ref Span<byte> buffer, in byte val, string solidityType)
        {
            buffer[31] = val;
            buffer = buffer.Slice(32);
        }
    }

    public class Int16Encoder : SolidityNumberEncoder<short>
    {
        public static readonly UInt16Encoder Instance = new UInt16Encoder();

        public Int16Encoder() : base(signed: true, 2, 2) { }

        public override void Encode(ref Span<byte> buffer, in short val, string solidityType)
        {
            Encode(ref buffer, val, solidityType, 2);
        }
    }

    public class UInt16Encoder : SolidityNumberEncoder<ushort>
    {
        public static readonly UInt16Encoder Instance = new UInt16Encoder();

        public UInt16Encoder() : base(signed: false, 2, 2) { }

        public override void Encode(ref Span<byte> buffer, in ushort val, string solidityType)
        {
            Encode(ref buffer, val, solidityType, 2);
        }
    }

    public class Int32Encoder : SolidityNumberEncoder<int>
    {
        public static readonly Int32Encoder Instance = new Int32Encoder();

        public Int32Encoder() : base(signed: true, 3, 4) { }

        public override void Encode(ref Span<byte> buffer, in int val, string solidityType)
        {
            var (byteSize, maxValue, minValue) = _signedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            else if (val < minValue)
                throw IntUnderflow(val, solidityType, minValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }

    public class UInt32Encoder : SolidityNumberEncoder<uint>
    {
        public static readonly UInt32Encoder Instance = new UInt32Encoder();

        public UInt32Encoder() : base(signed: false, 5, 8) { }

        public override void Encode(ref Span<byte> buffer, in uint val, string solidityType)
        {
            var (byteSize, maxValue) = _unsignedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }

    public class Int64Encoder : SolidityNumberEncoder<long>
    {
        public static readonly Int64Encoder Instance = new Int64Encoder();

        public Int64Encoder() : base(signed: false, 5, 8) { }

        public override void Encode(ref Span<byte> buffer, in long val, string solidityType)
        {
            var (byteSize, maxValue, minValue) = _signedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            else if (val < minValue)
                throw IntUnderflow(val, solidityType, minValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }

    public class UInt64Encoder : SolidityNumberEncoder<ulong>
    {
        public static readonly UInt64Encoder Instance = new UInt64Encoder();

        public UInt64Encoder() : base(signed: true, 5, 8) { }

        public override void Encode(ref Span<byte> buffer, in ulong val, string solidityType)
        {
            var (byteSize, maxValue) = _unsignedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }

    public class UInt256Encoder : SolidityNumberEncoder<UInt256>
    {
        public static readonly UInt256Encoder Instance = new UInt256Encoder();

        public UInt256Encoder() : base(signed: false, 9, 32) { }

        public override void Encode(ref Span<byte> buffer, in UInt256 val, string solidityType)
        {
            var (byteSize, maxValue) = _unsignedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }

    public class Int256Encoder : SolidityNumberEncoder<BigInteger>
    {
        public static readonly Int256Encoder Instance = new Int256Encoder();

        public Int256Encoder() : base(signed: true, 9, 32) { }

        public override void Encode(ref Span<byte> buffer, in BigInteger val, string solidityType)
        {
            var (byteSize, maxValue, minValue) = _signedTypeSizes[solidityType];
            if (val > maxValue)
                throw IntOverflow(val, solidityType, maxValue);
            else if (val < minValue)
                throw IntUnderflow(val, solidityType, minValue);
            Encode(ref buffer, val, solidityType, byteSize);
        }
    }


}
