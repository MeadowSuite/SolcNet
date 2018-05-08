using System;
using System.Text;

namespace SolCodeGen.SolidityTypeEncoding.Encoders
{
    public class StringEncoder : SolidityTypeEncoder<string>
    {
        public override int GetEncodedSize()
        {
            return Encoding.UTF8.GetByteCount(_val);
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            Span<byte> utf8 = Encoding.UTF8.GetBytes(_val);
            int len = utf8.Length;
            buffer = UInt256Encoder.EncodeUnchecked(buffer, len);
            utf8.CopyTo(buffer);
            int padding = len % 32;
            return buffer.Slice(len + padding);
        }
    }

}
