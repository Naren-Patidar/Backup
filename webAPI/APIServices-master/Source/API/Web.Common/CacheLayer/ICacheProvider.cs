using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.API.Common.CacheLayer
{
    public interface ICacheProvider
    {
        void AddItem(string key, string value);
        void AddItem(string key, string value, TimeSpan lifespan);
        
        void RemoveItem(string key);
        void RemoveAllItems();

        int GetCount();
        string GetItem(string key);
        List<string> GetAllKeys();
        List<string> GetAllObjects();
        List<string> GetAllObjectsOfType<T>();
        Dictionary<string, string> GetAllItems();
    }
}
