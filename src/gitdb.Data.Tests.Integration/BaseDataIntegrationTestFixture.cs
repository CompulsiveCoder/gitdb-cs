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
            db.Settings.Prefix = "Test-" + Guid.NewGuid ().ToString ().Substring (0, 8);
            db.Settings.IsVerbose = true;
        }
	}
}

