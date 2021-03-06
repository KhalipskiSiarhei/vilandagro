﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vilandagro.Core;

namespace Vilandagro.Infrastructure
{
    public class CustomRequestAware : IRequestAware
    {
        private static readonly object ObjSync = new object();

        private Dictionary<string, object> _keys;

        public CustomRequestAware()
        {
            _keys = new Dictionary<string, object>();
        }

        public virtual object this[string key]
        {
            get
            {
                lock (ObjSync)
                {
                    if (_keys.ContainsKey(key))
                    {
                        return _keys[key];
                    }

                    return null;
                }
            }

            set
            {
                lock (ObjSync)
                {
                    if (_keys.ContainsKey(key))
                    {
                        _keys[key] = value;
                    }
                    else
                    {
                        _keys.Add(key, value);
                    }
                }
            }
        }

        public T GetValue<T>(string key) where T : class
        {
            return GetValue<T>(key, false);
        }

        public T GetValue<T>(string key, bool throwExceptionIfNull) where T : class
        {
            lock (ObjSync)
            {
                var data = this[key] as T;

                if (data == null && throwExceptionIfNull)
                {
                    throw new InvalidOperationException(
                        string.Format("There is not a value with type {0} which is stored by the key {1}", typeof(T),
                            key));
                }

                return data;
            }
        }
    }
}