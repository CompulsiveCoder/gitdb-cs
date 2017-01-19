using System;

namespace gitdb.Data
{
    public class EntityTypeNotFoundException : Exception
    {
        public EntityTypeNotFoundException (string typeName) : base("The entity type '" + typeName + "' wasn't found in the system. Add some entities of that type first.")
        {
        }
    }
}

