#region
using System;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Web.SessionState;
#endregion

namespace Tesco.NGC.Utils

{
	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// A class to check about the current web session.
	/// </summary>
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.01" day="30" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Lawrie Griffiths</checker>
	///			<work_packet>WP/Barcelona/014</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	///		<version number="1.00" day="30" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Lawrie Griffiths</checker>
	///			<work_packet>WP/Barcelona/014</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header

	public class WebSession
	{
		#region Attributes
		private string currentMessage;
		private bool errorOccured = false;
		private string currPanel;
		#endregion

		#region Properties
		/// <summary>
		///  Gets or sets the panel that has instantiated
		///  the class.
		/// </summary>
		public string CurrentPanel 
		{
			get {return this.currPanel;}
			set {currPanel = value;}
		}

		/// <summary>
		///  Gets or sets the current message of the error, if
		///  there is one.
		/// </summary>
		public string Message 
		{
			get {return this.currentMessage;}
			set {currentMessage = value;}
		}

		/// <summary>
		/// Gets or sets the current instance to indicate that
		/// an error has been deduced from the error message.
		/// </summary>
		public bool ErrorOccured
		{
			get {return this.errorOccured;}
			set {errorOccured = value;}
		}
		#endregion

		#region Contructors
		/// <summary>
		/// Create a new instance of the CheckFor class to check for and
		/// action errors.
		/// </summary>
		public WebSession()
		{
		}

		/// <summary>
		/// Create a new instance of the CheckFor class to check for and
		/// action errors.
		/// </summary>
		/// <param name="panelName">The panel calling the method e.g. Search, Logon, Popup etc.</param>
		public WebSession(string panelName) 
		{
			this.CurrentPanel = panelName;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Check whether the web session has timed out.
		/// </summary>
		/// <returns>Returns true if the session has timed out.</returns>
		public bool CheckForWebSessionTimeout(HttpSessionState session) 
		{
			// Check to ensure that there are entries in the Session
			// hashtable.
			int count = session.Count;
			if (count > 0 || !StringUtils.IsStringEmpty(session.SessionID)) 
			{
				this.errorOccured = false;
			} 
			else 
			{
				this.Message = CrmStringBuilder.BuildMessage(CrmServiceException.ResourceList,"WebSessionTimedOut","");;
				this.ErrorOccured = true;
			}
			return this.ErrorOccured;
		}
		#endregion
	}
}
