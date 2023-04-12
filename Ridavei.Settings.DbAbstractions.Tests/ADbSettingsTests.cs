using Ridavei.Settings.DbAbstractions.Tests.Settings;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    [TestFixture]
    internal class ADbSettingsTests
    {
        private const string KeyName = "Name";
        private const string KeyValue = "Value";

        private MockDbSettings TestObj = CommonObjects.CreateMockDbSettings();

        [Test]
        public void Set_DBConnection__NoException()
        {
            Should.NotThrow(() =>
            {
                TestObj.Set(KeyName, KeyValue);
            });
        }

        [Test]
        public void Get_DBConnection__GetValue()
        {
            Should.NotThrow(() =>
            {
                TestObj.Get(KeyName).ShouldBe("Test");
            });
        }

        [Test]
        public void GetAll_DBConnection__GetDictionary()
        {
            Should.NotThrow(() =>
            {
                var dict = TestObj.GetAll();
                dict.ShouldNotBeNull();
                dict.Count.ShouldBe(0);
            });
        }
    }
}
