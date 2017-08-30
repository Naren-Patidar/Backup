#region Using
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl 
{

	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Create radio button controls.
	/// </summary>
	/// 
	/// <development> 
	/// 	<version number="1.12" day="02" month="03" year="2003">
	///			<checker>Stuart Forbes</checker>
	///			<developer>Lawrie Griffiths</developer>
	///			<description>Added repeatColumns element to config syntax</description>
	///		</version>
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added captionWidth element to config syntax</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.02" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	///		<version number="1.01" day="07" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>Bugzilla #76</work_packet>
	///			<description>Fixed bug with setting the correct focus.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList, ISimpleControl 
	{
		
		#region Declarations
		private string onChange = null;
		#endregion

		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.Enabled) {
				ht.Add(this.ID,this.SelectedItem.Value);
			}
		}
		#endregion
	
		#region Render
		/// <summary> Adds an onchange attribute to the control
		/// </summary>
		/// <param name="writer">The Html Text writer used to render the control</param>
		protected override void Render(HtmlTextWriter writer) 
		{
			if (onChange != null && onChange != String.Empty)
			{
				Attributes.Add("onclick", onChange);
			}
			base.Render(writer);
		}
		#endregion		

		#region Constructor
		/// <summary>
		/// Returns a Radio Button List
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public RadioButtonList(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) {

			// Get Configuration Details
			string id, dataValue, caption,tip,format;
			ListItemCollection listItems;
			ArrayList ignoreList;
			parent.GetSettings(configXmlElement,dataXmlElement, out dataValue, out id, out caption,out tip, out format, out listItems, out ignoreList);
			bool required = BaseControl.GetAttributeBool(configXmlElement, "required");
			bool rdOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			int captionWidth = BaseControl.GetAttributeInt(configXmlElement,"captionWidth");
			int repeatColumns = BaseControl.GetAttributeInt(configXmlElement, "repeatColumns");

			String errorCaption;
			string errorCaptionStr = BaseControl.GetAttributeString(configXmlElement, "errorCaption");
			if (errorCaptionStr==String.Empty)
				errorCaption = caption;
			else
				errorCaption = errorCaptionStr;
			string errorMessage = BaseControl.GetAttributeString(configXmlElement,"errormessage");

			onChange = BaseControl.GetAttributeString(configXmlElement, "onchange");

			// Setup Control according to configuration details
			int initialIndex = -1;
			if (rdOnly) {
				this.Enabled = false;
			}
			this.ID = id;
			this.CssClass = "formradiobutton";

			if (repeatColumns<=0)
				this.RepeatColumns = 3;
			else
				this.RepeatColumns = repeatColumns;

			this.RepeatDirection = RepeatDirection.Horizontal;
			this.RepeatLayout = RepeatLayout.Table;

			if (listItems != null) {
				foreach (ListItem item in listItems) {
					if (item.Value == dataValue) {
						initialIndex = this.Items.Count;
					}
					this.Items.Add(item);
				}
			}

			if (initialIndex >= 0) {
				this.SelectedIndex = initialIndex;
			}

			// Add Control's Caption to Page's Table Row
			TableCell controlTableCell = new TableCell();
			SimpleControlConstruct.AddRequiredFieldValidator(controlTableCell,this,parent,errorCaption,errorMessage,required);
			controlTableRow.Cells.Add(controlTableCell);
			// Add Control's Caption to Page's Table Row
			SimpleControlConstruct.AddCaption(controlTableRow,controlTableCell,caption,tip,required,captionWidth);
			if(!tip.Equals(String.Empty))
			{
				this.ToolTip = tip;
			}
			// Add Control to Page's Table Row
			controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlradiobuttonlist","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}