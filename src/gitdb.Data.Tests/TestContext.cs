using System;
using gitdb.Entities;
using gitter;

namespace gitdb.Data.Tests
{
	public class TestContext
	{
		public GitDBSettings Settings { get;set; }

		public DataTypeManager TypeManager { get; set; }

		public DataIdManager IdManager { get; set; }

		public DataPreparer Preparer { get;set; }
		public DataLinker Linker { get;set; }
		public DataSaver Saver { get;set; }
		public DataUpdater Updater { get;set; }
		public DataDeleter Deleter { get;set; }
		public DataReader Reader { get;set; }
        public DataChecker Checker { get;set; }

        public PropertyIndexer Indexer;

        public Gitter Gitter { get;set; }

		public EntityLinker EntityLinker { get; set; }

        public DirectoryContext Location { get; set; }

        public TestContext (string workingDirctory)
		{
            Location = new DirectoryContext (workingDirctory);

			Settings = new GitDBSettings ();
			Settings.IsVerbose = true;

            Gitter = new Gitter ();

            IdManager = new DataIdManager (Settings, Gitter);
            TypeManager = new DataTypeManager (Settings, Gitter);

            EntityLinker = new EntityLinker ();
            throw new NotImplementedException ();
            /*
            Indexer = new PropertyIndexer (DB);

            var preparer = new DataPreparer (Settings);
			Preparer = preparer;

			var reader = new DataReader (Settings, TypeManager, IdManager, Indexer);
			Reader = reader;

			var checker = new DataChecker (Settings, reader);
			Checker = checker;

            var saver = new DataSaver (Settings, TypeManager, IdManager, preparer, null, checker, Gitter); // The linker argument is null because it needs to be set after it's created below
			Saver = saver;

			var updater = new DataUpdater (Settings, null, preparer, checker); // The linker argument is null because it needs to be set after it's created below
			Updater = updater;

			var linker = new DataLinker (Settings, reader, saver, updater, checker, EntityLinker);
			Linker = linker;

			// TODO: Is there a way to avoid this messy hack?
			// Make sure the linker is set to the saver and updater
			saver.Linker = linker;
			updater.Linker = linker;*/
		}
	}
}

