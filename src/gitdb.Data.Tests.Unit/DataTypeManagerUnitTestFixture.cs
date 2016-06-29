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
            var context = GetTestDataContext ();

            var typeManager = new DataTypeManager (context.Settings, context.Gitter);

			var exampleType = typeof(ExampleArticle);

			typeManager.Add (exampleType);

            var filePath = context.Location.GetPath (typeManager.TypesFileName);

            var typesString = File.ReadAllText(filePath);

            var expectedTypesString = exampleType.FullName + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

			Assert.AreEqual (expectedTypesString, typesString);
		}

		[Test]
		public void Test_GetType()
        {
            var context = GetTestDataContext ();

            var typeManager = new DataTypeManager (context.Settings, context.Gitter);

			var exampleType = typeof(ExampleArticle);

            var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

            var filePath = context.Location.GetPath (typeManager.TypesFileName);

            File.WriteAllText(filePath, typesString);

			var type = typeManager.GetType (exampleType.Name);

			Assert.IsNotNull (type);
			Assert.AreEqual (exampleType, type);
		}


		[Test]
		public void Test_GetTypes()
        {
            var context = GetTestDataContext ();

            var typeManager = new DataTypeManager (context.Settings, context.Gitter);

			var exampleType = typeof(ExampleArticle);

            var typesString = exampleType.Name + typeManager.TypeNamesParser.PairSeparator + exampleType.FullName + ", " + exampleType.Assembly.GetName().Name;

            var filePath = context.Location.GetPath (typeManager.TypesFileName);

            File.WriteAllText(filePath, typesString);
			var types = typeManager.GetTypes();

			Assert.AreEqual (1, types.Count);

			Assert.IsTrue (types.ContainsKey (exampleType.Name));

			var fullTypeName = types [exampleType.Name];

            Assert.AreEqual (exampleType.FullName + ", " + exampleType.Assembly.GetName().Name, fullTypeName);
		}
	}
}

