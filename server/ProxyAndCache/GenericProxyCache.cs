using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace ProxyAndCache
{
    internal class GenericProxyCache
    {
        private static ObjectCache cache = MemoryCache.Default;

        public DateTimeOffset dt_default = ObjectCache.InfiniteAbsoluteExpiration;
        public T Get<T>(string CacheItemName, params object[] paramArray)
        {

            object cachedItem = cache[CacheItemName];
            if (cachedItem is T t)
            {
                return t;
            }
            t = (T)Activator.CreateInstance(typeof(T), args: paramArray);
            cache.Add(CacheItemName, t, dt_default);
            return t;
        }

        public T Get<T>(string CacheItemName, double dt_seconds, params object[] paramArray)
        {
            object cachedItem = cache[CacheItemName];
            if (cachedItem is T t)
            {
                return t;
            }
            t = (T)Activator.CreateInstance(typeof(T), args: paramArray);
            cache.Add(CacheItemName, t, DateTimeOffset.Now.AddSeconds(dt_seconds));
            return t;
        }

        public T Get<T>(string CacheItemName, DateTimeOffset dt, params object[] paramArray)
        {
            object cachedItem = cache[CacheItemName];
            if (cachedItem is T t)
            {
                return t;
            }
            t = (T)Activator.CreateInstance(typeof(T), args: paramArray);
            cache.Add(CacheItemName, t, dt);
            return t;
        }



    }
}
