#region Using
using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web.Caching;
using System.Globalization;
using System.Threading; 
using System.Resources;
using System.Reflection;
using System.Web.UI.HtmlControls;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary 
{
	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <summary>
	/// Generate asp from config files.
	/// </summary>
	/// 
	/// <development>
	///		<version number="1.18" day="19" month="05" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Defect 940</work_packet>
	///			<description>Removed use of sliding expiration for data cache.</description>
	///		</version>
	///		<version number="1.17" day="02" month="04" year="2003">
	///			<developer>Gary Bleads</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Performance Testing</work_packet>
	///			<description>Choices Function modified to improve performance. Session variables
	///			in 'var' are now tested to ensure they are strings starting with the 
	///			'greaterthan' character and with the 'lessthan' character before attempting
	///			to load them into the XML parser. Invalid XML causes an exception that takes
	///			around 10x longer to process than valid XML, so be applying simple 
	///			pre-selection we get a significant (~25%) overall performance improvement.
	///			Commented out some Trace statements in Choices().
	///			</description>
	///		</version>
	///		<version number="1.16" day="21" month="03" year="2003">
	///			<developer>Gary Bleads</developer>
	///			<checker>Bill Curtis</checker>
	///			<work_packet>Performance Testing</work_packet>
	///			<description>configXmlDoc.CloneNode replaced by 
	///			copyConfigXmlDoc.LoadXml(configXmlDoc.OuterXml) in configXmlDoc 
	///			to fix a performance problem. CloneNode appears to get slower and slower 
	///			during repeated calls.</description>
	///		</version>
	///		<version number="1.15" day="13" month="03" year="2003">
	///			<developer>Tom  Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>Defect 216</work_packet>
	///			<description>MakeChildControls method added to create child controls early</description>
	///		</version>
	///		<version number="1.14" day="11" month="03" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Defect 216</work_packet>
	///			<description>Added parameterisation of number to be returned
	///			in find.</description>
	///		</version>
	///		<version number="1.13" day="23" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added new helper method CheckAndExtractWidth.</description>
	///		</version>
	///		<version number="1.12" day="22" month="01" year="2003">
	///			<developer>Bill Curtis</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet></work_packet>
	///			<description>Added new method to get localised validation string.</description>
	///		</version>
	///		<version number="1.11" day="21" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Bill Curtis</checker>
	///			<work_packet></work_packet>
	///			<description>Added new method to enable localised string.</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.09" day="20" month="11" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add new getActualXpath method for comparing a control value with a data value</description>
	///		</version>
	/// 	<version number="1.08" day="12" month="11" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	/// 	<version number="1.08" day="12" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed WrapControlInHtml to fix problem with attributes
	///			being displayed in closing tag.</description>
	///		</version>
	/// 	<version number="1.07" day="06" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changes to support migration from Inproc session state
	///			to SQLServer.</description>
	///		</version>
	/// 	<version number="1.06" day="24" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Bug fix</work_packet>
	///			<description>Changes to CrmServer mean web references need updating.</description>
	///		</version>
	/// 	<version number="1.05" day="18" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>General enhancement.</work_packet>
	///			<description>Removed name of trace log from Trace constructor.</description>
	///		</version>
	/// 	<version number="1.04" day="17" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/barcelona/015</work_packet>
	///			<description>
	///			CSAControl gets a new property, ConfigDoc of class XmlDocument, which all descendant classes inherit. 
	///			The property get method will return the config xml document filtered on the choose/when conditions. 
	///
	///			Change the CSAControl.getAttribute… methods to use the new config file sub-elements in place of attributes.		
	///			</description>
	///		</version>
	/// 	<version number="1.03" day="24" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Add destructor method for clean up of logs.</description>
	///		</version>
	/// 	<version number="1.02" day="18" month="04" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Changed namespace to Fujitsu.eCrm.CSAControlLibrary.</description>
	///		</version>
	///		<version number="1.01" day="06" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Internationalisation and standardisation of error handling.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	///
	#endregion
	
	public class BaseControl : System.Web.UI.WebControls.WebControl, INamingContainer, IDisposable 
	{
		#region Attributes
		private static Trace trace = new Trace();
		private static readonly string CACHE_TAG_PREFIX;
		private const string DEFAULT_CACHE_TAG_PREFIX = "_CSAControlLibrary_";
		private static readonly int CACHE_RETENTION_MINUTES;
		private const int DEFAULT_CACHE_RETENTION_MINUTES = 5;
		private XmlElement dataXmlElement = null;
		private XmlDocument xmlDoc;
		private Hashtable controlIds;
		private HtmlGenericControl setFocus;
		internal InClass currentLocation;
		#endregion

		#region Enumeration
		internal enum InClass 
		{
			DataForm,
			PopupForm
		}
		#endregion

		#region Properties
		/// <summary>
		/// Set the control to be in focus when the page loads.
		/// </summary>
		internal HtmlGenericControl SetFocus
		{
			get { return this.setFocus; }
			set { this.setFocus = value; }
		}

		/// <summary>Get/Set the control to read only</summary>
		public bool ReadOnly 
		{
			get 
			{
				Object o = ViewState["readOnly"];
				if (o == null) 
				{
					return false;
				} 
				else 
				{
					return (bool)o;
				}
			}
			set 
			{
				ViewState["readOnly"] = value;
				Trace.WriteDebug("Setting Attribute ReadOnly=" + value.ToString());
			}
		}

		/// <summary>Get/Set the control's configuration file</summary>
		public string ConfigFile 
		{
			get 
			{
				Object o = ViewState["configFile"];
				if (o == null) 
				{
					return String.Empty;
				} 
				else 
				{
					return (string)o;
				}
			}
			set	
			{ 
				ViewState["configFile"] = value; 
				if (value == null) 
				{
					Trace.WriteDebug("Setting Attribute ConfigFile=null");
				} 
				else 
				{
					Trace.WriteDebug("Setting Attribute ConfigFile=" + value);
				}
			}
		}

		/// <summary>
		/// Get the configuration xml doc, defined in ConfigFile, with choose/when conditions resolved
		/// </summary>
		public XmlDocument ConfigXmlDoc
		{
			get
			{
				// Check the config file has been supplied
				if ((ConfigFile==null) || (ConfigFile.Length==0)) 
				{
					CrmServiceException ce = new CrmServiceException("Library", "Configuration", "ControlLibrary.MissingConfigFile", Trace, this.GetType().Name);
					throw ce;
				}
				//	throw new ApplicationException(
				//	"You must supply the ConfigFile attribute when using a DataForm");

				// Get the physical location of the configuration file on the disk
				string cacheTag = CacheTagPrefix + "_CONFIG_" + ConfigFile;

				// Try and get the Configuration XmlDoc from the Cache.
				XmlDocument configXmlDoc = (XmlDocument)Page.Cache[cacheTag];

				// Was there already a copy of the XMLDOC in the Cache?
				
	
				if (configXmlDoc != null) 
				
				{
					//  Yes. Use it.
					Trace.WriteDebug("Using existing XMLDocument " + cacheTag);
				} 
				else 
	
				
				{
					// No. Read the menu configuration XML from a file
					// Create an XMLDOM containing the file's content
					string xmlPath;
					HiResTimer timer2 = new HiResTimer();
					timer2.Start();
					configXmlDoc = new XmlDataDocument();
					try 
					{
						if (System.IO.Path.IsPathRooted(ConfigFile)) 
						{
							xmlPath = ConfigFile;
						} 
						else 
						{
							xmlPath = Page.MapPath(ConfigFile);
						}
						configXmlDoc.Load(xmlPath);
					} 
					catch (Exception e) 
					{
						CrmServiceException ce = new CrmServiceException("Library", "Configuration", "ControlLibrary.OpenConfigFileFailure", e, Trace, ConfigFile);
						throw ce;
						//throw new ApplicationException("Unable to open ConfigFile " + ConfigFile + " : " + e.Message);
					}

					timer2.Stop();
					Trace.WriteDebug("Created new XMLDocument from " + xmlPath + "Time"  + timer2.ElapsedMilliseconds.ToString("F3") + " ms" );

					// Save the XmlDom in the system cache so it's ready for next time.
					CacheDependency dependency = new CacheDependency(xmlPath);
					Page.Cache.Add(cacheTag,
						configXmlDoc,
						dependency, 
						System.DateTime.MaxValue, 
						System.TimeSpan.FromMinutes(CacheRetentionMinutes),
						System.Web.Caching.CacheItemPriority.Default,
						null);
				};

				// Clone the config document to effectively create an instance for the
				// current page load (otherwise changes on this page load will become
				// fixed for all following page loads)
				// (It's faster to create a brand new document that to clone the original)
				XmlDocument copyConfigXmlDoc = new XmlDocument();
				copyConfigXmlDoc.LoadXml(configXmlDoc.OuterXml);



				// resolve <sessionvar> nodes
				SessionVar(copyConfigXmlDoc);
				// resolve choose/when conditional clauses
				Choices(copyConfigXmlDoc);
				// return the parsed configuration file
				return copyConfigXmlDoc;

			}

		}

		/// <summary>
		/// Resolve sessionvar clauses, replace the referenced session variables
		/// with its value
		/// </summary>
		private void SessionVar(XmlDocument configXmlDoc) {

			foreach (XmlNode varNode in configXmlDoc.SelectNodes("//sessionvar")) {

				string varName = varNode.InnerText;
				string[] varNameElements = varName.Split( new char[]{'/'},2);
				object var = Page.Session[varNameElements[0]];

				// Set up default value to return if there is no session variable
				// or if the xpath founds nothing
				string varEval = String.Empty;
				XmlNode defaultNode = varNode.Attributes.GetNamedItem("default");
				if (defaultNode != null) {
					varEval = defaultNode.InnerText;
				}

				// Determine if the object is a string populated with XML.
				// If it is then set varEval to the inner text of the specified
				// node
				if (var != null) {
					try {
						XmlDocument xd = new XmlDocument();
						xd.LoadXml((string)var);
						string varPath = varNameElements[1];
						XmlNode xdNode = null;
						if (varPath == null) {
							xdNode = xd;
						} else {
							xdNode = xd.SelectSingleNode(varPath);
						}
						if (xdNode != null) {
							varEval = xdNode.InnerText;
						}
					} catch{
						varEval = var.ToString();
					}
				}

				// Replace the referenced session variable with its value
				XmlNode newChildNode = configXmlDoc.CreateTextNode(varEval);
				XmlNode parentNode = varNode.ParentNode;
				parentNode.ReplaceChild(newChildNode,varNode);
			}
		}

		/// <summary>
		/// Filters the config document, by selecting elements based on the choose/when/otherwise conditions.
		/// </summary>
		/// <param name="configXmlDoc">The document with choices to be made</param>
		/// <returns>The filtered document</returns>
		private void Choices(XmlDocument configXmlDoc) {
			ITraceState trState = Trace.StartProc("CSAControl.Choices");
			while (true) {
				// look for all the choose elements which are not nested
				XmlNodeList choices =  configXmlDoc.SelectNodes("descendant::choose[not (ancestor::choose)]");
				if (choices.Count==0) {
					break;
				}

				// Trace.WriteDebug("found " + choices.Count + " choose elements" );
				foreach ( XmlNode choice in choices) {
					XmlNodeList options = choice.SelectNodes("when | otherwise");
					XmlNode parent = choice.ParentNode;
					foreach (XmlNode option in options) {
						bool selected = false;
						if (option.Name == "otherwise") {
							//Trace.WriteDebug("otherwise element");
							selected = true;
						} else {
							// get the operator
							string op = option.Attributes["op"].Value;

							// get the comparison values
							string varEval = null;
							XmlAttribute val = option.Attributes["val"];

							// get the variable name and use the first part as a session variable key, 
							// and the rest as xpath for searching xml docs
							if (option.Attributes["var"] != null) {

								string varName = option.Attributes["var"].Value;

								object var = Page.Session[varName.Split( new char[]{'/'})[0]];

								// Determine if the object is a string populated with XML.
								// If it is then reassign var to the specified node
								XmlDocument xd = new XmlDocument();
								if ((var != null) & (var is string)) {
									try {
										string varStr = ((string)var).Trim();
										// For performance reasons, Only attempt to XML parse
										// strings that start with '<' and finish with '>'
										if ((varStr.Length > 2) &&	
											(varStr.StartsWith("<")) && 
											(varStr.EndsWith(">"))) {
											xd.LoadXml(varStr);
											if (varName.IndexOf('/') == -1) {
												var = xd;
											} 
											else {
												string varPath = varName.Substring(varName.IndexOf('/')+1);
												var = xd.SelectSingleNode(varPath);
											}
										}
									} catch{}
								}

								// Evaluate the variable to a string, which will be tested
								if (var != null) {
									switch (op) {
										case "not equals":
										case "equals":
											if (var is XmlNode) {
												varEval = ((XmlNode)var).InnerText;
											} else {
												varEval = var.ToString();
											}
											break;
										case "not exists":
										case "exists":
											varEval = String.Empty;
											break;
									}
								}

							} else {
								string key = option.Attributes["pagecache"].Value;
								object temp = Page.Cache[key];
								if (temp != null) {
									varEval = temp.ToString();
								}
							}

							switch (op) {							
								case "not equals": 
									selected = (varEval != val.Value);
									break;
								case "equals":
									selected = (varEval == val.Value);
									break;
								case "exists":
									selected = (varEval != null);
									break;
								case "not exists":
									selected = (varEval == null);
									break;
							}
						}
						if (selected) {
							// copy the contents of the selected choice to precede the choose element
							foreach (XmlNode choosenNode in option.ChildNodes) {
								parent.InsertBefore(choosenNode.CloneNode(true),choice);
							}
						}
						//Trace.WriteDebug("after choice selected = " + selected + ", xml Node = '" + parent.OuterXml + "'");
						if (selected) {
							// break the foreach loop on options
							break; 
						}
					}
					// remove the choose element;
					parent.RemoveChild(choice);
				}
			}
			Trace.EndProc(trState);
		}


		/// <summary>Get/Set the control's XML Document</summary>
		public XmlDocument XmlDoc 
		{
			get	{ return this.xmlDoc; }
			set	
			{
				this.xmlDoc = value; 
				this.SetXmlElement();
				if (value == null) 
				{
					Trace.WriteDebug("Setting Attribute XmlDoc=null");
				} 
				else 
				{
					Trace.WriteDebug("Setting Attribute XmlDoc=" + value.OuterXml);
				}
			}
		}

		/// <summary>Get/Set the control's current XPath</summary>
		public string XPath 
		{
			get 
			{
				Object o = ViewState["xpath"];
				if (o == null) 
				{
					return String.Empty;
				} 
				else 
				{
					return (string)o;
				}
			}
			set	
			{
				ViewState["xpath"] = value; 
				this.SetXmlElement();
				if (value == null) 
				{
					Trace.WriteDebug("Setting Attribute XPath=null");
				} 
				else 
				{
					Trace.WriteDebug("Setting Attribute XPath=" + value);
				}
			}
		}

		/// <summary>Get the control's Trace object</summary>
		public static Trace Trace { get { return BaseControl.trace; } }

		/// <summary>Get the control's current XML Element</summary>
		protected XmlElement DataXmlElement { get { return this.dataXmlElement; } }

		/// <summary>Get the control's Cache Tage Prefix</summary>
		protected static string CacheTagPrefix { get { return BaseControl.CACHE_TAG_PREFIX; } }

		/// <summary>Get the control's Cache retention threshold</summary>
		protected static int CacheRetentionMinutes { get { return BaseControl.CACHE_RETENTION_MINUTES; } }		

		#endregion	

		#region Constructor
		/// <summary>
		/// Set up caching options, taken from machine or web config file
		/// </summary>
		static BaseControl() {
			string cacheTagPrefixAppSetting = System.Configuration.ConfigurationSettings.AppSettings["CacheTagPrefix"];
			CACHE_TAG_PREFIX = 
				(cacheTagPrefixAppSetting == null) ? 
				DEFAULT_CACHE_TAG_PREFIX : 
				cacheTagPrefixAppSetting;

			string cacheRetentionMinutesAppSetting = System.Configuration.ConfigurationSettings.AppSettings["CacheRetentionMinutes"];
			try {
				int cacheRetentionMinutesAppIntSetting = int.Parse(cacheRetentionMinutesAppSetting);
				// make sure cache is within limits (between 0 and 1 year)
				if (cacheRetentionMinutesAppIntSetting < 0) {
					cacheRetentionMinutesAppIntSetting = 0;
				} else if (cacheRetentionMinutesAppIntSetting > (365*24*60)) {
					cacheRetentionMinutesAppIntSetting = (365*24*60);
				}
				CACHE_RETENTION_MINUTES = cacheRetentionMinutesAppIntSetting;
			} catch {
				CACHE_RETENTION_MINUTES = DEFAULT_CACHE_RETENTION_MINUTES;
			}
		}

		/// <summary>
		/// Constructor a CSA Web Control
		/// </summary>
		public BaseControl() 
		{
			Trace.WriteDebug("In BaseControl Constructor");
		}
		#endregion

		#region XML Access Methods
		/// <summary>
		/// Sets the dataXmlElement attribute from the XmlDocument parameter,applying 
		/// the contents of the XPath parameter if it has been supplied.
		/// </summary>
		private void SetXmlElement() 
		{
			if (this.XmlDoc == null){
				this.dataXmlElement = null;
			} else if ((this.XPath != null) && (this.XPath.Length > 0)) {
				// An XPath string has been supplied. Select the element specified by the XPath
				XmlNode node = this.XmlDoc.SelectSingleNode(XPath);
				if (node == null) {
					this.dataXmlElement = null;
				} else if (node.NodeType == XmlNodeType.Element) {
					this.dataXmlElement = (XmlElement)node;
				} else {
					CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.InvalidXPathString", Trace, XPath);
					throw ce;
					//throw new ApplicationException(
					//	"The XPath string '" + XPath + 
					//	"' does not represent a valid Xml Element");
				}
			} else {
				// No XPath specified, so default to the top-level document element.
				this.dataXmlElement = this.XmlDoc.DocumentElement;
			}
		}

		/// <summary>
		/// Reads the attributes of the supplied field elelement EL and
		/// returns the following strings:
		/// </summary>
		/// <param name="el">The XML Element</param>
		/// <param name="actualXPath">The contents of the XPath attribute</param>
		/// <param name="xPathItemName">the last part of the xpath after the final</param>
		/// <param name="id">Currently the same as XPathItemName</param>
		/// <param name="caption">The contents of the XPath attribute. Defaults to the XPathItemName</param>
		/// <param name="tip">tool tip text</param>
		/// <param name="format">format for displaying text</param>
		internal void GetFieldDetails(
			XmlElement el, 
			out string actualXPath,
			out string xPathItemName, 
			out string id, 
			out string caption,
			out string tip,
			out string format) 
		{

			string xpath = GetAttributeString(el, "xpath");
			caption = GetAttributeString(el, "caption");
			tip = GetAttributeString(el, "tip");
			format = GetAttributeString(el, "format");

			id = xpath;

			// Ensure the xpath value has been supplied
			if (xpath.Length == 0) {
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.MissingXPathAttribute", BaseControl.Trace, el.OuterXml);
				throw ce;
			}

			actualXPath = GetActualXPath(xpath);
			

			// Get part of the XPath attribute after the last "/" )
			int i = xpath.LastIndexOf("/");
			if (i >= 0) {
				xPathItemName = xpath.Substring(i+1);
			} else {
				xPathItemName = xpath;
			}

			// remove any xpath specific characters
			xPathItemName = xPathItemName.Replace("@",String.Empty);
			xPathItemName = xPathItemName.Replace("*",String.Empty);
			xPathItemName = xPathItemName.Replace(":",String.Empty);
			xPathItemName = xPathItemName.Replace("[",String.Empty);
			xPathItemName = xPathItemName.Replace("]",String.Empty);

			// Get the control's ID, or default it to the name of the data item
			if (id.Length == 0) {
				id = xPathItemName;
			}

			// If no caption supplied, use the data item name, replacing
			// the underscores with spaces and capitalising the first letter of
			// each word. i.e. "address_line_1" becomes "Address Line 1"
			if (caption.Length == 0) {
				string[] words = xPathItemName.Split(new Char[]{'_',' '});
				for (i=words.GetLowerBound(0); i<=words.GetUpperBound(0);i++) {
					string wrd = words[i];
					if (wrd.Length > 1) {
						words[i] = wrd.Substring(0,1).ToUpper() + wrd.Substring(1).ToLower();
					} else {
						words[i] = wrd.ToUpper();
					}
				}
				caption = String.Join(" ", words);
			}
		}
		/// <summary>
		/// transforms an xpath fragment into xpath which starts at the data element
		/// </summary>
		/// <param name="xpath">original xpath fragment</param>
		/// <returns>xpath from the document root or data element</returns>
		internal protected static string GetActualXPath(string xpath)
		{
			string actualXPath = xpath;
			// Attributes
			if (actualXPath.StartsWith("@")) 
			{
				actualXPath = "*/" + actualXPath;
			}

			if ((!actualXPath.StartsWith("/")) && (!actualXPath.StartsWith("."))) 
			{
				actualXPath = "descendant-or-self::" + actualXPath;
			} 
			return actualXPath;

		}

		/// <summary>
		/// Returns a string corresponding to the value of a node's child element.
		/// If the child element does not exists, returns an empty string.
		/// </summary>
		/// <param name="node">The XML Element</param>
		/// <param name="attributeName">The name of the XML Element's child element</param>
		/// <returns>The value of the child element</returns>
		public static string GetAttributeString(XmlNode node, string attributeName) {
			XmlText attribText = (XmlText) node.SelectSingleNode(attributeName +"/text()");
			if (attribText == null) {
				return String.Empty;
			} else {
				string attribValue = attribText.Value;
                   string returnedValue = Localization.GetLocalizedAttributeString(attribValue);

				if (returnedValue.Equals(attribValue)) {
					returnedValue=Localization.GetLocalizedFormatString(attribText.Value);
				}
				return returnedValue;
			}
		}

		/// <summary>
		/// Returns an integer corresponding to the value of a node's child element.
		/// If the child element does not exist or is invalid, returns -1.
		/// </summary>
		/// <param name="node">The XML Element</param>
		/// <param name="attributeName">The name of the XML Element's child element</param>
		/// <returns>The value of the child element</returns>
		public static int GetAttributeInt(XmlNode node, string attributeName) 
		{
			int i;
			string attribValue;
			XmlText attribText = (XmlText) node.SelectSingleNode(attributeName +"/text()");
			Regex intString = new Regex("^-*[0-9]+$");
			if (attribText == null) {
				return -1;
			} else if (intString.Match(attribText.Value).Success) {
				// it's already an integer 
				attribValue = attribText.Value;
			} else {	
				attribValue = Localization.GetLocalizedAttributeString(attribText.Value);

				if (attribValue.Equals(attribText.Value)) {
					attribValue=Localization.GetLocalizedFormatString(attribText.Value);

					if (attribValue.Equals(attribText.Value)) {
						attribValue=Localization.GetLocalizedValidationString(attribText.Value);
					}
				}
			}

			try 
			{
				i = int.Parse(attribValue);
			}	
			catch  
			{
				i = -1;
			}
			
			return i;
		}

		/// <summary>
		/// Returns a bool corresponding to the value of a node's child-element.
		/// If the child-element does not exist or is invalid, returns false.
		/// </summary>
		/// <param name="node">The XML Element</param>
		/// <param name="attributeName">The name of the XML Element's child-element</param>
		/// <returns>The boolean value of the child element</returns>
		public static bool GetAttributeBool(XmlNode node, string attributeName) 
		{
			XmlText attribText = (XmlText) node.SelectSingleNode(attributeName +"/text()");

			if (attribText == null) 
			{
				return false;
			} 
			else
			{
				string attribValue = Localization.GetLocalizedAttributeString(attribText.Value);
				if (attribValue.Equals(attribText.Value))
				{
					attribValue=Localization.GetLocalizedFormatString(attribText.Value);
				}

				string s = attribValue.ToLower();
				if ((s == "true") || (s == "1") || (s == "y")) 
				{
					return true;
				} 
				else 
				{
					return false;
				}
			}
		}

		#region Get List of Key-Value Pairs
		/// <summary>
		/// Retrieve reference data as specified by the options node
		/// </summary>
		/// <param name="optionsNode">The options node</param>
		/// <param name="keyList">The list of keys</param>
		/// <param name="textList">The description of the keys</param>
		/// <param name="orderList">The order of showing the description</param>
		/// <param name="ignoreList">The list of keys to ignore unless it matches the data value</param>
		internal protected void GetOptionsData(XmlNode optionsNode, out ArrayList keyList, out ArrayList textList, out ArrayList orderList, out ArrayList ignoreList) {

			keyList = new ArrayList();
			textList = new ArrayList();
			orderList = new ArrayList();
			ignoreList = new ArrayList();

			foreach (XmlNode sourceNode in optionsNode.ChildNodes) {
				switch (sourceNode.Name)	{
					case "option":
						string key = GetAttributeString(sourceNode,"valuefield");
						string text = GetAttributeString(sourceNode,"textfield");
						string order = GetAttributeString(sourceNode,"orderfield");
						XmlNode n = sourceNode.SelectSingleNode("orderfield");
						if (n != null) {
							order = n.InnerText;
						} else {
							order = text;
						}

						keyList.Add(key);
						textList.Add(text);
						orderList.Add(order);
						break;
					case "referencedata":
						GetReferenceData(sourceNode,keyList,textList,orderList);
						break;
					case "controldata":
						GetControlData(sourceNode,keyList,textList,orderList);
						break;
					case "ignore":
						foreach (XmlNode node in sourceNode.SelectNodes("valuefield")) {
							string ignoreKey = GetAttributeString(node,".");
							ignoreList.Add(ignoreKey);
						}
						break;
					default:
						break;
				}
			}
		}

		private void GetReferenceData(XmlNode referenceNode, ArrayList keyList, ArrayList textList, ArrayList orderList) {

			/*
            #region Read Configuration Details
			string domain = GetAttributeString(referenceNode,"domain");
			if (domain.Length == 0) {
				string[] args = new String[]{};
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.ReferenceDataDomainMissing", args);
				throw ce;
			}

			string view = GetAttributeString(referenceNode,"view");
			if (view.Length == 0)	 {
				view = domain;  // default to the standard domain's view
			}

			string textPath = GetAttributeString(referenceNode,"textfield");
			if (textPath.Length==0) {
				textPath = domain;
			}

			string keyPath = GetAttributeString(referenceNode,"valuefield");
			if (keyPath.Length==0) {
				keyPath = textPath;
			}

			string orderPath = GetAttributeString(referenceNode,"orderfield");
			if (orderPath.Length==0) {
				orderPath = textPath;
			}

			int maxFind = GetAttributeInt(referenceNode,"findnumber");
			if (maxFind <= 0)
				// Get as many records as you can, up to the limit specified in the admin_configuration
				maxFind = int.MaxValue; 

			keyPath = GetActualXPath(keyPath);
			textPath = GetActualXPath(textPath);
			orderPath = GetActualXPath(orderPath);

			bool restrict = GetAttributeBool(referenceNode,"restrict");
			XmlNode noCache = referenceNode.SelectSingleNode("nocache");
			#endregion

			#region Try get Reference Data from cache
			XmlDocument xmlDoc = null;
			string cacheTag = String.Empty;
			// Check if we are using the cache for this data.
			if (noCache == null)
			{
				// Try and get the XMLDocument containing the requested domain from the Cache.
				cacheTag = BaseControl.CacheTagPrefix + "_REFDATA_" + domain + "$" + view + "$" + this.Page.Session[WebSessionIndexes.Culture()];
				//XmlDocument xmlDoc = (XmlDocument)this.LinkPage.Cache[cacheTag];
				xmlDoc = (XmlDocument)this.Page.Cache[cacheTag];
			}
			#endregion

			#region Get Reference Data from server if not in cache
			// Was there already a copy of the reference data XmlDocument in the Cache?
			if (xmlDoc != null) {
				//  Yes. Use it.
				//this.LinkPage.Trace.Write("Using existing Reference Data : " + cacheTag);
				BaseControl.Trace.WriteDebug("Using existing Reference Data : " + cacheTag);
			} else {
				// No. Fetch the reference data from the server
				BaseControl.Trace.WriteDebug("Downloading new Reference Data " + cacheTag);

				string sessionId = (string)this.Page.Session[WebSessionIndexes.SessionId()];
				string resultXml;
				string viewXml;

				Service crmSvr = new Service();
				crmSvr.Url = System.Configuration.ConfigurationSettings.AppSettings["CRMServerURL"];

				bool success; 
				bool triedToConnect = false;
				while (true) {

					if (restrict) {
						//Only get details pertaining to the current customer
						success = crmSvr.Get(sessionId, domain, view, (string)this.Page.Session[WebSessionIndexes.CustomerCrmId()], out resultXml, out viewXml);
					} else {
						//Find details pertaining to all customers
						ConditionList emptyConditionList = new ConditionList();
						string where = emptyConditionList.ToString();
						int rowCount, endIndex;
						success = crmSvr.Find(sessionId, domain, view, where, 1, maxFind, out resultXml, out viewXml, out rowCount, out endIndex);
					}

					if (success) {
						break;
					} else if (!triedToConnect) {
						triedToConnect = true;

						sessionId = null;
						Result result = new Result();
						result.LoadXml(resultXml);
						result.MoveToTopResult();
						string name;
						result.GetResultElementByName("name",out name);

						if (name == "Connection.Validate") {
							string userName = (string)this.Page.Session[WebSessionIndexes.UserId()];
							string password = (string)this.Page.Session[WebSessionIndexes.Password()];
							string culture = (string)this.Page.Session[WebSessionIndexes.Culture()];
							string capabilityXml;
							success = crmSvr.Connect(userName,password,culture,out sessionId, out capabilityXml, out resultXml);
							if (success) {
								this.Page.Session[WebSessionIndexes.Capabilities()] = capabilityXml;
								this.Page.Session[WebSessionIndexes.SessionId()] = sessionId;
							} 
						}

						if (!success) {
							break;
						}
					} else {
						break;
					}

				}

				if (!success) {
					Result res = new Result();
					res.LoadXml(resultXml);
					throw (res.ToException());
				}

				xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(viewXml);

				// Save the XmlDocument in the system cache so it's ready for next time.
				if (noCache == null)
				{
					this.Page.Cache.Add(cacheTag,
						xmlDoc,
						null,
						System.DateTime.Now.AddMinutes(BaseControl.CacheRetentionMinutes),
						Cache.NoSlidingExpiration,
						System.Web.Caching.CacheItemPriority.Normal,
						null);
				}
			}
			#endregion

			*/
            //CopyData(xmlDoc,keyPath,textPath,orderPath,keyList,textList,orderList);

		}

		private void GetControlData(XmlNode referenceNode, ArrayList keyList, ArrayList textList, ArrayList orderList) {

			#region Read Configuration Details
			string controlId = GetAttributeString(referenceNode,"controlid");
			if (controlId.Length == 0) {
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.ControlDataControlIdMissing", controlId);
				throw ce;
			}

			string textPath = GetAttributeString(referenceNode,"textfield");
			if (textPath.Length==0) {
				textPath = controlId;
			}

			string keyPath = GetAttributeString(referenceNode,"valuefield");
			if (keyPath.Length==0) {
				keyPath = textPath;
			}

			string orderPath = GetAttributeString(referenceNode,"orderfield");
			if (orderPath.Length==0) {
				orderPath = textPath;
			}

			keyPath = GetActualXPath(keyPath);
			textPath = GetActualXPath(textPath);
			orderPath = GetActualXPath(orderPath);
			#endregion

			#region Get Base Control's XML Document
			XmlDocument xmlDoc = null;
			Control sourceControl = FindControl(controlId, this.Page);
			if (sourceControl is BaseControl) {
				xmlDoc = ((BaseControl)sourceControl).XmlDoc;
			}
			#endregion

			CopyData(xmlDoc,keyPath,textPath,orderPath,keyList,textList,orderList);

		}

		private static void CopyData(XmlDocument xmlDoc, string keyPath, string textPath, string orderPath, ArrayList keyList, ArrayList textList, ArrayList orderList) {
			if (xmlDoc != null) {
				// Get the values and text from the XmlDocument
				XmlNodeList keyNodes = xmlDoc.SelectNodes(keyPath);
				XmlNodeList textNodes = xmlDoc.SelectNodes(textPath);
				XmlNodeList orderNodes = xmlDoc.SelectNodes(orderPath);

				if (keyNodes.Count != textNodes.Count) {
					CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.ReferenceDataMismatch", keyPath, textPath);
					throw ce;
				}
				if (keyNodes.Count != orderNodes.Count) {
					CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.ReferenceDataMismatch", keyPath, orderPath);
					throw ce;
				}
				// Create a ListItem array containing the values takes from the XmlDocument
				for (int i=0;i<textNodes.Count;i++) {
					keyList.Add(keyNodes[i].InnerText);
					textList.Add(textNodes[i].InnerText);
					orderList.Add(orderNodes[i].InnerText);
				}
			}
		}

		private static Control FindControl(string id,Control outerControl) {
			if (outerControl.ID == id) {
				return outerControl;
			} else {
				foreach (Control innerControl in outerControl.Controls) {
					Control findControl = FindControl(id,innerControl);
					if (findControl != null) {
						return findControl;
					}
				}
				return null;
			}
		}
		#endregion
		#endregion

		#region Generate Control ID's
		/// <summary>
		/// Generate a unique control ID.
		/// </summary>
		/// <param name="controlName">The name of the control to generate ID for.</param>
		/// <param name="configNode">The configuration file to extract potential
		/// control ID names from.</param>
		/// <returns>A unique control ID.</returns>
		internal string GenerateControlId(
			string controlName,
			XmlNode configNode
			)
		{
			string newSuffix = String.Empty;
			bool gotId = false;
			// Check if there is XML to investigate.
			if (configNode != null)
			{
				// Check if the XML has an ID node.
				XmlNode idNode = configNode.SelectSingleNode("id/text()");//don't localize
				if (idNode != null)
				{
					newSuffix = idNode.Value;
					gotId = true;
				}
				if (!gotId)
				{
					// Check if there is a header, which is primarily for a dataform.
					idNode = configNode.SelectSingleNode("header/text()");
					if (idNode != null)
					{
						newSuffix = idNode.Value;
						gotId = true;
					}
				}
			}
			// Check if an ID has been found yet.
			if (!gotId)
				// Use the hashtable to generate a new id.
				newSuffix = this.CheckControlIdTable(controlName,false);
				int dotPosn =newSuffix.LastIndexOfAny(".".ToCharArray())+1;
				if (dotPosn >=0)
				{
					newSuffix =  newSuffix.Substring(dotPosn);
				}

			// Build the control ID.
			string controlId = controlName + newSuffix;
			// Now check that the control ID hasn't already been used.
			controlId = controlId + this.CheckControlIdTable(controlId,true);
			return controlId;
		}

		/// <summary>
		/// Generate a unique control ID.
		/// </summary>
		/// <param name="controlName">The name of the control to generate ID for.</param>
		/// <returns>A unique control ID.</returns>
		internal string GenerateControlId(
			string controlName
			)
		{
			return GenerateControlId(controlName,null);
		}

		/// <summary>
		/// Use the control ID hashtable to generate a suffix for
		/// a control depending upon its name.
		/// </summary>
		/// <param name="controlName">The name of the control.</param>
		/// <param name="checkAlreadyUsed">Check if the final control
		/// ID is already being used. Will generate another suffix
		/// if it is.</param>
		/// <returns>The new suffix.</returns>
		private string CheckControlIdTable(
			string controlName,
			bool checkAlreadyUsed
			)
		{
			string newSuffix = String.Empty;
			// Check if the control ID hashtable has been instantiated.
			if (this.controlIds == null)
				this.controlIds = new Hashtable();
			// Check if this an ID for this control has been asked for
			// previously.
			if (this.controlIds.ContainsKey(controlName))
			{
				// It has, so obtain the current integer value and increment
				// by 1.
				int suffix = (int)this.controlIds[controlName] + 1;
				// Store the new value.
				this.controlIds[controlName] = suffix;
				// Set the suffix.
				newSuffix = suffix.ToString();
			}
			else
			{
				// Is this a check for the ID being already in use.
				if (checkAlreadyUsed)
				{
					// This is a check for usage. As the hashtable doesn't
					// contain the value, a suffix isn't needed. Set to an
					// empty string.
					newSuffix = String.Empty;
					// Store this entry in the database.
					this.controlIds.Add(controlName,0);
				}
				else
				{
					// This is not a check for usage. This ID will require
					// a starting suffix of 1.
					newSuffix = "1";
					// Store this entry in the database.
					this.controlIds.Add(controlName,1);

				}
			}
			return newSuffix;
		}
		#endregion

		#region Helper Methods
		/// <summary>
		/// Examine the specified width of the table or columns. Obtain the integer
		/// value and determine the unit e.g. pixel or percentage.
		/// </summary>
		/// <param name="dimensionString">The string to be parsed holding the width.</param>
		/// <param name="elementName">The name of the element being examined.</param>
		/// <param name="unitType">The unit type. Will be 'pixel' or 'percentage'.</param>
		/// <returns>Will return the width value.</returns>
		public static int CheckAndExtractDimension(
			string dimensionString,
			string elementName,
			out UnitType unitType
			)
		{
			Regex r = new Regex(@"^(?<dimension>\d+)(?<unit>\%)?$",RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m = r.Match(dimensionString);
			int dimension = -1;
			unitType = UnitType.Percentage;
			if (m.Success)
			{
				// A match has been found with the string, so obtain the
				// width.
				dimension = Convert.ToInt32(m.Groups["dimension"].Value);
				// Get the unit as well.
				switch (m.Groups["unit"].Value)
				{
					case "%":
						unitType = UnitType.Percentage;
						break;
					default:
						unitType = UnitType.Pixel;
						break;
				}
			}
			else 
			{
				// A match hasn't been found, so throw an error as the
				// content will be invalid.
				CrmServiceException ce = new CrmServiceException("Library",
					"Data","CSAControlLibrary.InvalidElementContent",dimensionString,elementName);
				throw ce;
			}
			return dimension;
		}
		#endregion

		#region HTML Wrap
		/// <summary>
		/// Wrap a control in multiple SPAN tags with a
		/// class name e.g.
		/// 
		///   SPAN class="overall class"
		///      SPAN class="specific class"
		///      
		/// </summary>
		/// <param name="control">The control to be wrapped.</param>
		/// <param name="controlOverallClass">The name of the overall class.</param>
		/// <param name="controlSpecificClass">The name of the specific class.</param>
		/// <returns></returns>
		public static Control WrapControlInHtml(
			Control control,
			string controlOverallClass,
			string controlSpecificClass
			) 
		{

			// The controlSpecificClass may have been supplied as a xpath,
			// so get rid of the //.
			Regex regex = new Regex("/");
			controlSpecificClass = regex.Replace(controlSpecificClass,"");
			// Need to enclose the main table in a <SPAN> statement and
			// add the class name as the control name i.e. popup.
			HtmlGenericControl htmlControl = new HtmlGenericControl("span");
			htmlControl.Attributes["class"] = controlOverallClass.ToLower() + " " +	controlSpecificClass.ToLower();
			// Add the control to be wrapped to the sub HTML control.
			htmlControl.Controls.Add(control);
			// Return the control.
			return htmlControl;

		}
		/// <summary>
		/// makes sure the child controls have been created
		/// </summary>
		public  void MakeChildControls()
		{
			this.EnsureChildControls();
		}

		#endregion

		#region Destructor
		/// <summary>
		/// Destructor. Called by the Garbage Collector because
		/// the class is derived from IDisposable.
		/// 
		/// See Wrox Professional C# Second Edition Pages 142-146
		/// </summary>
		~BaseControl() 
		{
		}
		#endregion
	}
}