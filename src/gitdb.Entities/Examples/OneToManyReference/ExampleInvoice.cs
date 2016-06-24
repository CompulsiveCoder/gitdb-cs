using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace gitdb.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleInvoice : BaseEntity
	{
		public int NumberValue = 0;

		[TwoWay("Invoice")]
		public ExampleInvoiceItem[] Items { get;set; }

		public ExampleInvoice ()
		{
			Items = new ExampleInvoiceItem[]{ };
		}

		public ExampleInvoice(params ExampleInvoiceItem[] items)
		{
			Items = items;
			foreach (var item in Items) {
				item.Invoice = this;
			}
		}

		public void Add(ExampleInvoiceItem item)
		{
			var list = new List<ExampleInvoiceItem> ();
			if (Items != null)
				list.AddRange (Items);
			list.Add (item);
			Items = list.ToArray ();
		}

	}
}

