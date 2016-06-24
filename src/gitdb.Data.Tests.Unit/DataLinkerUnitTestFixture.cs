using System;
using NUnit.Framework;
using gitdb.Entities;

namespace gitdb.Data.Tests.Unit
{
	[TestFixture(Category="Unit")]
	public class DataLinkerUnitTestFixture : BaseDataUnitTestFixture
	{
		/// <summary>
		/// Ensure that an exception is thrown when a linked entity hasn't been saved yet. This is necessary because the linker cannot synchronise
		/// links with an entity that isn't in the data store.
		/// // TODO: Add a way to disable this check
		/// </summary>
		//[Test] // TODO: Remove or reimplement
		public void Test_CommitLinks_NonSavedEntity()
		{
			// TODO: Remove if not needed
			throw new NotImplementedException ();

			/*var data = new GitDB();

			var left = new ExampleInvoice ();
			var right = new ExampleInvoiceItem ();

			right.Invoice = left;

			// Try to save the "right" object without first saving the "left" object. It should throw an exception because it can't sync with
			// a non-existent entity
			data.Linker.CommitLinks(right);*/
		}

	}
}

