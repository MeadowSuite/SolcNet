using System;
using System.Text;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class StringEncoder : AbiTypeEncoder<string>
    {
        // utf-8 encoded and this value is interpreted as of bytes type and encoded further.
        // Note that the length used in this subsequent encoding is the number of bytes of 
        // the utf-8 encoded string, not its number of characters.

        public override int GetEncodedSize()
        {
            var len = Encoding.UTF8.GetByteCount(_val);
            int padded = PadLength(len, 32);
            return 32 * 2 + padded;
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            Span<byte> utf8 = Encoding.UTF8.GetBytes(_val);
            var start = buffer;
            buffer = UInt256Encoder.EncodeUnchecked(buffer, 32);
            buffer = UInt256Encoder.EncodeUnchecked(buffer, utf8.Length);
            var testHex = start.Slice(0, start.Length - buffer.Length).ToHexString(hexPrefix: false);
            utf8.CopyTo(buffer);
            int padded = PadLength(utf8.Length, 32);
            return buffer.Slice(padded);
        }
    }

}
