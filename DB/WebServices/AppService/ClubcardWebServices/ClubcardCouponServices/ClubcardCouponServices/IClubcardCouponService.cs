using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ClubcardCouponServices.CouponEnquiryService;

namespace Tesco.com.ClubcardCouponService
{
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    [ServiceKnownType(typeof(List<object>))]
    public interface IClubcardCouponService
    {
        [OperationContract]
        bool GetAvailableCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray, out int totalCoupons);

        [OperationContract]
        bool GetRedeemedCoupons(long houseHoldID, out string errorXml, out string couponDetail, string culture);

        [OperationContract]
        bool GetCouponInformation(string smartBarcode, string smartAlphacode, out string errorXml, out string couponDetail, string culture);
    }
}
