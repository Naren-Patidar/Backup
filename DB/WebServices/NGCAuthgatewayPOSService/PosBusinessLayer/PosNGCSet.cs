#region Using

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Tesco.NGC.DataAccessLayer;
using System.Diagnostics;
using NGCTrace;

#endregion

namespace Tesco.NGC.PosBusinessLayer
{
    /// <summary>
    /// Class for handling Auth Gateway Update Request
    /// </summary>
    public class PosNGCSet
    {

        #region Private Member Variables

        SqlParameter[] paramList = new SqlParameter[23];

        string sessionGuid = null;

        #endregion Private Member Variables

        #region ProcessRequest Method

        /// <summary>
        /// Method to process the incoming request and get the result, called from webservice
        /// </summary>
        /// <param name="requestXml">Request</param>
        /// <returns>Result</returns>
        public PosNGCResult ProcessRequest(PosNGCReq requestObj)
        {
            PosNGCResult posNGCResult = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.ProcessRequest()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + "AlternateID : " + requestObj.AlternateID + System.Environment.NewLine + "BonusPoints: " + requestObj.BonusPoints + System.Environment.NewLine + "Branch : " + requestObj.Branch + System.Environment.NewLine + "Cashier : " + requestObj.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + requestObj.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + requestObj.ClubcardNo + System.Environment.NewLine + "Country : " + requestObj.Country + System.Environment.NewLine + "Currency : " + requestObj.Currency + System.Environment.NewLine + "Date : " + requestObj.Date + System.Environment.NewLine + "GreenPoints : " + requestObj.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + requestObj.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + requestObj.PointsEarned + System.Environment.NewLine + "QualifySpend : " + requestObj.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + requestObj.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + requestObj.SequenceNo + System.Environment.NewLine + "TillNo : " + requestObj.TillNo + System.Environment.NewLine + "TillVer : " + requestObj.TillVer + System.Environment.NewLine + "Time : " + requestObj.Time + System.Environment.NewLine + "TotalSpend : " + requestObj.TotalSpend + System.Environment.NewLine + "Training : " + requestObj.Training + System.Environment.NewLine + "TrsType : " + requestObj.TrsType + System.Environment.NewLine + "version : " + requestObj.version);

                posNGCResult = new PosNGCResult();

                // Building the paramter list to be sent into SP from the request 
                BuildParameterList(requestObj);

                // Sending the parameter list to execute SP
                string connectionString = ConfigurationSettings.AppSettings["AdminConnectionString"].ToString();

                SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, "USP_AuthGateway_Pos_Set", paramList);

