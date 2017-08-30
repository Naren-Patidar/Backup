using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.IO;
using System.Xml.Serialization;
using Tesco.com.ClubcardOnline.Entities;
using System.Configuration;


namespace PreferenceServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class PreferenceService : IPreferenceService
    {
        PreferenceServiceProvider provider = new PreferenceServiceProvider();

        #region IPreferenceService Members

        public CustomerPreference ViewCustomerPreference(long customerID, PreferenceType PreferenceType, bool optionalPreference)
        {
            return provider.ViewCustomerPreference(customerID, PreferenceType, optionalPreference);
        }

        public ClubDetails ViewClubDetails(long customerID)
        {
            return provider.ViewClubDetails(customerID);
        }

        public void MaintainCustomerPreference(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails)
        {
            provider.MaintainCustomerPreference(customerID, customerPreference, customerDetails);
        }

        public void MaintainClubDetails(long customerID, ClubDetails cluDetails, string sEmailDTo)
        {
            provider.MaintainClubDetails(customerID, cluDetails, sEmailDTo);
        }

        public bool SendEmailToCustomers(CustomerPreference CustomerPreference, CustomerDetails customerDetails)
        {
            return provider.SendEmailToCustomers(CustomerPreference, customerDetails);
        }

        public bool SendEmailNoticeToCustomers(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails, string Pagedetails, string trackxml)
        {
            return provider.SendEmailNoticeToCustomers(customerID, customerPreference, customerDetails, Pagedetails,trackxml);
        }

        #endregion
    }
}
