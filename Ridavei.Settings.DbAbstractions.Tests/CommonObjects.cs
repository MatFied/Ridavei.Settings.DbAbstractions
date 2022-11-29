using System.Data;

using Ridavei.Settings.DbAbstractions.Tests.DBMock;

using NSubstitute;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    internal class CommonObjects
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
    }
}
