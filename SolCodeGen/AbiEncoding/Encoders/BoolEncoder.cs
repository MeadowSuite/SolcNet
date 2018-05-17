using System;
using System.Linq;

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

        static readonly byte[] ZEROx31 = Enumerable.Repeat((byte)0, 31).ToArray();

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out bool val)
        {
            // Input data validity check: last byte should be either 0 or 1.
            switch (buffer[31])
            {
                case 0:
                    val = false;
                    break;
                case 1:
                    val = true;
                    break;
                default:
                    throw Error(buffer);

            }

            // Input data validity check: all but the last byte should be zero.
            // Span<byte>.SequenceEquals should use the fast native memory slab comparer.
            // Disabled - ganache liters this padding with garbage bytes
            /*
            if (!buffer.Slice(0, 31).SequenceEqual(ZEROx31))
            {
                throw Error(buffer);
            }
            */

            return buffer.Slice(32);

            Exception Error(ReadOnlySpan<byte> data)
            {
                return new ArgumentException("Invalid boolean input data; should be 31 zeros followed by a 1 or 0; received: " + data.Slice(0, 32).ToHexString());
            }
        }
    }


}
