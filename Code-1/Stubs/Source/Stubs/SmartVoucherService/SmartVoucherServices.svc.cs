using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace SmartVoucherService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class SmartVoucherServices : ISmartVoucherServices
    {
        SmartVoucherServiceProvider provider = new SmartVoucherServiceProvider();

        #region ISmartVoucherService Members

        public GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber)
        {
            return provider.GetRewardDtls(ClubcardNumber);
        }

        public GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber)
        {
            return provider.GetUnusedVoucherDtls(ClubcardNumber);
        }

        public GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber)
        {
            return provider.GetUsedVoucherDtls(ClubcardNumber);
        }

        public GetVoucherValHHRsp GetVoucherValHH(string Household_ID, string CPStartDate, string CPEndDate)
        {
            throw new NotImplementedException();
        }

        public GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode)
        {
            return provider.GetRewardDtlsMiles(ClubcardNumber, ReasonCode);
        }

        public GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate)
        {
            return provider.GetVoucherValCPS(Clubcard_Number, CPStartDate, CPEndDate);
        }

        #endregion
    }
}

