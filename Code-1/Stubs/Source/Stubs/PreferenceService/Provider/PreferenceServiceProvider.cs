using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using Tesco.com.ClubcardOnline.Entities;
using ServiceUtility;

namespace PreferenceServices
{
    public class PreferenceServiceProvider
    {
        Helper helper = new Helper();

        public CustomerPreference ViewCustomerPreference(long customerID, PreferenceType PreferenceType, bool optionalPreference)
        {
            CustomerPreference preferences = new CustomerPreference();
            string preferencesXml = string.Empty;
            string xmlFileName = string.Empty;

            try
            {
                xmlFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/ViewCustomerPreference/{0}.xml", customerID));
                preferencesXml = helper.LoadXMLFile(xmlFileName);

                // convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(preferencesXml);
                MemoryStream stream = new MemoryStream(byteArray);

                XmlSerializer serializer = new XmlSerializer(typeof(CustomerPreference));
                preferences = (CustomerPreference)serializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                preferences = null;
                throw ex;
            }
            return preferences;
        }

        public ClubDetails ViewClubDetails(long customerID)
        {
            ClubDetails clubDetails = new ClubDetails();
            string preferencesXml = string.Empty;
            string xmlFileName = string.Empty;

            try
            {
                xmlFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/ViewClubDetails/{0}.xml", customerID));
                preferencesXml = helper.LoadXMLFile(xmlFileName);


                // convert string to stream
                byte[] byteArray = Encoding.UTF8.GetBytes(preferencesXml);
                MemoryStream stream = new MemoryStream(byteArray);

                XmlSerializer serializer = new XmlSerializer(typeof(ClubDetails));
                clubDetails = (ClubDetails)serializer.Deserialize(stream);
            }
            catch (Exception ex)
            {
                clubDetails = null;
                throw ex;
            }
            return clubDetails;
        }

        public void MaintainCustomerPreference(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails)
        {
            string xmlFileName = string.Empty;
            string preferencesXml = string.Empty;
            CustomerPreference OrignalPreferences = new CustomerPreference();
            try
            {
                xmlFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/ViewCustomerPreference/{0}.xml", customerID));
                preferencesXml = helper.LoadXMLFile(xmlFileName);

                OrignalPreferences = helper.XMLStringToObject(typeof(CustomerPreference), preferencesXml) as CustomerPreference;

                foreach (CustomerPreference pref in customerPreference.Preference)
                {
                    CustomerPreference search = OrignalPreferences.Preference.Find(p => p.PreferenceID == pref.PreferenceID);
                    if (search != null)
                    {

                        search.POptStatus = pref.POptStatus;
                    }
                    else
                    {
                        OrignalPreferences.Preference.Add(pref);
                    }
                }
                preferencesXml = helper.ObjectToXMLString(typeof(CustomerPreference), OrignalPreferences);
                helper.WriteXmlFile(xmlFileName, preferencesXml);
            }
            catch (Exception ex) { throw ex; }
        }

        public void MaintainClubDetails(long customerID, ClubDetails cluDetails, string sEmailDTo)
        {
            string xmlFileName = string.Empty;
            string clubDetailsXml = string.Empty;
            try
            {
                xmlFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("DataSource/ViewClubDetails/{0}.xml", customerID));
                clubDetailsXml = helper.ObjectToXMLString(typeof(ClubDetails), cluDetails);
                helper.WriteXmlFile(xmlFileName, clubDetailsXml);
            }
            catch (Exception ex) { throw ex; }
        }

        public bool SendEmailToCustomers(CustomerPreference CustomerPreference, CustomerDetails customerDetails)
        {
            return true;
        }

        public bool SendEmailNoticeToCustomers(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails, string Pagedetails, string trackxml)
        {
            return true;
        }
        
    }
}