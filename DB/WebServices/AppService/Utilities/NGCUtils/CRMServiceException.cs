using Microsoft.ApplicationBlocks.ExceptionManagement;
using System;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Xml.Schema;

namespace Tesco.NGC.Utils {

	#region Header
	///
	/// <summary>
	/// CRM Service Standard Solution for handling Application Errors
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.00" day="06" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class CrmServiceException : BaseApplicationException {

		/// <summary>The Enumeration of Systems Responsible for an Error</summary>
		public enum ActorType {
			/// <summary>The Client</summary>
			Client,
			/// <summary>The Library (i.e. Client or Server)</summary>
			Library,
			/// <summary>The Server</summary>
			Server,
			/// <summary>The User</summary>
			User
		}

		/// <summary>The Enumeration of Error Types</summary>
		public enum ErrorType {
			/// <summary>The Error is due to wrong meta data</summary>
			ConfigurationError,
			/// <summary>The code is buggy, this should not occur</summary>
			InternalError,
			/// <summary>The method received inappropriate data</summary>
			ParameterError,
			/// <summary>The User is not allowed in perform the requested action</summary>
			SessionLacksPrivileges,
			/// <summary>The Error is due to a problem with the Database</summary>
			SqlError,
			/// <summary>The Error is due to a problem with XML Documents</summary>
			XmlError
		}

		#region Constructor
		/// <summary>
		/// Create an Exception that incorporates an inner exception
		/// The message is generated from a named resource
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="innerException">The original exception</param>
		/// <param name="trace">The Trace, where messages are written</param>
		/// <param name="args">The values to place in the message template</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName, 
			Exception innerException, 
			Trace trace, 
			params string[] args) : base(CrmStringBuilder.BuildMessage(CrmServiceException.resourceList, messageName, args),innerException) {

			this.InitialiseAdditionalInformation(actor,category,messageName, args);
			this.Trace(trace);
		}

		/// <summary>
		/// Create an Exception that incorporates an inner exception
		/// The message is generated from a named resource
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="innerException">The original exception</param>
		/// <param name="args">The values to place in the message template</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName, 
			Exception innerException, 
			params string[] args) : base(CrmStringBuilder.BuildMessage(CrmServiceException.resourceList, messageName, args),innerException) {

			this.InitialiseAdditionalInformation(actor,category,messageName, args);
			this.Trace();
		}

		/// <summary>
		/// Create an Exception that does not incorporate an inner exception
		/// The message is generated from a named resource
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="trace">The Trace, where messages are written</param>
		/// <param name="args">The values to place in the message template</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName, 
			Trace trace, 
			params string[] args) : base(CrmStringBuilder.BuildMessage(CrmServiceException.resourceList, messageName, args)) {

			this.InitialiseAdditionalInformation(actor,category,messageName, args);
			this.Trace(trace);
		}

		/// <summary>
		/// Create an Exception that does not incorporate an inner exception
		/// The message is generated from a named resource
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="args">The values to place in the message template</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName, 
			params string[] args) : base(CrmStringBuilder.BuildMessage(CrmServiceException.resourceList, messageName, args)) {

			this.InitialiseAdditionalInformation(actor,category,messageName, args);
			this.Trace();
		}

		/// <summary>
		/// Create an Exception that incorporates an inner exception
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="message">The message in the UI Culture</param>
		/// <param name="innerException">The original exception</param>
		/// <param name="trace">The Trace, where messages are written</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName,
			string message,
			Exception innerException,
			Trace trace) : base(message,innerException) {

			InitialiseAdditionalInformation(actor,category,messageName,message);
			this.Trace(trace);
		}

		/// <summary>
		/// Create an Exception that does not incorporate an inner exception
		/// </summary>
		/// <param name="actor">The system responsible for the error</param>
		/// <param name="category">The error category</param>
		/// <param name="messageName">The name of the resource that forms a template error message</param>
		/// <param name="message">The message in the UI Culture</param>
		/// <param name="trace">The Trace, where messages are written</param>
		public CrmServiceException(
			string actor,
			string category,
			string messageName,
			string message,
			Trace trace) : base(message) {

			InitialiseAdditionalInformation(actor,category,messageName,message);
			this.Trace(trace);
		}

		private void InitialiseAdditionalInformation(
			string actor,
			string category,
			string messageName, 
			string uiMessage) {

			this.AdditionalInformation.Add("Actor",actor);
			this.AdditionalInformation.Add("Category",category);
			this.AdditionalInformation.Add("Name",messageName);
			this.AdditionalInformation.Add("UiMessage",uiMessage);
		}

		private void InitialiseAdditionalInformation(
			string actor,
			string category,
			string messageName, 
			params string[] args) {

			string uiMessage = CrmStringBuilder.BuildMessage(CrmServiceException.resourceList, messageName, args);
			InitialiseAdditionalInformation(actor,category,messageName,uiMessage);
		}

		private void Trace() {
			this.Trace(new Trace());
		}

		private void Trace(Trace trace) {
			if (trace != null) {
				trace.WriteError(this.Message + this.StackTrace);
			}
		}
		#endregion

		#region Static
		private static Stack resourceList;

		static CrmServiceException() {
			CrmServiceException.resourceList = new Stack();
			CrmServiceException.resourceList.Push(
				new ResourceManager("Tesco.NGC.Utils.Resources.ErrorMessages",Assembly.GetExecutingAssembly()));
		}

		/// <summary>
		/// Add an additional resource manager for CrmServiceException
		/// </summary>
		/// <param name="extraResourceManager">The additional resource manager</param>
		public static void AddResourceManager(ResourceManager extraResourceManager) {
			CrmServiceException.resourceList.Push(extraResourceManager);
		}
		
		/// <summary>
		/// The method is invoke when a XML document fails validation
		/// Raise an exception
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">Details of the XML validation error</param>
		public static void ThrowXmlValidationError(object sender, ValidationEventArgs e) {
			throw (
				new CrmServiceException(
					"Library",
					"XmlError",
					"XmlValidationError",e.Message));
		}
		#endregion

		#region Properties
		/// <summary>Gets the System Responsible for the Error</summary>
		/// <returns>The System Responsible for the Error</returns>
		public string Actor() { return this.AdditionalInformation.Get("Actor"); }
		
		/// <summary>Gets the Category of the Error</summary>
		/// <returns>The Category of the Error</returns>
		public string Category() { return this.AdditionalInformation.Get("Category"); }
		
		/// <summary>Gets the Name of the Error</summary>
		/// <returns>The Name of the Error</returns>
		public string Name() { return this.AdditionalInformation.Get("Name"); }
		
		/// <summary>Gets the Error Message in the User's Culture</summary>
		/// <returns>The Error Message in the User's Culture</returns>
		public string UiMessage() { return this.AdditionalInformation.Get("UiMessage"); }

		/// <summary>List of Resources to Error Messages</summary>
		public static Stack ResourceList { get { return CrmServiceException.resourceList; } }
		#endregion
	}
}