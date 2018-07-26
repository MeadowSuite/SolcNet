using NativeLibraryLoader;
using System.Collections.Generic;

namespace SolcNet.NativeLibraryLoader
{
    class CustomResolver : PathResolver
    {
        string _filePath;
        public CustomResolver(string filePath)
        {
            _filePath = filePath;
        }

        public override IEnumerable<string> EnumeratePossibleLibraryLoadTargets(string name)
        {
            yield return _filePath;
        }
    }
}
