using System;

namespace gitdb.Data
{
    public class FileNamer
    {
        public DirectoryContext Location { get; set; }

        public FileNamer (DirectoryContext directoryContext)
        {
            Location = directoryContext;
        }

        public string CreateFilePath(string typeName, string id)
        {
            var fileName = CreateFileName (typeName, id);
            
            return Location.GetPath (fileName);
        }

        public string CreateFileName(string typeName, string id){
            return String.Format("{0}-{1}", typeName, id);
        }
    }
}

