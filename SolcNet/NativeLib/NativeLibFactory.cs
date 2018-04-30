using AdvancedDLSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolcNet.NativeLib
{
    public class NativeLibFactory
    {
        public const string LIB_FILE = "solc";

        public static ISolcNativeLib Create()
        {
            var config = ImplementationOptions.UseLazyBinding;
            var resolver = new LibFilePathResolver();
            try
            {
                var builder = new NativeLibraryBuilder(config, resolver);
                var library = builder.ActivateInterface<ISolcNativeLib>(LIB_FILE);
                return library;
            }
            catch (FileNotFoundException)
            {
                var result = resolver.Resolve("solc");
                throw result.Exception ?? new Exception(result.ErrorReason);
            }
        }
    }
}
