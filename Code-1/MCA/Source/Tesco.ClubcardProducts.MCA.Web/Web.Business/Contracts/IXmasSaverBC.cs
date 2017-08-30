using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IXmasSaverBC
    {
        XmasSaverViewModel GetXmasSaverViewModel(string CustomerID, string Cluture);
        bool CheckCustomerIsXmasClubMember(string customerId, string culture);
        XmasSaverDates GetXmasDates();
        XmasSaverTopUpViewModel GetXmasSaverTopUpDetails(string customerId, string Cluture, DateTime startDate, DateTime endDate); 
        XmasSaverVoucherSavedViewModel GetXmasVoucherSavedSoFar(string cardNumber, string culture, DateTime startDate, DateTime endDate);
        XmasSaverViewModel CalculateBonus(XmasSaverViewModel model);
        string GetVouchersForXmasSaverCustomer(string custId, string culture);
    }
}
