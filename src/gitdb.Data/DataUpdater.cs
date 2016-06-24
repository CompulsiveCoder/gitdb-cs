using System;
using gitdb.Entities;
using System.Collections.Generic;
using System.IO;

namespace gitdb.Data
{
	public class DataUpdater : BaseDataAdapter
	{
		public GitDBSettings Settings;

		public DataChecker Checker;
		public DataLinker Linker;
        public DataPreparer Preparer;
        public FileNamer Namer;

		public List<BaseEntity> PendingUpdate = new List<BaseEntity>();

        public DataUpdater (DirectoryContext location, GitDBSettings settings, DataLinker linker, DataPreparer preparer, DataChecker checker)
		{
			Settings = settings;
			Linker = linker;
			Preparer = preparer;
            Checker = checker;
            Namer = new FileNamer (location);
		}

		public void Update(BaseEntity entity)
		{
			if (Checker.Exists (entity)) {
				if (Settings.IsVerbose)
					Console.WriteLine ("Updating: " + entity.GetType ().Name);

				Linker.CommitLinks (entity);

				InternalUpdate (entity);
			} else// if (!Data.PendingSave.Contains(entity)) // TODO: Remove if not needed
				throw new EntityNotFoundException (entity);
		}

		public void InternalUpdate(BaseEntity entity)
		{
            var json = entity.ToJson ();

            var filePath = Namer.CreateFilePath (entity.TypeName, entity.Id);

            File.WriteAllText (filePath, json);
        }

		public void DelayUpdate(BaseEntity entity)
		{
			if (!PendingUpdate.Contains(entity))
				PendingUpdate.Add (entity);
		}

		public void CommitPendingUpdates()
		{
			while (PendingUpdate.Count > 0)
			{
				try
				{
					var entity = PendingUpdate[0];
					if (Checker.Exists(entity))
					{
						Update (entity);
						PendingUpdate.RemoveAt (0);
					}
				}
				catch (EntityNotFoundException ex) {
					// TODO: Check if this exception should be thrown
					throw new UnsavedLinksException (ex.Entity);
				}
			}
		}
	}
}

