using System;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class BoolEncoder : AbiTypeEncoder<bool>
    {
        // Encoded the same as an uint8, where 1 is used for true and 0 for false

        public override Span<byte> Encode(Span<byte> buffer)
        {
            buffer[31] = _val ? (byte)1 : (byte)0;
            return buffer.Slice(32);
        }
    }


}
