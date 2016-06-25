using System;
using NUnit.Framework;
using gitdb.Entities;

namespace gitdb.Data.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class DataLinkerIntegrationTestFixture : BaseDataIntegrationTestFixture
	{

		/// <summary>
		/// Ensure that an exception is thrown when a linked entity hasn't been saved yet. This is necessary because the linker cannot synchronise
		/// links with an entity that isn't in the data store.
		/// </summary>
		[Test]
		public void Test_CommitLinks_NonSavedEntity()
		{
			var left = new ExampleInvoice ();
			var right = new ExampleInvoiceItem ();

			right.Invoice = left;

			var data = GetMockGitDB ();

			// Try to save the "right" object without first saving the "left" object. It should throw an exception because it can't sync with
			// a non-existent entity
			data.Linker.CommitLinks(right);
		}

		[Test]
		public void Test_TwoWayReference_Add()
		{
			var invoice = new ExampleInvoice ();

			var data = GetMockGitDB ();

			data.Save(invoice);

			var invoiceItem = new ExampleInvoiceItem ();

			invoiceItem.AddLink("Invoice", invoice);

			data.Save (invoiceItem);

			// The "left.Right" property should now contain a link to the "right" object
			Assert.IsNotNull (invoice.Items, "Linker failed to add the link to the other entity.");
		}

		[Test]
		public void Test_TwoWayReference_RemoveReverseLinkOnUpdate()
		{
			var author = new ExampleAuthor ();
			var article = new ExampleArticle ();

			author.Articles = new ExampleArticle[]{ article };
			article.Author = author;

			var data = GetMockGitDB ();

			data.Settings.IsVerbose = true;

			data.Save (article);
			data.Save (author);

			data.WriteSummary ();

			// Remove the article
			author.Articles = new ExampleArticle[]{ };

			data.Update(author);

			data.WriteSummary ();

			var newArticle = data.Get<ExampleArticle> (article.Id);

			Assert.IsNull(newArticle.Author, "Linker failed to remove the link.");
		}

        // TOD: Remove or reimplement
		//[Test]
		public void Test_TwoWayReference_RemoveOnDelete()
		{
            Console.WriteLine ("");
			Console.WriteLine ("Preparing test...");
			Console.WriteLine ("");

            var data = GetMockGitDB ();
			data.Settings.IsVerbose = true;

			data.WriteSummary ();

			var invoiceItem = new ExampleInvoiceItem ();
			var invoice = new ExampleInvoice (invoiceItem);

			data.Save(invoice, true);

			data.WriteSummary ();

			Console.WriteLine ("");
			Console.WriteLine ("Executing test code...");
			Console.WriteLine ("");

			data.Delete(invoice);

			data.WriteSummary ();

			var newInvoice = data.Get<ExampleInvoice> (invoice.Id);

			Assert.AreEqual(0, newInvoice.Items.Length, "Linker failed to remove the link.");
		}

		[Test]
		public void Test_SaveLinkedEntities()
		{
			Console.WriteLine ("");
			Console.WriteLine ("Preparing test");
			Console.WriteLine ("");

			var data = GetMockGitDB ();

			var invoice = new ExampleInvoice ();
			var invoiceItem = new ExampleInvoiceItem (invoice);

			Console.WriteLine ("");
			Console.WriteLine ("Starting test");
			Console.WriteLine ("");

			data.SaveLinkedEntities (invoice);

			var foundItem = data.Get<ExampleInvoiceItem> (invoiceItem.Id);

			Assert.IsNotNull (foundItem, "Linker failed to save the other entity.");
		}
			
		[Test]
		public void Test_UpdateLinkedEntities()
		{
			Console.WriteLine ("");
			Console.WriteLine ("Preparing test");
			Console.WriteLine ("");

			var data = GetMockGitDB ();

			var invoice = new ExampleInvoice ();
			var invoiceItem = new ExampleInvoiceItem (invoice);

			data.Save(invoice);
			data.Save (invoiceItem);

			invoiceItem.Amount = 2;

			Console.WriteLine ("");
			Console.WriteLine ("Starting test");
			Console.WriteLine ("");

			data.UpdateLinkedEntities (invoice);

			var foundRight = data.Get<ExampleInvoiceItem> (invoiceItem.Id);

			// The "right" object should have been updated in the data store
			Assert.AreEqual(2, foundRight.Amount, "Linker failed to update the other entity.");
		}
	}
}

