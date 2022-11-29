using Ridavei.Settings.DbAbstractions.Tests.Manager;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    public static class SettingsExtension
    {
        public static SettingsBuilder UseMockProvider(this SettingsBuilder builder, bool getReturnsValue = true)
        {
            var manager = new MockDbManager(CommonObjects.MockDBProviderFactory, CommonObjects.MockConnectionString);
            manager.GetReturnsValue = getReturnsValue;
            return builder.SetManager(manager);
        }

        public static SettingsBuilder UseMockDbConnection(this SettingsBuilder builder, bool getReturnsValue = true)
        {
            var manager = new MockDbManager(CommonObjects.DBConnectionSubstitute());
            manager.GetReturnsValue = getReturnsValue;
            return builder.SetManager(manager);
        }
    }
}
