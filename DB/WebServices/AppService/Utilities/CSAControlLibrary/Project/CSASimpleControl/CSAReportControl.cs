#region Using
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate a texbox.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class ReportControl : System.Web.UI.WebControls.Xml, ISimpleControl {

		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) {}
		#endregion

		#region Constructor
		/// <summary>
		/// Construct a control containing a text box
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public ReportControl(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement,
			XmlElement dataXmlElement) {

			try {
				// Get Configuration Details
				this.TransformSource = BaseControl.GetAttributeString(configXmlElement, "transformsource");

				if (dataXmlElement == null) {
					return;
				}

				XmlNode queryNode = configXmlElement.SelectSingleNode("./report");
				XmlNode layoutNode = configXmlElement.SelectSingleNode("./layout");

				/*Service CrmService = new Service();
				CrmService.Url = System.Configuration.ConfigurationSettings.AppSettings["CRMServerURL"];

				foreach (XmlAttribute xpathNode in queryNode.SelectNodes("//@xpath")) {
					string xpath = xpathNode.InnerText;

					XmlNode sourceNode = dataXmlElement.SelectSingleNode(xpath);
					if (sourceNode != null) {
						XmlNode parentNode = xpathNode.OwnerElement;
						parentNode.InnerText = sourceNode.InnerText;
					}

				}

				string reportOutXml;
				string resultXml;

				// Invoke the Report service to get the answers
				if (CrmService.Report((string)parent.Page.Session[WebSessionIndexes.SessionId()],queryNode.OuterXml,out reportOutXml,out resultXml)) {

					this.Document = new XmlDocument();
					this.Document.LoadXml(reportOutXml);

					// create a new transformer based on the Transform Source file
					// This doesn't directly transform the report data but generates more xslt from the report script which will.
					XslTransform reportToXslt = new XslTransform();
					string xsltFile = parent.Page.MapPath(this.TransformSource);
					reportToXslt.Load(xsltFile);
							
					// Take the layout section of the report script and transform it into xslt,
					// which will transform the report into an HTML table
					this.Transform = new XslTransform();
					this.Transform.Load(reportToXslt.Transform(layoutNode.CreateNavigator(),new XsltArgumentList()));

				}

				// Add Control's Caption to Page's Table Row
				TableCell controlTableCell = new TableCell();
				controlTableCell.Controls.Add(this);
				controlTableRow.Cells.Add(controlTableCell);
                 */
			} catch (Exception e) {
				string x = e.Message;
			}

		}
		#endregion

	}
}