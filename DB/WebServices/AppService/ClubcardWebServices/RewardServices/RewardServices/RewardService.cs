using System;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Tesco.com.ExchangesService
{
    public class RewardService : IRewardService
    {
        string connection = string.Empty;
        DataSet ds = null;
        string connectionString = string.Empty;
        string sresultXml = string.Empty;
        string serrorXml = string.Empty;

        public RewardService()
        {
            connection = Convert.ToString(ConfigurationSettings.AppSettings["ConnectionString"]);
            connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
        }

        public bool GetRewardDetail(long customerID, string culture, out string serrorXml, out string sresultXml)
        {
            Trace trace = null;
            ITraceState trState = null;
            bool bResult = false;
            DataSet dsCustomer = null;
            DataSet dsClubcard = null;
            string clubCardIds = string.Empty;
            long lcustomerID = customerID;
            string CulutreInfo = culture;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            serrorXml = string.Empty;
            sresultXml = string.Empty;

            try
            {
                ///////Tracing code starts here///////////
                trace = new Trace();
                trState = trace.StartProc("GetRewardDetail");
                trace.WriteInfo("CustomerID:" + customerID);
                ds = new DataSet();
                dsCustomer = new DataSet();
                dsCustomer = SqlHelper.ExecuteDataset(connectionString, "USP_Get_Household_Customers", lcustomerID);
                dsCustomer.Tables[0].TableName = "HouseholdCustomers";

                if (dsCustomer.Tables["HouseholdCustomers"].Rows.Count > 0)
                {
                    for (int i = 0; i < dsCustomer.Tables["HouseholdCustomers"].Rows.Count; i++)
                    {

                        lcustomerID = Convert.ToInt64(dsCustomer.Tables["HouseholdCustomers"].Rows[i].ItemArray[1].ToString().Trim());
                        dsClubcard = new DataSet();
                        dsClubcard = SqlHelper.ExecuteDataset(connectionString, "USP_Get_Clubcard_Recs", lcustomerID);
                        dsClubcard.Tables[0].TableName = "ClubCardDetails";

                        if (dsClubcard.Tables.Count > 0)
                        {
                            for (int j = 0; j < dsClubcard.Tables["ClubCardDetails"].Rows.Count; j++)
                            {
                                if (clubCardIds == "")
                                {
                                    clubCardIds = dsClubcard.Tables["ClubCardDetails"].Rows[j].ItemArray[0].ToString().Trim();
                                }
                                else
                                {
                                    clubCardIds = clubCardIds + "," + dsClubcard.Tables["ClubCardDetails"].Rows[j].ItemArray[0].ToString().Trim();
                                }
                            }
                        }
                        else
                        {
                            //Log service error, errorXml
                            throw new Exception(errorXml);
                        }
                    }
                }
                ds = SqlHelper.ExecuteDataset(connection, "USP_Get_RewardDetail", clubCardIds);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RewardDetails";
                    ds.Tables[1].TableName = "TokenDetails";
                }

                sresultXml = ds.GetXml();
                bResult = true;

            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Exchanges");
                serrorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                /*Tracing code*/
                trState.EndProc();
                trace = null;
                trState = null;
            }

            return bResult;
        }

        public bool GetTokenInfo(Guid guid, long bookingid, long productlineid, string culture, out string serrorXml, out string sresultXml)
        {
            Trace trace = null;
            ITraceState trState = null;
            bool bResult = false;
            string CulutreInfo = culture;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            serrorXml = string.Empty;
            sresultXml = string.Empty;

            try
            {
                ///////Tracing code starts here///////////
                trace = new Trace();
                trState = trace.StartProc("ActiveDateRangeConfig.GetTokenForPrint");
                trace.WriteInfo("Exchanges:" + guid);
                ds = new DataSet();

                object[] objDBParams = { guid, bookingid, productlineid };
                ds = SqlHelper.ExecuteDataset(connection, "USP_Get_RwdDetailbyGuidBIDandPLID", objDBParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TokenInfo";
                }

                sresultXml = ds.GetXml();
                bResult = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Exchanges");
                serrorXml = ex.InnerException.ToString();
                bResult = false;
            }
            finally
            {
                /*Tracing code*/
                trState.EndProc();
                trace = null;
                trState = null;
            }

            return bResult;
        }

        public bool InsertInStoreTokens(DataSet dsInStoreTokens)
        {
            Trace trace = null;
            ITraceState trState = null;
            bool bResult = false;
            bool bTableFlag = true;
            bool bTokenFlag = true;
            try
            {
                trace = new Trace();
                trState = trace.StartProc("Exchanges.InsertInstoreTokens");
                trace.WriteInfo("Exchanges:");

                string resultXml = dsInStoreTokens.GetXml();

                object[] objDBParams = { resultXml };
                if (dsInStoreTokens.Tables.Count > 0 && dsInStoreTokens.Tables[0].TableName == "ProductLine")
                {
                    foreach (DataRow row in dsInStoreTokens.Tables[0].Rows)
                    {
                        if (string.IsNullOrEmpty(row["ProductCode"].ToString().Trim())
                            || string.IsNullOrEmpty(row["ProductType"].ToString().Trim()) || string.IsNullOrEmpty(row["Clubcard"].ToString().Trim())
                            || string.IsNullOrEmpty(row["ProductLineId"].ToString().Trim()) || string.IsNullOrEmpty(row["BookingID"].ToString().Trim()))
                        {
                            bTokenFlag = false;
                            break;
                        }
                    }
                }
                else
                {
                    bTableFlag = false;
                }
                if (dsInStoreTokens.Tables.Count > 2 && bTokenFlag && dsInStoreTokens.Tables[1].TableName == "Tokens" && dsInStoreTokens.Tables[2].TableName == "Token")
                {
                    foreach (DataRow row in dsInStoreTokens.Tables[2].Rows)
                    {
                        if (string.IsNullOrEmpty(row["TokenId"].ToString().Trim()) || string.IsNullOrEmpty(row["ValidUntil"].ToString().Trim())
                            || string.IsNullOrEmpty(row["SupplierTokenCode"].ToString().Trim())
                            || string.IsNullOrEmpty(row["ProductLineId"].ToString().Trim()) || string.IsNullOrEmpty(row["BookingID"].ToString().Trim()))
                        {
                            bTokenFlag = false;
                            break;
                        }
                    }
                }
                else
                {
                    bTableFlag = false;
                }
                if (bTokenFlag && bTableFlag)
                {
                    SqlHelper.ExecuteNonQuery(connection, "USP_InsertUpdateInStoreTokenDetails", objDBParams);
                    bResult = true;
                }

            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Exchanges");
                bResult = false;
            }
            finally
            {
                /*Tracing code*/
                trState.EndProc();
                trace = null;
                trState = null;
            }

            return bResult;
        }
    }
}