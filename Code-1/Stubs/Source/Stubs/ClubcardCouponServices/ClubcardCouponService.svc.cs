using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Configuration;



namespace ClubcardCouponServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class ClubcardCouponService : IClubcardCouponService
    {
        ClubcardCouponServicesProvider provider = new ClubcardCouponServicesProvider();


        public bool GetAvailableCoupons(out string errorXml, out List<CouponInformation> resultArray, out int totalCoupons, long houseHoldID)
        {
            return provider.GetAvailableCoupons(out errorXml, out resultArray, out totalCoupons, houseHoldID);
        }

        public bool GetRedeemedCoupons(out string errorXml, out string couponDetail, long houseHoldID, string culture)
        {
            return provider.GetRedeemedCoupons(out errorXml, out couponDetail, houseHoldID, culture);
        }
    }
}
