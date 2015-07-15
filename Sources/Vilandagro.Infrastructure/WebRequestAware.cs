using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vilandagro.Core;

namespace Vilandagro.Infrastructure
{
    public class WebRequestAware : IRequestAware
    {
        public object this[string key]
        {
            get { return HttpContext.Current.Items[key]; }
            set { HttpContext.Current.Items[key] = value; }
        }

        public T GetValue<T>(string key) where T : class
        {
            return GetValue<T>(key, false);
        }

        public T GetValue<T>(string key, bool throwExceptionIfNull) where T : class
        {
            var value = this[key] as T;

            if (value == null && throwExceptionIfNull)
            {
                throw new InvalidOperationException(
                    string.Format("There is not an object with the key={0} in the request aware object of type {1}", key,
                        GetType()));
            }
            return value;
        }
    }
}
