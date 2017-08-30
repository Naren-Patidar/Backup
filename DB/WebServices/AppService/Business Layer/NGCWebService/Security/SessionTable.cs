using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Timers;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using Tesco.NGC.DataAccessLayer;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Data;

namespace Tesco.NGC.Security
{
    class SessionTable : Hashtable, IDisposable 
    {
        // Timer to check inactive sessions
        private System.Timers.Timer sessionTimer;
        private ReaderWriterLock rwLock;
        #region Constructor
        /// <summary>
        /// Create a table for sessions
        /// Set up a timed event to periodically disconnect inactive sessions
        /// </summary>
        public SessionTable(ITrace trace)
        {

            ITraceState trState = trace.StartProc("SessionTable.SessionTable Instance");
            try
            {
                this.RwLock = new ReaderWriterLock();

                // Set up a time event to check for inactive sessions
                this.SessionTimer = new System.Timers.Timer();
                Int32 ConnectionCheckInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["ConnectionCheckInterval"]);
                this.SessionTimer.Interval = ConnectionCheckInterval;
                this.SessionTimer.Elapsed += new ElapsedEventHandler(this.RemoveInactiveSessions);
                this.SessionTimer.Start();
            }
            finally
            {
                trState.EndProc();
            }
        }
        #endregion

        #region Add Session
        /// <summary>
        /// Add a new session to the table
        /// First checks if session should have session rights.
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="userName">The username of the person/system requesting session</param>
        /// <param name="password">The password of the person/system requesting session</param>
        /// <param name="culture">The culture of the person/system requesting session</param>
        /// <param name="capabilityXml">The capabilities of the person/system requesting session</param>
        /// <param name="newSession">The new Session object.</param>
        /// <returns>A new GUID (converted to string) assigned to the session</returns>
        public string Add(
            ITrace trace,
            string userName,
            string password,
            string culture,
            out string capabilityXml,
            out Session newSession)
        {

            ITraceState trState = trace.StartProc("SessionTable.Add UserName=" + userName);
            newSession = null;
            string sessionId;
            try
            {
                try
                {
                    sessionId = Guid.NewGuid().ToString();
                    newSession = new Session(trace, sessionId, userName, password, culture);
                    capabilityXml = newSession.CapabilityXml;
                    
                    // Record session
                    Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                    this.RwLock.AcquireWriterLock(WaitInterval);

                    try
                    {
                        this.Add(sessionId, newSession);
                    }
                    finally
                    {
                        this.RwLock.ReleaseWriterLock();
                    }
                }
                catch (Exception e)
                {
                    trace.WriteError(e.Message);
                    throw e;
                }
            }
            finally
            {
                trState.EndProc();
            }
            return sessionId;
        }
        
        /// <summary>
        /// Add a new session to the table
        /// First checks if session should have session rights.
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="userName">The username of the person/system requesting session</param>
        /// <param name="password">The password of the person/system requesting session</param>
        /// <param name="culture">The culture of the person/system requesting session</param>
        /// <param name="capabilityXml">The capabilities of the person/system requesting session</param>
        /// <param name="newSession">The new Session object.</param>
        /// <returns>A new GUID (converted to string) assigned to the session</returns>
        public string Add(
            ITrace trace,
            string userName,
            string password,
            string culture,
            out string capabilityXml)
        {

            ITraceState trState = trace.StartProc("SessionTable.Add UserName=" + userName);
            Session newSession = null;
            string sessionId;
            try
            {
                try
                {
                    sessionId = Guid.NewGuid().ToString();
                    newSession = new Session(trace, sessionId, userName, password, culture);
                    capabilityXml = newSession.CapabilityXml;
                    Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                    this.RwLock.AcquireWriterLock(WaitInterval);
                    try
                    {
                        this.Add(sessionId, newSession);
                    }
                    finally
                    {
                        this.RwLock.ReleaseWriterLock();
                    }
                }
                catch (Exception e)
                {
                    trace.WriteError(e.Message);
                    throw e;
                }
            }
            finally
            {
                trState.EndProc();
            }
            return sessionId;
        }
        #endregion

