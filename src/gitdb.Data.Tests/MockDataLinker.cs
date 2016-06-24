using System;
using gitdb.Entities;

namespace gitdb.Data.Tests
{
	public class MockDataLinker : DataLinker
	{
		public bool EnableCommitLinks = false;
		public bool EnableSaveLinkedEntities = false;

		public MockDataLinker (GitDBSettings settings, DataReader reader, DataSaver saver, DataUpdater updater, DataChecker checker, EntityLinker entityLinker) : base(settings, reader, saver, updater, checker, entityLinker)
		{
		}

		public override void CommitLinks (BaseEntity entity)
		{
			if (EnableCommitLinks)
				base.CommitLinks (entity);
			else
				Console.WriteLine ("Bypassed DataLinker.CommitLinks for unit testing purposes.");
		}

		public override void SaveLinkedEntities (BaseEntity entity)
		{
			if (EnableSaveLinkedEntities)
				base.SaveLinkedEntities (entity);
			else
				Console.WriteLine ("Bypassed DataLinker.SaveLinkedEntities for unit testing purposes.");
		}
	}
}

