using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CCOHostIntegrationService.SVServiceReference;
using Tesco.com.IntegrationServices;
using Tesco.com.IntegrationServices.Messages;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Fujitsu.eCrm.Generic.SharedUtils;


namespace Tesco.com.IntegrationServices
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class SmartVoucherServices : ISmartVoucherServices
    {
        MessagingWebService svClient = null;        
        DataSet ds = null;

        public GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber)
        {
            GetRewardDtlsRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetRewardDtls");
            StringBuilder sb = new StringBuilder();
            sb.Append(" ClubcardNumber: " + ClubcardNumber);
            trace.WriteInfo(sb.ToString());            
            #endregion

            try
            {
                response = new GetRewardDtlsRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.ClubcardOnlineGetRewardDetails(ClubcardNumber);
                response = new GetRewardDtlsRsp(ds);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "ClubcardNumber:" + ClubcardNumber);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                response.ErrorMessage = ex.ToString();
            }
            finally
            {
                trState.EndProc();
            }

            return response;
        }
        
        public GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber)
        {
            GetUnusedVoucherDtlsRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetUnusedVoucherDtls");
            StringBuilder sb = new StringBuilder();
            sb.Append(" ClubcardNumber: " + ClubcardNumber);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                response = new GetUnusedVoucherDtlsRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.ClubcardOnlineGetUnusedVoucherDetails(ClubcardNumber);
                response = new GetUnusedVoucherDtlsRsp(ds);

            }
            catch (Exception ex)
            {
               Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "ClubcardNumber:" + ClubcardNumber);
               //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
               response.ErrorMessage = ex.ToString();
            }
            finally
            {
                trState.EndProc();
            }

            return response;
        }

        public GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber)
        {
            GetUsedVoucherDtlsRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetUsedVoucherDtls");
            StringBuilder sb = new StringBuilder();
            sb.Append(" ClubcardNumber: " + ClubcardNumber);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                response = new GetUsedVoucherDtlsRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.ClubcardOnlineGetUsedVoucherDetails(ClubcardNumber);
                response = new GetUsedVoucherDtlsRsp(ds);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "ClubcardNumber:" + ClubcardNumber);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                response.ErrorMessage = ex.ToString();
            }
            finally
            {
                trState.EndProc();
            }

            return response;
        }

        public GetVoucherValHHRsp GetVoucherValHH(string Household_ID, string CPStartDate, string CPEndDate)
        {
            GetVoucherValHHRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetVoucherValHH");
            StringBuilder sb = new StringBuilder();
            sb.Append(" HouseholdID: " + Household_ID);
            sb.Append(" CPStartDate: " + CPStartDate);
            sb.Append(" CPEndDate: " + CPEndDate);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                response = new GetVoucherValHHRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.ClubcardOnlineGetVoucherValueForHousehold(Household_ID,CPStartDate,CPEndDate);
                response = new GetVoucherValHHRsp(ds);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "HouseHoldid:" + Household_ID
                    + ":CPStartDate:" + CPStartDate + ":CPEndDate:" + CPEndDate);
            }
            finally
            {
                trState.EndProc();
            }

            return response;
        }

        public GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode)
        {
            GetRewardDtlsMilesRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetRewardDtlsMiles");
            StringBuilder sb = new StringBuilder();
            sb.Append(" ClubcardNumber: " + ClubcardNumber);
            sb.Append(" ReasonCode: " + ReasonCode);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                response = new GetRewardDtlsMilesRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.GetRewardDetailsMiles(ClubcardNumber,ReasonCode);
                response = new GetRewardDtlsMilesRsp(ds);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "ClubcardNumber:" + ClubcardNumber);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                response.ErrorMessage = ex.ToString();
            }
            finally
            {
                trState.EndProc();
            }

            return response;
        }

        public GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate)
        {
            GetVoucherValAllCPSRsp response = null;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SmartVoucherServices.GetVoucherValCPS");
            StringBuilder sb = new StringBuilder();
            sb.Append(" ClubcardNumber: " + Clubcard_Number);
            sb.Append(" CPStartDate: " + CPStartDate);
            sb.Append(" CPEndDate: " + CPEndDate);
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                response = new GetVoucherValAllCPSRsp();
                ds = new DataSet();
                svClient = new MessagingWebService();
                ds = svClient.ClubcardOnlineGetVoucherValueForAllCPS(Clubcard_Number,CPStartDate,CPEndDate);
                response = new GetVoucherValAllCPSRsp(ds);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "ClubcardNumber:" + Clubcard_Number
                + ":CPStartDate:" + CPStartDate + ":CPEndDate:" + CPEndDate);
            }
            finally
            {
                trState.EndProc();
            }


            return response;
        }
    }
}
