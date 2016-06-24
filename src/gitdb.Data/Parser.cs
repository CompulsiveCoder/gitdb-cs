using System;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using gitdb.Entities;

namespace gitdb.Data
{
	public class Parser
	{
		public Parser ()
		{
		}

		public BaseEntity Parse(Type type, string json)
		{
			return (BaseEntity)JsonConvert.DeserializeObject(json, type);
		}

		public T Parse<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}

