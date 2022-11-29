using System.Data;
using System.Data.Common;

using Ridavei.Settings.Base;

using Ridavei.Settings.DbAbstractions.Manager;
using Ridavei.Settings.DbAbstractions.Settings;

using Ridavei.Settings.DbAbstractions.Tests.Settings;

namespace Ridavei.Settings.DbAbstractions.Tests.Manager
{
    public class MockDbManager : ADbManager
    {
        public bool GetReturnsValue = true;

        public MockDbManager(DbProviderFactory dbFactory, string connectionString) : base(dbFactory, connectionString) { }

        public MockDbManager(IDbConnection connection) : base(connection) { }

        protected override ADbSettings CreateDbSettings(string dictionaryName, DbProviderFactory dbFactory, string connectionString)
        {
            return new MockDbSettings(dictionaryName, dbFactory, connectionString);
        }

        protected override ADbSettings CreateDbSettings(string dictionaryName, IDbConnection connection)
        {
            return new MockDbSettings(dictionaryName, connection);
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, DbProviderFactory dbFactory, string connectionString, out ASettings settings)
        {
            settings = GetReturnsValue ? CreateDbSettings(dictionaryName, dbFactory, connectionString) : null;
            return GetReturnsValue;
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, IDbConnection connection, out ASettings settings)
        {
            settings = GetReturnsValue ? CreateDbSettings(dictionaryName, connection) : null;
            return GetReturnsValue;
        }
    }
}