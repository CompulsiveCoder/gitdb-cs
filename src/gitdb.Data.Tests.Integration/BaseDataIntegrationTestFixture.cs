using System;
using NUnit.Framework;

namespace gitdb.Data.Tests
{
    public abstract class BaseDataIntegrationTestFixture : BaseTestFixture
	{
		public BaseDataIntegrationTestFixture ()
		{
		}

        // TODO: Is this needed?
        public GitDB GetMockGitDB()
        {
            var db = new MockGitDB (Environment.CurrentDirectory);

            InitializeDB (db);

            return db;
        }

        public void InitializeDB(GitDB db)
        {
            db.Settings.IsVerbose = true;
        }
	}
}

