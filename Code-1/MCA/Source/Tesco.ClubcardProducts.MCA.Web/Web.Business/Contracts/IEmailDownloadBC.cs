using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{

    public interface IEmailDownloadBC
    {
         string GetCustomeridbyGUID(string guid);

         List<HouseholdCustomerDetails> GetHouseholdDetailsofCustomer(long customerid, string culture);

         List<VoucherDetails> GetUnusedVoucherDeatils(long customerID, long cardNumber, string culture);

         bool RecordCouponAndVoucherPrintedDataSet(List<VoucherDetails> voucherDetailsList, List<CouponDetails> couponDetailsList, string customerID, string cardNumber, int typeofcall);

         

    }
}
