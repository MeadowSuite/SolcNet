using AdvancedDLSupport;
using SolcNet.NativeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolcNet.AdvDL
{
    public class LibFactory
    {
        public const string LIB_FILE = "solc";

        public static INativeSolcLib Create()
        {
            var config = ImplementationOptions.UseLazyBinding;
            var resolver = new LibFilePathResolver();
            try
            {
                var builder = new NativeLibraryBuilder(config, resolver);
                var library = builder.ActivateClass<AdvDLSolcLibBase, IAdvDLSolcLib>(LIB_FILE);
                return library;
            }
            catch (FileNotFoundException)
            {
                var result = resolver.Resolve(LIB_FILE);
                throw result.Exception ?? new Exception(result.ErrorReason);
            }
        }

    }
}
