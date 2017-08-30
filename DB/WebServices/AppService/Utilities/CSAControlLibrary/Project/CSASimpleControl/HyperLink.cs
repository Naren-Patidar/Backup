#region Using
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate a hyperlink.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Gary Bleads</checker>
	///			<work_packet>RS2003</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class HyperLink : System.Web.UI.WebControls.HyperLink, ISimpleControl
	{
		#region Get Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Create a new HyperLink control.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="controlTableRow"></param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public HyperLink(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) 
		{
			// Get the ID if there is one.
			string id = BaseControl.GetAttributeString(configXmlElement,"id");
			// Get an unlocalised url to use for when there isn't a id.
			string unlocalisedNavigateUrl = String.Empty;
			XmlNode node = configXmlElement.SelectSingleNode("navigateurl/text()");
			if (node != null)
				unlocalisedNavigateUrl = node.Value;
			// Get the tooltip.
			string tip = BaseControl.GetAttributeString(configXmlElement,"tip");
			// Get the text and localise.
			string url = BaseControl.GetAttributeString(configXmlElement,"url");
			// Get the text and localise.
			string text = BaseControl.GetAttributeString(configXmlElement,"text");
			// Get the image url and localise.
			string imageUrl = BaseControl.GetAttributeString(configXmlElement,"imageurl");
			// Get the navigate url and localise.
			string navigateUrl = BaseControl.GetAttributeString(configXmlElement,"navigateurl");
			// Get the height.
			int height = BaseControl.GetAttributeInt(configXmlElement,"hyperlinkheight");
			// Get the width.
			int width = BaseControl.GetAttributeInt(configXmlElement,"hyperlinkwidth");
			// Set the ID if there is one.
			if (!StringUtils.IsStringEmpty(id))
				this.ID = id;
			else
				this.ID = StringUtils.RemoveStrings(unlocalisedNavigateUrl," ",".","@","*",":","[","]");
			// Set the text if there is one.
			if (!StringUtils.IsStringEmpty(text))
				this.Text = text;
			// Set the image url if there is one.
			if (!StringUtils.IsStringEmpty(imageUrl))
				this.ImageUrl = imageUrl;
			// Set the tooltip for the image if there is one.
			if (!StringUtils.IsStringEmpty(tip))
				this.ToolTip = tip;
			// Set the tooltip for the image if there is one.
			if (!StringUtils.IsStringEmpty(navigateUrl))
				this.NavigateUrl = navigateUrl;
			if (width > 0)
				this.Width = width;
			// Set the image height.
			if (height > 0)
				this.Height = height;
			// Add the hyperlink to the pages row.
			TableCell controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlhyperlink","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}