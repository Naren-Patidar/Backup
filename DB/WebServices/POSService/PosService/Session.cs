using Fujitsu.eCrm.Generic.SharedUtils;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Configuration;
using System.Text.RegularExpressions;
using Fujitsu.eCrm.Generic.PosService;

namespace Fujitsu.eCrm.Generic.PosService.Security {

	#region Header
	///
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Maintains and manages a list sessions with current connections to the server.
	/// </summary>
	/// 
	/// <development>
	///    <version number="1.13" day="25" month="06" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Bug Fix</work_packet>
	///			<description>Store the determined culture in the Thread data storage. This may
	///			sometimes be 'default', so can't use the Thread UI culture.</description>
	///	   </version>
	///    <version number="1.12" day="12" month="06" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>WP/Barcelona/059</work_packet>
	///			<description>If a culture isn't present in admin_culture, then use 'default'.</description>
	///	   </version>
	///    <version number="1.11" day="13" month="02" year="2003">
	///			<developer>Eddie Tew</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/049</work_packet>
	///			<description>Authentication via CRMAuthenticate object identified in initialise.</description>
	///	   </version>
	///    <version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	///		<version number="1.02" day="14" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/045</work_packet>
	///			<description>Remove calls to Trace object Dispose, as this is now
	///			redundant.</description>
	///		</version>
	///		<version number="1.01" day="24" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Ensure there are calls to dispose method in all trace finally blocks.</description>
	///		</version>
	///		<version number="1.00" day="05" month="09" year="2002">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Contains details of a session in local memory.  The data is 
	///			regularly synchronised with the admin database.  Sessions are ended if
	///			they have been inactive longer than a configurable threshold</description>
	///		</version>
	/// </development>
	#endregion Header	

	public class Session 
	{

		#region Attributes
		private string sessionId;
		private string userName;
		private string userId;
		private string touchStatement;
		private string culture;
		private CultureInfo cultureInfo;
		private string capabilityXml;
		#endregion

		#region Constants
		private const string defaultCulture = "default";
        private static Regex literalExpression = new Regex(@"\A[^@\(\)]+\Z"); // does not contain '@', '(' or ')'
        private static Regex quoteExpression = new Regex("'");
        private static Regex stringTypeExpression = new Regex(@"\Astring(\(\d+\))?\Z");	// string or string(<digits>)
        private static Regex nstringTypeExpression = new Regex(@"\Anstring(\(\d+\))?\Z");	// string or string(<
		#endregion

