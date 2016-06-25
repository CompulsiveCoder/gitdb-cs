using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests.Unit
{
	[TestFixture]
	public class DataCheckerUnitTestFixture : BaseDataUnitTestFixture
	{
		[Test]
		public void Test_Check()
		{
            var context = GetTestDataContext ();

            var checker = new DataChecker (context.Settings, context.Reader);

			var exampleArticle = new ExampleArticle ();

            var json = exampleArticle.ToJson ();

            var filePath = context.Saver.Namer.CreateFilePath (exampleArticle.TypeName, exampleArticle.Id);

            File.WriteAllText (filePath, json);

			var exists = checker.Exists (exampleArticle);

			Assert.IsTrue (exists);
		}
	}
}

