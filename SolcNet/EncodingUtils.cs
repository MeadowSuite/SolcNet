using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolcNet
{
    static class EncodingUtils
    {
        static string BOMMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public static string RemoveBom(string p)
        {
            return p.Trim(new char[] { '\uFEFF', '\u200B' });
        }

        public static byte[] HexToBytes(string input)
        {
            var outputLength = input.Length / 2;
            var output = new byte[outputLength];
            for (var i = 0; i < outputLength; i++)
            {
                output[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }
            return output;
        }

        public static string ByteArrayToHex(byte[] barray)
        {
            char[] c = new char[barray.Length * 2];
            byte b;
            for (int i = 0; i < barray.Length; ++i)
            {
                b = ((byte)(barray[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(barray[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }

    }
}
