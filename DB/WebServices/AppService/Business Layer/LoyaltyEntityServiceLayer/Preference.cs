/*
 * File   : Preference.cs
 * Author : Sabhareesan O.K
 * email  :
 * File   : This file contains methods/properties related to Preference
 * Date   : 04/Feb/2012
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using System.Configuration;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using System.Collections;
using Tesco.NGC.Utils;

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class Preference
    {

        #region Fields

        /// <summary>
        ///Moved Fields from from Customer Class to Preference Class
        /// </summary>
        private Int64 customerID;
        private Int16 tescoGroupMailFlag;
        private Int16 partnerMailFlag;
        private Int16 researchPhoneFlag;
        private Int16 Ecoupon;
        private Int16 BAMilesStd;
        private Int16 BAMilesPremium;
        private Int16 AirMilesStd;
        private Int16 AirMilesPremium;
        private Int16 ViginAtlantic;
        private Int16 XmasSaver;
        private Int16 SaveTree;

        /* For NGC 3.6*/
        // Author: Sabhareesan O.K
        //Updated Date : 04-02-2012

        private Int16 _ViaMail;
        private Int16 _ViaSMS;
        private Int16 _ViaPost;
        private Int16 _ClubcardEMails;
        private Int16 _BabyToddler;

        private int _PreferenceMembershipID;
        private string _PreferenceEmailID;
        private string _PreferenceMobile;

        /* For NGC 3.6*/


        #endregion

        #region Property

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int64 CustomerID { get { return this.customerID; } set { this.customerID = value; } }

        /// <summary>
        ///  TescoMail
        /// </summary>
        public Int16 TescoGroupMailFlag { get { return this.tescoGroupMailFlag; } set { this.tescoGroupMailFlag = value; } }

        /// <summary>
        ///  PartnerMail
        /// </summary>
        public Int16 PartnerMailFlag { get { return this.partnerMailFlag; } set { this.partnerMailFlag = value; } }

         /// <summary>
        ///  ResearchPhone
        /// </summary>
        public Int16 ResearchPhoneFlag { get { return this.researchPhoneFlag; } set { this.researchPhoneFlag = value; } }

        //Added by Sadanand on 26-MAR-2010
        /// <summary>
        ///  Ecoupon
        /// </summary>
        public Int16 ECOUPON { get { return this.Ecoupon; } set { this.Ecoupon = value; } }

        /// <summary>
        ///  BAMilesStd
        /// </summary>
        public Int16 BAMILESSTD { get { return this.BAMilesStd; } set { this.BAMilesStd = value; } }

        /// <summary>
        ///  BAMilesPremium
        /// </summary>
        public Int16 BAMILESPREMIUM { get { return this.BAMilesPremium; } set { this.BAMilesPremium = value; } }

        /// <summary>
        ///  AirMilesStd
        /// </summary>
        public Int16 AIRMILESSTD { get { return this.AirMilesStd; } set { this.AirMilesStd = value; } }

        /// <summary>
        ///  AirMilesPremium
        /// </summary>
        public Int16 AIRMILESPREMIUM { get { return this.BAMilesPremium; } set { this.AirMilesPremium = value; } }
        /// <summary>
        ///  VirginAtlantic
        /// </summary>
        public Int16 VIRGINATLANTIC { get { return this.ViginAtlantic; } set { this.ViginAtlantic = value; } }
        /// <summary>
        ///  XmasSaver
        /// </summary>
        public Int16 XMASSAVER { get { return this.XmasSaver; } set { this.XmasSaver = value; } }
        
        /// <summary>
        ///  SaveTree
        /// </summary>
        public Int16 SAVETREE { get { return this.SaveTree; } set { this.SaveTree = value; } }

        #region For NGC 3.6 Release

        //Author : Sabhareesan O.K

        /// <summary>
        /// ViaMail
        /// </summary>
        public Int16 ViaMail { get { return this._ViaMail; } set { this._ViaMail = value; } }

        /// <summary>
        /// ViaSMS
        /// </summary>
        public Int16 ViaSMS { get { return this._ViaSMS; } set { this._ViaSMS = value; } }

        /// <summary>
        /// ViaPost
        /// </summary>
        public Int16 ViaPost { get { return this._ViaPost; } set { this._ViaPost = value; } }

        /// <summary>
        /// ClubcardEMails
        /// </summary>
        public Int16 ClubcardEMails { get { return this._ClubcardEMails; } set { this._ClubcardEMails = value; } }

        /// <summary>
        /// BabyToddler
        /// </summary>
        public Int16 BabyToddler { get { return this._BabyToddler; } set { this._BabyToddler = value; } }

        /// <summary>
        /// Preference MembershipID
        /// </summary>
        public int PreferenceMembershipID { get { return this._PreferenceMembershipID; } set { this._PreferenceMembershipID = value; } }
        /// <summary>
        /// Preference EmailID
        /// </summary>
        public string PreferenceEmailID { get { return this._PreferenceEmailID; } set { this._PreferenceEmailID = value; } }
        /// <summary>
        /// Preference Mobile
        /// </summary>
        public string PreferenceMobile { get { return this._PreferenceMobile; } set { this._PreferenceMobile = value; } }



        #endregion

        #endregion


        #region Config Details


        #endregion
        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public Preference()
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
        #region Method

        #region Your Preferences

        /// <summary>
        /// To get (view) the customer preference details based on the given CustomerID.
        /// Added by Sadanand on 25-Mar-2010.
        /// </summary>
        public String ViewCustomerPreference(Int64 customerID, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Preference.ViewCustomerPreference");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Preference.ViewCustomerPreference - customerID :" + customerID.ToString());
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CUSTOMER_PREFERENCES, customerID);
                ds.Tables[0].TableName = "CustomerPreference";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Preference.ViewCustomerPreference");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Preference.ViewCustomerPreference - viewXml :" + viewXml.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Preference.ViewCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Preference.ViewCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Preference.ViewCustomerPreference");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return viewXml;
        }

        /// <summary>
        /// Update Customer Preference
        /// </summary>
        /// <param name="objectXml">Preference details</param>/// 
        /// <returns></returns>
        public bool UpdateCustomerPreference(string objectXml, int userID, out long objectId, out string resultXml, char level)
        {

            objectId = 0;
            bool success;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Preference.UpdateCustomerPreference");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Preference.UpdateCustomerPreference - objectXml :" + objectXml.ToString());
                Hashtable htblCustPreference = ConvertXmlHash.XMLToHashTable(objectXml, "CustomerPreference");
                this.CustomerID = Convert.ToInt32(htblCustPreference[Constants.CUSTOMER_ID]);

                //Data Protection Preferences--V3.1[Req ID - 007]
                if (htblCustPreference["TescoMailFlag"] != null) this.TescoGroupMailFlag = Convert.ToInt16(htblCustPreference["TescoMailFlag"].ToString());
                if (htblCustPreference["PartnerMailFlag"] != null) this.PartnerMailFlag = Convert.ToInt16(htblCustPreference["PartnerMailFlag"].ToString());
                if (htblCustPreference["ResearchPhoneFlag"] != null) this.ResearchPhoneFlag = Convert.ToInt16(htblCustPreference["ResearchPhoneFlag"].ToString());
                if (htblCustPreference["Ecoupon"] != null) this.Ecoupon = Convert.ToInt16(htblCustPreference["Ecoupon"].ToString());
                if (htblCustPreference["BAMilesStd"] != null) this.BAMilesStd = Convert.ToInt16(htblCustPreference["BAMilesStd"].ToString());
                //if (htblCustPreference["BAMilesPremium"] != null) this.BAMilesPremium = Convert.ToInt16(htblCustPreference["BAMilesPremium"].ToString());
                if (htblCustPreference["BAMilesPremium"] != null) this.BAMilesPremium = Convert.ToInt16(DBNull.Value);
                if (htblCustPreference["AirMilesStd"] != null) this.AirMilesStd = Convert.ToInt16(htblCustPreference["AirMilesStd"].ToString());
                //if (htblCustPreference["AirMilesPremium"] != null) this.AirMilesPremium = Convert.ToInt16(htblCustPreference["AirMilesPremium"].ToString());
                if (htblCustPreference["AirMilesPremium"] != null) this.AirMilesPremium = Convert.ToInt16(DBNull.Value);
                if (htblCustPreference["VirginAtlantic"] != null) this.ViginAtlantic = Convert.ToInt16(htblCustPreference["VirginAtlantic"].ToString());
                if (htblCustPreference["XmasSaver"] != null) this.XmasSaver = Convert.ToInt16(htblCustPreference["XmasSaver"].ToString());
                if (htblCustPreference["SaveTree"] != null) this.SaveTree = Convert.ToInt16(htblCustPreference["SaveTree"].ToString());

                #region For NGC 3.6 - Added New Preferences
                //Author : Sabhareesan O.K
                //Modified Date: 01-Feb-2011
                //after AirMilesPremium - virigin

                if (htblCustPreference["ViaMail"] != null) this.ViaMail = Convert.ToInt16(htblCustPreference["ViaMail"].ToString());
                if (htblCustPreference["ViaSMS"] != null) this.ViaSMS = Convert.ToInt16(htblCustPreference["ViaSMS"].ToString());
                if (htblCustPreference["ViaPost"] != null) this.ViaPost = Convert.ToInt16(htblCustPreference["ViaPost"].ToString());
                if (htblCustPreference["ClubcardEMails"] != null) this.ClubcardEMails = Convert.ToInt16(htblCustPreference["ClubcardEMails"].ToString());
                if (htblCustPreference["BabyToddler"] != null) this.BabyToddler = Convert.ToInt16(htblCustPreference["BabyToddler"].ToString());
                if (htblCustPreference["MembershipID"] != null) this.PreferenceMembershipID = Convert.ToInt32(htblCustPreference["MembershipID"].ToString());
                if (htblCustPreference["EmailID"] != null) this.PreferenceEmailID = Convert.ToString(htblCustPreference["EmailID"].ToString());
                if (htblCustPreference["Mobile"] != null) this.PreferenceMobile = Convert.ToString(htblCustPreference["Mobile"].ToString());

                #endregion


                object[] objCustPrefParams = {             
                                        level,
                                        CustomerID,
                                        userID, 
                                        TescoGroupMailFlag,
                                        PartnerMailFlag,
                                        ResearchPhoneFlag,
                                        BAMilesStd,
                                        BAMilesPremium,
                                        AirMilesStd,
                                        AirMilesPremium,                                        
                                        XmasSaver,
                                        Ecoupon,
                                        SaveTree,
                                        ViaMail,
                                        ViaSMS,
                                        ViaPost,
                                        ViginAtlantic,
                                        ClubcardEMails,
                                        BabyToddler,
                                        PreferenceMembershipID,
                                        PreferenceEmailID,
                                        PreferenceMobile
                                     };

                //calls the SP to Update Customer Preference.
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CUSTOMER_PREFERENCES, objCustPrefParams);
                success = SqlHelper.result.Flag;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Preference.UpdateCustomerPreference");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Preference.UpdateCustomerPreference");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Preference.UpdateCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Preference.UpdateCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Preference.UpdateCustomerPreference");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return success;
        }
        #endregion
        
        #endregion
    }
}