		#region Construct
		/// <summary>
		/// Unathenticated session has no privileges/capabilities, only use from NGC POS Server
		/// </summary>
		/// <param name="trace"></param>
		/// <param name="userName"></param>
		/// <param name="culture"></param>
		public Session(ITrace trace,string userName,string culture) {

			this.userId = null;
			this.sessionId = null;
			this.userName = userName;
			this.culture = culture;

			string selectStatement = SelectUser(userName);
            AppSqlDataReader reader = new AppSqlDataReader(selectStatement, Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
			try {
				if (reader.Read()) {
					this.userId = Convert.ToString(reader[0]);
				}
			} finally {
				reader.Close();
			}

			if (this.userId != null) {
				this.sessionId = Guid.NewGuid().ToString();
				this.AddRecord(trace);
			}
		}


		private void AddRecord(ITrace trace) {

			ITraceState trState = trace.StartProc("Session.AddRecord SessionId="+this.SessionId);
			try {
				HiResTimer timer = new HiResTimer();

				string insertStatement = InsertSession(this.SessionId,this.UserId,this.CultureInfo.ToString());

				// Execute Insert Statement
				try {
                    SqlConnection sessionConnection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
					sessionConnection.Open();
					try {
						SqlCommand insertCommand = new SqlCommand(insertStatement,sessionConnection);
						timer.Start();
						insertCommand.ExecuteNonQuery();
						timer.Stop();
						trace.WriteDebug("Session.AddRecord Time="+timer.ElapsedMilliseconds+" SQL="+insertCommand);
					} finally {
						sessionConnection.Close();
					}
				} catch (Exception e) {
					HandleSqlExecutionException(e,trace,insertStatement);
				}
			} finally {
				trState.EndProc();
			}
		}


		/// <summary>
		/// Update the session's record on the Admin database, reflected it is active
		/// </summary>
		public void TouchRecord() {

			ITrace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
			ITraceState trState = trace.StartProc("Session.TouchRecord SessionId="+this.SessionId);

			// Execute Insert Statement
			try {
				HiResTimer timer = new HiResTimer();
                SqlConnection sessionConnection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
				sessionConnection.Open();
				try {
					SqlCommand touchCommand = new SqlCommand(this.TouchStatement,sessionConnection);
					timer.Start();
					touchCommand.ExecuteNonQuery();
					timer.Stop();
					trace.WriteDebug("Connection.TouchRecord Time="+timer.ElapsedMilliseconds+" SQL="+touchCommand);
				} finally {
					sessionConnection.Close();
				}
			} catch (Exception e) {
				HandleSqlExecutionException(e,trace,this.TouchStatement);
			} finally {
				trState.EndProc();

			}
		}

		/// <summary>
		/// Set the culture of the thread to the culture of the current user
		/// </summary>
		public void SetThreadUiCulture() {
			Thread.CurrentThread.CurrentUICulture = this.CultureInfo;
		}

		#region Properties
		/// <summary>The User Name of the session's owner</summary>
		public string UserName {
			get { return this.userName; }
			set { this.userName = value; }
		}

		/// <summary>The session's Unique Identifier</summary>
		public string SessionId { 
			get { return this.sessionId; } 
			set { this.sessionId = value; }
		}

		/// <summary>The User Idenitifier of the session's owner</summary>
		public string UserId { 
			get { return this.userId; } 
			set { this.userId = value; }
		}

		/// <summary>The Culture of the session's owner</summary>
		private string Culture { 
			get { return this.culture; }
			set { 
				this.culture = value;
				this.cultureInfo = new CultureInfo(value);
			}
		}

		/// <summary>The culture of the session's owner. This will have been
		/// checked against admin_culture and set to 'default' if not present.</summary>
		public string SessionCulture { get { return this.culture; } }

		/// <summary>The Culture of the session's owner</summary>
		private CultureInfo CultureInfo { get { return this.cultureInfo; } }

		/// <summary>SQL Statement to update the admin database</summary>
		private string TouchStatement { 
			get { return this.touchStatement; } 
			set { this.touchStatement = value; }
		}

        ///// <summary>The session's Service Privileges</summary>
        //public Privilege ApplicationServicePrivilege {
        //    get { return this.applicationServicePrivilege; } 
        //    set { this.applicationServicePrivilege = value; }
        //}

        ///// <summary>The session's Database Privileges</summary>
        //public Privilege ApplicationSqlCommandPrivilege {
        //    get { return this.applicationSqlCommandPrivilege; } 
        //    set { this.applicationSqlCommandPrivilege = value; }
        //}

		/// <summary>The session's Capability Privileges</summary>
		public string CapabilityXml {
			get { return this.capabilityXml; }
		}
		#endregion

        /// <summary>
        /// Retrieve the enabled user id
        /// </summary>
        /// <param name="userName">The session's username</param>
        /// <returns>The Generated SQL</returns>
        public string SelectUser(string userName)
        {
            // Need to encrypt the password to compare with database
            string formattedUserName = Format(userName, "string(50)", false);
            return
                "SELECT u.UserID " +
                "FROM ApplicationUser u " +
                "WHERE u.UserName = " + formattedUserName + " " +
                "AND u.UserStatusCode = 1 ";
        }


        /// <summary>
        /// Place quotes around a literal if it is a string or a date
        /// </summary>
        /// <param name="orginalDataValue">A literal without quotes</param>
        /// <param name="modelDataType">A literal without quotes</param>
        /// <param name="inSProc">Could any field be a parameter</param>
        /// <returns>A literal with quotes, if needed</returns>
        public string Format(string orginalDataValue, string modelDataType, bool inSProc)
        {
            if (orginalDataValue == null)
            {
                return this.Null;
            }
            if (inSProc)
            {
                if (!literalExpression.IsMatch(orginalDataValue))
                {
                    return orginalDataValue;
                }
            }
            string dataValue = orginalDataValue;
            if (nstringTypeExpression.IsMatch(modelDataType))
            {
                dataValue = quoteExpression.Replace(dataValue, "''");
                return this.Quote(dataValue, true);
            }
            if (stringTypeExpression.IsMatch(modelDataType) || (modelDataType == "guid") || (modelDataType == "boolean") || (modelDataType == "date"))
            {
                dataValue = quoteExpression.Replace(dataValue, "''");
                return this.Quote(dataValue, false);
            }
            return dataValue;
        }

        public string InsertSession(string sessionId, string userId, string culture)
        {
            return
                "INSERT INTO admin_session (session_crmid, usr_crmid, culture, touch_timestamp) " +
                "VALUES ('" + sessionId + "', '" + userId + "', '" + culture + "', " + this.GetUtcDate() + ") ";
        }
        #endregion

        /// <summary>
        /// Process the Exception and throw a CrmServiceException
        /// </summary>
        /// <param name="e">The SqlException</param>
        /// <param name="trace">The Trace file</param>
        /// <param name="sqlStatement">Strings to include in the CrmServiceException</param>
        public static void HandleSqlExecutionException(Exception e, ITrace trace, string sqlStatement)
        {

            string actor = "Server";
            string message = "ExecutionError";

            if (e is SqlException)
            {
                switch (((SqlException)e).Number)
                {
                    case -2:
                        //						// Not sure if -2 is only ever given for time outs, check text as well
                        Regex timeoutSeeker = new Regex("^Timeout expired");
                        if (timeoutSeeker.IsMatch(e.Message))
                        {
                            actor = "Server";
                            message = "TimeOut";
                        }
                        break;
                    case 2601:
                        actor = "Client";
                        message = "UniqueConstraint";
                        break;
                    default:
                        break;
                }
            }

            CrmServiceException e2 = new CrmServiceException(
                actor, "SqlError", message, e, trace, sqlStatement);
            throw e2;
        }

        /// <summary>
        /// Get UTC Datetime from Database in ISO8601 format
        /// yyyy-mm-ddThh24:mi:ss:mmm
        /// </summary>
        /// <returns>Function to get UTC Datetime</returns>
        public virtual string GetUtcDate()
        {
            return String.Empty;
        }

        /// <summary>
		/// Place quotes around a literal
		/// Additional prefix of N so that nvarchar is catered for [JSM 14/11/02]
		/// </summary>
		/// <param name="unquotedString">A literal without quotes</param>
		/// <param name="isUnicode">Identify the type of quote</param>
		/// <returns>A literal with quotes</returns>
		public string Quote(string unquotedString, bool isUnicode) {
			if (isUnicode) {
				return "N'"+unquotedString+"'";
			} else {
				return "'"+unquotedString+"'";
			}
		}

        /// <summary>
        /// SQL NULL
        /// </summary>
        public string Null
        {
            get { return "NULL"; }
        }
    }
}