        #region Validate Session
        /// <summary>
        /// Check the session has session rights, and privileges for the requested service
        /// Throw an exception if it does not.
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="sessionId">The session's GUID requesting service</param>
        /// <param name="serviceName">The requested service</param>
        /// <returns>The current Session object.</returns>
        public Session Validate(
            ITrace trace,
            string sessionId,
            string serviceName)
        {

            Int16 WaitInterval = Convert.ToInt16(ConfigurationSettings.AppSettings["WaitInterval"]);
            this.RwLock.AcquireReaderLock(WaitInterval);
            try
            {
                Session currentSession = this.Validate(sessionId);
                //the method wasn't performing any task in Barcelona 
                //this[sessionId].Validate(serviceName);
                if ((serviceName != "Disconnect") && (serviceName != "Connect"))
                {
                    Thread touchRecordThread = new Thread(new ThreadStart(this[sessionId].TouchRecord));
                    touchRecordThread.Name = Guid.NewGuid().ToString();
                    trace.WriteThread(touchRecordThread.Name);
                    touchRecordThread.Start();
                }
                return currentSession;
            }
            finally
            {
                this.RwLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Check the session has session rights, and privileges for the requested service
        /// Throw an exception if it does not.
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="sessionId">The session's GUID requesting service</param>
        /// <param name="serviceName">The requested service</param>
        /// <param name="xmlDomainName">The requested domain</param>
        /// <param name="xmlViewName">The requested view</param>
        /// <returns>The current Session object.</returns>
        public Session Validate(
            ITrace trace,
            string sessionId,
            string serviceName,
            string xmlDomainName,
            string xmlViewName)
        {

            //Code to complete************************************************************************
            //WaitInterval=1000
            //Declare it in the Configuration file
            Int16 WaitInterval = Convert.ToInt16(ConfigurationSettings.AppSettings["WaitInterval"]);
            this.RwLock.AcquireReaderLock(WaitInterval);
            try
            {
                Session currentSession = this.Validate(sessionId);
                //the method wasn't performing any task in Barcelona 
                //this[sessionId].Validate(serviceName, xmlDomainName, xmlViewName);
                Thread touchRecordThread = new Thread(new ThreadStart(this[sessionId].TouchRecord));
                touchRecordThread.Name = Guid.NewGuid().ToString();
                trace.WriteThread(touchRecordThread.Name);
                touchRecordThread.Start();
                return currentSession;
            }
            finally
            {
                this.RwLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// Check the session has session rights.  Throw an exception if it does not.
        /// Else set session time of last activity to now.
        /// </summary>
        /// <param name="sessionId">The session's GUID requesting service</param>
        /// <returns>The current Session object.</returns>
        private Session Validate(
            string sessionId)
        {

            // If this session is not known locally
            if (!this.ContainsKey(sessionId))
            {
                // An exception is thrown if unable to find on database, let it raise upwards
                Session newSession = new Session(sessionId);
                // Record session locally
                Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                LockCookie cookie = this.RwLock.UpgradeToWriterLock(WaitInterval);
                try
                {
                    this.Add(sessionId, newSession);
                }
                finally
                {
                    this.RwLock.DowngradeFromWriterLock(ref cookie);
                }
            }
            // Set the threads culture to the culture of the current user
            Session currentSession = ((Session)this[sessionId]);
            currentSession.SetThreadUiCulture();
            return currentSession;
        }
        #endregion

        #region Remove Sessions
        /// <summary>
        /// Check all sessions for any that have been inactive longer than sessionInactiveThreshold.
        /// Disconnect any that have been inactive too longer.
        /// Sychronise the table with the database's table
        /// </summary>
        private void RemoveInactiveSessions(object sender, ElapsedEventArgs e)
        {

            HiResTimer timer = new HiResTimer();
            timer.Start();
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            ITraceState trState = trace.StartProc("SessionTable.RemoveInactiveSessions");
            int nSessionsRemoved = 0;

            try
            {
                Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                try
                {
                    // Get list of active sessions from the database
                    ArrayList activeSessionList = new ArrayList();

                    //"SELECT s.session_crmid FROM admin_session s WHERE s.touch_timestamp > CONVERT(VARCHAR,DATEADD(ms,-600000,GETUTCDATE()),126)"
                    Int16 ConnectionInactiveThreshold = Convert.ToInt16(ConfigurationSettings.AppSettings["ConnectionInactiveThreshold"]);
                    string sSql = "SELECT s.sessionID " +
                                  "FROM AdminSession s " +
                                  "WHERE s.TouchTimestamp > CONVERT(VARCHAR,DATEADD(ms,-" + ConnectionInactiveThreshold + ",GETUTCDATE()),126) ";

                    SqlDataReader Reader = SqlHelper.ExecuteReader(CommandType.Text, sSql);
                    try
                    {
                        while (Reader.Read())
                        {
                            activeSessionList.Add(Convert.ToString(Reader[0]));
                        }
                    }
                    finally
                    {
                        Reader.Close();
                    }

                    string[] keyArray = new string[this.Keys.Count];
                    this.Keys.CopyTo(keyArray, 0);
                    foreach (string sessionId in keyArray)
                    {
                        if (!activeSessionList.Contains(sessionId))
                        {
                            trace.WriteDebug("Disconnect session " + sessionId);
                            base.Remove(sessionId);
                            nSessionsRemoved++;
                        }
                    }
                }
                finally
                {
                    this.RwLock.ReleaseWriterLock();
                }
            }
            catch (Exception e2)
            {
                trace.WriteError(e2.Message);
            }
            finally
            {
                trState.EndProc();
                timer.Stop();
            }
        }

        /// <summary>
        /// Remove the session from the session table.  This can be requested by the 
        /// session or by the application Server.
        /// </summary>
        /// <param name="trace">Instance of the trace file</param>
        /// <param name="sessionId">The session's GUID to be disconnected</param>
        public void Remove(
            ITrace trace,
            string sessionId)
        {

            ITraceState trState = trace.StartProc("SessionTable.Remove SessionId=" + sessionId);
            try
            {
                // Record session locally
                Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                this.RwLock.AcquireWriterLock(WaitInterval);
                try
                {
                    base.Remove(sessionId);
                }
                finally
                {
                    this.RwLock.ReleaseLock();
                }
            }
            finally
            {
                trState.EndProc();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get/Set a session in the session table
        /// </summary>
        public Session this[string sessionId]
        {
            get { return (Session)base[sessionId]; }
            set { base[sessionId] = value; }
        }

        private System.Timers.Timer SessionTimer
        {
            get { return this.sessionTimer; }
            set { this.sessionTimer = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReaderWriterLock RwLock
        {
            get { return this.rwLock; }
            set { this.rwLock = value; }
        }
        #endregion

        #region Destructors
        /// <summary>
        /// Release all resources being used by SessionTable().
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all resources being used by SessionTable().
        /// </summary>
        private void Dispose(bool disposing)
        {
            // Ensure that the timer is stopped and disposed for this
            // current session.
            this.SessionTimer.Stop();
            this.SessionTimer.Dispose();
        }

        /// <summary>
        /// If session is disposed then the timer should be ended.
        /// </summary>
        ~SessionTable()
        {
            Dispose(false);
        }
        #endregion

    }
}
