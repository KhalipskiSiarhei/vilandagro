using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Infrastructure
{
    public interface IRequestAware
    {
        object this[string key] { get; set; }

        T GetValue<T>(string key)
            where T : class;

        T GetValue<T>(string key, bool throwExceptionIfNull)
            where T : class;
    }
}