using System;
using System.IO;

namespace gitdb.Data
{
    public class DirectoryContext
    {
        public string WorkingDirectory { get;set; }

        public DirectoryContext (string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
        }

        public string GetPath(string relativePath)
        {
            return Path.Combine(WorkingDirectory, relativePath);
        }
    }
}

