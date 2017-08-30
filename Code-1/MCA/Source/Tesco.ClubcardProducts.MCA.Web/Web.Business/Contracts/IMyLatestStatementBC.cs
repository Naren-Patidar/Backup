using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IMyLatestStatementBC
    {
        bool IsHoldingPageEnabled(out DateTime? ptsSummaryStartDate, out DateTime? ptsSummaryEndDate);
        decimal FindConversionRate(string sRewardMilesRateFlag, string sCustomerType);
        MyLatestStatementViewModel GetMLSViewDetails(long CustomerId);
    }
}
