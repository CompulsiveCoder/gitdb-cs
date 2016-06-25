using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests.Unit
{
	[TestFixture]
	public class DataTypeManagerUnitTestFixture : BaseDataUnitTestFixture
	{
		[Test]
		public void Test_Add()
		{
            var directoryContext = GetDirectoryContext ();

            var typeManager = new DataTypeManager (directoryContext);

			var exampleType = typeof(ExampleArticle);

			typeManager.Add (exampleType);

            var filePath = directoryContext.GetPath (typeManager.TypesFileName);

            var typesString = File.ReadAllText(filePath);

            var expectedTypesString = exampleType.FullName + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

			Assert.AreEqual (expectedTypesString, typesString);
		}

		[Test]
		public void Test_GetType()
        {
            var directoryContext = GetDirectoryContext ();

            var typeManager = new DataTypeManager (directoryContext);

			var exampleType = typeof(ExampleArticle);

            var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

            var filePath = directoryContext.GetPath (typeManager.TypesFileName);

            File.WriteAllText(filePath, typesString);

			var type = typeManager.GetType (exampleType.Name);

			Assert.IsNotNull (type);
			Assert.AreEqual (exampleType, type);
		}


		[Test]
		public void Test_GetTypes()
        {
            var directoryContext = GetDirectoryContext ();

            var typeManager = new DataTypeManager (directoryContext);

			var exampleType = typeof(ExampleArticle);

            var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

            var filePath = directoryContext.GetPath (typeManager.TypesFileName);

            File.WriteAllText(filePath, typesString);
			var types = typeManager.GetTypes();

			Assert.AreEqual (1, types.Count);

			Assert.IsTrue (types.ContainsKey (exampleType.Name));

			var fullTypeName = types [exampleType.Name];

            Assert.AreEqual (exampleType.FullName + ", " + exampleType.Assembly.GetName().Name, fullTypeName);
		}
	}
}

