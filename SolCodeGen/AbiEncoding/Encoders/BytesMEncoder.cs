using SolCodeGen.Utils;
using System;
using System.Collections.Generic;

namespace SolCodeGen.AbiEncoding.Encoders
{
    public class BytesMEncoder : AbiTypeEncoder<IEnumerable<byte>>
    {
        public override int GetEncodedSize()
        {
            if (_info.Category != SolidityTypeCategory.BytesM)
            {
                throw UnsupportedTypeException();
            }
            return UInt256.SIZE;
        }

        public override void Encode(ref AbiEncodeBuffer buff)
        {
            if (_info.Category != SolidityTypeCategory.BytesM)
            {
                throw UnsupportedTypeException();
            }

            // bytes<M>: enc(X) is the sequence of bytes in X padded with trailing
            // zero-bytes to a length of 32 bytes.
            int i = 0;
            foreach (byte b in _val)
            {
                buff.HeadCursor[i++] = b;
            }
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

        public override void Decode(ref AbiDecodeBuffer buff, out IEnumerable<byte> val)
        {
            var bytes = new byte[_info.ArrayLength];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = buff.HeadCursor[i];
            }

#if ZERO_BYTE_CHECKS
            // data validity check: all bytes after the fixed M amount should be zero
            for (var i = bytes.Length; i < UInt256.SIZE; i++)
            {
                if (buff.HeadCursor[i] != 0)
                {
                    throw new ArgumentException($"Invalid {_info.SolidityName} input data; should be {_info.ArrayLength} bytes padded {UInt256.SIZE - _info.ArrayLength} zero-bytes; received: " + buff.HeadCursor.Slice(0, 32).ToHexString());
                }
            }
#endif

            val = bytes;
            buff.IncrementHeadCursor(UInt256.SIZE);
        }

    }

}
