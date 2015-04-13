using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Database.SqlCe
{
    public enum Mode
    {
        /// <summary>
        /// Create new DB. It means create new DB by the specified path/name and run all existed updates
        /// </summary>
        CreateDb = 1,

        /// <summary>
        /// Create new test DB. It means create new DB bu the specified path/name, run all updates and run all test data
        /// </summary>
        CreateTestDb = 2,

        /// <summary>
        /// Update existed DB by the specified path/name
        /// </summary>
        Update = 3,
    }
}
