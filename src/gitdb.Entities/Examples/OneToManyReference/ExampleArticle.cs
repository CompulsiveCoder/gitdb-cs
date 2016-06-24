using System;
using Newtonsoft.Json;

namespace gitdb.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleArticle : BaseEntity
	{
		[TwoWay("Articles")]
		public ExampleAuthor Author { get; set; }

		public ExampleArticle ()
		{
		}
	}
}

