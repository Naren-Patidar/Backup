using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class ISOLanguage
    {
        public string ISOLanguageCode { get; set; }
        public string ISOLanguageDescEnglish { get; set; }
    }

    public class ISOLanguages
    {
        static List<ISOLanguage> _list = new List<ISOLanguage>();
        static ObjectCache _cache = MemoryCache.Default;
        public static bool IsDataLoaded = false;
        public static List<ISOLanguage> List
        {
            get {
                var languages = _cache.Get(typeof(ISOLanguage).Name);
                return languages as List<ISOLanguage>;
            }
        }


    }
}
