using Common.Logging;
using DbUp.Engine.Output;

namespace Vilandagro.Database
{
    public class UpgradeLog : IUpgradeLog
    {
        private ILog _log;

        public UpgradeLog(ILog log)
        {
            _log = log;
        }

        public void WriteInformation(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        public void WriteError(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
    }
}
