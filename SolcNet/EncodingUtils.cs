using System;
using System.Collections.Generic;
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
    }
}
