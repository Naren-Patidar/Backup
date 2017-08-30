using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.ClubcardCouponService;
using System.Collections;
using System.Xml.Linq;
using System.Linq;

namespace Tesco.Framework.UITesting.Services
{
    public class ClubcardCouponAdaptor
    {
        public Int32 GetTotalCouponCount(string clubcardNumber, string culture, out Int32 totalCoupons)
        {
            Int32 count = 0;
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            Int64 householdId = customerAdaptor.GetHouseholdID(clubcardNumber, culture);
            using (ClubcardCouponServiceClient client = new ClubcardCouponServiceClient())
            {
                string errorXml = string.Empty;
                CouponInformation[] coupons;
                client.GetAvailableCoupons(out errorXml, out coupons, out totalCoupons, householdId);
                count = totalCoupons;
            }
            return count;
        }

        public Int32 GetAvailableCouponCount(string clubcardNumber, string culture, out Int32 totalCoupons)
        {
            Int32 count = 0;
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            Int64 householdId = customerAdaptor.GetHouseholdID(clubcardNumber, culture);
            using (ClubcardCouponServiceClient client = new ClubcardCouponServiceClient())
            {
                string errorXml = string.Empty;
                CouponInformation[] coupons;
                client.GetAvailableCoupons(out errorXml, out coupons, out totalCoupons, householdId);
                count = coupons.Length;
            }
            return count;
        }

        public Int32 GetUsedCouponCount(string clubcardNumber, string culture)
        {
            Int32 count = 0;
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            Int64 householdId = customerAdaptor.GetHouseholdID(clubcardNumber, culture);
            using (ClubcardCouponServiceClient client = new ClubcardCouponServiceClient())
            {
                string errorXml = string.Empty, coupons = string.Empty;
                
                client.GetRedeemedCoupons(out errorXml, out coupons, householdId, culture);
                XDocument xDoc = XDocument.Parse(coupons);
                List<string> couponDetails = (from t in xDoc.Descendants("UsedCouponDetail")
                         select  t.Element("RedemptionDate").GetValue<string>()).ToList();
                count = couponDetails.Count;
            }
            return count;
        }

        public List<string> GetUsedCouponUsedDates(string clubcardNumber, string culture)
        {
            List<string> dates = new List<string>();
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            Int64 householdId = customerAdaptor.GetHouseholdID(clubcardNumber, culture);
            using (ClubcardCouponServiceClient client = new ClubcardCouponServiceClient())
            {
                string errorXml = string.Empty, coupons = string.Empty;

                client.GetRedeemedCoupons(out errorXml, out coupons, householdId, culture);
                XDocument xDoc = XDocument.Parse(coupons);
                dates = (from t in xDoc.Descendants("UsedCouponDetail")
                         select t.Element("RedemptionDate").GetValue<string>().Substring(0, 10)).ToList();                
            }
            return dates;
        }

        public List<string> GetUsedCouponUsedStoreName(string clubcardNumber, string culture)
        {
            List<string> StoreName = new List<string>();
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            Int64 householdId = customerAdaptor.GetHouseholdID(clubcardNumber, culture);
            using (ClubcardCouponServiceClient client = new ClubcardCouponServiceClient())
            {
                string errorXml = string.Empty, coupons = string.Empty;

                client.GetRedeemedCoupons(out errorXml, out coupons, householdId, culture);
                XDocument xDoc = XDocument.Parse(coupons);
                StoreName = (from t in xDoc.Descendants("UsedCouponDetail")
                             select t.Element("RedemptionStoreName").GetValue<string>()).ToList();
            }
            return StoreName;
        }

        


    }
}
