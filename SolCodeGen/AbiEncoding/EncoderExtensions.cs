using SolCodeGen.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Memory<byte> data = new Memory<byte>(new byte[totalLen]);

            AbiEncodeBuffer buffer = new AbiEncodeBuffer(data, encoders.GetTypeInfo());

            // encode transaction arguments
            WriteParams(encoders, ref buffer);

            return data;
        }

        public static AbiTypeInfo[] GetTypeInfo(this IAbiTypeEncoder[] encoders)
        {
            AbiTypeInfo[] info = new AbiTypeInfo[encoders.Length];
            for(var i = 0; i < encoders.Length; i++)
            {
                info[i] = encoders[i].TypeInfo;
            }
            return info;
        }

        public static string GetHex(params IAbiTypeEncoder[] encoders)
        {
            // get length of all encoded params
            int totalLen =  GetEncodedLength(encoders);

            // create buffer to write encoded params into
            Span<byte> data = stackalloc byte[totalLen];
            AbiEncodeBuffer buff = new AbiEncodeBuffer(data, encoders.GetTypeInfo());

            // encode transaction arguments
            WriteParams(encoders, ref buff);

            // hex encode
            return HexConverter.GetHexFromBytes(data, hexPrefix: true);
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

        static void WriteParams(IAbiTypeEncoder[] encoders, ref AbiEncodeBuffer buff)
        {
            // encode transaction arguments
            foreach (var encoder in encoders)
            {
                encoder.Encode(ref buff);
            }
        }
    }
}
