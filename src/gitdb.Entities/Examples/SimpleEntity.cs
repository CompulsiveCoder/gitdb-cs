using System;

namespace gitdb.Entities.Examples
{
	[Serializable]
	public class SimpleEntity : BaseEntity
    {
        public string Text { get; set; }

        public int Number { get; set; }

        public SimpleEntity ()
        {
        }

        public SimpleEntity(string text, int number)
        {
            Text = text;
            Number = number;
        }
	}
}

