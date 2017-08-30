using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Tesco.NGC.DataAccessLayer;
using Tesco.com.ClubcardOnline.Entities;
using Tesco.com.ClubcardOnlineService;
using System.Net.Mail;
using System.IO;
using System.Xml;
using Tesco.NGC.Utils;
using System.Collections;

namespace Tesco.com.ClubcardOnline.CCOPreferences
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public partial class PreferenceService : IPreferenceService
    {
        string connectionString = string.Empty;

        #region Config Details


        //Added as part of ROI conncetion string management
        //begin
        private string culture = "";

        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public PreferenceService()
        {
            culture = ConfigurationSettings.AppSettings["Culture"].ToString();
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
        #endregion

        #region My Preferences

        /// <summary>
        /// To update the customer preference table for logged in customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="customerPreference"></param>
        public void MaintainCustomerPreference(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.maintainCustomerPreference - CustomerID:" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.maintainCustomerPreference - CustomerID:" + customerID.ToString());

                string sCulture = string.Empty;
                short iUserID = 0;
                DataTable dtPreference = new DataTable("PreferenceDataType");
                dtPreference.Columns.Add("PreferenceID", typeof(Int16));
                dtPreference.Columns.Add("updateDateTime", typeof(DateTime));
                dtPreference.Columns.Add("OptStatusID", typeof(Int16));

                for (int i = 0; i < customerPreference.Preference.Count; i++)
                {
                    dtPreference.Rows.Add(
                      customerPreference.Preference[i].PreferenceID,
                      customerPreference.Preference[i].UpdateDateTime, (Int16)customerPreference.Preference[i].POptStatus);
                }

                sCulture = customerPreference.Culture;
                iUserID = Helper.GetConsumerID(customerPreference.UserID);

                SqlParameter[] paramsToPreference = new SqlParameter[4];

                paramsToPreference[0] = new SqlParameter("@pCustomerID", SqlDbType.BigInt);
                paramsToPreference[0].Value = customerID;
                paramsToPreference[1] = new SqlParameter("@pUserID", SqlDbType.BigInt);
                paramsToPreference[1].Value = iUserID;
                paramsToPreference[2] = new SqlParameter("@pCulture", System.Data.SqlDbType.NVarChar);
                paramsToPreference[2].Value = sCulture;
                paramsToPreference[3] = new SqlParameter("@pPreferences", System.Data.SqlDbType.Structured);
                paramsToPreference[3].Value = dtPreference;

                SqlHelper.ExecuteNonQuery(connectionString,
                     CommandType.StoredProcedure, "USP_MaintainCustomerPreference", paramsToPreference);

                if (!string.IsNullOrEmpty(customerDetails.EmailId.ToString().Trim()))
                {
                    SendEmailToCustomers(customerPreference, customerDetails);
                }

                NGCTrace.NGCTrace.TraceInfo("End:PreferenceServices.Preference.maintainCustomerPreference - CustomerID:" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:PreferenceServices.Preference.maintainCustomerPreference - CustomerID:" + customerID.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.maintainCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.maintainCustomerPreference - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.maintainCustomerPreference");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
        }

        /// <summary>
        /// To get all the Enabled preference along with opted in preference
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="PreferenceType"></param>
        /// <param name="optionalPreference"></param>
        /// <returns>CustomerPreference Object</returns>
        public CustomerPreference ViewCustomerPreference(Int64 customerID, PreferenceType PreferenceType, bool optionalPreference)
        {
            CustomerPreference response = new CustomerPreference();

            DataSet ds = new DataSet();
            string sOptionalPreference = "N";
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.ViewClubDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.ViewClubDetails - customerID :" + customerID.ToString());

                if (optionalPreference)
                {
                    sOptionalPreference = "Y";
                }
                object[] objDBParams = { customerID, (short)PreferenceType, sOptionalPreference };
                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ViewCustomerPreference", objDBParams);

                ds.Tables[0].TableName = "CustomerPreference";
                List<CustomerPreference> preferenceList = new List<CustomerPreference>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CustomerPreference preferences = new CustomerPreference
                        {
                            PreferenceID = Convert.ToInt16(ds.Tables[0].Rows[i]["PreferenceID"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["PreferenceID"].ToString().Trim() : "0"),
                            POptStatus = (OptStatus)Enum.Parse(typeof(OptStatus), ds.Tables[0].Rows[i]["PreferenceOptStatusID"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["PreferenceOptStatusID"].ToString().Trim() : "2"),
                            PreferenceDescriptionEng = ds.Tables[0].Rows[i]["PreferenceDescEnglish"].ToString().Trim(),
                            PreferenceDescriptionLocal = ds.Tables[0].Rows[i]["PreferenceDescLocal"].ToString().Trim(),
                            CustomerPreferenceType = Convert.ToInt16(ds.Tables[0].Rows[i]["PreferenceType"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["PreferenceType"].ToString().Trim() : "0"),
                            IsDeleted = ds.Tables[0].Rows[i]["IsDeleted"].ToString().Trim(),
                            Sortseq = Convert.ToInt16(ds.Tables[0].Rows[i]["SortSeq"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["SortSeq"].ToString().Trim() : "0")
                        };
                        preferenceList.Add(preferences);
                    }
                    response.Preference = preferenceList;
                }

                NGCTrace.NGCTrace.TraceInfo("End:PreferenceServices.Preference.customerID");
                NGCTrace.NGCTrace.TraceDebug("End:PreferenceServices.Preference.customerID");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.customerID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.customerID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.customerID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            return response;

        }

        /// <summary>
        /// To send email to customer who have opted in for different preferences
        /// </summary>
        /// <param name="customerDetails"></param>
        /// <returns>Boolean</returns>

        public bool SendEmailToCustomers(CustomerPreference customerPreference, CustomerDetails customerDetails)
        {

            //create the mail message
            MailMessage mail;

            try
            {
                ///////Tracing code starts here///////////
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendEmailToCustomers");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendEmailToCustomers");
                //set the addresses


                string strTempalteHtmlpath = string.Empty;
                //mail = new MailMessage(strFrom, customerDetails.EmailId);


                for (int i = 0; i < customerPreference.Preference.Count; i++)
                {
                    string emailBody = string.Empty;
                    string strSubject = string.Empty;
                    string strHTML = string.Empty;
                    if (!string.IsNullOrEmpty(customerPreference.Preference[i].EmailSubject.ToString().Trim()))
                    {
                        string strEmailDisplayName = string.Empty;
                        string strFrom = string.Empty;
                        if (customerPreference.Preference[i].PreferenceID.ToString().Trim() == "43")
                        {
                            strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                            strEmailDisplayName = ConfigurationSettings.AppSettings["fromEmailDisplayName"];
                        }
                        else if (customerPreference.Preference[i].PreferenceID.ToString().Trim() == "48")
                        {
                            strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                            strEmailDisplayName = ConfigurationSettings.AppSettings["fromEmailDisplayNameBT"];
                        }
                        else
                        {
                            strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                            strEmailDisplayName = ConfigurationSettings.AppSettings["fromAddressForBT"];
                        }
                        MailAddress from = new MailAddress(strFrom, strEmailDisplayName);
                        MailAddress to = new MailAddress(customerDetails.EmailId);
                        mail = new MailMessage(from, to);

                        strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"] + customerPreference.Preference[i].PreferenceID.ToString().Trim() + ".htm");
                        strHTML = File.ReadAllText(strTempalteHtmlpath);
                        emailBody = strHTML.Replace("######", Helper.MasknFormatClubcard(customerDetails.CardNumber))
                                           .Replace("#####", customerDetails.Firstname)
                                           .Replace("####", customerDetails.Surname)
                                           .Replace("###", customerDetails.Title);

                        strSubject = customerPreference.Preference[i].EmailSubject.ToString().Trim();

                        mail.Subject = strSubject;
                        //Read Html Template File Path
                        //String builder to add mail body
                        StringBuilder strBlr = new StringBuilder();
                        strBlr = strBlr.Append(emailBody);
                        mail.Body = strBlr.ToString();
                        mail.IsBodyHtml = true;


                        //first we create the Plain Text part
                        AlternateView plainView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/plain");
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/html");

                        mail.AlternateViews.Add(plainView);
                        mail.AlternateViews.Add(htmlView);

                        //send the message
                        SmtpClient smtpMail = new SmtpClient(ConfigurationSettings.AppSettings["smtpClient"]);
                        smtpMail.Send(mail);
                        mail.Dispose();
                    }
                }



                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendEmailToCustomers");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendEmailToCustomers");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.SendEmailToCustomers" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.SendEmailToCustomers" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.SendEmailToCustomers");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return true;
        }


        public bool SendEmailNoticeToCustomers(long customerID, CustomerPreference customerPreference, CustomerDetails customerDetails, String Pagedetails, String trackxml)
        {


            try
            {
                ///////Tracing code starts here///////////
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendEmailNoticeToCustomers");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendEmailNoticeToCustomers");
                //set the addresses
                //create the mail message
                MailMessage mail;
                string sCulture = string.Empty;
                short iUserID = 0;
                DataTable dtPreference = new DataTable("PreferenceDataType");
                dtPreference.Columns.Add("PreferenceID", typeof(Int16));
                dtPreference.Columns.Add("updateDateTime", typeof(DateTime));
                dtPreference.Columns.Add("OptStatusID", typeof(Int16));

                for (int i = 0; i < customerPreference.Preference.Count; i++)
                {
                    dtPreference.Rows.Add(
                      customerPreference.Preference[i].PreferenceID,
                      customerPreference.Preference[i].UpdateDateTime, (Int16)customerPreference.Preference[i].POptStatus);
                }

                sCulture = customerPreference.Culture;
                iUserID = Helper.GetConsumerID(customerPreference.UserID);

                SqlParameter[] paramsToPreference = new SqlParameter[4];

                paramsToPreference[0] = new SqlParameter("@pCustomerID", SqlDbType.BigInt);
                paramsToPreference[0].Value = customerID;
                paramsToPreference[1] = new SqlParameter("@pUserID", SqlDbType.BigInt);
                paramsToPreference[1].Value = iUserID;
                paramsToPreference[2] = new SqlParameter("@pCulture", System.Data.SqlDbType.NVarChar);
                paramsToPreference[2].Value = sCulture;
                paramsToPreference[3] = new SqlParameter("@pPreferences", System.Data.SqlDbType.Structured);
                paramsToPreference[3].Value = dtPreference;

                SqlHelper.ExecuteNonQuery(connectionString,
                     CommandType.StoredProcedure, "USP_MaintainCustomerPreference", paramsToPreference);



                string strTempalteHtmlpath = string.Empty;
                string emailBody = string.Empty;
                string strSubject = string.Empty;
                string strHTML = string.Empty;
                string strEmailDisplayName = string.Empty;
                string strFrom = string.Empty;
                string Email = string.Empty;
                string FName = string.Empty;
                string MName = string.Empty;
                string SName = string.Empty;
                string Dob = string.Empty;
                string Gender = string.Empty;
                string Address = string.Empty;
                string MobilePhone = string.Empty;
                string DayTimePhone = string.Empty;
                string EvengingPhone = string.Empty;
                string message = string.Empty;
                Hashtable htblTrackFeilds = ConvertXmlHash.XMLToHashTable(trackxml, "TrackFields");
                if (!string.IsNullOrEmpty(customerDetails.EmailId.ToString().Trim()))
                {


                    if (htblTrackFeilds.Contains("Email"))
                    {
                        if (htblTrackFeilds["Email"].ToString() != string.Empty)
                        {
                            Email = "<li>" + htblTrackFeilds["Email"].ToString() + "</li>";

                        }
                        else Email = "";
                    }
                    else Email = "";

                    if (htblTrackFeilds.Contains("TrackFName"))
                    {
                        if (htblTrackFeilds["TrackFName"].ToString() != string.Empty)
                        {
                            FName = "<li>" + htblTrackFeilds["TrackFName"].ToString() + "</li>";
                        }
                        else FName = "";
                    }
                    else FName = "";
                    if (htblTrackFeilds.Contains("TrackMName"))
                    {
                        if (htblTrackFeilds["TrackMName"].ToString() != string.Empty)
                        {

                            MName = "<li>" + htblTrackFeilds["TrackMName"].ToString() + "</li>";
                        }
                        else MName = "";
                    }
                    else MName = "";
                    if (htblTrackFeilds.Contains("TrackSName"))
                    {
                        if (htblTrackFeilds["TrackSName"].ToString() != string.Empty)
                        {
                            SName = "<li>" + htblTrackFeilds["TrackSName"].ToString() + "</li>";

                        }
                        else SName = "";
                    }
                    else SName = "";
                    if (htblTrackFeilds.Contains("TrackDOB"))
                    {
                        if (htblTrackFeilds["TrackDOB"].ToString() != string.Empty)
                        {
                            Dob = "<li>" + htblTrackFeilds["TrackDOB"].ToString() + "</li>";

                        }
                        else Dob = "<b/> <BR/>";
                    }
                    else Dob = "";
                    if (htblTrackFeilds.Contains("TrackGender"))
                    {
                        if (htblTrackFeilds["TrackGender"].ToString() != string.Empty)
                        {
                            Gender = "<li>" + htblTrackFeilds["TrackGender"].ToString() + "</li>";
                        }
                        else Gender = "";
                    }
                    else Gender = "";
                    if (htblTrackFeilds.Contains("TrackMobile"))
                    {
                        if (htblTrackFeilds["TrackMobile"].ToString() != string.Empty)
                        {

                            MobilePhone = "<li>" + htblTrackFeilds["TrackMobile"].ToString() + "</li>";

                        }
                        else MobilePhone = "";
                    }
                    else MobilePhone = "";
                    if (htblTrackFeilds.Contains("TrackDayTimePhone"))
                    {
                        if (htblTrackFeilds["TrackDayTimePhone"].ToString() != string.Empty)
                        {

                            DayTimePhone = "<li>" + htblTrackFeilds["TrackDayTimePhone"].ToString() + "</li>";

                        }
                        else DayTimePhone = "";
                    }
                    else DayTimePhone = "";
                    if (htblTrackFeilds.Contains("TrackEveningPhone"))
                    {
                        if (htblTrackFeilds["TrackEveningPhone"].ToString() != string.Empty)
                        {

                            EvengingPhone = "<li>" + htblTrackFeilds["TrackEveningPhone"].ToString() + "</li>";

                        }
                        else EvengingPhone = "";
                    }
                    else EvengingPhone = "";
                    if (htblTrackFeilds.Contains("TrackAddress"))
                    {

                        if (htblTrackFeilds["TrackAddress"].ToString() != string.Empty)
                        {
                            Address = "<li>" + htblTrackFeilds["TrackAddress"].ToString() + "</li>";

                        }
                        else Address = "";
                    }
                    else Address = "";

                    message = Email + FName + MName + SName + Dob + MobilePhone + DayTimePhone + EvengingPhone + Address + Gender;
                    strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                    strEmailDisplayName = ConfigurationSettings.AppSettings["fromSecurityEmailDisplayName"];
                    MailAddress to = new MailAddress(customerDetails.EmailId.ToString().Trim());
                    MailAddress from = new MailAddress(strFrom, strEmailDisplayName);
                    mail = new MailMessage(from, to);
                    strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"] + "EmailNotice.htm");
                    strHTML = File.ReadAllText(strTempalteHtmlpath);

                    emailBody = strHTML.Replace("############", Pagedetails)
                                        .Replace("##########", message);


                    strSubject = "Clubcard Account update confirmation";

                    mail.Subject = strSubject;
                    //Read Html Template File Path
                    //String builder to add mail body
                    StringBuilder strBlr = new StringBuilder();
                    strBlr = strBlr.Append(emailBody);
                    mail.Body = strBlr.ToString();
                    mail.IsBodyHtml = true;

                    //first we create the Plain Text part
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/plain");
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/html");

                    mail.AlternateViews.Add(plainView);
                    mail.AlternateViews.Add(htmlView);
                    if (htblTrackFeilds.Contains("oldEmailAddress"))
                    {
                        if (htblTrackFeilds.Contains("bEmailChange"))
                        {
                            if (htblTrackFeilds["bEmailChange"].ToString() != string.Empty)
                            {
                                if (htblTrackFeilds["oldEmailAddress"].ToString() != string.Empty)
                                {
                                    if (to.Address.ToString() != htblTrackFeilds["oldEmailAddress"].ToString())
                                    {
                                        mail.CC.Add(new MailAddress(htblTrackFeilds["oldEmailAddress"].ToString()));
                                    }
                                }
                            }
                        }
                    }
                    //send the message
                    SmtpClient smtpMail = new SmtpClient(ConfigurationSettings.AppSettings["smtpClient"]);
                    smtpMail.Send(mail);
                    mail.Dispose();
                }
                // if user removes the existing email address then following lines will be executed.               
                else if (htblTrackFeilds.Contains("oldEmailAddress"))
                {
                    if (htblTrackFeilds["oldEmailAddress"].ToString() != string.Empty)
                    {

                        if (htblTrackFeilds.Contains("Email"))
                        {
                            if (htblTrackFeilds["Email"].ToString() != string.Empty)
                            {
                                Email = "<li>" + htblTrackFeilds["Email"].ToString() + "</li>";

                            }
                            else Email = "";
                        }
                        else Email = "";

                        if (htblTrackFeilds.Contains("TrackFName"))
                        {
                            if (htblTrackFeilds["TrackFName"].ToString() != string.Empty)
                            {
                                FName = "<li>" + htblTrackFeilds["TrackFName"].ToString() + "</li>";
                            }
                            else FName = "";
                        }
                        else FName = "";
                        if (htblTrackFeilds.Contains("TrackMName"))
                        {
                            if (htblTrackFeilds["TrackMName"].ToString() != string.Empty)
                            {

                                MName = "<li>" + htblTrackFeilds["TrackMName"].ToString() + "</li>";
                            }
                            else MName = "";
                        }
                        else MName = "";
                        if (htblTrackFeilds.Contains("TrackSName"))
                        {
                            if (htblTrackFeilds["TrackSName"].ToString() != string.Empty)
                            {
                                SName = "<li>" + htblTrackFeilds["TrackSName"].ToString() + "</li>";

                            }
                            else SName = "";
                        }
                        else SName = "";
                        if (htblTrackFeilds.Contains("TrackDOB"))
                        {
                            if (htblTrackFeilds["TrackDOB"].ToString() != string.Empty)
                            {
                                Dob = "<li>" + htblTrackFeilds["TrackDOB"].ToString() + "</li>";

                            }
                            else Dob = "<b/> <BR/>";
                        }
                        else Dob = "";
                        if (htblTrackFeilds.Contains("TrackGender"))
                        {
                            if (htblTrackFeilds["TrackGender"].ToString() != string.Empty)
                            {
                                Gender = "<li>" + htblTrackFeilds["TrackGender"].ToString() + "</li>";
                            }
                            else Gender = "";
                        }
                        else Gender = "";
                        if (htblTrackFeilds.Contains("TrackMobile"))
                        {
                            if (htblTrackFeilds["TrackMobile"].ToString() != string.Empty)
                            {

                                MobilePhone = "<li>" + htblTrackFeilds["TrackMobile"].ToString() + "</li>";

                            }
                            else MobilePhone = "";
                        }
                        else MobilePhone = "";
                        if (htblTrackFeilds.Contains("TrackDayTimePhone"))
                        {
                            if (htblTrackFeilds["TrackDayTimePhone"].ToString() != string.Empty)
                            {

                                DayTimePhone = "<li>" + htblTrackFeilds["TrackDayTimePhone"].ToString() + "</li>";

                            }
                            else DayTimePhone = "";
                        }
                        else DayTimePhone = "";
                        if (htblTrackFeilds.Contains("TrackEveningPhone"))
                        {
                            if (htblTrackFeilds["TrackEveningPhone"].ToString() != string.Empty)
                            {

                                EvengingPhone = "<li>" + htblTrackFeilds["TrackEveningPhone"].ToString() + "</li>";

                            }
                            else EvengingPhone = "";
                        }
                        else EvengingPhone = "";
                        if (htblTrackFeilds.Contains("TrackAddress"))
                        {

                            if (htblTrackFeilds["TrackAddress"].ToString() != string.Empty)
                            {
                                Address = "<li>" + htblTrackFeilds["TrackAddress"].ToString() + "</li>";

                            }
                            else Address = "";
                        }
                        else Address = "";

                        message = Email + FName + MName + SName + Dob + MobilePhone + DayTimePhone + EvengingPhone + Address + Gender;
                        strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                        strEmailDisplayName = ConfigurationSettings.AppSettings["fromSecurityEmailDisplayName"];
                        MailAddress to = new MailAddress(htblTrackFeilds["oldEmailAddress"].ToString());
                        MailAddress from = new MailAddress(strFrom, strEmailDisplayName);
                        mail = new MailMessage(from, to);
                        strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"] + "EmailNotice.htm");
                        strHTML = File.ReadAllText(strTempalteHtmlpath);

                        emailBody = strHTML.Replace("############", Pagedetails)
                                            .Replace("##########", message);


                        strSubject = "Clubcard Account update confirmation";

                        mail.Subject = strSubject;
                        //Read Html Template File Path
                        //String builder to add mail body
                        StringBuilder strBlr = new StringBuilder();
                        strBlr = strBlr.Append(emailBody);
                        mail.Body = strBlr.ToString();
                        mail.IsBodyHtml = true;

                        //first we create the Plain Text part
                        AlternateView plainView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/plain");
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/html");

                        mail.AlternateViews.Add(plainView);
                        mail.AlternateViews.Add(htmlView);
                        if (htblTrackFeilds.Contains("oldEmailAddress"))
                        {
                            if (htblTrackFeilds.Contains("bEmailChange"))
                            {
                                if (htblTrackFeilds["bEmailChange"].ToString() != string.Empty)
                                {
                                    if (htblTrackFeilds["oldEmailAddress"].ToString() != string.Empty)
                                    {
                                        if (to.Address.ToString() != htblTrackFeilds["oldEmailAddress"].ToString())
                                        {
                                            mail.CC.Add(new MailAddress(htblTrackFeilds["oldEmailAddress"].ToString()));
                                        }
                                    }
                                }
                            }
                        }
                        //send the message
                        SmtpClient smtpMail = new SmtpClient(ConfigurationSettings.AppSettings["smtpClient"]);
                        smtpMail.Send(mail);
                        mail.Dispose();

                    }

                }



                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendEmailNoticeToCustomers");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendEmailNoticeToCustomers");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.SendEmailNoticeToCustomers" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.SendEmailNoticeToCustomers" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.SendEmailNoticeToCustomers");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return true;
        }
        #endregion

        #region Baby Todler Club

        /// <summary>
        /// To get all the Club details for logged in customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns>ClubDetails Object</returns>

        public ClubDetails ViewClubDetails(Int64 customerID)
        {
            ClubDetails objClubPreference = new ClubDetails();
            DataSet ds = new DataSet();
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.ViewClubDetails");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.ViewClubDetails - customerID :" + customerID.ToString());

                ds = SqlHelper.ExecuteDataset(connectionString, "USP_ViewBabyToddlerClub", customerID);
                ds.Tables[0].TableName = "ClubDetails";
                List<ClubDetails> clubList = new List<ClubDetails>();
                List<ClubDetails> mediaList = new List<ClubDetails>();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ClubDetails objClubs = new ClubDetails
                        {
                            DateOfBirth = ds.Tables[0].Rows[i]["ExpectedActualBirthDate"].ToString().Trim(),
                            Media = Convert.ToInt16(ds.Tables[0].Rows[i]["MediaID"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["MediaID"].ToString().Trim() : "0"),
                            MembershipID = ds.Tables[0].Rows[i]["MembershipNumber"].ToString().Trim(),
                            IsDeleted = ds.Tables[0].Rows[i]["IsDeleted"].ToString().Trim(),
                            ClubID = Convert.ToInt16(ds.Tables[0].Rows[i]["ClubTypeID"].ToString().Trim() != "" ? ds.Tables[0].Rows[i]["ClubTypeID"].ToString().Trim() : "0"),
                        };
                        clubList.Add(objClubs);
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ClubDetails objClubs = new ClubDetails
                        {
                            MediaDesc = ds.Tables[1].Rows[i]["MediaDescEnglish"].ToString().Trim(),
                            Media = Convert.ToInt16(ds.Tables[1].Rows[i]["MediaID"].ToString().Trim() != "" ? ds.Tables[1].Rows[i]["MediaID"].ToString().Trim() : "0"),
                        };
                        mediaList.Add(objClubs);
                    }
                }
                objClubPreference.ClubInformation = clubList;
                objClubPreference.MediaDetails = mediaList;
                NGCTrace.NGCTrace.TraceInfo("End:PreferenceServices.Preference.ViewClubDetails");
                NGCTrace.NGCTrace.TraceDebug("End:PreferenceServices.Preference.ViewClubDetails");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.ViewClubDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.ViewClubDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.ViewClubDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
            finally
            {
            }
            return objClubPreference;
        }

        /// <summary>
        /// To update all the club details for logged in customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="cluDetails"></param>
        /// <param name="sEmailDTo"></param>
        public void MaintainClubDetails(long customerID, ClubDetails cluDetails, string sEmailDTo)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.MaintainClubDetails - CustomerID:" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.MaintainClubDetails - CustomerID:" + customerID.ToString());

                string sCulture = string.Empty;
                short iUserID = 0;
                DataTable dtClubDeatils = new DataTable("ClubDataType");
                dtClubDeatils.Columns.Add("ClubTypeID", typeof(Int16));
                dtClubDeatils.Columns.Add("MembershipNumber", typeof(string));
                dtClubDeatils.Columns.Add("IsDeleted", typeof(string));

                for (int i = 0; i < cluDetails.ClubInformation.Count; i++)
                {
                    dtClubDeatils.Rows.Add(
                      cluDetails.ClubInformation[i].ClubID,
                     cluDetails.ClubInformation[i].MembershipID, cluDetails.ClubInformation[i].IsDeleted);
                }

                DataTable dtChildMemberDeatils = new DataTable("ChildMemberDataType");
                dtChildMemberDeatils.Columns.Add("OriginalDOB", typeof(string), null);
                dtChildMemberDeatils.Columns.Add("NewDOB", typeof(string), null);

                for (int i = 0; i < cluDetails.DOBDetails.Count; i++)
                {
                    dtChildMemberDeatils.Rows.Add(
                        cluDetails.DOBDetails[i].DateOfBirth != "" ? cluDetails.DOBDetails[i].DateOfBirth : null,
                    cluDetails.DOBDetails[i].ChangedBirthDate != "" ? cluDetails.DOBDetails[i].ChangedBirthDate : null);
                }


                sCulture = cluDetails.Culture;
                iUserID = Helper.GetConsumerID(cluDetails.UserID);

                SqlParameter[] paramsToClub = new SqlParameter[6];

                paramsToClub[0] = new SqlParameter("@pCustomerID ", SqlDbType.BigInt);
                paramsToClub[0].Value = customerID;
                paramsToClub[1] = new SqlParameter("pJoinDate", SqlDbType.DateTime);
                paramsToClub[1].Value = cluDetails.JoinDate; ;
                paramsToClub[2] = new SqlParameter("pMediaID", SqlDbType.SmallInt);
                paramsToClub[2].Value = cluDetails.Media;
                paramsToClub[3] = new SqlParameter("@pUserID", SqlDbType.BigInt);
                paramsToClub[3].Value = iUserID;
                paramsToClub[4] = new SqlParameter("@pClub", System.Data.SqlDbType.Structured);
                paramsToClub[4].Value = dtClubDeatils;
                paramsToClub[5] = new SqlParameter("@pChildMembers", System.Data.SqlDbType.Structured);
                paramsToClub[5].Value = dtChildMemberDeatils;

                SqlHelper.ExecuteNonQuery(connectionString,
                     CommandType.StoredProcedure, "USP_InsertUpdateBabyToddlerClub", paramsToClub);

                //if (!string.IsNullOrEmpty(sEmailDTo.ToString().Trim()))
                //{
                //    SendBTConfirmationEmail(sEmailDTo);
                //}

                NGCTrace.NGCTrace.TraceInfo("End:PreferenceServices.Preference.MaintainClubDetails - CustomerID:" + customerID.ToString());
                NGCTrace.NGCTrace.TraceDebug("End:PreferenceServices.Preference.MaintainClubDetails - CustomerID:" + customerID.ToString());
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.MaintainClubDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.MaintainClubDetails - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.MaintainClubDetails");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Send Email to customer when customer Opts in for BnT Club
        /// </summary>
        /// <param name="strTo"></param>
        /// <returns>true/false</returns>
        public bool SendBTConfirmationEmail(string strTo)
        {
            bool bResult = false;
            //String builder to add mail body
            StringBuilder strBlr = new StringBuilder();
            string strClubcardID = string.Empty;
            string strTitle = string.Empty;
            string strCustName = string.Empty;
            string strHTML = string.Empty;
            string strTempalteHtmlpath = string.Empty;

            //create the mail message
            MailMessage mail;

            try
            {
                ///////Tracing code starts here///////////
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendBTConfirmationEmail");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendBTConfirmationEmail");
                //set the addresses
                string strFrom = ConfigurationSettings.AppSettings["fromAddressForBT"];
                string strSubject = "Welcome to Tesco Baby & Toddler Club";
                mail = new MailMessage(strFrom, strTo);

                mail.Subject = strSubject;

                //Read Html Template File Path
                strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"]);
                strHTML = File.ReadAllText(strTempalteHtmlpath);
                strBlr = strBlr.Append(strHTML);
                mail.Body = strBlr.ToString();
                mail.IsBodyHtml = true;

                //first we create the Plain Text part
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/plain");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/html");

                //add the views
                mail.AlternateViews.Add(plainView);
                mail.AlternateViews.Add(htmlView);

                //send the message
                SmtpClient smtpMail = new SmtpClient(ConfigurationSettings.AppSettings["smtpClient"]);
                smtpMail.Send(mail);

                mail.Dispose();

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("Start:PreferenceServices.Preference.SendBTConfirmationEmail");
                NGCTrace.NGCTrace.TraceDebug("Start:PreferenceServices.Preference.SendBTConfirmationEmail");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:PreferenceServices.Preference.SendBTConfirmationEmail");
                NGCTrace.NGCTrace.TraceError("Error:PreferenceServices.Preference.SendBTConfirmationEmail");
                NGCTrace.NGCTrace.TraceWarning("Warning:PreferenceServices.Preference.SendBTConfirmationEmail");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = true;
            }
            finally
            {
            }

            return bResult;
        }
        #endregion
    }
}
