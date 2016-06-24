using System;
using gitdb.Entities;

namespace gitdb.Data
{
	public class DataPreparer : BaseDataAdapter
	{
		public DataPreparer ()
		{
		}

		public BaseEntity PrepareForStorage(BaseEntity entity)
		{
			var validatedEntity = entity.Clone ();

            // TODO

			return validatedEntity;
		}
	}
}

