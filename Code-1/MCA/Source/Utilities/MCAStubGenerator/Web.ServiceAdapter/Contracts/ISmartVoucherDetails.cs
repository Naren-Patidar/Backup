using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Entities;
using Tesco.ClubcardProducts.MCA.Web.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Entities.Boost; 

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface ISmartVoucherDetails
    {
        List<VoucherDetails> GetUnusedVoucherDetailsForMLS(long customerID, long cardNumber, string culture);
        List<VoucherDetails> GetUnusedVoucherDetails(long customerID, long cardNumber, string culture);
        decimal GetRewardsDetail(string cardNumber);
        List<RewardAndPoints> GetCustomerVoucherValCPS(string cardNumber, string stDate, string enDate);
        List<VoucherRewardDetails> GetVoucherRewardDetails(long clubcardNumber);
        List<VoucherUsageSummary> GetUsedVoucherDetails(long clubcardNumber);
        List<MilesRewardDetails> GetRewardDetailsMiles(long clubcardNumber, int reasonCode);
    }
}
