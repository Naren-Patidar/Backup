#region Name Spaces

using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using USLoyaltySecurityServiceLayer;
using System.Xml;
using Tesco.NGC.Loyalty.EntityServiceLayer;

#endregion

namespace Tesco.com.LoyaltyEntityServiceLayer
{
    /// <summary>
    /// Tesco Customer Details
    /// </summary>
    public class Utility
    {

        #region Fields

        /// <summary>
        ///Profanitywords
        /// </summary>
        private string profanitywords;

        #endregion


        #region Properties


        public string ProfanityWords { get { return this.profanitywords; } set { this.profanitywords = value; } }


        #endregion


        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Utility()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCMarketingProfanityConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["MarketingProfanityConnectionString"]);
            }
        }
        //end

        #region Methods

        #region ProfanityCheck
        /// <summary>
        /// ProfanityCheck -- It is used to check the profanity values.
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">maxRowCount</param>

        public String ProfanityCheck(string profanitywords, out int rowCount)
        {
            string sReqult = "";
            rowCount = 0;
            DataSet ds = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Utility.ProfanityCheck");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Utility.ProfanityCheck");
                
                this.ProfanityWords = Convert.ToString(profanitywords);
                

                object[] objDBParams = { ProfanityWords };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_PROFANITY_CHECK, objDBParams);
                ds.Tables[0].TableName = "ProfanityCheck";
                rowCount = ds.Tables[0].Rows.Count;
                sReqult = ds.GetXml();

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Utility.ProfanityCheck");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Utility.ProfanityCheck - sReqult:" + sReqult.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Utility.ProfanityCheck Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Utility.ProfanityCheckils Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Utility.ProfanityCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
                ds = null;
            }
            return sReqult;
        }
        #endregion

        #endregion
    }
}
