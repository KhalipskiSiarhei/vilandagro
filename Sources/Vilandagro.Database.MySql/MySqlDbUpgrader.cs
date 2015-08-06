using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUp;
using DbUp.Builder;

namespace Vilandagro.Database.MySql
{
    public class MySqlDbUpgrader : DbUpgrader
    {
        public MySqlDbUpgrader(Mode mode, string connectionString) : base(mode, connectionString)
        {
        }

        protected override bool CreateDb()
        {
            // Run update only. We do not create MySql DB here because it should has already been created
            return Update();
        }

        protected override UpgradeEngineBuilder GetUpgradeEngineBuilder()
        {
            return DeployChanges.To.MySqlDatabase(_connectionString).WithPreprocessor(new MySqlPreprocessor());
        }
    }
}