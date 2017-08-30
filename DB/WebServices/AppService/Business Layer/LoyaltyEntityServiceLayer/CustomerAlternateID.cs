/*
 * File   : CustomerAlternateID.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods/properties related to Customer Alternate ID
 * Date   : 06/Aug/2008
 * 
 */

#region using

using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using System.Xml;


#endregion

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    /// <summary>
    /// Tesco Customer Alternate ID Details
    /// </summary>
    public class CustomerAlternateID
    {

        #region Fields

        /// <summary>
        /// CustomerID
        /// </summary>
        private Int64 customerID;

        /// <summary>
        /// IDType
        /// </summary>
        private Int16 customerAlternateIDType;

        /// <summary>
        /// CustomerAlternateID
        /// </summary>
        private string custAlternateID;

        

        #endregion

        #region Properties

        /// <summary>
        ///  CustomerID
        /// </summary>
        public Int64 CustomerID
        {
            get{return this.customerID;}
            set{this.customerID = value;}
        }

        /// <summary>
        ///  CustomerAlternateIDType
        /// </summary>
        public Int16 CustomerAlternateIDType
        {
            get{return this.customerAlternateIDType;}
            set{this.customerAlternateIDType = value;}
        }

        /// <summary>
        ///  CustomerAlternateID
        /// </summary>
        public string CustAlternateID
        {
            get{return this.custAlternateID;}
            set{this.custAlternateID = value;}
        }

        #endregion

        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public CustomerAlternateID()
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

         #region Get .comIds of the Customer based on customerId
        /// <summary>
        /// To get .comAccounts of the customer aganist CustomerID
        /// </summary>
        public String GetAlternativeIds(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(conditionXml, "CustomerAlternateID");

            string viewXml = String.Empty;
            rowCount = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds - conditionXml:" + conditionXml.ToString());
                if (htblCustomer["CustomerID"] != null)
                {
                    if (htblCustomer["CustomerID"].ToString() != "")
                    {
                        this.CustomerID = Convert.ToInt64(htblCustomer["CustomerID"].ToString());
                    }
                }

                object[] objDBParams = { CustomerID };

               
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_GET_ALTERNATIVEACCOUNTS, objDBParams);
                if (ds.Tables.Count > 0)
                    ds.Tables[0].TableName = "CustomerAlternateID";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds - viewXml:" + viewXml.ToString());
            }
            catch (Exception Ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds - Error Message : " + Ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.CustomerAlternateID.GetAlternativeIds");
                NGCTrace.NGCTrace.ExeptionHandling(Ex);
            }
            finally
            {

            }
            return viewXml;
        }



        #endregion
        

        #region Delete .comIdsofthecustomer
        /// <summary>
        /// To delete .comAccounts of the customer aganist CustomerID
        /// </summary>

        public bool DeLinkingDotcomAccounts(string objectXml,out string resultXml)
        {

            resultXml = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts - objectXml :" + objectXml.ToString());
                Hashtable htblCustomerAlternateID = ConvertXmlHash.XMLToHashTable(objectXml, "CusAlterId");
                if (htblCustomerAlternateID["CustomerAlternateID"] != null)
                {
                    
                        this.custAlternateID = (string)htblCustomerAlternateID["CustomerAlternateID"];
                   
                
                }

                object[] objAppUser = { custAlternateID };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETEDELINKINGACCOUNTS, objAppUser);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.CustomerAlternateID.DeLinkingDotcomAccounts");
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
