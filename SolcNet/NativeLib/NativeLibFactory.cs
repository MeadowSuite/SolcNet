using AdvancedDLSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolcNet.NativeLib
{
    public class NativeLibFactory
    {
        public static ISolcNativeLib Create()
        {
            ImplementationOptions config = default; //ImplementationOptions.UseIndirectCalls | ImplementationOptions.UseLazyBinding | ImplementationOptions.GenerateDisposalChecks;
            var resolver = new LibFilePathResolver();
            try
            {
                var library = new NativeLibraryBuilder(config, resolver).ActivateInterface<ISolcNativeLib>("solc");
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
