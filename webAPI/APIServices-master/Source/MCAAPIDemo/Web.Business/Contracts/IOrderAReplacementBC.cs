using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
     public interface IOrderAReplacementBC
    {
         bool ProcessOrderReplacementRequest(OrderReplacementModel model);
         OrderAReplacementModel GetOrderAReplacementModel(long customerId, string culture);
    }
}
