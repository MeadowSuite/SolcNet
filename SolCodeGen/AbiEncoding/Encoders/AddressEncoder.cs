using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class AddressEncoder : AbiTypeEncoder<Address>
    {
        static readonly byte[] ZEROx12 = Enumerable.Repeat((byte)0, 12).ToArray();

        // encoded the same way as an uint160
        public override Span<byte> Encode(Span<byte> buffer)
        {
            MemoryMarshal.Write(buffer.Slice(12), ref _val);
            return buffer.Slice(32);
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out Address val)
        {
            // data validity check: 20 address bytes should be left-padded with 12 zero-bytes
            // Disabled - ganache liters this padding with garbage bytes
            /*
            if (!buffer.Slice(0, 12).SequenceEqual(ZEROx12))
            {
                throw new ArgumentException("Invalid address input data; should be 20 address bytes, left-padded with 12 zero-bytes; received: " + buffer.Slice(0, 32).ToHexString());
            }
            */
            val = MemoryMarshal.Read<Address>(buffer.Slice(12));
            return buffer.Slice(32);
        }
    }

}
