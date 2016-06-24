using System;
using gitdb.Entities;
using System.IO;
using System.Collections.Generic;

namespace gitdb.Data
{
	public class DataDeleter : BaseDataAdapter
	{
		public DataIdManager IdManager;
		public DataLinker Linker;
        public FileNamer Namer;

        public List<BaseEntity> PendingDelete = new List<BaseEntity>();

        public DataDeleter (DirectoryContext location, DataIdManager idManager, DataLinker linker)
		{
			IdManager = idManager;
            Namer = new FileNamer (location);
			Linker = linker;
		}

		public void Delete(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException ("entity");
			
            //Console.WriteLine ("Deleting: " + entity.GetType ().Name);

            Linker.RemoveLinks (entity);

            var filePath = Namer.CreateFilePath (entity.TypeName, entity.Id);

            File.Delete (filePath);

            IdManager.Remove (entity);
        }

        // TODO: Should delayed deletion be removed? It's not currently being used by the data linker.
        public void CommitPendingDeletes()
        {
            while (PendingDelete.Count > 0)
            {
                Delete (PendingDelete[0]);
                PendingDelete.RemoveAt (0);
            }
        }

        public void DelayDelete(BaseEntity entity)
        {
            if (!PendingDelete.Contains(entity))
                PendingDelete.Add (entity);
        }
	}
}

