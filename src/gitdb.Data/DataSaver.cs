﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using gitdb.Entities;
using System.Collections.Generic;
using gitter;

namespace gitdb.Data
{
	public class DataSaver : BaseDataAdapter
	{
		public DataIdManager IdManager;
		public DataTypeManager TypeManager;
		public DataPreparer Preparer;
		public DataChecker Checker;
		public DataLinker Linker;
        public FileNamer Namer;
        public Gitter Gitter;

		public List<BaseEntity> PendingSave = new List<BaseEntity>();

        public DataSaver (GitDBSettings settings, DataTypeManager typeManager, DataIdManager idManager, DataPreparer preparer, DataLinker linker, DataChecker checker, Gitter gitter) : base(settings)
		{
			Settings = settings;
			IdManager = idManager;
			TypeManager = typeManager;
			Preparer = preparer;
			Checker = checker;
			Linker = linker;
            Gitter = gitter;
            Namer = new FileNamer (settings.Location);
		}

		public void Save(BaseEntity entity)
		{
			Save (entity,  true);
		}

		public void Save(BaseEntity entity, bool commitLinks)
		{
			if (!Checker.Exists (entity)) {
				var entityType = entity.GetType ();

				if (Settings.IsVerbose)
					Console.WriteLine ("Saving: " + entityType.Name);

				TypeManager.EnsureExists (entityType);

				// Commit links before saving, otherwise it will fail
				if (commitLinks)
					Linker.CommitLinks (entity);

				InternalSave (entity);
			} else
				throw new EntityAlreadyExistsException (entity);
		}

		public void InternalSave(BaseEntity entity)
		{
            var filePath = Namer.CreateFilePath (entity.TypeName, entity.Id);

            if (Settings.IsVerbose)
                Console.WriteLine ("  " + filePath);

			var json = Preparer.PrepareForStorage (entity).ToJson ();

            var isNewFile = !File.Exists (filePath);

            File.WriteAllText (filePath, json);

            if (isNewFile) {
                var relativePath = Settings.Location.GetRelativePath(filePath);

                var repo = Gitter.Open (Settings.Location.DataDirectory);

                repo.Add(relativePath);
            }

			IdManager.Add (entity);
		}

		public void DelaySave(BaseEntity entity)
		{
			if (!PendingSave.Contains(entity))
				PendingSave.Add (entity);
		}

		public void CommitPendingSaves()
		{
			while (PendingSave.Count > 0)
			{
				var entity = PendingSave [0];
				if (!Checker.Exists (entity)) {
					Save (entity);
					PendingSave.RemoveAt (0);
				}
			}
		}
	}
}

