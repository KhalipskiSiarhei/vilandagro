using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Database.SqlCe.Tests
{
    [TestFixture]
    public class DbUpgraderRunnerTests
    {
        [TestCase("mode=CreateDb")]
        [TestCase("mode=CreateTestDb")]
        public void RunUpgradeWithParameters(string mode)
        {
            string connectionString = string.Format(
                "connectionString=Data Source={0}",
                string.Concat(Guid.NewGuid(), ".sdf"));
            Program.Main(new string[] { mode, connectionString });
        }
    }
}
