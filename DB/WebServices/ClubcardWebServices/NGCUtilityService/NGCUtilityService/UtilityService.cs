using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Xml;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Tesco.NGC.Security;
//using USLoyaltySecurityServiceLayer;
using Tesco.NGC.Utils;
//using System.Web.Security;
using NGCTrace;
using Tesco.com.LoyaltyEntityServiceLayer;

namespace Tesco.com.NGCUtilityService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class UtilityService : IUtilityService
    {

        #region ProfanityCheck
        /// <summary>
        /// ProfanityCheck -- It is used to check Profanity words aganist the database 
        /// </summary>
        /// <param name="string">conditionXml</param>
        /// <param name="int">out rowCount</param>
        public bool ProfanityCheck(string profanitywords, out string errorXml, out string resultXml, out int rowCount)
        {
            XmlDocument doc = null;
            resultXml = string.Empty;
            errorXml = string.Empty;
            rowCount = 0;
            Utility utilityObject = null;
            bool bResult = false;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.CustomerService.ProfanityCheck errorXml" + errorXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.CustomerService.ProfanityCheck errorXml" + errorXml + "resultXml" + resultXml);
                utilityObject = new Utility();

                doc = new XmlDocument();
                resultXml = utilityObject.ProfanityCheck(profanitywords, out rowCount);
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.CustomerService.ProfanityCheck errorXml" + errorXml + "resultXml" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.CustomerService.ProfanityCheck errorXml" + errorXml + "resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.CustomerService.ProfanityCheck  errorXml" + errorXml + "resultXml" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.CustomerService.ProfanityCheck errorXml" + errorXml + "resultXml" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.CustomerService.ProfanityCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                doc = null;
                errorXml = null;
                utilityObject = null;

            }
            return bResult;
        }
        #endregion

    }
}
