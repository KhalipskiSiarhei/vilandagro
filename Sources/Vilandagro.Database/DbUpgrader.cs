﻿using System;
using System.IO;
using System.Linq;
using Common.Logging;
using DbUp.Builder;
using DbUp.Engine.Output;

namespace Vilandagro.Database
{
    public abstract class DbUpgrader
    {
        private const string DataSourceKey = "Data Source=";
        private const string DataDirectoryMask = "|DataDirectory|";

        protected const string ItemTemplate = " - {0}";

        protected readonly string _connectionString;

        protected Mode _mode;

        protected IUpgradeLog _upgradeLog;

        protected DbUpgrader(Mode mode, string connectionString)
        {
            _upgradeLog = new UpgradeLog(LogManager.GetLogger<DbUpgrader>());
            _mode = mode;
            _connectionString = connectionString;
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public virtual string GetPathOfConnectionString()
        {
            var dataSourceKeyValue = _connectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Single(s => s.StartsWith(DataSourceKey));
            var dataSource = dataSourceKeyValue.Replace(DataSourceKey, string.Empty);

            if (dataSource.Contains(DataDirectoryMask))
            {
                // TODO: This will not work for ASP.NET environment when ASP_Data folder is used... Need to fix it in the future in any way...
                dataSource = dataSource.Replace(DataDirectoryMask, Environment.CurrentDirectory);
            }
            return dataSource;
        }

        protected abstract UpgradeEngineBuilder GetUpgradeEngineBuilder();

        public bool Upgrade()
        {
            try
            {
                bool result;

                _upgradeLog.WriteInformation(
                    "Starting DB updating: connectionString={0}; mode={1}",
                    _connectionString,
                    _mode);

                if (_mode == Mode.CreateDb)
                {
                    result = CreateDb();
                }
                else if (_mode == Mode.CreateTestDb)
                {
                    result = CreateTestDb();
                }
                else if (_mode == Mode.Update)
                {
                    result = Update();
                }
                else
                {
                    throw new NotSupportedException(string.Format("Mode={0} is not supported by the application", _mode));
                }

                if (result)
                {
                    _upgradeLog.WriteInformation("Upgrade process has been finished successfully!");
                }
                else
                {
                    _upgradeLog.WriteWarning("Upgrade process has been failed, see messages above for more details");
                }

                return result;
            }
            catch (Exception ex)
            {
                _upgradeLog.WriteError("Unknown error during upgrading: {0}", ex);
                throw;
            }
        }

        /// <summary>
        /// Update existed DB to the newest version
        /// </summary>
        protected bool Update()
        {
            return RunUpgrade(@"Scripts/Updates", "DB Update");
        }

        /// <summary>
        /// Create new Db
        /// </summary>
        protected virtual bool CreateDb()
        {
            // Create empty DB
            File.Create(GetPathOfConnectionString()).Dispose();

            // Run update
            return Update();
        }

        /// <summary>
        /// Upgrade DB to the newest version and run test data
        /// </summary>
        /// <returns></returns>
        protected bool CreateTestDb()
        {
            return CreateDb() && RunUpgrade(@"Scripts/TestData", "Test Data");
        }

        protected bool RunUpgrade(string scriptsPath, string stepName)
        {
            var upgraded = false;

            try
            {
                var fullScriptsPath = Path.Combine(Environment.CurrentDirectory, scriptsPath);
                var upgradeEngineBuilder = GetUpgradeEngineBuilder();
                var updatesDeployer =
                    upgradeEngineBuilder
                        .WithScriptsFromFileSystem(fullScriptsPath)
                        .WithTransactionPerScript()
                        .LogScriptOutput()
                        .LogTo(_upgradeLog)
                        .Build();

                _upgradeLog.WriteInformation("Start step {0}", stepName);

                if (updatesDeployer.IsUpgradeRequired())
                {
                    _upgradeLog.WriteInformation("Step {0} is required", stepName);

                    var scriptsToExecute = updatesDeployer.GetScriptsToExecute();

                    if (scriptsToExecute.Any())
                    {
                        _upgradeLog.WriteInformation("There are the following sql-scripts to be executed:");
                        scriptsToExecute.ForEach(s => _upgradeLog.WriteInformation(ItemTemplate, s.Name));

                        var result = updatesDeployer.PerformUpgrade();

                        if (result.Successful)
                        {
                            upgraded = true;
                            _upgradeLog.WriteInformation("All script(s) have been applied successfully!!!");
                        }
                        else
                        {
                            var executedScripts = result.Scripts.ToList();

                            if (executedScripts.Any())
                            {
                                _upgradeLog.WriteWarning(
                                    "The following script(s) have been applied and then rollbecked because of error specified below");
                                executedScripts.ForEach(s => _upgradeLog.WriteWarning(ItemTemplate, s.Name));
                            }

                            _upgradeLog.WriteError("There is the following error during upgrade: {0}", result.Error);
                        }
                    }
                    else
                    {
                        _upgradeLog.WriteError(
                            "Step {0} is required but it was not get any scripts to be executed...",
                            stepName);
                    }
                }
                else
                {
                    _upgradeLog.WriteInformation("Step {0} is NOT required, there are not any sql-scripts to apply", stepName);
                    upgraded = true;
                }
            }
            catch (Exception ex)
            {
                _upgradeLog.WriteError(
                    "Unknown error has been occured during step {0}. Detailed error: {1}",
                    stepName,
                    ex);
            }

            _upgradeLog.WriteInformation("Step {0} has been finished", stepName);
            return upgraded;
        }
    }
}
