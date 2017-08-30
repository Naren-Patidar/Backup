using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Web.Security;
using System.Resources;
using System.Timers;
using System.Threading;
using System.DirectoryServices;
using System.EnterpriseServices;
using System.DirectoryServices.ActiveDirectory;
using Tesco.NGC.DataAccessLayer;
using Fujitsu.eCrm.Generic.SharedUtils;
using System;
using System.Text;

namespace Tesco.NGC.Security
{
    public class Session
    {
        #region Attributes
        private string userName;
        private short userId;
        private string culture;
        private CultureInfo cultureInfo;
        private string capabilityXml;
        private string displayName;
        public string AppName { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Construct a new Session Identified by sessionId
        /// Check a session can be created for the user the user
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="sessionId">The session's ID</param>
        /// <param name="userName">The session's User</param>
        /// <param name="password">The User's Password</param>
        /// <param name="cultureCode">The User's Culture</param>
        public Session(string domain, string userName, string password, string cultureCode, string AppName,string ldapPath)
        {

            #region Check Culture
            // Check that a valid culture has been supplied.
            try
            {
                new CultureInfo(cultureCode);  // test culture is valid for .Net
            }
            catch
            {
                CrmServiceException ce = new CrmServiceException("Client", "ParameterError", "UnknownCulture", cultureCode);
                throw ce;
            }
            // The culture is valid, so set the cultureInfo and culture attributes.
            this.Culture = cultureCode;
            // Test culture is valid for CRM Server.
           
            #endregion

            this.UserName = userName;
            this.AppName = AppName;
            string errorXml = string.Empty;

            // Check the request can be given connection
            //Authenticate user with the Active directory

            //if (AuthenticateUser(domain,userName, password))
            //{
               
            //}
            String result=AuthenticateUser(domain,userName, password,ldapPath);
            this.displayName = result;
            if (result != null)
            {
                this.DisplayName = this.displayName;
            }
            else
            {
                CrmServiceException ce = new CrmServiceException(
                    "User",
                    "SessionLacksPrivileges",
                    "Authentication Failed");
                throw ce;
            }

            //Check the user is there in the db
            //If present get the user id
            //Otherwise insert the user into the table with no privileges
            this.UserId = (short)GetUser();
            if (this.UserId != -1)
            {
                
            }
            else
            {
               CrmServiceException ce = new CrmServiceException(
                    "User",
                    "SessionLacksPrivileges",
                    "Validation failed for user");
                //throw ce;
            }

            // Record connection on the admin database
           
            // Setup user's privileges
          
            this.WriteCapabilityXml(this.UserId);
            // Set-up the thread session server culture.
            this.SetThreadUiCulture();
        }
        #endregion

        #region Methods

        

        //Check the user is present in the database table
        //Otherwise create the user with no privileges
        public int GetUser()
        {
            int retval = 0, identityParam = 0;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("Session.GetUser");
            StringBuilder sb = new StringBuilder();
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {
                //Execute SP to get the UserID from the db 
                //Create a user with no privileges incase the user is not existing in the table
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                object[] objSession = { UserName, Culture, identityParam };
                retval = SqlHelper.ExecuteNonQuery(connectionString, "USP_AuthenticateUser", objSession);
            }
            catch (Exception ex)
            {
                //CrmServiceException ce = new CrmServiceException(
                //    "Server",
                //    "SqlError",
                //    "ConnectionRefused");
                throw ex;
            }
            finally
            {
                trState.EndProc();
            }

            return retval;
        }

        //Authenticate user against the active directory
        //Check if the user is part of the NGC group
        public string AuthenticateUser(String domain, String userName, String password,String ldapPath)
        {
            //bool loginStatus = false;
            string loginStatus = null;
            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("Session.AuthenticateUser");
            StringBuilder sb = new StringBuilder();
            sb.Append(" Domain: " + domain);
            sb.Append(" UserName: " + userName);
            trace.WriteInfo(sb.ToString());            
            #endregion

            try
            {
                //Read the LDAP data from the Web.config file
                if (domain != null)
                {


                    String strLDAPPath = ldapPath;
                    String domainAndUserType = domain  + @"\" + userName;
                    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    search.Filter = "(SAMAccountName=" + userName + ")";
                    SearchResult result;
                    result = search.FindOne();
                    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    if (result.Path == "")
                    {
                        loginStatus = null;

                    }
                    else
                    {
                        loginStatus = displayName;

                    }

                    //if (domain == "in")
                    //{
                    //    String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathIN"]);
                    //    String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["INDomain"]) + @"\" + userName;
                    //    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    //    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    //    search.Filter = "(SAMAccountName=" + userName + ")";
                    //    SearchResult result;
                    //    result = search.FindOne();
                    //    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    //    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    //    if (result.Path == "")
                    //    {
                    //        loginStatus = null;
                           
                    //    }
                    //    else
                    //    {
                    //        loginStatus = displayName;

                    //    }
                    //}
                    //else if (domain == "us")
                    //{
                    //    String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathUS"]);
                    //    String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["usDomain"]) + @"\" + userName;
                    //    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    //    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    //    search.Filter = "(SAMAccountName=" + userName + ")";
                    //    SearchResult result;
                    //    result = search.FindOne();
                    //    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    //    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    //    if (result.Path == "")
                    //    {
                    //        loginStatus = null;

                    //    }
                    //    else
                    //    {
                    //        loginStatus = displayName;

                    //    }
                    //}
                    //else if (domain == "dev01")
                    //{
                    //    String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathDev01"]);
                    //    String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["dev01Domain"]) + @"\" + userName;
                    //    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    //    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    //    search.Filter = "(SAMAccountName=" + userName + ")";
                    //    SearchResult result;
                    //    result = search.FindOne();
                    //    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    //    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    //    if (result.Path == "")
                    //    {
                    //        loginStatus = null;
                            
                    //    }
                    //    else
                    //    {
                    //        loginStatus = displayName;

                    //    }
                    //}
                    //else if (domain == "ukroi")
                    //{
                    //    String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathUKROI"]);
                    //    String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["ukroiDomain"]) + @"\" + userName;
                    //    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    //    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    //    search.Filter = "(SAMAccountName=" + userName + ")";
                    //    SearchResult result;
                    //    result = search.FindOne();
                    //    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    //    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                      
                    //    if (result.Path == "")
                    //    {
                    //        loginStatus = null;
                           
                    //    }
                    //    else
                    //    {
                    //        loginStatus = displayName;
                    //    }
                    //}
                    //else if (domain == "tsl")
                    //{
                    //    String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPathTSL"]);
                    //    String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["tslDomain"]) + @"\" + userName;
                    //    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    //    DirectorySearcher search = new DirectorySearcher(objDirEntry);

                    //    search.Filter = "(SAMAccountName=" + userName + ")";
                    //    SearchResult result;
                    //    result = search.FindOne();
                    //    DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                    //    string displayName = directoryEntry.Properties["displayName"][0].ToString();
                    //    if (result.Path == "")
                    //    {
                    //        loginStatus = null;

                    //    }
                    //    else
                    //    {
                    //        loginStatus = displayName;
                    //    }
                    //}

                }
                
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            finally
            {
                trState.EndProc();
                
            }

            return loginStatus;
        }

        #region Write User's Capability XML
        /// <summary>
        /// Write a capability XML Document and read it to a string
        /// </summary>
        private void WriteCapabilityXml(short userID)
        {
            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("Session.WriteCapabilityXml");
            StringBuilder sb = new StringBuilder();
            sb.Append(" UserID: " + userID);
            trace.WriteInfo(sb.ToString());
            #endregion


            // Write XML (left-to-right) into a memory stream
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            // Add the root capabilities node
            writer.WriteStartElement("capabilities");
            string namespacePath = "urn:Fujitsu.eCrm.Seoul.CrmService/xsd/capabilities";
            writer.WriteAttributeString("xmlns:crm", namespacePath);
            // Add child element nodes to the capabilities node
            ProcessUserCapabilities(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            // Rewind memory stream, and read the XML from memory into a string
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            this.CapabilityXml = reader.ReadToEnd();
            reader.Close();

            // Set the capability xml
            //this.CapabilityXml=capabilityXml;

            trState.EndProc();
        }

        /// <summary>
        /// Find the Capabilities of the User and write them to the capability XML Document
        /// </summary>
        private void ProcessUserCapabilities(XmlTextWriter writer)
        {

            //Get the user capability data from the Database using DAL
            Trace trace = new Trace();

            #region Trace
            ITraceState trState = trace.StartProc("Session.ProcessUserCapabilities");           
            
            #endregion

            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                SqlDataReader Reader = SqlHelper.ExecuteReader(connectionString, "USP_AuthorizeUser", UserId);
                string maxPoints = String.Empty;
                string minPoints = String.Empty;
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        Int16 capabilityID = (Int16)Reader[0];
                        string capabilityName = (string)Reader[1];
                        maxPoints = Convert.ToString(Reader[2]);
                        minPoints = Convert.ToString(Reader[3]);
                        writer.WriteStartElement(capabilityName);
                        //ProcessProperties(capabilityCrmId, writer);
                        writer.WriteEndElement();
                    }
                    writer.WriteStartElement("MaxPoints");
                    writer.WriteValue(maxPoints);
                    writer.WriteEndElement();
                    writer.WriteStartElement("MinPoints");
                    writer.WriteValue(minPoints);
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteValue("Null");
                }
            }
            catch (Exception e)
            {
                trace.WriteError(e.Message);
            }
            finally
            {
                trState.EndProc();
            }
        }
        #endregion

        /// <summary>
        /// Set the culture of the thread to the culture of the current user
        /// </summary>
        public void SetThreadUiCulture()
        {
            //Thread.CurrentThread.CurrentUICulture = this.CultureInfo;
        }
        #endregion

        #region Properties
        /// <summary>The User Name of the session's owner</summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>The User Idenitifier of the session's owner</summary>
        public short UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        /// <summary>The Culture of the session's owner</summary>
        public string Culture
        {
            get { return this.culture; }
            set
            {
                this.culture = value;
                this.cultureInfo = new CultureInfo(value);
            }
        }

        /// <summary>The culture of the session's owner. This will have been
        /// checked against admin_culture and set to 'default' if not present.</summary>
        public string SessionCulture { get { return this.culture; } }

        /// <summary>The Culture of the session's owner</summary>
        private CultureInfo CultureInfo { get { return this.cultureInfo; } }

        /// <summary>The session's Capability Privileges</summary>
        public string CapabilityXml
        {
            get { return this.capabilityXml; }
            set { this.capabilityXml = value; }
        }
        public string DisplayName
        {
            get { return this.displayName; }
            set { this.displayName = value; }
        }
        #endregion
    }
}
