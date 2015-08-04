using DbUp;
using DbUp.Builder;

namespace Vilandagro.Database.SQLite
{
    public class SQLiteDbUpgrader : DbUpgrader
    {
        public SQLiteDbUpgrader(Mode mode, string connectionString)
            : base(mode, connectionString)
        {
        }

        protected override UpgradeEngineBuilder GetUpgradeEngineBuilder()
        {
            return DeployChanges.To.SQLiteDatabase(_connectionString).WithPreprocessor(new SQLitePreprocessor());
        }
    }
}