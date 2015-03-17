using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using NUnit.Framework;

namespace Vilandagro.Web.Tests
{
    public abstract class TestFixtureBase
    {
        private readonly ILog _log;

        protected TestFixtureBase()
        {
            _log = LogManager.GetLogger<TestFixtureBase>();
        }

        protected ILog Log
        {
            get { return _log; }
        }
    }
}
