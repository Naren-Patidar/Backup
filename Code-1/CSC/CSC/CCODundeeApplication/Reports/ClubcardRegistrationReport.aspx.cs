#region Codes To Generate Clubcard Registration Report
//Author -Mohan Mahapatra
//Purpose of report : This report is generated to find out total customers joined through various Join routes in specified time frame
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCODundeeApplication.NGCReportingService;
using System.Data;
using System.IO;
using System.Text;
using System.Collections;
using System.ServiceModel;
using System.Xml;
using System.Web.UI.HtmlControls;
using System.Globalization;
#endregion

#region namespace
namespace CCODundeeApplication.Reports
{
    #region class
    public partial class ClubcardRegistrationReport : System.Web.UI.Page
    {
        # region Variable Declaration
        protected string errMsgDOB = string.Empty;
        protected string errMsgStartDate = string.Empty;
        protected string errMsgEndDate = string.Empty;
        protected string spanClassDOBDropDown0 = "dtFld";
        protected string spanStartDateError0 = "display:none";
        protected string spanEndDateError0 = "display:none";
        private Hashtable htInputCriterias = new Hashtable();
        private Hashtable htSchedule = new Hashtable();
        private Hashtable htHeaders = new Hashtable();
        private Int32 i = 0, j = 0;
        string error = "false";
        string errorMsg1 = "";
        DataSet dsCapability = null;
        XmlDocument xmlCapability = null;
        # endregion

        #region Page_load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            { 
            #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC - Clubcard Registration Report - Page_Load");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC - Clubcard Registration Report - Page_Load");
            #endregion
                if (!IsPostBack)
                {
                    Label lblCustomerDtl = (Label)Master.FindControl("lblCustomerDtl");
                    lblCustomerDtl.Visible = false;
                    Label lblCustomePref = (Label)Master.FindControl("lblCustomePref");
                    lblCustomePref.Visible = false;
                    Label lblCustomerPts = (Label)Master.FindControl("lblCustomerPts");
                    lblCustomerPts.Visible = false;
                    Label lblCustomerCards = (Label)Master.FindControl("lblCustomerCards");
                    lblCustomerCards.Visible = false;
                    Label lblXmasSaver = (Label)Master.FindControl("lblXmasSaver");
                    lblXmasSaver.Visible = false;
                    Label lblViewPoints = (Label)Master.FindControl("lblViewPoints");
                    lblViewPoints.Visible = false;
                    Label lblresetpass = (Label)Master.FindControl("lblresetpass");
                    lblresetpass.Visible = false;
                    Label lblDelinking = (Label)Master.FindControl("lblDelinking");
                    lblDelinking.Visible = false;
                    Label lblCustomerCoupons = (Label)Master.FindControl("lblCustomerCoupons");
                    lblCustomerCoupons.Visible = false;
                    Label lblDataConfiguration = (Label)Master.FindControl("lblDataConfiguration");
                    lblDataConfiguration.Visible = false;
                    Label lblLastUpdatedBy = (Label)Master.FindControl("lblLastUpdatedBy");
                    Label lblLastUpdatedDate = (Label)Master.FindControl("lblLastUpdatedDate");

                    #region RoleCapabilityImplementation
                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            HtmlAnchor Join = (HtmlAnchor)Master.FindControl("Join");
                            HtmlAnchor FindUser = (HtmlAnchor)Master.FindControl("FindUser");
                            HtmlAnchor AddUser = (HtmlAnchor)Master.FindControl("AddUser");
                            HtmlAnchor FindGroup = (HtmlAnchor)Master.FindControl("FindGroup");
                            HtmlAnchor AddGroup = (HtmlAnchor)Master.FindControl("AddGroup");
                            HtmlAnchor CardRange = (HtmlAnchor)Master.FindControl("CardRange");
                            HtmlAnchor CardTypes = (HtmlAnchor)Master.FindControl("CardType");
                            HtmlAnchor Stores = (HtmlAnchor)Master.FindControl("Stores");
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            HtmlAnchor AccountOperationReports = (HtmlAnchor)Master.FindControl("AccReports");
                            HtmlAnchor PromotionalCodeReport = (HtmlAnchor)Master.FindControl("PromotionalCode");
                            Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                            HtmlAnchor PointsearnedReport = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                            if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                            {
                                PointsearnedReport.Disabled = false;
                            }
                            else
                            {
                                PointsearnedReport.Disabled = true;
                                PointsearnedReport.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("AccountOperationReports") != false)
                            {
                                AccountOperationReports.Disabled = false;
                            }
                            else
                            {
                                AccountOperationReports.Disabled = true;
                                AccountOperationReports.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("PromotionalCodeReport") != false)
                            {
                                PromotionalCodeReport.Disabled = false;
                            }
                            else
                            {
                                PromotionalCodeReport.Disabled = true;
                                PromotionalCodeReport.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("CreateNewCustomer") != false)
                            {
                                Join.Disabled = false;
                            }
                            else
                            {
                                Join.Disabled = true;
                                Join.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("AddUser") != false)
                            {
                                AddUser.Disabled = false;
                            }
                            else
                            {
                                AddUser.Disabled = true;
                                AddUser.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("AddGroup") != false)
                            {
                                AddGroup.Disabled = false;
                            }
                            else
                            {
                                AddGroup.Disabled = true;
                                AddGroup.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("FindGroup") != false)
                            {
                                FindGroup.Disabled = false;
                            }
                            else
                            {
                                FindGroup.Disabled = true;
                                FindGroup.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("finduser") != false)
                            {
                                FindUser.Disabled = false;
                            }
                            else
                            {
                                FindUser.Disabled = true;
                                FindUser.HRef = "";
                            }

                            if (dsCapability.Tables[0].Columns.Contains("editcardranges") != false)
                            {
                                CardRange.Disabled = false;
                            }
                            else
                            {
                                CardRange.Disabled = true;
                                CardRange.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("editcardtypes") != false)
                            {
                                CardTypes.Disabled = false;
                            }
                            else
                            {
                                CardTypes.Disabled = true;
                                CardTypes.HRef = "";
                            }
                            if (dsCapability.Tables[0].Columns.Contains("editstores") != false)
                            {
                                Stores.Disabled = false;
                            }
                            else
                            {
                                Stores.Disabled = true;
                                Stores.HRef = "";
                            }


                        }
                    }
                    #endregion

