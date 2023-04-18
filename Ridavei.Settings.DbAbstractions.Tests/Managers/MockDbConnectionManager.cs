using System.Data;

using Ridavei.Settings.DbAbstractions.Managers;
using Ridavei.Settings.DbAbstractions.Settings;

using Ridavei.Settings.DbAbstractions.Tests.Settings;

namespace Ridavei.Settings.DbAbstractions.Tests.Managers
{
    public class MockDbConnectionManager : ADbConnectionManager
    {
        public bool GetReturnsValue = true;

        public MockDbConnectionManager(IDbConnection connection) : base(connection) { }

        protected override ADbSettings CreateDbSettings(string dictionaryName)
        {
            return new MockDbSettings(dictionaryName);
        }

        protected override bool TryGetDbSettingsObject(string dictionaryName, out ADbSettings settings)
        {
            settings = GetReturnsValue ? CreateDbSettings(dictionaryName) : null;
            return GetReturnsValue;
        }
    }
}