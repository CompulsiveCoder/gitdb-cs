using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace gitdb.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleInvoiceItem : BaseEntity
	{
		public string Description = "";
		public int Amount = 0;

		[TwoWay("Items")]
		public ExampleInvoice Invoice { get; set; }

		public ExampleInvoiceItem ()
		{
		}

		public ExampleInvoiceItem(ExampleInvoice invoice)
		{
			Invoice = invoice;

			invoice.Add (this);
		}
	}
}

