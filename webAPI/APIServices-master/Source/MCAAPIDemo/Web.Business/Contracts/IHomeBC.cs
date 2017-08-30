using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IHomeBC
    {
        List<HouseholdCustomerDetails> GetHouseHoldCustomers(long CustomerID);
        HomeViewModel GetHomeViewModel(long customerId, string culture);
        StampModel GetStampViewModel(List<string> Urls);
    }
}
