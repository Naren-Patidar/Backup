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
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <summary>
	/// Generate a checkbox.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/052</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class CheckBox : System.Web.UI.WebControls.CheckBox, ISimpleControl 
	{
		
		#region Declarations
		private string onChange = null;
		#endregion

		#region Checked & Unchecked Values
		private string checkedValue;
		private string unCheckedValue;
		#endregion

		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.Enabled) {
				if (this.Checked)
					ht.Add(this.ID,this.checkedValue);
				else
					ht.Add(this.ID,this.unCheckedValue);
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
		/// Construct a Check Box List
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public CheckBox (
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) {

			// Get configuration details
			string id, dataValue, caption,tip,format;
			parent.GetSettings(configXmlElement,dataXmlElement,out dataValue,out id,out caption,out tip ,out format);
			bool readOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			int captionWidth = BaseControl.GetAttributeInt(configXmlElement,"captionWidth");
			int height = BaseControl.GetAttributeInt(configXmlElement,"checkboxheight");
			int width = BaseControl.GetAttributeInt(configXmlElement,"checkboxwidth");
			// Get the value that indicates the checkbox is checked.
			this.checkedValue = 
				BaseControl.GetAttributeString(configXmlElement,"checked");
			// Get the value that indicates the checkbox is checked.
			this.unCheckedValue = 
				BaseControl.GetAttributeString(configXmlElement,"unchecked");
			// Ensure that there are values for checked and unchecked.
			if (StringUtils.IsStringEmpty(this.checkedValue))
			{
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "CSAControlLibrary.MissingElement","checked","checkbox");
				throw ce;
			}
			if (StringUtils.IsStringEmpty(this.unCheckedValue))
			{
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "CSAControlLibrary.MissingElement","unchecked","checkbox");
				throw ce;
			}

			onChange = BaseControl.GetAttributeString(configXmlElement, "onchange");

			// Set the tooltip.
			this.ToolTip = tip;
			// Setup control according to configuration details
			if (readOnly)
				this.Enabled = false;
			else
				this.Enabled = true;
			// Set the ID.
			this.ID = id;
			// Set the class.
			this.CssClass = "formcheckbox";
			// Check if this box is ticked or not.
			if (dataValue == checkedValue)
				this.Checked = true;
			else
				this.Checked = false;
			// Set the width if there is one.
			if (width > 0)
				this.Width = width;
			// Set the height if there is one.
			if (height > 0)
				this.Height = height;
			// Add Control's Caption to Page's Table Row
			TableCell controlTableCell = new TableCell();
			// Add Control's Caption to Page's Table Row
			SimpleControlConstruct.AddCaption(controlTableRow,controlTableCell,caption,tip,false,captionWidth);
			if(!tip.Equals(String.Empty))
			{
				this.ToolTip = tip;
			}
			controlTableRow.Cells.Add(controlTableCell);
			// Add Control to Page's Table Row
			controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlcheckbox","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}