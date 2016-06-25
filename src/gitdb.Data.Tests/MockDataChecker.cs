using System;

namespace gitdb.Data.Tests
{
    public class MockDataChecker : DataChecker
    {
        public bool OverrideCheck = true;
        public bool ReturnValue = false;

        public MockDataChecker (GitDBSettings settings, DataReader reader) : base(settings, reader)
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

