using System;

using Ridavei.Settings.DbAbstractions.Managers;

using Ridavei.Settings.DbAbstractions.Tests.Base;
using Ridavei.Settings.DbAbstractions.Tests.Managers;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    [TestFixture]
    internal class ADbConnectionManagerTests : ADbManagerTests
    {
        protected override ADbManager Manager => new MockDbConnectionManager(CommonObjects.DBConnectionSubstitute());

        [Test]
        public void Constructor_NullDbConnection__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbConnectionManager(null);
            });
        }
    }
}
