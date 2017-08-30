#region Using
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Resources;
using System.Reflection;
using System.Net;
using System.Web;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils

{

	#region Header
	///
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2001</copyright>
	/// 
	/// <summary>
	/// Manages resultXml used by the services
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.01" day="22" month="11" year="2001">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Add resultFlag to object, if ANY result is an error or a 
	///			warning then the result should be false, otherwise it will be true</description>
	///		</version>
	///		<version number="1.00" day="19" month="11" year="2001">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class Result : XmlDocument 
	{

		#region Attribute
		private bool flag;
		private XmlElement currentElement;
		private XmlElement budElement;
		#endregion

		#region Static

		private static string nameSpace;
		private static string xsd;

		/// <summary>
		/// Setup the Result Class
		/// </summary>
		/// <param name="xsdDirectoryPath"></param>
		/// <param name="nameSpace"></param>
		public static void Initialise(string xsdDirectoryPath, string nameSpace) {
			Result.NameSpace = nameSpace;
			Result.Xsd = 			
				"<?xml version=\"1.0\" encoding=\"utf-8\" ?>"+
				"<xsd:schema id=\"result_list\" targetNamespace=\""+Result.NameSpace+"\" xmlns=\""+Result.NameSpace+"\" elementFormDefault=\"qualified\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">"+
					"<xsd:element name=\"result_list\">"+
						"<xsd:complexType>"+
							"<xsd:choice maxOccurs=\"unbounded\">"+
								"<xsd:element name=\"result\">"+
									"<xsd:complexType>"+
										"<xsd:sequence>"+
											"<xsd:element name=\"name\" type=\"xsd:string\" minOccurs=\"1\" />"+
											"<xsd:element name=\"ui_message\" type=\"xsd:string\" minOccurs=\"1\" />"+
											"<xsd:element name=\"actor\" type=\"xsd:string\" minOccurs=\"0\" />"+
											"<xsd:element name=\"category\" type=\"xsd:string\" minOccurs=\"0\" />"+
										"</xsd:sequence>"+
									"</xsd:complexType>"+
								"</xsd:element>"+
							"</xsd:choice>"+
						"</xsd:complexType>"+
					"</xsd:element>"+
				"</xsd:schema>";
			
			try {
				string path = Path.Combine(xsdDirectoryPath,"result_list.xsd");
				TextWriter streamWriter = new StreamWriter(path,false,Encoding.UTF8);
				try {
					streamWriter.Write(Result.Xsd);
				} finally {
					streamWriter.Flush();
					streamWriter.Close();
				}
			} catch {
				// Not critical, allow application to continue
			}
		}

		/// <summary>
		/// Get the user friendly error message when a non-CrmServerException
		/// has occured.
		/// </summary>
		/// <param name="exception">The exception that has occured.</param>
		/// <returns>The error message to be displayed to the client.</returns>
		public static string GetClientErrorMessageFromException (Exception exception)
		{
			string index = "";
			// Try and decide whether this is a nice CRM exception with
			// a sensible message or whether it is a nasty unhandled
			// way too much information. Decide by seeing if the message
			// is less than 400 characters, it must be a CRM exception.
			// Also, if the message starts with a least 2 integers then
			// we will assume that it is a crm server exception.
			string rawMessage = exception.Message;
			Regex r = new Regex("^[0-9][0-9]");
			Match m = r.Match(rawMessage);
			if (rawMessage.Length < 400 || m.Success)
				// It is a nice exception, so just return this message.
				return exception.Message;

			// It wasn't a nice exception, so try and work out what
			// kind of exception it was.
			if (exception is HttpException) 
				index = "HttpException";
			else if (exception is InvalidOperationException) 
				index = "InvalidOperationException";
			// WebException does not inherit Exception Class
			//else if (exception is WebException) 
			//	index = "WebException";
			else if (exception is MissingManifestResourceException) 
				index = "MissingManifestResourceException";
			else 
				index = "UnknownException";

			// See if there is a sensible message in the resource file
			// for this error.
			string message = CrmStringBuilder.BuildMessage(CrmServiceException.ResourceList,index,"");
			return message;
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Construct a result instance. Assume it is successful.
		/// Add top nodes to the XML Document
		/// </summary>
		public Result() 
		{
			this.Flag = true;
			this.BudElement = this.CreateElement("result_list");

			XmlAttribute xmlNamespace = this.CreateAttribute("xmlns:crm");
			xmlNamespace.Value = Result.NameSpace;
			this.BudElement.Attributes.Append(xmlNamespace);
			this.AppendChild(this.BudElement);
		}
		#endregion

		#region FromString
		/// <summary>
		/// Load the result instance from a string containing a result's XML Document
		/// </summary>
		/// <param name="resultXml">The string containing the result's XML Document</param>
		public override void LoadXml(string resultXml) 
		{
			base.LoadXml(resultXml);
			this.Flag = flag;

			this.BudElement = (XmlElement)this.SelectSingleNode("./result_list");
			XmlNodeList resultList = this.BudElement.SelectNodes("//result");
			if (resultList.Count != 0) 
			{
				this.BudElement = (XmlElement)resultList[resultList.Count-1];
				this.Flag = false;
			}
		}
		#endregion

		#region ToException

		/// <summary>
		/// Build an exception based on this result
		/// </summary>
		/// <returns>The new Exception (it is either an Exception or an CrmServiceException)</returns>
		public Exception ToException() 
		{
			return this.ToException(new Trace());
		}

		/// <summary>
		/// Build an exception based on this result
		/// </summary>
		/// <param name="trace">Error Messages are sent to Trace</param>
		/// <returns>The new Exception (it is either an Exception or an CrmServiceException)</returns>
		public Exception ToException(ITrace trace) 
		{
			if (this.MoveToTopResult()) 
			{
				return (this.GetException(trace));
			} 
			else 
			{
				return (new Exception());
			}
		}

		private Exception GetException(ITrace trace) 
		{

			string actor, category, name, uiMessage;
			bool isCrmServiceException = this.GetResultElementByName("actor",out actor);
			this.GetResultElementByName("ui_message",out uiMessage);
			this.GetResultElementByName("category",out category);
			this.GetResultElementByName("name",out name);

			if (this.MoveToNextResult()) 
			{
				Exception innerException = this.GetException(trace);
				if (isCrmServiceException) 
				{
					return (new CrmServiceException(actor,category,name,uiMessage,innerException,trace));
				} 
				else 
				{
					return (new Exception(uiMessage,innerException));
				}
			} 
			else 
			{
				if (isCrmServiceException) 
				{
					return (new CrmServiceException(actor,category,name,uiMessage,trace));
				} 
				else 
				{
					return (new Exception(uiMessage));
				}
			}
		}
		#endregion

		#region Add
		/// <summary>
		/// Add details of the Exception to the XML Document
		/// </summary>
		/// <param name="e">The Exception</param>
		public void Add(Exception e) 
		{
			if (e is CrmServiceException) 
			{
				CrmServiceException ce = (CrmServiceException)e;
				this.Add(ce, ce.Name(), ce.UiMessage(), ce.Actor(), ce.Category());
			} 
			else 
			{
				this.Add(e, e.GetType().Name, e.Message, String.Empty, String.Empty);
			}
		}

		private void Add(Exception e, string name, string uiMessage, string actor, string category) 
		{
			this.Flag = false;

			XmlElement newBudElement = this.CreateElement("result");
			this.BudElement.AppendChild(newBudElement);
			this.BudElement = newBudElement;

			XmlElement nameElement = this.CreateElement("name");
			nameElement.InnerText = name;
			this.BudElement.AppendChild(nameElement);

			XmlElement uiMessageElement = this.CreateElement("ui_message");
			uiMessageElement.InnerText = uiMessage;
			this.BudElement.AppendChild(uiMessageElement);

			XmlElement stackElement = this.CreateElement("stack_trace");
			stackElement.InnerText = e.StackTrace;
			this.BudElement.AppendChild(stackElement);

			XmlElement actorElement = this.CreateElement("actor");
			actorElement.InnerText = actor;
			this.BudElement.AppendChild(actorElement);

			XmlElement categoryElement = this.CreateElement("category");
			categoryElement.InnerText = category;
			this.BudElement.AppendChild(categoryElement);

			if (e.InnerException != null) 
			{
				Add(e.InnerException);
			}
		}
		#endregion

		#region Navigation
		/// <summary>
		/// Move to the top complex element (result)
		/// </summary>
		/// <returns>Success of the request</returns>
		public bool MoveToTopResult() 
		{
			try 
			{
				this.CurrentElement = (XmlElement)this.SelectSingleNode("./result_list/result");
				return true;
			} 
			catch 
			{
				return false;
			}
		}

		/// <summary>
		/// Move to the inner complex element (result) from the current result element
		/// </summary>
		/// <returns>Success of the request</returns>
		public bool MoveToNextResult() 
		{
			try 
			{
				this.CurrentElement = (XmlElement)this.CurrentElement.SelectSingleNode("./result");
				return true;
			} 
			catch 
			{
				return false;
			}
		}

		/// <summary>
		/// Determine the user friendly error message to be displayed
		/// on the client screen. This depends upon the original error
		/// message obtained and the current panel location.
		/// </summary>
		/// <param name="panelLocation">The panel type e.g. Search, Form, Popup etc.</param>
		/// <returns>The user friendly error message.</returns>
		public string GetClientErrorMessage(string panelLocation) 
		{
			// Get the name of the original error message
			this.MoveToTopResult();
			string origErrorIndex;
			this.GetResultElementByName("name",out origErrorIndex);

			// Now append the relavent tag to the front of the
			// original error message
			//
			//   <panel location>.<original error>
			//
			string newErrorIndex = origErrorIndex + "." + panelLocation;

			// Now return the error message, if there is one !!
			string newError = CrmStringBuilder.BuildMessage(CrmServiceException.ResourceList,newErrorIndex,newErrorIndex);
			if (newError == null || newError == "") 
				// A message wasn't found, so just return the index of the message
				// that was being searched for
				return newErrorIndex;
			else
				return newError;
		}

		/// <summary>
		/// Get the inner text of a simple element immediately below the current complex element (result)
		/// </summary>
		/// <param name="elementName">The Name of the Simple Element </param>
		/// <param name="innerText">The Inner Text of the Simple Element</param>
		/// <returns>Success of the request</returns>
		public bool GetResultElementByName(string elementName, out string innerText) 
		{
			try 
			{
				innerText = this.CurrentElement.SelectSingleNode("./"+elementName).InnerText;
				return true;
			} 
			catch 
			{
				innerText = null;
				return false;
			}
		}
		#endregion

		#region Properties
		/// <summary>Get the Result XSD</summary>
		public static string Xsd {
			get { return Result.xsd; }
			set { Result.xsd = value; }
		}

		/// <summary>Get the Result Namespace</summary>
		public static string NameSpace {
			get { return Result.nameSpace; }
			set { Result.nameSpace = value; }
		}

		/// <summary>Get or Set the Success Flag, was the method successful</summary>
		public bool Flag { 
			get { return this.flag; } 
			set { this.flag = value; }
		}

		private XmlElement BudElement { 
			get { return this.budElement; } 
			set { this.budElement = value; }
		}

		private XmlElement CurrentElement { 
			get { return this.currentElement; } 
			set { this.currentElement = value; }
		}
		#endregion
	}
}