using System;
using System.Xml;

namespace Fujitsu.eCrm {

	#region Header
	///
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// A Reusable class that can be used to extract information from the 
	/// XMLResult string returned from the CRMService.
	/// </summary>
	/// 
	/// <development>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class XmlResult {

		private const string uiMessageXPath = "//ui_message";
		private const string actorXPath = "//actor";
		private const string categoryXPath = "//category";
		private const string nameXPath = "//name";

		private XmlDocument xmlResultDocument;

		#region Constructor
		/// <summary>
		/// Constructor an object containing the result returned from the CRM Server
		/// </summary>
		/// <param name="xml">The XML Result string, as returned from the CRMService</param>
		public XmlResult(string xml) {
			xmlResultDocument = new XmlDocument();
			xmlResultDocument.LoadXml(xml);
		}
		#endregion

		#region Properties

		/// <summary>
		/// Returns a detailed error description message, as returned by
		/// the CRMServer following an error. 
		/// If no error occured, returns an emptry string
		/// </summary>
		public string UiMessage {
			get {
				XmlNode n = xmlResultDocument.SelectSingleNode(uiMessageXPath);
				if ((n != null) && (n.NodeType == XmlNodeType.Element))
					return n.InnerText;
				else
					return String.Empty;
			}
		}

		/// <summary>
		/// Returns who is responsible for the error
		/// </summary>
		public string Actor {
			get {
				XmlNode n = xmlResultDocument.SelectSingleNode(actorXPath);
				if ((n != null) && (n.NodeType == XmlNodeType.Element))
					return n.InnerText;
				else
					return String.Empty;
			}
		}

		/// <summary>
		/// Returns the name of the error message
		/// </summary>
		public string Name {
			get {
				XmlNode n = xmlResultDocument.SelectSingleNode(nameXPath);
				if ((n != null) && (n.NodeType == XmlNodeType.Element))
					return n.InnerText;
				else
					return String.Empty;
			}
		}

		/// <summary>
		/// Returns the error's category
		/// </summary>
		public string Category {
			get {
				XmlNode n = xmlResultDocument.SelectSingleNode(categoryXPath);
				if ((n != null) && (n.NodeType == XmlNodeType.Element))
					return n.InnerText;
				else
					return String.Empty;
			}
		}
		#endregion
	}
}