using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface ICustomerPreferenceBC
    {
        ClubDetails GetClubDetails(Int64 customerID);
        short GetSelectedStatementPreference(List<CustomerPreference> preferences);
        OptionsAndBenefitsModel GetOptionAndBenefitsModel(CustomerPreference preference, string customerId);
        bool UpdateCustomerPreferences(OptionsAndBenefitsModel optnModel, AccountDetails customerdetails, string customerId);
        bool UpdateClubDetails(long customerId, ClubDetails clubDetails, string emailIdTo);
        bool ValidateCustomerPreferences(OptionsAndBenefitsModel optnModel);
        int GetTabId(OptionsAndBenefitsModel optnModel);

        OptInsModel GetOptIns(CustomerPreference preference);
        OptInsModel GetOptIns(CustomerPreference preference, CustomerFamilyMasterData custDetails, bool isCustomerJoined);
        bool UpdateOptIns(OptInsModel model, AccountDetails customerdetails);

        ContactModel GetContactModel(CustomerPreference preference, CustomerFamilyMasterData customerData);
        bool UpdateContactPreferences(ContactModel model, AccountDetails customerdetails);
        CustomerFamilyMasterDataUpdate GetCustomerUpdateModel(CustomerFamilyMasterData customerData, ContactModel model, out bool checkRequired);
        bool ValidateContactPreferences(ContactModel contactModel);
        bool SendEmailToCustomer(long customerId, ContactModel objcustPref, AccountDetails customerdetails, string PageName);
        ContactModel RetainContactPrefValues(ContactModel model);
    }
}
