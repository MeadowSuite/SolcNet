using System;
using System.IO;
using System.Runtime.InteropServices;
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
            int padded = PadLength(len, UInt256.SIZE);
            return UInt256.SIZE * 2 + padded;
        }

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            Span<byte> utf8 = Encoding.UTF8.GetBytes(_val);

            // write data offset position into header
            int offset = buff.HeadLength + buff.DataAreaCursorPosition;
            UInt256Encoder.Instance.Encode(buff.HeadCursor, offset);
            buff.IncrementHeadCursor(UInt256.SIZE);

            // write payload len into data buffer
            int len = utf8.Length;
            UInt256Encoder.Instance.Encode(buff.DataAreaCursor, len);
            buff.IncrementDataCursor(UInt256.SIZE);

            // write payload into data buffer
            utf8.CopyTo(buff.DataAreaCursor);
            int padded = PadLength(len, UInt256.SIZE);
            buff.IncrementDataCursor(padded);
        }

        public override void Decode(ref AbiDecodeBuffer buff, out string val)
        {
            // Read the next header int which is the offset to the start of the data
            // in the data payload area.
            UInt256Encoder.Instance.Decode(buff.HeadCursor, out int startingPosition);

            // The first int in our offset of data area is the length of the rest of the payload.
            var encodedLength = buff.Buffer.Slice(startingPosition, UInt256.SIZE);
            UInt256Encoder.Instance.Decode(encodedLength, out int byteLen);

            // Read the actual payload from the data area
            var encodedString = buff.Buffer.Slice(startingPosition + UInt256.SIZE, byteLen);
            var bytes = new byte[byteLen];
            encodedString.CopyTo(bytes);
            val = Encoding.UTF8.GetString(bytes);
            int bodyLen = PadLength(bytes.Length, UInt256.SIZE);

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

            buff.IncrementHeadCursor(UInt256.SIZE);

        }
    }

}