                    Helper.GetMonthDdl(ddlStartMonth); //Load Month dropdown
                    Helper.GetYearDdlReport(ddlStartYear); //Load Year Dropdown
                    Helper.GetMonthDdl(ddlEndMonth); //Load Month dropdown
                    Helper.GetYearDdlReport(ddlEndYear); //Load Year Dropdown

                    if (ddlWeek != null)
                    {
                        DataTable dtWeek = Helper.PopulateWeekData();
                        ddlWeek.Items.Add(new ListItem("Please Select", "-1"));
                        foreach (DataRow row in dtWeek.Rows)
                        {
                            if (!ddlWeek.Items.Contains(new ListItem(row[1].ToString(), row[0].ToString())))
                            {
                                ddlWeek.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                            }
                        }

                    }
                    if (ddlPeriod != null)
                    {
                        DataTable dtPeriod = Helper.PopulatePeriodData();
                        ddlPeriod.Items.Add(new ListItem("Please Select", "-1"));
                        foreach (DataRow row in dtPeriod.Rows)
                        {
                            if (!ddlPeriod.Items.Contains(new ListItem(row[1].ToString(), row[0].ToString())))
                            {
                                ddlPeriod.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                            }
                        }
                    }

                    this.ddlEndDay.Enabled = true;
                    this.ddlEndMonth.Enabled = true;
                    this.ddlEndYear.Enabled = true;
                    this.ddlStartYear.Enabled = true;
                    this.ddlStartMonth.Enabled = true;
                    this.ddlStartDay.Enabled = true;
                    this.ddlWeek.Enabled = false;
                    this.ddlPeriod.Enabled = false;


                    ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");
                    leftNav.Visible = false;
                    // btnRunRpt.Attributes.Add("onclick", "return ValidateDateRegReport('" + ddlStartDay.ClientID + "','" + ddlEndDay.ClientID + "','" + ddlStartMonth.ClientID + "','" + ddlEndMonth.ClientID + "','" + ddlStartYear.ClientID + "','" + ddlEndYear.ClientID + "','" + divStartDate.ClientID + "','" + divEndDate.ClientID + "','" + divComparision.ClientID + "','" + ddlWeek.ClientID + "','" + ddlPeriod.ClientID + "')");
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - Page_Load");
                NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - Page_Load");
                #endregion
            }

            catch (Exception ex)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC - Clubcard Registration Report - Page_Load- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC - Clubcard Registration Report - Page_Load- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC - Clubcard Registration Report - Page_Load :");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
            }
            finally
            {
                
            }
        }
        # endregion

        #region Initialize the culture


        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture"));
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion

        #region btnRunRpt_Click
        protected void btnRunRpt_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        #endregion

        #region btnScheduleRpt_Click
        protected void btnScheduleRpt_Click(object sender, EventArgs e)
        {
            ScheduleReport();

        }
        #endregion

        #region ScheduleReport()
        private bool ScheduleReport()
        {
            NGCReportingServiceClient objReportingService = new NGCReportingServiceClient();
            CultureInfo currentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC - Clubcard Registration Report - ScheduleRpt");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC - Clubcard Registration Report - ScheduleRpt");
                #endregion
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                string stDate = null;
                string edDate = null;

                int week = 0;
                string period = "";
                string timeFrame = GetLocalResourceObject("lclTimeFrameResource1.Text").ToString();
                string headerXml = "";
                if (rdoDate.Checked && rdoDate.Visible)
                {
                    //string startDay = ddlStartDay.SelectedValue.ToString();
                    //string startMonth = ddlStartMonth.SelectedValue.ToString();
                    //string startYear = ddlStartYear.SelectedValue.ToString();

                    //string endDay = ddlEndDay.SelectedValue.ToString();
                    //string endMonth = ddlEndMonth.SelectedValue.ToString();
                    //string endYear = ddlEndYear.SelectedValue.ToString();

                    //startDate = Convert.ToDateTime( startDay+ "/" + startMonth + "/" + startYear);
                    //endDate = Convert.ToDateTime( startDay +"/" + endMonth + "/" + endYear);


                    //stDate = startDate.ToString("dd/MM/yyyy");
                    //edDate = endDate.ToString("dd/MM/yyyy");
                    //Added by Laxmi for fixing the date issue based on culture defect no:MKTG00008357 on 26/02/2013
                    int startDay = Convert.ToInt16(ddlStartDay.SelectedValue.ToString());
                    int startMonth = Convert.ToInt16(ddlStartMonth.SelectedValue.ToString());
                    int startYear = Convert.ToInt16(ddlStartYear.SelectedValue.ToString());
                    int endDay = Convert.ToInt16(ddlEndDay.SelectedValue.ToString());
                    int endMonth = Convert.ToInt16(ddlEndMonth.SelectedValue.ToString());
                    int endYear = Convert.ToInt16(ddlEndYear.SelectedValue.ToString());
                    startDate = new DateTime(startYear, startMonth, startDay);
                    endDate = new DateTime(endYear, endMonth, endDay);
                    stDate = startDate.ToString("d", currentCulture);
                    edDate = endDate.ToString("d", currentCulture);
                    headerXml = "<InputData><Input1>" + stDate + " - " + edDate + "</Input1><Input0>Time frame</Input0></InputData>";


                }
                if (rdoWeekNo.Checked)
                {
                    week = int.Parse(ddlWeek.SelectedItem.Value.ToString());
                    headerXml = "<InputData><Input1>" + ddlWeek.SelectedItem.Text.ToString() + "</Input1><Input0>Time frame</Input0></InputData>";

                }

                if (rdoPeriod.Checked)
                {
                    period = this.ddlPeriod.SelectedItem.Value.ToString();
                    headerXml = "<InputData><Input1>" + ddlPeriod.SelectedItem.Text.ToString() + "</Input1><Input0>Time frame</Input0></InputData>";

                }

                //DataSet ds = new DataSet();
                //ds = objReportingService.GetRegistrationReport(stDate, edDate, week, period.ToString());
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                    string inputXml = "<ReportParams><Input0>StartDate</Input0><Input1>" + stDate + "</Input1><Input2>EndDate</Input2><Input3>" + edDate + "</Input3><Input4>Week</Input4><Input5>" + week + "</Input5><Input6>Period</Input6><Input7>" + period + "</Input7></ReportParams>";
                    Helper.SetTripleDESEncryptedCookie("UserInput", inputXml);
                    Helper.SetTripleDESEncryptedCookie("ReportHeader", headerXml);
                    string reportName = "ClubcardRegistrationReport";
                    Helper.SetTripleDESEncryptedCookie("ReportName", reportName);
                    string script = "window.showModalDialog('SchedulePopup.aspx',null,'dialogLeft:400px; dialogTop:150px; dialogHeight:360px; dialogWidth:510px; status:no; resizable:no; scroll:no');";
                    string popup = "<script> " + script + " </script>";
                    Page.RegisterStartupScript("key", popup);
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - ScheduleRpt");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - ScheduleRpt");
                    #endregion
                    return true;
                //}

                //else
                //{
                //    string message = "No Records Found.";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + message + "');", true);
                //    #region Trace End
                //    NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - ScheduleRpt");
                //    NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - ScheduleRpt");
                //    #endregion
                //    return false;
                //}
                
            }
            catch (Exception ex)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC - Clubcard Registration Report - ScheduleRpt- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC - Clubcard Registration Report - ScheduleRpt- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC - Clubcard Registration Report - ScheduleRpt :" + ex.ToString());
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
                return false;
            }
            finally
            {
                if (objReportingService != null)
                {
                    if (objReportingService.State == CommunicationState.Faulted)
                    {
                        objReportingService.Abort();
                    }
                    else if (objReportingService.State != CommunicationState.Closed)
                    {
                        objReportingService.Close();
                    }
                }
            }
        }
        #endregion

        #region GenerateReport()
        private bool GenerateReport()
        {
            NGCReportingServiceClient objReportingService = new NGCReportingServiceClient();
            try
            { 
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC - Clubcard Registration Report - GenerateReport()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC - Clubcard Registration Report - GenerateReport()");
                #endregion

                
                string timeFrame = GetLocalResourceObject("lclTimeFrameResource1.Text").ToString();
                int week = 0;
                string period = "";
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                string stDate = null;
                string edDate = null;
                CultureInfo currentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
                if (rdoDate.Checked && rdoDate.Visible)
                {
                    //string startDay = ddlStartDay.SelectedValue.ToString();
                    //string startMonth = ddlStartMonth.SelectedValue.ToString();
                    //string startYear = ddlStartYear.SelectedValue.ToString();

                    //string endDay = ddlEndDay.SelectedValue.ToString();
                    //string endMonth = ddlEndMonth.SelectedValue.ToString();
                    //string endYear = ddlEndYear.SelectedValue.ToString();

                    ////startDate = Convert.ToDateTime(startMonth + "/" + startDay + "/" + startYear);
                    ////endDate = Convert.ToDateTime(endMonth + "/" + endDay + "/" + endYear);

                    //startDate = Convert.ToDateTime(startDay + "/" + startMonth + "/" + startYear);
                    //endDate = Convert.ToDateTime(endDay + "/" + endMonth + "/" + endYear);

                    //stDate = startDate.ToString("dd/MM/yyyy");
                    //edDate = endDate.ToString("dd/MM/yyyy");

                    //Added by Laxmi for fixing the date issue based on culture defect no:MKTG00008357 on 26/02/2013
                    int startDay = Convert.ToInt16(ddlStartDay.SelectedValue.ToString());
                    int startMonth = Convert.ToInt16(ddlStartMonth.SelectedValue.ToString());
                    int startYear = Convert.ToInt16(ddlStartYear.SelectedValue.ToString());
                    int endDay = Convert.ToInt16(ddlEndDay.SelectedValue.ToString());
                    int endMonth = Convert.ToInt16(ddlEndMonth.SelectedValue.ToString());
                    int endYear = Convert.ToInt16(ddlEndYear.SelectedValue.ToString());
                    startDate = new DateTime(startYear, startMonth, startDay);
                    endDate = new DateTime(endYear, endMonth, endDay);
                    stDate = startDate.ToString("d", currentCulture);
                    edDate = endDate.ToString("d", currentCulture);

                    htInputCriterias.Add("Input" + i, timeFrame);
                    i = i + 1;
                    htInputCriterias.Add("Input" + i, startDate.ToString("d", currentCulture) + " - " + endDate.ToString("d", currentCulture));
                    i = i + 1;

                    htSchedule.Add("Input" + j, "StartDate");
                    j = j + 1;
                    htSchedule.Add("Input" + j, stDate);
                    j = j + 1;
                    htSchedule.Add("Input" + j, "EndDate");
                    j = j + 1;
                    htSchedule.Add("Input" + j, edDate);
                    j = j + 1;


                }
                else if (rdoWeekNo.Checked && rdoWeekNo.Visible)
                {
                    week = int.Parse(ddlWeek.SelectedItem.Value.ToString());
                    htInputCriterias.Add("Input" + i, timeFrame);
                    i = i + 1;
                    htInputCriterias.Add("Input" + i, ddlWeek.SelectedItem.Text.ToString());
                    i = i + 1;

                    htSchedule.Add("Input" + j, "Week");
                    j = j + 1;
                    htSchedule.Add("Input" + j, ddlWeek.SelectedItem.Value.ToString());
                    j = j + 1;


                }
                else if (rdoPeriod.Checked && rdoPeriod.Visible)
                {
                    period = this.ddlPeriod.SelectedItem.Value.ToString();

                    htInputCriterias.Add("Input" + i, timeFrame);
                    i = i + 1;
                    htInputCriterias.Add("Input" + i, ddlPeriod.SelectedItem.Text.ToString());
                    i = i + 1;



                    htSchedule.Add("Input" + j, "Period");
                    j = j + 1;

                    htSchedule.Add("Input" + j, period.ToString());
                    j = j + 1;

                }
                DataSet ds = new DataSet();

                ds = objReportingService.GetRegistrationReport(stDate, edDate, week, period.ToString());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string headerXml = Helper.HashTableToXML(htInputCriterias, "InputData");
                    string reportName = GetLocalResourceObject("CSC.ClubcardRegistrationReport.Report.Name").ToString();
                    string reportRunDate = GetLocalResourceObject("CSC.ClubcardRegistrationReport.ColumnName.ReportRunDate").ToString();
                    Hashtable htLocalisedColNames = new Hashtable();
                    for (int m = 0; m < ds.Tables[0].Columns.Count; m++)
                    {
                        string colHeader = "";
                        string localizedHeader = "";
                        colHeader = ds.Tables[0].Columns[m].ColumnName;
                        if (m == 0)
                            localizedHeader = GetLocalResourceObject("CSC.ClubcardRegistrationReport.ColumnName." + colHeader).ToString();
                        else
                            localizedHeader = colHeader;
                        htLocalisedColNames.Add(colHeader, localizedHeader);
                    }

                    string userName = "";//this.Session[WebSessionIndexes.UserId()].ToString();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
                    {
                        userName = Helper.GetTripleDESEncryptedCookieValue("UserID");
                    }
                    else
                    {
                        userName = "";
                    }
                    DateTime Date = DateTime.Now;
                    string CurrentDate = Date.ToString("dd_MM_yyyy");
                    //string folder = ConfigurationSettings.AppSettings["NGCDownloadReportsFolder"];
                    //string sFile = folder + "MyExcel" + userName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
                    Response.Clear();
                    Response.ContentType = "application/ms-excel";
                    string sFile = ExtracttoExcel.ExportExcel(ds.GetXml(), reportName, reportRunDate, htLocalisedColNames, headerXml, "ClubcardRegistrationReport");
                    //Get the Cardtype column into the xml document
                    string CardTypeDesc = "";
                    if (htInputCriterias["ClubcardType"] != null)
                        CardTypeDesc = htInputCriterias["ClubcardType"].ToString();
                    string csvFileName = "UK" + "." + reportName + CardTypeDesc + "." + CurrentDate;
                    Response.AddHeader("Content-Disposition", "Attachment; filename=" + csvFileName + ".xlsx");

                    Response.ContentType = "application/vnd.ms-excel";
                    Response.WriteFile(sFile);
                    Response.Flush();
                    if (File.Exists(sFile))
                    {
                        File.Delete(sFile);
                    }
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - GenerateReport()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - GenerateReport()");
                    #endregion
                    return true;

                }
                else
                {
                    string message = GetLocalResourceObject("errMessage").ToString();//"No Records Found.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "alert('" + message + "');", true);
                    #region Trace End
                    NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - GenerateReport()");
                    NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - GenerateReport()");
                    #endregion
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC - Clubcard Registration Report - GenerateReport()- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC - Clubcard Registration Report - GenerateReport()- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC - Clubcard Registration Report - GenerateReport() :" + ex.ToString());
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
                return false;
            }
            finally
            {
                if (objReportingService != null)
                {
                    if (objReportingService.State == CommunicationState.Faulted)
                    {
                        objReportingService.Abort();
                    }
                    else if (objReportingService.State != CommunicationState.Closed)
                    {
                        objReportingService.Close();
                    }
                }
               
            }
            
        }
        #endregion

        #region TimeFrame_SelectedIndexChanged
        protected void TimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()");
                #endregion
                this.ddlEndDay.Enabled = false;
                this.ddlEndMonth.Enabled = false;
                this.ddlEndYear.Enabled = false;
                this.ddlStartYear.Enabled = false;
                this.ddlStartMonth.Enabled = false;
                this.ddlStartDay.Enabled = false;
                this.ddlWeek.Enabled = false;
                this.ddlPeriod.Enabled = false;


                if (this.rdoDate.Checked)
                {
                    this.ddlEndDay.Enabled = true;
                    this.ddlEndMonth.Enabled = true;
                    this.ddlEndYear.Enabled = true;
                    this.ddlStartYear.Enabled = true;
                    this.ddlStartMonth.Enabled = true;
                    this.ddlStartDay.Enabled = true;
                    this.ddlWeek.Enabled = false;
                    this.ddlPeriod.Enabled = false;
                    this.ddlPeriod.SelectedIndex = 0;
                    this.ddlWeek.SelectedIndex = 0;
                }
                else if (this.rdoWeekNo.Checked)
                {
                    this.ddlWeek.Enabled = true;
                    this.ddlStartDay.SelectedIndex = 0;
                    this.ddlStartMonth.SelectedIndex = 0;
                    this.ddlStartYear.SelectedIndex = 0;
                    this.ddlEndDay.SelectedIndex = 0;
                    this.ddlEndMonth.SelectedIndex = 0;
                    this.ddlEndYear.SelectedIndex = 0;
                    this.ddlPeriod.SelectedIndex = 0;
                }
                else if (this.rdoPeriod.Checked)
                {
                    this.ddlPeriod.Enabled = true;
                    this.ddlStartDay.SelectedIndex = 0;
                    this.ddlStartMonth.SelectedIndex = 0;
                    this.ddlStartYear.SelectedIndex = 0;
                    this.ddlEndDay.SelectedIndex = 0;
                    this.ddlEndMonth.SelectedIndex = 0;
                    this.ddlEndYear.SelectedIndex = 0;
                    this.ddlWeek.SelectedIndex = 0;
                }
                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()");
                #endregion
            }
            catch (Exception ex)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged()- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC - Clubcard Registration Report - TimeFrame_SelectedIndexChanged() :" + ex.ToString());
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                #endregion Trace Error
            }
            finally
            {
            
            
            }

        }
        #endregion
    }
    #endregion
}
#endregion

#endregion