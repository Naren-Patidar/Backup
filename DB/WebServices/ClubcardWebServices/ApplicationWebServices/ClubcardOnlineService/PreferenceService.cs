/*Created By : Sabhareesan O.K
* Reason: To split preference related function to seperate perferenceservice  class to improve performance
* Created Date : 01-Feb-2012
 * Updated Date : 02-Feb-2012
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Tesco.com.ClubcardOnlineService;

namespace Tesco.com.ClubcardOnlineService
{
    public class PreferenceService : IPreferenceService
    {
        Customer customerObject = null;
        Preference preferenceObject = null;

        #region Customer Preferences-Sabhari

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID">CustomerID, for which the preferences to be updated.</param>
        /// <param name="culture">Culture</param>
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="resultXml">XML string which contains the list of preferences.</param>
        /// <returns></returns>
        public bool GetCustomerPreferences(Int64 customerID, string culture, out string errorXml, out string resultXml)
        {
            preferenceObject = new Preference();
            errorXml = string.Empty;
            resultXml = string.Empty;
            bool bResult = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.PreferenceService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.PreferenceService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);

                resultXml = preferenceObject.ViewCustomerPreference(customerID, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.PreferenceService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);

                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.PreferenceService.GetCustomerPreferences customerID-" + customerID + "  resultXml-" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.PreferenceService.GetCustomerPreferences   customerID-" + customerID + "  resultXml-" + resultXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.PreferenceService.GetCustomerPreferences    customerID-" + customerID + "  resultXml-" + resultXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.PreferenceService.GetCustomerPreferences ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }

        /// <summary>
        /// Updates customer preferences.
        /// </summary>
        /// <param name="updateXml">XML string containing the preferences to be updated.</param>
        /// <param name="consumer">UserID to updated amendby column in the DB.</param>
        /// <param name="errorXml">XML string, contains error detail if any.</param>
        /// <param name="customerID">CustomerID</param>
        /// <returns></returns>
        public bool UpdateCustomerPreferences(string updateXml, string consumer, out string errorXml, out Int64 customerID, char level)
        {
            preferenceObject = new Preference();
            errorXml = string.Empty;
            bool bResult = false;
            int userID = 0;
            customerID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                //Get the AmendBy Id
                userID = Helper.GetConsumerID(consumer);
                preferenceObject.UpdateCustomerPreference(updateXml, userID, out customerID, out errorXml, level);

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml);
                bResult = true;
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences customerID-" + customerID + "  updateXml-" + updateXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences  customerID-" + customerID + "  updateXml-" + updateXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.PreferenceService.UpdateCustomerPreferences ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                if (customerObject != null)
                {
                    customerObject = null;
                }

            }

            return bResult;
        }

        #endregion

    }
}
