using System;

namespace SolCodeGen.AbiEncoding
{

    public interface IAbiTypeEncoder
    {
        AbiTypeInfo TypeInfo { get; }

        void SetTypeInfo(AbiTypeInfo info);

        int GetEncodedSize();

        /// <summary>
        /// Encodes and writes the value to the buffer, then returns the buffer 
        /// with this position/cursor incremented to where the next writer should
        /// start at.
        /// </summary>
        void Encode(ref AbiEncodeBuffer buffer);
    }

    public interface IAbiTypeEncoder<TVal> : IAbiTypeEncoder
    {
        void SetValue(in TVal val);
        void Decode(ref AbiDecodeBuffer buff, out TVal val);
    }

    public abstract class AbiTypeEncoder<TVal> : IAbiTypeEncoder<TVal>
    {
        public AbiTypeInfo TypeInfo => _info;

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

        public abstract void Decode(ref AbiDecodeBuffer buff, out TVal val);
        public abstract void Encode(ref AbiEncodeBuffer buff);

        public virtual int GetEncodedSize() => UInt256.SIZE;

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
