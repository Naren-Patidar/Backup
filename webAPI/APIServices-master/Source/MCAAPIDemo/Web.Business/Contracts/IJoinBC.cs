using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IJoinBC
    {
        PersonalDetailsViewModel GetJoinData(long customerId);
        void SetCustomerJoinData(PersonalDetailsViewModel viewModel, long customerID);
        bool IsProfaneText(PersonalDetailsViewModel viewModel, long customerID);
        bool IsLegalPolicyEnabled();
    }
}
