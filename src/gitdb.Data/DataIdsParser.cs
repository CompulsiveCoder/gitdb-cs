using System;

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
            return idsString.Split(IdSeparator);
        }

        public string CompileIds(string[] ids)
        {
            return String.Join (IdSeparator.ToString(), ids);
        }
    }
}

