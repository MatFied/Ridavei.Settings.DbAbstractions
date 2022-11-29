﻿using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Ridavei.Settings.DbAbstractions.Settings;

namespace Ridavei.Settings.DbAbstractions.Tests.Settings
{
    public class MockDbSettings : ADbSettings
    {
        private const string _tryGetValue = "Test";
        private static Dictionary<string, string> _getAllDict = new Dictionary<string, string>();

        public MockDbSettings(string dictionaryName, IDbConnection connection) : base(dictionaryName, connection) { }

        public MockDbSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString) : base(dictionaryName, dbFactory, connectionString) { }

        protected override int AddOrUpdateValueInDb(IDbConnection connection, string key, string value)
        {
            return 1;
        }

        protected override IReadOnlyDictionary<string, string> GetAllValuesFromDb(IDbConnection connection)
        {
            return _getAllDict;
        }

        protected override bool TryGetValueFromDb(IDbConnection connection, string key, out string value)
        {
            value = _tryGetValue;
            return true;
        }
    }
}
