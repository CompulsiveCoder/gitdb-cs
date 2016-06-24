using System;
using NUnit.Framework;
using gitdb.Entities;

namespace gitdb.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataDeleterIntegrationTestFixture : BaseDataIntegrationTestFixture
	{
		[Test]
		public void Test_Delete()
		{
			Console.WriteLine ("");
			Console.WriteLine ("Executing test");
			Console.WriteLine ("");

			// Create the entity
			var entity = new SimpleEntity ();

			entity.NumberValue = new Random ().Next (10);

			var data = GetMockGitDB ();

			data.Saver.Save (entity);

			Console.WriteLine ("");
			Console.WriteLine ("Executing test");
			Console.WriteLine ("");

			data.Deleter.Delete (entity);

			Console.WriteLine ("");
			Console.WriteLine ("Analysing test");
			Console.WriteLine ("");

			var foundEntity = data.Get<SimpleEntity> (entity.Id);

			Assert.IsNull (foundEntity);
		}
	}
}

