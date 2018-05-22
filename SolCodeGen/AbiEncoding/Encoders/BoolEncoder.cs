using HoshoEthUtil;
using SolCodeGen.Utils;
using System;
using System.Linq;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class BoolEncoder : AbiTypeEncoder<bool>
    {
        // Encoded the same as an uint8, where 1 is used for true and 0 for false

        public override void Encode(ref AbiEncodeBuffer buffer)
        {
            buffer.HeadCursor[UInt256.SIZE - 1] = _val ? (byte)1 : (byte)0;
            buffer.IncrementHeadCursor(UInt256.SIZE);
        }

        static readonly byte[] ZEROx31 = Enumerable.Repeat((byte)0, UInt256.SIZE - 1).ToArray();

        public override void Decode(ref AbiDecodeBuffer buff, out bool val)
        {
            // Input data validity check: last byte should be either 0 or 1.
            switch (buff.HeadCursor[31])
            {
                case 0:
                    val = false;
                    break;
                case 1:
                    val = true;
                    break;
                default:
                    throw Error(buff.HeadCursor);

            }

#if ZERO_BYTE_CHECKS
            // Input data validity check: all but the last byte should be zero.
            // Span<byte>.SequenceEquals should use the fast native memory slab comparer.
            if (!buff.HeadCursor.Slice(0, UInt256.SIZE - 1).SequenceEqual(ZEROx31))
            {
                throw Error(buff.HeadCursor.Slice(0, UInt256.SIZE - 1));
            }
#endif

            buff.IncrementHeadCursor(UInt256.SIZE);

            Exception Error(ReadOnlySpan<byte> payload)
            {
                return new ArgumentException("Invalid boolean input data; should be 31 zeros followed by a 1 or 0; received: " + payload.Slice(0, UInt256.SIZE).ToHexString());
            }
        }
    }


}
