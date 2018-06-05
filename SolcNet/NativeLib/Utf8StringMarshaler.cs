using System;
using System.Runtime.InteropServices;

namespace SolcNet.NativeLib
{
    public class Utf8StringMarshaler : ICustomMarshaler
    {
        string _clean;

        public const string CLEAN = "clean";

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new Utf8StringMarshaler(cookie);
        }

        public Utf8StringMarshaler(string clean)
        {
            _clean = clean;
        }

        public void CleanUpManagedData(object ManagedObj)
        {

        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            if (_clean == CLEAN)
            {
                Marshal.FreeHGlobal(pNativeData);
            }
        }

        public int GetNativeDataSize()
        {
            throw new NotImplementedException();
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return EncodingUtils.StringToUtf8((string)ManagedObj);
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return EncodingUtils.Utf8ToString(pNativeData);
        }
    }

}
