using System;
using System.Collections.Generic;
using System.Linq;

namespace SolCodeGen.AbiEncoding.Encoders
{

    public class BytesEncoder : AbiTypeEncoder<IEnumerable<byte>>
    {
        public override int GetEncodedSize()
        {
            if (_info.Category != SolidityTypeCategory.Bytes)
            {
                throw UnsupportedTypeException();
            }

            // 32 byte length prefix + byte array length + 32 byte remainder padding
            var len = _val.Count();
            return (UInt256.SIZE * 2) + PadLength(len, UInt256.SIZE);
        }

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            if (_info.Category != SolidityTypeCategory.Bytes)
            {
                throw UnsupportedTypeException();
            }

            // bytes, of length k(which is assumed to be of type uint256):
            // enc(X) = enc(k) pad_right(X), i.e.the number of bytes is encoded as a uint256 
            // followed by the actual value of X as a byte sequence, followed  by the minimum
            // number of zero-bytes such that len(enc(X)) is a multiple of 32.
            // write length prefix


            // write data offset position into header
            int offset = buff.HeadLength + buff.DataAreaCursorPosition;
            UInt256Encoder.Instance.Encode(buff.HeadCursor, offset);
            buff.IncrementHeadCursor(UInt256.SIZE);

            // write payload len into data buffer          
            int len = _val.Count();
            UInt256Encoder.Instance.Encode(buff.DataAreaCursor, len);
            buff.IncrementDataCursor(UInt256.SIZE);

            // write payload into data buffer
            int i = 0;
            foreach (byte b in _val)
            {
                buff.DataAreaCursor[i++] = b;
            }
            int padded = PadLength(len, UInt256.SIZE);
            buff.IncrementDataCursor(padded);
        }

        public override void Decode(ref AbiDecodeBuffer buff, out IEnumerable<byte> val)
        {
            // Read the next header int which is the offset to the start of the data
            // in the data payload area.
            UInt256Encoder.Instance.Decode(buff.HeadCursor, out int startingPosition);

            // The first int in our offset of data area is the length of the rest of the payload.
            var encodedLength = buff.Buffer.Slice(startingPosition, UInt256.SIZE);
            UInt256Encoder.Instance.Decode(encodedLength, out int byteLen);

            // Read the actual payload from the data area
            var payload = buff.Buffer.Slice(startingPosition + UInt256.SIZE, byteLen);
            var bytes = payload.ToArray();
            int bodyLen = PadLength(bytes.Length, UInt256.SIZE);
            val = bytes;

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
