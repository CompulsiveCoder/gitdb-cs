using System;
using System.Collections.Generic;
using gitdb.Entities;
using Newtonsoft.Json;
using System.IO;

namespace gitdb.Data
{
    public class PropertyIndexer
    {
        public GitDB DB { get; set; }
        
        public PropertyIndexer (GitDB db)
        {
            DB = db;
        }

        public BaseEntity Read(Type entityType, string propertyName, object propertyValue)
        {
            if (DB.Settings.IsVerbose) {
                Console.WriteLine ("Reading entity: " + entityType.FullName);
                Console.WriteLine ("Property name: " + propertyName);
                Console.WriteLine ("Property value: " + propertyValue);
            }

            if (entityType == null)
                throw new ArgumentNullException ("entityType");

            var index = DB.Indexer.GetIndex (entityType, propertyName, propertyValue);

            if (index.Length == 0)
                return null;
            else if (index.Length > 1)
                throw new Exception ("Multiple matching entities were found. Use the lister if that is intentional. The reader is for single entities.");
            else
                return index [0];
        }

        public BaseEntity Read(string entityTypeName, string propertyName, object propertyValue)
        {
            if (DB.TypeManager.Exists (entityTypeName)) {
                var entityType = DB.TypeManager.GetType (entityTypeName);
                if (entityType == null)
                    throw new InvalidTypeException (entityType);
                return Read (entityType, propertyName, propertyValue);
            } else {
                if (DB.Settings.IsVerbose)
                    Console.WriteLine ("Type doesn't exist (not found by TypeManager): " + entityTypeName);
                return null;
            }
        }

        public T Read<T>(string propertyName, object propertyValue)
            where T : BaseEntity
        {
            return (T)Read (typeof(T).FullName, propertyName, propertyValue);
        }

        public void IndexProperty(Type entityType, string propertyName)
        {
            IndexProperty (entityType.FullName, propertyName);
        }
       
        public void IndexProperty(string entityTypeName, string propertyName)
        {
            Console.WriteLine ("Indexing property");
            Console.WriteLine ("Entity type: " + entityTypeName);
            Console.WriteLine ("Property name: " + propertyName);

            var data = GenerateIndexData (entityTypeName, propertyName);

            foreach (var value in data.Keys) {
                WriteIndexEntityPropertyValueFile (entityTypeName, propertyName, value, data[value].ToArray());
            }
        }

        public void WriteIndexEntityPropertyValueFile(string entityTypeName, string propertyName, object value, BaseEntity[] matchingEntities)
        {
            var key = CreateIndexEntityPropertyValueKey (entityTypeName, propertyName, value);

            var path = DB.Settings.Location.GetPath (key);

            var json = JsonConvert.SerializeObject (matchingEntities);

            File.WriteAllText (path, json);
        }

        public Dictionary<object, List<BaseEntity>> GenerateIndexData(string entityTypeName, string propertyName)
        {
            var data = new Dictionary<object, List<BaseEntity>> ();

            foreach (var entity in DB.Get(entityTypeName)) {
                var propertyValue = GetPropertyValue (entity, propertyName);

                List<BaseEntity> list = null;

                if (!data.ContainsKey (propertyValue)) {
                    list = new List<BaseEntity> ();

                    data.Add (propertyValue, list);
                } else {
                    list = data [propertyValue];
                }

                if (!list.Contains(entity))
                    list.Add (entity);
            }

            return data;
        }

        public BaseEntity[] GetIndex(Type entityType, string propertyName, object value)
        {
            return GetIndex (entityType.FullName, propertyName, value);
        }

        public BaseEntity[] GetIndex(string entityTypeName, string propertyName, object value)
        {
            var key = CreateIndexEntityPropertyValueKey(entityTypeName, propertyName, value);

            var path = DB.Settings.Location.GetPath (key);

            if (!File.Exists (path))
                throw new Exception ("No entity property value index file found: " + path); // TODO: Create a custom exception

            var content = File.ReadAllText (path);

            var entities = (BaseEntity[])JsonConvert.DeserializeObject (content, typeof(BaseEntity[]));

            return entities;
        }

        public object GetPropertyValue(BaseEntity entity, string propertyName)
        {
            var type = entity.GetType ();

            var propertyInfo = type.GetProperty (propertyName);

            var value = propertyInfo.GetValue (entity);

            return value;
        }

        public string CreateIndexEntityPropertyValueKey(string entityTypeName, string propertyName, object value)
        {
            // TODO: Use a short hash of the value so long values don't cause errors in file names
            var key = entityTypeName + "-" + propertyName + "--" + value.ToString().ToLower();

            return key;

        }
    }
}

