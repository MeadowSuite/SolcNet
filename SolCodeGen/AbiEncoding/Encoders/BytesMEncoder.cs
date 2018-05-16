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
            return 32;
        }

        public override Span<byte> Encode(Span<byte> buffer)
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
                buffer[i++] = b;
            }
            return buffer.Slice(32);
        }

        public override ReadOnlySpan<byte> Decode(ReadOnlySpan<byte> buffer, out IEnumerable<byte> val)
        {
            var bytes = new byte[_info.ArrayLength];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = buffer[i];
            }
            // data validity check: all bytes after the fixed M amount should be zero
            for (var i = bytes.Length; i < 32; i++)
            {
                if (buffer[i] != 0)
                {
                    throw new ArgumentException($"Invalid {_info.SolidityName} input data; should be {_info.ArrayLength} bytes padded {32 - _info.ArrayLength} zero-bytes; received: " + buffer.Slice(0, 32).ToHexString());
                }
            }
            val = bytes;
            return buffer.Slice(32);
        }

    }

}
