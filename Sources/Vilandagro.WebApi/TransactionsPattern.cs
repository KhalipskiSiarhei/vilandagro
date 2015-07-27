using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vilandagro.WebApi
{
    public enum TransactionsPattern
    {
        PerBatch = 0,
        PerRequest = 1,
    }
}