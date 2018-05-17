using System;
using System.IO;
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
            buffer = UInt256Encoder.Encode(buffer, 32); // encode starting position (immediately after this 32-byte pointer)
            buffer = UInt256Encoder.Encode(buffer, utf8.Length);
            utf8.CopyTo(buffer);
            int padded = PadLength(utf8.Length, 32);
            return buffer.Slice(padded);
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out string val)
        {
            // Obtain our starting position for our data.
            buffer = UInt256Encoder.Decode(buffer, out var startingPosition);

            // We advanced our pointer 32-bytes already, so we account for that
            startingPosition -= 32;

            // We advance the pointer to our starting position
            buffer = buffer.Slice((int)startingPosition);

            // Decode our string length.
            buffer = UInt256Encoder.Decode(buffer, out var strLen);
            if (strLen > int.MaxValue)
            {
                throw new ArgumentException($"String input data is invalid: the byte length prefix is {strLen} which is unlikely to be intended");
            }


            var bytes = new byte[(int)strLen];
            buffer.Slice(0, bytes.Length).CopyTo(bytes);
            val = Encoding.UTF8.GetString(bytes);
            int bodyLen = PadLength(bytes.Length, 32);

            // data validity check: should be right-padded with zero bytes
            // Disabled - ganache liters this padding with garbage bytes
            /*
            for (var i = bytes.Length; i < bodyLen; i++)
            {
                if (buffer[i] != 0)
                {
                    throw new ArgumentException($"Invalid string input data; should be {bytes.Length} followed by {bodyLen - bytes.Length} zero-bytes");
                }
            }
            */

            return buffer.Slice(bodyLen);
        }
    }

}
