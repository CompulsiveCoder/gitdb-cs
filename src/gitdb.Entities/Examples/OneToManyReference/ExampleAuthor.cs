using System;
using Newtonsoft.Json;

namespace gitdb.Entities
{
	[Serializable]
	[JsonObject(IsReference = true)]
	public class ExampleAuthor : BaseEntity
	{
		[TwoWay("Author")]
		public ExampleArticle[] Articles { get;set; }
	}
}

