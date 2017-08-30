using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.PreferenceService;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace Tesco.Framework.UITesting.Services
{
      
    public class PreferenceServiceAdaptor : BaseAdaptor
    {
        static string a = string.Empty;
        List<string> PreferenceIds = new List<string>();
        public string GetPreference(string clubcardNumber, string strCulture)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);

            CustomerServiceAdaptor csa= new CustomerServiceAdaptor();
             long custId=csa.GetCustomerID(clubcardNumber,strCulture);
            string type = null;
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {

                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);
                List<string> PreferenceIds = new List<string>();
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();
                    string PrefID = string.Empty;

                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.POptStatus == OptStatus.OPTED_IN)
                        {
                            PrefID = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(PrefID);
                        }
                    }
                    if (PreferenceIds.Contains("13"))
                        type = Enums.Preferences.XmasSaver.ToString();
                    else if (PreferenceIds.Contains("17"))
                        type = Enums.Preferences.VirginAtlantic.ToString();
                    else if (PreferenceIds.Contains("11") || PreferenceIds.Contains("12"))
                        type = Enums.Preferences.Avios.ToString();
                    else if (PreferenceIds.Contains("10") || PreferenceIds.Contains("14"))
                        type = Enums.Preferences.BAAvios.ToString();
                    else
                        type = Enums.Preferences.NoPreference.ToString();
                }
            }
            return type;
        }

        public List<string> ViewPreference(string clubcardNumber, string strCulture)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);

            CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            long custId = csa.GetCustomerID(clubcardNumber, strCulture);            
          
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {

                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);                
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();
                    
                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.PreferenceID.ToString().Equals("7") || pref.PreferenceID.ToString().Equals("8") || pref.PreferenceID.ToString().Equals("9"))
                        {
                            a = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(a);
                        }
                    }
                }
            }
            return PreferenceIds;
        }

        public string GetPreference_contact(string clubcardNumber, string strCulture)
        {
            CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            long custId = csa.GetCustomerID(clubcardNumber, strCulture);
            string value = string.Empty;
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {

                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);
                List<string> PreferenceIds = new List<string>();
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();
                    string PrefID = string.Empty;

                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.POptStatus == OptStatus.OPTED_IN)
                        {
                            PrefID = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(PrefID);
                        }
                    }
                    switch (strCulture)
                    {
                        case "en-GB":
                            if (PreferenceIds.Contains("7") || PreferenceIds.Contains("8") || PreferenceIds.Contains("9"))
                            {
                                value = "UK";
                            }
                            break;
                        case "cs-CZ":
                        case "pl-PL":
                            for (int i = 0; i < PreferenceIds.Count; i++)
                            {
                                List<int> abc = PreferenceIds.Select(int.Parse).ToList();
                                    if(Enumerable.Range(27,35).Contains(abc[i] ))
                                    value = "Grp country";
                            }
                            break;
                    }
                }
            }
            return value;
        }

        public bool IsContactPreferenceEnabled(long custId, int id)
        {
            bool isEnabled = false;
            List<CustomerPreference> prefs = GetCustomerPreferences(custId);
            CustomerPreference pref = prefs.Find(p => p.PreferenceID == id);
            if (pref != null)
            {
                isEnabled = true;
            }
            return isEnabled;
        }

        public bool IsContactPreferenceOpted(long custId, int id)
        {
            bool isOpted = false;
            List<CustomerPreference> prefs = GetCustomerPreferences(custId);
            CustomerPreference pref = prefs.Find(p => p.PreferenceID == id);
            if (pref != null)
            {
                isOpted = pref.POptStatus == OptStatus.OPTED_IN;
            }
            return isOpted;
        }

        
         /// <summary>
         /// method to get the enabled preference ids
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="custId"></param>
         /// <returns></returns>
         public List<Int16> GetEnabledPreferences(long custId)
         {
             List<Int16> securityPrefs = new List<Int16>();
             List<CustomerPreference> prefs = new List<CustomerPreference>();
             prefs = GetCustomerPreferences(custId);
             prefs = prefs.FindAll(p => new short[] { 7, 8, 9, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38}.Contains(p.PreferenceID));
             prefs.ForEach(p => securityPrefs.Add(p.PreferenceID));
             return securityPrefs;
         }
 
         /// <summary>
         /// method to get the opted security preferences
         /// </summary>
         /// <param name="custId"></param>
         /// <param name="prefType"></param>
         /// <returns></returns>
         public List<Int16> GetOptedPreferences(long custId)
         {
             List<Int16> securityPrefs = new List<Int16>();
             List<CustomerPreference> prefs = new List<CustomerPreference>();
             prefs = GetCustomerPreferences(custId);
             prefs = prefs.FindAll(p => new short[] { 7, 8, 9, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 }.Contains(p.PreferenceID) && p.POptStatus == OptStatus.OPTED_IN);
             prefs.ForEach(p => securityPrefs.Add(p.PreferenceID));
             return securityPrefs;
         }

        private List<CustomerPreference> GetCustomerPreferences(long customerId)
        {           
            List<CustomerPreference> prefs = new List<CustomerPreference> ();
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {
                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(customerId, 0, true);
                List<string> PreferenceIds = new List<string>();
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    prefs = cs.Preference.ToList();
                }
            }
            return prefs;
        }

        public void GetPreference_dietary(string clubcardNumber, string strCulture, int prefId, string status)
        {
            CustomLogs.LogMessage("GetPreference_dietary started", TraceEventType.Start);
            CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            long custId = csa.GetCustomerID(clubcardNumber, strCulture);
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {
                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);
                List<string> PreferenceIds = new List<string>();
                List<string> PreferenceIds1 = new List<string>();
                List<int> abc = new List<int>();
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();
                    string PrefID = string.Empty;
                    string PrefID1 = string.Empty;
                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.POptStatus == OptStatus.OPTED_IN)
                        {
                            PrefID = pref.PreferenceID.ToString().Trim();
                            PreferenceIds.Add(PrefID);
                        }
                        if (pref.POptStatus == OptStatus.OPTED_OUT)
                        {
                            PrefID1 = pref.PreferenceID.ToString().Trim();
                            PreferenceIds1.Add(PrefID1);
                        }

                    }
                    switch (status)
                    {
                        case "OptIn":
                            for (int i = 0; i < PreferenceIds.Count; i++)
                            {
                                abc = PreferenceIds.Select(int.Parse).ToList();
                                if (Enumerable.Range(1, 5).Contains(abc[i]))
                                {
                                    if (prefId.Equals(abc[i]))
                                        CustomLogs.LogInformation("Preference ID " + abc[i] + "has been selected");
                                }
                            }
                            break;
                        case "OptOut":
                            for (int i = 0; i < PreferenceIds1.Count; i++)
                            {
                                abc = PreferenceIds1.Select(int.Parse).ToList();
                                if (Enumerable.Range(1, 5).Contains(abc[i]))
                                {
                                    if (prefId.Equals(abc[i]))
                                        CustomLogs.LogInformation("Preference ID " + abc[i] + "has been un selected");
                                }
                            }
                            break;
                    }
                }
            }
            CustomLogs.LogMessage("GetPreference_dietary completed", TraceEventType.Stop);
        }

        public bool CheckDietaryPreference_optin(string clubcardNumber, string strCulture, int prefId)
        {
            CustomLogs.LogMessage("CheckDietaryPreference_optin started", TraceEventType.Start);
            CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            long custId = csa.GetCustomerID(clubcardNumber, strCulture);

            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {
                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);
                List<string> PreferenceIds = new List<string>();
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();

                    string PrefID = string.Empty;

                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.CustomerPreferenceType == 2)
                        {
                            if (pref.POptStatus == OptStatus.OPTED_OUT)
                            {
                                PrefID = pref.PreferenceID.ToString().Trim();
                                PreferenceIds.Add(PrefID);
                            }
                        }
                    }

                }
                if (PreferenceIds.Contains(prefId.ToString()))
                {
                    CustomLogs.LogInformation("Preference ID is already Opted out");
                    return true;
                }
                else
                {
                    CustomLogs.LogInformation("Preference ID is already Opted in");
                    return false;
                }
            }
        }

        public List<string> CheckDietaryPreferencesinDB(string clubcardNumber, string strCulture)
        {
            CustomerServiceAdaptor csa = new CustomerServiceAdaptor();
            List<string> PreferenceDesc = new List<string>();
            string PrefDesc = string.Empty;
            long custId = csa.GetCustomerID(clubcardNumber, strCulture);
            // string value1 = string.Empty;
            using (PreferenceServiceClient client = new PreferenceServiceClient())
            {

                Tesco.Framework.UITesting.Services.PreferenceService.CustomerPreference cs = client.ViewCustomerPreference(custId, 0, true);
                if (cs != null && cs.Preference != null && cs.Preference.ToList().Count > 0)
                {
                    // To load the Opted Preference
                    List<CustomerPreference> objPreferenceFilter = new List<CustomerPreference>();
                    objPreferenceFilter = cs.Preference.ToList();

                    foreach (var pref in objPreferenceFilter)
                    {
                        if (pref.CustomerPreferenceType == 2)
                        {
                            PrefDesc = pref.PreferenceDescriptionEng.ToString().Trim();
                            PreferenceDesc.Add(PrefDesc);
                        }
                    }
                }
                return PreferenceDesc;

            }
        }

        public string ContactPreferenceStatus(long custId, int id)
        {
            string prefStatus=String.Empty;
            List<CustomerPreference> prefs = GetCustomerPreferences(custId);
            CustomerPreference pref = prefs.Find(p => p.PreferenceID == id);
            if (pref != null)
            {
                prefStatus = pref.POptStatus.ToString();
            }
            return prefStatus;
        }

    }
}
