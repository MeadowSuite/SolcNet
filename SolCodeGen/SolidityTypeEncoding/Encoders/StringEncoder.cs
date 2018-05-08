using System;
using System.Text;

namespace SolCodeGen.SolidityTypeEncoding.Encoders
{
    public class StringEncoder : SolidityTypeEncoder<string>
    {
        public override int GetEncodedSize()
        {
            var len = Encoding.UTF8.GetByteCount(_val);
            int padded = PadLength(len, 32);
            return 32 + padded;
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            Span<byte> utf8 = Encoding.UTF8.GetBytes(_val);
            int len = utf8.Length;
            buffer = UInt256Encoder.EncodeUnchecked(buffer, len);
            utf8.CopyTo(buffer);
            int padded = PadLength(len, 32);
            return buffer.Slice(padded);
        }
    }

}