                // Populating the result object with the updated parameter list
                posNGCResult = BuildResultObject(requestObj);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.ProcessRequest -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.ProcessRequest- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.ProcessRequest ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.ProcessRequest()" + System.Environment.NewLine + "posNGCResult : " + System.Environment.NewLine + "AlternateID : " + posNGCResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNGCResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNGCResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNGCResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNGCResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNGCResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNGCResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNGCResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNGCResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNGCResult.FlexMsgNo + System.Environment.NewLine + "FlexMsg : " + posNGCResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNGCResult.SessionId + System.Environment.NewLine + "Status : " + posNGCResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNGCResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNGCResult.Surname + System.Environment.NewLine + "Title : " + posNGCResult.Title + System.Environment.NewLine + "TrsType : " + posNGCResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNGCResult.UpToDate);
            }
            return posNGCResult;
        }

        #endregion ProcessRequest Method

        #region BuildParameterList Method

        /// <summary>
        /// Method for building the parameter list for processing the request by sending them to SP
        /// </summary>
        /// <param name="req">Request Object</param>
        private void BuildParameterList(PosNGCReq requestObj)
        {
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameterList()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + "AlternateID : " + requestObj.AlternateID + System.Environment.NewLine + "BonusPoints: " + requestObj.BonusPoints + System.Environment.NewLine + "Branch : " + requestObj.Branch + System.Environment.NewLine + "Cashier : " + requestObj.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + requestObj.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + requestObj.ClubcardNo + System.Environment.NewLine + "Country : " + requestObj.Country + System.Environment.NewLine + "Currency : " + requestObj.Currency + System.Environment.NewLine + "Date : " + requestObj.Date + System.Environment.NewLine + "GreenPoints : " + requestObj.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + requestObj.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + requestObj.PointsEarned + System.Environment.NewLine + "QualifySpend : " + requestObj.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + requestObj.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + requestObj.SequenceNo + System.Environment.NewLine + "TillNo : " + requestObj.TillNo + System.Environment.NewLine + "TillVer : " + requestObj.TillVer + System.Environment.NewLine + "Time : " + requestObj.Time + System.Environment.NewLine + "TotalSpend : " + requestObj.TotalSpend + System.Environment.NewLine + "Training : " + requestObj.Training + System.Environment.NewLine + "TrsType : " + requestObj.TrsType + System.Environment.NewLine + "version : " + requestObj.version);

                sessionGuid = Guid.NewGuid().ToString();

                ValidateTime(requestObj.Time);

                DateTime txnDate = GetDateTime(requestObj.Date + requestObj.Time);

                paramList[0] = BuildParameter("@CultureIsoCode", SqlDbType.VarChar, 7, ParameterDirection.Input, ConfigurationSettings.AppSettings["cultureInfo"].ToString());

                paramList[1] = BuildParameter("@UsrName", SqlDbType.NVarChar, 20, ParameterDirection.Input, ConfigurationSettings.AppSettings["userName"].ToString());

                paramList[2] = BuildParameter("@SessionID", SqlDbType.VarChar, 36, ParameterDirection.Input, sessionGuid);

                if (string.IsNullOrEmpty(requestObj.ClubcardNo))
                {
                    paramList[3] = BuildParameter("@ClubcardID", SqlDbType.BigInt, 19, ParameterDirection.Input, 0);
                }
                else
                {
                    paramList[3] = BuildParameter("@ClubcardID", SqlDbType.BigInt, 19, ParameterDirection.Input, long.Parse(requestObj.ClubcardNo));
                }
                paramList[4] = BuildParameter("@CustomerName", SqlDbType.NVarChar, 100, ParameterDirection.Input, ConfigurationSettings.AppSettings["customerName"].ToString());

                paramList[5] = BuildParameter("@TxnTypeCode", SqlDbType.SmallInt, 2, ParameterDirection.Input, 1);

                paramList[6] = BuildParameter("@TescoStoreID", SqlDbType.Int, 4, ParameterDirection.Input, Int32.Parse(requestObj.Branch));

                paramList[7] = BuildParameter("@TxnDate", SqlDbType.DateTime, 8, ParameterDirection.Input, txnDate);

                paramList[8] = BuildParameter("@PosID", SqlDbType.SmallInt, 6, ParameterDirection.Input, requestObj.TillNo);

                paramList[9] = BuildParameter("@TxnNbr", SqlDbType.Int, 5, ParameterDirection.Input, requestObj.ReceiptNo);

                paramList[10] = BuildParameter("@CashierID", SqlDbType.NVarChar, 20, ParameterDirection.Input, requestObj.Cashier);

                paramList[11] = BuildParameter("@AmountSpent", SqlDbType.Decimal, 17, ParameterDirection.Input, decimal.Parse(requestObj.TotalSpend));

                paramList[12] = BuildParameter("@TotalPoints", SqlDbType.Decimal, 17, ParameterDirection.Input, decimal.Parse(requestObj.PointsEarned));

                paramList[13] = BuildParameter("@WelcomePoints", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);

                //paramList[14] = BuildParameter("@ProductPoints", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);

                paramList[14] = BuildParameter("@ExtraPoints1", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);

                paramList[15] = BuildParameter("@ExtraPoints2", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);

                if (string.IsNullOrEmpty(requestObj.BonusPoints))
                {
                    paramList[16] = BuildParameter("@BonusPoints", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);
                }
                else
                {
                    paramList[16] = BuildParameter("@BonusPoints", SqlDbType.Decimal, 15, ParameterDirection.Input, decimal.Parse(requestObj.BonusPoints));
                }
                paramList[17] = BuildParameter("@Training", SqlDbType.Bit, 1, ParameterDirection.Input, requestObj.Training ? 1 : 0);

                paramList[18] = BuildParameter("@DefaultDataProtectionPref", SqlDbType.SmallInt, 2, ParameterDirection.Input, int.Parse(ConfigurationSettings.AppSettings["DefaultDataProtectionPreference"].ToString()));

                paramList[19] = BuildParameter("@AlternateID", SqlDbType.NVarChar, 50, ParameterDirection.Input, requestObj.AlternateID);

                if (string.IsNullOrEmpty(requestObj.GreenPoints))
                {
                    paramList[20] = BuildParameter("@GreenPoints", SqlDbType.Decimal, 15, ParameterDirection.Input, 0);
                }
                else
                {
                    paramList[20] = BuildParameter("@GreenPoints", SqlDbType.Decimal, 15, ParameterDirection.Input, decimal.Parse(requestObj.GreenPoints));
                }
                string createCustomerSkeleton = ConfigurationSettings.AppSettings["CreateCustomerSkeleton"].ToString();
                paramList[21] = BuildParameter("@CreateCustomerSkeletonFlag", SqlDbType.Char, 1, ParameterDirection.Input, createCustomerSkeleton.Trim());
                paramList[22] = BuildParameter("@StatusMsgNo", SqlDbType.SmallInt, 2, ParameterDirection.Output, null);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameterList -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameterList- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameterList ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameterList()");
            }
        }

        #endregion BuildParameterList Method

        #region BuildResultObject Method

        /// <summary>
        /// Method for populating the response object from the updated request object
        /// </summary>
        /// <param name="requestObj">Updated Request</param>
        /// <returns></returns>
        private PosNGCResult BuildResultObject(PosNGCReq requestObj)
        {
            PosNGCResult posNgcResult = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildResultObject()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + requestObj.AlternateID + System.Environment.NewLine + requestObj.BonusPoints + System.Environment.NewLine + requestObj.Branch + System.Environment.NewLine + requestObj.Cashier + System.Environment.NewLine + requestObj.CheckOutBankNo + System.Environment.NewLine + requestObj.ClubcardNo + System.Environment.NewLine + requestObj.Country + System.Environment.NewLine + requestObj.Currency + System.Environment.NewLine + requestObj.Date + System.Environment.NewLine + requestObj.GreenPoints + System.Environment.NewLine + requestObj.InterfaceVer + System.Environment.NewLine + requestObj.PointsEarned + System.Environment.NewLine + requestObj.QualifySpend + System.Environment.NewLine + requestObj.ReceiptNo + System.Environment.NewLine + requestObj.SequenceNo + System.Environment.NewLine + requestObj.TillNo + System.Environment.NewLine + requestObj.TillVer + System.Environment.NewLine + requestObj.Time + System.Environment.NewLine + requestObj.TotalSpend + System.Environment.NewLine + requestObj.Training + System.Environment.NewLine + requestObj.TrsType + System.Environment.NewLine + requestObj.version);

                posNgcResult = new PosNGCResult();

                posNgcResult.TrsType = requestObj.TrsType;

                posNgcResult.SessionId = sessionGuid;

                posNgcResult.ClubCardNo = requestObj.ClubcardNo;

                posNgcResult.AlternateID = requestObj.AlternateID;

                posNgcResult.StatusMsgNo = paramList[22].Value.ToString();

                //Changed from "0" to "1" as per Retalix Suggesstion (for printing and displaying  on cashier display.)
                if (posNgcResult.StatusMsgNo.Equals("1"))
                {
                    posNgcResult.Status = (PosNGCResultStatus)0;
                }
                else
                {
                    posNgcResult.Status = (PosNGCResultStatus)1;
                }
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildResultObject -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildResultObject- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildResultObject ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildResultObject()" + System.Environment.NewLine + "posNgcResult : " + System.Environment.NewLine + "AlternateID : " + posNgcResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNgcResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNgcResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNgcResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNgcResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNgcResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNgcResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNgcResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNgcResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNgcResult.FlexMsgNo + System.Environment.NewLine + "FlexMsg : " + posNgcResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNgcResult.SessionId + System.Environment.NewLine + "Status : " + posNgcResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNgcResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNgcResult.Surname + System.Environment.NewLine + "Title : " + posNgcResult.Title + System.Environment.NewLine + "TrsType : " + posNgcResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNgcResult.UpToDate);
            }
            return posNgcResult;
        }

        #endregion BuildResultObject Method

        #region BuildParameter Method

        /// <summary>
        /// Method to Build Parameter, set its direction and value
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="dbType">Parameter Type</param>
        /// <param name="size">Parameter Size</param>
        /// <param name="dir">Parameter Direction</param>
        /// <param name="paramVal">Parameter Value</param>
        /// <returns></returns>
        private SqlParameter BuildParameter(string paramName, SqlDbType dbType, int size, ParameterDirection dir, object paramVal)
        {
            SqlParameter param = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameter()" + System.Environment.NewLine + "paramName : " + paramName);

                param = new SqlParameter(paramName, dbType, size);

                param.Direction = dir;

                param.Value = paramVal;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameter -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameter- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameter ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.BuildParameter()");
            }
            return param;
        }

        #endregion BuildParameter Method

        #region GetDateTime Method

        /// <summary>
        /// Method to build a datetime object from date and time in request
        /// </summary>
        /// <param name="datetime">datetime string</param>
        /// <returns>DateTime object</returns>
        private DateTime GetDateTime(string datetime)
        {
            string centuryPart = string.Empty;

            StringBuilder tempDateTime = new System.Text.StringBuilder(datetime);

            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.GetDateTime()" + System.Environment.NewLine + "datetime : " + datetime);

                //tempDateTime.Insert(6,datetime.Substring(0,2));

                tempDateTime.Insert(8, datetime.Substring(2, 2));

                centuryPart = datetime.Substring(0, 2);

                tempDateTime.Remove(0, 4);

                tempDateTime = tempDateTime.Insert(2, "/");

                tempDateTime = tempDateTime.Insert(5, "/");

                tempDateTime = tempDateTime.Insert(8, " ");

                tempDateTime = tempDateTime.Insert(11, ":");

                tempDateTime = tempDateTime.Insert(14, ":");

                //tempDateTime = tempDateTime.Insert(6,"20");

                tempDateTime = tempDateTime.Insert(6, centuryPart);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.GetDateTime -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.GetDateTime- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.GetDateTime ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.GetDateTime()" + System.Environment.NewLine + "tempDateTime : " + tempDateTime.ToString());
            }
            return Convert.ToDateTime(tempDateTime.ToString());
        }

        #endregion GetDateTime Method

        #region ValidateTime Method

        /// <summary>
        /// Method to Validate Time 
        /// </summary>
        /// <param name="time">Time in HHMMSS format</param>
        /// <returns>Boolean</returns>
        private bool ValidateTime(string time)
        {
            bool retFlag = false;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCSet.ValidateTime()" + System.Environment.NewLine + "time : " + time);

                int temp = Int32.Parse(time);

                retFlag = true;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCSet.ValidateTime -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCSet.ValidateTime- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCSet.ValidateTime ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                retFlag = false;

                throw new Exception("Invalid Time Format");
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCSet.ValidateTime()" + System.Environment.NewLine + "retFlag : " + retFlag);
            }
            return retFlag;
        }

        #endregion ValidateTime Method

    }
}
