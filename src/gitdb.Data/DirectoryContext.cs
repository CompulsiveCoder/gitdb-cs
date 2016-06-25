using System;
using System.IO;

namespace gitdb.Data
{
    public class DirectoryContext
    {
        public string DataDirectory { get;set; }

        public DirectoryContext (string workingDirectory)
        {
            DataDirectory = workingDirectory;
        }

        public string GetPath(string relativePath)
        {
            return Path.Combine(DataDirectory, relativePath);
        }
    }
}

