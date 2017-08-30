#region using

using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;

#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class ConfigManagement
    {
        #region Fields

        /// <summary>
        /// CustomerID
        /// </summary>
        private string sessionID;

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int16 userID;

        /// <summary>
        /// Customer Title
        /// </summary>
        private string titleEnglish;

        /// <summary>
        /// Customer Name1
        /// </summary>
        private string name1;
        /// <summary>
        /// Customer Name2
        /// </summary>
        private string name2;
        /// <summary>
        /// Customer Name3
        /// </summary>
        private string name3;
        /// <summary>
        /// Customer Name4
        /// </summary>
        private string name4;
        /// <summary>
        /// Description 0
        /// </summary>
        private string desc0 = null;
        /// <summary>
        /// Description 1
        /// </summary>
        private string desc1 = null;
        /// <summary>
        /// Description 2b
        /// </summary>
        private string desc2 = null;
        /// <summary>
        /// Description 3
        /// </summary>
        private string desc3 = null;
        /// <summary>
        /// Description 4
        /// </summary>
        private string desc4 = null;
        /// <summary>
        /// Mailable 0
        /// </summary>
        private short mailable0;
        /// <summary>
        /// Mailable 1
        /// </summary>
        private short mailable1;
        /// <summary>
        /// Mailable 2
        /// </summary>
        private short mailable2;
        /// <summary>
        /// Mailable 3
        /// </summary>
        private short mailable3;
        /// <summary>
        /// Mailable 4
        /// </summary>
        private short mailable4;
        /// <summary>
        /// Mailable 0
        /// </summary>
        private short notmailable0;
        /// <summary>
        /// Mailable 1
        /// </summary>
        private short notmailable1;
        /// <summary>
        /// Mailable 2
        /// </summary>
        private short notmailable2;
        /// <summary>
        /// Mailable 3
        /// </summary>
        private short notmailable3;
        /// <summary>
        /// Mailable 4
        /// </summary>
        private short notmailable4;

        private string culture="";

        private string reasonCode;

        private string optionalreasonCode0;  
        private string optionalreasonCode1;
        private string optionalreasonCode2;
        private string optionalreasonCode3;
        private string optionalreasonCode4;


        
        #endregion

        #region Properties

        /// <summary>
        ///  SessionID
        /// </summary>
        public string SessionID 
        { 
            get { return this.sessionID; } 
            set { this.sessionID = value; } 
        }

        /// <summary>
        ///  User Identification
        /// </summary>
        public Int16 UserID
        {
            get { return this.userID; }
            set { this.userID = value; }
        }
        /// <summary>
        ///  Name1
        /// </summary>
        public string Name1
        {
            get { return this.name1; }
            set { this.name1 = value; }
        }
        /// <summary>
        ///  Name2
        /// </summary>
        public string Name2
        {
            get { return this.name2; }
            set { this.name2 = value; }
        }
        /// <summary>
        ///  Name1
        /// </summary>
        public string Name3
        {
            get { return this.name3; }
            set { this.name3 = value; }
        }
        /// <summary>
        ///  Name1
        /// </summary>
        public string Name4
        {
            get { return this.name4; }
            set { this.name4 = value; }
        }
        /// <summary>
        ///  Description0
        /// </summary>
        public string Desc0
        {
            get { return this.desc0; }
            set { this.desc0 = value; }
        }
        /// <summary>
        ///  Description1
        /// </summary>
        public string Desc1
        {
            get { return this.desc1; }
            set { this.desc1 = value; }
        }
        /// <summary>
        ///  Description2
        /// </summary>
        public string Desc2
        {
            get { return this.desc2; }
            set { this.desc2 = value; }
        }
        /// <summary>
        ///  Description3
        /// </summary>
        public string Desc3
        {
            get { return this.desc3; }
            set { this.desc3 = value; }
        }
        /// <summary>
        ///  Description4
        /// </summary>
        public string Desc4
        {
            get { return this.desc4; }
            set { this.desc4 = value; }
        }

        /// <summary>
        ///  Mailable0
        /// </summary>
        public short Mailable0
        {
            get { return this.mailable0; }
            set { this.mailable0 = value; }
        }
        /// <summary>
        ///  Mailable1
        /// </summary>
        public short Mailable1
        {
            get { return this.mailable1; }
            set { this.mailable1 = value; }
        }
        /// <summary>
        ///  Mailable2
        /// </summary>
        public short Mailable2
        {
            get { return this.mailable2; }
            set { this.mailable2 = value; }
        }
        /// <summary>
        ///  Mailable3
        /// </summary>
        public short Mailable3
        {
            get { return this.mailable3; }
            set { this.mailable3 = value; }
        }
        /// <summary>
        ///  Mailable4
        /// </summary>
        public short Mailable4
        {
            get { return this.mailable4; }
            set { this.mailable4 = value; }
        }

        /// <summary>
        ///  NotMailable0
        /// </summary>
        public short NotMailable0
        {
            get { return this.notmailable0; }
            set { this.notmailable0 = value; }
        }
        /// <summary>
        ///  Mailable1
        /// </summary>
        public short NotMailable1
        {
            get { return this.notmailable1; }
            set { this.notmailable1 = value; }
        }
        /// <summary>
        ///  Mailable2
        /// </summary>
        public short NotMailable2
        {
            get { return this.notmailable2; }
            set { this.notmailable2 = value; }
        }
        /// <summary>
        ///  Mailable3
        /// </summary>
        public short NotMailable3
        {
            get { return this.notmailable3; }
            set { this.notmailable3 = value; }
        }
        /// <summary>
        ///  Mailable4
        /// </summary>
        public short NotMailable4
        {
            get { return this.notmailable4; }
            set { this.notmailable4 = value; }
        }
        public string Culture
        {
            get { return this.culture; }
            set { this.culture = value; }
        }

        public string Reasoncode
        {
            get { return this.reasonCode; }
            set { this.reasonCode = value; }
        }

        public string OptionalReasoncode0
        {
            get { return this.optionalreasonCode0; }
            set { this.optionalreasonCode0 = value; }
        }


        public string OptionalReasoncode1
        {
            get { return this.optionalreasonCode1; }
            set { this.optionalreasonCode1 = value; }
        }

        public string OptionalReasoncode2
        {
            get { return this.optionalreasonCode2; }
            set { this.optionalreasonCode2 = value; }
        }

        public string OptionalReasoncode3
        {
            get { return this.optionalreasonCode3; }
            set { this.optionalreasonCode3 = value; }
        }

        public string OptionalReasoncode4
        {
            get { return this.optionalreasonCode4; }
            set { this.optionalreasonCode4 = value; }
        }

        #endregion

        //Added as part of ROI conncetion string management
        //begin
         private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ConfigManagement()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
            }
        }
        //end

        #region Methods

        #region View
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public String View(Int64 userId, string culture)
        {
         
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ConfigManagement.View");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ConfigManagement.View - userId :"+userId.ToString());
                
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.VIEW_DEITARYPREFERENCES, userId, culture);
                ds.Tables[0].TableName = "Configuration";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ConfigManagement.View");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ConfigManagement.View - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ConfigManagement.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ConfigManagement.View - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ConfigManagement.View");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }


        #endregion

        #region ViewCustomerStatus
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public String ViewCustomerstatus(Int64 userId, string culture)
        {
            
            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus - userId :" + userId.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.VIEW_CUSTOMERPREFERENCES, userId, culture);
                ds.Tables[0].TableName = "Configuration";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ConfigManagement.ViewCustomerstatus");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }


        #endregion

        #region UPDATE
        /// <summary>
        /// To update the Role details 
        /// </summary>
        public bool Update(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {
            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ConfigManagement.Update");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ConfigManagement.Update - objectXml :" + objectXml.ToString());
                Hashtable htblRole = ConvertXmlHash.XMLToHashTable(objectXml, "Configuration");
                this.Name1 = Convert.ToString(htblRole[Constants.NAME_1]);
                this.Name2 = Convert.ToString(htblRole[Constants.NAME_2]);
                this.Name3 = Convert.ToString(htblRole[Constants.NAME_3]);

                this.Desc0 = Convert.ToString(htblRole[Constants.DESC_0]);
                this.Desc1 = Convert.ToString(htblRole[Constants.DESC_1]);
                this.Desc2 = Convert.ToString(htblRole[Constants.DESC_2]);
                this.Desc3 = Convert.ToString(htblRole[Constants.DESC_3]);
                this.Desc4 = Convert.ToString(htblRole[Constants.DESC_4]);
                this.Culture = Convert.ToString(htblRole[Constants.CULTURECONFIG]);
                this.UserID = Convert.ToInt16(htblRole[Constants.USERID]);
                string mail0 = Convert.ToString(htblRole[Constants.MAILABLE_0]);
                if (mail0 == "Yes")
                    this.Mailable0 = 0;
                else
                    this.Mailable0 = 1;
                string mail1 = Convert.ToString(htblRole[Constants.MAILABLE_1]);
                if (mail1 == "Yes")
                    this.Mailable1 = 0;
                else
                    this.Mailable1 = 1;
                string mail2 = Convert.ToString(htblRole[Constants.MAILABLE_2]);
                if (mail2 == "Yes")
                    this.Mailable2 = 0;
                else
                    this.Mailable2 = 1;
                string mail3 = Convert.ToString(htblRole[Constants.MAILABLE_3]);
                if (mail3 == "Yes")
                    this.Mailable3 = 0;
                else
                    this.Mailable3 = 1;
                string mail4 = Convert.ToString(htblRole[Constants.MAILABLE_4]);
                if (mail4 == "Yes")
                    this.Mailable4 = 0;
                else
                    this.Mailable4 = 1;

                string notmail0 = Convert.ToString(htblRole[Constants.NotMAILABLE_0]);
                if (notmail0 == "Yes")
                    this.NotMailable0 = 0;
                else
                    this.NotMailable0 = 1;
                string notmail1 = Convert.ToString(htblRole[Constants.NotMAILABLE_1]);
                if (notmail1 == "Yes")
                    this.NotMailable1 = 0;
                else
                    this.NotMailable1 = 1;
                string notmail2 = Convert.ToString(htblRole[Constants.NotMAILABLE_2]);
                if (notmail2 == "Yes")
                    this.NotMailable2 = 0;
                else
                    this.NotMailable2 = 1;
                string notmail3 = Convert.ToString(htblRole[Constants.NotMAILABLE_3]);
                if (notmail3 == "Yes")
                    this.NotMailable3 = 0;
                else
                    this.NotMailable3 = 1;
                string notmail4 = Convert.ToString(htblRole[Constants.NotMAILABLE_4]);
                if (notmail4 == "Yes")
                    this.NotMailable4 = 0;
                else
                    this.NotMailable4 = 1; 
/*
                this.NotMailable0 = Convert.ToInt16(htblRole[Constants.NotMAILABLE_0]);
                this.NotMailable1 = Convert.ToInt16(htblRole[Constants.NotMAILABLE_1]);
                this.NotMailable2 = Convert.ToInt16(htblRole[Constants.NotMAILABLE_2]);
                this.NotMailable3 = Convert.ToInt16(htblRole[Constants.NotMAILABLE_3]);
                this.NotMailable4 = Convert.ToInt16(htblRole[Constants.NotMAILABLE_4]);*/

                //this.RoleDesc = Convert.ToString(htblRole[Constants.ROLE_DESC]);
                //this.AmendBy = sessionUserID;

                object[] objConfig = { sessionUserID, Name1, Name2, Name3, Desc0, Desc1, Desc2, Desc3, Desc4, Mailable0, Mailable1, Mailable2, Mailable3, Mailable4, NotMailable0, NotMailable1, NotMailable2, NotMailable3, NotMailable4, Culture };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.UPDATE_CONFIGURATION, objConfig);
                //objectID = this.RoleID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ConfigManagement.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ConfigManagement.Update");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ConfigManagement.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ConfigManagement.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ConfigManagement.Update");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }
        #endregion            

        #region Reasoncode Validation
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public bool ValidateReasoncode(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ConfigManagement.ValidateReasoncode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ConfigManagement.ValidateReasoncode - objectXml :" + objectXml);
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "CheckReasoncode");
                this.Reasoncode = (string)htblAppUser[Constants.ValidateReasoncode];
                this.culture = (string)htblAppUser[Constants.CULTURECONFIG];

                object[] objChkReason = { Reasoncode, culture};

                
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.CHECK_REASONCODE, objChkReason);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ConfigManagement.ValidateReasoncode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ConfigManagement.ValidateReasoncode");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ConfigManagement.ValidateReasoncode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ConfigManagement.ValidateReasoncode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ConfigManagement.ValidateReasoncode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                
            }
            return SqlHelper.result.Flag;
        }


        #endregion

        #region Upate Optional Reasoncode 
        /// <summary>
        /// To get (view) the customer details based on the given CustomerID
        /// </summary>
        public bool UpdateOptionalReasoncode(string objectXml, int sessionUserID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode - objectXml :" + objectXml);
                Hashtable htblAppUser = ConvertXmlHash.XMLToHashTable(objectXml, "Reasoncode");
                this.OptionalReasoncode0 = (string)htblAppUser[Constants.OptionalReasoncode0];
                this.OptionalReasoncode1 = (string)htblAppUser[Constants.OptionalReasoncode1];
                this.OptionalReasoncode2 = (string)htblAppUser[Constants.OptionalReasoncode2];
                this.OptionalReasoncode3 = (string)htblAppUser[Constants.OptionalReasoncode3];
                this.OptionalReasoncode4 = (string)htblAppUser[Constants.OptionalReasoncode4];                
                object[] objChkReason = { OptionalReasoncode0, OptionalReasoncode1, OptionalReasoncode2, OptionalReasoncode3, OptionalReasoncode4, sessionUserID};
                
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.UPDATEOPTIONAL_REASONCODE, objChkReason);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ConfigManagement.UpdateOptionalReasoncode");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return SqlHelper.result.Flag;
        }


        #endregion
        
        #endregion

       

    }
}
