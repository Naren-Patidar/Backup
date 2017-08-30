/*Created By : Satheesh
* Reason: To get customer specific statements
* Created Date : 07-Feb-2012
 * Updated Date : 07-Feb-2012
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Xml;
using System.Data;
using USLoyaltySecurityServiceLayer;
using NGCTrace;

namespace Tesco.com.ClubcardOnlineService
{
    public class LatestStatementService : ILatestStatementService
    {
        Customer customerObject = null;
        Clubcard clubcardObject = null;
        string errorXml = string.Empty;
        string resultXml = string.Empty;



        #region GetLatestStatement

        /// <summary>
        /// Gets the customer latest statement datails.
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="culture"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool GetLatestStatementDetails(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            clubcardObject = new Clubcard();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails resultXml" + resultXml);
                resultXml = clubcardObject.GetLatestStatementDetails(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails customerID" + customerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails resultXml" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails customerID" + customerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails customerID" + customerID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.ClubcardService.GetLatestStatementDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                errorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (clubcardObject != null)
                {
                    clubcardObject = null;
                }

            }

            return bResult;
        }
        #endregion
    }
}
