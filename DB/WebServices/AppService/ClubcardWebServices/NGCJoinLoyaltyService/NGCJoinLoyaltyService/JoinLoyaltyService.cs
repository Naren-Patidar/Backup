using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net.Mail;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using NGCTrace;
using Tesco.NGC.Utils;
using System.Collections;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;


namespace Tesco.com.NGCJoinLoyaltyService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class JoinLoyaltyService : IJoinLoyaltyService
    {
        string connection = string.Empty;
        Join joinObject = null;

         //Added as part of ROI conncetion string management
        //begin
        private string culture="";
        private string connectionString="";
        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public JoinLoyaltyService()
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

        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            mailSent = true;
        }

        #region Fields

        /// <summary>
        /// Customer Title
        /// </summary>
        public string titleEnglish;

        /// <summary>
        /// Customer Name3
        /// </summary>
        public string name3;

        /// <summary>
        /// Customer EmailAddress
        /// </summary>
        public string emailAddress;

        /// <summary>
        /// Customer EmailAddress
        /// </summary>
        public long clubcard;

        #endregion

        #region Properties


        /// <summary>
        ///  Customer Title
        /// </summary>
        public string TitleEnglish { get { return this.titleEnglish; } set { this.titleEnglish = value; } }

        /// <summary>
        ///  Customer Name3
        /// </summary>
        public string Name3 { get { return this.name3; } set { this.name3 = value; } }

        /// <summary>
        ///  Customer EmailAddress
        /// </summary>
        public string EmailAddress { get { return this.emailAddress; } set { this.emailAddress = value; } }


        /// <summary>
        ///  ClubcardID
        /// </summary>
        public long ClubcardID { get { return this.clubcard; } set { this.clubcard = value; } }

        #endregion


        public String AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture)
        {
            string viewXml = String.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate  objectXml" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate objectXml" + objectXml);
                joinObject = new Join();
                viewXml = joinObject.AccountCreate(dotcomCustomerID, objectXml, source, culture);

                Hashtable htblCustomer = ConvertXmlHash.XMLToHashTable(objectXml, "customer");
                Hashtable htblClubcard = ConvertXmlHash.XMLToHashTable(viewXml, "NewDataSet");

                this.TitleEnglish = (string)htblCustomer[Constants.CUSTOMER_TITLE_ENGLISH];
                this.Name3 = (string)htblCustomer[Constants.CUSTOMER_NAME3];
                this.EmailAddress = (string)htblCustomer[Constants.USER_EMAILID];
                this.ClubcardID = Convert.ToInt64(htblClubcard["Clubcard"]);
                if (!string.IsNullOrEmpty(EmailAddress.Trim()))
                {
                    SendJoinConfirmationEmail(EmailAddress, TitleEnglish, Name3, ClubcardID);
                }
                NGCTrace.NGCTrace.TraceInfo("Start:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate  objectXml" + objectXml);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate objectXml" + objectXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate  objectXml" + objectXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate objectXml" + objectXml + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCJoinLoyaltyService.JoinLoyaltyService.AccountCreate ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }
            return viewXml;

        }

        /// <summary>
        /// This method sends an email once the customer activate their account in MCA
        /// </summary>
        /// <param name="strTo">To whom the email should go</param>
        /// <returns></returns>
        public bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID)
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
                NGCTrace.NGCTrace.TraceInfo("Start:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceDebug("Start:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
                //set the addresses
                string strFrom = ConfigurationSettings.AppSettings["fromAddressForJoin"];
                string strSubject = "Thank you for joining Clubcard";
                string sURL = ConfigurationSettings.AppSettings["hostURL"];
                mail = new MailMessage(strFrom, strTo);

                mail.Subject = strSubject;

                //Create the Html part
                //strBlr.Append("<html><body>");
                //strBlr.Append("<table style='font-family:Verdana; font-size:13px;width:700px'>");
                //strBlr.Append("<tr><td>Dear " + title + " " + custName + ",<br /><br />");
                //strBlr.Append("Thank you for joining Clubcard! ");
                //strBlr.Append("Keep a look out for your Clubcard and keyfobs which will arrive in the post within the next 10 days.<br /><br />");
                //strBlr.Append("Start collecting points now! <a href='" + sURL + clubcardID + "'>Click here </a> to print a temporary Clubcard that you can use in store and online while you wait for your Clubcard and keyfobs to arrive.<br /><br />");

                //strBlr.Append("Your Clubcard number is " + clubcardID + ". ");
                //strBlr.Append("If you have an iPhone, Blackberry or Nokia smartphone, you can use this number to download your Clubcard to your mobile <a href='http://www.tesco.com/clubcard/clubcard/smartphones.asp'>here.</a> ");
                //strBlr.Append("You can also use your Clubcard number to collect points when ordering items online from Tesco.com or Tesco direct, ");
                //strBlr.Append("but please allow 48 hours for your Clubcard number to be activated online.<br /><br />");

                //strBlr.Append("As a Clubcard member, you will receive Clubcard statements throughout the year containing any vouchers you have earned, plus money off and extra points coupons for products we think you might like.<br /><br />");
                //strBlr.Append("If you have any questions you can call us on 0800 59 16 88 (lines are open Mon - Fri 9am - 8pm, Sat 9am - 5pm, Sun closed) or <a href='http://www.tesco.com/clubcard'>visit our website</a> for more information.<br /><br />");
                //strBlr.Append("Best wishes,<br />");
                //strBlr.Append("The Clubcard team");
                //strBlr.Append("</td></tr>");
                //strBlr.Append("</table>");
                //strBlr.Append("</body></html>");

                //Read Html Template File Path
                strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"]);
                strClubcardID = Convert.ToString(ConfigurationSettings.AppSettings["ClubcardID"]);
                strCustName = Convert.ToString(ConfigurationSettings.AppSettings["CustName"]);
                strTitle = Convert.ToString(ConfigurationSettings.AppSettings["Title"]);
                strHTML = File.ReadAllText(strTempalteHtmlpath);
              //strHTML = strHTML.Replace(strClubcardID, Convert.ToString(clubcardID)).Replace(strTitle, title).Replace(strCustName, custName);
                strHTML = strHTML.Replace(strClubcardID, Convert.ToString(clubcardID)).Replace(strCustName, Convert.ToString(custName)).Replace(strTitle, Convert.ToString(title));
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

                //// Set the method that is called back when the send operation ends.
                //smtpMail.SendCompleted += new
                //SendCompletedEventHandler(SendCompletedCallback);
                //// The userState can be any object that allows your callback 
                //// method to identify this send operation.
                //// For this example, the userToken is a string constant.

                //string userState = "";
                //smtpMail.SendAsync(mail, userState);

                //// If the user canceled the send, and mail hasn't been sent yet,
                //// then cancel the pending operation.
                //if (mailSent == false)
                //{
                //    smtpMail.SendAsyncCancel();
                //}
                //// Clean up.
                //mail.Dispose();

                ////smtpMail.Send(mail);

                bResult = true;
                NGCTrace.NGCTrace.TraceInfo("Start:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceDebug("Start:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceError("Error:JoinLoyaltyService.SendJoinConfirmationEmail Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceWarning("Warning:JoinLoyaltyService.SendJoinConfirmationEmail");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = true;
            }
            finally
            {
            }

            return bResult;
        }
        /// <summary>
        /// check customer information for duplicattion and promotion code
        /// </summary>
        /// <param name="inputXml"></param>
        /// <param name="resultXml"></param>
        /// <returns></returns>
        public bool AccountDuplicateCheck(string inputXml, out string resultXml)
        {

            bool bResult = false;
            Join objJoin = new Join();
            resultXml = string.Empty;

            try
            {
                ///////Tracing code starts here///////////
                NGCTrace.NGCTrace.TraceInfo("Start:JoinLoyaltyService.AccountDuplicateCheck ");
                NGCTrace.NGCTrace.TraceDebug("Start:JoinLoyaltyService.AccountDuplicateCheck inputXml :" + inputXml);

                bResult = objJoin.AccountDuplicateCheck(inputXml, out resultXml);

                NGCTrace.NGCTrace.TraceInfo("End:JoinLoyaltyService.AccountDuplicateCheck ");
                NGCTrace.NGCTrace.TraceDebug("End:JoinLoyaltyService.AccountDuplicateCheck resultXml :" + resultXml);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:JoinLoyaltyService.AccountDuplicateCheck ErrorMessage:" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:JoinLoyaltyService.AccountDuplicateCheck Customer Name:" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:JoinLoyaltyService.AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                bResult = false;
            }


            return bResult;
        }
    }

}
