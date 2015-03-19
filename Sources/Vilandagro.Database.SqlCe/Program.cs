using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Vilandagro.Database.SqlCe
{
    class Program
    {
        private static readonly string Vilandagro = "Vilandagro";

        private static readonly string RunTestData = "RunTestData";

        private static readonly string ValandagroPrefix = Vilandagro + "=";

        private static readonly string RunTestDataPrefix = RunTestData + "=";

        private static readonly ILog _log = LogManager.GetLogger<Program>();

        static void Main(string[] args)
        {
            try
            {
                var connectionString = GetConnectionString(args);
                var runTestData = GetRunTestData(args);
                var dbUpgrader = new DbUpgrader(connectionString);

                _log.InfoFormat("Starting DB updating: connectionString={0}; runTestData={1}", connectionString,
                    runTestData);
                if (runTestData)
                {
                    dbUpgrader.UpgradeAndRunTestData();
                }
                else
                {
                    dbUpgrader.Update();
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Unknown error", ex);
            }

            Console.ReadKey();
        }

        private static string GetConnectionString(string[] args)
        {
            var connectionString =
                args.SingleOrDefault(a => a.StartsWith(ValandagroPrefix, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(connectionString))
            {
                connectionString = connectionString.Substring(ValandagroPrefix.Length);
            }

            return connectionString ?? ConfigurationManager.ConnectionStrings[Vilandagro].ConnectionString;
        }

        private static bool GetRunTestData(string[] args)
        {
            var runTestData =
                args.SingleOrDefault(a => a.StartsWith(RunTestDataPrefix, StringComparison.InvariantCultureIgnoreCase));
            bool runTestDataValue;

            if (!string.IsNullOrEmpty(runTestData))
            {
                runTestData = runTestData.Substring(RunTestDataPrefix.Length);
            }

            if (!string.IsNullOrEmpty(runTestData) && Boolean.TryParse(runTestData, out runTestDataValue))
            {
                return runTestDataValue;
            }
            else
            {
                return Boolean.Parse(ConfigurationManager.AppSettings[RunTestData]);
            }
        }
    }
}
