using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Providers
{
    public abstract class CachingProviderBase
    {
        protected MemoryCache cache = new MemoryCache("CachingProvider");
        static readonly object padlock = new object();

        protected virtual void AddItem(string key, object value)
        {
            lock (padlock)
            {
                cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        }


        protected virtual void AddItem(string key, object value, DateTimeOffset duration)
        {
            lock (padlock)
            {
                cache.Add(key, value, duration);
            }
        }

        protected virtual void AddItem(string key, object value, CacheItemPolicy _policy)
        {
            lock (padlock)
            {
                cache.Add(key, value, _policy);
            }
        }

        protected virtual void RemoveItem(string key)
        {
            lock (padlock)
            {
                cache.Remove(key);
            }
        }

        protected virtual object GetItem(string key, bool remove = false)
        {
            lock (padlock)
            {
                var res = cache[key];

                if (res != null && remove)
                {
                    cache.Remove(key);
                }
                else
                {
                    //WriteToLog("CachingProvider-GetItem: Don't contains key: " + key);
                }

                return res;
            }
        }
    }
}

