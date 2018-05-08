using System;
using System.Collections.Generic;
using System.Linq;

namespace SolCodeGen.SolidityTypeEncoding.Encoders
{
    public class ByteArrayEncoder : SolidityTypeEncoder<IEnumerable<byte>>
    {
        public override int GetEncodedSize()
        {
           switch(_info.Category)
            {
                case SolidityTypeCategory.Bytes:
                    // 32 byte length prefix + byte array length + 32 byte remainder padding
                    var len = _val.Count();
                    return 32 + len + (len % 32);
                case SolidityTypeCategory.BytesM:
                    return 32;
                default:
                    throw UnsupportedTypeException();
            }
        }

        public override Span<byte> Encode(Span<byte> buffer)
        {
            if (_info.Category == SolidityTypeCategory.Bytes)
            {
                // bytes, of length k(which is assumed to be of type uint256):
                // enc(X) = enc(k) pad_right(X), i.e.the number of bytes is encoded as a uint256 
                // followed by the actual value of X as a byte sequence, followed  by the minimum
                // number of zero-bytes such that len(enc(X)) is a multiple of 32.
                // write length prefix
                int len = _val.Count();
                buffer = UInt256Encoder.EncodeUnchecked(buffer, len);
                int i = 0;
                foreach (byte b in _val)
                {
                    buffer[i++] = b;
                }
                int padding = len % 32;
                return buffer.Slice(len + padding);
            }
            else if (_info.Category == SolidityTypeCategory.BytesM)
            {
                // bytes<M>: enc(X) is the sequence of bytes in X padded with trailing
                // zero-bytes to a length of 32 bytes.
                int i = 0;
                foreach(byte b in _val)
                {
                    buffer[i++] = b;
                }
                return buffer.Slice(32);
            }

            throw UnsupportedTypeException();

        }

    }

}
