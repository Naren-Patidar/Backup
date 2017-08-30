#region Using
using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl 
{

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate a check box list.
	/// </summary>
	/// 
	/// <development> 
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
	/// 	<version number="1.03" day="20" month="11" year="2002">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Add styles</description>
	///		</version>
	/// 	<version number="1.02" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	/// </development>
	#endregion

	public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, ISimpleControl 
	{
		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.Enabled) {
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Construct a Check Box List
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public CheckBoxList (
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) {

			// Get Configuration Details
			string id, dataValue, caption,tip,format;
			ListItemCollection listItems;
			ArrayList ignoreList;
			parent.GetSettings(configXmlElement,dataXmlElement, out dataValue, out id, out caption,out tip ,out format, out listItems, out ignoreList);
			bool required = BaseControl.GetAttributeBool(configXmlElement, "required");
			bool rdOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			int captionWidth = BaseControl.GetAttributeInt(configXmlElement,"captionWidth");

			String errorCaption;
			string errorCaptionStr = BaseControl.GetAttributeString(configXmlElement, "errorCaption");
			if (errorCaptionStr==String.Empty)
				errorCaption = caption;
			else
				errorCaption = errorCaptionStr;
			string errorMessage = BaseControl.GetAttributeString(configXmlElement,"errormessage");

			// Setup Control according to configuration details
			if (rdOnly) {
				this.Enabled = false;
			}			

			this.ID = id;
			this.CssClass = "formcheckbox";
			this.RepeatColumns = 3;
			this.RepeatDirection = RepeatDirection.Horizontal;
			this.RepeatLayout = RepeatLayout.Table;

			if (listItems != null) {
				foreach (ListItem item in listItems) {
					this.Items.Add(item);
				}
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
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlcheckbox","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}