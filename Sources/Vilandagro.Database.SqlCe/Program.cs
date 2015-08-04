using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Vilandagro.Database.SqlCe
{
    public class Program
    {
        private const string ConnectionStringKey = "ConnectionString";

        private const string ModeKey = "Mode";

        private const string ConnectionStringPrefix = ConnectionStringKey + "=";

        private const string ModePrefix = ModeKey + "=";

        private static readonly ILog Log = LogManager.GetLogger<Program>();

        public static void Main(string[] args)
        {
            bool result = true;

            try
            {
                var connectionString = GetConnectionString(args);
                var mode = GetMode(args);
                var databaseUpgrader = new SqlCeDbUpgrader(mode, connectionString);

                result = databaseUpgrader.Upgrade();
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Unknown error", ex);
                result = false;
            }

            if (!result)
            {
                Console.ReadKey();
            }
        }

        private static string GetConnectionString(string[] args)
        {
            var connectionString =
                args.SingleOrDefault(a => a.StartsWith(ConnectionStringPrefix, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(connectionString))
            {
                connectionString = connectionString.Substring(ConnectionStringPrefix.Length);
            }

            return connectionString ?? ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
        }

        private static Mode GetMode(string[] args)
        {
            var modeStr =
                args.SingleOrDefault(a => a.StartsWith(ModePrefix, StringComparison.InvariantCultureIgnoreCase));
            Mode mode;

            if (!string.IsNullOrEmpty(modeStr))
            {
                modeStr = modeStr.Substring(ModePrefix.Length);
            }

            if (!string.IsNullOrEmpty(modeStr) && Enum.TryParse(modeStr, out mode))
            {
                return mode;
            }
            else
            {
                return (Mode)Enum.Parse(typeof(Mode), ConfigurationManager.AppSettings[ModeKey]);
            }
        }
    }
}