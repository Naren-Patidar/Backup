using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Tesco.com.ClubcardOnline.Entities;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Tesco.com.ClubcardOnline.WebService.Messages;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Net.Mail;

namespace Tesco.com.ClubcardOnline.WebService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class ClubcardOnlineService : IClubcardOnlineService
    {
        string connection = string.Empty;
        DataSet ds = null;

        //Added as part of ROI conncetion string management
        //begin
        private string culture = "";

        //Constructor to pick culture value from the servcie app config
        //to dynamically decide to pick connection string for ROI from machine.config
        public ClubcardOnlineService()
        {
            culture = ConfigurationSettings.AppSettings["Culture"].ToString();
            if (culture.ToLower().Trim() == "en-ie")
            {
                //ROI connection string
                connection = Convert.ToString(ConfigurationSettings.AppSettings["ROINGCAdminConnectionString"]);
            }
            else
            {
                //UK and group connectionstring
                connection = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
            }
        }
        //end
        /// <summary>
        /// Checks whether an user exists in Clubcard system using Clubcard Number and postcode as input parameter 
        /// </summary
        /// <param name="ClubcardNumber"></param>
        /// <param name="PostCode"></param>
        /// <returns></returns>
        public ValidateClubcardAccountExistsResponse ValidateClubcardAccountExists(long ClubcardNumber, string PostCode)
        {
            ValidateClubcardAccountExistsResponse response = null;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : ValidateClubcardAccountExists Clubcard Number :" + ClubcardNumber);
                NGCTrace.NGCTrace.TraceDebug("Start : ValidateClubcardAccountExists");
                response = new ValidateClubcardAccountExistsResponse();
                string result = string.Empty;
                bool booleanResult = false;

                SqlParameter[] Params = new SqlParameter[3];
                Params[0] = new SqlParameter("pClubcardNumber", SqlDbType.BigInt);
                Params[1] = new SqlParameter("pPostCode", SqlDbType.VarChar);
                Params[2] = new SqlParameter("pIsMatched", SqlDbType.Char);
                Params[0].Value = ClubcardNumber;
                Params[1].Value = PostCode;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_ValidateClubcardAccountExists", Params);
                if (ds.Tables[0].Rows.Count >= 1)
                {
                    result = ds.Tables[0].Rows[0][0].ToString();
                    if (result.ToUpper() == "Y")
                    {
                        booleanResult = true;
                    }
                    else if (result.ToUpper() == "N")
                    {
                        booleanResult = false;
                    }
                    else
                    {
                        booleanResult = false;
                    }
                }

                NGCTrace.NGCTrace.TraceInfo("End : ValidateClubcardAccountExists Clubcard Number :" + ClubcardNumber);
                NGCTrace.NGCTrace.TraceDebug("End : ValidateClubcardAccountExists");

                response = new ValidateClubcardAccountExistsResponse(booleanResult);

            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ValidateClubcardAccountExists - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ValidateClubcardAccountExists - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ValidateClubcardAccountExists");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                response = new ValidateClubcardAccountExistsResponse();
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clubcardNumber">Clubcard number.</param>
        /// <param name="address">Address entity.</param>
        /// <param name="customer">Customer entity.</param>
        /// <returns></returns>
        public Messages.AccountFindByClubcardNumberResponse AccountFindByClubcardNumber(long clubcardNumber, ClubcardCustomer customer, DataSet dsConfig)
        {
            AccountFindByClubcardNumberResponse response = null;
            //Trace trace = null;
            //ITraceState trState = null;
            StringBuilder sb = null;
            try
            {
                /* 14 Dec, 2010 - Tracing code*/
                ///////Tracing code starts here///////////
                //trace = new Trace();
                //trState = trace.StartProc("AccountFindByClubcardNumber");

                sb = new StringBuilder();
                sb.Append("Name:");
                sb.Append(customer.FirstName);
                sb.Append(",Surname:");
                sb.Append(customer.Surname);
                sb.Append(",PostCode:");
                sb.Append(customer.Address.PostCode);
                sb.Append(",Address Line1:");
                sb.Append(customer.Address.AddressLine1);
                sb.Append(",ClucbardNumber:");
                sb.Append(clubcardNumber.ToString());


                NGCTrace.NGCTrace.TraceInfo("Start : AccountFindByClubcardNumber :" + sb);
                NGCTrace.NGCTrace.TraceDebug("Start : AccountFindByClubcardNumber");


                ////////Tracing code ends here///////////

                response = new AccountFindByClubcardNumberResponse();
                response.Matched = false;
                response.ContactDetailMatchStatus = "E";

                SqlParameter[] Params = new SqlParameter[10];
                Params[0] = new SqlParameter("CustomerID", SqlDbType.BigInt);
                Params[1] = new SqlParameter("ClubcardID", SqlDbType.BigInt);
                Params[2] = new SqlParameter("Name1", SqlDbType.NVarChar, 100);
                Params[3] = new SqlParameter("Name2", SqlDbType.NVarChar, 100);
                Params[4] = new SqlParameter("CustomerAlternateID", SqlDbType.NVarChar, 100);
                Params[5] = new SqlParameter("BusinessName", SqlDbType.NVarChar, 100);
                Params[6] = new SqlParameter("BusinessRegistrationNumber", SqlDbType.NVarChar, 14);
                Params[7] = new SqlParameter("DateOfBirth", SqlDbType.DateTime);
                Params[8] = new SqlParameter("PhoneNumber", SqlDbType.NVarChar, 40);
                Params[9] = new SqlParameter("PostCode", SqlDbType.NVarChar, 20);

                Params[0].Value = null;
                Params[1].Value = clubcardNumber;
                Params[2].Value = null;
                Params[3].Value = null;
                Params[4].Value = null;
                Params[5].Value = null;
                Params[6].Value = null;
                Params[7].Value = null;
                Params[8].Value = null;
                Params[9].Value = null;

                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetCustomers", Params);

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    response.Matched = true;
                    response.ContactDetailMatchStatus = "Y";

                    foreach (DataRow dr in dsConfig.Tables[0].Rows)
                    {
                        //To check the postcode with MailingAddressPostCode column of DB.
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            if (ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Address.PostCode != null) && (customer.Address.PostCode != string.Empty))
                            {
                                if (customer.Address.PostCode.Trim().Replace(" ", "").ToUpper() !=
                                        ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString().Trim().Replace(" ", "").ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode3Digits" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            if (ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Address.PostCode != null) && (customer.Address.PostCode != string.Empty))
                            {
                                string last3Digits = customer.Address.PostCode.Trim().Replace(" ", "");
                                string dslast3Digits = ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString().Trim().Replace(" ", "");
                                if (last3Digits.Substring(last3Digits.Length - 3, 3).ToUpper() !=
                                        dslast3Digits.Substring(dslast3Digits.Length - 3, 3).ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }

                        if (dr["ConfigurationName"].ToString().Trim() == "Name3" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check surname with Name3 column of DB
                            if (ds.Tables[0].Rows[0]["Name3"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Surname != null) && (customer.Surname != string.Empty))
                            {
                                if (customer.Surname.Trim().ToUpper() != ds.Tables[0].Rows[0]["Name3"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check addressline1.
                            if (ds.Tables[0].Rows[0]["MailingAddressLine1"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if (customer.Address.AddressLine1 != null)
                            {

                                if ((customer.Address.AddressLine1.Trim().ToUpper() != ds.Tables[0].Rows[0]["MailingAddressLine1"].ToString().Trim().ToUpper()))
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }

                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "Name1" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check FirstName with Name1 column of DB.
                            if (ds.Tables[0].Rows[0]["Name1"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.FirstName != null) && (customer.FirstName != string.Empty))
                            {
                                if (customer.FirstName.Trim().ToUpper() != ds.Tables[0].Rows[0]["Name1"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check FirstName with Mobile Number column of DB.
                            if (ds.Tables[0].Rows[0]["mobile_phone_number"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.ContactDetail.MobileContactNumber != null) && (customer.ContactDetail.MobileContactNumber != string.Empty))
                            {
                                if (customer.ContactDetail.MobileContactNumber.Trim().ToUpper() != ds.Tables[0].Rows[0]["mobile_phone_number"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "SSN" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check SSN with SSN of DB.
                            if (ds.Tables[0].Rows[0]["SSN"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.SSN != null) && (customer.SSN != string.Empty))
                            {
                                if (customer.SSN.Trim().ToUpper() != ds.Tables[0].Rows[0]["SSN"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check EmailAddress with EmailAddress of DB.
                            if (ds.Tables[0].Rows[0]["email_address"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.ContactDetail.EmailAddress != null) && (customer.ContactDetail.EmailAddress != string.Empty))
                            {
                                if (customer.ContactDetail.EmailAddress.Trim().ToUpper() != ds.Tables[0].Rows[0]["email_address"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "DayofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check DOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.DayOfBirth != null) && (customer.DayOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.DayOfBirth.Trim() != dob.Day.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MonthofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {

                            //To check MOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.MonthOfBirth != null) && (customer.MonthOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.MonthOfBirth.Trim() != dob.Month.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "YearofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check YOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.YearOfBirth != null) && (customer.YearOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.YearOfBirth.Trim() != dob.Year.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                    }
                    //return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                }
                NGCTrace.NGCTrace.TraceInfo("End : AccountFindByClubcardNumber :" + sb);
                NGCTrace.NGCTrace.TraceDebug("End : AccountFindByClubcardNumber");

                response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
            }
            catch (Exception ex)
            {
                // Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                NGCTrace.NGCTrace.TraceCritical("Critical:ValidateClubcardAccountExists - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ValidateClubcardAccountExists - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ValidateClubcardAccountExists");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                response.ErrorStatusCode = "6500";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                /* 14 Dec, 2010 - Tracing code*/
                NGCTrace.NGCTrace.TraceInfo("ServiceResponse:" + response.Matched.ToString() + ",ContactDetailMatchStatus:" + response.ContactDetailMatchStatus);
                //trace.WriteInfo("ServiceResponse:" + response.Matched.ToString() + ",ContactDetailMatchStatus:" + response.ContactDetailMatchStatus);
                //trState.EndProc();
                //trace = null;
                //trState = null;
                sb = null;
            }

            return response;
        }


        /// <summary>
        /// Method changed to send a xml instead of a dataset as parameter
        /// </summary>
        /// <param name="clubcardNumber">Clubcard number.</param>
        /// <param name="address">Address entity.</param>
        /// <param name="customer">Customer entity.</param>
        /// <returns></returns>
        public Messages.AccountFindByClubcardNumberResponse AccountFindByClubcardNumberXmlParameter(long clubcardNumber, ClubcardCustomer customer, String ConfigXml)
        {
            AccountFindByClubcardNumberResponse response = null;
            //Trace trace = null;
            //ITraceState trState = null;
            StringBuilder sb = null;
            try
            {
                /* 14 Dec, 2010 - Tracing code*/
                ///////Tracing code starts here///////////
                //trace = new Trace();
                //trState = trace.StartProc("AccountFindByClubcardNumber");

                sb = new StringBuilder();
                sb.Append("Name:");
                sb.Append(customer.FirstName);
                sb.Append(",Surname:");
                sb.Append(customer.Surname);
                sb.Append(",PostCode:");
                sb.Append(customer.Address.PostCode);
                sb.Append(",Address Line1:");
                sb.Append(customer.Address.AddressLine1);
                sb.Append(",ClucbardNumber:");
                sb.Append(clubcardNumber.ToString());


                NGCTrace.NGCTrace.TraceInfo("Start : AccountFindByClubcardNumber :" + sb);
                NGCTrace.NGCTrace.TraceDebug("Start : AccountFindByClubcardNumber");


                ////////Tracing code ends here///////////

                response = new AccountFindByClubcardNumberResponse();
                response.Matched = false;
                response.ContactDetailMatchStatus = "E";

                SqlParameter[] Params = new SqlParameter[10];
                Params[0] = new SqlParameter("CustomerID", SqlDbType.BigInt);
                Params[1] = new SqlParameter("ClubcardID", SqlDbType.BigInt);
                Params[2] = new SqlParameter("Name1", SqlDbType.NVarChar, 100);
                Params[3] = new SqlParameter("Name2", SqlDbType.NVarChar, 100);
                Params[4] = new SqlParameter("CustomerAlternateID", SqlDbType.NVarChar, 100);
                Params[5] = new SqlParameter("BusinessName", SqlDbType.NVarChar, 100);
                Params[6] = new SqlParameter("BusinessRegistrationNumber", SqlDbType.NVarChar, 14);
                Params[7] = new SqlParameter("DateOfBirth", SqlDbType.DateTime);
                Params[8] = new SqlParameter("PhoneNumber", SqlDbType.NVarChar, 40);
                Params[9] = new SqlParameter("PostCode", SqlDbType.NVarChar, 20);

                Params[0].Value = null;
                Params[1].Value = clubcardNumber;
                Params[2].Value = null;
                Params[3].Value = null;
                Params[4].Value = null;
                Params[5].Value = null;
                Params[6].Value = null;
                Params[7].Value = null;
                Params[8].Value = null;
                Params[9].Value = null;

                //To covert ConfigXml to dsconfig starts
                DataSet dsconfig = new DataSet();
                XmlDocument resulDoc = null;
                resulDoc = new XmlDocument();
                resulDoc.LoadXml(ConfigXml);
                dsconfig.ReadXml(new XmlNodeReader(resulDoc));
                //To covert ConfigXml to dsconfig ends


                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetCustomers", Params);

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    response.Matched = true;
                    response.ContactDetailMatchStatus = "Y";

                    foreach (DataRow dr in dsconfig.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        //To check the postcode with MailingAddressPostCode column of DB.
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            if (ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Address.PostCode != null) && (customer.Address.PostCode != string.Empty))
                            {
                                if (customer.Address.PostCode.Trim().Replace(" ", "").ToUpper() !=
                                        ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString().Trim().Replace(" ", "").ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressPostCode3Digits" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            if (ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Address.PostCode != null) && (customer.Address.PostCode != string.Empty))
                            {
                                string last3Digits = customer.Address.PostCode.Trim().Replace(" ", "");
                                string dslast3Digits = ds.Tables[0].Rows[0]["MailingAddressPostCode"].ToString().Trim().Replace(" ", "");
                                if (last3Digits.Substring(last3Digits.Length - 3, 3).ToUpper() !=
                                        dslast3Digits.Substring(dslast3Digits.Length - 3, 3).ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }

                        if (dr["ConfigurationName"].ToString().Trim() == "Name3" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check surname with Name3 column of DB
                            if (ds.Tables[0].Rows[0]["Name3"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.Surname != null) && (customer.Surname != string.Empty))
                            {
                                if (customer.Surname.Trim().ToUpper() != ds.Tables[0].Rows[0]["Name3"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MailingAddressLine1" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check addressline1.
                            if (ds.Tables[0].Rows[0]["MailingAddressLine1"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if (customer.Address.AddressLine1 != null)
                            {

                                if ((customer.Address.AddressLine1.Trim().ToUpper() != ds.Tables[0].Rows[0]["MailingAddressLine1"].ToString().Trim().ToUpper()))
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }

                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "Name1" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check FirstName with Name1 column of DB.
                            if (ds.Tables[0].Rows[0]["Name1"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.FirstName != null) && (customer.FirstName != string.Empty))
                            {
                                if (customer.FirstName.Trim().ToUpper() != ds.Tables[0].Rows[0]["Name1"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MobilePhoneNumber" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check FirstName with Mobile Number column of DB.
                            if (ds.Tables[0].Rows[0]["mobile_phone_number"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.ContactDetail.MobileContactNumber != null) && (customer.ContactDetail.MobileContactNumber != string.Empty))
                            {
                                if (customer.ContactDetail.MobileContactNumber.Trim().ToUpper() != ds.Tables[0].Rows[0]["mobile_phone_number"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "SSN" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check SSN with SSN of DB.
                            if (ds.Tables[0].Rows[0]["SSN"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.SSN != null) && (customer.SSN != string.Empty))
                            {
                                if (customer.SSN.Trim().ToUpper() != ds.Tables[0].Rows[0]["SSN"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "EmailAddress" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check EmailAddress with EmailAddress of DB.
                            if (ds.Tables[0].Rows[0]["email_address"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.ContactDetail.EmailAddress != null) && (customer.ContactDetail.EmailAddress != string.Empty))
                            {
                                if (customer.ContactDetail.EmailAddress.Trim().ToUpper() != ds.Tables[0].Rows[0]["email_address"].ToString().Trim().ToUpper())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "DayofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check DOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.DayOfBirth != null) && (customer.DayOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.DayOfBirth.Trim() != dob.Day.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "MonthofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {

                            //To check MOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.MonthOfBirth != null) && (customer.MonthOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.MonthOfBirth.Trim() != dob.Month.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                        if (dr["ConfigurationName"].ToString().Trim() == "YearofBirth" && dr["ConfigurationType"].ToString().Trim() == "20")
                        {
                            //To check YOB with Date of Birth of DB.
                            if (ds.Tables[0].Rows[0]["family_member_1_dob"].ToString() == string.Empty)
                            {
                                response.ContactDetailMatchStatus = string.Empty;
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                            else if ((customer.YearOfBirth != null) && (customer.YearOfBirth != string.Empty))
                            {
                                DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["family_member_1_dob"].ToString());
                                if (customer.YearOfBirth.Trim() != dob.Year.ToString())
                                {
                                    response.ContactDetailMatchStatus = "N";
                                    return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                                }
                            }
                            else
                            {
                                response.ContactDetailMatchStatus = "N";
                                return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                            }
                        }
                    }
                    //return response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
                }
                NGCTrace.NGCTrace.TraceInfo("End : AccountFindByClubcardNumberXmlParameter :" + sb);
                NGCTrace.NGCTrace.TraceDebug("End : AccountFindByClubcardNumberXmlParameter");

                response = new AccountFindByClubcardNumberResponse(response.Matched, response.ContactDetailMatchStatus);
            }
            catch (Exception ex)
            {
                // Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountFindByClubcardNumberXmlParameter - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountFindByClubcardNumberXmlParameter - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountFindByClubcardNumberXmlParameter");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                response.ErrorStatusCode = "6500";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                /* 14 Dec, 2010 - Tracing code*/
                NGCTrace.NGCTrace.TraceInfo("ServiceResponse:" + response.Matched.ToString() + ",ContactDetailMatchStatus:" + response.ContactDetailMatchStatus);
                //trace.WriteInfo("ServiceResponse:" + response.Matched.ToString() + ",ContactDetailMatchStatus:" + response.ContactDetailMatchStatus);
                //trState.EndProc();
                //trace = null;
                //trState = null;
                sb = null;
            }

            return response;
        }

        /// <summary>
        /// Method to check whether the account is duplicate.
        /// </summary>
        /// <param name="address">Address entity.</param>
        /// <param name="customer">Customer entity</param>
        /// <returns></returns>
        public Messages.AccountDuplicateCheckResponse AccountDuplicateCheck(ClubcardCustomer customer)
        {
            AccountDuplicateCheckResponse response = null;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : AccountDuplicateCheck");
                NGCTrace.NGCTrace.TraceDebug("Start : AccountDuplicateCheck");

                response = new AccountDuplicateCheckResponse();
                response.HasDuplicates = false;


                SqlParameter[] Params = new SqlParameter[10];
                Params[0] = new SqlParameter("CustomerID", SqlDbType.BigInt);
                Params[1] = new SqlParameter("ClubcardID", SqlDbType.BigInt);
                Params[2] = new SqlParameter("Name1", SqlDbType.NVarChar, 100);
                Params[3] = new SqlParameter("Name2", SqlDbType.NVarChar, 100);
                Params[4] = new SqlParameter("CustomerAlternateID", SqlDbType.NVarChar, 100);
                Params[5] = new SqlParameter("BusinessName", SqlDbType.NVarChar, 100);
                Params[6] = new SqlParameter("BusinessRegistrationNumber", SqlDbType.NVarChar, 14);
                Params[7] = new SqlParameter("DateOfBirth", SqlDbType.DateTime);
                Params[8] = new SqlParameter("PhoneNumber", SqlDbType.NVarChar, 40);
                Params[9] = new SqlParameter("PostCode", SqlDbType.NVarChar, 20);

                Params[0].Value = null;
                Params[1].Value = null;
                Params[2].Value = null;
                Params[3].Value = null;
                Params[4].Value = null;
                Params[5].Value = null;
                Params[6].Value = null;
                Params[7].Value = null;
                Params[8].Value = null;
                Params[9].Value = customer.Address.PostCode;

                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetCustomers", Params);

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        response.HasDuplicates = true;

                        //To check surname with Name3 column of DB
                        if (customer.Surname != null && customer.Surname != string.Empty)
                        {
                            if (customer.Surname.Trim().ToUpper() != ds.Tables[0].Rows[i]["Name3"].ToString().Trim().ToUpper())
                            {
                                response.HasDuplicates = false;
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["Name3"].ToString().Trim() != string.Empty)
                            {
                                response.HasDuplicates = false;
                            }
                        }

                        //To check addressline1.
                        if (customer.Address.AddressLine1 != null && customer.Address.AddressLine1 != string.Empty)
                        {
                            //Remove all the spaces in the addressline1 for better comparision.(As per Requirement)
                            if ((customer.Address.AddressLine1.Trim().Replace(" ", "").ToUpper() !=
                                ds.Tables[0].Rows[i]["MailingAddressLine1"].ToString().Trim().Replace(" ", "").ToUpper()))
                            {
                                response.HasDuplicates = false;
                            }
                        }
                        else
                        {
                            //Remove all the spaces in the addressline1 for better comparision.(As per Requirement)
                            if (ds.Tables[0].Rows[i]["MailingAddressLine1"].ToString().Trim() != string.Empty)
                            {
                                response.HasDuplicates = false;
                            }
                        }

                        //To check FirstName with Name1 column of DB.
                        if (customer.FirstName != null && customer.FirstName != string.Empty)
                        {
                            if (customer.FirstName.Trim().ToUpper() != ds.Tables[0].Rows[i]["Name1"].ToString().Trim().ToUpper())
                            {
                                response.HasDuplicates = false;
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["Name1"].ToString().Trim() != string.Empty)
                            {
                                response.HasDuplicates = false;
                            }
                        }

                        //To check Initials with Name2 column of DB.
                        if (customer.Initials != null && customer.Initials != string.Empty)
                        {
                            if (customer.Initials.Trim().ToUpper() != ds.Tables[0].Rows[i]["Name2"].ToString().Trim().ToUpper())
                            {
                                response.HasDuplicates = false;
                            }
                        }
                        else
                        {
                            if (ds.Tables[0].Rows[i]["Name2"].ToString().Trim() != string.Empty)
                            {
                                response.HasDuplicates = false;
                            }
                        }

                        //If all the creteria matches then exit from the service.
                        if (response.HasDuplicates)
                        {
                            response = new AccountDuplicateCheckResponse(response.HasDuplicates);
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End : AccountDuplicateCheck");
                NGCTrace.NGCTrace.TraceDebug("End : AccountDuplicateCheck");

                response = new AccountDuplicateCheckResponse(response.HasDuplicates);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                response = new AccountDuplicateCheckResponse();
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dotcomCustomerID">Dotcom CustomerID.</param>
        /// <param name="clubCardNumber">Clubcard Number.</param>
        /// <param name="address">Address entity.</param>
        /// <param name="customer">Customer entity.</param>
        /// <param name="dietaryPreference">DietaryPreference entity.</param>
        /// <param name="contactPreference">ContactPreference entity.</param>
        /// <param name="contactDetails">ContactDetails entity.</param>
        /// <param name="houseHold">HouseHold entity.</param>
        /// <returns></returns>
        public AccountUpdateResponse AccountUpdate(long dotcomCustomerID, long clubCardNumber, ClubcardCustomer customer)
        {
            AccountUpdateResponse response = null;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : AccountUpdate");
                NGCTrace.NGCTrace.TraceDebug("Start : AccountUpdate");
                string consumer = ConfigurationSettings.AppSettings["Consumer"].ToString();

                SqlParameter[] Params = new SqlParameter[37];
                Params[0] = new SqlParameter("pCustomerID", SqlDbType.BigInt);
                Params[1] = new SqlParameter("pCustomerAlternateID", SqlDbType.NVarChar, 100);
                Params[2] = new SqlParameter("pClubcardID", SqlDbType.BigInt);
                Params[3] = new SqlParameter("pTitleEnglish", SqlDbType.VarChar, 20);
                Params[4] = new SqlParameter("pName1", SqlDbType.NVarChar, 100);
                Params[5] = new SqlParameter("pName2", SqlDbType.NVarChar, 100);
                Params[6] = new SqlParameter("pName3", SqlDbType.NVarChar, 100);
                Params[7] = new SqlParameter("pDateOfBirth", SqlDbType.DateTime);
                Params[8] = new SqlParameter("pSex", SqlDbType.NChar, 1);
                Params[9] = new SqlParameter("pMailingAddressLine1", SqlDbType.NVarChar, 160);
                Params[10] = new SqlParameter("pMailingAddressLine2", SqlDbType.NVarChar, 160);
                Params[11] = new SqlParameter("pMailingAddressLine3", SqlDbType.NVarChar, 160);
                Params[12] = new SqlParameter("pMailingAddressLine4", SqlDbType.NVarChar, 160);
                Params[13] = new SqlParameter("pMailingAddressLine5", SqlDbType.NVarChar, 160);
                Params[14] = new SqlParameter("pMailingAddressLine6", SqlDbType.NVarChar, 160);
                Params[15] = new SqlParameter("pMailingAddressPostCode", SqlDbType.NVarChar, 20);
                Params[16] = new SqlParameter("pDaytimePhoneNumber", SqlDbType.VarChar, 40);
                Params[17] = new SqlParameter("pEveningPhoneNumber", SqlDbType.VarChar, 40);
                Params[18] = new SqlParameter("pMobilePhoneNumber", SqlDbType.VarChar, 40);
                Params[19] = new SqlParameter("pEmailAddress", SqlDbType.NVarChar, 320);
                Params[20] = new SqlParameter("pDOB1", SqlDbType.DateTime);
                Params[21] = new SqlParameter("pDOB2", SqlDbType.DateTime);
                Params[22] = new SqlParameter("pDOB3", SqlDbType.DateTime);
                Params[23] = new SqlParameter("pDOB4", SqlDbType.DateTime);
                Params[24] = new SqlParameter("pDOB5", SqlDbType.DateTime);
                Params[25] = new SqlParameter("pNoOfHHMember", SqlDbType.TinyInt);
                Params[26] = new SqlParameter("pDiabeticFlag", SqlDbType.SmallInt);
                Params[27] = new SqlParameter("pTeetotalFlag", SqlDbType.SmallInt);
                Params[28] = new SqlParameter("pVegetarianFlag", SqlDbType.SmallInt);
                Params[29] = new SqlParameter("pHalalFlag", SqlDbType.SmallInt);
                Params[30] = new SqlParameter("pCeliacFlag", SqlDbType.SmallInt);
                Params[31] = new SqlParameter("pLactoseFlag", SqlDbType.SmallInt);
                Params[32] = new SqlParameter("pOption1", SqlDbType.SmallInt);
                Params[33] = new SqlParameter("pOption2", SqlDbType.SmallInt);
                Params[34] = new SqlParameter("pOption3", SqlDbType.SmallInt);
                Params[35] = new SqlParameter("pKosherFlag", SqlDbType.SmallInt);
                Params[36] = new SqlParameter("pAmendBy", SqlDbType.SmallInt);

                //Assign the values to parameters.
                Params[0].Value = dotcomCustomerID;
                Params[1].Value = dotcomCustomerID;
                Params[2].Value = clubCardNumber;
                Params[3].Value = customer.Title;
                Params[4].Value = customer.FirstName;
                Params[5].Value = customer.Initials;
                Params[6].Value = customer.Surname;
                Params[7].Value = DBNull.Value;
                Params[8].Value = customer.Gender;

                //For address fields
                if (customer.Address != null)
                {
                    Params[9].Value = customer.Address.AddressLine1;
                    Params[10].Value = customer.Address.AddressLine2;
                    Params[11].Value = customer.Address.AddressLine3;
                    Params[12].Value = customer.Address.AddressLine4;
                    Params[13].Value = customer.Address.AddressLine5;
                    Params[14].Value = customer.Address.AddressLine6;
                    Params[15].Value = customer.Address.PostCode;
                }

                //For contcat detail fields
                if (customer.ContactDetail != null)
                {
                    Params[16].Value = customer.ContactDetail.DayContactNumber;
                    Params[17].Value = customer.ContactDetail.EveningContactNumber;
                    Params[18].Value = customer.ContactDetail.MobileContactNumber;
                    Params[19].Value = customer.ContactDetail.EmailAddress;
                }

                Params[20].Value = DBNull.Value;
                Params[21].Value = DBNull.Value;
                Params[22].Value = DBNull.Value;
                Params[23].Value = DBNull.Value;
                Params[24].Value = DBNull.Value;

                if (customer.Households.Length > 0)
                {
                    //Dynamically assign the ages entered to the parameter value.
                    for (int i = 0; i < customer.Households[0].PeopleAges.Length; i++)
                    {
                        Params[20 + i].Value = (customer.Households[0].PeopleAges[i].ToString() != null ? Convert.ToDateTime("1/1/" + (DateTime.Now.Year - customer.Households[0].PeopleAges[i])).ToString("dd/MM/yyyy") : null);
                    }
                    Params[25].Value = customer.Households[0].TotalPeople;
                }
                else
                {
                    Params[25].Value = null;
                }

                //For dietary preferences fields
                if (customer.DietaryPreferences != null)
                {
                    Params[26].Value = customer.DietaryPreferences.IsDiabetic;
                    Params[27].Value = customer.DietaryPreferences.IsTeetotal;
                    Params[28].Value = customer.DietaryPreferences.IsVegiterian;
                    Params[29].Value = customer.DietaryPreferences.IsHalal;
                    Params[35].Value = customer.DietaryPreferences.IsKoshar;
                }

                Params[30].Value = null;
                Params[31].Value = null;
                Params[32].Value = null;
                Params[33].Value = null;
                Params[34].Value = null;
                Params[36].Value = GetUserID(consumer);

                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, "USP_UpdateCustomerDetails_Orchestration", Params);

                NGCTrace.NGCTrace.TraceInfo("End : AccountUpdate");
                NGCTrace.NGCTrace.TraceDebug("End : AccountUpdate");
                response = new AccountUpdateResponse();
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                response = new AccountUpdateResponse();
            }

            return response;
        }

        /// <summary>
        /// Links Tesco PLC clubcard account with Dotcom Customer ID.
        /// </summary>
        /// <param name="dotcomCustomerID"></param>
        /// <param name="clubCardNumber"></param>
        /// <returns></returns>
        public AccountLinkResponse AccountLink(long dotcomCustomerID, long clubCardNumber)
        {
            AccountLinkResponse response = null;
            string consumer = ConfigurationSettings.AppSettings["Consumer"].ToString();
            string dotcomCustomer = ConfigurationSettings.AppSettings["DotcomCustomer"].ToString();
            string dotcomCustomerId1 = string.Empty;
            //Trace trace = null;
            //ITraceState trState = null;

            try
            {
                /* 14 Dec, 2010 - Tracing code*/
                ///////Tracing code starts here///////////
                //trace = new Trace();
                //trState = trace.StartProc("AccountLink");
                //trace.WriteInfo("DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceInfo("Start : AccountLink DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start : AccountLink");
                response = new AccountLinkResponse();
                dotcomCustomerId1 = Convert.ToString(dotcomCustomerID);
                response = IGHSAccountLink(dotcomCustomerId1, clubCardNumber);

                NGCTrace.NGCTrace.TraceInfo("End : AccountLink DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceDebug("End : AccountLink");

            }
            catch (Exception ex)
            {
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "DotcomID:" + dotcomCustomerID.ToString() + "ClubcardNumber:" + clubCardNumber);
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                response.ErrorStatusCode = "6500";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                /* 14 Dec, 2010 - Tracing code*/
                //trState.EndProc();
                //trace = null;
                //trState = null;
            }

            return response;
        }



        #region IGHS
        /// <summary>
        /// Links Tesco PLC clubcard account with Dotcom Customer ID
        /// Added by Lakshmi as a part of IGHS-login solution(dotcomid datatype changed from int to string)on 15/01/2013.
        /// </summary>
        /// <param name="dotcomCustomerID"></param>
        /// <param name="clubCardNumber"></param>
        /// <returns></returns>
        public AccountLinkResponse IGHSAccountLink(string dotcomCustomerID, long clubCardNumber)
        {
            AccountLinkResponse response = null;
            string consumer = ConfigurationSettings.AppSettings["Consumer"].ToString();
            string dotcomCustomer = ConfigurationSettings.AppSettings["DotcomCustomer"].ToString();
            //Trace trace = null;
            //ITraceState trState = null;

            try
            {
                /* 14 Dec, 2010 - Tracing code*/
                ///////Tracing code starts here///////////
                //trace = new Trace();
                //trState = trace.StartProc("AccountLink");
                //trace.WriteInfo("DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceInfo("Start : AccountLink DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceDebug("Start : AccountLink");
                response = new AccountLinkResponse();

                SqlParameter[] Params = new SqlParameter[5];
                Params[0] = new SqlParameter("pDotcomCustomerID", SqlDbType.NVarChar);
                Params[1] = new SqlParameter("pCustomerID", SqlDbType.BigInt);
                Params[2] = new SqlParameter("pClubcardID", SqlDbType.BigInt);
                Params[3] = new SqlParameter("pCustomerAlternateIDType", SqlDbType.TinyInt);
                Params[4] = new SqlParameter("pInsertBy", SqlDbType.SmallInt);
                Params[0].Value = dotcomCustomerID;
                Params[1].Value = null;
                Params[2].Value = clubCardNumber;
                Params[3].Value = GetCustomerType(dotcomCustomer);
                Params[4].Value = GetUserID(consumer);
                SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, "USP_CreateCustomerAlternateID", Params);

                NGCTrace.NGCTrace.TraceInfo("End : AccountLink DotcomID:" + dotcomCustomerID.ToString() + ", ClubcardNumber:" + clubCardNumber.ToString());
                NGCTrace.NGCTrace.TraceDebug("End : AccountLink");

            }
            catch (Exception ex)
            {
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "DotcomID:" + dotcomCustomerID.ToString() + "ClubcardNumber:" + clubCardNumber);
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountDuplicateCheck - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountDuplicateCheck");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                response.ErrorStatusCode = "6500";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                /* 14 Dec, 2010 - Tracing code*/
                //trState.EndProc();
                //trace = null;
                //trState = null;
            }

            return response;
        }
        #endregion
        /// <summary>
        /// Creates a Clubcard account within the Tesco Plc Clubcard system.
        /// </summary>
        /// <param name="dotcomCustomerID"></param>
        /// <param name="customer"></param>
        /// <param name="source"></param>
        /// <param name="isDuplicate"></param>
        /// <returns></returns>
        public Messages.AccountCreateResponse AccountCreate(long dotcomCustomerID, ClubcardCustomer customer, string source, string isDuplicate)
        {
            AccountCreateResponse response = null;
            StringBuilder sb = null;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : AccountCreate");
                NGCTrace.NGCTrace.TraceDebug("Start : AccountCreate");

                sb = new StringBuilder();
                sb.Append("DotcomCustomerID:");
                sb.Append(dotcomCustomerID);
                sb.Append("First Name:");
                sb.Append(customer.FirstName);
                sb.Append(",Surname:");
                sb.Append(customer.Surname);

                if (customer.Address != null)
                {
                    sb.Append(",PostCode:");
                    sb.Append(customer.Address.PostCode);
                    sb.Append(",Address Line1:");
                    sb.Append(customer.Address.AddressLine1);
                }

                sb.Append(",Source:");
                sb.Append(source);

                string consumer = ConfigurationSettings.AppSettings["Consumer"].ToString();

                response = new AccountCreateResponse();
                response.ClubcardNumber = 0;

                SqlParameter[] Params = new SqlParameter[35];
                Params[0] = new SqlParameter("pDotcomCustomerID", SqlDbType.BigInt);
                Params[1] = new SqlParameter("pTitleEnglish", SqlDbType.VarChar, 20);
                Params[2] = new SqlParameter("pName1", SqlDbType.NVarChar, 200);
                Params[3] = new SqlParameter("pName2", SqlDbType.VarChar, 200);
                Params[4] = new SqlParameter("pName3", SqlDbType.NVarChar, 200);
                Params[5] = new SqlParameter("pDateOfBirth", SqlDbType.DateTime);
                Params[6] = new SqlParameter("pSex", SqlDbType.NChar, 2);
                Params[7] = new SqlParameter("pMailingAddressLine1", SqlDbType.NVarChar, 160);
                Params[8] = new SqlParameter("pMailingAddressLine2", SqlDbType.NVarChar, 160);
                Params[9] = new SqlParameter("pMailingAddressLine3", SqlDbType.NVarChar, 160);
                Params[10] = new SqlParameter("pMailingAddressLine4", SqlDbType.NVarChar, 160);
                Params[11] = new SqlParameter("pMailingAddressLine5", SqlDbType.NVarChar, 160);
                Params[12] = new SqlParameter("pMailingAddressLine6", SqlDbType.NVarChar, 160);
                Params[13] = new SqlParameter("pMailingAddressPostCode", SqlDbType.NVarChar, 40);
                Params[14] = new SqlParameter("pIsDiabetic", SqlDbType.NChar, 2);
                Params[15] = new SqlParameter("pIsVegetarian", SqlDbType.NChar, 2);
                Params[16] = new SqlParameter("pIsTeeTotal", SqlDbType.NChar, 2);
                Params[17] = new SqlParameter("pIsKosher", SqlDbType.NChar, 2);
                Params[18] = new SqlParameter("pIsHalal", SqlDbType.NChar, 2);
                Params[19] = new SqlParameter("pWantTescoInfo", SqlDbType.NChar, 2);
                Params[20] = new SqlParameter("pWantPartnerInfo", SqlDbType.NChar, 2);
                Params[21] = new SqlParameter("pIsResearchContactable", SqlDbType.NChar, 2);
                Params[22] = new SqlParameter("pIsContactable", SqlDbType.NChar, 2);
                Params[23] = new SqlParameter("pEmailAddress", SqlDbType.NVarChar, 640);
                Params[24] = new SqlParameter("pDayContactNumber", SqlDbType.VarChar, 40);
                Params[25] = new SqlParameter("pEveningContactNumber", SqlDbType.VarChar, 40);
                Params[26] = new SqlParameter("pMobileContactNumber", SqlDbType.VarChar, 40);
                Params[27] = new SqlParameter("pNumberOfPeople", SqlDbType.TinyInt);
                Params[28] = new SqlParameter("pAge1", SqlDbType.TinyInt);
                Params[29] = new SqlParameter("pAge2", SqlDbType.TinyInt);
                Params[30] = new SqlParameter("pAge3", SqlDbType.TinyInt);
                Params[31] = new SqlParameter("pAge4", SqlDbType.TinyInt);
                Params[32] = new SqlParameter("pCustomerSource", SqlDbType.NChar, 2);
                Params[33] = new SqlParameter("pIsDuplicate", SqlDbType.NChar, 2);
                Params[34] = new SqlParameter("pInsertBy", SqlDbType.SmallInt);

                //Assign the values to parameters.
                Params[0].Value = dotcomCustomerID;
                Params[1].Value = customer.Title;
                Params[2].Value = customer.FirstName;
                Params[3].Value = customer.Initials;
                Params[4].Value = customer.Surname;
                Params[5].Value = customer.DateOfBirth;

                response = new AccountCreateResponse();

                //To check the gender. It should be either M or F.
                if (!string.IsNullOrEmpty(customer.Gender)
                    && (customer.Gender.Trim() != "M" && customer.Gender.Trim() != "m")
                    && (customer.Gender.Trim() != "F" && customer.Gender.Trim() != "f"))
                {
                    response.ErrorStatusCode = "6500";
                    response.ErrorMessage = "Gender should be either M or F or blank.";

                    return response;
                }

                //If gender is not blank then convert it to uppercase.
                Params[6].Value = string.IsNullOrEmpty(customer.Gender) ? customer.Gender : customer.Gender.Trim().ToUpper();


                //For address fields
                if (customer.Address != null)
                {
                    Params[7].Value = customer.Address.AddressLine1;
                    Params[8].Value = customer.Address.AddressLine2;
                    Params[9].Value = customer.Address.AddressLine3;
                    Params[10].Value = customer.Address.AddressLine4;
                    Params[11].Value = customer.Address.AddressLine5;
                    Params[12].Value = customer.Address.AddressLine6;
                    Params[13].Value = customer.Address.PostCode;
                }

                //For DietaryPreferences fields
                if (customer.DietaryPreferences != null)
                {
                    Params[14].Value = customer.DietaryPreferences.IsDiabetic;
                    Params[15].Value = customer.DietaryPreferences.IsVegiterian;
                    Params[16].Value = customer.DietaryPreferences.IsTeetotal;
                    Params[17].Value = customer.DietaryPreferences.IsKoshar;
                    Params[18].Value = customer.DietaryPreferences.IsHalal;
                }

                //For ContactPreference fields
                if (customer.ContactPreference != null)
                {
                    Params[19].Value = customer.ContactPreference.WantTescoInfo;
                    Params[20].Value = customer.ContactPreference.WantPartnerInfo;
                    Params[21].Value = customer.ContactPreference.ResearchContactable;
                    Params[22].Value = customer.ContactPreference.Contactable;
                }

                //For ContactDetail fields
                if (customer.ContactDetail != null)
                {
                    Params[23].Value = customer.ContactDetail.EmailAddress;
                    Params[24].Value = customer.ContactDetail.DayContactNumber;
                    Params[25].Value = customer.ContactDetail.EveningContactNumber;
                    Params[26].Value = customer.ContactDetail.MobileContactNumber;
                }

                Params[27].Value = DBNull.Value;
                Params[28].Value = DBNull.Value;
                Params[29].Value = DBNull.Value;
                Params[30].Value = DBNull.Value;
                Params[31].Value = DBNull.Value;

                //For Household fields
                if (customer.Households.Length > 0)
                {
                    //Dynamically assign the ages entered to the parameter value.
                    for (int i = 0; i < customer.Households[0].PeopleAges.Length; i++)
                    {
                        Params[28 + i].Value = customer.Households[0].PeopleAges[i].ToString();
                    }
                    Params[27].Value = customer.Households[0].TotalPeople;
                }

                //Check the length of source. It should be of one character
                if (!string.IsNullOrEmpty(source) && source.Trim().Length > 1)
                {
                    response.ErrorStatusCode = "6500";
                    response.ErrorMessage = "Source should be of one character.";

                    return response;
                }

                Params[32].Value = string.IsNullOrEmpty(source) ? source : source.Trim().ToUpper();

                //Check the character entered for IsDuplicate field. It should Y or N or blank
                if (!string.IsNullOrEmpty(isDuplicate)
                    && (isDuplicate.Trim() != "Y" && isDuplicate.Trim() != "y")
                    && (isDuplicate.Trim() != "N" && isDuplicate.Trim() != "n"))
                {
                    response.ErrorStatusCode = "6500";
                    response.ErrorMessage = "Is Duplicate field accept only Y or N or blank.";

                    return response;
                }

                Params[33].Value = string.IsNullOrEmpty(isDuplicate) ? isDuplicate : isDuplicate.Trim().ToUpper();
                Params[34].Value = GetUserID(consumer);

                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_CreatePendingCustomer", Params);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count == 1)
                    {
                        response.ClubcardNumber = Convert.ToInt64(ds.Tables[0].Rows[0][0].ToString());
                        response.ErrorStatusCode = "0";

                        if (response.ClubcardNumber == -1)
                        {
                            response.ErrorMessage = "Duplicate record found.";
                        }
                        else
                        {
                            //Once clubcard is generated successfully, send join confirmation email.
                            if ((customer.ContactDetail != null) && (!string.IsNullOrEmpty(customer.ContactDetail.EmailAddress)))
                            {
                                SendJoinConfirmationEmail(customer.ContactDetail.EmailAddress.ToString(), customer.Title, customer.Surname, response.ClubcardNumber);
                            }
                        }
                    }
                    else if (ds.Tables[0].Columns.Count == 2)
                    {
                        response.ErrorStatusCode = "9500";
                        response.ErrorMessage = ds.Tables[0].Rows[0][1].ToString();
                    }
                }
                else
                {
                    response.ErrorStatusCode = "9500";
                    response.ErrorMessage = "Some internal error in Stored procedure";
                }

                NGCTrace.NGCTrace.TraceInfo("End : AccountCreate");
                NGCTrace.NGCTrace.TraceDebug("End : AccountCreate");
            }
            catch (Exception ex)
            {
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                NGCTrace.NGCTrace.TraceCritical("Critical:AccountCreate - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:AccountCreate - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:AccountCreate");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                response = new AccountCreateResponse();
            }

            return response;
        }


        /// <summary>
        /// Gets the userID for provided consumer.
        /// </summary>
        /// <param name="consumer"></param>
        /// <returns>User ID.</returns>
        private short GetUserID(string consumer)
        {
            short userID = 0;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : GetUserID");
                NGCTrace.NGCTrace.TraceDebug("Start : GetUserID");
                SqlParameter[] Params = new SqlParameter[10];
                Params[0] = new SqlParameter("userName", SqlDbType.NVarChar, 20);
                Params[1] = new SqlParameter("userDescription", SqlDbType.NVarChar, 100);
                Params[2] = new SqlParameter("roleName", SqlDbType.NVarChar, 20);
                Params[3] = new SqlParameter("culture", SqlDbType.Char, 5);

                Params[0].Value = consumer;
                Params[1].Value = null;
                Params[2].Value = null;
                Params[3].Value = null;

                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_GetApplicationUsers", Params);

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    userID = Convert.ToInt16(ds.Tables[0].Rows[0]["UserID"].ToString());
                }
                NGCTrace.NGCTrace.TraceInfo("End : GetUserID");
                NGCTrace.NGCTrace.TraceDebug("End : GetUserID");
            }
            catch (Exception ex)
            {
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                NGCTrace.NGCTrace.TraceCritical("Critical:GetUserID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:GetUserID - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:GetUserID");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }

            return userID;
        }

        /// <summary>
        /// This method is used to get CustomerAlternateIDType by taking CustomerAlternateIDTypeEnglish 
        /// from the table CustomerAlternateIDType
        /// Author:Robin Apoto
        /// Date:03/June/2010
        /// </summary>
        /// <param name="dotcomCustomer"></param>
        /// <returns></returns>
        private short GetCustomerType(string dotcomCustomer)
        {
            short dotcomCustomerID = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start : GetCustomerType");
                NGCTrace.NGCTrace.TraceDebug("Start : GetCustomerType");

                SqlParameter[] Params = new SqlParameter[2];
                Params[0] = new SqlParameter("@pCustomerAlternateIDTypeEnglish", SqlDbType.NVarChar, 100);
                Params[1] = new SqlParameter("@pCustomerAlternateIDType", SqlDbType.TinyInt, 1);

                Params[0].Value = dotcomCustomer;
                Params[1].Value = null;

                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "USP_Get_CustomerAlternateIDType", Params);

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    dotcomCustomerID = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                }
                NGCTrace.NGCTrace.TraceInfo("Start : GetCustomerType");
                NGCTrace.NGCTrace.TraceDebug("Start : GetCustomerType");

            }
            catch (Exception ex)
            {
                //Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "Clubcardonline");
                NGCTrace.NGCTrace.TraceCritical("Critical:GetCustomerType - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:GetCustomerType - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:GetCustomerType");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }

            return dotcomCustomerID;
        }



        /// <summary>
        /// This method sends an email once the customer activate their account in MCA
        /// </summary>
        /// <param name="strTo">To whom the email should go</param>
        /// <returns></returns>
        public bool SendActivationEmail(string strTo)
        {
            bool bResult = false;
            //String builder to add mail body
            StringBuilder strBlr = new StringBuilder();

            //create the mail message
            MailMessage mail;



            try
            {
                ///////Tracing code starts here///////////

                NGCTrace.NGCTrace.TraceInfo("Start : SendActivationEmail- To email ID:" + strTo);
                NGCTrace.NGCTrace.TraceDebug("Start : SendActivationEmail");
                //set the addresses
                string strFrom = ConfigurationSettings.AppSettings["emailFromAddress"];
                string strSubject = "Welcome to My Clubcard Account";
                string strHTML = string.Empty;
                string strTempalteHtmlpath = string.Empty;

                if (!String.IsNullOrEmpty(strTo))
                {

                    mail = new MailMessage(strFrom, strTo);

                    mail.Subject = strSubject;

                    //Create the Html part
                    //To embed images, we need to use the prefix 'cid' in the img src value
                    //the cid value will map to the Content-Id of a Linked resource.
                    //thus <img src='cid:companylogo'> will map to a LinkedResource with a ContentId of 'companylogo'
                    //strBlr.Append("<html><body>");
                    //strBlr.Append("<table style='font-family:Verdana; font-size:14px;width:700px'>");
                    //strBlr.Append("<tr><td><img src=cid:companylogo></td></tr>");
                    //strBlr.Append("<tr><td><hr style='height:20px; color:#49BCD7; width:700px; text-align:left;' /></td></tr>");
                    //strBlr.Append("<tr><td><b>Dear Customer</b>,<br /><br />");
                    //strBlr.Append("Thank you for registering online with your Clubcard. You can now access<br /> ");
                    //strBlr.Append("<a style='color:navy;' href='https://secure.tesco.com/register/?from=https://secure.tesco.com/clubcard/myaccount/home.aspx'><b>My Clubcard account</b></a> anytime with your tesco.com login details, where<br /> you can");
                    //strBlr.Append("<ul>");
                    //strBlr.Append("<li>View and amend your personal details</li>");
                    //strBlr.Append("<li>View your points balance and where you have earned points</li>");
                    //strBlr.Append("<li>View and print your vouchers</li>");
                    //strBlr.Append("<li>View your eCoupons</li>");
                    //strBlr.Append("<li>Order a replacement card or key fobs</li>");
                    //strBlr.Append("</ul>");
                    //strBlr.Append("We hope you enjoy the benefits of <a style='color:navy;' href='https://secure.tesco.com/register/?from=https://secure.tesco.com/clubcard/myaccount/home.aspx'><b>My Clubcard account</b></a>");
                    //strBlr.Append("<br /><br />");
                    //strBlr.Append("Kind regards<br />");
                    //strBlr.Append("The Clubcard team");
                    //strBlr.Append("</td></tr>");
                    //strBlr.Append("<tr><td><hr style='height:7px; color:#49BCD7; width:700px; text-align:left;' /></td></tr>");
                    //strBlr.Append("<tr>");
                    //strBlr.Append("<td style='font-size:smaller;color:Gray;'>This is an email from Tesco.com Limited (Company Number 3942522). Registered in England. Registered Office: Tesco House, Delamare Road, Cheshunt, Hertfordshire EN8 9SL. VAT Registration Number: GB 220 4302 31.");
                    //strBlr.Append("<br /><br />Neither Tesco.com Limited nor any company within the Tesco Plc Group accepts responsibility for the accuracy or completeness of the contents of this email as it has been transmitted over a public network. If you recieve this email in error please accept our apology. If this is the case we would be obliged if you would contact the sender and then delete this email.</td>");
                    //strBlr.Append("</tr>");
                    //strBlr.Append("</table>");
                    //strBlr.Append("</body></html>");

                    strTempalteHtmlpath = Convert.ToString(ConfigurationSettings.AppSettings["TemplatePath"]);
                    strHTML = File.ReadAllText(strTempalteHtmlpath);
                    strBlr = strBlr.Append(strHTML);
                    mail.Body = strBlr.ToString();
                    mail.IsBodyHtml = true;

                    //first we create the Plain Text part
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/plain");
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(strBlr.ToString(), null, "text/html");

                    //create the LinkedResource (embedded image)
                    LinkedResource logo = new LinkedResource(ConfigurationSettings.AppSettings["imagePath"]);
                    logo.ContentId = "companylogo";

                    //add the LinkedResource to the appropriate view
                    htmlView.LinkedResources.Add(logo);

                    //add the views
                    mail.AlternateViews.Add(plainView);
                    mail.AlternateViews.Add(htmlView);

                    //send the message
                    SmtpClient smtpMail = new SmtpClient(ConfigurationSettings.AppSettings["smtpClient"]);

                    smtpMail.Send(mail);
                }
                NGCTrace.NGCTrace.TraceInfo("End : SendActivationEmail- To email ID:" + strTo);
                NGCTrace.NGCTrace.TraceDebug("End : SendActivationEmail");
                bResult = true;
            }
            catch (Exception ex)
            {
                bResult = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:SendActivationEmail - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:SendActivationEmail - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:SendActivationEmail");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }

            return bResult;
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

            //create the mail message
            MailMessage mail;



            try
            {
                ///////Tracing code starts here///////////


                NGCTrace.NGCTrace.TraceInfo("Start : SendActivationEmail- To email ID:" + strTo + "Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceDebug("Start : SendActivationEmail");

                //set the addresses
                string strFrom = ConfigurationSettings.AppSettings["fromAddressForJoin"];
                string strSubject = "Thank you for joining Clubcard";

                mail = new MailMessage(strFrom, strTo);

                mail.Subject = strSubject;

                //Create the Html part
                strBlr.Append("<html><body>");
                strBlr.Append("<table style='font-family:Verdana; font-size:13px;width:700px'>");
                strBlr.Append("<tr><td>Dear " + title + " " + custName + ",<br /><br />");
                strBlr.Append("Thank you for joining Clubcard! ");
                strBlr.Append("Keep a look out for your Clubcard and keyfobs which will arrive in the post within the next 10 days.<br /><br />");

                strBlr.Append("Your Clubcard number is " + clubcardID + ". ");
                strBlr.Append("If you have an iPhone, Blackberry or Nokia smartphone, you can use this number to download your Clubcard to your mobile <a href='http://www.tesco.com/clubcard/clubcard/smartphones.asp'>here.</a> ");
                strBlr.Append("You can also use your Clubcard number to collect points when ordering items online from Tesco.com or Tesco direct, ");
                strBlr.Append("but please allow 48 hours for your Clubcard number to be activated online.<br /><br />");

                strBlr.Append("As a Clubcard member, you will receive Clubcard statements throughout the year containing any vouchers you have earned, plus money off and extra points coupons for products we think you might like.<br /><br />");
                strBlr.Append("If you have any questions you can call us on 0800 59 16 88 (lines are open Mon - Fri 9am - 8pm, Sat 9am - 5pm, Sun closed) or <a href='http://www.tesco.com/clubcard'>visit our website</a> for more information.<br /><br />");
                strBlr.Append("Best wishes,<br />");
                strBlr.Append("The Clubcard team");
                strBlr.Append("</td></tr>");
                strBlr.Append("</table>");
                strBlr.Append("</body></html>");

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

                NGCTrace.NGCTrace.TraceInfo("End : SendActivationEmail- To email ID:" + strTo + "Customer Name:" + custName);
                NGCTrace.NGCTrace.TraceDebug("End : SendActivationEmail");
                bResult = true;
            }
            catch (Exception ex)
            {
                bResult = false;
                NGCTrace.NGCTrace.TraceCritical("Critical:SendActivationEmail - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:SendActivationEmail - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:SendActivationEmail");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            finally
            {

            }

            return bResult;
        }
    }
}
