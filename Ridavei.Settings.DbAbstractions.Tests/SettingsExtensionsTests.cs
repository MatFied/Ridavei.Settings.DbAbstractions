using Ridavei.Settings.DbAbstractions.Tests.Extensions;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests
{
    [TestFixture]
    internal class SettingsExtensionsTests
    {
        private readonly SettingsBuilder _builder = SettingsBuilder.CreateBuilder();

        private const string DictionaryName = "Test";

        [Test]
        public void GetSettings_MockProvider__RetrieveSettings()
        {
            Should.NotThrow(() =>
            {
                using (var settings = _builder
                    .UseMockProvider()
                    .GetSettings(DictionaryName))
                {
                    settings.ShouldNotBeNull();
                }
            });
        }

        [Test]
        public void GetSettings_MockDbConnection__RetrieveSettings()
        {
            Should.NotThrow(() =>
            {
                using (var settings = _builder
                    .UseMockDbConnection()
                    .GetSettings(DictionaryName))
                {
                    settings.ShouldNotBeNull();
                }
            });
        }

        [Test]
        public void GetOrCreateSettings_MockProvider__RetrieveSettings()
        {
            Should.NotThrow(() =>
            {
                using (var settings = _builder
                    .UseMockProvider(false)
                    .GetOrCreateSettings(DictionaryName))
                {
                    settings.ShouldNotBeNull();
                }
            });
        }

        [Test]
        public void GetOrCreateSettings_MockDbConnection__RetrieveSettings()
        {
            Should.NotThrow(() =>
            {
                using (var settings = _builder
                    .UseMockDbConnection(false)
                    .GetOrCreateSettings(DictionaryName))
                {
                    settings.ShouldNotBeNull();
                }
            });
        }
    }
}
