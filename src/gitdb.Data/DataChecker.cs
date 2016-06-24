using System;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data
{
	public class DataChecker
	{
		public GitDBSettings Settings;

		public DataReader Reader;

        public FileNamer Namer;

		public DataChecker (DirectoryContext location, DataReader reader, GitDBSettings settings)
		{
			Settings = settings;
			Reader = reader;
            Namer = new FileNamer (location);
		}

        public virtual bool Exists(BaseEntity entity)
		{
			if (Settings.IsVerbose)
				Console.WriteLine ("Checking if entity exists: " + entity.GetType().Name);

			var entityType = entity.GetType ();

            var filePath = Namer.CreateFilePath (entity.TypeName, entity.Id);

            var exists = File.Exists (filePath);

			if (Settings.IsVerbose)
				Console.WriteLine ("  Exists: " + exists);

			return exists;
		}
	}
}

