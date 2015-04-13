using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vilandagro.Infrastructure
{
    public class ThreadRequestAware : IRequestAware
    {
        public object this[string key]
        {
            get
            {
                var data = Thread.GetData(Thread.GetNamedDataSlot(key));
                return data;
            }

            set
            {
                Thread.SetData(Thread.GetNamedDataSlot(key), value);
            }
        }

        public T GetValue<T>(string key) where T : class
        {
            return GetValue<T>(key, false);
        }

        public T GetValue<T>(string key, bool throwExceptionIfNull) where T : class
        {
            var data = this[key] as T;

            if (data == null && throwExceptionIfNull)
            {
                throw new InvalidOperationException(
                    string.Format("There is not a value with type {0} which is stored by the key {1}",
                        typeof (T), key));
            }

            return data;
        }
    }
}
