﻿using System;
using System.Collections.Generic;
using gitdb.Entities;
using System.IO;

namespace gitdb.Data
{
	public class DataIdManager
    {
        public DirectoryContext Context { get;set; }

        public DataIdsParser IdsParser = new DataIdsParser ();

        public DataIdManager (DirectoryContext context)
        {
            if (context == null)
                throw new ArgumentNullException ("context");

            Context = context;
        }

		public void Add(BaseEntity entity)
		{
            var ids = new List<string>(GetIds (entity.TypeName));

			if (!ids.Contains (entity.Id))
				ids.Add (entity.Id);

			SetIds (entity.TypeName, ids.ToArray ());
		}

		public void Remove(BaseEntity entity)
		{
			var ids = new List<string>(GetIds (entity.TypeName));

			if (!ids.Contains (entity.Id))
				ids.Remove (entity.Id);

            SetIds (entity.TypeName, ids.ToArray ());
		}

		public string[] GetIds(string entityType)
        {
            var filePath = Context.GetPath (entityType + "-Ids");

            if (File.Exists (filePath)) {
                var content = File.ReadAllText (filePath);

                var ids = IdsParser.ParseIds (content);

                return ids;
            } else
                return new string[] {};
		}

		public void SetIds(string entityType, string[] ids)
        {
            var filePath = Context.GetPath (entityType + "-Ids");

            var idsString = IdsParser.CompileIds (ids);

            File.WriteAllText (filePath, idsString);
		}

		public string[] ConvertToStringArray(Guid[] ids)
		{
			var idStrings = new List<string> ();

			foreach (Guid id in ids)
				idStrings.Add (id.ToString());

			return idStrings.ToArray ();
		}
	}
}

