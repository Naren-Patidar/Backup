#region Using
using System;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using System.Resources;
using System.Reflection;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils
{
	
	#region Header
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// A Reusable class that can be used to Build an XmlDocument from a Template
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.01" day="31" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Wp/Barcelona/023</work_packet>
	///			<description>Changed to ensure any empty elements are removed. Created a new
	///			constructor to enable switching between XML template string and file. Also
	///			enable hash table checking to be switched on and off.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion Header

	public class XmlTemplate 
	{
		#region Constants
		private const string KEEP = "keep";
		private const string UPDATE_VERSION_ATTRIBUTE_NAME = "update_version";
		private const string UPDATE_TYPE_ATTRIBUTE_NAME = "update_type";
		private const string CRMID_ATTRIBUTE_SUFFIX = "_crmid";
		private const string UPDATE_TYPE_ADD = "add";
		private const string UPDATE_TYPE_DELETE = "delete";
		private const string UPDATE_TYPE_REMOVE = "remove";
		private const string UPDATE_TYPE_UPDATE = "update";
		private const string UPDATE_TYPE_INCREMENT = "increment";
		private const string UPDATE_TYPE_BLANK = "";
		private const string UPDATE_TYPE_NONE = "none";
		#endregion

		#region Attributes
		private XmlDocument xmlTemplateDoc;
		private ITrace trace;
		#endregion

		#region Enumerations
		/// <summary>Enumeration of update types</summary>
		public enum UpdateType 
		{	
			/// <summary>Empty update type, equalivant to none</summary>
			blank,
			/// <summary>Do not update</summary>
			none,
			/// <summary>Add record</summary>
			add,
			/// <summary>Update record if it exists, else Add record</summary>
			increment,
			/// <summary>Mark as Obsolete record</summary>
			delete,
			/// <summary>Delete record</summary>
			remove,
			/// <summary>Update record</summary>
			update
		};
		#endregion

		#region Properties
		private bool checkElementExists;
		/// <summary>
		/// Switch the checking of hash table contents against template
		/// elements on and off. When switched on, any entries in the hash
		/// table (not beginning with an underscore) that are not in the
		/// template will cause an exception to be thrown.
		/// </summary>
		public bool CheckElementExists 
		{
			set { this.checkElementExists = value; }
			get { return this.checkElementExists; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Construct an XmlTemplate object based on the contents of a XML file
		/// or XML string.
		/// </summary>
		/// <param name="templateSource">This will depend upon the setting of the isTemplate
		/// parameter. If isTemplate is true, then a string containing the XML template
		/// will be expected. If isTemplate is false, then a string containing the template
		/// file path will be expected.</param>
		/// <param name="isTemplateFile">If set to false, the template source will be the
		/// string passed in through the templateSource parameter. If set to true, then
		/// the template source will be a file, with the path being also supplied through
		/// templateSource parameter.</param>
		/// <param name="trace">The trace to which an messages is written.</param>
		/// <param name="checkElements">Set to true to ensure that all elements in the hash table are
		/// expected to be found in the template.</param>
		public XmlTemplate(string templateSource,ITrace trace, bool isTemplateFile,bool checkElements) 
		{
			this.trace = trace;
			ITraceState trState = null;
			if (this.trace != null)
				trState = this.trace.StartProc("XmlTemplate.Constructor(" +
					templateSource + ",'Trace Object'," + isTemplateFile.ToString() + ")");
			try 
			{
				// Set the property to determine whether element cheking will be
				// performed.
				this.CheckElementExists = checkElements;

				// Check what the source of the template will be.
				if (isTemplateFile) 
				{
					// The source of the template will be through a supplied
					// string.
					this.xmlTemplateDoc = new XmlDocument();
					try 
					{
						this.xmlTemplateDoc.Load(templateSource);
					} 
					catch (Exception e) 
					{
						CrmServiceException ce = new CrmServiceException(
							"Library",
							"XmlError",
							"XmlTemplate.LoadDocFromFileFailure",
							e,
							this.trace,
							templateSource);
						throw ce;
					}
				} 
				else 
				{
					this.xmlTemplateDoc = new XmlDocument();
					try 
					{
						this.xmlTemplateDoc.LoadXml(templateSource);
					} 
					catch (Exception e) 
					{
						CrmServiceException ce = new CrmServiceException(
							"Library",
							"XmlError",
							"XmlTemplate.LoadDocFromXmlFailure",
							e,
							this.trace,
							templateSource);
						throw ce;
					}
				}
			}
			finally 
			{
				if (this.trace != null)
					this.trace.EndProc(trState);
			}
		}

		/// <summary>
		/// Construct an XmlTemplate object based on the contents of a XML file.
		/// </summary>
		/// <param name="fileName">The path of the file containing the template.</param>
		/// <param name="trace">The trace to which an messages is written.</param>
		public XmlTemplate(string fileName,ITrace trace) : this(fileName,trace,true,true)
		{
		}

//		//--------------------------------------------------------------------
//		// Alternative constructor to load the template from the CRMServer.
//		//
//		// Not currently in use.
//		//--------------------------------------------------------------------
//
//		public XmlTemplate(Page webPage, string sessionId, string domainName, string viewName)
//		{
//
//			this.xmlTemplateDoc = new XmlDocument();
//			_page = webPage;
//
//			// Try and get the template XmlDoc from the Cache.
//			string cacheTag = CACHE_TAG_PREFIX + "_TEMPLATE_" + sessionId + "$" + domainName + "$" + viewName;
//			this.xmlTemplateDoc = (XmlDocument)_page.Cache[cacheTag];
//
//			// Was there already a copy of the template in the Cache?
//			if (this.xmlTemplateDoc != null) 
//			{
//				//  Yes. Use it.
//				_page.Trace.Write("Using existing XMLTemplate " + cacheTag);
//			}
//			else
//			{
//				// No. Read the template from the CRMServer.
//				//Page.Trace.Write("Downloading new XMLTemplate " + cacheTag);
//
//				CRMServer.XmlGeneric crmSvr = new CRMServer.XmlGeneric();
//				string resultXml;
//				string xmlView;
//				bool success = crmSvr.XmlTemplate(sessionId, domainName, viewName, out resultXml, out xmlView);
//				if (!success)
//				{
//					XmlResult res = new XmlResult(resultXml);
//					string[] args = {res.Code, res.Message, domainName, viewName};
//					CrmServiceException.Throw(_resourceManager,"ReadTemplateFailure",args);
//					//throw new ApplicationException("Error '" + res.Code + 
//					//		"' reading XML Template from CRMServer : " + res.Message);
//				}
//				this.xmlTemplateDoc = new XmlDocument();
//				this.xmlTemplateDoc.LoadXml(xmlView);
//
//				// Save the XmlDom in the system cache so it's ready for next time.
//				_page.Cache.Add(cacheTag,
//					this.xmlTemplateDoc,
//					null, 
//					System.DateTime.MaxValue, 
//					System.TimeSpan.FromMinutes(CACHE_RETENTION_MINUTES),
//					System.Web.Caching.CacheItemPriority.Normal,
//					null);
//			}
//		}
		#endregion

		#region Utilities
		/// <summary>
		/// Used only for tracing. If the supplied parameter is null, returns "NULL",
		/// otherwise returns the string unaltered.
		/// </summary>
		/// <param name="str">The supplied string</param>
		/// <returns>The altered string</returns>
		private string DeNull(string str) {
			return (str==null) ? "NULL" : str;
		}

		/// <summary>
		/// Returns the Element nearest the top of the hierarchy containing the
		/// attribute "update_type".
		/// </summary>
		/// <param name="template">The XML Document</param>
		/// <returns>The first child of the XML Document</returns>
		private XmlElement GetTopLevelElement(XmlDocument template) {

			XmlElement el = template.DocumentElement;
			do {
				if (el.Attributes[UPDATE_TYPE_ATTRIBUTE_NAME] != null) {
					// The current element contains an "update_type" element, so finish.
					return el;
				} else {
					// The current element does not contain an "update_type" element.
					// Try again with the first child element.
					XmlNodeList childNodes = el.ChildNodes;
					el = null;
					foreach (XmlNode node in childNodes) {
						if (node.NodeType == XmlNodeType.Element) {
							el = (XmlElement)node;
							break;
						}
					}
				}
			} while (el != null);

			// No element appears to contain the "update_type" element. Raise an error.
			CrmServiceException ce = new CrmServiceException(
				"Library",
				"XmlError",
				"XmlTemplate.MissingUpdateType",
				this.trace,
				template.Name);
			throw ce;
		}

		/// <summary>
		/// Returns the Element that is the container for the selected Node.
		/// </summary>
		/// <param name="selectedNode">The child element</param>
		/// <returns>The parent element</returns>
		private XmlElement GetContainerElement(XmlNode selectedNode) {

			if ((selectedNode == null) || (selectedNode.NodeType != XmlNodeType.Element)) {
				return null;
			}

			XmlElement el = (XmlElement)selectedNode;
			while (el != null) {
				if (el.Attributes[UPDATE_TYPE_ATTRIBUTE_NAME] != null) {
					// The current element contains an "update_type" element, so finish.
					return el;
				} else {
					// The current element does not contain an "update_type" element.
					// Try again with the parent element.
					if (el.ParentNode.NodeType == XmlNodeType.Element) {
						el = (XmlElement)el.ParentNode;
					} else {
						el = null;
					}
				}
			}

			// No parent container element found
			//throw new ApplicationException("Container Element not found for element " + selectedElement.LocalName);
			return null;
		}

		/// <summary>
		/// Returns the first element in the originalXmlDoc which
		/// has an attribute that matches the supplied crmID.
		/// If the crmID cannot be found, an error is raised.
		/// If crmIdName or crmID are not supplied, highest level
		/// element in the document is returned.
		/// </summary>
		/// <param name="xmlDoc">The xmlDocument to search</param>
		/// <param name="crmIdName">The name of the attribute containing the crmid</param>
		/// <param name="crmId">The CrmId field to search for</param>
		/// <returns>The first element</returns>
		private XmlElement SelectStartElement(
			XmlDocument xmlDoc, 
			string crmIdName, 
			string crmId) {
			
			if (xmlDoc==null) {
				// No document supplied - return null
				return null;
			}

			if ((crmIdName==null) || (crmId==null) ||
				(crmIdName.Length==0) || (crmId.Length==0)) {
				// crmId or crmIDName missing - get the top-level element
				return xmlDoc.DocumentElement;
			}

			// Try to get the matching element from the original Document by using an
			// xpath string like //*[@phone_crmid='1234ABC']
			string xpath = "//*[@" + crmIdName + "='" + crmId + "']";

			XmlNode selectedNode = xmlDoc.SelectSingleNode(xpath);
			if ((selectedNode != null) && (selectedNode.NodeType == XmlNodeType.Element)) {
				return ((XmlElement) selectedNode);
			} else {
				string[] args = {xpath,xmlDoc.OuterXml};
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.MissingMatchingAttribute",
					this.trace,
					args);
				throw ce;
			}
		}
/*
		/// <summary>
		/// Sets the CrmID attribute of the supplied XmlElement. 
		/// The attributes name is assumed to be the elements name + "_crmid", i.e.
		/// customer customer_crmid="0"
		/// </summary>
		/// <param name="crmId">The new ID</param>
		/// <param name="el">The element</param>
		private void SetCrmId(string crmId, XmlElement el) {
			// get the element's attribute who's name ends with "{elementname}_crmid"
			string crmIdName = el.LocalName + CRMID_ATTRIBUTE_SUFFIX;
			XmlAttribute crmIdAttribute = el.Attributes[crmIdName];
			if (crmIdAttribute != null) {
				crmIdAttribute.Value = crmId;
			} else {
				string[] args = {crmIdName,el.OuterXml };
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.MissingAttribute",
					this.trace,
					args);
				throw ce;
			}
		}
*/
		/// <summary>
		/// Gets the CrmID attribute of the supplied XmlElement. 
		/// The attributes name is assumed to be the elements name + "_crmid", i.e.
		/// customer customer_crmid="0"
		/// </summary>
		/// <param name="el">The element</param>
		/// <returns>The element's ID</returns>
		private string GetCrmId(XmlElement el) {
			// get the element's attribute who's name ends with "{elementname}_crmid"
			string crmIdName = el.LocalName + CRMID_ATTRIBUTE_SUFFIX;
			XmlAttribute crmIdAttribute = el.Attributes[crmIdName];
			if (crmIdAttribute != null) {
				return crmIdAttribute.Value;
			} else {
				string[] args = {crmIdName,el.OuterXml};
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.MissingAttribute",
					this.trace,
					args);
				throw ce;
			}
		}

		/// <summary>
		/// Gets the UpdateType attribute of the supplied XmlElement. 
		/// </summary>
		/// <param name="el">The element</param>
		/// <returns>The element's ID</returns>
		private UpdateType GetUpdateType(XmlElement el) {
			XmlAttribute attr = el.Attributes[UPDATE_TYPE_ATTRIBUTE_NAME];
			if (attr != null) {
				string attrValue = attr.Value.ToLower();
				if (attrValue == UPDATE_TYPE_ADD) return UpdateType.add;
				if (attrValue == UPDATE_TYPE_INCREMENT) return UpdateType.increment;
				if (attrValue == UPDATE_TYPE_DELETE) return UpdateType.delete;
				if (attrValue == UPDATE_TYPE_REMOVE) return UpdateType.remove;
				if (attrValue == UPDATE_TYPE_UPDATE) return UpdateType.update;
				if (attrValue == UPDATE_TYPE_NONE) return UpdateType.none;
				if (attrValue == UPDATE_TYPE_BLANK) return UpdateType.blank;

				// Unknown UpdateType - throw an error
				string[] args = {attrValue,el.OuterXml};
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.InvalidUpdateType",
					this.trace,
					args);
				throw ce;
			} else {
				string[] args = {UPDATE_TYPE_ATTRIBUTE_NAME,el.OuterXml };
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.MissingAttribute",
					this.trace,
					args);
				throw ce;
			}
		}

		/// <summary>
		/// Set up all update_type attributes to the specified type
		/// </summary>
		/// <param name="updateType">The type to set the attribute to</param>
		public void SetUpdateType(UpdateType updateType) {

			// For all elements with subelements
			foreach (XmlElement tableElement in xmlTemplateDoc.SelectNodes("/*/descendant::*[*]")) {
				if (updateType == UpdateType.add) {
					if (GetCrmId(tableElement) == String.Empty) {
						SetUpdateType(UpdateType.add,tableElement);
					} else {
						SetUpdateType(UpdateType.none,tableElement);
					}
				} else if (updateType == UpdateType.blank) {
					SetUpdateType(UpdateType.blank,tableElement);
				} else if (updateType == UpdateType.delete) {
					if (GetCrmId(tableElement) == String.Empty) {
						SetUpdateType(UpdateType.none,tableElement);
					} else {
						SetUpdateType(UpdateType.delete,tableElement);
					}
				} else if (updateType == UpdateType.remove) {
					if (GetCrmId(tableElement) == String.Empty) {
						SetUpdateType(UpdateType.none,tableElement);
					} else {
						SetUpdateType(UpdateType.remove,tableElement);
					}
				} else if (updateType == UpdateType.increment) {
					if (GetCrmId(tableElement) == String.Empty) {
						SetUpdateType(UpdateType.increment,tableElement);
					} else {
						SetUpdateType(UpdateType.update,tableElement);
					}
					SetIncrementField(tableElement);
				} else if (updateType == UpdateType.none) {
					SetUpdateType(UpdateType.none,tableElement);
				} else if (updateType == UpdateType.update) {
					if (GetCrmId(tableElement) == String.Empty) {
						SetUpdateType(UpdateType.add,tableElement);
					} else {
						SetUpdateType(UpdateType.update,tableElement);
					}
				}
			}
		}

		/// <summary>
		/// Set the Increment attribute of fields to true if they have content
		/// </summary>
		/// <param name="el"></param>
		private void SetIncrementField(XmlElement el) {
			// select increment attribute of field elements that have a text node
			foreach (XmlNode n in el.SelectNodes("./*[text()]/@increment")) {
				n.InnerText = "true";
			}
		}

		/// <summary>
		/// Sets the UpdateType attribute of the supplied XmlElement. 
		/// </summary>
		/// <param name="newUpdateType">The element's Update Type</param>
		/// <param name="el">The element</param>
		private void SetUpdateType(UpdateType newUpdateType, XmlElement el) {

			// The element's CrmId whos name ends with "{elementname}_crmid"
			XmlAttribute updateTypeAttribute = el.Attributes[UPDATE_TYPE_ATTRIBUTE_NAME];
			if (updateTypeAttribute != null) {
				switch(newUpdateType) {
					case UpdateType.add:
						updateTypeAttribute.Value = UPDATE_TYPE_ADD;
						break;
					case UpdateType.increment:
						updateTypeAttribute.Value = UPDATE_TYPE_INCREMENT;
						break;
					case UpdateType.delete:
						updateTypeAttribute.Value = UPDATE_TYPE_DELETE;
						break;
					case UpdateType.remove:
						updateTypeAttribute.Value = UPDATE_TYPE_REMOVE;
						break;
					case UpdateType.update:
						updateTypeAttribute.Value = UPDATE_TYPE_UPDATE;
						break;
					case UpdateType.none:
						updateTypeAttribute.Value = UPDATE_TYPE_NONE;
						break;
					default:
						updateTypeAttribute.Value = UPDATE_TYPE_BLANK;
						break;
				}
			} else {
				string[] args = {UPDATE_TYPE_ATTRIBUTE_NAME,el.OuterXml };
				CrmServiceException ce = new CrmServiceException(
					"Library",
					"XmlError",
					"XmlTemplate.MissingAttribute",
					this.trace,
					args);
				throw ce;
			}
		}


		/// <summary>
		/// Copy the CRMID's for all the parent container elements from
		/// the original. 
		/// If the parent element existed in the original, copy its
		/// CRMID and set its update_type to NONE. 
		/// If it didn't exist in the original, create a new CRMID
		/// and set its update_type to NEW.
		/// </summary>
		/// <param name="templateElement">The new XML Document</param>
		/// <param name="originalBaseElement">The original XML Document</param>
		private void CopyDocumentIDs(
			XmlElement templateElement,
			XmlElement originalBaseElement) { 

			// Get the node that is the parent of the current template element
			XmlNode templateParentNode = templateElement.ParentNode;

			// Whilst there are unprocessed parent elements remaining ....
			while ((templateParentNode != null) && (templateParentNode.NodeType == XmlNodeType.Element)) {
				// Get the template element that is the container for the selected element
				XmlElement templateContainerElement = GetContainerElement((XmlElement)templateParentNode);

				// Is the container element valid?
				if (templateContainerElement != null) {
					// Yes. Has the updatetype of the current element already been set?
					if (GetUpdateType(templateContainerElement) != UpdateType.blank) {
						// Yes. No need to do anymore work. Exit the loop.
						break;
					} else {
						XmlNode originalParentNode = null;
						string elementName = templateContainerElement.LocalName;

						if (originalBaseElement != null) {
							// Try to get the matching container element from the original XML.
							// First look at the children of the Base Element
							string xpath = "descendant-or-self::" + elementName;
							originalParentNode =  originalBaseElement.SelectSingleNode(xpath);
						}

						if ((originalParentNode==null) && (originalBaseElement != null)) {
							// Not a child, try the parents of the Base Element
							string xpath = "ancestor::" + elementName;
							originalParentNode = originalBaseElement.SelectSingleNode(xpath);
						}
						
						if ((originalParentNode != null) && 
							(originalParentNode.NodeType == XmlNodeType.Element)) {
							// The matching element exists in the original Xml. Copy its
							// attributes and set the updateType to "None".
							CopyAttributes((XmlElement)originalParentNode, templateContainerElement);
							SetUpdateType(UpdateType.none, templateContainerElement);
						} else {
							// The element does not exist in the original Xml. Create a 
							// new CRMID and set the updateType to "Add"
							//SetCrmId(Guid.NewGuid().ToString(),templateContainerElement);
							SetUpdateType(UpdateType.add, templateContainerElement);
						}

						// continue with the parents of the element we've just processed.
						templateParentNode = templateContainerElement.ParentNode;
					}
				} else {
					// No more parent container elements remaining. Exit the loop
					break;
				}
			}
		}

		/// <summary>
		/// Routine to copy all the attribute values from one XmlElement to another.
		/// (Excluding the update_type attribute)
		/// </summary>
		/// <param name="srcElement">The original Element</param>
		/// <param name="destElement">The new Element</param>
		private void CopyAttributes(
			XmlElement srcElement, 
			XmlElement destElement) {

			// Copy all the attributes
			foreach(XmlAttribute attr in destElement.Attributes) {
				XmlAttribute srcAttr = srcElement.Attributes[attr.Name];
				if ((srcAttr != null) && (srcAttr.Name != UPDATE_TYPE_ATTRIBUTE_NAME)) {
					attr.Value = srcAttr.Value;
				}
			}
		}

		/// <summary>
		/// Selects an element from an XmlNode, using just its name.
		/// </summary>
		/// <param name="node">The Document to search</param>
		/// <param name="elementName">The Element to search for</param>
		/// <returns>The found Element</returns>
		private XmlElement GetElement(
			XmlNode node, 
			String elementName) {

			if (node==null) {
				return null;
			} else {
				// Try to get the matching element from the original XML.
				// Form an xpath statement in the style 
				// 'descendant-or-self::Customer'.
				string xpath;
				if (elementName.Substring(0,1) == "/") {
					xpath = elementName;
				} else {
					xpath = "descendant-or-self::" + elementName;
				}

				XmlNode selectedNode = node.SelectSingleNode(xpath);
				if ((selectedNode != null) && 
					(selectedNode.NodeType == XmlNodeType.Element)) {
					return ((XmlElement) selectedNode);
				} else {
					return null;		   
				}
			}
		}
		#endregion

		#region CreateTopElement
		/// <summary>
		/// Modifes the XmlTemplateDocument to set the top-level 
		/// update-type to 'add'. Used to create a new instance of a
		/// top-level element, such as customer.
		/// </summary>
		public void CreateTopElement() {
			ITraceState trState = null;
			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.CreateTopElement");
			}
			try {
				// Find the first element in the template hierarcy
				XmlElement el = GetTopLevelElement(this.xmlTemplateDoc);
				// Set the topElement's CrmId
				//SetCrmId(Guid.NewGuid().ToString(), el);
				// Set the topElement's update_type 
				SetUpdateType(UpdateType.add,el);
			} finally {
				if (this.trace != null) {
					this.trace.EndProc(trState);
				}
			}
		}
		#endregion

		#region DeleteTopElement
		/// <summary>
		/// Updates the XmlTemplateDocument to delete an
		/// entire top-level element, such as customer. The CRMID of
		/// the object to be deleted is extracted from the 
		/// originalXmlDocument.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		public void DeleteTopElement(XmlDocument originalXmlDoc) {
			ITraceState trState = null;

			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.DeleteTopElement");
			}
			try {
				// Get the CrmId of the original XmlDocument
				XmlElement originalTopLevelElement = GetTopLevelElement(originalXmlDoc);
				// string originalCrmId = GetCrmId(originalTopLevelElement);			
				// Find the first element in the template hierarcy
				XmlElement el = GetTopLevelElement(this.xmlTemplateDoc);
				// Set the CrmId & timestamp
				CopyAttributes(originalTopLevelElement, el);
				// Set the topElement's update_type 
				SetUpdateType(UpdateType.delete,el);
			} finally {
				if (this.trace != null) {
					this.trace.EndProc(trState);
				}
			}
		}
		#endregion

		#region AddSubElement

		/// <summary>
		/// Updates the supplied template XmlDocument to add one or
		/// more subelements. 
		/// The data items must be supplied in another Template, 
		/// with details of where to graft the new template into this 
		/// template
		/// </summary>
		/// <param name="subTemplate">The template to copy</param>
		/// <param name="xlink">The common parent element</param>
		/// <param name="xvalue">The conditions of this' parent element</param>
		public void AddSubElement(XmlTemplate subTemplate,string xlink,string xvalue) {

			XmlNode newNode = subTemplate.xmlTemplateDoc.SelectSingleNode("//"+xlink);

			XmlNode compareNode = newNode.SelectSingleNode("./"+xvalue);
			string compareValue;
			if (compareNode.NodeType == XmlNodeType.Attribute) {
				compareValue = compareNode.Value;
			} else {
				compareValue = compareNode.InnerText;
			}

			XmlNode oldNode = this.xmlTemplateDoc.SelectSingleNode("//"+xlink+"["+xvalue+"='"+compareValue+"']");

			Graft(oldNode,newNode,true);
		
		}

		private void Graft(XmlNode parentTargetNode, XmlNode parentSourceNode, bool isRoot) {

			// Copy Attributes
			if (!isRoot) {
				foreach (XmlNode sourceNode in parentSourceNode.Attributes) {
					// create a new attribute belonging to this
					XmlAttribute newAttribute = this.xmlTemplateDoc.CreateAttribute(sourceNode.Name);
					// make the new attribute have values of the source node
					newAttribute.Value = sourceNode.Value;
					// add the new attribute to the target node
					parentTargetNode.Attributes.Append(newAttribute);
				}
			}

			// Copy Complex Elements
			foreach (XmlNode sourceNode in parentSourceNode.SelectNodes("./*[*]")) {
				// create a new element belonging to this
				XmlElement newElement = this.xmlTemplateDoc.CreateElement(sourceNode.Name);
				// make the new element have values of the source node
				Graft(newElement,sourceNode,false);
				// add the new element to the target node
				parentTargetNode.AppendChild(newElement);
			}

			// Copy Simple Elements
			if (!isRoot) {
				foreach (XmlNode sourceNode in parentSourceNode.SelectNodes("./*[not(*)]")) {
					// create a new element belonging to this
					XmlElement newElement = this.xmlTemplateDoc.CreateElement(sourceNode.Name);
					// make the new element have values of the source node
					newElement.InnerText = sourceNode.InnerText;
					Graft(newElement,sourceNode,false);
					// add the new element to the target node
					parentTargetNode.AppendChild(newElement);
				}
			}
		}


		/// <summary>
		/// Updates the supplied template XmlDocument to add one or
		/// more subelements. 
		/// The data items must be supplied in a Hashtable, containing
		/// the items xpath in the key field, and the new data
		/// in the value field.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="dataItems">The data items to be copied</param>
		/// <param name="crmIdName">The element type to be copied</param>
		/// <param name="crmId">The element to be copied</param>
		public void AddSubElement(
			XmlDocument originalXmlDoc, 
			Hashtable dataItems,
			string crmIdName,
			string crmId) {

			ITraceState trState = null;

			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.AddSubElement");
				if (this.trace.TraceLevel.Debug) {
					if (originalXmlDoc==null) {
						this.trace.WriteDebug("originalXmlDoc = null");
					} else {
						this.trace.WriteDebug("originalXmlDoc = not null");
					}

					foreach (string key in dataItems.Keys) {
						this.trace.WriteDebug("dataItems[" + key + "] = '" + DeNull((string)dataItems[key]) + "'");
					}
					this.trace.WriteDebug("crmIdName = " + DeNull(crmIdName));
					this.trace.WriteDebug("crmId=" + DeNull(crmId));
				}
			}
			try {

				XmlElement originalBaseElement = SelectStartElement(originalXmlDoc, crmIdName, crmId);

				foreach (string key in dataItems.Keys) {
					// Don't attempt to send any dataItems who's name starts with an
					// underscore to the CRM server. This lets us have items in the
					// form which don't appear in the database.
					if (key.StartsWith("_")) {
						continue;
					}

					// Get the value of the item to be added
					//string val=(string)dataItems[key];

					// If the item is empty, don't bother adding it.
					//if (val.Length==0) {
					//	continue;
					//}

					// Get the selected subelement from the template.
					XmlNode selectedNode = GetElement(this.xmlTemplateDoc,key);
					if (selectedNode == null) {
						selectedNode = this.xmlTemplateDoc.SelectSingleNode("//@"+key);
					}

					if (selectedNode == null) {
						// Check whether this check has been switched on or
						// off when the object was instantiated.
						if (this.CheckElementExists) 
						{
							// The checking was switched on, so throw an error,
							// as the element can't be found.
							string[] args = {key,this.xmlTemplateDoc.OuterXml };
							CrmServiceException ce = new CrmServiceException(
								"Library",
								"XmlError",
								"XmlTemplate.ElementNotFound",
								this.trace,
								args);
							throw ce;
						} 
						else 
						{
							// The checking was switched off, so just continue
							// onwards with the next hash table entry.
							continue;
						}
					}
				
					// Set its value
					if (selectedNode.NodeType == XmlNodeType.Element) {
						if (dataItems[key] != null) {
							selectedNode.InnerText = (string)dataItems[key];
						} else {
							XmlNode nilNode = selectedNode.Attributes.GetNamedItem("nil");
							if (nilNode == null) {
								continue;
							}
							nilNode.InnerText = "true";
						}
					} else {
						selectedNode.Value = (string)dataItems[key];
					}

					// Set the container element's CRMID and UpdateType, 
					// unless we've not already done it.
					if (selectedNode.NodeType == XmlNodeType.Element) {
						XmlAttribute attr = this.xmlTemplateDoc.CreateAttribute(KEEP);
						selectedNode.Attributes.Append(attr);
					}

					// Set the container element's CRMID and UpdateType, 
					// unless we've not already done it.
					XmlElement containerElement;
					if (selectedNode.NodeType == XmlNodeType.Element) {
						containerElement = (XmlElement)selectedNode.ParentNode;
					} else {
						containerElement = ((XmlAttribute)selectedNode).OwnerElement;
					}

					if (containerElement == null) {
						string[] args = {selectedNode.OuterXml,this.xmlTemplateDoc.OuterXml };
						CrmServiceException ce = new CrmServiceException(
							"Library",
							"XmlError",
							"XmlTemplate.ContainerElementNotFound",
							this.trace,
							args);
						throw ce;
					}

					if (GetCrmId(containerElement).Length <=1) {
						//SetCrmId(Guid.NewGuid().ToString(),containerElement);
						SetUpdateType(UpdateType.add,containerElement);

						// Copy or generate new CRMID's for all the parent container elements
						CopyDocumentIDs(containerElement,originalBaseElement);
					}
				}
			} finally {
				if (this.trace != null)
					this.trace.EndProc(trState);
			}
		}

		/// <summary>
		/// Overlay of AddSubElement without the CRMID and CRMIDName
		/// parameters.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="dataItems">The data items to be copied</param>
		public void AddSubElement(
			XmlDocument originalXmlDoc, 
			Hashtable dataItems) {

			AddSubElement(originalXmlDoc, dataItems, null, null);
		}
			
		/// <summary>
		/// Overlay of AddSubElement without the originalXmlDoc,
		/// CrmID and CrmIdName parameters.
		/// </summary>
		/// <param name="dataItems">The data items to be copied</param>
		public void AddSubElement(
			Hashtable dataItems) {

			AddSubElement(null, dataItems, null, null);
		}
		#endregion

		#region UpdateSubElement
		/// <summary>
		/// Updates the supplied template XmlDocument to modify one or
		/// more subelements. 
		/// The data items must be supplied in a Hashtable, containing
		/// the items xpath in the key field, and the new data
		/// in the value field.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="dataItems">The data items to be copied</param>
		/// <param name="crmIdName">The element type to be copied</param>
		/// <param name="crmId">The element to be copied</param>
		/// <param name="deduceOriginal">Attempt to determine the original document container
		/// and copy its attributes. Helps when there are repeating elements with the same
		/// name, otherwise the first occurence attributes are always copied.</param>
		/// <returns>true if any SubElements have been changed</returns>
		public bool UpdateSubElement(
			XmlDocument originalXmlDoc, 
			Hashtable dataItems,
			string crmIdName,
			string crmId,
			bool deduceOriginal) {
			ITraceState trState = null;

			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.UpdateSubElement");
				if (this.trace.TraceLevel.Debug) {	// print the parameter values
					if (originalXmlDoc==null) {
						this.trace.WriteDebug("originalXmlDoc = null");
					} else {
						this.trace.WriteDebug("originalXmlDoc = not null");
					}
					foreach (string key in dataItems.Keys) {
						this.trace.WriteDebug("dataItems[" + key + "] = '" + DeNull((string)dataItems[key]) + "'");
					}
					this.trace.WriteDebug("crmIdName = " + DeNull(crmIdName));
					this.trace.WriteDebug("crmId=" + DeNull(crmId));
				}
			}
			bool subElementChanged = false;
			try {
				XmlElement originalBaseElement = SelectStartElement(originalXmlDoc, crmIdName, crmId);

				foreach (string key in dataItems.Keys) {
					// Don't attempt to send any dataItems who's name starts with an
					// underscore to the CRM server. This lets us have items in the
					// form which don't appear in the database.
					if (key.StartsWith("_")) {
						continue;
					}

					string newValue=null;
					if (dataItems[key] != null) {
						newValue=dataItems[key].ToString();
					} 

					// Get the selected subelement from the template XmlDoc.
					XmlElement selectedElement = GetElement(this.xmlTemplateDoc,key);
					if (selectedElement == null) {
						// Check whether this check has been switched on or
						// off when the object was instantiated.
						if (this.CheckElementExists) 
						{
							// The checking was switched on, so throw an error,
							// as the element can't be found.
							string[] args = {key,this.xmlTemplateDoc.OuterXml };
							CrmServiceException ce = new CrmServiceException(
								"Library",
								"XmlError",
								"XmlTemplate.ElementNotFound",
								this.trace,
								args);
							throw ce;
						} 
						else 
						{
							// The checking was switched off, so just continue
							// onwards with the next hash table entry.
							continue;
						}
					}
					// Try to get the original value from the old XmlDocument
					XmlElement originalElement = GetElement(originalBaseElement, key);

					// If the new value is the same as the original value, there's no point
					// in updating it.
					if (originalElement != null) {
						string originalValue = originalElement.InnerText;
						if (originalValue.Equals(newValue)) {
							continue;  // jump to the end of the loop
						}
					}

					// If the original value and current values are both null, do nothing.
					if (newValue == null) {
						if (originalElement == null) {
							continue;
						}
					} else if ((originalElement == null) && (newValue.Length<1)) {
						continue;
					}

					// Get the template container element of the currently selected element
					XmlElement containerElement = GetContainerElement(selectedElement);
					if (containerElement == null) {
						string[] args = {selectedElement.OuterXml,this.xmlTemplateDoc.OuterXml };
						CrmServiceException ce = new CrmServiceException(
							"Library",
							"XmlError",
							"XmlTemplate.ContainerElementNotFound",
							this.trace,
							args);
						throw ce;
					}

					// Copy the entire container element and its contents from the 
					// original Xml, unless we've already done it.
					if (GetCrmId(containerElement).Length <=1) 
					{
						XmlElement originalContainer = null;
						// See if support for repeating elements has been requested.
						if (deduceOriginal)
						{
							// Check if the basis to work out the orignal container
							// is null.
							if (originalElement == null)
							{
								// There's a problem because the element being updated
								// here is not in the original document. If this is a
								// repeater element, then it is not going to be possible
								// to work out which container element is correct. If
								// it is a repeater element, then the key(xpath) should
								// contain a where clause to point the correct element.
								// Examine the key(xpath) and try and work out the next
								// level i.e. the container element.
								Regex r = new Regex("^(?<grab>.*)/.*$");
								Match m = r.Match(key);
								if (m.Success)
								{
									// Get the cut down key.
                                    string cutKey = m.Groups["grab"].Value;
									try 
									{
										// See if we can get the orignal element now.
										originalElement = GetElement(originalBaseElement,cutKey);
										// Check to see if this has been achieved.
										if (originalElement != null)
											originalContainer = GetContainerElement(originalElement);
									} catch {}
								}
							}
							else
							{
								// The orignal element is in the XML document passed to
								// the class, so work out the container element from it.
								originalContainer = GetContainerElement(originalElement);
							}
						}
						if (originalContainer == null)
							originalContainer = GetElement(originalBaseElement, containerElement.LocalName);

						if (originalContainer != null) 
						{
							// Copy the entire container element from the original data

							// First the attributes
							CopyAttributes(originalContainer, containerElement);

							// Then the values
							//foreach(XmlNode n in originalContainer.ChildNodes)
							//{
							//	if ((n.NodeType==XmlNodeType.Element) && 
							//		(n.ChildNodes.Count==1) && 
							//		(n.ChildNodes[0].NodeType==XmlNodeType.Text))
							//	{
							//		XmlNodeList nl = containerElement.GetElementsByTagName(n.Name);
							//		if (nl.Count>0)
							//		{
							//			nl[0].InnerText = n.InnerText;
							//		}
							//	}
							//}

							// Set the UpdateType to "update" for the current container.
							SetUpdateType(UpdateType.update,containerElement);
						} 
						else 
						{
							// Create a new container element & set its type to "Add"
							SetUpdateType(UpdateType.add,containerElement);
							//SetCrmId(Guid.NewGuid().ToString(),containerElement);
						}

						// Copy or generate new CRMID's for all the parent container elements
						CopyDocumentIDs(containerElement,originalBaseElement);
				
						// Get the selected element again, as we've overwritten the container element
						selectedElement = GetElement(this.xmlTemplateDoc,key);
						if (selectedElement==null) 
						{
							string[] args = {key,this.xmlTemplateDoc.OuterXml };
							CrmServiceException ce = new CrmServiceException(
								"Library",
								"XmlError",
								"XmlTemplate.TemplateMismatch",
								this.trace,
								args);
							throw ce;
						}
					} 
					else
					{
						// Ensure that the update_type is 'update' fpr this
						// element.
						SetUpdateType(UpdateType.update,containerElement);
					}

					// Write the updated data into the template
					if (newValue == null) {
						XmlNode nilNode = selectedElement.Attributes.GetNamedItem("nil");
						if (nilNode == null) {
							continue;
						}
						nilNode.InnerText = "true";
					} else {
						selectedElement.InnerText = newValue;
					}
					XmlAttribute attr = this.xmlTemplateDoc.CreateAttribute(KEEP);
					selectedElement.Attributes.Append(attr);

					subElementChanged = true;

				}
			} finally {
				if (this.trace != null) {
					this.trace.EndProc(trState);
				}
			}
			return subElementChanged;
		}

		/// <summary>
		/// Overlay of UpdateSubElement without the CRMID and CRMIDName
		/// parameters.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="dataItems">The data items to be copied</param>
		/// <returns>true if any SubElements have been changed</returns>
		public bool UpdateSubElement( 
			XmlDocument originalXmlDoc, 
			Hashtable dataItems) {

			return this.UpdateSubElement(originalXmlDoc, dataItems, null, null, false);
		}

		/// <summary>
		/// Updates the supplied template XmlDocument to modify one or
		/// more subelements. 
		/// The data items must be supplied in a Hashtable, containing
		/// the items xpath in the key field, and the new data
		/// in the value field.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="dataItems">The data items to be copied</param>
		/// <param name="crmIdName">The element type to be copied</param>
		/// <param name="crmId">The element to be copied</param>
		/// <returns>true if any SubElements have been changed</returns>
		public bool UpdateSubElement(
			XmlDocument originalXmlDoc, 
			Hashtable dataItems,
			string crmIdName,
			string crmId
			)
		{
			return this.UpdateSubElement(originalXmlDoc, dataItems, crmIdName, crmId, false);
		}

		#endregion

		#region DeleteSubElement
		/// <summary>
		/// Updates the supplied template XmlDocument to delete a
		/// single subelement.
		/// </summary>
		/// <param name="originalXmlDoc">The original XML Document from which to copy information</param>
		/// <param name="crmIdName">The element type to be deleted</param>
		/// <param name="crmId">The element to be deleted</param>
		/// <returns>true if any SubElements have been changed</returns>
		public void DeleteSubElement(
			XmlDocument originalXmlDoc, 
			string crmIdName,
			string crmId) {
			ITraceState trState = null;

			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.DeleteSubElement");
				if (this.trace.TraceLevel.Debug) {	// print the parameter values
					if (originalXmlDoc==null) {
						this.trace.WriteDebug("originalXmlDoc = null");
					} else {
						this.trace.WriteDebug("originalXmlDoc = not null");
					}
					this.trace.WriteDebug("crmIdName = " + DeNull(crmIdName));
					this.trace.WriteDebug("crmId=" + DeNull(crmId));
				}
			}

			try {
				XmlElement originalBaseElement = SelectStartElement(originalXmlDoc, crmIdName, crmId);

				// Get the selected subelement from the template XmlDoc using an
				// xpath in the form '//*[@address_crmid]'.
				string xpath = "//*[@" + crmIdName + "]";
				XmlNode selectedNode = this.xmlTemplateDoc.SelectSingleNode(xpath);
				if ((selectedNode == null) || (selectedNode.NodeType != XmlNodeType.Element)) {
					string[] args = {xpath, selectedNode.OuterXml};
					CrmServiceException ce = new CrmServiceException(
						"Library",
						"XmlError",
						"XmlTemplate.MissingAttribute",
						this.trace,
						args);
					throw ce;
				}
				XmlElement selectedElement = (XmlElement)selectedNode;
				// Copy all attributes (including the CRMID) from the original element
				CopyAttributes(originalBaseElement, selectedElement);
				// Set the UpdateType to "delete"
				SetUpdateType(UpdateType.delete,selectedElement);
				// Copy or generate new CRMID's for all the parent container elements
				CopyDocumentIDs(selectedElement,originalBaseElement);
			} finally {
				if (this.trace != null)
					this.trace.EndProc(trState);
			}
		}
		#endregion

		#region GetXmlString
		/// <summary>
		/// Recursive routine called by GetXmlString to remove any elements which
		/// do not have the "KEEP" attribute and are empty.
		/// </summary>
		/// <param name="element">The Element being converted into a string</param>
		private void RemoveBlankFields(XmlNode element) {

			if (element == null) {
				return;
			}

			ArrayList elementsToRemove = new ArrayList();
			ArrayList attributesToRemove = new ArrayList();

			// Check field elements
			foreach(XmlNode n in element.SelectNodes("*[not(*)]")) {
				XmlNode keepNode = n.Attributes.GetNamedItem(KEEP);
				if (keepNode != null) {
					((XmlElement)n).RemoveAttribute(KEEP);
				} else if (n.InnerText == String.Empty) {
					XmlNode nullNode = n.Attributes.GetNamedItem("nil");
					if (nullNode == null) {
						elementsToRemove.Add(n);
					} else if (nullNode.Value != "true") {
						elementsToRemove.Add(n);
					}
				}
			}

			// Check field elements
			foreach(XmlNode n in element.SelectNodes("@*")) {
				if ((n.Value == String.Empty) && (n.Name != UPDATE_TYPE_ATTRIBUTE_NAME)) {
					attributesToRemove.Add(n);
				}
			}

			// If element check its subelements and attributes
			foreach(XmlNode n in element.SelectNodes("*")) {
				RemoveBlankFields(n);
			}

			// Delete any unwanted child nodes.
			for (int i=0; i<elementsToRemove.Count; i++) {
				element.RemoveChild((XmlNode)elementsToRemove[i]);
			}
			for (int i=0; i<attributesToRemove.Count; i++) {
				element.Attributes.Remove((XmlAttribute)attributesToRemove[i]);
			}
		}

		/// <summary>
		/// Returns a string containing the TemplateXML. Any unused parts of the
		/// XML will be removed. (i.e optimised)
		/// </summary>
		/// <returns></returns>
		public string GetXmlString() {

			ITraceState trState = null;
			if (this.trace != null) {
				trState = this.trace.StartProc("XmlTemplate.GetXmlString");
			}
			XmlDocument xmlDoc;
			try {
				xmlDoc = (XmlDocument)this.xmlTemplateDoc.Clone();
				// Check that if there is an update_version that has not been
				// populated, then this shoudl be zero.
				foreach(XmlNode node in xmlDoc.SelectNodes("//*/@update_version")) {
					if (node.Value.Equals("")) {
						// The update_version is null, so set it to 0.
						node.Value = "0";
					}
				}

				// Remove empty fields which don't have the "zzGSBUpdated" attribute.
				RemoveBlankFields(xmlDoc.DocumentElement);

				// Remove empty records
				bool finished = false;
				while (!finished) {
					// remove (1) record nodes with no updates and (2) and has no subrecords
					XmlNode node = xmlDoc.SelectSingleNode("//*[(@"+UPDATE_TYPE_ATTRIBUTE_NAME+"='"+UPDATE_TYPE_BLANK+"' or @"+UPDATE_TYPE_ATTRIBUTE_NAME+"='"+UPDATE_TYPE_NONE+"') and not(*)]");
					if (node != null) {
						node.ParentNode.RemoveChild(node);
						continue;
					} 
					// remove subrecords below a deleted record
					node = xmlDoc.SelectSingleNode("//*[@"+UPDATE_TYPE_ATTRIBUTE_NAME+"='"+UPDATE_TYPE_DELETE+"' or @"+UPDATE_TYPE_ATTRIBUTE_NAME+"='"+UPDATE_TYPE_REMOVE+"']/*");
					if (node != null) {
						node.ParentNode.RemoveChild(node);
						continue;
					}
					// remove records without fields and if it is not being deleted
					node = xmlDoc.SelectSingleNode("//*[@"+UPDATE_TYPE_ATTRIBUTE_NAME+"!='"+UPDATE_TYPE_DELETE+"' and @"+UPDATE_TYPE_ATTRIBUTE_NAME+"!='"+UPDATE_TYPE_REMOVE+"' and not(*)]");
					if (node != null) {
						node.ParentNode.RemoveChild(node);
						continue;
					}
					finished = true;
				}

				if (this.trace != null)
					this.trace.WriteDebug("Return Value = " + xmlDoc.OuterXml);

			} finally {
				if (this.trace != null)
					this.trace.EndProc(trState);
			}
			return xmlDoc.OuterXml;
		}
		#endregion
	}
}