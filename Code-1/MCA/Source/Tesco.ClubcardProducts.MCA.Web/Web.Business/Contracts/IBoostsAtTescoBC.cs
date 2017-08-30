using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IBoostsAtTescoBC
    {
        RewardAndToken GetRewardAndTokens(long customerId);
        
        bool IsCurrenltyBCVEPeriod();
        bool IsExchangeEnabled();
        bool IsUnSpentBoostTokensAvailable(long customerId);
        bool RecordRewardTokenPrintDetails(List<Token> tokens, long customerId, string tokenFlag);
        List<Token> GetTokens(Guid gid, long bookingidval, long productlineidval, string culture);
    }
}
