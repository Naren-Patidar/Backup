using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.ServiceManager
{
    public static class Extensions
    {
        public static string JsonText(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static double GetEpochTime(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime ConvertFromUnixTimestamp(this string timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            double dTimeStamp = 0;
            if (double.TryParse(timestamp, out dTimeStamp))
            {
                try
                {
                    return origin.AddSeconds(dTimeStamp);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    return origin.AddMilliseconds(dTimeStamp);
                }                
            }
            return origin;
        }
    }
}