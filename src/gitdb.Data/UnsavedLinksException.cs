using System;
using gitdb.Entities;

namespace gitdb.Data
{
	public class UnsavedLinksException : Exception
	{
		public UnsavedLinksException (BaseEntity entity) : base("Some of the entities linked to '" + entity.GetType().Name + " have not been saved. Use the Data.SaveLinkedEntities(entity) function.")
		{
		}
	}
}

