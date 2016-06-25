using System;

namespace gitdb.Data
{
	public class GitDBSettings
	{
		public string Prefix = "";

		public bool IsVerbose = false;

        public DirectoryContext Location;

		public GitDBSettings (string dataDirectory, string prefix)
		{
			Prefix = prefix;
            Location = new DirectoryContext (dataDirectory);
		}

		public GitDBSettings ()
		{
            Prefix = Guid.NewGuid ().ToString ();
            Location = new DirectoryContext (Environment.CurrentDirectory);
		}

        static public GitDBSettings Verbose
        {
            get {
                var settings = new GitDBSettings ();
                settings.IsVerbose = true;
                return settings;
            }
        }
	}
}

