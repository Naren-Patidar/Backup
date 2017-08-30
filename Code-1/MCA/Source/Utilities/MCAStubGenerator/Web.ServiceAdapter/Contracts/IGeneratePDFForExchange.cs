using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;

using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface IGeneratePDFForExchange
    {
        PdfDocument GetExchangeDocument(List<Token> tokensTobePrinted,BoostBackgroundTemplate resouceValues);

    }
}
