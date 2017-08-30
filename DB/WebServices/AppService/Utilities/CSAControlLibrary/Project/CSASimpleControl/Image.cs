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
	/// Generate an image.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class Image : System.Web.UI.WebControls.Image, ISimpleControl
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
		/// Create a new Image control.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="controlTableRow"></param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public Image(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) 
		{
			// Get the ID if there is one.
			string id = BaseControl.GetAttributeString(configXmlElement,"id");
			// Get an unlocalised url to use for when there isn't a id.
			string unlocalisedImageUrl = String.Empty;
			XmlNode node = configXmlElement.SelectSingleNode("imageurl/text()");
			if (node != null)
				unlocalisedImageUrl = node.Value;
			// Get the tooltip.
			string tip = BaseControl.GetAttributeString(configXmlElement,"tip");
			// Get the navigate URL.
			string navigateUrl = BaseControl.GetAttributeString(configXmlElement,"navigateurl");
			// Get the image URL.
			string imageUrl = BaseControl.GetAttributeString(configXmlElement,"imageurl");
			// Localise the image.
			imageUrl = Localization.GetLocalizedImageString(imageUrl);
			// Get the height.
			int height = BaseControl.GetAttributeInt(configXmlElement,"imageheight");
			// Get the width.
			int width = BaseControl.GetAttributeInt(configXmlElement,"imagewidth");
			// Set the ID if there is one.
			if (!StringUtils.IsStringEmpty(id))
				this.ID = id;
			else
				this.ID = StringUtils.RemoveStrings(unlocalisedImageUrl,"@",".","*",":","[","]");
			// Set the tooltip for the image if there is one.
			if (!StringUtils.IsStringEmpty(tip))
				this.ToolTip = tip;
			// Set the image URL.
			this.ImageUrl = imageUrl;
			// Set the image width.
			if (width > 0)
				this.Width = width;
			// Set the image height.
			if (height > 0)
				this.Height = height;
			// Create a hyperlink to wrap the image. This enables the image to
			// be clicked to navigate to a different page.
			Control newControl = this;
			if (!StringUtils.IsStringEmpty(navigateUrl))
			{
				System.Web.UI.WebControls.HyperLink hyper = new System.Web.UI.WebControls.HyperLink();
				hyper.NavigateUrl = navigateUrl;
				hyper.ID = "hyper" + StringUtils.RemoveStrings(unlocalisedImageUrl,"@",".","*",":","[","]");
				// Add the image to the hyperlink.
				hyper.Controls.Add(this);
				newControl = hyper;
			}
			// Add the hyperlink to the pages row.
			TableCell controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(newControl,"controlimage","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}
