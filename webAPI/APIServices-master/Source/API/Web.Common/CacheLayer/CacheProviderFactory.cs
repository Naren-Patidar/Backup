using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Tesco.ClubcardProducts.MCA.API.Common.CacheLayer
{
    public static class CacheProviderFactory
    {
        private static bool _RedisUnavailable = false;

        static string ActiveCacheProvider
        {
            get
            {
                return ConfigurationManager.AppSettings["activecacheprovider"];
            }
        }

        public static ICacheProvider GetActiveCacheProvider()
        {
            switch (CacheProviderFactory.ActiveCacheProvider)
            {
                case "redis":
                    try
                    {
                        if (!CacheProviderFactory._RedisUnavailable)
                        {
                            return new RedisCacheProvider();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        var sockEx = ex.GetBaseException();
                        if (sockEx != null && sockEx is System.Net.Sockets.SocketException)
                        {
                            if ((sockEx as System.Net.Sockets.SocketException).ErrorCode == 10061)
                            {
                                CacheProviderFactory._RedisUnavailable = true;
                                return null;
                            }
                        }
                        throw;
                    }

                default:
                    return null;
            }
        }
    }
}
