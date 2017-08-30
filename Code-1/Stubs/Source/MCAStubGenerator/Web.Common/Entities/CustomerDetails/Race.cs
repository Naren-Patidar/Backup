using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable]
    public class Race
    {
        public int RaceID { get; set; }
        public string Racedescenglish { get; set; }
        public string Racedesclocal { get; set; }
    }

    public class Races
    {
        static List<Race> _list = new List<Race>();
        static ObjectCache _cache = MemoryCache.Default;

        public static List<Race> List
        {
            get
            {
                var races = _cache.Get(typeof(Race).Name);
                return races as List<Race>;
            }
        }
    }
}
