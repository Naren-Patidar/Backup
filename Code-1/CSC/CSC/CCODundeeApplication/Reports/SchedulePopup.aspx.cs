using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Collections.Generic;
using CCODundeeApplication.NGCReportingService;
using System.ServiceModel;
namespace CCODundeeApplication.Reports
{

    public partial class SchedulePopup : System.Web.UI.Page
    {
        #region Declaration
        SqlCommand cmdObj = new SqlCommand();
        NGCReportingServiceClient objReportingService = null;
        // Modified by Syed Amjadulla on 5th Feb'2010 to fetch data from Report DB
        // SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));

        //SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["ReportDBNGCConnectionString"]));

        #endregion

        #region Properties
        private string UserID;

        #endregion

        private string errorMsg = string.Empty, alert = string.Empty, cardType = string.Empty, activeLastWeek = string.Empty;
        private string sHour = string.Empty, sMinute = string.Empty;
        private Hashtable htValues = new Hashtable();
        private int i = 0;
        private bool success = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.SessionId = this.Session[WebSessionIndexes.SessionId()].ToString();
        }
        #region Initialize the culture

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
        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            string RecurrenceType;

            if (RdbDaily.Checked == true)
            {
                RecurrenceType = "D";
            }
            else if (RdbWeekly.Checked == true)
            {
                RecurrenceType = "W";
            }
            else
                RecurrenceType = "P";

            if (TextBox1.Text != "")
            {
                int res;
                if (!(int.TryParse(TextBox1.Text.ToString(), out res)))
                {
                    errorMsg = "Please provide valid Schedule time";
                    //errorMsg = Localization.GetLocalizedAttributeString("Time.Hour.NumericError");

                    alert = "<script>alert(' " + errorMsg + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                    return;
                }
                if (Convert.ToInt32(TextBox1.Text) > 23 || Convert.ToInt32(TextBox1.Text) < 0)
                {
                    errorMsg = "Please provide valid Schedule time";
                    //errorMsg = Localization.GetLocalizedAttributeString("Time.Hour.Error");
                    alert = "<script>alert(' " + errorMsg + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                    return;
                }
            }
            else
            {
                errorMsg = "Please provide valid Schedule time";
                //errorMsg = Localization.GetLocalizedAttributeString("Time.Hour.Empty");
                alert = "<script>alert(' " + errorMsg + " ');</script>";
                Page.RegisterStartupScript("key", alert);
                return;
            }
            if (TextBox2.Text != "")
            {
                int res;
                if (!(int.TryParse(TextBox2.Text.ToString(), out res)))
                {
                    errorMsg = "Please provide valid Schedule time";
                    //errorMsg = Localization.GetLocalizedAttributeString("Time.Minute.NumericError");
                    alert = "<script>alert(' " + errorMsg + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                    return;
                }
                if (Convert.ToInt32(TextBox2.Text) > 59 || Convert.ToInt32(TextBox2.Text) < 0)
                {
                    errorMsg = "Please provide valid Schedule time";
                    //errorMsg = Localization.GetLocalizedAttributeString("Time.Minute.Error");
                    alert = "<script>alert(' " + errorMsg + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                    return;
                }
            }
            else
            {
                errorMsg = "Please provide valid Schedule time";
                //errorMsg = Localization.GetLocalizedAttributeString("Time.Minute.Empty");
                alert = "<script>alert(' " + errorMsg + " ');</script>";
                Page.RegisterStartupScript("key", alert);
                return;
            }
            sHour = TextBox1.Text.ToString();
            sMinute = TextBox2.Text.ToString();

            if (TextBox1.Text.ToString().Length == 1)
            {
                sHour = "0" + TextBox1.Text.ToString();
            }
            if (TextBox2.Text.ToString().Length == 1)
            {
                sMinute = "0" + TextBox2.Text.ToString();
            }
            string ScheduleTimeHHMM = sHour + ":" + sMinute;
            if (TxtBxEmail.Text == "")
            {
                errorMsg = "Please provide valid Email Address";
                //errorMsg = Localization.GetLocalizedAttributeString("Email.Address.Empty");
                alert = "<script>alert(' " + errorMsg + " ');</script>";
                Page.RegisterStartupScript("key", alert);
                return;
            }
            string EmailRecepients = TxtBxEmail.Text.ToString();
            string[] subDir = EmailRecepients.Split(';');
            if (subDir.Length > 10)
            {
                errorMsg = "Please provide valid Email Address";
                //errorMsg = Localization.GetLocalizedAttributeString("Email.Length.ExceedsLimit");
                alert = "<script>alert(' " + errorMsg + " ');</script>";
                Page.RegisterStartupScript("key", alert);
                return;
            }
            else
            {
                foreach (string emailid in subDir)
                {
                    //Regular expression that will use a pattern match to validate an e-mail address
                    //bool returnvalue=validationEmail(emailid);
                    Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
                    Match emailMatch = emailRegex.Match(emailid);

                    //checks if the mail id is in valid format and returns if not
                    if (!emailMatch.Success)
                    {
                        //errorMsg = Localization.GetLocalizedAttributeString("Email.Address.Error");
                        errorMsg = "Please provide valid Email Address";
                        alert = "<script>alert(' " + errorMsg + " ');</script>";
                        Page.RegisterStartupScript("key", alert);
                        return;
                    }
                }
                htValues.Add("EmailRecepients ", EmailRecepients);
            }

