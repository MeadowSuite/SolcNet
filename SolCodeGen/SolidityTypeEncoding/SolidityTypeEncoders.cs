using System;
using System.Runtime.InteropServices;

namespace SolCodeGen.SolidityTypeEncoding
{

    public interface ISolidityTypeEncoder
    {
        int GetEncodedSize();
        Span<byte> Encode(Span<byte> buffer);
    }

    public interface ISolidityTypeEncoder<TVal> : ISolidityTypeEncoder
    {
        void SetValue(in TVal val);
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
    }

    public class BoolEncoder : SolidityTypeEncoder<bool>
    {
        public override Span<byte> Encode(Span<byte> buffer)
        {
            buffer[31] = _val ? (byte)1 : (byte)0;
            return buffer.Slice(32);
        }
    }

    public class AddressEncoder : SolidityTypeEncoder<Address>
    {
        public override Span<byte> Encode(Span<byte> buffer)
        {
            MemoryMarshal.Write(buffer.Slice(12), ref _val);
            return buffer.Slice(32);
        }
    }

    public class StringEncoder : SolidityTypeEncoder<string>
    {
        public override Span<byte> Encode(Span<byte> buffer)
        {
            throw new NotImplementedException();
        }
    }

}
