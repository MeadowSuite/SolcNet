using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{

    [StructLayout(LayoutKind.Sequential)]
    public struct UInt256 : IComparable<UInt256>, IEquatable<UInt256>
    {
        public const int SIZE = 32;

        public static readonly UInt256 MaxValue = new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue);
        public static readonly UInt256 MinValue = 0;
        public static readonly UInt256 Zero = 0;

        // parts are big-endian
        readonly ulong Part1;
        readonly ulong Part2;
        readonly ulong Part3;
        readonly ulong Part4;

        public UInt256(byte[] arr) : this()
        {
            if (arr.Length < 32)
            {
                Array.Resize(ref arr, 32);
            }
            var pos = 0;
            FromByteArray(arr, ref pos, ref this);
        }

        public UInt256(BigInteger value) : this(value.ToByteArray())
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public UInt256(int value) : this()
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            unchecked
            {
                Part1 = (ulong)value;
            }
        }

        public UInt256(long value) : this()
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            unchecked
            {
                Part1 = (ulong)value;
            }
        }

        public UInt256(uint value) : this()
        {
            Part1 = value;
        }

        public UInt256(ulong value) : this()
        {
            Part1 = value;
        }

        public UInt256(ulong part1, ulong part2, ulong part3, ulong part4)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            Part4 = part4;
        }

        public UInt256(ref ulong part1, ref ulong part2, ref ulong part3, ref ulong part4)
        {
            Part1 = part1;
            Part2 = part2;
            Part3 = part3;
            Part4 = part4;
        }

        public static UInt256 DivRem(UInt256 dividend, UInt256 divisor, out UInt256 remainder)
        {
            BigInteger remainderBigInt;
            var result = new UInt256(BigInteger.DivRem(dividend.ToBigInteger(), divisor.ToBigInteger(), out remainderBigInt));
            remainder = new UInt256(remainderBigInt);
            return result;
        }

        public static UInt256 Pow(UInt256 value, int exponent) => new UInt256(BigInteger.Pow(value.ToBigInteger(), exponent));

        public static double Log(UInt256 value, double baseValue) => BigInteger.Log(value.ToBigInteger(), baseValue);


        public static explicit operator double(UInt256 value) => (double)value.ToBigInteger();
        public static explicit operator ulong(UInt256 value) => (ulong)value.ToBigInteger();
        public static explicit operator uint(UInt256 value) => (uint)value.ToBigInteger();
        public static explicit operator ushort(UInt256 value) => (ushort)value.ToBigInteger();
        public static explicit operator byte(UInt256 value) => (byte)value.ToBigInteger();

        public static implicit operator BigInteger(UInt256 value) => value.ToBigInteger();
        public static implicit operator UInt256(byte value) => new UInt256(value);
        public static implicit operator UInt256(int value) => new UInt256(value);
        public static implicit operator UInt256(long value) => new UInt256(value);
        public static implicit operator UInt256(sbyte value) => new UInt256(value);
        public static implicit operator UInt256(short value) => new UInt256(value);
        public static implicit operator UInt256(uint value) => new UInt256(value);
        public static implicit operator UInt256(ulong value) => new UInt256(value);
        public static implicit operator UInt256(ushort value) => new UInt256(value);


        public static Boolean operator !=(UInt256 left, UInt256 right) => !(left == right);
        public static Boolean operator ==(UInt256 left, UInt256 right) => (left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 == right.Part3) && (left.Part4 == right.Part4);
        public static UInt256 operator %(UInt256 dividend, UInt256 divisor) => new UInt256(dividend.ToBigInteger() % divisor.ToBigInteger());
        public static UInt256 operator +(UInt256 left, UInt256 right) => new UInt256(left.ToBigInteger() + right.ToBigInteger());
        public static UInt256 operator -(UInt256 left, UInt256 right) => new UInt256(left.ToBigInteger() - right.ToBigInteger());
        public static UInt256 operator *(UInt256 left, UInt256 right) => new UInt256(left.ToBigInteger() * right.ToBigInteger());
        public static UInt256 operator /(UInt256 dividend, UInt256 divisor) => new UInt256(dividend.ToBigInteger() / divisor.ToBigInteger());
        public static UInt256 operator ~(UInt256 value) => new UInt256(~value.Part1, ~value.Part2, ~value.Part3, ~value.Part4);
        public static UInt256 operator >>(UInt256 value, int shift) => new UInt256(value.ToBigInteger() >> shift);

        public static Boolean operator <(UInt256 left, UInt256 right)
        {
            if (left.Part1 < right.Part1)
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 < right.Part2))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 < right.Part3))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 == right.Part3) && (left.Part4 < right.Part4))
            {
                return true;
            }
            return false;
        }

        public static Boolean operator <=(UInt256 left, UInt256 right)
        {
            if (left.Part1 < right.Part1)
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 < right.Part2))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 < right.Part3))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 == right.Part3) && (left.Part4 < right.Part4))
            {
                return true;
            }
            return left == right;
        }

        public static Boolean operator >(UInt256 left, UInt256 right)
        {
            if (left.Part1 > right.Part1)
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 > right.Part2))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 > right.Part3))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 == right.Part3) && (left.Part4 > right.Part4))
            {
                return true;
            }

            return false;
        }

        public static Boolean operator >=(UInt256 left, UInt256 right)
        {
            if (left.Part1 > right.Part1)
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 > right.Part2))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 > right.Part3))
            {
                return true;
            }
            if ((left.Part1 == right.Part1) && (left.Part2 == right.Part2) && (left.Part3 == right.Part3) && (left.Part4 > right.Part4))
            {
                return true;
            }

            return left == right;
        }


        public static UInt256 Parse(String value) => new UInt256(BigInteger.Parse("0" + value));
        public static UInt256 Parse(String value, IFormatProvider provider) => new UInt256(BigInteger.Parse("0" + value, provider));
        public static UInt256 Parse(String value, NumberStyles style) => new UInt256(BigInteger.Parse("0" + value, style));
        public static UInt256 Parse(String value, NumberStyles style, IFormatProvider provider) => new UInt256(BigInteger.Parse("0" + value, style, provider));


        public int CompareTo(UInt256 other)
        {
            if (this == other)
            {
                return 0;
            }
            if (this < other)
            {
                return -1;
            }
            if (this > other)
            {
                return +1;
            }

            throw new Exception();
        }

        public override Boolean Equals(Object obj)
        {
            return obj is UInt256 other ? Equals(other) : false;
        }

        public bool Equals(UInt256 other)
        {
            return (other.Part1 == Part1) && (other.Part2 == Part2) && (other.Part3 == Part3) && (other.Part4 == Part4);
        }

        public override int GetHashCode()
        {
            return (Part1, Part2, Part3, Part4).GetHashCode();
        }

        public BigInteger ToBigInteger()
        {
            var buffer = new byte[33];
            int position = 0;
            ToByteArray(buffer, ref position, ref this);
            return new BigInteger(buffer);
        }

        public static void FromByteArray(Span<byte> buffer, ref int position, ref UInt256 num)
        {
            if (buffer.Length - position < 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            num = MemoryMarshal.Read<UInt256>(buffer.Slice(position));

            position += 32;
        }

        public static void ToByteArray(Span<byte> buffer, ref int position, ref UInt256 num)
        {
            if (buffer.Length - position < 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            MemoryMarshal.Write(buffer.Slice(position), ref num);

            position += 8;
        }

        public byte[] ToByteArray()
        {
            int position = 0;
            var buffer = new byte[32];
            ToByteArray(buffer, ref position, ref this);
            return buffer;
        }

        public static void ToByteArraySafe(byte[] buffer, ref int position, ref UInt256 num)
        {
            byte[] Order(byte[] value)
            {
                return BitConverter.IsLittleEndian ? value : value.Reverse().ToArray();
            }

            Buffer.BlockCopy(Order(BitConverter.GetBytes(num.Part1)), 0, buffer, position, 8);
            Buffer.BlockCopy(Order(BitConverter.GetBytes(num.Part2)), 0, buffer, position += 8, 8);
            Buffer.BlockCopy(Order(BitConverter.GetBytes(num.Part3)), 0, buffer, position += 8, 8);
            Buffer.BlockCopy(Order(BitConverter.GetBytes(num.Part4)), 0, buffer, position += 8, 8);
            position += 8;
        }

        public byte[] ToByteArraySafe()
        {
            var buffer = new byte[32];
            int pos = 0;
            ToByteArraySafe(buffer, ref pos, ref this);
            return buffer;
        }

        public override string ToString()
        {
            return ToBigInteger().ToString();
        }
        

    }

}