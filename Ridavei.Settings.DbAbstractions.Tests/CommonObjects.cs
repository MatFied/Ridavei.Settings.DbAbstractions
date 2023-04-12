using System;
using System.Data;

using Ridavei.Settings.DbAbstractions.Tests.DBMock;
using Ridavei.Settings.DbAbstractions.Tests.Settings;

using NSubstitute;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    internal static class CommonObjects
    {
        public static IDbConnection DBConnectionSubstitute()
        {
            var res = Substitute.For<IDbConnection>();
            res.State.Returns(ConnectionState.Broken);
            res.When(x => x.Close()).Do(x => res.State.Returns(ConnectionState.Closed));
            return res;
        }

        public static readonly MockDBProviderFactory MockDBProviderFactory = new MockDBProviderFactory();
        public const string MockConnectionString = "ConnectionString";
        private const string DictionaryName = "Test";

        public static MockDbSettings CreateMockDbSettings()
        {
            var res = new MockDbSettings(DictionaryName);
            res.SetRunQueryMethod(MockRunQuery);
            return res;
        }

        private static object MockRunQuery(Func<IDbConnection, object> func)
        {
            return func(null);
        }
    }
}
