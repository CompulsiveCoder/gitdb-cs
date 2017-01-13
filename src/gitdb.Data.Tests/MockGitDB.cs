using System;
using NUnit.Framework;
using System.Collections.Generic;
using gitdb.Entities;

namespace gitdb.Data.Tests
{
    public class MockGitDB : GitDB
    {
        public List<BaseEntity> SavedEntities = new List<BaseEntity>();
        public List<BaseEntity> UpdatedEntities = new List<BaseEntity>();
        public List<BaseEntity> DeletedEntities = new List<BaseEntity>();

        public bool EnableDefaultFunctionality = false;

        public MockGitDB (string workingDirectory) : base(workingDirectory)
        {
        }

        public override void Save (gitdb.Entities.BaseEntity entity)
        {
            SavedEntities.Add (entity);

            if (EnableDefaultFunctionality)
                base.Save (entity);
        }

        public override void Update (gitdb.Entities.BaseEntity entity)
        {
            UpdatedEntities.Add (entity);

            if (EnableDefaultFunctionality)
                base.Update (entity);
        }

        public override void Delete (gitdb.Entities.BaseEntity entity)
        {
            DeletedEntities.Add (entity);

            if (EnableDefaultFunctionality)
                base.Delete (entity);
        }
    }
}