            //this.Session["InputValues"] = Helper.HashTableToXML(htValues, "Values");
            string ReportParams;
            string ReportHeadings;
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserInput")))
            {
                ReportParams = Helper.GetTripleDESEncryptedCookieValue("UserInput");
            }
            else
            {
                ReportParams = "";
            }
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportHeader")))
            {
                ReportHeadings = Helper.GetTripleDESEncryptedCookieValue("ReportHeader");
            }
            else
            {
                ReportHeadings = "";
            }

            string reportCulture = "en-GB";
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                Culture = Helper.GetTripleDESEncryptedCookieValue("Culture");
                if (Culture == "Thai (Thailand)")
                    reportCulture = "th-TH";
                else
                    reportCulture = "en-GB";
            }
            else
            {
                Culture = "";
            }
            string UserName = "";
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
            {
                UserName = Helper.GetTripleDESEncryptedCookieValue("UserID");
            }
            else
            {
                UserName = "";
            }


            string Capabilityname = "";

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportName")))
            {
                Capabilityname = Helper.GetTripleDESEncryptedCookieValue("ReportName");
            }
            else
            {
                Capabilityname = "";
            }


            string NGCReportFormatterDirectory = ConfigurationSettings.AppSettings["NGCReportFormatterDirectory"];
            try
            {
                objReportingService = new NGCReportingServiceClient();
                Hashtable htSchedule = new Hashtable();
                htSchedule["NGCReportFormatterDirectory"] = NGCReportFormatterDirectory;
                htSchedule["Capabilityname"] = Capabilityname;
                htSchedule["UserName"] = UserName;
                htSchedule["RecurrenceType"] = RecurrenceType;
                htSchedule["ScheduleTimeHHMM"] = ScheduleTimeHHMM;
                htSchedule["EmailRecepients"] = EmailRecepients;
                htSchedule["ReportParams"] = ReportParams;
                htSchedule["ReportHeadings"] = ReportHeadings;
                htSchedule["Culture"] = reportCulture;
                htSchedule["DefaultCulture"] = ConfigurationSettings.AppSettings["CultureDefault"];
                htSchedule["LocalisationPath"] = ConfigurationSettings.AppSettings["LocalisationPath"];

                string scheduleXml = Helper.HashTableToXML(htSchedule, "ScheduleReport");
                string Message = objReportingService.ScheduleReport(scheduleXml);



                //alert = "<script>alert(' " + Message + " ');</script>";

                alert = "<script> if (confirm(' " + Message + " ')){window.close()}</script>";



                Page.RegisterStartupScript("key", alert);



                //if (Message == "Schedule Complete")
                //{
                //    CloseWindow(); 
                //}
            }
            catch (SqlException ex)
            {
                //Temporary solution provided by Syed Amjadulla on 25th Jan'2010 for re-scheduling of Report 
                if (ex.ErrorCode == -2146232060)
                {
                    // Error : Cannot add, update, or delete a job (or its steps or schedules) that originated from an MSX server.
                    cmdObj.ExecuteNonQuery();
                    string Message = Localization.GetLocalizedAttributeString("NGCMarketing.SchedulePopup.Complete");
                    alert = "<script>alert(' " + Message + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                }
                else
                {
                    alert = "<script>alert(' " + ex.Message.ToString() + " ');</script>";
                    Page.RegisterStartupScript("key", alert);
                }
                return;
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

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            CloseWindow();
        }
        public void CloseWindow()
        {
            string script = "window.close()";
            string popup = "<script> " + script + " </script>";
            Page.RegisterStartupScript("key", popup);
        }

        protected void btnTerminateSchedule_Click(object sender, System.EventArgs e)
        {

            string UserName = "";
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserID")))
            {
                UserName = Helper.GetTripleDESEncryptedCookieValue("UserID");
            }
            else
            {
                UserName = "";
            }
            string Capabilityname = "";

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("ReportName")))
            {
                Capabilityname = Helper.GetTripleDESEncryptedCookieValue("ReportName");
            }
            else
            {
                Capabilityname = "";
            }
            try
            {
                objReportingService = new NGCReportingServiceClient();
                Hashtable htSchedule = new Hashtable();
                htSchedule["Capabilityname"] = Capabilityname;
                htSchedule["UserName"] = UserName;


                string scheduleXml = Helper.HashTableToXML(htSchedule, "ScheduleReport");
                string Message = objReportingService.TerminateSchedule(scheduleXml);


                alert = "<script> if (confirm(' " + Message + " ')){window.close()}</script>";

                Page.RegisterStartupScript("key", alert);
                RdbDaily.Checked = true;
                TextBox1.Text = "";
                TextBox2.Text = "";
                TxtBxEmail.Text = "";
            }
            catch
            {

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
    }
}
