using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests.Unit
{
    [TestFixture(Category="Unit")]
    public class DataListerUnitTestFixture : BaseDataUnitTestFixture
    {
        [Test]
        public void Test_List()
        {
            var context = GetTestDataContext ();

            var exampleArticle = new ExampleArticle ();

            var json = exampleArticle.ToJson ();

            var filePath = Path.Combine(Environment.CurrentDirectory, exampleArticle.TypeName + "-" + exampleArticle.Id);

            File.WriteAllText (filePath, json);

            var idsFilePath = Path.GetFullPath (exampleArticle.TypeName + "-Ids");

            File.WriteAllText (idsFilePath, exampleArticle.Id);

            var typesFilePath = Path.GetFullPath (context.TypeManager.TypesFileName);

            var typeEntry = exampleArticle.TypeName + ":" + exampleArticle.GetType ().FullName + ", " + exampleArticle.GetType ().Assembly.GetName ().Name;

            File.WriteAllText (typesFilePath, typeEntry);

            var lister = new DataLister (context.Settings, context.TypeManager, context.IdManager, context.Reader);

            var articles = lister.Get<ExampleArticle> ();

            Assert.AreEqual (1, articles.Length);

            Assert.IsNotNull (articles [0]);

            Assert.AreEqual (exampleArticle.Id, articles [0].Id);
        }
    }
}

