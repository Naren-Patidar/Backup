#region Using
using System;
#endregion

namespace Tesco.NGC.Utils

{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Provide consistent indexes to use with the Session state
	/// dictionary.
	/// </summary>
	/// <development> 
	///    <version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.03" day="06" month="11" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>WP/Barcelona/017</work_packet>
	///			<description>Added new index for ReportXml and ReportName.</description>
	///		</version>
	///		<version number="1.03" day="06" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>WP/Barcelona/031</work_packet>
	///			<description>Added new index for LocalXml and SessionXml.</description>
	///		</version>
	///		<version number="1.02" day="17" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>Wp/barcelona/014</work_packet>
	///			<description>Added new index for ASP session ID.</description>
	///		</version>
	///		<version number="1.02" day="17" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>Wp/barcelona/015</work_packet>
	///			<description>Added new capabilities</description>
	///		</version>
	///		<version number="1.01" day="16" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Lawrie Griffiths</checker>
	///			<work_packet>Wp/barcelona/014</work_packet>
	///			<description>Added new indexes for new exception handling.</description>
	///		</version>
	///		<version number="1.00" day="16" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Lawrie Griffiths</checker>
	///			<work_packet>Wp/barcelona/014</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class WebSessionIndexes
	{
		#region Static
		/// <summary>
		/// The name of the index to use when storing a culture for
		/// a web session.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing the culture of the web session.</returns>
		public static string Culture () {return "culture";}
		/// <summary>
		/// The name of the index to use when storing the installed culture for
		/// a web session.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing the installed culture of the web session.</returns>
		public static string InstalledCulture () {return "InstalledCulture";}

		/// <summary>
		/// The name of the index to use when storing the current
		/// customer_crmid.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing the current customer_crmid.</returns>
		public static string CustomerCrmId () {return "customer_crmid";}

		/// <summary>
		/// The name of the index to use when storing a crm id
		/// for a particular node of the XML document.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing the crm id for the current node of the
		/// XML document.</returns>
		public static string ElementCrmId () {return "elementCrmId";}

		/// <summary>
		/// The name of the index to use when storing an error
		/// message to be accessed on a different web page.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when an error that may need to be accessed on a different
		/// page.</returns>
		public static string ExceptionMessage () {return "exceptionMessage";}

		/// <summary>
		/// The name of the index to use when storing an XML docuemnt
		/// that will only be in scope for the duration of the page.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when an XML document that will only be in scope for the
		/// duration of the page.</returns>
		public static string LocalXmlDoc () {return "localXmlDoc";}

		/// <summary>
		/// The name of the index to use when storing the XML
		/// that will only be in scope for the duration of the page.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when an XML string will only be in scope for the
		/// duration of the page.</returns>
		public static string LocalXml () {return "localXml";}

		/// <summary>
		/// The name of the index to use when storing the address of
		/// a web page.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// storing the address of a web page.</returns>
		public static string PageAddress () {return "pageAddress";}

		/// <summary>
		/// The name of the index to use when storing the password of
		/// the user.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// storing the password of the user.</returns>
		public static string Password () {return "password";}

		/// <summary>
		/// The name of the index to use when storing a session
		/// crm for the current data view.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing a top level crmid for the current session.</returns>
		public static string SessionCrmId () {return "sessionCrmId";}

		/// <summary>
		/// The name of the index to use when storing a session
		/// id returned from the CRM server.
		/// </summary>
		/// <returns>A string containing the name of the index to use
		/// when storing a session id returned from the CRM server.</returns>
		public static string SessionId () {return "sessionId";}

		/// <summary>
		/// The name of the index to use when storing the current
		/// session XML document.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when storing the current session XML document.</returns>
		public static string SessionXmlDoc () {return "sessionXmlDoc";}

		/// <summary>
		/// The name of the index to use when storing the current
		/// session XML string.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when storing the current session XML string.</returns>
		public static string SessionXml () {return "sessionXml";}

		/// <summary>
		/// The name of the index to use when storing the current
		/// user.
		/// </summary>
		/// <returns>A string containing the name of the index to use 
		/// when storing the current user.</returns>
		public static string UserId () {return "userId";}

		/// <summary>
		/// The name of the source of the exception. This is specifically
		/// for unhandled errors that will proporgate through to the
		/// global.asax handlers.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ExceptionSource () {return "exceptionSource";}

		/// <summary>
		/// The date and time of the exception.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ExceptionDateAndTime () {return "exceptionDateAndTime";}

		/// <summary>
		/// The ID of the exception.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ExceptionErrorId () {return "exceptionErrorId";}

		/// <summary>
		/// The stacktrace of the exception. This is specifically
		/// for unhandled errors that will proporgate through to the
		/// global.asax handlers.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ExceptionStackTrace () {return "exceptionStackTrace";}

		/// <summary>
		/// The type of the exception. This is specifically for unhandled
		/// errors that will proporgate through to the global.asax handlers.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ExceptionType () {return "exceptionType";}

		/// <summary>
		/// The name of the index to use when storing the ASP session ID
		/// allocated.
		/// </summary>
		/// <returns>A string containing the name of the index.</returns>
		public static string ASPSessionID() {return "ASPSessionID";}

		/// <summary>
		/// The name of the index to use when storing the user's capabilities.
		/// </summary>
		/// <returns>A string which indexes the capabilities in the session</returns>
		public static string Capabilities() {return "capabilities";}

		/// <summary>
		/// The name of the index to use when storing the name of report script file.
		/// </summary>
		/// <returns>A string which indexes the report file name in the session</returns>
		public static string ReportName() {return "reportname";}

		/// <summary>
		/// The name of the index to use when storing the xml form of a report.
		/// </summary>
		/// <returns>A string which indexes the report xml in the session</returns>
		public static string ReportXml() {return "reportXml";}

		/// <summary>
		/// The name of the index to use when storing log file path outputted from
		/// the execution of a batch script.
		/// </summary>
		/// <returns>A string which indexes the report xml in the session</returns>
		public static string BatchScriptLogFileName() {return "batchScriptLogFileName";}

		/// <summary>
		/// The name of the index to use when storing the name of the customer.
		/// </summary>
		/// <returns>A string which indexes the customer name in the session</returns>
		public static string CustomerName() {return "customerName";}

		/// <summary>
		/// The card number of the current customer.
		/// </summary>
		/// <returns>A string which indexes the card number in the session</returns>
		public static string CustomerCardNumber() {return "customercardNumber";}

		/// <summary>
		/// Is the current customer a primary customer ?
		/// </summary>
		/// <returns>A string which indexes whether the customer
		/// is primary in the session</returns>
		public static string CustomerPrimary() {return "customerPrimary";}

		/// <summary>
		/// Is the current customer a skeleton customer ?
		/// </summary>
		/// <returns>A string which indexes whether the customer
		/// is a skeleton in the session</returns>
		public static string CustomerSkeleton() {return "customerSkeleton";}

		/// <summary>
		/// The current customer XML.
		/// </summary>
		/// <returns>A string which indexes the current customer XML in the session</returns>
		public static string CustomerXml() {return "customerXml";}

		/// <summary>
		/// Store the customer card account crmid.
		/// </summary>
		/// <returns></returns>
		public static string CustomerCardAccountCrmId() {return "customerCardAccountCrmId";}

		/// <summary>
		/// Store the customer card account status code.
		/// </summary>
		/// <returns></returns>
		public static string CustomerCardAccountStatusCode() {return "customerCardAccountStatusCode";}

		/// <summary>
		/// Store the customer use status.
		/// </summary>
		/// <returns></returns>
		public static string CustomerUseStatusDescription() {return "customerUseStatusDescription";}

		/// <summary>
		/// Store the customer mail status.
		/// </summary>
		/// <returns></returns>
		public static string CustomerMailStatusDescription() {return "customerMailStatusDescription";}

		/// <summary>
		/// Store the customer use status.
		/// </summary>
		/// <returns></returns>
		public static string CustomerUseStatusCode() {return "customerUseStatusCode";}

		/// <summary>
		/// Store the number of customers in the reward group..
		/// </summary>
		/// <returns></returns>
		public static string CustomerRewardGroupSize() {return "customerRewardGroupSize";}

		/// <summary>
		/// Store the customer mail status.
		/// </summary>
		/// <returns></returns>
		public static string CustomerMailStatusCode() {return "customerMailStatusCode";}

		/// <summary>
		/// Store the URL address of the current page being visited. This is useful for
		/// returning to a page just visited.
		/// </summary>
		/// <returns></returns>
		public static string ReferrerUrl() {return "referrerUrl";}

		/// <summary>
		/// Store the URL address of the previous page being visited. This is useful for
		/// returning to a page just visited.
		/// </summary>
		/// <returns></returns>
		public static string LastReferrerUrl() {return "lastReferrerUrl";}

		/// <summary>
		/// Store whether the search for a customer was done using a card number.
		/// </summary>
		/// <returns></returns>
		public static string CustomerCardAccountPrimary() {return "customerCardAccountPrimary";}

		#endregion
	}
}
