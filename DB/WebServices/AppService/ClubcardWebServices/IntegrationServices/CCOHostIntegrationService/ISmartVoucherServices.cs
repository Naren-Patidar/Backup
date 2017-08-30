using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Tesco.com.IntegrationServices.Messages;


namespace Tesco.com.IntegrationServices
{
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
