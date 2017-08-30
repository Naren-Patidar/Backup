using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.VoucherandCouponDetails;
namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IPDFGenerator
    {
        MemoryStream GetPDFDocumentStream<T, S>(List<T> list, S template, AccountDetails customerAccountDetails);
        MemoryStream GetCouponsAndVouchersDocument(VoucherandCouponDetails voucherandcoupondetailslist, AccountDetails customerAccountDetails, PdfBackgroundTemplate resourceValues);
    }
}
