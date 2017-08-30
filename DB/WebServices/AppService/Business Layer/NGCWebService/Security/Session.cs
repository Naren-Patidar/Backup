using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;
using Fujitsu.eCrm.Generic.SharedUtils;
using Tesco.NGC.DataAccessLayer;
using System.Resources;
using System.Timers;
using System.Threading;
using System.Xml;
using System.IO;
using System.EnterpriseServices;
using System.DirectoryServices;
using System.Data;


namespace Tesco.NGC.Security
{
    class Session
    {
        #region Attributes
        private string sessionId;
        private string userName;
        private short userId;
        private string culture;
        private CultureInfo cultureInfo;
        private string capabilityXml;
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
        public Session(ITrace trace, string sessionId, string userName, string password, string cultureCode)
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

            // Check the request can be given connection
            //****************************************************************************************
            //Authenticate user with the Active directory
            if (AuthenticateUser(userName, password))
            //****************************************************************************************
            {
                trace.WriteDebug("Authentication OK for user : " + userName);
            }
            else
            {
                trace.WriteDebug("Authentication failed for user : " + userName);
                CrmServiceException ce = new CrmServiceException(
                    "User",
                    "SessionLacksPrivileges",
                    "ConnectionRefused", trace);
                throw ce;
            }

            //Authenticate(trace,userName,password);

            // Record connection on the admin database
            this.AddRecord(trace);
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
        /*public Session(ITrace trace, string userName, string culture)
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
            String sSql = "SELECT  au.UserName,au.UserID, adu.ISOLanguageCode " +
                         "FROM dbo.ApplicationUser AS au INNER JOIN dbo.AdminSession AS ads " +
                         "ON au.UserID = ads.UserID " +
                         "WHERE ads.SessionID = '" + this.SessionId + "'";
            try
            {
                using (SqlDataReader Reader = SqlHelper.ExecuteReader(System.Data.CommandType.Text, sSql))
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
                using (SqlDataReader Reader = SqlHelper.ExecuteReader(CommandType.Text, sSql))
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
        bool AuthenticateUser(ITrace trace, String userName, String password)
        {
            //String strADPath = "LDAP://HSCDC/dc=in,dc=tesco,dc=org";
            //String domainAndUserType = "in" + @"\" + userName;

            //Add below entries to web.config
            //LDAPServerName ="LDAP://HSCDC/"	 
            //LDAPBaseDN 	="DC=in,DC=tesco,DC=org"
            //LDAPGroupName ="CN=NGC"

            bool loginStatus = false;
            ITraceState trState = trace.StartProc("Session.AuthenticateUser userName=" + userName);
            try
            {
                //Read the LDAP data from the Web.config file
                String LDAPServerName = Convert.ToString(ConfigurationSettings.AppSettings["LDAPServerName"]);
                String LDAPBaseDN = Convert.ToString(ConfigurationSettings.AppSettings["LDAPBaseDN"]);
                String LDAGroupName = Convert.ToString(ConfigurationSettings.AppSettings["GroupName"]);
                String strADPath = LDAPServerName + LDAPBaseDN;
                DirectoryEntry objDirEntry = new DirectoryEntry(strADPath, userName, password);
                DirectorySearcher search = new DirectorySearcher(objDirEntry);

                search.Filter = "(SAMAccountName=" + userName + ")";
                SearchResult result;
                result = search.FindOne();
                ResultPropertyValueCollection valcol = result.Properties["memberOf"];
                if (valcol.Count > 0)
                {
                    foreach (object o in valcol)
                    {
                        //check user exist in Group we are searching for
                        if (o.ToString().Contains(LDAPGroupName))
                        {
                            loginStatus = true;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                trace.WriteError(e2.Message);
                return false;
            }
            finally
            {
                trace.EndProc();
            }

            return loginStatus;
        }
        private void AddRecord(ITrace trace)
        {

            ITraceState trState = trace.StartProc("Session.AddRecord SessionId=" + this.SessionId);
            try
            {
                HiResTimer timer = new HiResTimer();
				try {
                   timer.Start();
                   //Execute SP to create a Session record in the DB using the DAL
                   SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "addSession");
                timer.Stop();
                trace.WriteDebug("Session.AddRecord Time="+timer.ElapsedMilliseconds+" SP=addSession");
            }
            catch (Exception e)
            {
                trace.WriteError(e.Message);
            }
			} finally {
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
            writer.WriteAttributeString("xmlns:crm",ConfigurationSettings.AppSettings["CapabilitiesXsdNamespace"]);
            // Add child element nodes to the capabilities node
            ProcessUserCapabilities(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            // Rewind memory stream, and read the XML from memory into a string
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(stream);
            string capabilityXml = reader.ReadToEnd();
            reader.Close();

            // Set the capability xml
            this.CapabilityXml=capabilityXml;
        }

        /// <summary>
        /// Find the Capabilities of the User and write them to the capability XML Document
        /// </summary>
        private void ProcessUserCapabilities(XmlTextWriter writer)
        {

            //Get the user capability data from the Database using DAL
            ITrace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            String sSql = "SELECT  c.CapabilityID, c.CapabilityName "+
                          "FROM    dbo.ApplicationUser AS au INNER JOIN "+
                          " dbo.UserInRole ON au.UserID = dbo.UserInRole.UserID INNER JOIN"+
                          " dbo.Role ON dbo.UserInRole.RoleID = dbo.Role.RoleID INNER JOIN"+
                          " dbo.RoleCapability INNER JOIN"+
                          " dbo.Capability AS c ON dbo.RoleCapability.CapabilityID = c.CapabilityID ON dbo.Role.RoleID = dbo.RoleCapability.RoleID"+
                          " WHERE au.UserID = " + this.SessionId ;
            
            try
            {
                SqlDataReader Reader = SqlHelper.ExecuteReader(System.Data.CommandType.Text, sSql);
                while (Reader.Read())
                {
                    Int16 capabilityID = (Int16)Reader[0];
                    string capabilityName = (string)Reader[1];
                    writer.WriteStartElement(capabilityName);
                    //ProcessProperties(capabilityCrmId, writer);
                    writer.WriteEndElement();
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

            ITrace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("Session.TouchRecord SessionId=" + this.SessionId);

            // Execute Update Statement

            try
            {
                HiResTimer timer = new HiResTimer();
                try
                {
                    String sSql = "UPDATE AdminSession SET TouchTimestamp = CONVERT(varchar,GETUTCDATE(),126) WHERE SessionID = '" + this.SessionId + "'";
                    timer.Start();
                    SqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, sSql);
                    timer.Stop();
                    trace.WriteDebug("Connection.TouchRecord Time=" + timer.ElapsedMilliseconds + " SQL=" + sSql);
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
            Thread.CurrentThread.CurrentUICulture = this.CultureInfo;
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
        private string Culture
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
            set { this.CapabilityXml = value; }
        }
        #endregion
    }
}
