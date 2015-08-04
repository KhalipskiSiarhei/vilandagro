using System;
using System.IO;
using System.Linq;
using Common.Logging;
using DbUp;
using DbUp.Builder;
using DbUp.Engine.Output;

namespace Vilandagro.Database.SqlCe
{
    public class SqlCeDbUpgrader : DbUpgrader
    {
        public SqlCeDbUpgrader(Mode mode, string connectionString)
            : base(mode, connectionString)
        {
        }

        protected override UpgradeEngineBuilder GetUpgradeEngineBuilder()
        {
            return DeployChanges.To.SqlCeDatabase(_connectionString);
        }
    }
}