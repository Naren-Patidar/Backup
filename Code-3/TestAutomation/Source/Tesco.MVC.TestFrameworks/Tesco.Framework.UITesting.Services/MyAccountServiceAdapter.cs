using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.MyAccountCustomerService;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Web;
using System.Xml;
using System.ServiceModel;
using System.IO;

namespace Tesco.Framework.UITesting.Services
{
    public class MyAccountServiceAdapter
    {
        CustomerSvcsCoreSDAClient customerSvcscoreSDAClient = null;

        public string GetPersonalDetails(int webCustomerID)
        {
                customerSvcscoreSDAClient = new CustomerSvcsCoreSDAClient();
               // return "<PersonalDetailsEntity>  <WebCustomerID>72720531</WebCustomerID>  <ClubcardNumber>634004022010553358</ClubcardNumber>  <DisplayName></DisplayName>  <EmailAddress>sam@devchanged.com</EmailAddress>  <Title>Mr  </Title>  <Surname>goyal</Surname>  <Forename>nitin</Forename>  <Initials></Initials>  <IsStaffMember>0</IsStaffMember>  <ClubcardPostcode>AL7 4FG </ClubcardPostcode> <PreviousEmailAddress>sam@dev.com</PreviousEmailAddress>  <RetailServicesCustomerID>72720531</RetailServicesCustomerID></PersonalDetailsEntity>";
                long CustomerID = Convert.ToInt64(customerSvcscoreSDAClient.CustomerMartiniIDGet(webCustomerID));
                if (CustomerID != 0)
                {
                    if (customerSvcscoreSDAClient.GetPersonalDetails(CustomerID) != null)
                    {

                        return customerSvcscoreSDAClient.GetPersonalDetails(CustomerID);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

         
        }
        
    }
}
