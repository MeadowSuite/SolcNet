using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SolCodeGen.AbiEncoding
{
    public static class EncoderExtensions
    {
        public static Span<byte> ToEncodedBuffer(this IEnumerable<IAbiTypeEncoder> encoders)
        {
            // get length of all encoded params
            int totalLen = 0;
            foreach(var encoder in encoders)
            {
                var len = encoder.GetEncodedSize();
                Debug.Assert(len % 32 == 0);
                totalLen += len;
            }
            // create buffer to write encoded params into
            Span<byte> buff = new byte[totalLen];

            // encode transaction arguments
            Span<byte> cursor = buff;
            foreach(var encoder in encoders)
            {
                cursor = encoder.Encode(cursor);
            }

            return buff;
        }

        public static string ToEncodedHex(this IEnumerable<IAbiTypeEncoder> encoders)
        {
            // get length of all encoded params
            int totalLen = 0;
            foreach (var encoder in encoders)
            {
                var len = encoder.GetEncodedSize();
                Debug.Assert(len % 32 == 0);
                totalLen += len;
            }
            // create buffer to write encoded params into
            Span<byte> buff = stackalloc byte[totalLen];

            // encode transaction arguments
            Span<byte> cursor = buff;
            foreach (var encoder in encoders)
            {
                cursor = encoder.Encode(cursor);
            }

            return HexConverter.GetHexFromBytes(buff, hexPrefix: true);
        }

        public static string ToEncodedHex(this IAbiTypeEncoder encoder)
        {
            return new[] { encoder }.ToEncodedHex();
        }
    }
}
