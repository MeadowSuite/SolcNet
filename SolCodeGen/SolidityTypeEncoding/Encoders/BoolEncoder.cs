using System;

namespace SolCodeGen.SolidityTypeEncoding.Encoders
{
    public class BoolEncoder : SolidityTypeEncoder<bool>
    {
        public override Span<byte> Encode(Span<byte> buffer)
        {
            buffer[31] = _val ? (byte)1 : (byte)0;
            return buffer.Slice(32);
        }
    }


}
