using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IXmasSaverBC
    {
        XmasSaverViewModel GetXmasSaverViewModel(string CustomerID, string Cluture);
        bool CheckCustomerIsXmasClubMember(string customerId, string culture);

    }
}
