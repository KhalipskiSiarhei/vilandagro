using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Database.MySql.Tests
{
    [TestFixture]
    public class MySqlDbUpgraderTests
    {
        private const string ConnectionString =
            "server=localhost;database=test;uid=root;password=";

        private DbUpgrader _dbUpgrader;

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateDb(Mode mode)
        {
            // Arrange
            _dbUpgrader = new MySqlDbUpgrader(mode, ConnectionString);

            // Act
            var result = _dbUpgrader.Upgrade();

            // Asserts
            Assert.IsTrue(result);
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateUpdateDb(Mode mode)
        {
            // Arrange
            _dbUpgrader = new MySqlDbUpgrader(mode, ConnectionString);
            var result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);

            _dbUpgrader = new MySqlDbUpgrader(Mode.Update, ConnectionString);
            result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);
        }
    }
}