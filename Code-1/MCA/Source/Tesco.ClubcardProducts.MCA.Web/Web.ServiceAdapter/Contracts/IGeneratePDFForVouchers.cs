using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PdfSharp.Pdf;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public  interface IGeneratePDFForVouchers
    {
        void PrintVouchersDocument(List<VoucherDetails> couponDetailsList, AccountDetails customerAccountDetails, CouponBackgroundTemplate template);
    }
}
