using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SolCodeGen.AbiEncoding
{
    public static class EncoderUtil
    {
        public static ReadOnlyMemory<byte> GetBytes(params IAbiTypeEncoder[] encoders)
        {
            // get length of all encoded params
            int totalLen = GetEncodedLength(encoders);

            // create buffer to write encoded params into
            Memory<byte> buff = new Memory<byte>(new byte[totalLen]);

            // encode transaction arguments
            WriteParams(encoders, buff.Span);

            return buff;
        }

        public static string GetHex(params IAbiTypeEncoder[] encoders)
        {
            // get length of all encoded params
            int totalLen =  GetEncodedLength(encoders);

            // create buffer to write encoded params into
            Span<byte> buff = stackalloc byte[totalLen];

            // encode transaction arguments
            WriteParams(encoders, buff);

            // hex encode
            return HexConverter.GetHexFromBytes(buff, hexPrefix: true);
        }

        public static string ToEncodedHex(this IAbiTypeEncoder encoder)
        {
            return GetHex(encoder);
        }

        static int GetEncodedLength(params IAbiTypeEncoder[] encoders)
        {
            // get length of all encoded params
            int totalLen = 0;
            foreach (var encoder in encoders)
            {
                var len = encoder.GetEncodedSize();
                Debug.Assert(len % 32 == 0);
                totalLen += len;
            }
            return totalLen;
        }

        static void WriteParams(IAbiTypeEncoder[] encoders, Span<byte> buff)
        {
            // encode transaction arguments
            Span<byte> cursor = buff;
            foreach (var encoder in encoders)
            {
                var start = cursor;
                cursor = encoder.Encode(cursor);
                var hex = start.Slice(0, start.Length - cursor.Length).ToHexString(hexPrefix: false);
            }
        }
    }
}
