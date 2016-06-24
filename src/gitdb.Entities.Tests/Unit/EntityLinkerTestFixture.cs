using System;
using NUnit.Framework;

namespace gitdb.Entities.Tests
{
	[TestFixture(Category="Unit")]
	public class EntityLinkerTestFixture
	{
		[Test]
		public void Test_AddLink_OneWay_SingleEntity()
		{
			var source = new ExampleReferenceSource ();
			var target = new ExampleReferenceTarget ();

			var adder = new EntityLinker ();

			adder.AddLink (source, "Target", target);

			//Assert.IsNotNull (source.Target, "Link failed.");
			//Assert.IsNotNull (target.Left, "Reverse link failed.");
		}

		// TODO: Remove if not needed
		//[Test]
		public void Test_AddLink_TwoWay_SingleEntity()
		{
			var invoice = new ExampleInvoice ();
			var invoiceItem = new ExampleInvoiceItem ();

			var adder = new EntityLinker ();

			adder.AddLink (invoice, "Right", invoiceItem);

			Assert.IsNotNull (invoice.Items, "Link failed.");
			Assert.AreEqual(1, invoice.Items.Length, "Link failed.");
			Assert.IsNotNull (invoiceItem.Invoice, "Reverse link failed.");
		}

		[Test]
		public void Test_RemoveEntityFromObject_Array()
		{
			var linker = new EntityLinker ();

			var entity = new ExampleInvoiceItem ();

			var obj = new ExampleInvoiceItem[]{entity};

			var propertyInfo = typeof(ExampleInvoice).GetProperty ("Items");

			var newObj = linker.RemoveEntityFromObject (entity, obj, propertyInfo);

			Assert.IsNotNull (newObj);
			Assert.AreEqual (0, ((ExampleInvoiceItem[])newObj).Length);
		}
	}
}

