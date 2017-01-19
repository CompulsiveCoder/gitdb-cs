using System;
using NUnit.Framework;
using gitdb.Entities.Examples;

namespace gitdb.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataUpdaterTestFixture : BaseDataIntegrationTestFixture
	{
		[Test]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void Test_Update_EntityNotFoundException()
		{
			// Create the entity
			var entity = new SimpleEntity ();

			var data = GetMockGitDB ();

			// Call the Update function which should throw an exception because it hasn't been saved yet
			data.Updater.Update(entity);
		}
	}
}

