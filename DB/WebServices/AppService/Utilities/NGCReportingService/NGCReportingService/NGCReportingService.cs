using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using System.Data;
using NGCTrace;

namespace NGCReportingServiceLayer
{
    public class NGCReportingService:INGCReportingService
    {
        #region GetRegistrationReport
        /// <summary>
        /// GetRegistrationReport -- It is used to fetch Customers joined through different join routes in specified time period
        /// </summary>
        /// <param name="DateTime">startdate</param>
        /// <param name="DateTime">enddate</param>
        /// <param name="Int">week</param>
        /// <param name="String">period</param>

        public DataSet GetRegistrationReport(string startdate, string enddate, int week, string period)
        {
            
            ReportSchedule scheduleObj = null;
            
            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

                ds = scheduleObj.GetRegistrationReport(startdate, enddate, week,  period);

                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.GetRegistrationReport ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion 
    
        
        #region GetPromotionalReport
        /// <summary>
        /// GetPromotionalReport -- It is used to fetch Customers joined through different Promotional codes in specified time period
        /// </summary>
        /// <param name="DateTime">startdate</param>
        /// <param name="DateTime">enddate</param>
        /// <param name="Int">week</param>
        /// <param name="String">period</param>
        public DataSet GetPromotionalReport(string startdate, string enddate, int week, string period)
        {
            ReportSchedule scheduleObj = null;
            
            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                
                ds = scheduleObj.GetPromotionalReport(startdate, enddate, week, period);

                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.GetPromotionalReport ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion 



        #region ScheduleReport
        /// <summary>
        /// ScheduleReport -- It is used to schedule reports
        /// </summary>
        /// <param name="string">ScheduleXml</param>
               
        public string ScheduleReport(string ScheduleXml)
        {


            ReportSchedule scheduleObj = null;
            scheduleObj = new ReportSchedule();
            string resultStr = "";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.ScheduleReport  ScheduleXml - " + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.ScheduleReport  ScheduleXml - " + ScheduleXml);

               resultStr = scheduleObj.ScheduleReport(ScheduleXml);

               NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.ScheduleReport  ScheduleXml - " + ScheduleXml);
               NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.ScheduleReport  ScheduleXml - " + ScheduleXml);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.ScheduleReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.ScheduleReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.ScheduleReport ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            
            }
            finally
            {
                scheduleObj = null;

            }

            return resultStr;
        }
        #endregion 


        #region TerminateSchedule
        /// <summary>
        /// TerminateSchedule -- It is used to terminate/delete report schedule 
        /// </summary>
        /// <param name="string">ScheduleXml</param>

        public string TerminateSchedule(string ScheduleXml)
        {


            ReportSchedule scheduleObj = null;
            scheduleObj = new ReportSchedule();
            string resultStr = "";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule  ScheduleXml - " + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule  ScheduleXml - " + ScheduleXml);

                resultStr = scheduleObj.TerminateSchedule(ScheduleXml);

                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule  ScheduleXml - " + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule  ScheduleXml - " + ScheduleXml);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.TerminateSchedule ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {
                scheduleObj = null;

            }

            return resultStr;
        }
        #endregion 

        #region Get Points Earned Report
        /// <summary>
        /// GetpointsEarnedReport -- It is used to fetch Customers joined through different join routes in specified time period
        /// </summary>
        /// <param name="DateTime">startdate</param>
        /// <param name="DateTime">enddate</param>

        public DataSet GetPointsEarnedReport(string startdate, string enddate,int calenderid, string period)
        {
            ReportSchedule scheduleObj = null;

            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport");
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport");

                ds = scheduleObj.GetPointsEarnedReport(startdate, enddate, calenderid, period);

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport");
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport");
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical::ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error::ClubcardOnlineService.NGCReportingService.GetPointsEarnedReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning::ClubcardOnlineService.NGCReportingService.GetPointsEarnedReports ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion 
        

        #region PopulateWeekData
        /// <summary>
        /// Author: Mohan Mahapatra
        /// PopulatePeriodData -- Get Weeks  from Tesco calendar table
        /// </summary>
        public DataSet PopulateWeekData()
        {
            ReportSchedule scheduleObj = null;

            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData");
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData");
                ds = scheduleObj.PopulateWeekData();
                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData");
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData");
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.PopulateWeekData ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion

        #region PopulatePeriodData
        /// <summary>
        /// Author: Mohan Mahapatra
        /// PopulatePeriodData -- Get Periods  from Tesco calendar table
        /// </summary>
        public DataSet PopulatePeriodData()
        {
            ReportSchedule scheduleObj = null;

            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData");
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData");

                ds = scheduleObj.PopulatePeriodData();

                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData");
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData");
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.PopulatePeriodData ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion
        #region GetCustomerLoadReport
        /// <summary>
        /// Author: Neeta Kewlani
        /// GetCustomerLoadReport -- Get details for Customer Load Report
        /// </summary>
        /// <param name="DateTime">startdate</param>
        /// <param name="DateTime">enddate</param>

        public DataSet GetCustomerLoadReport(string startdate, string enddate, int week, string period)
        {
            ReportSchedule scheduleObj = null;

            bool bResult = false;
            scheduleObj = new ReportSchedule();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

                ds = scheduleObj.GetCustomerLoadReport(startdate, enddate, week, period);

                NGCTrace.NGCTrace.TraceInfo("End:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport  StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceCritical("Critical:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCReportingServiceLayer.NGCReportingService.GetCustomerLoadReport ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }
            finally
            {
                scheduleObj = null;

            }

            return ds;
        }
        #endregion 
    }
}
