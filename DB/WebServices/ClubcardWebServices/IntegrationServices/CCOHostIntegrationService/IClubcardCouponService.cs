using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using CCOHostIntegrationService.CouponEnquiryService;

namespace Tesco.com.IntegrationServices
{
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IClubcardCouponService
    {
        [OperationContract]
        bool GetAvailableCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray);

        [OperationContract]
        bool GetRedeemedCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray);
    }
}