using System;

namespace gitdb.Entities
{
	public class ExampleEntity : BaseEntity
	{
		public string Text { get; set; }

		public int Number { get; set; }

		public ExampleEntity ()
		{
		}

		public ExampleEntity(string text, int number)
		{
			Text = text;
			Number = number;
		}
	}
}

