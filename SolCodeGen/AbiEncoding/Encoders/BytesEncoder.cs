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
            return (32 * 2) + PadLength(len, 32);
        }

        public override Span<byte> Encode(Span<byte> buffer)
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
            int len = _val.Count();
            buffer = UInt256Encoder.Encode(buffer, 32);
            buffer = UInt256Encoder.Encode(buffer, len);
            int i = 0;
            foreach (byte b in _val)
            {
                buffer[i++] = b;
            }
            int padded = PadLength(len, 32);
            return buffer.Slice(padded);
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out IEnumerable<byte> val)
        {
            buffer = UInt256Encoder.Decode(buffer, out var lenPrefix);
            if (lenPrefix != 32)
            {
                throw new ArgumentException("Bytes input data should start with the number 32 encoded as a uint256");
            }

            buffer = UInt256Encoder.Decode(buffer, out var len);
            if (len > int.MaxValue)
            {
                throw new ArgumentException($"Bytes input data is invalid: the byte length prefix is {len} which is unlikely to be intended");
            }

            var bytes = new byte[(int)len];
            buffer.Slice(0, bytes.Length).CopyTo(bytes);
            val = bytes;
            int bodyLen = PadLength(bytes.Length, 32);

            // data validity check: should be right-padded with zero bytes
            for (var i = bytes.Length; i < bodyLen; i++)
            {
                if (buffer[i] != 0)
                {
                    throw new ArgumentException($"Invalid bytes input data; should be {bytes.Length} followed by {bodyLen - bytes.Length} zero-bytes");
                }
            }

            return buffer.Slice(bodyLen);
        }

    }

}
