/*
 * File : ReportSchedule.cs
 * Author : Syed Amjadulla
 * Objective : Contains properties and methods to maintain the report scheduling
 * Date : 15th Sep'2009
 */
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Tesco.NGC.DataAccessLayer;
using Tesco.NGC.Utils;
using System.Data.SqlClient;
using NGCTrace;
namespace Tesco.NGC.Loyalty.EntityServiceLayer
{
    public class ReportSchedule
    {
        #region Fields
        private int scheduleID;
        private Int16 reportID;
        private string sqlJobName;
        private string emailAddresses;
        private char recurrenceType;
        private Int16 scheduleHours;
        private Int16 scheduleMinutes;
        private string reportParams;
        private string startDateRpt;
        private string endDateRpt;
        private string NGCreportFormatterdirectory;
        private string capabilityName;
        private string userName;
        private string scheduleTimeHHMM;
        private string emailRecepients;
        private string reportHeadings;
        private string culture;
        private string defaultculture;
        private string localisationpath;
        //private DateTime? startdate;
        //private DateTime? enddate;
        private string weeknumber;
        private string periodnumber;
        //private string year;
        private string calenderid;


        #endregion

        #region Properties

        public int ScheduleID { get { return this.scheduleID; } set { this.scheduleID = value; } }
        public Int16 ReportID { get { return this.reportID; } set { this.reportID = value; } }
        public string SqlJobName { get { return this.sqlJobName; } set { this.sqlJobName = value; } }
        public string EmailAddresses { get { return this.emailAddresses; } set { this.emailAddresses = value; } }
        public string ReportParams { get { return this.reportParams; } set { this.reportParams = value; } }
        public char RecurrenceType { get { return this.recurrenceType; } set { this.recurrenceType = value; } }
        public Int16 ScheduleHours { get { return this.scheduleHours; } set { this.scheduleHours = value; } }
        public Int16 ScheduleMinutes { get { return this.scheduleMinutes; } set { this.scheduleMinutes = value; } }
        public string StartDateRpt { get { return this.startDateRpt; } set { this.startDateRpt = value; } }
        public string EndDateRpt { get { return this.endDateRpt; } set { this.endDateRpt = value; } }
        public string WeekNumber { get { return this.weeknumber; } set { this.weeknumber = value; } }
        public string PeriodNumber { get { return this.periodnumber; } set { this.periodnumber = value; } }
        public string NGCReportFormatterDirectory { get { return this.NGCreportFormatterdirectory; } set { this.NGCreportFormatterdirectory = value; } }
        public string Capabilityname { get { return this.capabilityName; } set { this.capabilityName = value; } }
        public string UserName { get { return this.userName; } set { this.userName = value; } }
        public string ScheduleTimeHHMM { get { return this.scheduleTimeHHMM; } set { this.scheduleTimeHHMM = value; } }
        public string EmailRecepients { get { return this.emailRecepients; } set { this.emailRecepients = value; } }
        public string ReportHeadings { get { return this.reportHeadings; } set { this.reportHeadings = value; } }
        public string Culture { get { return this.culture; } set { this.culture = value; } }
        public string DefaultCulture { get { return this.defaultculture; } set { this.defaultculture = value; } }
        public string LocalisationPath { get { return this.localisationpath; } set { this.localisationpath = value; } }
        public string CalenderID { get { return this.calenderid; } set { this.calenderid = value; } }

        #endregion

