using System;

namespace gitdb.Data
{
	public class GitDBSettings
	{
		public bool IsVerbose = false;

        public DirectoryContext Location;

        public GitDBSettings (string dataDirectory)
        {
            Location = new DirectoryContext (dataDirectory);
        }

		public GitDBSettings ()
		{
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

        static public GitDBSettings CreateVerbose(string dataDirectory)
        {
            var settings = new GitDBSettings (dataDirectory);
            settings.IsVerbose = true;
            return settings;
        }
	}
}

