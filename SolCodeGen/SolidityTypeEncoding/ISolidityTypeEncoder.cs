using System;

namespace SolCodeGen.SolidityTypeEncoding
{
    public interface ISolidityTypeEncoder
    {
        int GetEncodedSize();

        /// <summary>
        /// Encodes and writes the value to the buffer, then returns the buffer 
        /// with this position/cursor incremented to where the next writer should
        /// start at.
        /// </summary>
        Span<byte> Encode(Span<byte> buffer);
    }

    public interface ISolidityTypeEncoder<TVal> : ISolidityTypeEncoder
    {
        void SetValue(in TVal val);
        void SetTypeInfo(SolidityTypeInfo info);
    }

    public abstract class SolidityTypeEncoder<TVal> : ISolidityTypeEncoder<TVal>
    {
        protected SolidityTypeInfo _info;
        protected TVal _val;

        public virtual void SetTypeInfo(SolidityTypeInfo info)
        {
            _info = info;
        }

        public virtual void SetValue(in TVal val)
        {
            _val = val;
        }

        public abstract Span<byte> Encode(Span<byte> buffer);
        public virtual int GetEncodedSize() => 32;


        protected Exception UnsupportedTypeException()
        {
            return new ArgumentException($"Encoder does not support solidity type category '{_info.Category}', type name: {_info.SolidityName}");
        }
    }

}
