using System;
using NUnit.Framework;
using System.IO;
using gitdb.Entities.Examples;

namespace gitdb.Data.Tests.Unit
{
	[TestFixture(Category="Unit")]
	public class DataSaverUnitTestFixture : BaseDataUnitTestFixture
	{
		[Test]
		public void Test_Save()
		{
			Console.WriteLine ("Preparing test");

			// Create the entity
			var entity = new SimpleEntity ();

			var context = GetMockGitDB ();

            var checker = new MockDataChecker (context.Settings, context.Reader);
            checker.ReturnValue = false;

            context.Checker = checker;

			var mockLinker = new MockDataLinker (
				context.Settings,
				context.Reader,
				context.Saver,
				context.Updater,
				context.Checker,
				context.EntityLinker
			);

			var saver = new DataSaver (
				context.Settings,
				context.TypeManager,
				context.IdManager,
				context.Preparer,
				mockLinker,
				context.Checker,
                context.Gitter);

			Console.WriteLine ("Executing test");

			// Save the entity
			saver.Save (entity);

            var filePath = context.Saver.Namer.CreateFilePath (entity.TypeName, entity.Id);

            Assert.IsTrue (File.Exists (filePath));

            var typesFilePath = Path.Combine(context.Settings.Location.DataDirectory, context.TypeManager.TypesFileName);

            Assert.IsTrue(File.Exists(typesFilePath));
		}

		// TODO: Should this be moved to integration tests?
		[Test]
		[ExpectedException(typeof(EntityAlreadyExistsException))]
		public void Test_Save_EntityAlreadyExistsException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			var context = GetMockGitDB ();

			var mockLinker = new MockDataLinker (
				context.Settings,
				context.Reader,
				context.Saver,
				context.Updater,
				context.Checker,
				context.EntityLinker
			);

            var saver = new DataSaver (
				context.Settings,
				context.TypeManager,
				context.IdManager,
				context.Preparer,
				mockLinker,
				context.Checker,
                context.Gitter);

			// Save the entity
			saver.Save (entity);

			// Call the Save function again which should throw an exception
			saver.Save(entity);
		}
	}
}

