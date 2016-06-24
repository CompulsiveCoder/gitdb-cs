using System;

namespace gitdb.Data.Tests
{
    public class MockDataChecker : DataChecker
    {
        public bool OverrideCheck = true;
        public bool ReturnValue = false;

        public MockDataChecker (DirectoryContext location, DataReader reader, GitDBSettings settings) : base(location, reader, settings)
        {
        }

        public override bool Exists (gitdb.Entities.BaseEntity entity)
        {
            if (!OverrideCheck)
                return base.Exists (entity);
            else
                return ReturnValue;
        }
    }
}

