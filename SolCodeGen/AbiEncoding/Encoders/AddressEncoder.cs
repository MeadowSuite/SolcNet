using System;
using System.Runtime.InteropServices;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class AddressEncoder : AbiTypeEncoder<Address>
    {
        // encoded the same way as an uint160

        public override Span<byte> Encode(Span<byte> buffer)
        {
            MemoryMarshal.Write(buffer.Slice(12), ref _val);
            return buffer.Slice(32);
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out Address val)
        {
            val = MemoryMarshal.Read<Address>(buffer);
            return buffer.Slice(32);
        }
    }

}
