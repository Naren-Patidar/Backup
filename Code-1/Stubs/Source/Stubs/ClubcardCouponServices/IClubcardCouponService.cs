using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;


namespace ClubcardCouponServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IClubcardCouponService
    {

        [OperationContract]
        bool GetAvailableCoupons(out string errorXml, out List<CouponInformation> resultArray, out int totalCoupons, long houseHoldID);
        [OperationContract]
        bool GetRedeemedCoupons(out string errorXml, out string couponDetail, long houseHoldID, string culture);
        
        
        // TODO: Add your service operations here
    }


}
