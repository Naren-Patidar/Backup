using System;
using System.Collections;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Text;
using System.Xml;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Tesco.NGC.Security;
using Fujitsu.eCrm.Generic.SharedUtils;
using USLoyaltySecurityServiceLayer;
using Tesco.NGC.Utils;
using System.Web.Security;
using NGCTrace;
using Tesco.NGC.SecurityLayer;

namespace Tesco.com.ClubcardOnlineService
{
    public class MarketingService : IMarketingService
    {
        //create the individual object for each class
        Agency agencyObject = null;
        Role RoleObject = null;
        Offer offobj = null;
        Coupons couponobj = null;
        Vouchers vocherobj = null;
        PartnerOutlet partnerOutletObject = null;
        Partner partnerObject = null;
        #region Agency


        public bool ViewAgency(int agencynumber, string culture, out string resultsDoc, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            resultsDoc = string.Empty;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append(" AgencyNumber: " + agencynumber);
            sb.Append(" Culture: " + culture);
            trace.WriteInfo(sb.ToString());
            #endregion


            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                //SessionTable st = new SessionTable ;
                //Session currentSession = st.Validate(t, sessionID.ToString(), "Get");

                agencyObject = new Agency();
                resultsDoc = agencyObject.View(agencynumber, culture);

                if (resultsDoc != null && resultsDoc != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "AgencyNumber:" + agencynumber.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool AddAgency(string updateXML, short sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.AddAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                agencyObject = new Agency();
                agencyObject.Add(updateXML, sessionID, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }
        #endregion


        #region Find Partner/Agency


        //Updated by :Sabhareesan O.K 
        //Purpose: Change from web serices to WCF
        /// <summary>
        /// Function is to search the agency with respect to conditional XML and culture
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool SearchAgency(string conditionXml, int maxRowCount, out int rowCount, string culture, out string resultsDoc, out string resultXml)
        {
            bool bSearchResult = false;
            resultXml = string.Empty;
            resultsDoc = string.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.SearchAgency conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.SearchAgency resultXml:" + resultXml);

                agencyObject = new Agency();
                resultsDoc = agencyObject.Search(conditionXml, maxRowCount, out rowCount, culture);

                if (resultsDoc != null && resultsDoc != "</NewDataSet>")
                {
                    bSearchResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.SearchAgency conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.SearchAgency resultXml:" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.SearchAgency conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.SearchAgency conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.SearchAgency");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bSearchResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }
              
            }
            return bSearchResult;

        }

        #endregion


        #region Change Point Limit
        //Author:Sabhareesan O.K
        //Purpose: Purpose: Change from web serices to WCF


        /// <summary>
        /// Function for update the CSD Points for selected one
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="sessionUserID"></param>
        /// <param name="objectID"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool UpdatePointsLimit(string objectXml, string sessionUserID, out long objectID, out string resultXml)
        {
            objectID = 0;
            resultXml = string.Empty;
            bool bResult_UPL = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdatePointsLimit conditionXml:" + objectXml + "SessionId:" + sessionUserID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdatePointsLimit resultXml:" + resultXml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionUserID, "Get");


                RoleObject = new Role();

                RoleObject.UpdatePointsLimit(objectXml, Convert.ToInt16(currentSession.UserId), out objectID, out resultXml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult_UPL = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdatePointsLimit conditionXml:" + objectXml + "SessionId:" + sessionUserID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdatePointsLimit resultXml:" + resultXml);



            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdatePointsLimit SessionId:" + sessionUserID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdatePointsLimit SessionId:" + sessionUserID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdatePointsLimit");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bResult_UPL = false;
            }
            finally
            {

                if (RoleObject != null)
                {
                    RoleObject = null;
                }
                if (t != null)
                    t = null;

            }
            return bResult_UPL;
        }



