using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IPointsBC
    {
        PointsViewModel GetPointsViewdetails(long customerID, string culture);
        PointsSummaryModel GetPointsSummaryModel(long customerID, int offerid, string culture);
        CustomerTransactions GetCustomerTransactions(long customerID, int offerID, bool merchantFlag,string cultrure);
        List<Offer> GetOffersForCustomer(long customerId, string culture);
        PointsSummary GetPointsSummary(long customerID, int offerid, string culture);
        Dictionary<string, string> GetPreviousePoints(long customerID, string culture);
        decimal GetRewardsDetailForVoucherCustomer(long clubcardID, string culture);
        decimal GetRewardsDetailForSchemeCustomer(Int32 OptedPreference, Offer offer);
        Int32 GetOptedSchemeId(long customerID, out string optedPreference);
    }
}
