using System;
using Newtonsoft.Json;
using gitdb.Entities;

namespace gitdb.Data
{
	public class BaseDataAdapter
	{
        public GitDBSettings Settings { get;set; }

        public BaseDataAdapter(GitDBSettings settings)
        {
            Settings = settings;
        }
	}
}

