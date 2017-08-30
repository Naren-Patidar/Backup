using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ServiceModel;
using System.Diagnostics;
using PrintVouchersAtKiosk.customerservice;
using PrintVouchersAtKiosk.BigExchange;
using System.Text.RegularExpressions;
using System.Threading;
using System.Globalization;
using PrintVouchersAtKiosk.ClubcardActivationService;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PrintVouchersAtKiosk
{
    /// <summary>
    ///Validates the customers credential against NGC database 
    ///Author Dimple 
    ///Modified by seema to make the screen multi lingual and to support multiple country validation
    /// </summary>
    public partial class Verification : Base
    {

        private CustomerServiceClient customerServiceClient;
        private ClubcardOnlineServiceClient clubcardOnlineServiceClient;
        private string[] ArrSelFields;
        private DataSet dsCustomerData = new DataSet();
        private string IsPostCodeLast3Digit = String.Empty;
        private string defaultDayValue = String.Empty;
        private string defaultMonthValue = String.Empty;
        private string defaultYearValue = String.Empty;

        string showFirstNameField = String.Empty;
        string showLastNameField = String.Empty;
        string showDateOfBirthField = String.Empty;
        string showHouseNumberField = String.Empty;
        string showPostCodeField = String.Empty;
        string showPhoneNumberField = String.Empty;
        string showSSNField = String.Empty;
        string showEmailField = String.Empty;

        private string IsFirstNameOptional = String.Empty;
        private string IsLastNameOptional = String.Empty;
        private string IsDateOfBirthOptional = String.Empty;
        private string IsHouseNumberOptional = String.Empty;
        private string IsPostCodeOptional = String.Empty;
        private string IsPhoneNumberOptional = String.Empty;
        private string IsSSNOptional = String.Empty;
        private string IsEmailOptional = String.Empty;

        bool DisableFirstControl = true;

       #region Properties

        /// <summary>
        /// Gets or sets the session booking.
        /// </summary>
        /// <value>The session booking.</value>
        public BigExchange.BookingPrintVoucher BookingPrintVoucher
        {
            get
            {
                if (Session["BookingPrintVoucher"] == null)
                {
                    //Session["BookingPrintVoucher"] = Helper.CreateBooking("O");
                    Server.Transfer("Error.aspx");
                    return null;
                }
                else
                {
                    return (BigExchange.BookingPrintVoucher)Session["BookingPrintVoucher"];
                }

            }
            set
            {
                Session["BookingPrintVoucher"] = value;
            }
        }

        #endregion

        public void CheckFieldsVisibility()
        {
            string concatSelFieldList = String.Empty;
            
            DataTable CustomerTable = null;
            DataColumn ColName = null;
            DataColumn ColType = null;
            try
            {
                concatSelFieldList = String.Empty;

                defaultDayValue = Convert.ToString(GetLocalResourceObject("txtDOBResource1.Text")).ToUpper();
                defaultMonthValue = Convert.ToString(GetLocalResourceObject("txtmonthResource1.Text")).ToUpper();
                defaultYearValue = Convert.ToString(GetLocalResourceObject("txtyearResource1.Text")).ToUpper();

                CustomerTable = new DataTable("Customer");
                ColName = new DataColumn("ConfigurationName");
                ColType = new DataColumn("ConfigurationType");
                CustomerTable.Columns.Add(ColName);
                CustomerTable.Columns.Add(ColType);

                if (showFirstNameField.ToLower() == "true")
                {
                    divFirstName.Visible = true;
                    concatSelFieldList = concatSelFieldList + "1,";
                    CustomerTable.Rows.Add("Name1", "20");
                }
                else
                {
                    divFirstName.Visible = false;
                }
                if (showLastNameField.ToLower() == "true")
                {
                    divLastName.Visible = true;
                    concatSelFieldList = concatSelFieldList + "2,";
                    CustomerTable.Rows.Add("Name3", "20");
                }
                else
                {
                    divLastName.Visible = false;
                }
                if (showEmailField.ToLower() == "true")
                {
                    divEmail.Visible = true;
                    concatSelFieldList = concatSelFieldList + "3,";
                    CustomerTable.Rows.Add("EmailAddress", "20");
                }
                else
                {
                    divEmail.Visible = false;
                }

                if (showHouseNumberField.ToLower() == "true")
                {
                    divHouseNumber.Visible = true;
                    concatSelFieldList = concatSelFieldList + "4,";
                    CustomerTable.Rows.Add("MailingAddressLine1", "20");
                }
                else
                {
                    divHouseNumber.Visible = false;
                }
                if (showPostCodeField.ToLower() == "true")
                {
                    divPostcode.Visible = true;
                    concatSelFieldList = concatSelFieldList + "5,";
                    if (IsPostCodeLast3Digit.ToLower() == "true")
                        CustomerTable.Rows.Add("MailingAddressPostCode3Digits", "20");
                    else
                        CustomerTable.Rows.Add("MailingAddressPostCode", "20");
                }
                else
                {
                    divPostcode.Visible = false;
                }

                if (showPhoneNumberField.ToLower() == "true")
                {
                    divPhoneNumber.Visible = true;
                    concatSelFieldList = concatSelFieldList + "6,";
                    CustomerTable.Rows.Add("MobilePhoneNumber", "20");
                }
                else
                {
                    divPhoneNumber.Visible = false;
                }
                if (showSSNField.ToLower() == "true")
                {
                    divSSN.Visible = true;
                    concatSelFieldList = concatSelFieldList + "7,";
                    CustomerTable.Rows.Add("SSN", "20");
                }
                else
                {
                    divSSN.Visible = false;
                }
                if (showDateOfBirthField.ToLower() == "true")
                {
                    string dateDelimiter = Helper.GetCultureSpecificDate("01/01/2012").Substring(2,1);
                    divDateOfBirth.Visible = true;
                    concatSelFieldList = concatSelFieldList + "8,";
                    CustomerTable.Rows.Add("DayofBirth", "20");
                    CustomerTable.Rows.Add("MonthofBirth", "20");
                    CustomerTable.Rows.Add("YearofBirth", "20");
                    //Date redenring as per country configuration
                    hdnDateFormat.Value = ConfigurationManager.AppSettings["DateFormat"];
                    string firstControl, secondControl, thirdControl, keySize;
                    firstControl = secondControl = thirdControl = keySize = string.Empty;
                    switch (ConfigurationManager.AppSettings["DateFormat"])
                    {
                        case "ymd": firstControl = "txtyearymd"; keySize = "4"; secondControl = "txtmonthymd"; thirdControl = "txtDOBymd";
                            divyearymd.Attributes.Add("onkeyup", "First_Second()"); divmonthymd.Attributes.Add("onkeyup", "Second_Third()");divDOBymd.Attributes.Add("onkeyup", "Focus_Third()");
                            break;
                        case "ydm": firstControl = "txtyearydm"; keySize = "4"; secondControl = "txtDOBydm"; thirdControl = "txtmonthydm";
                            divyearydm.Attributes.Add("onkeyup", "First_Second()"); divDOBydm.Attributes.Add("onkeyup", "Second_Third()");divmonthydm.Attributes.Add("onkeyup", "Focus_Third()");
                            break;
                        case "mdy": firstControl = "txtmonthmdy"; keySize = "2"; secondControl = "txtDOBmdy"; thirdControl = "txtyearmdy";
                            divmonthmdy.Attributes.Add("onkeyup", "First_Second()"); divDOBmdy.Attributes.Add("onkeyup", "Second_Third()");divyearmdy.Attributes.Add("onkeyup", "Focus_Third()");
                            break;
                        case "dmy": firstControl = "txtDOBdmy"; keySize = "2"; secondControl = "txtmonthdmy"; thirdControl = "txtyeardmy";
                            divDOBdmy.Attributes.Add("onkeyup", "First_Second()"); divmonthdmy.Attributes.Add("onkeyup", "Second_Third()");divyeardmy.Attributes.Add("onkeyup", "Focus_Third()");
                            break;
                    }

                    string ScriptFirst = @"function First_Second() { var sDOB = document.getElementById('" + firstControl + "').value;" +
                        "EnableNext('" + thirdControl + "', 8, '" + ConfigurationManager.AppSettings["IsDateOfBirthOptional"].ToString().ToLower() + "');" +
                        "var lenFn = sDOB.length; var filter = /[0-9]/; var keyPressed = document.getElementById('lblCharCode').value; if (!filter.test(keyPressed)) {return false;}" +
                    "else if (lenFn == " + keySize + " ) " +
                    "{" +
                        "document.getElementById('" + secondControl + "').focus();" +
                        "document.getElementById('" + secondControl + "').select();" + 
                        "return false;" +
                    "}" +
                        "}";

                    string ScriptSecond = @"function Second_Third() { var sDOB = document.getElementById('" + secondControl + "').value;" +
                        "EnableNext('" + thirdControl + "', 8, '" + ConfigurationManager.AppSettings["IsDateOfBirthOptional"].ToString().ToLower() + "');" +
                        "var lenFn = sDOB.length; var filter = /[0-9]/; var keyPressed = document.getElementById('lblCharCode').value; if (!filter.test(keyPressed)) {return false;} " +
                    "else if (lenFn == " + 2 + ") " +
                    "{" +
                        "document.getElementById('" + thirdControl + "').focus();" +
                        "document.getElementById('" + thirdControl + "').select();" + 
                        "return false;" +
                    "}" +
                        "}";

                    string ScriptThird = @"function Focus_Third() { EnableNext('" + thirdControl + "', 8, '" + ConfigurationManager.AppSettings["IsDateOfBirthOptional"].ToString().ToLower() + "');}";


                    ClientScript.RegisterClientScriptBlock(this.GetType(), "First_Second", ScriptFirst, true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Second_Third", ScriptSecond, true);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Focus_Third", ScriptThird, true);
                }
                else
                {
                    divDateOfBirth.Visible = false;
                }
                if (CustomerTable != null)
                    dsCustomerData.Tables.Add(CustomerTable);
                if (concatSelFieldList != string.Empty)
                {
                    ArrSelFields = concatSelFieldList.Split(',');
                }
                else
                {
                    //Not matched contact details
                    Logger.Write("No Fields are enabled in Verification Screen" + " Verification CheckFieldsVisibility()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                    Server.Transfer("MessageForAddress.aspx?ErrorMsg=ZeroFields");
                }

            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Verification CheckFieldsVisibility()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
            
        }
       void EnableField()
       {
           try
           {
               for (int i = 0; i <= ArrSelFields.Length - 2; i++)
               {
                   int field = Convert.ToInt32(ArrSelFields[i]);
                   if (i == 0)
                   {
                       switch (field)
                       {
                           case 1:
                               txtFirstName.CssClass = "text";
                               txtFirstName.Visible = true;
                               txtFirstName.Focus();
                               lblCurrentField.Value = "1";
                               lblFirstName.Visible = false;
                               pnlFirstName.CssClass = "inputboxes addressboxwhite";
                               if (IsFirstNameOptional.ToUpper()=="TRUE" || txtFirstName.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 2:
                               txtLastName.CssClass = "text";
                               txtLastName.Visible = true;
                               txtLastName.Focus();
                               lblCurrentField.Value = "2";
                               lblLastName.Visible = false;
                               pnlLastName.CssClass = "inputboxes addressboxwhite";
                               if (IsLastNameOptional.ToUpper() == "TRUE" || txtLastName.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 3:
                               txtEmail.CssClass = "text";
                               txtEmail.Visible = true;
                               txtEmail.Focus();
                               lblCurrentField.Value = "3";
                               lblEmail.Visible = false;
                               txtEmail.CssClass = "inputboxes addressboxwhite";
                               if (IsEmailOptional.ToUpper() == "TRUE" || txtEmail.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 4:
                               txtHouseNo.CssClass = "text";
                               txtHouseNo.Visible = true;
                               txtHouseNo.Focus();
                               lblCurrentField.Value = "4";
                               lblHouseNo.Visible = false;
                               pnlHouseNo.CssClass = "inputboxes addressboxwhite";
                               if (IsHouseNumberOptional.ToUpper() == "TRUE" || txtHouseNo.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 5:
                               txtPostcode.CssClass = "text";
                               txtPostcode.Visible = true;
                               txtPostcode.Focus();
                               lblCurrentField.Value = "5";
                               lblPostcode.Visible = false;
                               pnlPostcode.CssClass = "inputboxes addressboxwhite";
                               if (IsPostCodeOptional.ToUpper() == "TRUE" || txtPostcode.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 6:
                               txtPhoneNumber.CssClass = "text";
                               txtPhoneNumber.Visible = true;
                               txtPhoneNumber.Focus();
                               lblCurrentField.Value = "6";
                               lblPhoneNumber.Visible = false;
                               pnlPhoneNumber.CssClass = "inputboxes addressboxwhite";
                               if (IsPhoneNumberOptional.ToUpper() == "TRUE" || txtPhoneNumber.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 7:
                               txtSSN.CssClass = "text";
                               txtSSN.Visible = true;
                               txtSSN.Focus();
                               lblCurrentField.Value = "7";
                               lblSSN.Visible = false;
                               pnlSSN.CssClass = "inputboxes addressboxwhite";
                               if (IsSSNOptional.ToUpper() == "TRUE" || txtSSN.Text.Trim() != string.Empty)
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                           case 8:
                               string focus_FirstControl = string.Empty, ControlFocus = string.Empty;
                               switch (ConfigurationManager.AppSettings["DateFormat"])
                               {
                                   case "ymd": txtyearymd.Focus(); focus_FirstControl = "txtyearymd"; txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = true;
                                       lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = false;
                                       divmonthymd.Attributes.Add("class", "input92 paddingtop-10");
                                       divDOBymd.Attributes.Add("class", "input92 paddingtop-10");
                                       divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                       break;
                                   case "ydm": txtyearydm.Focus(); focus_FirstControl = "txtyearydm"; txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = true;
                                       lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = false;
                                       divmonthydm.Attributes.Add("class", "input92 paddingtop-10");
                                       divDOBydm.Attributes.Add("class", "input92 paddingtop-10");
                                       divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                       break;
                                   case "mdy": txtmonthmdy.Focus(); focus_FirstControl = "txtmonthmdy"; txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = true;
                                       lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = false;
                                       divmonthmdy.Attributes.Add("class", "input92 paddingtop-10");
                                       divDOBmdy.Attributes.Add("class", "input92 paddingtop-10");
                                       divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                       break;
                                   case "dmy": txtDOBdmy.Focus(); focus_FirstControl = "txtDOBdmy"; txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = true;
                                       lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = false;
                                       divmonthdmy.Attributes.Add("class", "input92 paddingtop-10");
                                       divDOBdmy.Attributes.Add("class", "input92 paddingtop-10");
                                       divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                       break;
                               }
                               lblCurrentField.Value = "8";
                               ClientScript.RegisterStartupScript(this.GetType(), "Focus_FirstControl", "<script language=JavaScript>Focus_FirstControl('" + focus_FirstControl + "');</script>");
                               if (IsDateOfBirthOptional.ToUpper() == "TRUE")
                               {
                                   DisableFirstControl = false;
                               }
                               break;
                       }
                   }
                   else
                   {
                       switch (field)
                       {
                           case 1:
                               txtFirstName.Visible = false;
                               lblFirstName.Text = txtFirstName.Text;
                               lblFirstName.Visible = true;
                               pnlFirstName.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 2:
                               txtLastName.Visible = false;
                               lblLastName.Text = txtLastName.Text;
                               lblLastName.Visible = true;
                               pnlLastName.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 3:
                               txtEmail.Visible = false;
                               lblEmail.Text = txtEmail.Text;
                               lblEmail.Visible = true;
                               pnlEmail.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 4:
                               txtHouseNo.Visible = false;
                               lblHouseNo.Text = txtHouseNo.Text;
                               lblHouseNo.Visible = true;
                               pnlHouseNo.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 5:
                               txtPostcode.Visible = false;
                               lblPostcode.Text = txtPostcode.Text;
                               lblPostcode.Visible = true;
                               pnlPostcode.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 6:
                               txtPhoneNumber.Visible = false;
                               lblPhoneNumber.Text = txtPhoneNumber.Text;
                               lblPhoneNumber.Visible = true;
                               pnlPhoneNumber.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 7:
                               txtSSN.Visible = false;
                               lblSSN.Text = txtSSN.Text;
                               lblSSN.Visible = true;
                               pnlSSN.CssClass = "inputboxes addressboxgrey";
                               break;
                           case 8:
                               string focus_FirstControl = string.Empty, ControlFocus = string.Empty;
                               switch (ConfigurationManager.AppSettings["DateFormat"])
                               {
                                   case "ymd":
                                       txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = false;
                                       lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = true;
                                       divmonthymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divDOBymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                       break;
                                   case "ydm":
                                       txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = false;
                                       lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = true;
                                       divmonthydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divDOBydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                       break;
                                   case "mdy":
                                       txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = false;
                                       lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = true;
                                       divmonthmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divDOBmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                       break;
                                   case "dmy":
                                       txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = false;
                                       lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = true;
                                       divmonthdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divDOBdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                       divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                       break;
                               }
                               break;
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Logger.Write(ex.Message + " Verification EnableField()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
               Response.Redirect("Error.aspx", false);
           }
    
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                showFirstNameField = ConfigurationManager.AppSettings["showFirstNameField"];
                showLastNameField = ConfigurationManager.AppSettings["showLastNameField"];
                showDateOfBirthField = ConfigurationManager.AppSettings["showDateOfBirthField"];
                showHouseNumberField = ConfigurationManager.AppSettings["showHouseNumberField"];
                showPostCodeField = ConfigurationManager.AppSettings["showPostCodeField"];
                IsPostCodeLast3Digit = ConfigurationManager.AppSettings["IsPostCodeLast3Digit"];
                showPhoneNumberField = ConfigurationManager.AppSettings["showPhoneNumberField"];
                showSSNField = ConfigurationManager.AppSettings["showSSNField"];
                showEmailField = ConfigurationManager.AppSettings["showEmailField"];

                IsFirstNameOptional = ConfigurationManager.AppSettings["IsFirstNameOptional"];
                IsLastNameOptional = ConfigurationManager.AppSettings["IsLastNameOptional"];
                IsDateOfBirthOptional = ConfigurationManager.AppSettings["IsDateOfBirthOptional"];
                IsHouseNumberOptional = ConfigurationManager.AppSettings["IsHouseNumberOptional"];
                IsPostCodeOptional = ConfigurationManager.AppSettings["IsPostCodeOptional"];
                IsPhoneNumberOptional = ConfigurationManager.AppSettings["IsPhoneNumberOptional"];
                IsSSNOptional = ConfigurationManager.AppSettings["IsSSNOptional"];
                IsEmailOptional = ConfigurationManager.AppSettings["IsEmailOptional"];

                CheckFieldsVisibility();
                BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();

                if (!IsPostBack)
                {
                   
                     //PV : Set sessinfor start time in BookingPrintVoucher 
                    if (Request.QueryString["Existing"] != null)
                    {
                        if (Request.QueryString["Existing"].ToString().Trim() == "false")
                        {
                            BookingPrintVoucher.Status = 1;
                            BookingPrintVoucher.CouponStatusID = 1;
                            BookingPrintVoucher.StartTime = Convert.ToDateTime(DateTime.Now);
                            clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsVerifiedStartTime);
                        }
                    }

                    if (Request.QueryString["ErrorMsg"] != null)
                    {
                        txtPostcode.Text = BookingPrintVoucher.PostCode;
                        txtHouseNo.Text = BookingPrintVoucher.AddressLine1;

                        txtSSN.Text = BookingPrintVoucher.SSN;

                        switch (ConfigurationManager.AppSettings["DateFormat"])
                        {
                            case "ymd": lblDOBymd.Text = txtDOBymd.Text = Convert.ToString(Session["BirthDay"]);
                                lblmonthymd.Text = txtmonthymd.Text = Convert.ToString(Session["BirthMonth"]);
                                lblyearymd.Text = txtyearymd.Text = Convert.ToString(Session["BirthYear"]);
                                break;
                            case "ydm": lblDOBydm.Text = txtDOBydm.Text = Convert.ToString(Session["BirthDay"]);
                                lblmonthydm.Text = txtmonthydm.Text = Convert.ToString(Session["BirthMonth"]);
                                lblyearydm.Text = txtyearydm.Text = Convert.ToString(Session["BirthYear"]);
                                break;
                            case "mdy": lblDOBmdy.Text = txtDOBmdy.Text = Convert.ToString(Session["BirthDay"]);
                                lblmonthmdy.Text = txtmonthmdy.Text = Convert.ToString(Session["BirthMonth"]);
                                lblyearmdy.Text = txtyearmdy.Text = Convert.ToString(Session["BirthYear"]);
                                break;
                            case "dmy": lblDOBdmy.Text = txtDOBdmy.Text = Convert.ToString(Session["BirthDay"]);
                                lblmonthdmy.Text = txtmonthdmy.Text = Convert.ToString(Session["BirthMonth"]);
                                lblyeardmy.Text = txtyeardmy.Text = Convert.ToString(Session["BirthYear"]);
                                break;
                        }
                       

                        txtFirstName.Text = Convert.ToString(Session["FirstName"]); //BookingPrintVoucher.FirstName;
                        txtLastName.Text = Convert.ToString(Session["LastName"]); //BookingPrintVoucher.Surname;

                        txtEmail.Text = BookingPrintVoucher.Email;
                        txtPhoneNumber.Text = BookingPrintVoucher.MobileNo;

                        if (Request.QueryString["ErrorMsg"] == "InvalidDOB")
                        {
                            EnableField();
                            //disble 1st field
                            int firstField = Convert.ToInt32(lblCurrentField.Value);
                            switch (firstField)
                            {
                                case 1:
                                    txtFirstName.Visible = false;
                                    lblFirstName.Text = txtFirstName.Text;
                                    lblFirstName.Visible = true;
                                    pnlFirstName.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 2:
                                    txtLastName.Visible = false;
                                    lblLastName.Text = txtLastName.Text;
                                    lblLastName.Visible = true;
                                    pnlLastName.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 3:
                                    txtEmail.Visible = false;
                                    lblEmail.Text = txtEmail.Text;
                                    lblEmail.Visible = true;
                                    pnlEmail.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 4:
                                    txtHouseNo.Visible = false;
                                    lblHouseNo.Text = txtHouseNo.Text;
                                    lblHouseNo.Visible = true;
                                    pnlHouseNo.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 5:
                                    txtPostcode.Visible = false;
                                    lblPostcode.Text = txtPostcode.Text;
                                    lblPostcode.Visible = true;
                                    pnlPostcode.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 6:
                                    txtPhoneNumber.Visible = false;
                                    lblPhoneNumber.Text = txtPhoneNumber.Text;
                                    lblPhoneNumber.Visible = true;
                                    pnlPhoneNumber.CssClass = "inputboxes addressboxgrey";
                                    break;
                                case 7:
                                    txtSSN.Visible = false;
                                    lblSSN.Text = txtSSN.Text;
                                    lblSSN.Visible = true;
                                    pnlSSN.CssClass = "inputboxes addressboxgrey";
                                    break;
                            }
                            //Enable DOB

                            string focus_FirstControl = string.Empty, ControlFocus = string.Empty;
                            switch (ConfigurationManager.AppSettings["DateFormat"])
                            {
                                case "ymd": txtyearymd.Focus(); focus_FirstControl = "txtyearymd"; txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = true;
                                    lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = false;
                                    divmonthymd.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBymd.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116white");

                                    break;
                                case "ydm": txtyearydm.Focus(); focus_FirstControl = "txtyearydm"; txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = true;
                                    lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = false;
                                    divmonthydm.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBydm.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                                case "mdy": txtmonthmdy.Focus(); focus_FirstControl = "txtmonthmdy"; txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = true;
                                    lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = false;
                                    divmonthmdy.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBmdy.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                                case "dmy": txtDOBdmy.Focus(); focus_FirstControl = "txtDOBdmy"; txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = true;
                                    lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = false;
                                    divmonthdmy.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBdmy.Attributes.Add("class", "input92 paddingtop-10");
                                    divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                            }
                            lblCurrentField.Value = "8";
                            ClientScript.RegisterStartupScript(this.GetType(), "Focus_FirstControl", "<script language=JavaScript>Focus_FirstControl('" + focus_FirstControl + "');</script>");
                            spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("confirm"));

                            if (showDateOfBirthField.ToLower() == "true" && IsDateOfBirthOptional.ToString().ToLower() == "true")
                            {
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                            }
                            return;
                        }
                    }
                    EnableField();
                    if (ArrSelFields.Length == 2)
                    {
                        spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("confirm"));
                    }
                    else
                    {
                        spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("next"));//"NEXT";
                    }
                }
                if (DisableFirstControl)
                {
                    lnkConfirm.Attributes["disabled"] = "true";
                    pnlConfirm.CssClass = "confirm inactive";
                }
                else
                {
                    lnkConfirm.Attributes.Remove("disabled");
                    pnlConfirm.CssClass = "confirm";
                }
              
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Verification Page_Load()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkConfirm control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
          protected void lnkConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string confirm = Convert.ToString(GetLocalResourceObject("confirm"));
                string next = Convert.ToString(GetLocalResourceObject("next"));
                string strDOB, strMonth, strYear;
                strDOB = strMonth = strYear = string.Empty;
                DataRow []dataRow=null;
                string regDate = @"^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$";
        
                #region Confirm

                if (spanNEXT.InnerText == confirm)
                {
                    Logger.Write(" Start of Verification lnkConfirm_Click()", "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);
                    BigExchange.ClubcardRewardClient clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                    BookingPrintVoucher.StatusLoginAttempts = "FAIL";//Default
                    try
                    {
                        Net35BasicAuthentication();
                        string conditionXml = string.Format("<?xml version='1.0' encoding='utf-16'?><customer><CustomerID>0</CustomerID><cardAccountNumber>{0}</cardAccountNumber></customer>", long.Parse(BookingPrintVoucher.Clubcard));
                        int maxRowCount = 1;
                        string culture = ConfigurationManager.AppSettings["CountryCode"];
                        bool result = false;
                        string errorXML = string.Empty;
                        string resultXml = string.Empty;
                        int rowCount = 0;
                        string FirstName = string.Empty;
                        string LastName = string.Empty;
                        string ClubcardID = string.Empty;
                        bool IsOverAllStatus = false;
                        DateTime dob;
                        try
                        {
                            Logger.Write(" Start of Verification lnkConfirm_Click() -SearchCustomer()", "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);
                            this.customerServiceClient = new CustomerServiceClient();
                            result = customerServiceClient.SearchCustomer(out errorXML, out resultXml, out rowCount, conditionXml, maxRowCount, culture);
                            Logger.Write(" End of Verification lnkConfirm_Click() -SearchCustomer()", "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);
                        }
                        catch (Exception ex)
                        {
                            //clubcardRewardClient.Logging(BookingPrintVoucher, "NGC ClubcardService Exception :" + ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);//(NGC error)//removed table error logging
                            Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                            Server.Transfer("MessageForAddress.aspx?ErrorMsg=ClubcardService");
                        }


                        try
                        {
                            switch (ConfigurationManager.AppSettings["DateFormat"])
                            {
                                case "ymd": strDOB = txtDOBymd.Text; strMonth = txtmonthymd.Text; strYear = txtyearymd.Text;
                                    break;
                                case "ydm": strDOB = txtDOBydm.Text; strMonth = txtmonthydm.Text; strYear = txtyearydm.Text;
                                    break;
                                case "mdy": strDOB = txtDOBmdy.Text; strMonth = txtmonthmdy.Text; strYear = txtyearmdy.Text;
                                    break;
                                case "dmy": strDOB = txtDOBdmy.Text; strMonth = txtmonthdmy.Text; strYear = txtyeardmy.Text;
                                    break;
                            }

                            //Optional Field Check
                            if ((showFirstNameField.ToLower() == "true" && IsFirstNameOptional.ToLower() == "true") && txtFirstName.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='Name1'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ((showLastNameField.ToLower() == "true" && IsLastNameOptional.ToLower() == "true") && txtLastName.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='Name3'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ((showEmailField.ToLower() == "true" && IsEmailOptional.ToLower() == "true") && txtEmail.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='EmailAddress'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ((showHouseNumberField.ToLower() == "true" && IsHouseNumberOptional.ToLower() == "true") && txtHouseNo.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='MailingAddressLine1'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ((showPostCodeField.ToLower() == "true" && IsPostCodeOptional.ToLower() == "true") && txtPostcode.Text.Trim() == string.Empty)
                            {
                                if (IsPostCodeLast3Digit.ToLower() == "true")
                                    dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='MailingAddressPostCode3Digits'");
                                else
                                    dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='MailingAddressPostCode'");

                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ((showPhoneNumberField.ToLower() == "true" && IsPhoneNumberOptional.ToLower() == "true") && txtPhoneNumber.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='MobilePhoneNumber'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if ( (showSSNField.ToLower() == "true" && IsSSNOptional.ToLower() == "true") && txtSSN.Text.Trim() == string.Empty)
                            {
                                dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='SSN'");
                                dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                            }
                            if (showDateOfBirthField.ToLower() == "true" && IsDateOfBirthOptional.ToLower() == "true")
                            {
                                if ((Convert.ToString(strDOB.Trim()) == string.Empty || Convert.ToString(strDOB.Trim()).ToUpper() == defaultDayValue)
                                    || (Convert.ToString(strMonth.Trim()) == string.Empty || Convert.ToString(strMonth.Trim()).ToUpper() == defaultMonthValue)
                                    || (Convert.ToString(strYear.Trim()) == string.Empty || Convert.ToString(strYear.Trim()).ToUpper() == defaultYearValue))
                                {
                                    dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='DayofBirth'");
                                    dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);

                                    dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='MonthofBirth'");
                                    dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);

                                    dataRow = dsCustomerData.Tables["Customer"].Select("ConfigurationName='YearofBirth'");
                                    dsCustomerData.Tables["Customer"].Rows.Remove(dataRow[0]);
                                }
                            }
                            

                            //set the NGC ClubcardCard activation service object
                            ClubcardCustomer clubCardCustomer = new ClubcardCustomer();
                            clubCardCustomer.Address = new Address();
                            clubCardCustomer.ContactDetail = new ContactDetail();
                            clubCardCustomer.Address.AddressLine1 = Convert.ToString(txtHouseNo.Text.Trim());
                            clubCardCustomer.Address.PostCode = Convert.ToString(txtPostcode.Text.Trim());
                            clubCardCustomer.FirstName = Convert.ToString(txtFirstName.Text.Trim());
                            clubCardCustomer.Surname = Convert.ToString(txtLastName.Text.Trim());
                            clubCardCustomer.SSN = Convert.ToString(txtSSN.Text.Trim());
                            clubCardCustomer.ContactDetail.MobileContactNumber = Convert.ToString(txtPhoneNumber.Text.Trim());
                            clubCardCustomer.ContactDetail.EmailAddress = Convert.ToString(txtEmail.Text.Trim());

                            
                            //set BookingPrintVoucher with customer entered credentials
                            XElement xe = XElement.Parse(resultXml);

                            var FirstNames = (from c in xe.Descendants("Customer")
                                              select (string)c.Element("Name1")).Take(1);

                            var Surnames = (from c in xe.Descendants("Customer")
                                            select (string)c.Element("Name3")).Take(1);

                            var ClubcardIDs = (from c in xe.Descendants("Customer")

                                               select (long)c.Element("ClubcardID")).Take(1);

                            foreach (var id in FirstNames)
                            {
                                BookingPrintVoucher.FirstName = id;
                                FirstName = id;
                                continue;
                            }

                            foreach (var id in Surnames)
                            {
                                BookingPrintVoucher.Surname = id;
                                LastName = id;
                                continue;
                            }

                            foreach (var id in ClubcardIDs)
                            {
                                ClubcardID = Convert.ToString(id);
                                continue;
                            }

                            Session["FirstName"] = Convert.ToString(txtFirstName.Text.Trim());
                            Session["LastName"] = Convert.ToString(txtLastName.Text.Trim());
                            Session["BirthDay"] = strDOB.Trim();
                            Session["BirthMonth"] = strMonth.Trim();
                            Session["BirthYear"] = strYear.Trim();
                            BookingPrintVoucher.AddressLine1 = Convert.ToString(txtHouseNo.Text.Trim());
                            BookingPrintVoucher.PostCode = Convert.ToString(txtPostcode.Text.Trim());
                            BookingPrintVoucher.Email = Convert.ToString(txtEmail.Text.Trim());
                            BookingPrintVoucher.SSN = Convert.ToString(txtSSN.Text.Trim());
                            BookingPrintVoucher.MobileNo = Convert.ToString(txtPhoneNumber.Text.Trim());

                            try
                            {
                                //ChangeDATE
                                if ((Convert.ToString(strDOB.Trim()) != string.Empty && Convert.ToString(strDOB.Trim()).ToUpper() != defaultDayValue)
                                    && (Convert.ToString(strMonth.Trim()) != string.Empty && Convert.ToString(strMonth.Trim()).ToUpper() != defaultMonthValue)
                                    && (Convert.ToString(strYear.Trim()) != string.Empty && Convert.ToString(strYear.Trim()).ToUpper() != defaultYearValue))
                                {
                                    if (strDOB.Trim().Length < 2)
                                    {
                                        strDOB = "0" + strDOB.Trim();
                                    }
                                    if (strMonth.Trim().Length < 2)
                                    {
                                        strMonth = "0" + strMonth.Trim();
                                    } 
                                    string sDate=strDOB+ "/" + strMonth + "/" + strYear.Trim();
                                    if (!(DateTime.TryParse(sDate, out dob)) || !(Helper.IsRegexMatch(sDate.ToString().Trim(), regDate, true, true)))
                                    {
                                         Server.Transfer("MessageForAddress.aspx?ErrorMsg=InvalidDOB");
                                    }
                                    BookingPrintVoucher.DOB = dob;
                                    clubCardCustomer.DayOfBirth = dob.Day.ToString();
                                    clubCardCustomer.MonthOfBirth = dob.Month.ToString();
                                    clubCardCustomer.YearOfBirth = dob.Year.ToString();
                                }
                                else
                                {
                                    BookingPrintVoucher.DOB = null;
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                //clubcardRewardClient.Logging(BookingPrintVoucher, "NGC ActivationService Exception :" + ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);//Removed old table logging mechanism
                                Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                                // Response.Redirect("Error.aspx", false);
                                Server.Transfer("MessageForAddress.aspx?ErrorMsg=InvalidDOB");
                            }



                            long ClubcardNo = long.Parse(BookingPrintVoucher.Clubcard);
                            Logger.Write(" Start of Verification lnkConfirm_Click() -AccountFindByClubcardNumber()", "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);
                            clubcardOnlineServiceClient = new ClubcardOnlineServiceClient();
                            AccountFindByClubcardNumberResponse AccountFindByClubcardNumberResponse = new ClubcardActivationService.AccountFindByClubcardNumberResponse();
                            //Call NGC activation service
                            AccountFindByClubcardNumberResponse = clubcardOnlineServiceClient.AccountFindByClubcardNumber(ClubcardNo, clubCardCustomer, dsCustomerData);
                            //customer has keyed in invalid data
                            if (AccountFindByClubcardNumberResponse == null || AccountFindByClubcardNumberResponse.ContactDetailMatchStatus.ToUpper() == "N" || AccountFindByClubcardNumberResponse.ContactDetailMatchStatus == String.Empty)
                            {
                                IsOverAllStatus = false;
                            }//customer has keyed in valid data
                            else if (AccountFindByClubcardNumberResponse != null && AccountFindByClubcardNumberResponse.ContactDetailMatchStatus == "Y")
                            {
                                IsOverAllStatus = true;
                            }
                            Logger.Write(" End of Verification lnkConfirm_Click() -AccountFindByClubcardNumber() ContactDetailMatchStatus: " + AccountFindByClubcardNumberResponse.ContactDetailMatchStatus, "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);

                        }
                        catch (Exception ex)
                        {
                            //clubcardRewardClient.Logging(BookingPrintVoucher, "NGC ActivationService Exception :" + ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);//Removed old table logging mechanism
                            Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                           // Response.Redirect("Error.aspx", false);
                            Server.Transfer("MessageForAddress.aspx?ErrorMsg=ActivationService");
                        }

                       
                        if (ClubcardID != "")
                        {

                            if (IsOverAllStatus)
                            {
                                try
                                {
                                    BookingPrintVoucher.StatusLoginAttempts = "PASS";
                                    clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.InsertLoginAttempts);

                                    //BookingPrintVoucher.EndTime = Convert.ToDateTime(DateTime.Now);
                                    //BookingPrintVoucher.Status = 1;
                                    //BookingPrintVoucher.CouponStatusID = 1;
                                    //clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsVerifiedTime);
                                    Response.Redirect("SelectAllVouchers.aspx", false);
                                }
                                catch (Exception ex)
                                {
                                   // clubcardRewardClient.Logging(BookingPrintVoucher, "PrintVoucherDB Exception :" + ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);//(Database error)//removed table error logging
                                    Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                                }
                            }
                            else
                            {
                                //Not matched contact details
                                BookingPrintVoucher.StatusLoginAttempts = "FAIL";//Default
                                clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.InsertLoginAttempts);
                                Response.Redirect("MessageForAddress.aspx?FinalPage=InValid", false);
                            }
                        }
                        else
                        {
                            //Not matched clubcard
                            BookingPrintVoucher.StatusLoginAttempts = "FAIL";
                            clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.InsertLoginAttempts);
                            Response.Redirect("MessageForAddress.aspx?FinalPage=InValid", false);

                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                        Response.Redirect("Error.aspx", false);
                    }
                    finally
                    {
                        if (this.customerServiceClient != null)
                        {
                            if (this.customerServiceClient.State == CommunicationState.Faulted)
                            {
                                this.customerServiceClient.Abort();
                            }
                            else if (this.customerServiceClient.State != CommunicationState.Closed)
                            {
                                this.customerServiceClient.Close();
                            }
                        }

                        if (this.clubcardOnlineServiceClient != null)
                        {
                            if (this.clubcardOnlineServiceClient.State == CommunicationState.Faulted)
                            {
                                this.clubcardOnlineServiceClient.Abort();
                            }
                            else if (this.clubcardOnlineServiceClient.State != CommunicationState.Closed)
                            {
                                this.clubcardOnlineServiceClient.Close();
                            }
                        }
                    }
                    Logger.Write(" End of Verification lnkConfirm_Click()", "Information", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Information);
                }
                #endregion
                else if (spanNEXT.InnerText == next)
                {
                    int currTextBox = System.Convert.ToInt32(lblCurrentField.Value);
                    int NextField = 0;
                    int CurrIndex = 0;

                    for (int i = 0; i <= ArrSelFields.Length - 2; i++)
                    {
                        if (currTextBox == Convert.ToInt32(ArrSelFields[i]))
                        {
                            CurrIndex = i;
                            NextField = Convert.ToInt32(ArrSelFields[i + 1]);
                            break;
                        }
                    }

                    if (CurrIndex != ArrSelFields.Length - 2)
                    {
                        spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("next"));

                        switch (currTextBox)
                        {

                            case 1:
                                txtFirstName.Visible = false;
                                lblFirstName.Text = txtFirstName.Text;
                                lblFirstName.Visible = true;
                                pnlFirstName.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 2:
                                txtLastName.Visible = false;
                                lblLastName.Text = txtLastName.Text;
                                lblLastName.Visible = true;
                                pnlLastName.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 3:
                                txtEmail.Visible = false;
                                lblEmail.Text = txtEmail.Text;
                                lblEmail.Visible = true;
                                pnlEmail.CssClass = "inputboxes addressboxgrey";
                                break;

                            case 4:
                                txtHouseNo.Visible = false;
                                lblHouseNo.Text = txtHouseNo.Text;
                                lblHouseNo.Visible = true;
                                pnlHouseNo.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 5:
                                txtPostcode.Visible = false;
                                lblPostcode.Text = txtPostcode.Text;
                                lblPostcode.Visible = true;
                                pnlPostcode.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 6:
                                txtPhoneNumber.Visible = false;
                                lblPhoneNumber.Text = txtPhoneNumber.Text;
                                lblPhoneNumber.Visible = true;
                                pnlPhoneNumber.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 7:
                                txtSSN.Visible = false;
                                lblSSN.Text = txtSSN.Text;
                                lblSSN.Visible = true;
                                pnlSSN.CssClass = "inputboxes addressboxgrey";
                                break;
                            case 8:
                               
                                string focus_FirstControl = string.Empty, ControlFocus=string.Empty;
                                switch (ConfigurationManager.AppSettings["DateFormat"])
                                {
                                    case "ymd": 
                                        txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = false;
                                        lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = true;
                                        lblyearymd.Text = txtyearymd.Text; lblmonthymd.Text =txtmonthymd.Text; lblDOBymd.Text = txtDOBymd.Text;
                                        divmonthymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divDOBymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116grey");

                                        break;
                                    case "ydm": 
                                        txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = false;
                                        lblyearydm.Text = txtyearydm.Text; lblmonthydm.Text = txtmonthydm.Text; lblDOBydm.Text = txtDOBydm.Text;
                                        lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = true;
                                        divmonthydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divDOBydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                        break;
                                    case "mdy": 
                                        txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = false;
                                        lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = true;
                                        lblyearmdy.Text = txtyearmdy.Text; lblmonthmdy.Text = txtmonthmdy.Text; lblDOBmdy.Text = txtDOBmdy.Text;
                                        divmonthmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divDOBmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                        break;
                                    case "dmy": 
                                        txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = false;
                                        lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = true;
                                        lblyeardmy.Text = txtyeardmy.Text; lblmonthdmy.Text = txtmonthdmy.Text; lblDOBdmy.Text = txtDOBdmy.Text;
                                        divmonthdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divDOBdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                        divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                        break;
                                }
                                break;

                        }
                        switch (NextField)
                        {
                            case 1:
                                txtFirstName.CssClass = "text";
                                txtFirstName.Visible = true;
                                txtFirstName.Focus();
                                lblCurrentField.Value = "1";
                                lblFirstName.Visible = false;
                                pnlFirstName.CssClass = "inputboxes addressboxwhite";
                                if ((showFirstNameField.ToLower() == "true" && IsFirstNameOptional.ToString().ToLower() == "true")|| txtFirstName.Text.Trim() != string.Empty)
                                {
                                    lnkConfirm.Attributes.Remove("disabled");
                                    pnlConfirm.CssClass="confirm";
                                }
                                break;
                            case 2:
                                txtLastName.CssClass = "text";
                                txtLastName.Focus();
                                lblCurrentField.Value = "2";
                                txtLastName.Visible = true;
                                lblLastName.Visible = false;
                                pnlLastName.CssClass = "inputboxes addressboxwhite";
                                if ((showLastNameField.ToLower() == "true" && IsLastNameOptional.ToString().ToLower() == "true") || txtLastName.Text.Trim() != string.Empty)
                                {
                                    lnkConfirm.Attributes.Remove("disabled");
                                    pnlConfirm.CssClass = "confirm"; 
                                }
                                break;
                            case 3:
                                txtEmail.CssClass = "text";
                                txtEmail.Visible = true;
                                txtEmail.Focus();
                                lblCurrentField.Value = "3";
                                lblEmail.Visible = false;
                                pnlEmail.CssClass = "inputboxes addressboxwhite";
                                if ((showEmailField.ToLower() == "true" && IsEmailOptional.ToString().ToLower() == "true" )|| txtEmail.Text.Trim() != string.Empty)
                                {
                                    
                                    lnkConfirm.Attributes.Remove("disabled");
                                   pnlConfirm.CssClass = "confirm"; 
                                    
                                }
                                break;
                            case 4:
                                txtHouseNo.CssClass = "text";
                                txtHouseNo.Visible = true;
                                txtHouseNo.Focus();
                                lblCurrentField.Value = "4";
                                lblHouseNo.Visible = false;
                                pnlHouseNo.CssClass = "inputboxes addressboxwhite";
                                if ((showHouseNumberField.ToLower() == "true" && IsHouseNumberOptional.ToString().ToLower() == "true") || txtHouseNo.Text.Trim() != string.Empty)
                                {
                                    
                                    lnkConfirm.Attributes.Remove("disabled");
                                    pnlConfirm.CssClass = "confirm"; 
                                    
                                }
                                break;
                            case 5:
                                txtPostcode.CssClass = "text";
                                txtPostcode.Visible = true;
                                txtPostcode.Focus();
                                lblCurrentField.Value = "5";
                                lblPostcode.Visible = false;
                                pnlPostcode.CssClass = "inputboxes addressboxwhite";
                                if ((showPostCodeField.ToLower() == "true" && IsPostCodeOptional.ToString().ToLower() == "true") || txtPostcode.Text.Trim() != string.Empty)
                                {
                                    
                                    lnkConfirm.Attributes.Remove("disabled");
                                   pnlConfirm.CssClass = "confirm";
                                    
                                }
                                break;
                            case 6:
                                txtPhoneNumber.CssClass = "text";
                                txtPhoneNumber.Visible = true;
                                txtPhoneNumber.Focus();
                                lblCurrentField.Value = "6";
                                lblPhoneNumber.Visible = false;
                                pnlPhoneNumber.CssClass = "inputboxes addressboxwhite";
                                if ((showPhoneNumberField.ToLower() == "true" && IsPhoneNumberOptional.ToString().ToLower() == "true") || txtPhoneNumber.Text.Trim() != string.Empty)
                                {
                                    
                                    lnkConfirm.Attributes.Remove("disabled");
                                   pnlConfirm.CssClass = "confirm";
                                    
                                }
                                break;
                            case 7:
                                txtSSN.CssClass = "text";
                                txtSSN.Visible = true;
                                txtSSN.Focus();
                                lblCurrentField.Value = "7";
                                lblSSN.Visible = false;
                                pnlSSN.CssClass = "inputboxes addressboxwhite";
                                if ((showSSNField.ToLower() == "true" && IsSSNOptional.ToString().ToLower() == "true") || txtSSN.Text.Trim() != string.Empty)
                                {
                                    
                                    lnkConfirm.Attributes.Remove("disabled");
                                    pnlConfirm.CssClass = "confirm";
                                    
                                }
                                break;
                            case 8:

                                string focus_FirstControl = string.Empty, ControlFocus = string.Empty;
                                switch (ConfigurationManager.AppSettings["DateFormat"])
                                {
                                    case "ymd": txtyearymd.Focus(); focus_FirstControl = "txtyearymd"; txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = true;
                                        lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = false;
                                        divmonthymd.Attributes.Add("class", "input92 paddingtop-10");
                                        divDOBymd.Attributes.Add("class", "input92 paddingtop-10");
                                        divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116white");

                                        break;
                                    case "ydm": txtyearydm.Focus(); focus_FirstControl = "txtyearydm"; txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = true;
                                        lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = false;
                                        divmonthydm.Attributes.Add("class", "input92 paddingtop-10");
                                        divDOBydm.Attributes.Add("class", "input92 paddingtop-10");
                                        divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                        break;
                                    case "mdy": txtmonthmdy.Focus(); focus_FirstControl = "txtmonthmdy"; txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = true;
                                        lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = false;
                                        divmonthmdy.Attributes.Add("class", "input92 paddingtop-10");
                                        divDOBmdy.Attributes.Add("class", "input92 paddingtop-10");
                                        divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                        break;
                                    case "dmy": txtDOBdmy.Focus(); focus_FirstControl = "txtDOBdmy"; txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = true;
                                        lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = false;
                                        divmonthdmy.Attributes.Add("class", "input92 paddingtop-10");
                                        divDOBdmy.Attributes.Add("class", "input92 paddingtop-10");
                                        divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                        break;
                                }
                                lblCurrentField.Value = "8";
                                ClientScript.RegisterStartupScript(this.GetType(), "Focus_FirstControl", "<script language=JavaScript>Focus_FirstControl('" + focus_FirstControl + "');</script>");


                                if (showDateOfBirthField.ToLower() == "true" && IsDateOfBirthOptional.ToString().ToLower() == "true")
                                {
                                    lnkConfirm.Attributes.Remove("disabled");
                                    pnlConfirm.CssClass = "confirm";
                                }
                                
                                break;

                        }
                        if (CurrIndex + 1 == ArrSelFields.Length - 2)
                        {
                            spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("confirm"));
                        }

                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Verification lnkConfirm_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            } 
        }
        /// <summary>
        /// Handles the Click event of the lnkBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkBack_Click(object sender, EventArgs e)
        {
            int currTextBox = 0;
            int firstField = 0;
            int prevField = 0;
            try
            {
                currTextBox = System.Convert.ToInt32(lblCurrentField.Value);
                firstField = System.Convert.ToInt32(ArrSelFields[0]);
                for (int i = 0; i <= ArrSelFields.Length - 2; i++)
                {
                    if (currTextBox == Convert.ToInt32(ArrSelFields[i]))
                    {
                        if (i != 0)
                        {
                            prevField = Convert.ToInt32(ArrSelFields[i - 1]);
                            break;
                        }
                    }
                }

                spanNEXT.InnerText = Convert.ToString(GetLocalResourceObject("next"));
                if (currTextBox != firstField)
                {

                    switch (currTextBox)
                    {
                        case 1:
                            txtFirstName.Visible = false;
                            lblFirstName.Text = txtFirstName.Text;
                            lblFirstName.Visible = true;
                            pnlFirstName.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 2:
                            txtLastName.Visible = false;
                            lblLastName.Text = txtLastName.Text;
                            lblLastName.Visible = true;
                            pnlLastName.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 3:
                            txtEmail.Visible = false;
                            lblEmail.Text = txtEmail.Text;
                            lblEmail.Visible = true;
                            pnlEmail.CssClass = "inputboxes addressboxgrey";
                            break;

                        case 4:
                            txtHouseNo.Visible = false;
                            lblHouseNo.Text = txtHouseNo.Text;
                            lblHouseNo.Visible = true;
                            pnlHouseNo.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 5:
                            txtPostcode.Visible = false;
                            lblPostcode.Text = txtPostcode.Text;
                            lblPostcode.Visible = true;
                            pnlPostcode.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 6:
                            txtPhoneNumber.Visible = false;
                            lblPhoneNumber.Text = txtPhoneNumber.Text;
                            lblPhoneNumber.Visible = true;
                            pnlPhoneNumber.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 7:
                            txtSSN.Visible = false;
                            lblSSN.Text = txtSSN.Text;
                            lblSSN.Visible = true;
                            pnlSSN.CssClass = "inputboxes addressboxgrey";
                            break;
                        case 8:
                            
                            string focus_FirstControl = string.Empty, ControlFocus = string.Empty;
                            switch (ConfigurationManager.AppSettings["DateFormat"])
                            {
                                case "ymd":
                                    txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = false;
                                    lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = true;
                                    lblyearymd.Text = txtyearymd.Text; lblmonthymd.Text = txtmonthymd.Text; lblDOBymd.Text = txtDOBymd.Text;
                                    divmonthymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divDOBymd.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116grey");

                                    break;
                                case "ydm": 
                                    txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = false;
                                    lblyearydm.Text = txtyearydm.Text; lblmonthydm.Text = txtmonthydm.Text; lblDOBydm.Text =txtDOBydm.Text;
                                    lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = true;
                                    divmonthydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divDOBydm.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                    break;
                                case "mdy": 
                                    txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = false;
                                    lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = true;
                                    lblyearmdy.Text = txtyearmdy.Text; lblmonthmdy.Text = txtmonthmdy.Text; lblDOBmdy.Text = txtDOBmdy.Text;
                                    divmonthmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divDOBmdy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                    break;
                                case "dmy": 
                                    txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = false;
                                    lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = true;
                                    lblyeardmy.Text = txtyeardmy.Text; lblmonthdmy.Text = txtmonthdmy.Text; lblDOBdmy.Text = txtDOBdmy.Text; 
                                    divmonthdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divDOBdmy.Attributes.Add("class", "input92 paddingtop-10 input92grey");
                                    divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116grey");
                                    break;
                            }
                            break;
                    }
                    switch (prevField)
                    {
                        case 1:
                            txtFirstName.CssClass = "text";
                            txtFirstName.Visible = true;
                            txtFirstName.Focus();
                            lblCurrentField.Value="1";
                            lblFirstName.Visible = false;
                            pnlFirstName.CssClass = "inputboxes addressboxwhite";
                            if (IsFirstNameOptional.ToString().ToLower() == "true" || txtFirstName.Text.Trim() != string.Empty)
                            {

                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 2:
                            txtLastName.CssClass = "text";
                            txtLastName.Visible = true;
                            txtLastName.Focus();
                            lblCurrentField.Value = "2";
                            lblLastName.Visible = false;
                            pnlLastName.CssClass = "inputboxes addressboxwhite";
                            if (IsLastNameOptional.ToString().ToLower() == "true" || txtLastName.Text.Trim() != string.Empty)
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 3:
                            txtEmail.CssClass = "text";
                            txtEmail.Visible = true;
                            txtEmail.Focus();
                            lblCurrentField.Value = "3";
                            lblEmail.Visible = false;
                            pnlEmail.CssClass = "inputboxes addressboxwhite";
                            if (IsEmailOptional.ToString().ToLower() == "true" || txtEmail.Text.Trim() != string.Empty)
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                               pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 4:
                            txtHouseNo.CssClass = "text";
                            txtHouseNo.Visible = true;
                            txtHouseNo.Focus();
                            lblCurrentField.Value = "4";
                            lblHouseNo.Visible = false;
                            pnlHouseNo.CssClass = "inputboxes addressboxwhite";
                            if (IsHouseNumberOptional.ToString().ToLower() == "true" || txtHouseNo.Text.Trim() != string.Empty)
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 5:
                            txtPostcode.CssClass = "text";
                            txtPostcode.Visible = true;
                            txtPostcode.Focus();
                            lblCurrentField.Value = "5";
                            lblPostcode.Visible = false;
                            pnlPostcode.CssClass = "inputboxes addressboxwhite";
                            if (IsPostCodeOptional.ToString().ToLower() == "true" || txtPostcode.Text.Trim() != string.Empty)
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 6:
                            txtPhoneNumber.CssClass = "text";
                            txtPhoneNumber.Visible = true;
                            txtPhoneNumber.Focus();
                            lblCurrentField.Value = "6";
                            lblPhoneNumber.Visible = false;
                            pnlPhoneNumber.CssClass = "inputboxes addressboxwhite";
                            if (IsPhoneNumberOptional.ToString().ToLower() == "true" || txtPhoneNumber.Text.Trim() != string.Empty)
                            {
                               pnlConfirm.CssClass = "confirm";
                               lnkConfirm.Attributes.Remove("disabled");
                            }
                            break;
                        case 7:
                            txtSSN.CssClass = "text";
                            txtSSN.Visible = true;
                            txtSSN.Focus();
                            lblCurrentField.Value = "7";
                            lblSSN.Visible = false;
                            pnlSSN.CssClass = "inputboxes addressboxwhite";
                            if (IsSSNOptional.ToString().ToLower() == "true" || txtSSN.Text.Trim() != string.Empty)
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                        case 8:
                            
                            string focus_FirstControl = string.Empty, ControlFocus=string.Empty;
                            lblCurrentField.Value = "8";
                            switch (ConfigurationManager.AppSettings["DateFormat"])
                            {
                                case "ymd": txtyearymd.Focus(); focus_FirstControl = "txtyearymd"; txtDOBymd.Visible = txtyearymd.Visible = txtmonthymd.Visible = true;
                                    lblDOBymd.Visible = lblmonthymd.Visible = lblyearymd.Visible = false; 
                                    divmonthymd.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBymd.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearymd.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                                case "ydm": txtyearydm.Focus(); focus_FirstControl = "txtyearydm"; txtDOBydm.Visible = txtyearydm.Visible = txtmonthydm.Visible = true;
                                    lblDOBydm.Visible = lblmonthydm.Visible = lblyearydm.Visible = false;
                                    divmonthydm.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBydm.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearydm.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                                case "mdy": txtmonthmdy.Focus(); focus_FirstControl = "txtmonthmdy"; txtDOBmdy.Visible = txtyearmdy.Visible = txtmonthmdy.Visible = true;
                                    lblDOBmdy.Visible = lblmonthmdy.Visible = lblyearmdy.Visible = false;
                                    divmonthmdy.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBmdy.Attributes.Add("class", "input92 paddingtop-10");
                                    divyearmdy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                                case "dmy": txtDOBdmy.Focus(); focus_FirstControl = "txtDOBdmy"; txtDOBdmy.Visible = txtyeardmy.Visible = txtmonthdmy.Visible = true;
                                    lblDOBdmy.Visible = lblmonthdmy.Visible = lblyeardmy.Visible = false;
                                    divmonthdmy.Attributes.Add("class", "input92 paddingtop-10");
                                    divDOBdmy.Attributes.Add("class", "input92 paddingtop-10");
                                    divyeardmy.Attributes.Add("class", "input92 paddingtop-10 input116white");
                                    break;
                            }

                            ClientScript.RegisterStartupScript(this.GetType(), "Focus_FirstControl", "<script language=JavaScript>Focus_FirstControl('" + focus_FirstControl + "');</script>");
                            if (IsDateOfBirthOptional.ToString().ToLower() == "true")
                            {
                                
                                lnkConfirm.Attributes.Remove("disabled");
                                pnlConfirm.CssClass = "confirm";
                                
                            }
                            break;
                    }
                }
                else if (currTextBox == firstField)
                {
                    Response.Redirect("Default.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Verification lnkBack_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
  
        /// <summary>
        /// Handles the Click event of the lnkCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            BigExchange.ClubcardRewardClient clubcardRewardClient = null;
            try
            {
                 clubcardRewardClient = new BigExchange.ClubcardRewardClient();
                //PV Update Transaction table and set status to Cancel and Timetaken(Currenttime - session starttime)
                BookingPrintVoucher.EndTime = Convert.ToDateTime(DateTime.Now);
                BookingPrintVoucher.Status = 2; //Cancelled FROM VERIFICATION
                BookingPrintVoucher.CouponStatusID = 2;
                if (BookingPrintVoucher.TransactionID > 0)
                {
                    clubcardRewardClient.Logging(BookingPrintVoucher, "", BigExchange.LoggingOperations.UpdateTranDetailsVerifiedTime);
                }
            }
            catch (Exception ex)
            {
                BookingPrintVoucher.Status = 5;//Error 
                BookingPrintVoucher.CouponStatusID = 5;
                //clubcardRewardClient.Logging(BookingPrintVoucher, ex.ToString(), BigExchange.LoggingOperations.SaveToTransError);//removed table error logging
                Logger.Write(ex.Message + " Verification lnkBack_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
            }
            finally
            {
                Response.Redirect("CancelAndRestart.aspx", false);
            }

        }
        
        private static void Net35BasicAuthentication()
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static bool customXertificateValidation(Object sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                        System.Security.Cryptography.X509Certificates.X509Chain chain,
                        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //try
            //{
            //    switch (certificate.Subject)
            //    {
            //        case "CN=ClubcardWebServices.ops.ukroi.tesco.org, OU=Tesco Stores Ltd, O=Tesco PLC, L=Cheshunt, C=GB":
            //            return true;
            //        case "CN=ClubcardWebServices.prd.ukroi.tesco.org, OU=Tesco Stores Ltd, O=Tesco PLC, L=Cheshunt, C=GB":
            //            return true;
            //        case "CN=b2bprod.ocset.net, OU=TESCO, O=Tesco PLC, L=Cheshunt, C=GB":
            //            return true;
            //        case "CN=Clubcardwebservices.ukroi.tesco.org, OU=tesco.com, O=Tesco PLC, L=Cheshunt, C=GB":
            //            return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return true;
        }

        /// <summary>
        /// Handles the Click event of the lnkTerms control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void lnkTerms_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("TermsAndCondition.aspx?page=Verification.aspx",false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex.Message + " Verification lnkTerms_Click()", "Critical", 1, ApplicationConstants.EventId, System.Diagnostics.TraceEventType.Error);
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}