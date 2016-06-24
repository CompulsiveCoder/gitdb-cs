using System;

namespace gitdb.Data
{
	public class GitDBSettings
	{
		public string Prefix = "";

		public bool IsVerbose = false;

		public GitDBSettings (string prefix)
		{
			Prefix = prefix;
		}

		public GitDBSettings ()
		{
			Prefix = Guid.NewGuid ().ToString ();
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

