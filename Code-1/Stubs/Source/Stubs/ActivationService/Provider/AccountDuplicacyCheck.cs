using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivationService.Provider
{
    public abstract class AccountDuplicacyCheck
    {
        public abstract bool IsAccountDuplicate(long ClubcardNumber, ClubcardOnline.Web.Entities.CustomerActivationServices.ClubcardCustomer customer, System.Data.DataSet dsConfig);

    }
}