using System;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data
{
	public class DataReader : BaseDataAdapter
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;

        public FileNamer Namer;

		public DataReader (DirectoryContext location, DataTypeManager typeManager, DataIdManager idManager)
		{
			TypeManager = typeManager;
			IdManager = idManager;
            Namer = new FileNamer (location);
		}

		public T Read<T>(string entityId)
            where T : BaseEntity
		{
            return (T)Read (typeof(T).Name, entityId);
		}

		public BaseEntity Read(string entityTypeName, string entityId)
		{
			if (TypeManager.Exists (entityTypeName)) {
				var entityType = TypeManager.GetType (entityTypeName);
				return Read (entityType, entityId);
			} else
				return null;
		}

		public BaseEntity Read(Type entityType, string entityId)
		{
			if (entityType == null)
				throw new ArgumentNullException ("entityType");
			
            var filePath = Namer.CreateFileName(entityType.Name, entityId);

            if (!File.Exists (filePath))
                return null;

            var json = File.ReadAllText(filePath);

			if (String.IsNullOrEmpty (json))
				return null;
			
			var entity = new Parser ().Parse (entityType, json);

			return entity;
		}
	}
}