        /// <summary>
        /// To search the Roles
        /// If the number of records in the resultset is greater than 
        /// the maximum row count then the method returns empty resultset
        /// </summary>
        /// <param name="conditionXml">Search criteria as xml formatted string</param>/// 
        /// <param name="maxRowCount">Maximum row count for the resultset</param>/// 
        /// <returns>No of records in the resultset</param>/// 
        /// <returns>Role records in xml format</returns>        
        public bool SearchCSDPointsLimit(string conditionXml, int maxRowCount, out int rowCount, string culture, out string xmlDoc, out string resultXml)
        {
            xmlDoc = string.Empty;
            resultXml = string.Empty;
            rowCount = 0;
            bool bSearchCSDFlag = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit resultXml:" + resultXml);

                RoleObject = new Role();

                xmlDoc = RoleObject.Search(conditionXml, maxRowCount, out rowCount, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmlDoc != null && xmlDoc != "</NewDataSet>")
                {
                    bSearchCSDFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit resultXml:" + resultXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.SearchCSDPointsLimit");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bSearchCSDFlag = false;

            }
            finally
            {
                if (RoleObject != null)
                {
                    RoleObject = null;
                }

            }

            return bSearchCSDFlag;

        }

        #endregion

        #region Collection Period

        #region Maintain Collection Period

        //Updated by :Sabhareesan O.K 
        //Purpose: Maintain Collection Period 

        /// <summary>
        /// To get the Collection Periods
        /// </summary>
        /// <param name="partnerID">unique identifier of the Partner table</param>/// 
        /// <returns>Partner record in xml format</returns>
        public bool ViewCollectionPeriod(string conditionXml, int maxRowCount, out int rowCount, string culture, out string xmlDoc, out string resultXml)
        {
            bool bViewCollectFlag = false;
            //xmlDoc = null;
            xmlDoc = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewCollectionPeriod conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewCollectionPeriod resultXml:" + resultXml);

                offobj = new Offer();
                xmlDoc = offobj.ViewCollectionPeriod(conditionXml, maxRowCount, out rowCount, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmlDoc != "" && xmlDoc != "</NewDataSet>")
                {
                    bViewCollectFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewCollectionPeriod conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewCollectionPeriod resultXml:" + resultXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewCollectionPeriod conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewCollectionPeriod conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = ex.InnerException.ToString();
                bViewCollectFlag = false;
            }

            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }
            }


            return bViewCollectFlag;

        }
        /// <summary>
        /// Add a new Offer
        /// </summary>
        /// <param name="objectXml">Offer details</param>/// 
        /// <returns>Success True/False</returns>
        /// <returns out param>OfferID of the new Offer</returns>
        public bool AddCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml)
        {
            bool bAddCollectFlag = false;
            resultXml = string.Empty;
            objectId = 0;


            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.AddCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.AddCollectionPeriod resultXml:" + resultXml);


                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, userID, "Get");


                offobj = new Offer();
                //calling the add function from offer class and return the boolean variable
                offobj.Add(objectXml, Convert.ToInt32(currentSession.UserId), objectId, resultXml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bAddCollectFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.AddCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.AddCollectionPeriod resultXml:" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.AddCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.AddCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.AddCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bAddCollectFlag = false;
            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }
                if (t != null)
                    t = null;

            }

            return bAddCollectFlag;

        }
        /// <summary>
        /// Update Offer details
        /// </summary>
        /// <returns>Success True/False</returns>
        /// <returns outparam>OfferID of the updated Offer</returns>
        public bool UpdateCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml)
        {
            bool bUpdateCollectFlag = false;
            resultXml = string.Empty;
            objectId = 0;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod resultXml:" + resultXml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, userID, "Get");

                offobj = new Offer();
                //calling the add function from offer class and return the boolean variable
                offobj.Update(objectXml, Convert.ToInt32(currentSession.UserId), objectId, resultXml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bUpdateCollectFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod resultXml:" + resultXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bUpdateCollectFlag = false;


            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }


                if (t != null)
                    t = null;

            }

            return bUpdateCollectFlag;

        }
        /// <summary>
        /// Update Offer details
        /// </summary>
        /// <returns>Success True/False</returns>
        /// <returns outparam>OfferID of the updated Offer</returns>
        public bool DeleteCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml)
        {
            bool bDeleteCollectFlag = false;
            resultXml = string.Empty;
            objectId = 0;


            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod resultXml:" + resultXml);


                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, userID, "Get");


                offobj = new Offer();
                //calling the add function from offer class and return the boolean variable
                offobj.Delete(objectXml, Convert.ToInt32(currentSession.UserId), objectId, resultXml);


                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bDeleteCollectFlag = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod resultXml:" + resultXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.DeleteCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);


                resultXml = ex.InnerException.ToString();
                bDeleteCollectFlag = false;


            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }
                if (t != null)
                    t = null;


            }

            return bDeleteCollectFlag;

        }

        #endregion

        #region Reward Mailing OverView

        /// <summary>
        /// view the reward mailing details
        /// </summary>
        /// <param name="offerID"></param>
        /// <param name="culture"></param>
        /// <param name="sessionID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool ViewRewardMailingDetails(long offerID, string culture, string sessionID, out string xmldoc, out string resultxml)
        {
            xmldoc = string.Empty;
            resultxml = string.Empty;
            bool bViewRewardFlag = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails SessionID:" + sessionID + "OfferID:" + offerID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails resultXml:" + resultxml);

                offobj = new Offer();
                xmldoc = offobj.ViewRewardMailingDetails(offerID, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmldoc != null && xmldoc != "</NewDataSet>")
                {
                    bViewRewardFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails SessionID:" + sessionID + "OfferID:" + offerID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails resultXml:" + resultxml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails SessionID:" + sessionID + "OfferID:" + offerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails SessionID:" + sessionID + "OfferID:" + offerID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewRewardMailingDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bViewRewardFlag = false;
            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }
            }
            return bViewRewardFlag;

        }

        /// <summary>
        /// update Reward Mailing Details
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="userID"></param>
        /// <param name="objectId"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool UpdateRewardMailingDetails(string objectXml, string userID, out long objectId, out string resultXml)
        {
            bool bUpdateRewardmailFlag = false;
            objectId = 0;
            resultXml = string.Empty;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails resultXml:" + resultXml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, userID, "Get");


                offobj = new Offer();
                offobj.UpdateRewardMailingDetails(objectXml, Convert.ToInt32(currentSession.UserId), out objectId, out resultXml);


                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bUpdateRewardmailFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails resultXml:" + resultXml);


            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateRewardMailingDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bUpdateRewardmailFlag = false;
            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }

                if (t != null)
                    t = null;

            }
            return bUpdateRewardmailFlag;
        }

        #endregion

        #region Setup Coupon Types

        /// <summary>
        /// View the coupon setup types
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <param name="sessionId"></param>
        /// <param name="xmldoc"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool ViewCouponSetupTypes(string conditionXml, int maxRowCount, out int rowCount, string culture, string sessionId, out string xmldoc, out string resultxml)
        {

            xmldoc = string.Empty;
            resultxml = string.Empty;
            bool bViewCouponSetupFlag = false;
            rowCount = 0;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes SessionID:" + sessionId + "Condition XML:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes resultXml:" + resultxml);

                couponobj = new Coupons();
                xmldoc = couponobj.View(conditionXml, maxRowCount, out rowCount, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmldoc != null && xmldoc != "</NewDataSet>")
                {
                    bViewCouponSetupFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes SessionId:" + sessionId + "Condition XML:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes resultXml:" + resultxml);


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes sessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes sessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewCouponSetupTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bViewCouponSetupFlag = false;
            }
            finally
            {
                if (couponobj != null)
                {
                    couponobj = null;
                }

            }

            return bViewCouponSetupFlag;
        }

        /// <summary>
        /// Delete the coupon setup types
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="sessionId"></param>
        /// <param name="sessionCrmid"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool DeleteCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml)
        {
            sessionCrmid = 0;
            resultxml = string.Empty;
            bool bDelCouponSetupFlag = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes SessionId:" + sessionId + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes resultXml:" + resultxml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId, "Get");


                couponobj = new Coupons();
                bDelCouponSetupFlag = couponobj.Delete(objectXml, Convert.ToInt32(currentSession.UserId), out sessionCrmid, out resultxml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultxml != null && resultxml != "</NewDataSet>")
                {
                    bDelCouponSetupFlag = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes SessionId:" + sessionId + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes resultXml:" + resultxml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.DeleteCouponSetupTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bDelCouponSetupFlag = false;
            }
            finally
            {
                if (couponobj != null)
                {
                    couponobj = null;
                }
                if (t != null)
                    t = null;


            }

            return bDelCouponSetupFlag;

        }


        /// <summary>
        /// Insert the coupon setup types
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="sessionId"></param>
        /// <param name="sessionCrmid"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool InsertCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml)
        {
            sessionCrmid = 0;
            resultxml = string.Empty;
            bool bInsCouponSetupFlag = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes conditionXml:" + objectXml + "SessionID:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes resultXml:" + resultxml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId, "Get");


                couponobj = new Coupons();
                couponobj.Insert(objectXml, Convert.ToInt32(currentSession.UserId), out sessionCrmid, out resultxml);


                //Assign TRUE if the resultXML is not empty string 
                if (resultxml != null && resultxml != "</NewDataSet>")
                {
                    bInsCouponSetupFlag = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes conditionXml:" + objectXml + "SessionID:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes resultXml:" + resultxml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes SessionID:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes SessionID:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.InsertCouponSetupTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bInsCouponSetupFlag = false;
            }
            finally
            {
                if (couponobj != null)
                {
                    couponobj = null;
                }

                if (t != null)
                    t = null;

            }

            return bInsCouponSetupFlag;

        }


        /// <summary>
        /// Update the coupon setup types
        /// </summary>
        /// <param name="objectXml"></param>
        /// <param name="sessionId"></param>
        /// <param name="sessionCrmid"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool UpdateCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml)
        {
            sessionCrmid = 0;
            resultxml = string.Empty;
            bool bUpdCouponSetupFlag = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes conditionXml:" + objectXml + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes resultXml:" + resultxml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId, "Get");

                couponobj = new Coupons();
                couponobj.Update(objectXml, Convert.ToInt32(currentSession.UserId), out sessionCrmid, out resultxml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultxml != null && resultxml != "</NewDataSet>")
                {
                    bUpdCouponSetupFlag = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes conditionXml:" + objectXml + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes resultXml:" + resultxml);


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateCouponSetupTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bUpdCouponSetupFlag = false;

            }
            finally
            {
                if (couponobj != null)
                {
                    couponobj = null;
                }

                if (t != null)
                    t = null;

            }

            return bUpdCouponSetupFlag;

        }



        #endregion

        #region Setup Voucher Types

        /// <summary>
        /// To get the Voucher Details
        /// </summary>
        /// <param name="offerID"></param>
        /// <param name="culture"></param>
        /// <param name="sessionId"></param>
        /// <param name="xmldoc"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool ViewVoucherDetails(long offerID, string culture, string sessionId, out string xmldoc, out string resultxml)
        {
            xmldoc = string.Empty;
            resultxml = string.Empty;
            bool bViewVochDetails = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewVoucherDetails SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewVoucherDetails resultXml:" + resultxml);


                vocherobj = new Vouchers();
                xmldoc = vocherobj.View(offerID, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmldoc != null && xmldoc != "</NewDataSet>")
                {
                    bViewVochDetails = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewVoucherDetails SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewVoucherDetails resultXml:" + resultxml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewVoucherDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bViewVochDetails = false;
            }
            finally
            {
                if (vocherobj != null)
                {
                    vocherobj = null;
                }


            }

            return bViewVochDetails;

        }

        /// <summary>
        /// To get the Vocuher Barcode Details
        /// </summary>
        /// <param name="offerID"></param>
        /// <param name="culture"></param>
        /// <param name="sessionId"></param>
        /// <param name="xmldoc"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool ViewVoucherBarCodeDetails(long offerID, string culture, string sessionId, out string xmldoc, out string resultxml)
        {
            xmldoc = string.Empty;
            resultxml = string.Empty;
            bool bViewVochBarDetails = false;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails resultXml:" + resultxml);


                vocherobj = new Vouchers();
                xmldoc = vocherobj.ViewVocuhers(offerID, culture);

                //Assign TRUE if the resultXML is not empty string 
                if (xmldoc != null && xmldoc != "</NewDataSet>")
                {
                    bViewVochBarDetails = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails resultXml:" + resultxml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewVoucherBarCodeDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultxml = ex.InnerException.ToString();
                bViewVochBarDetails = false;
            }
            finally
            {
                if (vocherobj != null)
                {
                    vocherobj = null;
                }


            }

            return bViewVochBarDetails;

        }
        /// <summary>
        /// For update the voucher details
        /// </summary>
        /// <param name="conditionXML"></param>
        /// <param name="sessionId"></param>
        /// <param name="SessionCrmid"></param>
        /// <param name="resultxml"></param>
        /// <returns></returns>
        public bool UpdateVoucherDetails(string conditionXML, string sessionId, out long SessionCrmid, out string resultxml)
        {
            SessionCrmid = 0;
            resultxml = string.Empty;
            bool bUpdateVoucherDetails = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdateVoucherDetails conditionXml:" + conditionXML + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdateVoucherDetails resultXml:" + resultxml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId, "Get");

                vocherobj = new Vouchers();
                vocherobj.Update(conditionXML, Convert.ToInt32(currentSession.UserId), out SessionCrmid, out resultxml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultxml != null && resultxml != "</NewDataSet>")
                {
                    bUpdateVoucherDetails = true;
                }


                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdateVoucherDetails conditionXml:" + conditionXML + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdateVoucherDetails resultXml:" + resultxml);


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateVoucherDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);


                resultxml = ex.InnerException.ToString();
                bUpdateVoucherDetails = false;
            }
            finally
            {
                if (vocherobj != null)
                {
                    vocherobj = null;
                }
                if (t != null)
                    t = null;



            }

            return bUpdateVoucherDetails;
        }

        public bool AddVoucherDetails(string conditionXML, string sessionId, out long SessionCrmid, out string resultxml)
        {
            SessionCrmid = 0;
            resultxml = string.Empty;
            bool bAddVoucherDetails = false;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.AddVoucherDetails conditionXml:" + conditionXML + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.AddVoucherDetails resultXml:" + resultxml);


                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId, "Get");

                vocherobj = new Vouchers();
                vocherobj.Add(conditionXML, Convert.ToInt32(currentSession.UserId), out SessionCrmid, out resultxml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultxml != null && resultxml != "</NewDataSet>")
                {
                    bAddVoucherDetails = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.AddVoucherDetails conditionXml:" + conditionXML + "SessionId:" + sessionId);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.AddVoucherDetails resultXml:" + resultxml);


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.AddVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.AddVoucherDetails SessionId:" + sessionId + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.AddVoucherDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);


                resultxml = ex.InnerException.ToString();
                bAddVoucherDetails = false;
            }
            finally
            {
                if (vocherobj != null)
                {
                    vocherobj = null;
                }
                if (t != null)
                    t = null;

            }

            return bAddVoucherDetails;
        }

        #endregion


        #endregion

        #region Static Function

        #region Connection
        /// <summary>
        /// Check if a capability exists in the supplied XML.
        /// </summary>
        /// <param name="capabilityXml">The capability XML.</param>
        /// <param name="capability">The capability to check for.</param>
        /// <returns>Returns true if capability found, otherwise false.</returns>
        public bool DoesCapabilityExist(string capabilityXml, string capability)
        {

            bool bDoesCapexist = false;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability);


                XmlDocument document = new XmlDocument();
                document.LoadXml(capabilityXml);
                XmlNode node = document.SelectSingleNode("capabilities/" + capability);
                if (node == null)
                    bDoesCapexist = false;
                else
                    bDoesCapexist = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.DoesCapabilityExist CapabilityXml:" + capabilityXml + "Capability:" + capability + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.DoesCapabilityExist");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bDoesCapexist = false;
            }

            return bDoesCapexist;
        }
        #endregion


        #endregion

        public bool ViewPartnerOutlets(long PartnerID, string sessionID, out string resultDoc, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            resultDoc = string.Empty;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append(" PartnerNumber: " + PartnerID);
            trace.WriteInfo(sb.ToString());
            #endregion
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                partnerOutletObject = new PartnerOutlet();
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                resultDoc = partnerOutletObject.View(PartnerID, currentSession.Culture);

                if (resultDoc != null && resultDoc != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "PartnerNumber:" + PartnerID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool ViewPartnerTransactions(int maxRowCount, string conditionXml, string sessionId, string culture, out int rowCount, out string resultsDoc, out string resultXml)
        {
            bool bSearchResult = false;
            resultXml = string.Empty;
            resultsDoc = string.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewPartnerTransactions conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewPartnerTransactions resultXml:" + resultXml);

                partnerObject = new Partner();
                resultsDoc = partnerObject.SearchTrans(conditionXml, maxRowCount, out rowCount, culture);

                if (resultsDoc != null && resultsDoc != "</NewDataSet>")
                {
                    bSearchResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewPartnerTransactions conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewPartnerTransactions resultXml:" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewPartnerTransactions conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewPartnerTransactions conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewPartnerTransactions");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bSearchResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

            }
            return bSearchResult;

        }

        public bool AddPartner(string updateXML, string sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.AddAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                partnerObject = new Partner();
                partnerObject.Add(updateXML, currentSession.UserId, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (partnerObject != null)
                {
                    partnerObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool AddPartnerOutlets(string updateXML, string sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.AddAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                partnerOutletObject = new PartnerOutlet();
                partnerOutletObject.Add(updateXML, currentSession.UserId, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (partnerObject != null)
                {
                    partnerObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool UpdatePartnerOutlets(string updateXML, string sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.UpdatePartnerOutlets");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                partnerOutletObject = new PartnerOutlet();
                partnerOutletObject.Update(updateXML, currentSession.UserId, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool GetOptionalCustomerStatus(long OfferID, string sessionId, out string resultsDoc, out string resultXml)
        {
            bool bSearchResult = false;
            resultXml = string.Empty;
            resultsDoc = string.Empty;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus");
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId.ToString(), "Get");
                offobj = new Offer();
                resultsDoc = offobj.GetOptinalCustomerStatus(OfferID, currentSession.Culture);
                if (resultsDoc != null && resultsDoc != "</NewDataSet>")
                {
                    bSearchResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus");

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.GetOptionalCustomerStatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bSearchResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

            }
            return bSearchResult;
        }

        public bool UpdateAgency(string updateXML, string sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.UpdateAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                agencyObject = new Agency();
                agencyObject.Update(updateXML, currentSession.UserId, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool DeletePartnerOutlets(string conditionXml, string sessionId, out long sessionCrmId, out string resultXml)
        {
            bool bSearchResult = false;
            resultXml = string.Empty;
            string resultsDoc = string.Empty;
            sessionCrmId = 0;
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.DeletePartnerOutlets conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.DeletePartnerOutlets resultXml:" + resultXml);
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionId.ToString(), "Get");
                partnerOutletObject = new PartnerOutlet();
                partnerOutletObject.Delete(conditionXml, currentSession.UserId, out sessionCrmId, out resultXml);
                bSearchResult = true;

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.DeletePartnerOutlets conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.DeletePartnerOutlets resultXml:" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.DeletePartnerOutlets conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.DeletePartnerOutlets conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.DeletePartnerOutlets");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bSearchResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

            }
            return bSearchResult;
        }

        public bool UpdatePartner(string updateXML, string sessionID, out long sessionCrmId, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            sessionCrmId = 0;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.UpdateAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append("SessionID: " + sessionID);
            trace.WriteInfo(sb.ToString());
            #endregion

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                partnerObject = new Partner();
                partnerObject.Update(updateXML, currentSession.UserId, out sessionCrmId, out resultXml);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "SessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool ViewPartnerType(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {
            resultXml = "";

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewPartnerType conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewPartnerType resultXml" + resultXml);
                errorXml = string.Empty;
                Partner partnerObj = new Partner();
                resultXml = partnerObj.ViewPartnerType(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewPartnerType conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewPartnerType resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewPartnerType conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewPartnerType conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewPartnerType");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        public bool ViewReports(string conditionXml, int maxRowCount, out int rowCount, string culture, string sessionId, out string resultXml)
        {


            bool bViewResult = false;

            resultXml = string.Empty;

            rowCount = 0;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewReports conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewReports resultXml:" + resultXml);

                offobj = new Offer();
                resultXml = offobj.ViewReports(conditionXml, maxRowCount, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bViewResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewReports conditionXml:" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewReports resultXml:" + resultXml);
            }

            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewReports conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewReports conditionXml:" + conditionXml + "- Error Message :" + ex.ToString());

                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewReports");

                NGCTrace.NGCTrace.ExeptionHandling(ex);



                resultXml = ex.InnerException.ToString();

                bViewResult = false;

            }

            finally
            {

                if (offobj != null)
                {

                    offobj = null;

                }



            }

            return bViewResult;



        }

        public bool ViewAssociatedPartners(string conditionXml, short sessionID, int retrieveNumber, out string resultsDoc, out string resultXml, int maxcount, out int rowCount, string culture)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            resultsDoc = string.Empty;
            bool bResult = false;
            rowCount = 0;
            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewAssociatedPartners");
            StringBuilder sb = new StringBuilder();
            sb.Append(" sessionID: " + sessionID);
            sb.Append(" retrieveNumber: " + retrieveNumber);
            sb.Append(" Culture: " + culture);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                partnerObject = new Partner();
                resultXml = partnerObject.ViewAssociatedPartners(conditionXml, maxcount, out rowCount, culture);

                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "sessionID:" + sessionID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }


        public bool ViewPartner(long PartnerID, string sessionID, out string resultsDoc, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            resultsDoc = string.Empty;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewAgency");
            StringBuilder sb = new StringBuilder();
            sb.Append(" PartnerNumber: " + PartnerID);
            trace.WriteInfo(sb.ToString());
            #endregion
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                partnerObject = new Partner();
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                resultsDoc = partnerObject.View(PartnerID, currentSession.Culture);

                if (resultsDoc != null && resultsDoc != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "PartnerNumber:" + PartnerID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool ViewPartners(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.ViewPartners conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.ViewPartners resultXml" + resultXml);
                errorXml = string.Empty;
                Partner partnerObj = new Partner();
                resultXml = partnerObj.ViewPartners(conditionXml, maxRowCount, out rowCount, culture);
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.ViewPartners conditionXml" + conditionXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.ViewPartners resultXml" + resultXml);
                return true;
            }
            catch (Exception ex)
            {
                //set out parameters
                errorXml = ex.InnerException.ToString();
                resultXml = string.Empty;
                rowCount = 0;
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.ViewPartners conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.ViewPartners conditionXml" + conditionXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.ViewPartners");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                return false;
            }
            finally
            {

            }
        }

        //Updated by :Sabhareesan O.K 
        //Purpose: Change from web serices to WCF
        /// <summary>
        /// Function is to Update mailing Details
        /// </summary>
        /// <param name="conditionXml"></param>
        /// <param name="maxRowCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool UpdateMailingDetails(string objectXml, string userID, out long objectId, out string resultXml)
        {

            bool bUpdateMailFlag = false;
            resultXml = string.Empty;
            objectId = 0;

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod resultXml:" + resultXml);

                //convert the session id string into short 
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, userID, "Get");

                offobj = new Offer();
                //calling the add function from offer class and return the boolean variable
                offobj.UpdateMailingDetails(objectXml, currentSession.UserId, out objectId, out resultXml);

                //Assign TRUE if the resultXML is not empty string 
                if (resultXml != null && resultXml != "</NewDataSet>")
                {
                    bUpdateMailFlag = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "Condition XML:" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod resultXml:" + resultXml);

            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                resultXml = ex.InnerException.ToString();
                bUpdateMailFlag = false;


            }
            finally
            {
                if (offobj != null)
                {
                    offobj = null;
                }


                if (t != null)
                    t = null;

            }

            return bUpdateMailFlag;


        }


        public bool ViewMailingDetails(long OfferID, string sessionID, out string resultDoc, out string resultXml)
        {
            //XmlDocument agencyInfo = null;
            resultXml = string.Empty;
            resultDoc = string.Empty;
            bool bResult = false;

            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewMailingDetails");
            StringBuilder sb = new StringBuilder();
            sb.Append(" OfferID: " + OfferID);
            trace.WriteInfo(sb.ToString());
            #endregion
            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                offobj = new Offer();
                SessionTable st = new SessionTable(t);
                Tesco.NGC.SecurityLayer.Session currentSession = st.Validate(t, sessionID.ToString(), "Get");
                resultDoc = offobj.ViewMailingDetails(OfferID, currentSession.Culture);

                if (resultDoc != null && resultDoc != "</NewDataSet>")
                {
                    bResult = true;
                }

                return bResult;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "PartnerNumber:" + OfferID.ToString());
                resultXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                if (agencyObject != null)
                {
                    agencyObject = null;
                }

                trState.EndProc();
            }

            return bResult;
        }

        public bool Connect(string userName, string password, string culture, string AppName, out string sessionId, out string capabilityXml, out string resultXml)
        {


            sessionId = String.Empty;
            capabilityXml = String.Empty;
            resultXml = String.Empty;
            bool bResult = false;

            //Result result = new Result();
            #region Trace
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("MarketingService.ViewAssociatedPartners");
            StringBuilder sb = new StringBuilder();
            sb.Append(" sessionID: " + sessionId);
            sb.Append(" Culture: " + culture);
            trace.WriteInfo(sb.ToString());
            #endregion

            Tesco.NGC.Utils.Trace t = new Tesco.NGC.Utils.Trace();
            try
            {
                //Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                SessionTable st = new SessionTable(t);
                // rwLock.AcquireReaderLock(WaitInterval);

                IsInitialised(String.Empty, userName, password, culture);
                //sessionId = Global.sessionTable.Add(trace, userName, password, culture, AppName, out capabilityXml);
                //Global.sessionTable.Validate(trace, sessionId, "Connect");
                sessionId = st.Add(t, userName, password, culture, AppName, out capabilityXml);
                st.Validate(t, sessionId.ToString(), "connect");
                bResult = true;

            }
            catch (Exception e)
            {
                //    ExceptionManager.Publish(e);
                //    result.Add(e);
                //    resultXml = result.OuterXml;
                //    sessionId = Guid.NewGuid().ToString();
                //    capabilityXml = String.Empty;

                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userName + "- Error Message :" + e.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod UserID:" + userName + "- Error Message :" + e.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardOnlineService.MarketingService.UpdateCollectionPeriod");
                NGCTrace.NGCTrace.ExeptionHandling(e);

                resultXml = e.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                // rwLock.ReleaseReaderLock();
            }
            trState.EndProc();
            return bResult;
        }

        /// <summary>
        /// Check All incoming parameters of public services aren't null
        /// </summary>
        /// <param name="paramValueList">The list of parameters to check</param>
        protected static void IsInitialised(params object[] paramValueList)
        {
            for (int i = paramValueList.GetLowerBound(0); i <= paramValueList.GetUpperBound(0); i++)
            {
                if (paramValueList[i] == null)
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                    MethodBase method = stackTrace.GetFrame(1).GetMethod();
                    string methodName = method.Name;
                    string parameterName = method.GetParameters()[i].Name;
                }
            }
        }


       

     
    }
}