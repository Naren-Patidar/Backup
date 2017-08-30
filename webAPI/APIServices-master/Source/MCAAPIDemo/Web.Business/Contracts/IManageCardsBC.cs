using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Activation = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IManageCardsBC
    {
        ManageCardsViewModel GetManageCardsModel(List<HouseholdCustomerDetails> households, Dictionary<string, string> resources);
      //  string GetAllCustomerNames(List<HouseholdCustomerDetails> customers, string strSeparator);
        bool IsHidden(string key);
    }
}
