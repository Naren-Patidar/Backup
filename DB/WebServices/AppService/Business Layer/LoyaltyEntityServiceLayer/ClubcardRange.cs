using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Tesco.NGC.Utils;
using Tesco.NGC.DataAccessLayer;

using System.Configuration;
using System.Collections;
//using Microsoft.ApplicationBlocks.ExceptionManagement.

namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class ClubcardRange
    {
        #region Header
        ///
        /// <summary>
        /// Details of Clubcard Range
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

        // Clubcard Range Id        
        private int clubcardRangeID;

        // mincardRangeNumber
        private long mincardNumber;

        /// Tesco MaxcardNumber      
        private long maxcardNumber;

        /// Tesco ClubcardType        
        private int clubcardType;

        // Insert DateTime   
        private DateTime insertDateTime;

        // InsertBy    
        private short insertBy;

        // Amend DateTime   
        private DateTime amendDateTime;

        // Amend By    
        private short amendBy;

        //is Delete
        private char isDelete;

        
        #endregion

        #region Properties

        /// <summary>
        ///  Clubcard Range ID
        /// </summary>
        public int ClubcardRangeID { get { return this.clubcardRangeID; } set { this.clubcardRangeID = value; } }

        /// <summary>
        ///  MinCard Number
        /// </summary>
        public long MinCardNumber { get { return this.mincardNumber; } set { this.mincardNumber = value; } }

        /// <summary>
        ///  MaxCard Number
        /// </summary>
        public long MaxCardNumber { get { return this.maxcardNumber; } set { this.maxcardNumber = value; } }

        /// <summary>
        ///  Clubcard Type
        /// </summary>
        public int ClubcardType { get { return this.clubcardType; } set { this.clubcardType = value; } }

        /// <summary>
        ///  Insert DateTime
        /// </summary>
        public DateTime InsertDateTime { get { return this.insertDateTime; } set { this.insertDateTime = value; } }

        /// <summary>
        ///  Insert By
        /// </summary>
        public short InsertBy { get { return this.insertBy; } set { this.insertBy = value; } }

        /// <summary>
        ///  Amend DateTime
        /// </summary>
        public DateTime AmendDateTime { get { return this.amendDateTime; } set { this.amendDateTime = value; } }

        /// <summary>
        ///  Amend By
        /// </summary>
        public short AmendBy { get { return this.amendBy; } set { this.amendBy = value; } }

        /// <summary>
        ///  Is Delete
        /// </summary>
        public char IsDelete { get { return this.isDelete; } set { this.isDelete = value; } }

        #endregion
        //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ClubcardRange()
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
        public string ViewCardRange(string conditionXml, int maxRowCount, int rowCount, string culture)
        {

            DataSet ds = new DataSet();
            string viewXml = String.Empty;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.ViewCardRange");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.ViewCardRange - conditionXml" + conditionXml.ToString()); 
                object[] objCardRangeDetails = { };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CLUBCARDRANGE,objCardRangeDetails);
                ds.Tables[0].TableName = "ClubcardRange";
                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.ViewCardRange");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.ViewCardRange - viewXml " + viewXml.ToString()); 
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.ViewCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.ViewCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.ViewCardRange");
                NGCTrace.NGCTrace.ExeptionHandling(ex); 
            }
            finally
            {
                
            }
            return viewXml;
        }

        public string View(long ClubcardRangeId, string culture)
        {

           
            DataSet ds = new DataSet();
            string viewXml = String.Empty;


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.View - ClubcardRangeId : " + ClubcardRangeId.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.View - ClubcardRangeId : " + ClubcardRangeId.ToString()); 
                
                object[] objPopupData = { ClubcardRangeId };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.SP_VIEW_CLUBCARDRANGE_POPUP, objPopupData);

                viewXml = ds.GetXml();
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.View - ClubcardRangeId : " + ClubcardRangeId.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.View - viewXml : " + viewXml.ToString()); 
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.View - Error Message :" + ex.ToString() + " - ClubcardRangeId :" + ClubcardRangeId.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.View - Error Message :" + ex.ToString() + " - ClubcardRangeId :" + ClubcardRangeId.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.View");
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
        /// To Add the ClubCard Range details to the database
        /// </summary>

        public bool AddCardRange(string objectXml, int userID, long objectId, string resultXml)
        {
            
            objectId = 0;
            resultXml = string.Empty;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.AddCardRange");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.AddCardRange - objectXml : " + objectXml.ToString());
                Hashtable htblAddCardRange = ConvertXmlHash.XMLToHashTable(objectXml, "ClubcardRange");
                this.MinCardNumber = Convert.ToInt64(htblAddCardRange[Constants.MIN_CARD_NUMBER]);
                this.MaxCardNumber = Convert.ToInt64(htblAddCardRange[Constants.MAX_CARD_NUMBER]);
                // this.InsertDateTime = (DateTime)(htblAddCardRange[Constants.INSERTED_DATETIME_CARDRANGE]);
                this.ClubcardType = Convert.ToInt32(htblAddCardRange[Constants.CARD_TYPE]);
                object[] objAddCardRanges = {    
                                              MinCardNumber,
                                              MaxCardNumber,
                                             // DateTime.Now,
                                              ClubcardType,
                                              userID                                 
                                            };
                
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_ADD_CLUBCARDRANGE,objAddCardRanges);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.AddCardRange");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.AddCardRange");
            }
            catch (Exception ex)
            {
                resultXml = SqlHelper.resultXml;
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.AddCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.AddCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.AddCardRange");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                
            }
            finally
            {
                
            }
            //return SqlHelper.result.Flag; Ram Later it should be Uncommented 
            return true;
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// To update ClubcardRange details 
        /// </summary>
        public bool UpdateCardRange(string objectXml, int userID, long objectId, string resultXml)
        {
           
            int identityParam = 0;
            objectId = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.UpdateCardRange - objectXml : " + objectXml.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.UpdateCardRange - objectXml : " + objectXml.ToString());
                Hashtable htUpdateCardRange = ConvertXmlHash.XMLToHashTable(objectXml, "ClubcardRange");


                this.MinCardNumber = Convert.ToInt64(htUpdateCardRange[Constants.MIN_CARD_NUMBER]);
                this.MaxCardNumber = Convert.ToInt64(htUpdateCardRange[Constants.MAX_CARD_NUMBER]);
                //this.InsertDateTime = (DateTime)htUpdateCardRange[Constants.INSERTED_DATETIME_CARDRANGE];
                this.ClubcardRangeID = Convert.ToInt32(htUpdateCardRange[Constants.CARD_RANGE_ID]);
                object[] objUpdateCardRange = { 
                                        MinCardNumber,  
                                        MaxCardNumber,
                                       // DateTime.Now,//InsertDateTime,
                                        ClubcardRangeID,
                                        userID
                                     };
               
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_UPDATE_CLUBCARDRANGE,objUpdateCardRange);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.UpdateCardRange - objectXml : " + objectXml.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.UpdateCardRange - objectXml : " + objectXml.ToString());

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.UpdateCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.UpdateCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.UpdateCardRange");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  

            }
            finally
            {
                
            }
            return true;
        }

        #endregion

        #region DELETE
        /// <summary>
        /// To Delete ClubcardRange details 
        /// </summary>
        public bool DeleteCardRange(string objectXml, int userID, long objectId, string resultXml)
        {
            
            int retval = 0, identityParam = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.Customer.DeleteCardRange - objectXml : " + objectXml.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.Customer.DeleteCardRange - objectXml : " + objectXml.ToString());
                Hashtable htUpdateCardRange = ConvertXmlHash.XMLToHashTable(objectXml, "ClubcardRange");
                this.ClubcardRangeID = Convert.ToInt32(htUpdateCardRange[Constants.CARD_RANGE_ID]);
                object[] objDeleteCardRange = {                                         
                                        ClubcardRangeID
                                     };
                
                objectId = SqlHelper.ExecuteNonQuery(connectionString, Constants.SP_DELETE_CLUBCARDRANGE,objDeleteCardRange);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.Customer.DeleteCardRange - objectXml : " + objectXml.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.Customer.DeleteCardRange - objectId : " + objectId.ToString());
                //return retval;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.Customer.DeleteCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.Customer.DeleteCardRange - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.Customer.DeleteCardRange");
                NGCTrace.NGCTrace.ExeptionHandling(ex);  
                //return retval;
            }
            finally
            {
                
            }
            return true;
        }
        #endregion

        #endregion

    }
}
