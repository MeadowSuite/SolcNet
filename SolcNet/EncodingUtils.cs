using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SolcNet
{
    public static class EncodingUtils
    {
        static string BOMMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        public static string RemoveBom(string p)
        {
            return p.Trim(new char[] { '\uFEFF', '\u200B' });
        }

        public static IntPtr StringToUtf8(string str)
        {
            if (str == null)
            {
                return IntPtr.Zero;
            }
            int len = Encoding.UTF8.GetByteCount(str);
            byte[] buffer = new byte[len + 1];
            Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);
            IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

        public static string Utf8ToString(IntPtr utf8)
        {
            int len = 0;
            while (Marshal.ReadByte(utf8, len) != 0) ++len;
            byte[] buffer = new byte[len];
            Marshal.Copy(utf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        public static ValueTuple<T1, T2, T3>[] Flatten<T1, T2, T3>(this Dictionary<T1, Dictionary<T2, T3>> dicts)
        {
            return FlattenNestedDictionaries(dicts);
        }

        public static ValueTuple<T1, T2, T3>[] FlattenNestedDictionaries<T1, T2, T3>(Dictionary<T1, Dictionary<T2, T3>> dicts)
        {
            var items = new List<(T1, T2, T3)>();
            foreach (var kp in dicts)
            {
                foreach (var c in kp.Value)
                {
                    items.Add((kp.Key, c.Key, c.Value));
                }
            }
            return items.ToArray();
        }

    }
}
