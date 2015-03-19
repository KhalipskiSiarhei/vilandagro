using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using DbUp;
using DbUp.Engine.Output;

namespace Vilandagro.Database.SqlCe
{
    public class DbUpgrader
    {
        private const string ItemTemplate = " - {0}";

        private readonly string _connectionString;

        private IUpgradeLog _upgradeLog;

        public DbUpgrader(string connectionString)
        {
            _upgradeLog = new UpgradeLog(LogManager.GetLogger<DbUpgrader>());
            _connectionString = connectionString;
        }

        /// <summary>
        /// Update DB to the newest version
        /// </summary>
        public bool Update()
        {
            return RunUpgrade(_connectionString, @"Scripts\Updates", "DB Update");
        }

        /// <summary>
        /// Upgrade DB to the newest version and run test data
        /// </summary>
        /// <returns></returns>
        public bool UpgradeAndRunTestData()
        {
            return Update() && RunUpgrade(_connectionString, @"Scripts\TestData", "Test Data");
        }

        private bool RunUpgrade(string connectionString, string scriptsPath, string stepName)
        {
            var upgraded = false;

            try
            {
                var fullScriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, scriptsPath);
                var updatesDeployer =
                    DeployChanges.To.SqlCeDatabase(connectionString)
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
