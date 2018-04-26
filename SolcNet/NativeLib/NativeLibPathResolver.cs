using AdvancedDLSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.OSPlatform;
using static System.Runtime.InteropServices.Architecture;
using static System.Runtime.InteropServices.RuntimeInformation;
using static AdvancedDLSupport.ResolvePathResult;
using PlatInfo = System.ValueTuple<System.Runtime.InteropServices.OSPlatform, System.Runtime.InteropServices.Architecture>;

namespace SolcNet.NativeLib
{
    class LibFilePathResolver : ILibraryPathResolver
    {
        static readonly Dictionary<PlatInfo, (string Prefix, string Extension)> PlatformPaths = new Dictionary<PlatInfo, (string, string)>
        {
            [(Windows, X64)] = ("win-x64",   ".dll"),
            [(Windows, X86)] = ("win-x86",   ".dll"),
            [(Linux,   X64)] = ("linux-x64", ".so"),
            [(OSX,     X86)] = ("osx-x64",   ".dylib"),
        };

        static readonly OSPlatform[] SupportedPlatforms = { Windows, OSX, Linux };
        static string SupportedPlatformDescriptions() => string.Join("\n", PlatformPaths.Keys.Select(GetPlatformDesc));

        static string GetPlatformDesc((OSPlatform OS, Architecture Arch) info) => $"{info.OS}; {info.Arch}";

        static readonly OSPlatform CurrentOSPlatform = SupportedPlatforms.FirstOrDefault(IsOSPlatform);
        static readonly PlatInfo CurrentPlatformInfo = (CurrentOSPlatform, ProcessArchitecture);
        static readonly Lazy<string> CurrentPlatformDesc = new Lazy<string>(() => GetPlatformDesc((CurrentOSPlatform, ProcessArchitecture)));

        static readonly Dictionary<PlatInfo, ResolvePathResult> Cache = new Dictionary<PlatInfo, ResolvePathResult>();

        public ResolvePathResult Resolve(string library)
        {
            ResolvePathResult result;
            if (Cache.TryGetValue(CurrentPlatformInfo, out result))
            {
                return result;
            }

            if (!PlatformPaths.TryGetValue(CurrentPlatformInfo, out (string Prefix, string Extension) platform))
            {
                result = FromError(string.Join("\n", $"Unsupported platform: {CurrentPlatformDesc.Value}", "Must be one of:", SupportedPlatformDescriptions()));
            }
            else
            {
                string libLocation = Path.GetDirectoryName(typeof(ISolcNativeLib).Assembly.Location);
                string filePath = Path.Combine(libLocation, "native", platform.Prefix, library) + platform.Extension;
                if (!File.Exists(filePath))
                {
                    result = FromError($"Platform can be supported but '{library}' lib not found for {CurrentPlatformDesc.Value} at {filePath}");
                }
                else
                {
                    result = FromSuccess(filePath);
                }
            }

            Cache[CurrentPlatformInfo] = result;
            return result;

        }
    }
}
