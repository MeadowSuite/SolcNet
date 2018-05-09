
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SolCodeGen.SolidityTypes
{

    [StructLayout(LayoutKind.Sequential)]
    public struct UInt24 : IEquatable<UInt24>, IComparable<UInt24>
    {
        public const int SIZE = 3;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3;

        public static implicit operator BigInteger(UInt24 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt24(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt24(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt24(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt24(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt24(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt24(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt24(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt24 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt24 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt24 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt24 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt24 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt24 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt24 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt24 v) => (long)v.ToBigInteger();

        public static UInt24 operator %(UInt24 a, UInt24 b) => BigInteger.Remainder(a, b);
        public static UInt24 operator +(UInt24 a, UInt24 b) => BigInteger.Add(a, b);
        public static UInt24 operator -(UInt24 a, UInt24 b) => BigInteger.Min(a, b);
        public static UInt24 operator *(UInt24 a, UInt24 b) => BigInteger.Multiply(a, b);
        public static UInt24 operator /(UInt24 a, UInt24 b) => BigInteger.Divide(a, b);
        public static UInt24 operator |(UInt24 a, UInt24 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt24 operator &(UInt24 a, UInt24 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt24 operator ^(UInt24 a, UInt24 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt24 operator >>(UInt24 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt24 operator <<(UInt24 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt24 a, UInt24 b) => a.Equals(b);
        public static bool operator !=(UInt24 a, UInt24 b) => !a.Equals(b);
        public static bool operator >(UInt24 a, UInt24 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt24 a, UInt24 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt24 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt24>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt24 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt24 other ? Equals(other) : false;
        }

        public int CompareTo(UInt24 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt40 : IEquatable<UInt40>, IComparable<UInt40>
    {
        public const int SIZE = 5;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5;

        public static implicit operator BigInteger(UInt40 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt40(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt40(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt40(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt40(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt40(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt40(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt40(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt40 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt40 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt40 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt40 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt40 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt40 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt40 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt40 v) => (long)v.ToBigInteger();

        public static UInt40 operator %(UInt40 a, UInt40 b) => BigInteger.Remainder(a, b);
        public static UInt40 operator +(UInt40 a, UInt40 b) => BigInteger.Add(a, b);
        public static UInt40 operator -(UInt40 a, UInt40 b) => BigInteger.Min(a, b);
        public static UInt40 operator *(UInt40 a, UInt40 b) => BigInteger.Multiply(a, b);
        public static UInt40 operator /(UInt40 a, UInt40 b) => BigInteger.Divide(a, b);
        public static UInt40 operator |(UInt40 a, UInt40 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt40 operator &(UInt40 a, UInt40 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt40 operator ^(UInt40 a, UInt40 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt40 operator >>(UInt40 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt40 operator <<(UInt40 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt40 a, UInt40 b) => a.Equals(b);
        public static bool operator !=(UInt40 a, UInt40 b) => !a.Equals(b);
        public static bool operator >(UInt40 a, UInt40 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt40 a, UInt40 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt40 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt40>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt40 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt40 other ? Equals(other) : false;
        }

        public int CompareTo(UInt40 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt48 : IEquatable<UInt48>, IComparable<UInt48>
    {
        public const int SIZE = 6;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6;

        public static implicit operator BigInteger(UInt48 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt48(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt48(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt48(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt48(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt48(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt48(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt48(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt48 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt48 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt48 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt48 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt48 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt48 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt48 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt48 v) => (long)v.ToBigInteger();

        public static UInt48 operator %(UInt48 a, UInt48 b) => BigInteger.Remainder(a, b);
        public static UInt48 operator +(UInt48 a, UInt48 b) => BigInteger.Add(a, b);
        public static UInt48 operator -(UInt48 a, UInt48 b) => BigInteger.Min(a, b);
        public static UInt48 operator *(UInt48 a, UInt48 b) => BigInteger.Multiply(a, b);
        public static UInt48 operator /(UInt48 a, UInt48 b) => BigInteger.Divide(a, b);
        public static UInt48 operator |(UInt48 a, UInt48 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt48 operator &(UInt48 a, UInt48 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt48 operator ^(UInt48 a, UInt48 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt48 operator >>(UInt48 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt48 operator <<(UInt48 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt48 a, UInt48 b) => a.Equals(b);
        public static bool operator !=(UInt48 a, UInt48 b) => !a.Equals(b);
        public static bool operator >(UInt48 a, UInt48 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt48 a, UInt48 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt48 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt48>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt48 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt48 other ? Equals(other) : false;
        }

        public int CompareTo(UInt48 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt56 : IEquatable<UInt56>, IComparable<UInt56>
    {
        public const int SIZE = 7;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7;

        public static implicit operator BigInteger(UInt56 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt56(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt56(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt56(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt56(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt56(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt56(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt56(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt56 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt56 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt56 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt56 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt56 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt56 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt56 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt56 v) => (long)v.ToBigInteger();

        public static UInt56 operator %(UInt56 a, UInt56 b) => BigInteger.Remainder(a, b);
        public static UInt56 operator +(UInt56 a, UInt56 b) => BigInteger.Add(a, b);
        public static UInt56 operator -(UInt56 a, UInt56 b) => BigInteger.Min(a, b);
        public static UInt56 operator *(UInt56 a, UInt56 b) => BigInteger.Multiply(a, b);
        public static UInt56 operator /(UInt56 a, UInt56 b) => BigInteger.Divide(a, b);
        public static UInt56 operator |(UInt56 a, UInt56 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt56 operator &(UInt56 a, UInt56 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt56 operator ^(UInt56 a, UInt56 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt56 operator >>(UInt56 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt56 operator <<(UInt56 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt56 a, UInt56 b) => a.Equals(b);
        public static bool operator !=(UInt56 a, UInt56 b) => !a.Equals(b);
        public static bool operator >(UInt56 a, UInt56 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt56 a, UInt56 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt56 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt56>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt56 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt56 other ? Equals(other) : false;
        }

        public int CompareTo(UInt56 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt72 : IEquatable<UInt72>, IComparable<UInt72>
    {
        public const int SIZE = 9;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9;

        public static implicit operator BigInteger(UInt72 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt72(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt72(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt72(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt72(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt72(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt72(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt72(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt72 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt72 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt72 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt72 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt72 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt72 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt72 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt72 v) => (long)v.ToBigInteger();

        public static UInt72 operator %(UInt72 a, UInt72 b) => BigInteger.Remainder(a, b);
        public static UInt72 operator +(UInt72 a, UInt72 b) => BigInteger.Add(a, b);
        public static UInt72 operator -(UInt72 a, UInt72 b) => BigInteger.Min(a, b);
        public static UInt72 operator *(UInt72 a, UInt72 b) => BigInteger.Multiply(a, b);
        public static UInt72 operator /(UInt72 a, UInt72 b) => BigInteger.Divide(a, b);
        public static UInt72 operator |(UInt72 a, UInt72 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt72 operator &(UInt72 a, UInt72 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt72 operator ^(UInt72 a, UInt72 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt72 operator >>(UInt72 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt72 operator <<(UInt72 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt72 a, UInt72 b) => a.Equals(b);
        public static bool operator !=(UInt72 a, UInt72 b) => !a.Equals(b);
        public static bool operator >(UInt72 a, UInt72 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt72 a, UInt72 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt72 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt72>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt72 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt72 other ? Equals(other) : false;
        }

        public int CompareTo(UInt72 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt80 : IEquatable<UInt80>, IComparable<UInt80>
    {
        public const int SIZE = 10;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10;

        public static implicit operator BigInteger(UInt80 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt80(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt80(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt80(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt80(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt80(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt80(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt80(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt80 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt80 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt80 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt80 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt80 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt80 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt80 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt80 v) => (long)v.ToBigInteger();

        public static UInt80 operator %(UInt80 a, UInt80 b) => BigInteger.Remainder(a, b);
        public static UInt80 operator +(UInt80 a, UInt80 b) => BigInteger.Add(a, b);
        public static UInt80 operator -(UInt80 a, UInt80 b) => BigInteger.Min(a, b);
        public static UInt80 operator *(UInt80 a, UInt80 b) => BigInteger.Multiply(a, b);
        public static UInt80 operator /(UInt80 a, UInt80 b) => BigInteger.Divide(a, b);
        public static UInt80 operator |(UInt80 a, UInt80 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt80 operator &(UInt80 a, UInt80 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt80 operator ^(UInt80 a, UInt80 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt80 operator >>(UInt80 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt80 operator <<(UInt80 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt80 a, UInt80 b) => a.Equals(b);
        public static bool operator !=(UInt80 a, UInt80 b) => !a.Equals(b);
        public static bool operator >(UInt80 a, UInt80 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt80 a, UInt80 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt80 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt80>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt80 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt80 other ? Equals(other) : false;
        }

        public int CompareTo(UInt80 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt88 : IEquatable<UInt88>, IComparable<UInt88>
    {
        public const int SIZE = 11;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11;

        public static implicit operator BigInteger(UInt88 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt88(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt88(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt88(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt88(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt88(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt88(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt88(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt88 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt88 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt88 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt88 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt88 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt88 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt88 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt88 v) => (long)v.ToBigInteger();

        public static UInt88 operator %(UInt88 a, UInt88 b) => BigInteger.Remainder(a, b);
        public static UInt88 operator +(UInt88 a, UInt88 b) => BigInteger.Add(a, b);
        public static UInt88 operator -(UInt88 a, UInt88 b) => BigInteger.Min(a, b);
        public static UInt88 operator *(UInt88 a, UInt88 b) => BigInteger.Multiply(a, b);
        public static UInt88 operator /(UInt88 a, UInt88 b) => BigInteger.Divide(a, b);
        public static UInt88 operator |(UInt88 a, UInt88 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt88 operator &(UInt88 a, UInt88 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt88 operator ^(UInt88 a, UInt88 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt88 operator >>(UInt88 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt88 operator <<(UInt88 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt88 a, UInt88 b) => a.Equals(b);
        public static bool operator !=(UInt88 a, UInt88 b) => !a.Equals(b);
        public static bool operator >(UInt88 a, UInt88 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt88 a, UInt88 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt88 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt88>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt88 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt88 other ? Equals(other) : false;
        }

        public int CompareTo(UInt88 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt96 : IEquatable<UInt96>, IComparable<UInt96>
    {
        public const int SIZE = 12;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12;

        public static implicit operator BigInteger(UInt96 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt96(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt96(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt96(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt96(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt96(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt96(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt96(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt96 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt96 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt96 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt96 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt96 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt96 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt96 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt96 v) => (long)v.ToBigInteger();

        public static UInt96 operator %(UInt96 a, UInt96 b) => BigInteger.Remainder(a, b);
        public static UInt96 operator +(UInt96 a, UInt96 b) => BigInteger.Add(a, b);
        public static UInt96 operator -(UInt96 a, UInt96 b) => BigInteger.Min(a, b);
        public static UInt96 operator *(UInt96 a, UInt96 b) => BigInteger.Multiply(a, b);
        public static UInt96 operator /(UInt96 a, UInt96 b) => BigInteger.Divide(a, b);
        public static UInt96 operator |(UInt96 a, UInt96 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt96 operator &(UInt96 a, UInt96 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt96 operator ^(UInt96 a, UInt96 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt96 operator >>(UInt96 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt96 operator <<(UInt96 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt96 a, UInt96 b) => a.Equals(b);
        public static bool operator !=(UInt96 a, UInt96 b) => !a.Equals(b);
        public static bool operator >(UInt96 a, UInt96 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt96 a, UInt96 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt96 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt96>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt96 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt96 other ? Equals(other) : false;
        }

        public int CompareTo(UInt96 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt104 : IEquatable<UInt104>, IComparable<UInt104>
    {
        public const int SIZE = 13;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13;

        public static implicit operator BigInteger(UInt104 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt104(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt104(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt104(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt104(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt104(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt104(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt104(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt104 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt104 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt104 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt104 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt104 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt104 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt104 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt104 v) => (long)v.ToBigInteger();

        public static UInt104 operator %(UInt104 a, UInt104 b) => BigInteger.Remainder(a, b);
        public static UInt104 operator +(UInt104 a, UInt104 b) => BigInteger.Add(a, b);
        public static UInt104 operator -(UInt104 a, UInt104 b) => BigInteger.Min(a, b);
        public static UInt104 operator *(UInt104 a, UInt104 b) => BigInteger.Multiply(a, b);
        public static UInt104 operator /(UInt104 a, UInt104 b) => BigInteger.Divide(a, b);
        public static UInt104 operator |(UInt104 a, UInt104 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt104 operator &(UInt104 a, UInt104 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt104 operator ^(UInt104 a, UInt104 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt104 operator >>(UInt104 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt104 operator <<(UInt104 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt104 a, UInt104 b) => a.Equals(b);
        public static bool operator !=(UInt104 a, UInt104 b) => !a.Equals(b);
        public static bool operator >(UInt104 a, UInt104 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt104 a, UInt104 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt104 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt104>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt104 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt104 other ? Equals(other) : false;
        }

        public int CompareTo(UInt104 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt112 : IEquatable<UInt112>, IComparable<UInt112>
    {
        public const int SIZE = 14;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14;

        public static implicit operator BigInteger(UInt112 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt112(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt112(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt112(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt112(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt112(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt112(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt112(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt112 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt112 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt112 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt112 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt112 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt112 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt112 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt112 v) => (long)v.ToBigInteger();

        public static UInt112 operator %(UInt112 a, UInt112 b) => BigInteger.Remainder(a, b);
        public static UInt112 operator +(UInt112 a, UInt112 b) => BigInteger.Add(a, b);
        public static UInt112 operator -(UInt112 a, UInt112 b) => BigInteger.Min(a, b);
        public static UInt112 operator *(UInt112 a, UInt112 b) => BigInteger.Multiply(a, b);
        public static UInt112 operator /(UInt112 a, UInt112 b) => BigInteger.Divide(a, b);
        public static UInt112 operator |(UInt112 a, UInt112 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt112 operator &(UInt112 a, UInt112 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt112 operator ^(UInt112 a, UInt112 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt112 operator >>(UInt112 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt112 operator <<(UInt112 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt112 a, UInt112 b) => a.Equals(b);
        public static bool operator !=(UInt112 a, UInt112 b) => !a.Equals(b);
        public static bool operator >(UInt112 a, UInt112 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt112 a, UInt112 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt112 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt112>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt112 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt112 other ? Equals(other) : false;
        }

        public int CompareTo(UInt112 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt120 : IEquatable<UInt120>, IComparable<UInt120>
    {
        public const int SIZE = 15;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15;

        public static implicit operator BigInteger(UInt120 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt120(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt120(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt120(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt120(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt120(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt120(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt120(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt120 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt120 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt120 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt120 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt120 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt120 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt120 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt120 v) => (long)v.ToBigInteger();

        public static UInt120 operator %(UInt120 a, UInt120 b) => BigInteger.Remainder(a, b);
        public static UInt120 operator +(UInt120 a, UInt120 b) => BigInteger.Add(a, b);
        public static UInt120 operator -(UInt120 a, UInt120 b) => BigInteger.Min(a, b);
        public static UInt120 operator *(UInt120 a, UInt120 b) => BigInteger.Multiply(a, b);
        public static UInt120 operator /(UInt120 a, UInt120 b) => BigInteger.Divide(a, b);
        public static UInt120 operator |(UInt120 a, UInt120 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt120 operator &(UInt120 a, UInt120 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt120 operator ^(UInt120 a, UInt120 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt120 operator >>(UInt120 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt120 operator <<(UInt120 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt120 a, UInt120 b) => a.Equals(b);
        public static bool operator !=(UInt120 a, UInt120 b) => !a.Equals(b);
        public static bool operator >(UInt120 a, UInt120 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt120 a, UInt120 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt120 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt120>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt120 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt120 other ? Equals(other) : false;
        }

        public int CompareTo(UInt120 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt128 : IEquatable<UInt128>, IComparable<UInt128>
    {
        public const int SIZE = 16;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16;

        public static implicit operator BigInteger(UInt128 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt128(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt128(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt128(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt128(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt128(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt128(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt128(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt128 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt128 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt128 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt128 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt128 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt128 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt128 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt128 v) => (long)v.ToBigInteger();

        public static UInt128 operator %(UInt128 a, UInt128 b) => BigInteger.Remainder(a, b);
        public static UInt128 operator +(UInt128 a, UInt128 b) => BigInteger.Add(a, b);
        public static UInt128 operator -(UInt128 a, UInt128 b) => BigInteger.Min(a, b);
        public static UInt128 operator *(UInt128 a, UInt128 b) => BigInteger.Multiply(a, b);
        public static UInt128 operator /(UInt128 a, UInt128 b) => BigInteger.Divide(a, b);
        public static UInt128 operator |(UInt128 a, UInt128 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt128 operator &(UInt128 a, UInt128 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt128 operator ^(UInt128 a, UInt128 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt128 operator >>(UInt128 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt128 operator <<(UInt128 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt128 a, UInt128 b) => a.Equals(b);
        public static bool operator !=(UInt128 a, UInt128 b) => !a.Equals(b);
        public static bool operator >(UInt128 a, UInt128 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt128 a, UInt128 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt128 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt128>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt128 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt128 other ? Equals(other) : false;
        }

        public int CompareTo(UInt128 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt136 : IEquatable<UInt136>, IComparable<UInt136>
    {
        public const int SIZE = 17;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17;

        public static implicit operator BigInteger(UInt136 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt136(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt136(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt136(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt136(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt136(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt136(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt136(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt136 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt136 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt136 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt136 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt136 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt136 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt136 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt136 v) => (long)v.ToBigInteger();

        public static UInt136 operator %(UInt136 a, UInt136 b) => BigInteger.Remainder(a, b);
        public static UInt136 operator +(UInt136 a, UInt136 b) => BigInteger.Add(a, b);
        public static UInt136 operator -(UInt136 a, UInt136 b) => BigInteger.Min(a, b);
        public static UInt136 operator *(UInt136 a, UInt136 b) => BigInteger.Multiply(a, b);
        public static UInt136 operator /(UInt136 a, UInt136 b) => BigInteger.Divide(a, b);
        public static UInt136 operator |(UInt136 a, UInt136 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt136 operator &(UInt136 a, UInt136 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt136 operator ^(UInt136 a, UInt136 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt136 operator >>(UInt136 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt136 operator <<(UInt136 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt136 a, UInt136 b) => a.Equals(b);
        public static bool operator !=(UInt136 a, UInt136 b) => !a.Equals(b);
        public static bool operator >(UInt136 a, UInt136 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt136 a, UInt136 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt136 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt136>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt136 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt136 other ? Equals(other) : false;
        }

        public int CompareTo(UInt136 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt144 : IEquatable<UInt144>, IComparable<UInt144>
    {
        public const int SIZE = 18;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18;

        public static implicit operator BigInteger(UInt144 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt144(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt144(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt144(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt144(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt144(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt144(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt144(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt144 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt144 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt144 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt144 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt144 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt144 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt144 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt144 v) => (long)v.ToBigInteger();

        public static UInt144 operator %(UInt144 a, UInt144 b) => BigInteger.Remainder(a, b);
        public static UInt144 operator +(UInt144 a, UInt144 b) => BigInteger.Add(a, b);
        public static UInt144 operator -(UInt144 a, UInt144 b) => BigInteger.Min(a, b);
        public static UInt144 operator *(UInt144 a, UInt144 b) => BigInteger.Multiply(a, b);
        public static UInt144 operator /(UInt144 a, UInt144 b) => BigInteger.Divide(a, b);
        public static UInt144 operator |(UInt144 a, UInt144 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt144 operator &(UInt144 a, UInt144 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt144 operator ^(UInt144 a, UInt144 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt144 operator >>(UInt144 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt144 operator <<(UInt144 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt144 a, UInt144 b) => a.Equals(b);
        public static bool operator !=(UInt144 a, UInt144 b) => !a.Equals(b);
        public static bool operator >(UInt144 a, UInt144 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt144 a, UInt144 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt144 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt144>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt144 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt144 other ? Equals(other) : false;
        }

        public int CompareTo(UInt144 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt152 : IEquatable<UInt152>, IComparable<UInt152>
    {
        public const int SIZE = 19;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19;

        public static implicit operator BigInteger(UInt152 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt152(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt152(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt152(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt152(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt152(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt152(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt152(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt152 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt152 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt152 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt152 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt152 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt152 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt152 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt152 v) => (long)v.ToBigInteger();

        public static UInt152 operator %(UInt152 a, UInt152 b) => BigInteger.Remainder(a, b);
        public static UInt152 operator +(UInt152 a, UInt152 b) => BigInteger.Add(a, b);
        public static UInt152 operator -(UInt152 a, UInt152 b) => BigInteger.Min(a, b);
        public static UInt152 operator *(UInt152 a, UInt152 b) => BigInteger.Multiply(a, b);
        public static UInt152 operator /(UInt152 a, UInt152 b) => BigInteger.Divide(a, b);
        public static UInt152 operator |(UInt152 a, UInt152 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt152 operator &(UInt152 a, UInt152 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt152 operator ^(UInt152 a, UInt152 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt152 operator >>(UInt152 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt152 operator <<(UInt152 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt152 a, UInt152 b) => a.Equals(b);
        public static bool operator !=(UInt152 a, UInt152 b) => !a.Equals(b);
        public static bool operator >(UInt152 a, UInt152 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt152 a, UInt152 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt152 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt152>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt152 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt152 other ? Equals(other) : false;
        }

        public int CompareTo(UInt152 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt160 : IEquatable<UInt160>, IComparable<UInt160>
    {
        public const int SIZE = 20;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20;

        public static implicit operator BigInteger(UInt160 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt160(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt160(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt160(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt160(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt160(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt160(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt160(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt160 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt160 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt160 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt160 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt160 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt160 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt160 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt160 v) => (long)v.ToBigInteger();

        public static UInt160 operator %(UInt160 a, UInt160 b) => BigInteger.Remainder(a, b);
        public static UInt160 operator +(UInt160 a, UInt160 b) => BigInteger.Add(a, b);
        public static UInt160 operator -(UInt160 a, UInt160 b) => BigInteger.Min(a, b);
        public static UInt160 operator *(UInt160 a, UInt160 b) => BigInteger.Multiply(a, b);
        public static UInt160 operator /(UInt160 a, UInt160 b) => BigInteger.Divide(a, b);
        public static UInt160 operator |(UInt160 a, UInt160 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt160 operator &(UInt160 a, UInt160 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt160 operator ^(UInt160 a, UInt160 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt160 operator >>(UInt160 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt160 operator <<(UInt160 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt160 a, UInt160 b) => a.Equals(b);
        public static bool operator !=(UInt160 a, UInt160 b) => !a.Equals(b);
        public static bool operator >(UInt160 a, UInt160 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt160 a, UInt160 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt160 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt160>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt160 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt160 other ? Equals(other) : false;
        }

        public int CompareTo(UInt160 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt168 : IEquatable<UInt168>, IComparable<UInt168>
    {
        public const int SIZE = 21;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21;

        public static implicit operator BigInteger(UInt168 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt168(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt168(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt168(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt168(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt168(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt168(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt168(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt168 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt168 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt168 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt168 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt168 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt168 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt168 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt168 v) => (long)v.ToBigInteger();

        public static UInt168 operator %(UInt168 a, UInt168 b) => BigInteger.Remainder(a, b);
        public static UInt168 operator +(UInt168 a, UInt168 b) => BigInteger.Add(a, b);
        public static UInt168 operator -(UInt168 a, UInt168 b) => BigInteger.Min(a, b);
        public static UInt168 operator *(UInt168 a, UInt168 b) => BigInteger.Multiply(a, b);
        public static UInt168 operator /(UInt168 a, UInt168 b) => BigInteger.Divide(a, b);
        public static UInt168 operator |(UInt168 a, UInt168 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt168 operator &(UInt168 a, UInt168 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt168 operator ^(UInt168 a, UInt168 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt168 operator >>(UInt168 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt168 operator <<(UInt168 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt168 a, UInt168 b) => a.Equals(b);
        public static bool operator !=(UInt168 a, UInt168 b) => !a.Equals(b);
        public static bool operator >(UInt168 a, UInt168 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt168 a, UInt168 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt168 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt168>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt168 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt168 other ? Equals(other) : false;
        }

        public int CompareTo(UInt168 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt176 : IEquatable<UInt176>, IComparable<UInt176>
    {
        public const int SIZE = 22;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22;

        public static implicit operator BigInteger(UInt176 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt176(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt176(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt176(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt176(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt176(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt176(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt176(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt176 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt176 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt176 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt176 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt176 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt176 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt176 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt176 v) => (long)v.ToBigInteger();

        public static UInt176 operator %(UInt176 a, UInt176 b) => BigInteger.Remainder(a, b);
        public static UInt176 operator +(UInt176 a, UInt176 b) => BigInteger.Add(a, b);
        public static UInt176 operator -(UInt176 a, UInt176 b) => BigInteger.Min(a, b);
        public static UInt176 operator *(UInt176 a, UInt176 b) => BigInteger.Multiply(a, b);
        public static UInt176 operator /(UInt176 a, UInt176 b) => BigInteger.Divide(a, b);
        public static UInt176 operator |(UInt176 a, UInt176 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt176 operator &(UInt176 a, UInt176 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt176 operator ^(UInt176 a, UInt176 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt176 operator >>(UInt176 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt176 operator <<(UInt176 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt176 a, UInt176 b) => a.Equals(b);
        public static bool operator !=(UInt176 a, UInt176 b) => !a.Equals(b);
        public static bool operator >(UInt176 a, UInt176 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt176 a, UInt176 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt176 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt176>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt176 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt176 other ? Equals(other) : false;
        }

        public int CompareTo(UInt176 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt184 : IEquatable<UInt184>, IComparable<UInt184>
    {
        public const int SIZE = 23;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23;

        public static implicit operator BigInteger(UInt184 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt184(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt184(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt184(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt184(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt184(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt184(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt184(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt184 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt184 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt184 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt184 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt184 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt184 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt184 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt184 v) => (long)v.ToBigInteger();

        public static UInt184 operator %(UInt184 a, UInt184 b) => BigInteger.Remainder(a, b);
        public static UInt184 operator +(UInt184 a, UInt184 b) => BigInteger.Add(a, b);
        public static UInt184 operator -(UInt184 a, UInt184 b) => BigInteger.Min(a, b);
        public static UInt184 operator *(UInt184 a, UInt184 b) => BigInteger.Multiply(a, b);
        public static UInt184 operator /(UInt184 a, UInt184 b) => BigInteger.Divide(a, b);
        public static UInt184 operator |(UInt184 a, UInt184 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt184 operator &(UInt184 a, UInt184 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt184 operator ^(UInt184 a, UInt184 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt184 operator >>(UInt184 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt184 operator <<(UInt184 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt184 a, UInt184 b) => a.Equals(b);
        public static bool operator !=(UInt184 a, UInt184 b) => !a.Equals(b);
        public static bool operator >(UInt184 a, UInt184 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt184 a, UInt184 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt184 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt184>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt184 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt184 other ? Equals(other) : false;
        }

        public int CompareTo(UInt184 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt192 : IEquatable<UInt192>, IComparable<UInt192>
    {
        public const int SIZE = 24;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24;

        public static implicit operator BigInteger(UInt192 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt192(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt192(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt192(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt192(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt192(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt192(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt192(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt192 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt192 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt192 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt192 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt192 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt192 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt192 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt192 v) => (long)v.ToBigInteger();

        public static UInt192 operator %(UInt192 a, UInt192 b) => BigInteger.Remainder(a, b);
        public static UInt192 operator +(UInt192 a, UInt192 b) => BigInteger.Add(a, b);
        public static UInt192 operator -(UInt192 a, UInt192 b) => BigInteger.Min(a, b);
        public static UInt192 operator *(UInt192 a, UInt192 b) => BigInteger.Multiply(a, b);
        public static UInt192 operator /(UInt192 a, UInt192 b) => BigInteger.Divide(a, b);
        public static UInt192 operator |(UInt192 a, UInt192 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt192 operator &(UInt192 a, UInt192 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt192 operator ^(UInt192 a, UInt192 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt192 operator >>(UInt192 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt192 operator <<(UInt192 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt192 a, UInt192 b) => a.Equals(b);
        public static bool operator !=(UInt192 a, UInt192 b) => !a.Equals(b);
        public static bool operator >(UInt192 a, UInt192 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt192 a, UInt192 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt192 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt192>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt192 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt192 other ? Equals(other) : false;
        }

        public int CompareTo(UInt192 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt200 : IEquatable<UInt200>, IComparable<UInt200>
    {
        public const int SIZE = 25;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25;

        public static implicit operator BigInteger(UInt200 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt200(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt200(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt200(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt200(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt200(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt200(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt200(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt200 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt200 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt200 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt200 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt200 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt200 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt200 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt200 v) => (long)v.ToBigInteger();

        public static UInt200 operator %(UInt200 a, UInt200 b) => BigInteger.Remainder(a, b);
        public static UInt200 operator +(UInt200 a, UInt200 b) => BigInteger.Add(a, b);
        public static UInt200 operator -(UInt200 a, UInt200 b) => BigInteger.Min(a, b);
        public static UInt200 operator *(UInt200 a, UInt200 b) => BigInteger.Multiply(a, b);
        public static UInt200 operator /(UInt200 a, UInt200 b) => BigInteger.Divide(a, b);
        public static UInt200 operator |(UInt200 a, UInt200 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt200 operator &(UInt200 a, UInt200 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt200 operator ^(UInt200 a, UInt200 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt200 operator >>(UInt200 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt200 operator <<(UInt200 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt200 a, UInt200 b) => a.Equals(b);
        public static bool operator !=(UInt200 a, UInt200 b) => !a.Equals(b);
        public static bool operator >(UInt200 a, UInt200 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt200 a, UInt200 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt200 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt200>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt200 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt200 other ? Equals(other) : false;
        }

        public int CompareTo(UInt200 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt208 : IEquatable<UInt208>, IComparable<UInt208>
    {
        public const int SIZE = 26;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26;

        public static implicit operator BigInteger(UInt208 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt208(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt208(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt208(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt208(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt208(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt208(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt208(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt208 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt208 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt208 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt208 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt208 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt208 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt208 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt208 v) => (long)v.ToBigInteger();

        public static UInt208 operator %(UInt208 a, UInt208 b) => BigInteger.Remainder(a, b);
        public static UInt208 operator +(UInt208 a, UInt208 b) => BigInteger.Add(a, b);
        public static UInt208 operator -(UInt208 a, UInt208 b) => BigInteger.Min(a, b);
        public static UInt208 operator *(UInt208 a, UInt208 b) => BigInteger.Multiply(a, b);
        public static UInt208 operator /(UInt208 a, UInt208 b) => BigInteger.Divide(a, b);
        public static UInt208 operator |(UInt208 a, UInt208 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt208 operator &(UInt208 a, UInt208 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt208 operator ^(UInt208 a, UInt208 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt208 operator >>(UInt208 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt208 operator <<(UInt208 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt208 a, UInt208 b) => a.Equals(b);
        public static bool operator !=(UInt208 a, UInt208 b) => !a.Equals(b);
        public static bool operator >(UInt208 a, UInt208 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt208 a, UInt208 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt208 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt208>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt208 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt208 other ? Equals(other) : false;
        }

        public int CompareTo(UInt208 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt216 : IEquatable<UInt216>, IComparable<UInt216>
    {
        public const int SIZE = 27;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26, P27;

        public static implicit operator BigInteger(UInt216 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt216(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt216(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt216(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt216(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt216(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt216(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt216(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt216 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt216 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt216 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt216 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt216 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt216 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt216 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt216 v) => (long)v.ToBigInteger();

        public static UInt216 operator %(UInt216 a, UInt216 b) => BigInteger.Remainder(a, b);
        public static UInt216 operator +(UInt216 a, UInt216 b) => BigInteger.Add(a, b);
        public static UInt216 operator -(UInt216 a, UInt216 b) => BigInteger.Min(a, b);
        public static UInt216 operator *(UInt216 a, UInt216 b) => BigInteger.Multiply(a, b);
        public static UInt216 operator /(UInt216 a, UInt216 b) => BigInteger.Divide(a, b);
        public static UInt216 operator |(UInt216 a, UInt216 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt216 operator &(UInt216 a, UInt216 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt216 operator ^(UInt216 a, UInt216 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt216 operator >>(UInt216 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt216 operator <<(UInt216 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt216 a, UInt216 b) => a.Equals(b);
        public static bool operator !=(UInt216 a, UInt216 b) => !a.Equals(b);
        public static bool operator >(UInt216 a, UInt216 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt216 a, UInt216 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt216 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt216>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt216 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt216 other ? Equals(other) : false;
        }

        public int CompareTo(UInt216 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt224 : IEquatable<UInt224>, IComparable<UInt224>
    {
        public const int SIZE = 28;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26, P27, P28;

        public static implicit operator BigInteger(UInt224 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt224(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt224(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt224(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt224(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt224(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt224(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt224(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt224 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt224 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt224 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt224 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt224 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt224 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt224 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt224 v) => (long)v.ToBigInteger();

        public static UInt224 operator %(UInt224 a, UInt224 b) => BigInteger.Remainder(a, b);
        public static UInt224 operator +(UInt224 a, UInt224 b) => BigInteger.Add(a, b);
        public static UInt224 operator -(UInt224 a, UInt224 b) => BigInteger.Min(a, b);
        public static UInt224 operator *(UInt224 a, UInt224 b) => BigInteger.Multiply(a, b);
        public static UInt224 operator /(UInt224 a, UInt224 b) => BigInteger.Divide(a, b);
        public static UInt224 operator |(UInt224 a, UInt224 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt224 operator &(UInt224 a, UInt224 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt224 operator ^(UInt224 a, UInt224 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt224 operator >>(UInt224 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt224 operator <<(UInt224 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt224 a, UInt224 b) => a.Equals(b);
        public static bool operator !=(UInt224 a, UInt224 b) => !a.Equals(b);
        public static bool operator >(UInt224 a, UInt224 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt224 a, UInt224 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt224 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt224>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt224 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt224 other ? Equals(other) : false;
        }

        public int CompareTo(UInt224 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt232 : IEquatable<UInt232>, IComparable<UInt232>
    {
        public const int SIZE = 29;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26, P27, P28, P29;

        public static implicit operator BigInteger(UInt232 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt232(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt232(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt232(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt232(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt232(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt232(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt232(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt232 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt232 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt232 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt232 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt232 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt232 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt232 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt232 v) => (long)v.ToBigInteger();

        public static UInt232 operator %(UInt232 a, UInt232 b) => BigInteger.Remainder(a, b);
        public static UInt232 operator +(UInt232 a, UInt232 b) => BigInteger.Add(a, b);
        public static UInt232 operator -(UInt232 a, UInt232 b) => BigInteger.Min(a, b);
        public static UInt232 operator *(UInt232 a, UInt232 b) => BigInteger.Multiply(a, b);
        public static UInt232 operator /(UInt232 a, UInt232 b) => BigInteger.Divide(a, b);
        public static UInt232 operator |(UInt232 a, UInt232 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt232 operator &(UInt232 a, UInt232 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt232 operator ^(UInt232 a, UInt232 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt232 operator >>(UInt232 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt232 operator <<(UInt232 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt232 a, UInt232 b) => a.Equals(b);
        public static bool operator !=(UInt232 a, UInt232 b) => !a.Equals(b);
        public static bool operator >(UInt232 a, UInt232 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt232 a, UInt232 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt232 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt232>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt232 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt232 other ? Equals(other) : false;
        }

        public int CompareTo(UInt232 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt240 : IEquatable<UInt240>, IComparable<UInt240>
    {
        public const int SIZE = 30;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26, P27, P28, P29, P30;

        public static implicit operator BigInteger(UInt240 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt240(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt240(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt240(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt240(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt240(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt240(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt240(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt240 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt240 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt240 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt240 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt240 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt240 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt240 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt240 v) => (long)v.ToBigInteger();

        public static UInt240 operator %(UInt240 a, UInt240 b) => BigInteger.Remainder(a, b);
        public static UInt240 operator +(UInt240 a, UInt240 b) => BigInteger.Add(a, b);
        public static UInt240 operator -(UInt240 a, UInt240 b) => BigInteger.Min(a, b);
        public static UInt240 operator *(UInt240 a, UInt240 b) => BigInteger.Multiply(a, b);
        public static UInt240 operator /(UInt240 a, UInt240 b) => BigInteger.Divide(a, b);
        public static UInt240 operator |(UInt240 a, UInt240 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt240 operator &(UInt240 a, UInt240 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt240 operator ^(UInt240 a, UInt240 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt240 operator >>(UInt240 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt240 operator <<(UInt240 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt240 a, UInt240 b) => a.Equals(b);
        public static bool operator !=(UInt240 a, UInt240 b) => !a.Equals(b);
        public static bool operator >(UInt240 a, UInt240 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt240 a, UInt240 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt240 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt240>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt240 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt240 other ? Equals(other) : false;
        }

        public int CompareTo(UInt240 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    public struct UInt248 : IEquatable<UInt248>, IComparable<UInt248>
    {
        public const int SIZE = 31;
        public static BigInteger MaxValue = BigInteger.Pow(2, SIZE * 8);

        readonly byte P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12, P13, P14, P15, P16, P17, P18, P19, P20, P21, P22, P23, P24, P25, P26, P27, P28, P29, P30, P31;

        public static implicit operator BigInteger(UInt248 v) => new BigInteger(v.GetSpan().ToArray());
        public static implicit operator UInt248(BigInteger v) => FromBigInteger(v);
        public static implicit operator UInt248(byte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt248(sbyte v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt248(uint v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt248(int v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt248(ulong v) => FromBigInteger(new BigInteger(v));
        public static implicit operator UInt248(long v) => FromBigInteger(new BigInteger(v));

        public static explicit operator byte(UInt248 v) => (byte)v.ToBigInteger();
        public static explicit operator sbyte(UInt248 v) => (sbyte)v.ToBigInteger();
        public static explicit operator short(UInt248 v) => (short)v.ToBigInteger();
        public static explicit operator ushort(UInt248 v) => (ushort)v.ToBigInteger();
        public static explicit operator uint(UInt248 v) => (uint)v.ToBigInteger();
        public static explicit operator int(UInt248 v) => (int)v.ToBigInteger();
        public static explicit operator ulong(UInt248 v) => (ulong)v.ToBigInteger();
        public static explicit operator long(UInt248 v) => (long)v.ToBigInteger();

        public static UInt248 operator %(UInt248 a, UInt248 b) => BigInteger.Remainder(a, b);
        public static UInt248 operator +(UInt248 a, UInt248 b) => BigInteger.Add(a, b);
        public static UInt248 operator -(UInt248 a, UInt248 b) => BigInteger.Min(a, b);
        public static UInt248 operator *(UInt248 a, UInt248 b) => BigInteger.Multiply(a, b);
        public static UInt248 operator /(UInt248 a, UInt248 b) => BigInteger.Divide(a, b);
        public static UInt248 operator |(UInt248 a, UInt248 b) => a.ToBigInteger() | b.ToBigInteger();
        public static UInt248 operator &(UInt248 a, UInt248 b) => a.ToBigInteger() & b.ToBigInteger();
        public static UInt248 operator ^(UInt248 a, UInt248 b) => a.ToBigInteger() ^ b.ToBigInteger();
        public static UInt248 operator >>(UInt248 a, int shift) => a.ToBigInteger() >> shift;
        public static UInt248 operator <<(UInt248 a, int shift) => a.ToBigInteger() << shift;

        public static bool operator ==(UInt248 a, UInt248 b) => a.Equals(b);
        public static bool operator !=(UInt248 a, UInt248 b) => !a.Equals(b);
        public static bool operator >(UInt248 a, UInt248 b) => a.CompareTo(b) > 0;
        public static bool operator <(UInt248 a, UInt248 b) => a.CompareTo(b) < 0;

        public BigInteger ToBigInteger()
        {
            Span<byte> bytes = stackalloc byte[SIZE + 1];
            GetSpan().CopyTo(bytes);
            return new BigInteger(bytes.ToArray());
        }

        public static UInt248 FromBigInteger(BigInteger bigInt)
        {
            if (bigInt < 0)
            {
                throw new OverflowException();
            }
            else if (bigInt > MaxValue)
            {
                throw new OverflowException();
            }
            var arr = bigInt.ToByteArray();
            if (arr.Length < SIZE)
            {
                Array.Resize(ref arr, SIZE);
            }
            return MemoryMarshal.Read<UInt248>(arr);
        }

        public unsafe ReadOnlySpan<byte> GetSpan()
        {
            var thisPtr = Unsafe.AsPointer(ref Unsafe.AsRef(this));
            var thisSpan = new ReadOnlySpan<byte>(thisPtr, SIZE);
            return thisSpan;
        }

        public bool Equals(UInt248 other)
        {
            return GetSpan().SequenceEqual(other.GetSpan());
        }

        public override bool Equals(object obj)
        {
            return obj is UInt248 other ? Equals(other) : false;
        }

        public int CompareTo(UInt248 other)
        {
            return GetSpan().SequenceCompareTo(other.GetSpan());
        }

        public override int GetHashCode()
        {
            int hash = 0;
            var arr = GetSpan();
            for (int i = 0; i < arr.Length; i++)
                hash ^= arr[i].GetHashCode();
            return hash;
        }
        
        public override string ToString()
        {
            return ToBigInteger().ToString();
        }

    }


}
