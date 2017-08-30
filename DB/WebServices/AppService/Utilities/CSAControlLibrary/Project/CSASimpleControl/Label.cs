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
	/// Generate a label.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class Label : System.Web.UI.WebControls.Label, ISimpleControl
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
		/// Create a new Label control.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="controlTableRow"></param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public Label(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) 
		{
			// Get the ID if there is one.
			string id = BaseControl.GetAttributeString(configXmlElement,"id");
			// Get the tooltip.
			string tip =BaseControl.GetAttributeString(configXmlElement,"tip");
			// Get the text and localise.
			string text = 
				BaseControl.GetAttributeString(configXmlElement,"text");
			// Get the height.
			int height = BaseControl.GetAttributeInt(configXmlElement,"labelheight");
			// Get the width.
			int width = BaseControl.GetAttributeInt(configXmlElement,"labelwidth");
			// Set the ID if there is one.
			if (!StringUtils.IsStringEmpty(id))
				this.ID = id;
			// Set the text if there is one.
			if (!StringUtils.IsStringEmpty(text))
				this.Text = text;
			// Set the tooltip for the image if there is one.
			if (!StringUtils.IsStringEmpty(tip))
				this.ToolTip = tip;
			if (width > 0)
				this.Width = width;
			// Set the image height.
			if (height > 0)
				this.Height = height;
			// Add the hyperlink to the pages row.
			TableCell controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controllabel","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}
