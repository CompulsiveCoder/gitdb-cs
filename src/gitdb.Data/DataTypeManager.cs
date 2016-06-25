using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace gitdb.Data
{
	public class DataTypeManager
	{
        public DirectoryContext Context { get;set; }

        public TypeNamesParser TypeNamesParser = new TypeNamesParser();

        public string TypesFileName = "types";

        public DataTypeManager (DirectoryContext context)
		{
            if (context == null)
                throw new ArgumentNullException ("context");
			
            Context = context;
		}

		public string[] GetTypeNames()
		{
            return (from type in GetTypes ().Keys
                select type).ToArray();
		}

		public Dictionary<string, string> GetTypes()
        {
            var filePath = Context.GetPath (TypesFileName);

            if (File.Exists (filePath)) {
                var content = File.ReadAllText (filePath);

                var typeNames = TypeNamesParser.ParseTypeDefinitions (content);

                return typeNames;
            } else
                return new Dictionary<string, string> ();
		}

		public void Add(Type type)
		{
            Add (type.FullName, type.FullName + ", " + type.Assembly.GetName().Name);
		}

		public void Add(string typeName, string typeFullName)
		{
            // TODO: Change this function to append directly to the file
			var types = GetTypes();

			if (!types.ContainsKey (typeName))
				types.Add (typeName, typeFullName);

			SetTypes (types);
		}

		public void SetTypes(Dictionary<string, string> typeDefinitions)
        {
            var typesString = TypeNamesParser.CompileTypeDefinitions (typeDefinitions);

            var filePath = Context.GetPath (TypesFileName);

            File.WriteAllText (filePath, typesString);
		}

		public bool Exists(string typeName)
		{
			var types = GetTypes ();
			return types.ContainsKey (typeName)
			|| types.ContainsValue (typeName);
		}

		public void EnsureExists(Type type)
		{
            EnsureExists (type.FullName, type.FullName + ", " + type.Assembly.GetName().Name);
		}

		public void EnsureExists(string typeName, string typeFullName)
		{
			if (!Exists (typeName)) {
				Add (typeName, typeFullName);
			}
		}

		public Type GetType(string typeName)
		{
			var types = GetTypes ();
			if (types.ContainsKey (typeName)) {
				var typeFullName = types [typeName];			
				return Type.GetType (typeFullName);
			} else
				throw new InvalidOperationException ("The type '" + typeName + "' was not found.");
		}

	}
}

