using System;
using gitdb.Entities;

namespace gitdb.Data
{
	public class DataPreparer : BaseDataAdapter
	{
        public DataPreparer (GitDBSettings settings) : base(settings)
		{
		}

		public BaseEntity PrepareForStorage(BaseEntity entity)
		{
			var validatedEntity = entity.Clone ();

            // TODO: Add validation

			return validatedEntity;
		}
	}
}

