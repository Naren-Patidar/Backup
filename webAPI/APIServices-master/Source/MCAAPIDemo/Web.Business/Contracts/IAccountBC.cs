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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IAccountBC
    {
        long GetCustomerId(long clubcardNumber, int maxRows, string culture);
        long GetHouseholdId(long clubcardNumber, string culture);
        void ParseActivationStatus(ref Activation.CustomerActivationStatusdetails activationDetails, string dotcomcustomerId, string culture);
        CustomerSecurityBlockerStatus ParseSecurityVerificationStatus(string customerID);
        List<HouseholdCustomerDetails> GetHouseHoldCustomersData(long customerId, string culture);
        List<Clubcard> GetClubcardsCustomerData(long customerID, string culture);
        void NoteSecurityAttemptInAudit(CustomerSecurityAttemptAudit securityAttemptAudit);
        AccountDetails GetMyAccountDetail(long customerID, string culture);
        DbConfiguration GetDBConfigurations(List<DbConfigurationTypeEnum> configurationTypes, string culture);
     //   string GetCustomerFullName(HouseholdCustomerDetails household);
        bool IsAccountDuplicate(CustomerFamilyMasterDataUpdate customerData);
    }
}
