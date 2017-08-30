using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sider;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

/*
 * C:\Windows\system32>sc create redis-master binpath= "\"C:\Program Files\Redis\master\RedisService_1.1.exe\" \"C:\Program
 Files\Redis\master\redis.conf\"" start= "auto" DisplayName= "RedisMaster"
http://androidyou.blogspot.in/2012/02/how-to-test-c-redis-client-sider-step.html
 * https://github.com/chakrit/sider
 * */
namespace Tesco.ClubcardProducts.MCA.API.Common.CacheLayer
{
    class RedisCacheProvider : ICacheProvider
    {
        static readonly object padlock = new object();
        RedisClient _rClient = null;

        public string MasterHost 
        {
            get
            {
                string sHost = ConfigurationManager.AppSettings["redishost"];
                return String.IsNullOrWhiteSpace(sHost) ? "localhost" : sHost;
            }
        }

        public int Port 
        { 
            get 
            {
                int iPort = 0;
                if (!Int32.TryParse(ConfigurationManager.AppSettings["redisport"], out iPort))
                {
                    iPort = 6379;
                }
                return iPort; 
            } 
        }

        public string RedisPassword 
        { 
            get
            {
                string spwd = ConfigurationManager.AppSettings["redispassword"];
                if (!String.IsNullOrWhiteSpace(spwd))
                {
                    return CryptoUtility.DecryptTripleDES(spwd);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public RedisCacheProvider()
        {
            this._rClient = new RedisClient(this.MasterHost, this.Port);
            if (!String.IsNullOrWhiteSpace(this.RedisPassword))
            {
                this._rClient.Auth(this.RedisPassword);
            }
        }

        public int LifeSpan 
        {
            get
            {
                int iSpan = 300;
                string s = ConfigurationManager.AppSettings["cachelifespanseconds"];
                if (!Int32.TryParse(s, out iSpan))
                {
                    iSpan = 300;
                }
                return iSpan;
            }
        }

        #region ICacheProvider

        public void AddItem(string key, string value)
        {
            this.AddItem(key, value, new TimeSpan(0, 0, this.LifeSpan));
        }

        public void AddItem(string key, string value, TimeSpan expiry)
        {
            lock (padlock)
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new Exception("key cannot be empty");
                }

                key = key.ToLower();

                if (this._rClient.Keys("").Contains(key))
                {
                    this.RemoveItem(key);
                }

                this._rClient.Set(key, value);
                this._rClient.Expire(key, expiry);
            }
        }

        public void RemoveItem(string key)
        {
            lock (padlock)
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new Exception("key cannot be empty");
                }

                key = key.ToLower();
                string data = this._rClient.Get(key);
                if (!String.IsNullOrWhiteSpace(data))
                {
                    this._rClient.Expire(key, new TimeSpan(0, 0, 0, 0, 10));
                }
            }
        }

        public void RemoveAllItems()
        {
            lock (padlock)
            {
                this._rClient.FlushAll();
            }
        }

        public int GetCount()
        {
            lock (padlock)
            {
                return this._rClient.Keys("*").Length;
            }
        }

        public string GetItem(string key)
        {
            lock (padlock)
            {
                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new Exception("key cannot be empty");
                }

                return this._rClient.Get(key.ToLower());
            }
        }

        public List<string> GetAllKeys()
        {
            lock (padlock)
            {
                return this._rClient.Keys("*").ToList<string>();
            }
        }

        public List<string> GetAllObjects()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllObjectsOfType<T>()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetAllItems()
        {
            throw new NotImplementedException();
        }

        #endregion ICacheProvider

    }
}
