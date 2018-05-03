using AdvancedDLSupport;
using SolcNet.NativeLib;
using System.Runtime.InteropServices;

namespace SolcNet.AdvDL
{
    public abstract class AdvDLSolcLibBase : NativeLibraryBase, IAdvDLSolcLib, INativeSolcLib
    {
        protected AdvDLSolcLibBase(string path, ImplementationOptions options, TypeTransformerRepository transformerRepository)
            : base(path, options, transformerRepository)
        {

        }

        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string compileJSON([MarshalAs(UnmanagedType.LPStr)] string input, bool optimize);
        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string compileJSONCallback([MarshalAs(UnmanagedType.LPStr)] string input, bool optimize, AvdDLReadFileCallback readCallback);
        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string compileJSONMulti([MarshalAs(UnmanagedType.LPStr)] string input, bool optimze);
        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string compileStandard([MarshalAs(UnmanagedType.LPStr)] string input, AvdDLReadFileCallback readCallback);
        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string license();
        [return: MarshalAs(UnmanagedType.LPStr)]
        public abstract string version();


        public string Compile(string input, ReadFileCallback readCallback)
        {
           return compileStandard(input, new AvdDLReadFileCallback(readCallback));
        }

        public string CompileLegacyJson(string input, bool optimize)
        {
            return compileJSON(input, optimize);
        }

        public string CompileLegacyJson(string input, bool optimize, ReadFileCallback readCallback)
        {
            return compileJSONCallback(input, optimize, new AvdDLReadFileCallback(readCallback));
        }

        public string CompileLegacyJsonMutli(string input, bool optimize)
        {
            return compileJSONMulti(input, optimize);
        }
        
        public string GetLicense()
        {
            return license();
        }

        public string GetVersion()
        {
            return version();
        }
    }
}
