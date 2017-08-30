using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using InstoreClubcardReward.Data;

namespace InstoreClubcardReward.Business
{
    public class KioskMaster
    {

        // from the IP address get the URL including parameters for the entry page
        // returns an empty string if not in Kiosk table
        public static string KioskEntryPage(string clientIP)
        {

            string ConnectionString = ConfigurationManager.ConnectionStrings["PrintVouchers_KioskDB"].ConnectionString;
            // basic url - web config setting
            string EntryPageURL = ConfigurationManager.AppSettings["EntryPageURL"];

            // string for constructing parameters
            string parameters = string.Empty;

            // collection for output from stored procedure
            try
            {
                System.Collections.ObjectModel.Collection<SelectKioskMasterRow> KioskRow;
                KioskRow = SelectKioskMaster.Execute(ConnectionString, clientIP);

                // if there is a record for the ip address
                if (KioskRow.Count == 1)
                {
                    // select on key so only one entry
                    foreach (SelectKioskMasterRow row in KioskRow)
                    {
                        parameters = string.Format("?KioskID={0}&StoreID={1}&KioskNo={2}", row.KioskID, row.StoreID, row.KioskNo);
                        break;
                    }
                    // ip address found so can be used
                    EntryPageURL = EntryPageURL + parameters;


                }
                else
                {
                    //  blank as not found
                    EntryPageURL = string.Empty;

                }
            }
            catch (Exception)
            {
                // error blank as en effect not found
                EntryPageURL = string.Empty;
            }

            return EntryPageURL;

        }
        // from the IP address get the parameters for the entry page
        // returns an empty string if not in Kiosk table
        public static string KioskEntryPageWCF(string clientIP)
        {

            string ConnectionString = ConfigurationManager.ConnectionStrings["PrintVouchers_KioskDB"].ConnectionString;
            // basic url - web config setting
            //string EntryPageURL = ConfigurationManager.AppSettings["EntryPageURL"];
            string rtnURLPara = string.Empty;

            // string for constructing parameters
            string parameters = string.Empty;

            // collection for output from stored procedure
            try
            {
                System.Collections.ObjectModel.Collection<SelectKioskMasterRow> KioskRow;
                KioskRow = SelectKioskMaster.Execute(ConnectionString, clientIP);

                // if there is a record for the ip address
                if (KioskRow.Count == 1)
                {
                    // select on key so only one entry
                    foreach (SelectKioskMasterRow row in KioskRow)
                    {
                        parameters = string.Format("?KioskID={0}&StoreID={1}&KioskNo={2}", row.KioskID, row.StoreID, row.KioskNo);
                        break;
                    }
                    // ip address found so can be used
                    //EntryPageURL = EntryPageURL + parameters;
                    rtnURLPara = parameters;

                }
                else
                {
                    //  blank as not found
                    //EntryPageURL = string.Empty;
                    rtnURLPara = string.Empty;

                }
            }
            catch (Exception)
            {
                // error blank as en effect not found
                //EntryPageURL = string.Empty;
                rtnURLPara = string.Empty;
            }

            //return EntryPageURL;
            return rtnURLPara;
        }

    }
}
