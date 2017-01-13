using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataSaverIntegrationTestFixture : BaseDataIntegrationTestFixture
	{
		[Test]
		public void Test_Save()
		{
			Console.WriteLine ("Preparing test");

			// Create the entity
			var entity = new SimpleEntity ();

			Console.WriteLine ("Executing test");

			var data = GetGitDB ();

			// Save the entity
			data.Save (entity);

            var filePath = new FileNamer (new DirectoryContext (Environment.CurrentDirectory)).CreateFilePath (entity.TypeName, entity.Id);

            Assert.IsTrue (File.Exists (filePath));
		}

		[Test]
		[ExpectedException(typeof(EntityAlreadyExistsException))]
		public void Test_Save_EntityAlreadyExistsException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			var data = GetGitDB ();

			// Save the entity
			data.Save (entity);

			// Call the Save function again which should throw an exception
			data.Save(entity);
		}
	}
}

