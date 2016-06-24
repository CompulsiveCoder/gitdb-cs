using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace gitdb.Entities
{
	[Serializable]
	public class BaseEntity
	{
		public string Id;

		// TODO: Remove
		//[JsonIgnore]
		//[NonSerialized]
		//public EntityLog Log;

		public bool IsPendingLinkCommit = false;

		//public string[] ChangedProperties = new String[]{};

        public string TypeName
        {
            get { return GetType ().Name; }
        }

		public BaseEntity ()
		{
			Id = Guid.NewGuid ().ToString();
			// TODO: Remove
		//	Log = new EntityLog ();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject (this);
		}

		// TODO: Remove
		/*public EntityLink GetLink()
		{
			return new EntityLink(this);
		}*/

		public void AddLink(string propertyName, BaseEntity linkedEntity)
		{
			var adder = new EntityLinker ();

			adder.AddLink (this, propertyName, linkedEntity);
		}

		public void AddLinks(string propertyName, BaseEntity[] linkedEntities)
		{
			var adder = new EntityLinker ();

			foreach (var linkedEntity in linkedEntities) {
				adder.AddLink (this, propertyName, linkedEntity);
			}
		}

		public BaseEntity Clone()
		{
			return new EntityCloner ().Clone (this);
		}

		/*public void AddChangedProperty(string propertyName)
		{
			var list = new List<string> ();
			list.
		}*/
	}
}

