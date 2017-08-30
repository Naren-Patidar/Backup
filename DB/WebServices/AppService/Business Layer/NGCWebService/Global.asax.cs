using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Tesco.NGC.Utils;
using NGCSecurityServiceLayer;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Tesco.NGC.Security;

namespace Tesco.NGC.NGCWebService
{

    #region Header
    ///
    /// <department>Fujitsu, e-Innovations, eCRM</department>
    /// <copyright>(c) Fujitsu Consulting, 2001</copyright>
    /// 
    /// <summary>
    /// Set up TCP Server Channel
    /// </summary>
    /// 
    /// <development> 
    ///    <version number="1.10" day="16" month="01" year="2003">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Steve Lang</checker>
    ///			<work_packet>WP/Barcelona/046</work_packet>
    ///			<description>Namespaces conform to standards</description>
    ///	</version>
    ///		<version number="1.02" day="13" month="01" year="2003">
    ///			<developer>Mark Hart</developer>
    ///			<checker>Stephen Lang</checker>
    ///			<work_packet>WP/Barcelona/045</work_packet>
    ///			<description>Add a call to XmlApplication close when application
    ///			ends.</description>
    ///		</version>
    ///		<version number="1.01" day="24" month="09" year="2002">
    ///			<developer>Mark Hart</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Ensure there are calls to Trace dispose method in finally block.</description>
    ///		</version>
    ///		<version number="1.00" day="19" month="11" year="2001">
    ///			<developer>Stephen Lang</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Set up TCP Server Channel</description>
    ///		</version>
    /// </development>
    /// 
    #endregion Header

    public class Global : System.Web.HttpApplication
    {
        public static SessionTable _sessionTable;
        public static SessionTable sessionTable
        {
            get { return _sessionTable; }
            set { _sessionTable = value; }
        }

        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            }
            Trace trace = new Trace();
            TraceState trState = trace.StartProc("Global.Application_Start");
            Initialise(trace);
            trace.EndProc(trState);
        }

        protected void Session_Start(Object sender, EventArgs e) { }

        protected void Application_BeginRequest(Object sender, EventArgs e) { }

        protected void Application_EndRequest(Object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e) { }

        protected void Application_Error(Object sender, EventArgs e) { }

        protected void Session_End(Object sender, EventArgs e) { }

        protected void Application_End(Object sender, EventArgs e)
        {
            Close();
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

        #region Initialise Admin Classes
        private static void Initialise(Trace trace)
        {
            // Add a call to SessionTable.Dispose to ensure that only 1 activity
            // thread is running at any one time. Check that an instance does
            // exist first.
            if (sessionTable != null)
            {
                sessionTable.Dispose();
            }
            // Set up Table of Sessions - initially empty
            sessionTable = new SessionTable(trace);
        }
        #endregion

        #region Close
        /// <summary>
        /// Close all resources tidily.
        /// </summary>
        public static void Close()
        {

            // Close the sessionTable, especially its thread
            try
            {
                sessionTable.Dispose();
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
            }

            // Now complete by calling the Trace close.
            Trace.Close();
        }
        #endregion

    }
}