using System;
using NUnit.Framework;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data.Tests.Integration
{
    [TestFixture(Category="Integration")]
    public class DataListerIntegrationTestFixture : BaseDataIntegrationTestFixture
    {
        [Test]
        public void Test_List()
        {
            var db = GetGitDB ();

            var exampleArticle = new ExampleArticle ();

            var json = exampleArticle.ToJson ();

            var filePath = db.Saver.Namer.CreateFilePath (exampleArticle.TypeName, exampleArticle.Id);

            File.WriteAllText (filePath, json);

            var idsFilePath = Path.GetFullPath (exampleArticle.TypeName + "-Ids");

            File.WriteAllText (idsFilePath, exampleArticle.Id);

            var typesFilePath = Path.GetFullPath (db.TypeManager.TypesFileName);

            var typeEntry = exampleArticle.TypeName + ":" + exampleArticle.GetType().FullName + ", " + exampleArticle.GetType().Assembly.GetName().Name;

            File.WriteAllText (typesFilePath, typeEntry);

            var articles = db.Get<ExampleArticle> ();

            Assert.AreEqual(1, articles.Length);

            Assert.IsNotNull (articles [0]);

            Assert.AreEqual (exampleArticle.Id, articles [0].Id);
        }
    }
}

