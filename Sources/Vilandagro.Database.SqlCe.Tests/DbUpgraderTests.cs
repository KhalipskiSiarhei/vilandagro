﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Vilandagro.Database.SqlCe.Tests
{
    [TestFixture]
    public class DbUpgraderTests
    {
        private DbUpgrader _dbUpgrader;
        private string _dbFileName;

        [SetUp]
        public void SetUp()
        {
            _dbFileName = string.Concat(Guid.NewGuid().ToString(), ".sdf");
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
            var dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dbFileName);

            _dbUpgrader = new DbUpgrader(mode, string.Format("Data Source={0}", dbFilePath));

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
            _dbUpgrader = new DbUpgrader(mode, string.Format("Data Source={0}", _dbFileName));

            // Act
            var result = _dbUpgrader.Upgrade();

            // Asserts
            Assert.IsTrue(result);
            Assert.IsFalse(_dbFileName == _dbUpgrader.GetPathOfConnectionString());
            Assert.IsTrue(_dbUpgrader.GetPathOfConnectionString().Contains(_dbFileName));
            Assert.IsTrue(File.Exists(_dbUpgrader.GetPathOfConnectionString()));
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateUpdateDb_DbIsConfiguredViaAbsolutePath(Mode mode)
        {
            // Arrange
            var dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dbFileName);
            var connectionString = string.Format("Data Source={0}", dbFilePath);
            _dbUpgrader = new DbUpgrader(mode, connectionString);
            var result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);

            _dbUpgrader = new DbUpgrader(Mode.Update, connectionString);
            result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);
        }

        [TestCase(Mode.CreateDb)]
        [TestCase(Mode.CreateTestDb)]
        public void CreateUpdateDb_DbIsConfiguredViaRelativePath(Mode mode)
        {
            // Arrange
            var connectionString = string.Format("Data Source={0}", _dbFileName);
            _dbUpgrader = new DbUpgrader(mode, connectionString);
            var result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);

            _dbUpgrader = new DbUpgrader(Mode.Update, connectionString);
            result = _dbUpgrader.Upgrade();
            Assert.IsTrue(result);
        }

        [Test]
        public void Update_DbDoesNotExist_FalseReturned()
        {
            var connectionString = string.Format("Data Source={0}", string.Concat(Guid.NewGuid().ToString(), ".sdf"));
            _dbUpgrader = new DbUpgrader(Mode.Update, connectionString);

            var result = _dbUpgrader.Upgrade();

            Assert.IsFalse(result);
        }
    }
}