using System;
using System.Collections.Generic;
using gitdb.Entities;
using System.IO;
using gitter;

namespace gitdb.Data
{
	public class DataIdManager
    {
        public GitDBSettings Settings { get;set; }

        public Gitter Gitter { get; set; }

        public DataIdsParser IdsParser = new DataIdsParser ();

        public DataIdManager (GitDBSettings settings, Gitter gitter)
        {
            if (settings == null)
                throw new ArgumentNullException ("settings");

            if (gitter == null)
                throw new ArgumentNullException ("gitter");

            Settings = settings;
            Gitter = gitter;
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

			if (ids.Contains (entity.Id))
				ids.Remove (entity.Id);

            SetIds (entity.TypeName, ids.ToArray ());
		}

		public string[] GetIds(string entityType)
        {
            var filePath = Settings.Location.GetPath (entityType + "-Ids");

            if (File.Exists (filePath)) {
                var content = File.ReadAllText (filePath);

                var ids = IdsParser.ParseIds (content);

                return ids;
            } else
                return new string[] {};
		}

		public void SetIds(string entityType, string[] ids)
        {
            var filePath = Settings.Location.GetPath (entityType + "-Ids");

            var idsString = IdsParser.CompileIds (ids);

            var isNewFile = !File.Exists (filePath);

            File.WriteAllText (filePath, idsString);

            if (isNewFile) {
                var relativePath = Settings.Location.GetRelativePath(filePath);

                var repo = Gitter.Open (Settings.Location.DataDirectory);

                repo.Add(relativePath);
            }
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

