using System;

namespace gitdb.Data
{
    public class InvalidTypeException : Exception
    {
        public InvalidTypeException (Type entityType) : base("Invalid type: " + entityType.FullName)
        {
        }
    }
}

