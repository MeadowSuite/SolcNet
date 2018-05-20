using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SolCodeGen.AbiEncoding.Encoders
{

    public abstract class NumberEncoder<TInt> : AbiTypeEncoder<TInt> where TInt : struct
    {
        protected static Dictionary<string, (int ByteSize, BigInteger MaxValue)> _unsignedTypeSizes
            = new Dictionary<string, (int ByteSize, BigInteger MaxValue)>(UInt256.SIZE);

        protected static Dictionary<string, (int ByteSize, BigInteger MaxValue, BigInteger MinValue)> _signedTypeSizes
            = new Dictionary<string, (int ByteSize, BigInteger MaxValue, BigInteger MinValue)>(UInt256.SIZE);

        static NumberEncoder()
        {
            for (var i = 1; i <= UInt256.SIZE; i++)
            {
                var bitSize = i * 8;
                var maxIntValue = BigInteger.Pow(2, bitSize);
                _unsignedTypeSizes.Add("uint" + bitSize, (i, maxIntValue));
                _signedTypeSizes.Add("int" + bitSize, (i, maxIntValue / 2 + 1, -maxIntValue / 2));
            }
        }

        protected abstract bool Signed { get; }
        protected abstract BigInteger AsBigInteger { get; }

        public BigInteger MaxValue => Signed ? _signedTypeSizes[_info.SolidityName].MaxValue : _unsignedTypeSizes[_info.SolidityName].MaxValue;
        public BigInteger MinValue => Signed ? _signedTypeSizes[_info.SolidityName].MinValue : 0;

        public override void SetValue(in TInt val)
        {
            base.SetValue(val);
            var bigInt = AsBigInteger;
            if (bigInt > MaxValue)
            {
                throw IntOverflow();
            }
            if (Signed && bigInt < MinValue)
            {
                throw IntUnderflow();
            }
        }

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            Encode(buff.HeadCursor);
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

        public void Encode(Span<byte> buff)
        {
            var byteSize = _info.PrimitiveTypeByteSize;
            Span<byte> valBytes = stackalloc byte[Unsafe.SizeOf<TInt>()];
            MemoryMarshal.Write(valBytes, ref _val);

            //Span<byte> valBytes = MemoryMarshal.Cast<TInt, byte>(new[] { _val });
            //Unsafe.WriteUnaligned(ref valBytes[0], _val);

            if (BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < byteSize; i++)
                {
                    buff[31 - i] = valBytes[i];
                }
            }
            else
            {
                for (var i = 0; i < byteSize; i++)
                {
                    buff[31 - byteSize + i] = valBytes[i];
                }
            }
        }

        protected Exception IntOverflow()
        {
            return new OverflowException($"Max value for type '{_info}' is {MaxValue}, was given {_val}");
        }

        protected Exception IntUnderflow()
        {
            return new OverflowException($"Min value for type '{_info}' is {MinValue}, was given {_val}");
        }

        public override void Decode(ref AbiDecodeBuffer buff, out TInt val)
        {
            Decode(buff.HeadCursor, out val);
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

        public void Decode(ReadOnlySpan<byte> buffer, out TInt val)
        {
            Span<TInt> num = new TInt[1];
            Span<byte> byteView = MemoryMarshal.Cast<TInt, byte>(num);

            var byteSize = _info.PrimitiveTypeByteSize;
            var padSize = UInt256.SIZE - byteSize;
            if (BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < byteSize; i++)
                {
                    byteView[byteSize - i - 1] = buffer[i + padSize];
                }
            }
            else
            {
                for (var i = 0; i < byteSize; i++)
                {
                    byteView[i] = buffer[i + padSize];
                }
            }

            // data validity check: should be padded with zero-bytes
            // Disabled - ganache liters this padding with garbage bytes
            /*
            for (var i = 0; i < padSize; i++)
            {
                if (buffer[i] != 0)
                {
                    throw new ArgumentException($"Invalid {_info.SolidityName} input data; should be {byteSize} bytes, left-padded with {UInt256.SIZE - byteSize} zero-bytes; received: " + buffer.Slice(0, 32).ToHexString());
                }
            }
            */

            val = num[0];
        }
    }


    public class Int8Encoder : AbiTypeEncoder<sbyte>
    {
        static readonly byte[] ZEROx31 = Enumerable.Repeat((byte)0, UInt256.SIZE - 1).ToArray();

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            buff.HeadCursor[UInt256.SIZE - 1] = unchecked((byte)_val);
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

        public override void Decode(ref AbiDecodeBuffer buff, out sbyte val)
        {
            if (!buff.HeadCursor.Slice(0, UInt256.SIZE - 1).SequenceEqual(ZEROx31))
            {
                throw new ArgumentException("Invalid int8 input data; should be 31 zeros followed by a int8/byte; received: " + buff.HeadCursor.Slice(0, UInt256.SIZE).ToHexString());
            }
            val = unchecked((sbyte)buff.HeadCursor[UInt256.SIZE - 1]);
            buff.IncrementHeadCursor(UInt256.SIZE);
        }
    }

    public class UInt8Encoder : AbiTypeEncoder<byte>
    {
        static readonly byte[] ZEROx31 = Enumerable.Repeat((byte)0, UInt256.SIZE - 1).ToArray();

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            buff.HeadCursor[UInt256.SIZE - 1] = _val;
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

        public override void Decode(ref AbiDecodeBuffer buff, out byte val)
        {
            // Disabled from ganache memory litering
            /*
            if (!buffer.Slice(0, 31).SequenceEqual(ZEROx31))
            {
                throw new ArgumentException("Invalid uint8 input data; should be 31 zeros followed by a uint8/byte; received: " + buffer.Slice(0, UInt256.SIZE).ToHexString());
            }
            */
            val = buff.HeadCursor[UInt256.SIZE - 1];
            buff.IncrementHeadCursor(UInt256.SIZE);
        }
    }

    public class Int16Encoder : NumberEncoder<short>
    {
        protected override bool Signed => true;
        protected override BigInteger AsBigInteger => _val;
    }

    public class UInt16Encoder : NumberEncoder<ushort>
    {
        protected override bool Signed => false;
        protected override BigInteger AsBigInteger => _val;
    }

    public class Int32Encoder : NumberEncoder<int>
    {
        protected override bool Signed => true;
        protected override BigInteger AsBigInteger => _val;
    }

    public class UInt32Encoder : NumberEncoder<uint>
    {
        protected override bool Signed => false;
        protected override BigInteger AsBigInteger => _val;
    }

    public class Int64Encoder : NumberEncoder<long>
    {
        protected override bool Signed => true;
        protected override BigInteger AsBigInteger => _val;
    }

    public class UInt64Encoder : NumberEncoder<ulong>
    {
        protected override bool Signed => false;
        protected override BigInteger AsBigInteger => _val;
    }

    public class Int256Encoder : NumberEncoder<BigInteger>
    {
        protected override bool Signed => true;
        protected override BigInteger AsBigInteger => _val;
        public override void Encode(ref AbiEncodeBuffer buff)
        {
            Span<byte> arr = _val.ToByteArray();
            arr.CopyTo(buff.HeadCursor.Slice(UInt256.SIZE - arr.Length));
            buff.IncrementHeadCursor(UInt256.SIZE);
        }
    }

    public class UInt256Encoder : NumberEncoder<UInt256>
    {
        protected override bool Signed => false;
        protected override BigInteger AsBigInteger => _val;

        public static UInt256Encoder Instance => UncheckedInstance.Value;
        static readonly Lazy<UInt256Encoder> UncheckedInstance = new Lazy<UInt256Encoder>(() => 
        {
            var inst = new UInt256Encoder();
            inst.SetTypeInfo(AbiTypeMap.GetSolidityTypeInfo("uint256"));
            return inst;
        });

        public override void SetValue(in UInt256 val)
        {
            // Skip unnecessary bounds check on max uint256 value.
            // An optimization only for this common type at the moment.
            if (_info.SolidityName == "uint256")
            {
                _val = val;
            }
            else
            {
                base.SetValue(val);
            }
        }

        /// <summary>
        /// Encodes a solidity 'uint256' (with no overflow checks since its the max value)
        /// </summary>
        public void Encode(ref AbiEncodeBuffer buff, in UInt256 val)
        {
            var encoder = UncheckedInstance.Value;
            encoder._val = val;
            encoder.Encode(ref buff);
        }

        public void Encode(Span<byte> data, in UInt256 val)
        {
            var encoder = UncheckedInstance.Value;
            encoder._val = val;
            encoder.Encode(data);
        }

        public void Decode(ReadOnlySpan<byte> buffer, out int val)
        {
            Decode(buffer, out UInt256 num);
            val = (int)num;
        }

        public void Decode(ref AbiDecodeBuffer buff, out int val)
        {
            Decode(ref buff, out UInt256 num);
            val = (int)num;
        }

    }


}
