using System;

using Ridavei.Settings.DbAbstractions.Managers;

using Ridavei.Settings.DbAbstractions.Tests.Base;
using Ridavei.Settings.DbAbstractions.Tests.DBMock;
using Ridavei.Settings.DbAbstractions.Tests.Managers;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    [TestFixture]
    internal class ADbProviderFactoryManagerTests : ADbManagerTests
    {
        protected override ADbManager Manager => new MockDbProviderFactoryManager(CommonObjects.MockDBProviderFactory, CommonObjects.MockConnectionString);

        [Test]
        public void Constructor_NullDBProviderFactory__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbProviderFactoryManager(null, null);
            });
        }

        [Test]
        public void Constructor_NullConnectionString__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbProviderFactoryManager(new MockDBProviderFactory(), null);
            });
        }
    }
}
