using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Tesco.com.ClubcardOnline.Entities;

namespace PreferenceServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]    
    public interface IPreferenceService
    {
        [OperationContract]
        CustomerPreference ViewCustomerPreference(long customerID, PreferenceType PreferenceType, bool optionalPreference);

        [OperationContract]
        ClubDetails ViewClubDetails(long customerID);

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
