using System;

namespace SolCodeGen.AbiEncoding
{
    public interface IAbiTypeEncoder
    {
        int GetEncodedSize();

        /// <summary>
        /// Encodes and writes the value to the buffer, then returns the buffer 
        /// with this position/cursor incremented to where the next writer should
        /// start at.
        /// </summary>
        Span<byte> Encode(Span<byte> buffer);
    }

    public interface IAbiTypeEncoder<TVal> : IAbiTypeEncoder
    {
        void SetValue(in TVal val);
        void SetTypeInfo(AbiTypeInfo info);
        ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out TVal val);
    }

    public abstract class AbiTypeEncoder<TVal> : IAbiTypeEncoder<TVal>
    {
        protected AbiTypeInfo _info;
        protected TVal _val;

        public virtual void SetTypeInfo(AbiTypeInfo info)
        {
            _info = info;
        }

        public virtual void SetValue(in TVal val)
        {
            _val = val;
        }

        public abstract ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out TVal val);

        public abstract Span<byte> Encode(Span<byte> buffer);
        public virtual int GetEncodedSize() => 32;


        protected Exception UnsupportedTypeException()
        {
            return new ArgumentException($"Encoder does not support solidity type category '{_info.Category}', type name: {_info.SolidityName}");
        }

        protected int PadLength(int len, int multiple)
        {
            return ((len - 1) / multiple + 1) * multiple;
        }
    }

}
