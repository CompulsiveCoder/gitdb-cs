using System;
using System.Linq;

namespace gitdb.Data
{
    public class DataIdsParser
    {
        public char IdSeparator = ',';

        public DataIdsParser ()
        {
        }

        public string[] ParseIds(string idsString)
        {
            return (from id in idsString.Split (IdSeparator)
                             where !String.IsNullOrEmpty (id.ToString ())
                             select id).ToArray ();
        }

        public string CompileIds(string[] ids)
        {
            return String.Join (IdSeparator.ToString(), ids);
        }
    }
}

