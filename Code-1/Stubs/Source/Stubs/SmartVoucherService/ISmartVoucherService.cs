using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SmartVoucherService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface ISmartVoucherServices
    {
        
        [OperationContract]
        GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber);

        [OperationContract]
        GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber);

        [OperationContract]
        GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber);

        [OperationContract]
        GetVoucherValHHRsp GetVoucherValHH(string Household_ID, string CPStartDate, string CPEndDate);

        [OperationContract]
        GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode);

        [OperationContract]
        GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate);
    
    }
}
