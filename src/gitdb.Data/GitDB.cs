using System;
using gitdb.Data;
using gitdb.Entities;
using System.Collections.Generic;
using gitter;
using System.IO;

namespace gitdb.Data
{
	public class GitDB
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;

		public DataPreparer Preparer;

		public DataSaver Saver;
		public DataDeleter Deleter;
		public DataUpdater Updater;
		public DataReader Reader;
		public DataLister Lister;
		public DataLinker Linker;
		public DataChecker Checker;

		public EntityLinker EntityLinker;

		public GitDBSettings Settings = new GitDBSettings();

        public Gitter Gitter;

        #region Construction
        public GitDB (string workingDirectory)
        {
            Construct (new GitDBSettings (workingDirectory));
        }

        public GitDB (GitDBSettings settings)
        {
            Construct (settings);
        }

        public void Construct(GitDBSettings settings)
		{
            if (settings != null)
                Settings = settings;
            else
                Settings = new GitDBSettings (Environment.CurrentDirectory);

            if (Settings.IsVerbose) {
                Console.WriteLine ("Constructing GitDB:");
                Console.WriteLine ("  " + settings.Location.DataDirectory);
            }

            Gitter = new Gitter ();

            TypeManager = new DataTypeManager (Settings, Gitter);
            IdManager = new DataIdManager (Settings, Gitter);

			EntityLinker = new EntityLinker ();

            var preparer = new DataPreparer (Settings);
			Preparer = preparer;

			var reader = new DataReader (Settings, TypeManager, IdManager);
			Reader = reader;

			var lister = new DataLister (Settings, TypeManager, IdManager, reader);
			Lister = lister;

            var checker = new DataChecker (Settings, reader);
			Checker = checker;

            var saver = new DataSaver (Settings, TypeManager, IdManager, preparer, null, checker, Gitter); // The linker argument is null because it needs to be set after it's created below
			Saver = saver;

            var updater = new DataUpdater (Settings, null, preparer, checker); // The linker argument is null because it needs to be set after it's created below
			Updater = updater;

			var linker = new DataLinker (Settings, reader, saver, updater, checker, EntityLinker);
			Linker = linker;

			var deleter = new DataDeleter (Settings, IdManager, linker);
			Deleter = deleter;

			// TODO: Is there a way to avoid this messy hack?
			// Make sure the linker is set to the saver and updater
			saver.Linker = linker;
			updater.Linker = linker;

            Gitter = new Gitter ();

            EnsureRepositoryExists ();
		}
        #endregion

        #region Save/Update/Delete
		public virtual void SaveOrUpdate(BaseEntity entity)
		{
			if (Settings.IsVerbose)
				Console.WriteLine ("Save/update");
			
			if (Exists (entity))
				Update (entity);
			else
				Save (entity);
		}

        public virtual void Save(BaseEntity[] entities)
		{
			foreach (var entity in entities) {
				Save (entity);
			}
		}

        public virtual void Save(BaseEntity entity)
		{
			Save (entity, false);
		}

        public virtual void Save(BaseEntity entity, bool saveLinkedEntities)
		{
			Saver.Save (entity, saveLinkedEntities);

            ApplyPending ();
		}

        public virtual void Update(BaseEntity[] entities)
		{
			foreach (var entity in entities) {
				Update (entity);
			}
		}

        public virtual void Update(BaseEntity entity)
		{
			Updater.Update (entity);

			ApplyPending ();
		}

        public virtual void Delete(BaseEntity entity)
		{
			Deleter.Delete (entity);

			ApplyPending ();
		}
        #endregion

        #region Get
        public virtual T Get<T>(string id)
            where T : BaseEntity
		{
			return Reader.Read<T> (id);
		}

        public virtual T[] Get<T>()
            where T : BaseEntity
		{
			return Lister.Get<T> ();
		}

        public virtual BaseEntity[] GetAll()
		{
			return Lister.GetAll();
		}

        public virtual BaseEntity Get(string entityTypeName, string entityId)
		{
			return Reader.Read (entityTypeName, entityId);
		}

        public virtual BaseEntity Get(Type entityType, string entityId)
		{
			return Reader.Read (entityType, entityId);
		}
        #endregion

        protected void ApplyPending()
        {
            if (Settings.IsVerbose)
                Console.WriteLine ("Applying pending operations");

            Saver.CommitPendingSaves ();

            Updater.CommitPendingUpdates ();

            Deleter.CommitPendingDeletes ();
        }

		public Type GetType(string typeName)
		{
			return TypeManager.GetType (typeName);
		}

		public int Count(string typeName)
		{
			return Lister.Get (typeName).Length;
		}

		public bool Exists(BaseEntity entity)
		{
			return Checker.Exists (entity);
		}

		public bool TypeExists(string typeName)
		{
			return TypeManager.Exists (typeName);
		}

		public void SaveLinkedEntities(BaseEntity entity)
		{
			Linker.SaveLinkedEntities (entity);
		}

		public void UpdateLinkedEntities(BaseEntity entity)
		{
			Linker.UpdateLinkedEntities (entity);
		}

		public void WriteSummary()
		{
			Console.WriteLine ("");
			Console.WriteLine ("Redis data summary...");

			var types = TypeManager.GetTypes ();
			if (types.Count == 0)
				Console.WriteLine ("  [empty]");
			else {
				foreach (var typeName in types.Keys) {
					Console.WriteLine (typeName + ": " + Count (typeName));
				}
			}
			Console.WriteLine ("");
		}

        public void Init()
        {
            if (!Directory.Exists (Settings.Location.DataDirectory))
                Directory.CreateDirectory (Settings.Location.DataDirectory);

            var gitDir = Path.Combine (Settings.Location.DataDirectory, ".git");
            var isInitialized = Directory.Exists (gitDir);
            if (!isInitialized) {
                Gitter.Init (Settings.Location.DataDirectory);
            }
        }

        public void Commit()
        {
            var repo = Gitter.Open (Settings.Location.DataDirectory);
            repo.Commit ();
        }

        public void EnsureRepositoryExists()
        {
            if (!Gitter.IsRepository (Settings.Location.DataDirectory))
                Gitter.Init (Settings.Location.DataDirectory);
        }
	}
}

