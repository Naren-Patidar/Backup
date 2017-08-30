using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Tesco.ClubcardProducts.MCA.Web.Business.Contracts
{
    public interface IPersonalDetailsBC
    {
        PersonalDetailsViewModel GetCustomerDataView(long customerId);
        bool SetCustomerDataView(PersonalDetailsViewModel viewModel, long customerID,Dictionary<string,string> resourceKeys);
        AddressDetails PopulateAddress(string postCodeInput);
        CustomerPreference GetCustomerPreferences(long customerId);
        CustomerFamilyMasterData GetCustomerDetails(long customerId);
        bool IsProfaneText(PersonalDetailsViewModel viewModel, long customerID, Dictionary<string, string> resourceKeys);
        bool IsAccountDuplicate(PersonalDetailsViewModel viewModel, long customerID, Dictionary<string, string> resourceKeys);
        bool UpdateCustomerDetails(CustomerFamilyMasterDataUpdate customerData);
        CustomerAddressVerificationModel GetMyAccountData(long dotcomCustomerID, Int64 customerID);
    }
}
