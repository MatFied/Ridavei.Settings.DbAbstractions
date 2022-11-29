using System;

using Ridavei.Settings.DbAbstractions.Tests.Settings;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    [TestFixture]
    internal class ADbSettingsTests
    {
        private const string DictionaryName = "Test";

        private const string KeyName = "Name";
        private const string KeyValue = "Value";

        private static readonly MockDbSettings SettingsDBConnection = new MockDbSettings(DictionaryName, CommonObjects.DBConnectionSubstitute());
        private static readonly MockDbSettings SettingsDBProviderFactory = new MockDbSettings(DictionaryName, CommonObjects.MockDBProviderFactory, CommonObjects.MockConnectionString);

        [Test]
        public void Constructor_NulllDbConnection__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbSettings(DictionaryName, null);
            });
        }

        [Test]
        public void Constructor_NullDBProviderFactory__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbSettings(DictionaryName, null, null);
            });
        }

        [Test]
        public void Constructor_NullConnectionString__RaisesException()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                new MockDbSettings(DictionaryName, CommonObjects.MockDBProviderFactory, null);
            });
        }

        [Test]
        public void Set_DBConnection__NoException()
        {
            Should.NotThrow(() =>
            {
                SettingsDBConnection.Set(KeyName, KeyValue);
            });
        }

        [Test]
        public void Set_DBProviderFactory__NoException()
        {
            Should.NotThrow(() =>
            {
                SettingsDBProviderFactory.Set(KeyName, KeyValue);
            });
        }

        [Test]
        public void Get_DBConnection__GetValue()
        {
            Should.NotThrow(() =>
            {
                SettingsDBConnection.Get(KeyName).ShouldBe("Test");
            });
        }

        [Test]
        public void Get_DBProviderFactory__GetValue()
        {
            Should.NotThrow(() =>
            {
                SettingsDBProviderFactory.Get(KeyName).ShouldBe("Test");
            });
        }

        [Test]
        public void GetAll_DBConnection__GetDictionary()
        {
            Should.NotThrow(() =>
            {
                var dict = SettingsDBConnection.GetAll();
                dict.ShouldNotBeNull();
                dict.Count.ShouldBe(0);
            });
        }

        [Test]
        public void GetAll_DBProviderFactory__GetDictionary()
        {
            Should.NotThrow(() =>
            {
                var dict = SettingsDBProviderFactory.GetAll();
                dict.ShouldNotBeNull();
                dict.Count.ShouldBe(0);
            });
        }
    }
}
