using System.Data;
using System.Data.Common;

namespace Ridavei.Settings.DbAbstractions.Tests.DBMock
{
    internal class MockDbConnection : DbConnection
    {
        public override string ConnectionString { get; set; }

        public override string Database => "";

        public override string DataSource => "";

        public override string ServerVersion => "";

        public override ConnectionState State => ConnectionState.Broken;

        public override void ChangeDatabase(string databaseName)
        {
        }

        public override void Close()
        {
        }

        public override void Open()
        {
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return null;
        }

        protected override DbCommand CreateDbCommand()
        {
            return null;
        }
    }
}
