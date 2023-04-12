using Ridavei.Settings.DbAbstractions.Tests.Managers;

namespace Ridavei.Settings.DbAbstractions.Tests.Extensions
{
    public static class SettingsExtensions
    {
        public static SettingsBuilder UseMockProvider(this SettingsBuilder builder, bool getReturnsValue = true)
        {
            return builder.SetManager(new MockDbProviderFactoryManager(CommonObjects.MockDBProviderFactory, CommonObjects.MockConnectionString)
            {
                GetReturnsValue = getReturnsValue
            });
        }

        public static SettingsBuilder UseMockDbConnection(this SettingsBuilder builder, bool getReturnsValue = true)
        {
            return builder.SetManager(new MockDbConnectionManager(CommonObjects.DBConnectionSubstitute())
            {
                GetReturnsValue = getReturnsValue
            });
        }
    }
}
