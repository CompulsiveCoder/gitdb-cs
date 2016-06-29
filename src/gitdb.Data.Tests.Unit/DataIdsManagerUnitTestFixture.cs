using System;
using NUnit.Framework;
using System.IO;
using gitdb.Entities;

namespace gitdb.Data.Tests.Unit
{
    [TestFixture(Category="Unit")]
    public class DataIdsManagerUnitTestFixture : BaseDataUnitTestFixture
    {
        [Test]
        public void Test_GetIds()
        {
            var context = GetTestDataContext ();

            var entityType = typeof(ExampleArticle);

            var fileName = entityType.FullName + "-Ids";

            var filePath = context.Location.GetPath (fileName);

            var idsString = Guid.NewGuid ().ToString () + "," + Guid.NewGuid ().ToString ();

            File.WriteAllText (filePath, idsString);

            var idManager = new DataIdManager (context.Settings, context.Gitter);

            var ids = idManager.GetIds (entityType.FullName);

            Assert.AreEqual (2, ids.Length);
        }

        [Test]
        public void Test_SetIds()
        {
            var context = GetTestDataContext ();

            var entityType = typeof(ExampleArticle);

            var fileName = entityType.FullName + "-Ids";

            var filePath = context.Location.GetPath (fileName);

            var ids = new string[]{Guid.NewGuid ().ToString (), Guid.NewGuid ().ToString ()};

            //File.WriteAllText (filePath, idsString);

            var idManager = new DataIdManager (context.Settings, context.Gitter);

            idManager.SetIds (entityType.FullName, ids);

            var content = File.ReadAllText(filePath);

            var expectedContent = ids[0] + "," + ids[1];

            Assert.AreEqual (expectedContent, content);
        }
    }
}

