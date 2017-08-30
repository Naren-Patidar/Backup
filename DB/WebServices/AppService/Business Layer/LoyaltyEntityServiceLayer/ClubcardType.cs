using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;

using System.Configuration;
using System.Collections;


namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class ClubcardType
    {
        #region Header
        ///
        /// <summary>
        /// Fields, Properties, Methods regarding ClubCard Type
        /// </summary>
        /// <development>
        ///		<version number="1.00" date="13/Aug/2008">
        ///			<developer>Ramesh</developer>
        ///			<Reviewer></Reviewer>
        ///			<description>Initial Implementation</description>
        ///		</version>
        ///	<development>
        ///	
        #endregion

        #region Fields

        /// Tesco ClubcardType        
        private short clubcardType;

        // Clubcard Type Description
        private string clubcardTypeDescription;

        /// Tesco Card Number Length      
        private Int16 cardNumberLength;

        // Insert DateTime   
        private DateTime insertDateTime;

        // InsertBy    
        private short insertBy;

        // Amend DateTime   
        private DateTime amendDateTime;

        // Amend By    
        private short amendBy;

        //is Deleted
        private char isDeleted;

        

        #endregion

        #region Properties

        /// <summary>
        ///  Clubcard Type
        /// </summary>
        public short ClubcardTypeID
        {
            get { return this.clubcardType; }
            set { this.clubcardType = value; }
        }

        /// <summary>
        ///  Clubcard Type Description
        /// </summary>
        public string ClubcardTypeDescription
        {
            get { return this.clubcardTypeDescription; }
            set { this.clubcardTypeDescription = value; }
        }

        /// <summary>
        ///  Length of the Clubcard Number
        /// </summary>
        public short CardNumberLength
        {
            get { return this.cardNumberLength; }
            set { this.cardNumberLength = value; }
        }

        /// <summary>
        ///  Insert DateTime
        /// </summary>
        public DateTime InsertDateTime
        {
            get { return this.insertDateTime; }
            set { this.insertDateTime = value; }
        }

        /// <summary>
        ///  Insert By
        /// </summary>
        public short InsertBy
        {
            get { return this.insertBy; }
            set { this.insertBy = value; }
        }

        /// <summary>
        ///  Amend DateTime
        /// </summary>
        public DateTime AmendDateTime
        {
            get { return this.amendDateTime; }
            set { this.amendDateTime = value; }
        }

        /// <summary>
        ///  Amend By
        /// </summary>
        public short AmendBy
        {
            get { return this.amendBy; }
            set { this.amendBy = value; }
        }

        /// <summary>
        ///  Is Delete
        /// </summary>
        public char IsDeleteted
        {
            get { return this.isDeleted; }
            set { this.isDeleted = value; }
        }

        #endregion
         //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ClubcardType()
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

        #region VIEW
        /// <summary>
        /// To get all the ClubcardRange details
        /// </summary>

        public string ViewAllCardTypes(string conditionXml, int maxRowCount, out int rowCount, string culture)
        {
            
            DataSet ds = new DataSet();
            rowCount = 0;
            string flag = "0";
            string viewXml = String.Empty;
            if (conditionXml == "FROMMCA")
            {
                flag = "1";
            }
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ClubcardType.ViewAllCardTypes");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ClubcardType.ViewAllCardTypes - conditionXml :" + conditionXml.ToString()); 
                object[] objCardType = { culture, flag };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_ALL_CLUBCARDTYPES, objCardType);
                ds.Tables[0].TableName = "ClubcardType";
                rowCount = ds.Tables[0].Rows.Count;
                if (rowCount > maxRowCount)
                    viewXml = "";
                else
                    viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ClubcardType.ViewAllCardTypes ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ClubcardType.ViewAllCardTypes - viewXml :" + viewXml.ToString()); 
               
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ClubcardType.ViewAllCardTypes - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ClubcardType.ViewAllCardTypes - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ClubcardType.ViewAllCardTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
            }
            return viewXml;
        }
        #endregion

        #region ADD
        /// <summary>
        /// To Add the ClubCard Type details to the database
        /// </summary>

        public bool AddCardTypes(string objectXml, int userID, out long objectId, out string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;
            string flag = "0";
            string viewXml = String.Empty;
            string SequenceTable = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ClubcardType.AddCardTypes");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ClubcardType.AddCardTypes - objectXml :" + objectXml);
                Hashtable htCardTypes = ConvertXmlHash.XMLToHashTable(objectXml, "ClubcardType");
                this.ClubcardTypeID = Convert.ToInt16(htCardTypes[Constants.CLUBCARD_TYPE]);
                this.ClubcardTypeDescription = (string)htCardTypes[Constants.CLUBCARD_DESC];
                this.CardNumberLength = Convert.ToInt16(htCardTypes[Constants.CLUBCARD_NUMBERLENGTH]);
                SequenceTable = (string)htCardTypes[Constants.CLUBCARD_SEQ_TABLE];
                if (htCardTypes["ConditionalFlag"].ToString() == "1")
                {
                    flag = "1";
                }
                
                object[] objAddCardType = { ClubcardTypeDescription, CardNumberLength, SequenceTable, userID, flag };
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CLUBCARDType, objAddCardType);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ClubcardType.AddCardTypes ");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ClubcardType.AddCardTypes");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ClubcardType.AddCardTypes - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ClubcardType.AddCardTypes - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ClubcardType.AddCardTypes");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                
            }
            finally
            {
               
            }
            return SqlHelper.result.Flag;

        }
        #endregion

        #region UPDATE
        /// <summary>
        /// To update CardType details 
        /// </summary>
        public bool Update(string objectXml, int userID, out long objectId, out string resultXml)
        {
           
            objectId = 0;
            resultXml = string.Empty;
            string flag = "0";
            string SequenceTable = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ClubcardType.Update ");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ClubcardType.Update - objectXml :" + objectXml);
                Hashtable htCardTypes = ConvertXmlHash.XMLToHashTable(objectXml, "ClubcardType");
                this.ClubcardTypeID = Convert.ToInt16(htCardTypes[Constants.CLUBCARD_TYPE]);
                this.ClubcardTypeDescription = (string)htCardTypes[Constants.CLUBCARD_DESC_UPDATE];
                this.CardNumberLength = Convert.ToInt16(htCardTypes[Constants.CLUBCARD_NUMBERLENGTH]);
                SequenceTable = (string)htCardTypes[Constants.CLUBCARD_SEQ_TABLE];
                if (htCardTypes["ConditionalFlag"].ToString() == "1")
                {
                    flag = "1";
                }
                
                object[] objAddCardType = { ClubcardTypeDescription, CardNumberLength, ClubcardTypeID, userID, SequenceTable, flag };
                SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARDType, objAddCardType);
                objectId = this.ClubcardTypeID;
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ClubcardType.Update");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ClubcardType.Update");
            }

            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ClubcardType.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ClubcardType.Update - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ClubcardType.Update");
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
