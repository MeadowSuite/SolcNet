using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolcNet
{
    class SourceFileResolver
    {
        string _solSourceRoot;
        string _lastSourceDir;

        Dictionary<string, string> _fileContents = new Dictionary<string, string>();

        public SourceFileResolver(string solSourceRoot, Dictionary<string, string> fileContents)
        {
            _solSourceRoot = solSourceRoot;
            _fileContents = fileContents ?? new Dictionary<string, string>();
        }

        public void ReadSolSourceFileManaged(string path, ref string contents, ref string error)
        {
            try
            {
                string sourceFilePath = path;
                // if given path is relative and a root is provided, combine them
                if (!Path.IsPathRooted(path) && _solSourceRoot != null)
                {
                    sourceFilePath = Path.Combine(_solSourceRoot, path);
                }
                if (!File.Exists(sourceFilePath) && _lastSourceDir != null)
                {
                    sourceFilePath = Path.Combine(_lastSourceDir, path);
                }

                sourceFilePath = sourceFilePath.Replace('\\', '/');
                if (!_fileContents.TryGetValue(sourceFilePath, out contents))
                {
                    if (File.Exists(sourceFilePath))
                    {
                        _lastSourceDir = Path.GetDirectoryName(sourceFilePath);
                        contents = File.ReadAllText(sourceFilePath, Encoding.UTF8);
                        _fileContents.Add(sourceFilePath, contents);
                    }
                    else
                    {
                        error = "Source file not found: " + path;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
            }
        }
    }
}
