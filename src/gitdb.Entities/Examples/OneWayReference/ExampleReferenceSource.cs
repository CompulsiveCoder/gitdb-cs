using System;

namespace gitdb.Entities
{
	public class ExampleReferenceSource : BaseEntity
	{
		public ExampleReferenceTarget Target { get; set; }

		public ExampleReferenceSource ()
		{
		}
	}
}

