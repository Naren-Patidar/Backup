using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Tesco.com.ClubcardOnline.Entities;
using System.Collections.Generic;

namespace Tesco.com.ClubcardOnline.CCOPreferences
{
    [ServiceContract]
    public interface IPreferenceService
    {
        [OperationContract]
        CustomerPreference ViewCustomerPreference(Int64 customerID, PreferenceType PreferenceType, bool optionalPreference);

        [OperationContract]
        ClubDetails ViewClubDetails(Int64 customerID);

        [OperationContract]
        void MaintainCustomerPreference(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails);

        [OperationContract]
        void MaintainClubDetails(long customerID, ClubDetails cluDetails, string sEmailDTo);

        [OperationContract]
        bool SendEmailToCustomers(CustomerPreference CustomerPreference, CustomerDetails customerDetails);

        [OperationContract]
        bool SendEmailNoticeToCustomers(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails, string Pagedetails, string trackxml);
    }
}
