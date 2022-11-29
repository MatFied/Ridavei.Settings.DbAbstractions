using System.Data.Common;

namespace Ridavei.Settings.DbAbstractions.Tests.DBMock
{
    internal class MockDBProviderFactory : DbProviderFactory
    {
        public override DbConnection CreateConnection()
        {
            return new MockDbConnection();
        }
    }
}
