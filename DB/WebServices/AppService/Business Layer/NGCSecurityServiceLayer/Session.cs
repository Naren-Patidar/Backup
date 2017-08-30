using System;
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
using Tesco.NGC.Utils;
using System;
namespace Tesco.NGC.SecurityLayer
{
    public class Session
    {
        #region Attributes
        private string sessionId;
        private string userName;
        private short userId;
        private string culture;
        private CultureInfo cultureInfo;
        private string capabilityXml;
        public string AppName { get; set; }
        #endregion

        #region Constants
        private const string defaultCulture = "default";
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
        public Session(Trace trace, string sessionId, string userName, string password, string cultureCode, string AppName)
        {

            #region Check Culture
            // Check that a valid culture has been supplied.
            try
            {
                new CultureInfo(cultureCode);  // test culture is valid for .Net
            }
            catch
            {
                CrmServiceException ce = new CrmServiceException("Client", "ParameterError", "UnknownCulture", trace, cultureCode);
                throw ce;
            }
            // The culture is valid, so set the cultureInfo and culture attributes.
            this.Culture = cultureCode;
            // Test culture is valid for CRM Server.
            if (!this.CheckForCulture(cultureCode))
                // The culture supplied does not exist in the culture table,
                // so set the culture attribute to use 'default'.
                this.culture = defaultCulture;
            #endregion

            this.UserName = userName;
            this.SessionId = sessionId;
            this.AppName = AppName;

            // Check the request can be given connection
            //Authenticate user with the Active directory
            if (AuthenticateUser(trace, userName, password))
            {
                trace.WriteDebug("Authentication OK for user : " + userName);
            }
            else
            {
                trace.WriteDebug("Authentication failed for user : " + userName);
                CrmServiceException ce = new CrmServiceException(
                    "User",
                    "SessionLacksPrivileges",
                    "Authentication Failed", trace);
                throw ce;
            }

            //Check the user is there in the db
            //If present get the user id
            //Otherwise insert the user into the table with no privileges
            this.UserId = (short)GetUser(trace);
            if (this.UserId != -1)
            {
                trace.WriteDebug("Validation Ok for User : " + userName);
            }
            else
            {
                trace.WriteDebug("Validation failed for user : " + userName);
                CrmServiceException ce = new CrmServiceException(
                    "User",
                    "SessionLacksPrivileges",
                    "Validation failed for user", trace);
                //throw ce;
            }

            // Record connection on the admin database
            if (this.UserId != -1)
            {
                this.AddRecord(trace);
            }
            // Setup user's privileges
            this.WriteCapabilityXml(this.UserId);
            // Set-up the thread session server culture.
            this.SetThreadUiCulture();
        }
                
        /// <summary>
        /// Unathenticated session has no privileges/capabilities, only use from NGC POS Server
        /// Complete the coding while diong the POS Server
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="userName"></param>
        /// <param name="culture"></param>
        /*public Session(Trace trace, string userName, string culture)
        {

            this.userId = null;
            this.sessionId = null;
            this.userName = userName;
            this.culture = culture;

            // Check the request can be given connection
            //****************************************************************************************
            //Authenticate user with the Active directory
            //****************************************************************************************
            try
            {
                if (reader.Read())
                {
                    this.userId = Convert.ToString(reader[0]);
                }
            }
            finally
            {
                reader.Close();
            }

            if (this.userId != null)
            {
                this.sessionId = Guid.NewGuid().ToString();
                this.AddRecord(trace);
            }
        }*/

        /// <summary>
        /// Construct an existing Session Identified by sessionId
        /// Check the session already exists, may have been created by another CRM Server
        /// </summary>
        /// <param name="sessionId">The session's ID</param>
        
        public Session(string sessionId)
        {
            this.SessionId = sessionId;
            // Validate the sessionId and get the user details
            String sSql = "SELECT  au.UserName,au.UserID, ads.ISOLanguageCode " +
                          " FROM ApplicationUser AS au INNER JOIN ASPState.dbo.AdminSession AS ads " +
                          "  ON au.UserID = ads.UserID  where ads.SessionID = '" + this.SessionId + "'";
            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                using (SqlDataReader Reader = SqlHelper.ExecuteReader(connectionString, System.Data.CommandType.Text, sSql))
                {
                    if (Reader.Read())
                    {
                        this.UserName = Convert.ToString(Reader[0]);
                        this.UserId = Convert.ToInt16(Reader[1]);
                        // Get the session culture.
                        string sessionCulture = Convert.ToString(Reader[2]);
                        // Set the culture to this value.
                        this.Culture = sessionCulture;
                        // Check if this exists in the database.
                        if (!this.CheckForCulture(sessionCulture))
                            // It doesn't exist, so alter the culture to 'default'
                            this.culture = defaultCulture;
                    }
                    else
                    {
                        // Throw exception as session is unknown to any CRM Server
                        CrmServiceException ce = new CrmServiceException(
                            "User",
                            "SessionLacksPrivileges",
                            "Connection.Validate", this.SessionId);
                        throw ce;
                    }
                }
            }
            catch (Exception e)
            {

            }
            // Setup user's privileges
            WriteCapabilityXml(this.UserId);
        }
        #endregion

