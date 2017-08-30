using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IActivationBC
    {
        ActivationRequestStatusEnum ProcessActivationRequest(string dotcomCustomerID, long clubcardNumber, ClubcardCustomer customerEntity);
        bool IsPrevCard(long clubcard);
    }
}