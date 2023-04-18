using System.Data;

using Ridavei.Settings.DbAbstractions.Managers;

using NUnit.Framework;
using Shouldly;

namespace Ridavei.Settings.DbAbstractions.Tests.Base
{
    internal abstract class ADbManagerTests
    {
        protected abstract ADbManager Manager { get; }

        [Test]
        public void RunQuery__GetValue()
        {
            Should.NotThrow(() =>
            {
                Manager.RunQuery(RunTestQuery).ShouldBe(1);
            });
        }

        private int RunTestQuery(IDbConnection connection)
        {
            return 1;
        }
    }
}