        #region Methods
        private bool CheckForCulture(string cultureCode)
        {
            // Test culture is valid
            String sSql = "SELECT c.ISOLanguageCode " +
                          "FROM ISOLanguage c " +
                          "WHERE c.ISOLanguageCode = '" + cultureCode + "' ";
            try
            {
                string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]);
                using (SqlDataReader Reader = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sSql))
                {
                    if (Reader.Read())
                    {
                        Reader.Close();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        //Authenticate user against the active directory
        //Check if the user is part of the NGC group
        public bool AuthenticateUser(Trace trace, String userName, String password)
        {
            //String strADPath = "LDAP://HSCDC/dc=in,dc=tesco,dc=org";
            //String domainAndUserType = "in" + @"\" + userName;

            //Add below entries to web.config
            //LDAPServerName ="LDAP://HSCDC/"	 
            //LDAPBaseDN 	="DC=in,DC=tesco,DC=org"
            //LDAPGroupName ="CN=NGC"

            bool loginStatus = false;
            TraceState trState = trace.StartProc("Session.AuthenticateUser userName=" + userName);
            try
            {
                //Read the LDAP data from the Web.config file
                String strLDAPPath = Convert.ToString(ConfigurationSettings.AppSettings["LDAPPath"]);
                //String LDAPBaseDN = Convert.ToString(ConfigurationSettings.AppSettings["LDAPBaseDN"]);
                String LDAPGroupName = Convert.ToString(ConfigurationSettings.AppSettings["LDAPGroupName"]);
                //String strADPath = LDAPServerName + LDAPBaseDN;
                String domainAndUserType = Convert.ToString(ConfigurationSettings.AppSettings["Domain"]) + @"\" + userName;
                DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                DirectorySearcher search = new DirectorySearcher(objDirEntry);

                search.Filter = "(SAMAccountName=" + userName + ")";
                SearchResult result;
                result = search.FindOne();
                if (result.Path == "")
                {
                    loginStatus = false;
                }
                else
                {
                    loginStatus = true;

                }
                //ResultPropertyValueCollection valcol = result.Properties["memberOf"];
                //if (valcol.Count > 0)
                //{
                //    foreach (object o in valcol)
                //    {
                //        //check user exist in Group we are searching for
                //        //if (o.ToString().Contains(LDAPGroupName))
                //        //{
                //            loginStatus = true;
                //            break;
                //        //}
                //    }
                //}

            }
            catch (Exception ex)
            {
                trace.WriteError(ex.Message);
                return false;
            }
            finally
            {
                trState.EndProc();
            }

            return loginStatus;
        }
                
        
        //Check the user is present in the database table
        //Otherwise create the user with no privileges
        public int GetUser(Trace trace)
        {
            int retval = 0, identityParam = 0;
            TraceState trState = trace.StartProc("Session.GetUser userName=" + userName);
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
                CrmServiceException ce = new CrmServiceException(
                    "Server",
                    "SqlError",
                    "ConnectionRefused", trace);
                throw ce;
            }
            finally
            {
                trState.EndProc();
            }

            return retval;
        }

        
        private void AddRecord(Trace trace)
        {

            TraceState trState = trace.StartProc("Session.AddRecord SessionId=" + this.SessionId);
            try
            {
                //Execute SP to create a Session record in the DB using the DAL
                object[] objSession = { SessionId, UserId, Culture };
                string connectionString = ConfigurationSettings.AppSettings["SessionConnectionString"].ToString();
                SqlHelper.ExecuteNonQuery(connectionString, "USP_AddSession", objSession);
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
        #region Write User's Capability XML
        /// <summary>
        /// Write a capability XML Document and read it to a string
        /// </summary>
        private void WriteCapabilityXml(short userID)
        {

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
        }

        /// <summary>
        /// Find the Capabilities of the User and write them to the capability XML Document
        /// </summary>
        private void ProcessUserCapabilities(XmlTextWriter writer)
        {

            //Get the user capability data from the Database using DAL
            Trace trace = new Trace();

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
        }
        #endregion

        /// <summary>
        /// Update the session's record on the database, reflected it is active
        /// </summary>
        public void TouchRecord()
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("Session.TouchRecord SessionId=" + this.SessionId);

            // Execute Update Statement

            try
            {
                HiResTimer timer = new HiResTimer();
                try
                {
                    string conStr = ConfigurationSettings.AppSettings["SessionConnectionString"].ToString();
                    using (SqlConnection connection = new SqlConnection(conStr))
                    {
                        String sSql = "UPDATE AdminSession SET TouchTimestamp = CONVERT(varchar,GETUTCDATE(),126) WHERE SessionID = '" + this.SessionId + "'";
                        timer.Start();
                        SqlCommand command = new SqlCommand(sSql, connection);
                        command.ExecuteNonQuery();

                        //SqlHelper.ExecuteNonQuery(conStr,CommandType.Text, sSql);
                        timer.Stop();
                        trace.WriteDebug("Connection.TouchRecord Time=" + timer.ElapsedMilliseconds + " SQL=" + sSql);
                    }
                }
                finally
                {

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

        /// <summary>The session's Unique Identifier</summary>
        public string SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = value; }
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
        #endregion
    }
}
