using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp;
using NUnit.Framework;

namespace Vilandagro.Database.SQLite.Tests
{
    [TestFixture]
    public class SQLiteSupportTests
    {
        private static string _dbFilePath;

        [SetUp]
        public void SetUp()
        {
            _dbFilePath = Path.Combine(Environment.CurrentDirectory, string.Concat(Guid.NewGuid().ToString(), ".db"));
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_dbFilePath))
            {
                File.Delete(_dbFilePath);
            }
        }

        [Test]
        public void CanUseSQLite()
        {
            var connectionString = string.Format("Data Source={0}; Version=3;", _dbFilePath);

            if (!File.Exists(_dbFilePath))
            {
                SQLiteConnection.CreateFile(_dbFilePath);
            }

            DeployChanges.To
                .SQLiteDatabase(connectionString)
                .WithScript("Script0001", "CREATE TABLE IF NOT EXISTS Foo (Id int)")
                .Build();
        }
    }
}