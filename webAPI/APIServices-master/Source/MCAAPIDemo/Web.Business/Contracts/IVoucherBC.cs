using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IVoucherBC
    {
        VouchersViewModel GetVoucherViewDetails(long customerId, string culture);
        AccountDetails GetCustomerAccountDetails(long customerId, string culture);
        //bool IsUnSpentVouchersExist(long customerId, string culture);
        //void PrintVoucher(List<VoucherDetails> voucherDetailsList, AccountDetails customerAccountDetails, PdfBackgroundTemplate template);
        //bool RecordVouchersPrinted(List<VoucherDetails> selectedVouchers, long customerId, long cardNumber);
    }
}
