using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests
{
	public class BaseTestFixture
	{
        public string OriginalDirectory;

        public bool DeleteTemporaryDirectory = false;

        public BaseTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
			Console.WriteLine ("Setting up test fixture " + this.GetType ().FullName);

            OriginalDirectory = Environment.CurrentDirectory;

            var tmpDir = new TemporaryDirectoryCreator ().Create ();

            Directory.SetCurrentDirectory (tmpDir);

            Console.WriteLine ("Original directory:");
            Console.WriteLine (" " + OriginalDirectory);
            Console.WriteLine ("Current directory:");
            Console.WriteLine (" " + tmpDir);
            Console.WriteLine ();
		}

        [TearDown]
        public virtual void TearDown()
        {
            var tmpDir = Environment.CurrentDirectory;

            Directory.SetCurrentDirectory (OriginalDirectory);

            if (DeleteTemporaryDirectory && tmpDir.ToLower().Contains(".tmp")) {
                Directory.Delete (tmpDir, true);
            }
        }

		public TestContext GetTestDataContext()
		{
            return new TestContext (Environment.CurrentDirectory);
		}

        public DirectoryContext GetDirectoryContext()
        {
            return new DirectoryContext (Environment.CurrentDirectory);
        }

		public GitDB GetGitDB()
		{
            var data = new GitDB (Environment.CurrentDirectory);

			return data;
		}
	}
}

