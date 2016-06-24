using System;
using gitdb.Entities;

namespace gitdb.Data
{
	public class EntityNotFoundException : Exception
	{
		public BaseEntity Entity { get;set; }

		public EntityNotFoundException (BaseEntity entity) : base("'" + entity.GetType ().Name + "' entity not found in data store.")
		{
			Entity = entity;
		}
	}
}

