using System;
using NUnit.Framework;
using gitdb.Entities.Examples;

namespace gitdb.Data.Tests.Integration
{
    [TestFixture]
    public class PropertyIndexerIntegrationTextFixture : BaseDataIntegrationTestFixture
    {
        [Test]
        public void Test_Index()
        {
            Console.WriteLine ("");
            Console.WriteLine ("Preparing test");
            Console.WriteLine ("");

            var data = GetGitDB ();

            var example1a = new SimpleEntity ();
            example1a.Text = "One";
            data.Save (example1a);

            var example1b = new SimpleEntity ();
            example1b.Text = "One";
            data.Save (example1b);

            var example2a = new SimpleEntity ();
            example2a.Text = "Two";
            data.Save (example2a);

            var example2b = new SimpleEntity ();
            example2b.Text = "Two";
            data.Save (example2b);

            var example3a = new SimpleEntity ();
            example3a.Text = "Three";
            data.Save (example3a);

            var example3b = new SimpleEntity ();
            example3b.Text = "Three";
            data.Save (example3b);

            var indexer = data.Indexer;

            indexer.IndexProperty (typeof(SimpleEntity), "Text");

            var indexOne = indexer.GetIndex (typeof(SimpleEntity), "Text", "One");

            Assert.AreEqual (2, indexOne.Length);
            Assert.AreEqual (example1a.Id, indexOne[0].Id);
            Assert.AreEqual (example1b.Id, indexOne[1].Id);

            var indexTwo = indexer.GetIndex (typeof(SimpleEntity), "Text", "Two");

            Assert.AreEqual (2, indexTwo.Length);
            Assert.AreEqual (example2a.Id, indexTwo[0].Id);
            Assert.AreEqual (example2b.Id, indexTwo[1].Id);

            var indexThree = indexer.GetIndex (typeof(SimpleEntity), "Text", "Three");

            Assert.AreEqual (2, indexThree.Length);
            Assert.AreEqual (example3a.Id, indexThree[0].Id);
            Assert.AreEqual (example3b.Id, indexThree[1].Id);
        }

        [Test]
        public void Test_IndexAndRead()
        {
            Console.WriteLine ("");
            Console.WriteLine ("Preparing test");
            Console.WriteLine ("");

            var data = GetGitDB ();

            var example1a = new SimpleEntity ();
            example1a.Text = "One";
            data.Save (example1a);

            var example2a = new SimpleEntity ();
            example2a.Text = "Two";
            data.Save (example2a);

            var example3a = new SimpleEntity ();
            example3a.Text = "Three";
            data.Save (example3a);

            var indexer = data.Indexer;

            indexer.IndexProperty (typeof(SimpleEntity), "Text");

            var foundEntity1 = indexer.Read (typeof(SimpleEntity), "Text", "One");

            Assert.AreEqual (example1a.Id, foundEntity1.Id);

            var foundEntity2 = indexer.Read (typeof(SimpleEntity), "Text", "Two");

            Assert.AreEqual (example2a.Id, foundEntity2.Id);

            var foundEntity3 = indexer.Read (typeof(SimpleEntity), "Text", "Three");

            Assert.AreEqual (example3a.Id, foundEntity3.Id);
        }
    }
}

