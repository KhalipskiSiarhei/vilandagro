using System;
using System.IO;
using NUnit.Framework;

namespace Vilandagro.Database.SQLite.Tests
{
    [TestFixture]
    public class SQLiteDbUpgraderTests
    {
        private DbUpgrader _dbUpgrader;
        private string _dbFileName;

        [SetUp]
        public void SetUp()
        {
            _dbFileName = string.Concat(Guid.NewGuid().ToString(), ".db");
        }

        [TearDown]
        public void TearDown()
        {
            if (_dbUpgrader != null && File.Exists(_dbUpgrader.GetPathOfConnectionString()))
            {
                File.Delete(_dbUpgrader.GetPathOfConnectionString());
            }
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateDb_DbIsConfiguredViaAbsolutePath(Mode mode)
        {
            // Arrange
            var dbFilePath = Path.Combine(Environment.CurrentDirectory, _dbFileName);

            _dbUpgrader = new SQLiteDbUpgrader(mode, string.Format("Data Source={0}; Version=3;", dbFilePath));

            // Act
            var result = _dbUpgrader.Upgrade();

            // Asserts
            Assert.IsTrue(result);
            Assert.IsTrue(dbFilePath == _dbUpgrader.GetPathOfConnectionString());
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateDb_DbIsConfiguredViaRelativePath(Mode mode)
        {
            // Arrange
            _dbUpgrader = new SQLiteDbUpgrader(mode, string.Format("Data Source={0}; Version=3;", _dbFileName));

            // Act
            var result = _dbUpgrader.Upgrade();

            // Asserts
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateDb_DbIsConfiguredViaDataDirectoryMask(Mode mode)
        {
            // Arrange
            _dbUpgrader = new SQLiteDbUpgrader(mode, string.Format(@"Data Source=|DataDirectory|\{0}; Version=3;", _dbFileName));

            // Act
            var result = _dbUpgrader.Upgrade();

            // Asserts
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateUpdateDb_DbIsConfiguredViaAbsolutePath(Mode mode)
        {
            // Arrange
            var dbFilePath = Path.Combine(Environment.CurrentDirectory, _dbFileName);
            var connectionString = string.Format("Data Source={0}; Version=3;", dbFilePath);
            _dbUpgrader = new SQLiteDbUpgrader(mode, connectionString);
            var result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);

            _dbUpgrader = new SQLiteDbUpgrader(Mode.Update, connectionString);
            result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateUpdateDb_DbIsConfiguredViaRelativePath(Mode mode)
        {
            // Arrange
            var connectionString = string.Format("Data Source={0}; Version=3;", _dbFileName);
            _dbUpgrader = new SQLiteDbUpgrader(mode, connectionString);
            var result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);

            _dbUpgrader = new SQLiteDbUpgrader(Mode.Update, connectionString);
            result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);
        }

        [Test]
        public void Update_DbDoesNotExist_DbCreatedAutomatically()
        {
            var connectionString = string.Format("Data Source={0}; Version=3;", string.Concat(Guid.NewGuid().ToString(), ".db"));
            _dbUpgrader = new SQLiteDbUpgrader(Mode.Update, connectionString);

            var result = _dbUpgrader.Upgrade();

            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }

        [Test]
        public void Update_DbDoesNotExistWithDataDirectoryMask_DbCreatedAutomatically()
        {
            var connectionString = string.Format(@"Data Source=|DataDirectory|\{0}; Version=3;",
                string.Concat(Guid.NewGuid().ToString(), ".db"));
            _dbUpgrader = new SQLiteDbUpgrader(Mode.Update, connectionString);

            var result = _dbUpgrader.Upgrade();

            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }
    }
}