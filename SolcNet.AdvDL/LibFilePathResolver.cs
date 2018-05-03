using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvancedDLSupport;
using SolcNet.NativeLib;

namespace SolcNet.AdvDL
{
    class LibFilePathResolver : ILibraryPathResolver
    {
        public ResolvePathResult Resolve(string library)
        {
            try
            {
                var path = LibPathResolver.Resolve(library);
                return ResolvePathResult.FromSuccess(path);
            }
            catch (Exception ex)
            {
                return ResolvePathResult.FromError(ex);
            }
        }
    }
}
