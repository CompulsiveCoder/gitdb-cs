using System;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data
{
	public class DataReader : BaseDataAdapter
	{
		public DataTypeManager TypeManager;
		public DataIdManager IdManager;
        public PropertyIndexer Indexer;

        public FileNamer Namer;

        public DataReader (GitDBSettings settings, DataTypeManager typeManager, DataIdManager idManager, PropertyIndexer indexer) : base(settings)
		{
			TypeManager = typeManager;
			IdManager = idManager;
            Indexer = indexer;
            Namer = new FileNamer (settings.Location);
		}

		public T Read<T>(string entityId)
            where T : BaseEntity
		{
            return (T)Read (typeof(T).FullName, entityId);
        }

		public BaseEntity Read(string entityTypeName, string entityId)
		{
            if (TypeManager.Exists (entityTypeName)) {
                var entityType = TypeManager.GetType (entityTypeName);
                if (entityType == null)
                    throw new InvalidTypeException (entityType);
                return Read (entityType, entityId);
            } else {
                if (Settings.IsVerbose)
                    Console.WriteLine ("Type doesn't exist (not found by TypeManager): " + entityTypeName);
                return null;
            }
		}

		public BaseEntity Read(Type entityType, string entityId)
		{
            if (Settings.IsVerbose) {
                Console.WriteLine ("Reading entity: " + entityType.FullName);
                Console.WriteLine ("ID: " + entityId);
            }

			if (entityType == null)
				throw new ArgumentNullException ("entityType");
			
            var filePath = Namer.CreateFilePath(entityType.FullName, entityId);

            if (!File.Exists (filePath)) {
                if (Settings.IsVerbose)
                    Console.WriteLine (entityType.FullName + " entity file not found: " + filePath);
                return null;
            }

            var json = File.ReadAllText(filePath);

            if (String.IsNullOrEmpty (json)) {
                if (Settings.IsVerbose)
                    Console.WriteLine ("Entity file empty: " + entityType.FullName);
                return null;
            }
			
			var entity = new Parser ().Parse (entityType, json);

			return entity;
        }
	}
}