        //Added as part of ROI conncetion string management
        //begin
        
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ReportSchedule()
        {
            culture = ConfigurationManager.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCReportDBNGCConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connectionString = Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]);
            }
        }
        //end

        #region Methods

        public bool Add(string objectXml, short sessionUserID, out long objectID, out string resultXml)
        {

            resultXml = string.Empty;
            objectID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.Add");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.Add - objectXml :" + objectXml.ToString());
                Hashtable htblReportSchedule = ConvertXmlHash.XMLToHashTable(objectXml, "ReportSchedule ");
                this.ReportID = Convert.ToInt16(htblReportSchedule[Constants.ReportID]);
                this.RecurrenceType = Convert.ToChar(htblReportSchedule[Constants.ReoccurenceType]);
                this.ScheduleHours = Convert.ToInt16(htblReportSchedule[Constants.ScheduleHours]);
                this.ScheduleMinutes = Convert.ToInt16(htblReportSchedule[Constants.ScheduleMinutes]);
                this.EmailAddresses = Convert.ToString(htblReportSchedule[Constants.EmailRecepients]);
                this.ReportParams = Convert.ToString(htblReportSchedule[Constants.ReportParams]);

                // Modified by Syed Amjadulla on 12th Mar'2010 to fetch data from Report DB              
                //string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                object[] objDBParams = {ReportID,
                                        sessionUserID,
                                        RecurrenceType,
                                        ScheduleHours,
                                        ScheduleMinutes,
                                        EmailAddresses,
                                        ReportParams};
                objectID = SqlHelper.ExecuteNonQuery(connectionString, Constants.AddReportSchedule, objDBParams);
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.Add");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.Add");


            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.Add - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.Add");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                resultXml = SqlHelper.resultXml;
                return false;
            }
            finally
            {
            }

            return SqlHelper.result.Flag;
        }



        #endregion


        public DataSet GetPromotionalReport(string startdate, string enddate, int week, string period)
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.GetPromotionalReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.GetPromotionalReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

                this.WeekNumber = week.ToString();
                this.PeriodNumber = period.ToString();
                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                if (startdate == null && enddate == null)
                {
                    object[] objRewardParams = { 
                                       
                                       DBNull.Value ,  DBNull.Value,int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETPROMOTIONALCODEREPORT, objRewardParams);
                    if (ds.Tables.Count > 0)
                        ds.Tables[0].TableName = "PromotionalDetails";
                }
                else
                {
                    object[] objRewardParams = { 
                                       
                                      Convert.ToDateTime(startdate,enGBCulture) , Convert.ToDateTime(enddate,enGBCulture),int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETPROMOTIONALCODEREPORT, objRewardParams);
                    if (ds.Tables.Count > 0)
                        ds.Tables[0].TableName = "PromotionalDetails";
                }

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.GetPromotionalReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.GetPromotionalReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.GetPromotionalReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.GetPromotionalReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.GetPromotionalReport");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;

        }
        public DataSet GetRegistrationReport(string startdate, string enddate, int week, string period)
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.GetRegistrationReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.GetRegistrationReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

                this.WeekNumber = week.ToString();
                this.PeriodNumber = period.ToString();
                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                if (startdate == null && enddate == null)
                {
                    object[] objRewardParams = { 
                                       
                                       DBNull.Value ,  DBNull.Value,int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETCLUBCARDREGISTRATIONREPORT, objRewardParams);
                    if (ds.Tables.Count > 0)
                        ds.Tables[0].TableName = "ClubcardRegistrationDetails";
                }
                else
                {
                    object[] objRewardParams = { 
                                       
                                      Convert.ToDateTime(startdate,enGBCulture) , Convert.ToDateTime(enddate,enGBCulture),int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETCLUBCARDREGISTRATIONREPORT, objRewardParams);
                    if (ds.Tables.Count > 0)
                        ds.Tables[0].TableName = "ClubcardRegistrationDetails";
                }

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.GetRegistrationReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.GetRegistrationReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.GetRegistrationReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.GetRegistrationReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.GetRegistrationReport");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;
            //return viewXml;
        }

        public string ScheduleReport(string ScheduleXml)
        {

            string Message = "";

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.ScheduleReport : ScheduleXml" + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.ScheduleReport : ScheduleXml" + ScheduleXml);
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                SqlCommand cmdObj = new SqlCommand();
                Hashtable htblReportSchedule = ConvertXmlHash.XMLToHashTable(ScheduleXml, "ScheduleReport");

                this.NGCReportFormatterDirectory = htblReportSchedule[Constants.NGCreportFormatterdirectory].ToString();
                this.Capabilityname = htblReportSchedule[Constants.capabilityName].ToString();
                this.UserName = htblReportSchedule[Constants.userName].ToString();
                this.RecurrenceType = Convert.ToChar(htblReportSchedule[Constants.ReoccurenceType1]);
                this.scheduleTimeHHMM = htblReportSchedule[Constants.scheduleTimeHHMM].ToString();
                this.EmailRecepients = htblReportSchedule[Constants.emailRecepients].ToString();
                this.ReportParams = htblReportSchedule[Constants.ReportParams1].ToString();
                this.ReportHeadings = htblReportSchedule[Constants.reportHeadings].ToString();
                this.Culture = htblReportSchedule[Constants.culture].ToString();
                this.DefaultCulture = htblReportSchedule[Constants.defaultculture].ToString();
                this.LocalisationPath = htblReportSchedule[Constants.LocalisationPath].ToString();

                cmdObj.CommandText = Constants.AddReportSchedule;
                cmdObj.Parameters.Add("@NGCReportFormatterDirectory", SqlDbType.Text, 255);
                cmdObj.Parameters.Add("@Capabilityname", SqlDbType.Text, 100);
                cmdObj.Parameters.Add("@UserName", SqlDbType.Text, 20);
                cmdObj.Parameters.Add("@RecurrenceType", SqlDbType.Text, 1);
                cmdObj.Parameters.Add("@ScheduleTimeHHMM", SqlDbType.Text, 5);
                cmdObj.Parameters.Add("@EmailRecepients", SqlDbType.Text, 550);
                cmdObj.Parameters.Add("@ReportParams", SqlDbType.Text, 2000);
                cmdObj.Parameters.Add("@ReportHeadings", SqlDbType.Text, 2000);
                cmdObj.Parameters.Add("@ReportCulture", SqlDbType.Text, 15);
                cmdObj.Parameters.Add("@DefaultCulture", SqlDbType.Text, 15);
                cmdObj.Parameters.Add("@LocalizationPath", SqlDbType.Text, 255);


                cmdObj.Parameters["@NGCReportFormatterDirectory"].Value = this.NGCReportFormatterDirectory;
                cmdObj.Parameters["@Capabilityname"].Value = this.Capabilityname;
                cmdObj.Parameters["@UserName"].Value = this.UserName;
                cmdObj.Parameters["@RecurrenceType"].Value = this.RecurrenceType;
                cmdObj.Parameters["@ScheduleTimeHHMM"].Value = this.scheduleTimeHHMM;
                cmdObj.Parameters["@EmailRecepients"].Value = this.EmailRecepients;
                cmdObj.Parameters["@ReportParams"].Value = this.ReportParams;
                cmdObj.Parameters["@ReportHeadings"].Value = this.ReportHeadings;
                cmdObj.Parameters["@ReportCulture"].Value = this.Culture;
                //cmdObj.Parameters["@DefaultCulture"].Value = Convert.ToString(ConfigurationSettings.AppSettings["CultureDefault"]); 
                cmdObj.Parameters["@DefaultCulture"].Value = this.DefaultCulture;


                cmdObj.Parameters["@LocalizationPath"].Value = this.LocalisationPath;


                cmdObj.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmdObj.Connection = connection;
                cmdObj.ExecuteNonQuery();

                Message = "Schedule Completed Successfully";
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.ScheduleReport : ScheduleXml" + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.ScheduleReport : ScheduleXml" + ScheduleXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.ScheduleReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.ScheduleReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.ScheduleReport");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return Message;
        }

        public string TerminateSchedule(string ScheduleXml)
        {

            string Message = "";

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.TerminateSchedule : ScheduleXml" + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.TerminateSchedule : ScheduleXml" + ScheduleXml);
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = connectionString;
                SqlCommand cmdObj = new SqlCommand();
                Hashtable htblReportSchedule = ConvertXmlHash.XMLToHashTable(ScheduleXml, "ScheduleReport");


                this.Capabilityname = htblReportSchedule[Constants.capabilityName].ToString();
                this.UserName = htblReportSchedule[Constants.userName].ToString();

                cmdObj.CommandText = Constants.DeleteReportSchedule;
                cmdObj.Parameters.Add("@Capabilityname", SqlDbType.Text, 100);
                cmdObj.Parameters.Add("@UserName", SqlDbType.Text, 20);

                cmdObj.Parameters["@Capabilityname"].Value = this.Capabilityname;
                cmdObj.Parameters["@UserName"].Value = this.UserName;

                cmdObj.CommandType = CommandType.StoredProcedure;
                connection.Open();
                cmdObj.Connection = connection;

                if (cmdObj.ExecuteNonQuery() > 0)
                {
                    Message = "Termination Completed Successfully";
                }
                else
                {
                    Message = "Termination Unsuccessfully";
                }


                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.TerminateSchedule : ScheduleXml" + ScheduleXml);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.TerminateSchedule : ScheduleXml" + ScheduleXml);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.TerminateSchedule - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.TerminateSchedule - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.TerminateSchedule");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }

            return Message;
        }


        public DataSet GetPointsEarnedReport(string startdate, string enddate, int calenderid, string period)
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:Start:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport StartDate-Enddate-Calendarid-Period-year - " + startdate + "-" + enddate + "-" + calenderid + "-" + periodnumber);
                NGCTrace.NGCTrace.TraceDebug("Start:Start:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport StartDate-Enddate-Calendarid-Period-year - " + startdate + "-" + enddate + "-" + calenderid + "-" + periodnumber);

                //due to command execution time out the commandtime out increased to 1800 sec.since the points earned report having bulk data in the table.
                this.CalenderID = calenderid.ToString();
                this.PeriodNumber = period.ToString();

                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand cmdObj = new SqlCommand();
                cmdObj.CommandType = CommandType.StoredProcedure;
                cmdObj.CommandTimeout = 1800;

                cmdObj.CommandText = "USP_GetEarnedPointsReport";
                cmdObj.Connection = connection;

                cmdObj.Parameters.Add("@StartDate", SqlDbType.DateTime, 30);
                cmdObj.Parameters.Add("@EndDate", SqlDbType.DateTime, 30);
                cmdObj.Parameters.Add("@TescoCalenderID", SqlDbType.Int, 20);
                cmdObj.Parameters.Add("@Period", SqlDbType.NVarChar, 20);

                if (startdate == null && enddate == null)
                {
                    cmdObj.Parameters["@StartDate"].Value = null;
                    cmdObj.Parameters["@EndDate"].Value = null;
                }
                else
                {
                    cmdObj.Parameters["@StartDate"].Value = DateTime.Parse(startdate, enGBCulture);
                    cmdObj.Parameters["@EndDate"].Value = DateTime.Parse(enddate, enGBCulture);
                }
                cmdObj.Parameters["@TescoCalenderID"].Value = this.CalenderID;
                cmdObj.Parameters["@Period"].Value = this.PeriodNumber;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmdObj;
                da.Fill(ds, "PointEarnedDetails");




                NGCTrace.NGCTrace.TraceInfo("End:Start:Start:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport StartDate-Enddate-Calendarid-Period-year - " + startdate + "-" + enddate + "-" + calenderid + "-" + periodnumber);
                NGCTrace.NGCTrace.TraceDebug("End:Start:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport StartDate-Enddate-Calendarid-Period-year - " + startdate + "-" + enddate + "-" + calenderid + "-" + periodnumber);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport: - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.GetPointsEarnedReport");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;
            //return viewXml;
        }



        public DataSet PopulateWeekData()
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.PopulateWeekData");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.PopulateWeekData");

                object[] objRewardParams = { 
                                       
                                       "Week"
                                     };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETDROPDOWNDATA, objRewardParams);
                ds.Tables[0].TableName = "ClubcardRegistrationDetails";

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.PopulateWeekData");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.PopulateWeekData");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.PopulateWeekData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.PopulateWeekData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.PopulateWeekData");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;
        }

        public DataSet PopulatePeriodData()
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.PopulatePeriodData");
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.PopulatePeriodData");


                object[] objRewardParams = { 
                                       
                                       "Period"
                                     };
                ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETDROPDOWNDATA, objRewardParams);
                ds.Tables[0].TableName = "ClubcardRegistrationDetails";

                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.PopulatePeriodData");
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.PopulatePeriodData");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.PopulatePeriodData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.PopulatePeriodData - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.PopulatePeriodData");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;
        }

        /// <summary>
        /// Author: Neeta Kewlani
        /// Details: Method is to get Customer Load details for Report.
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="week"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public DataSet GetCustomerLoadReport(string startdate, string enddate, int week, string period)
        {
            DataSet ds = new DataSet();


            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("Start:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                this.WeekNumber = week.ToString();
                this.PeriodNumber = period.ToString();
                System.Globalization.CultureInfo enGBCulture = new System.Globalization.CultureInfo("en-GB");
                if (startdate == null && enddate == null)
                {
                    object[] objRewardParams = { 
                                       
                                       DBNull.Value ,  DBNull.Value,int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETCUSTOMERLOADREPORT, objRewardParams);
                    ds.Tables[0].TableName = "CustomerLoadReport";
                }
                else
                {
                    object[] objRewardParams = { 
                                   
                                      DateTime.Parse(startdate,enGBCulture) , DateTime.Parse(enddate,enGBCulture),int.Parse(this.WeekNumber),this.PeriodNumber,null
                                     };

                    ds = SqlHelper.ExecuteDataset(connectionString, Constants.USP_GETCUSTOMERLOADREPORT, objRewardParams);
                    ds.Tables[0].TableName = "CustomerLoadReport";
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);
                NGCTrace.NGCTrace.TraceDebug("End:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport : StartDate-Enddate-Week-Period - " + startdate + "-" + enddate + "-" + week.ToString() + "-" + period);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoyaltyEntityService.ReportSchedule.GetCustomerLoadReport");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {

            }
            return ds;

        }
    }
}
