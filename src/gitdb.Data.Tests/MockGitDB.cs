using System;

namespace gitdb.Data.Tests
{
    public class MockGitDB : GitDB
    {
        public MockGitDB (string workingDirectory) : base(workingDirectory)
        {
        }
    }
}

