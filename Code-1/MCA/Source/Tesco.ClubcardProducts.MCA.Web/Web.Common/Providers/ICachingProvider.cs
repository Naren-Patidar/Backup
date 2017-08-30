using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Providers
{
    public interface IGlobalCachingProvider
    {
        void AddItem(string key, object value, CacheItemPolicy _policy);
        void AddItem(string key, object value, DateTimeOffset duration);
        object GetItem(string key);
    }
}

