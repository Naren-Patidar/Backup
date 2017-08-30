using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable]
    public class Province
    {
        public int ProvinceID { get; set; }
        public string ProvinceNameEnglish { get; set; }
        public string ProvinceNameLocal { get; set; }
    }

    public class Provinces
    {
        static List<Province> _list = new List<Province>();
        static ObjectCache _cache = MemoryCache.Default;

        public static List<Province> List
        {
            get
            {
                var provinces = _cache.Get(typeof(Province).Name);
                return provinces as List<Province>;
            }
        }
    }
}
