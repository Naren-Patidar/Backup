using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml;
using Fujitsu.eCrm.Generic.LocalizationLibrary;

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate a drop down list.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>Bug fix</work_packet>
	///			<description>Add text place holder so the drop down list can be replaced by a text box</description>
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

	public class DropDownList : System.Web.UI.WebControls.DropDownList, ISimpleControl 
	{
		#region Declarations
		private string onChange = null;
		#endregion

		#region Properties
		internal System.Web.UI.WebControls.TextBox textBox;
		#endregion

		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.Enabled) {
				// The list is enabled
				if (this.SelectedItem != null) {
					// A list item is selected
					ht.Add(this.ID,this.SelectedItem.Value);
				}
			}
		}
		#endregion

		#region Add Attribute
		/// <summary> Adds an onchange attribute to the control
		/// </summary>
		/// <param name="writer">The Html Text writer used to render the control</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer) 
		{
			if (onChange != null && !onChange.Equals(String.Empty)) 
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Onchange, onChange);
			}
			base.AddAttributesToRender(writer);
		}
		#endregion		

		#region Constructor
		/// <summary>
		/// Returns a DropDownList control.
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public DropDownList(
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
			int size = BaseControl.GetAttributeInt(configXmlElement, "size");

			String errorCaption;
			string errorCaptionStr = BaseControl.GetAttributeString(configXmlElement, "errorCaption");
			if (errorCaptionStr==String.Empty)
				errorCaption = caption;
			else
				errorCaption = errorCaptionStr;
			string errorMessage = BaseControl.GetAttributeString(configXmlElement,"errormessage");

			onChange = BaseControl.GetAttributeString(configXmlElement, "onchange");

			// Setup Control according to configuration details
			if (rdOnly) {
				this.Enabled = false;
			}
			// setting Width property does not render correctly
			if (size>=0) {
				this.Style["width"]=size.ToString()+"px";
			}

			this.CssClass = "formdropdown";
			this.ID = id;

			if (listItems != null) {
				foreach (ListItem item in listItems) {
					this.Items.Add(item);

					// If the list item's value (or text if the value is not supplied)
					// matches the initialValue parameter, select that item.
					if ((item.Value == null) || (item.Value.Length == 0)) {
						if (item.Text == dataValue) {
							this.SelectedIndex = this.Items.Count-1;
						}
					} else if (item.Value == dataValue || item.Text == dataValue) {
						this.SelectedIndex = this.Items.Count-1;
					}
				}
			}

			// Add Control's Caption to Page's Table Row
			TableCell controlTableCell = new TableCell();
			if (required) {
				SimpleControlConstruct.AddRequiredFieldValidator(controlTableCell,this,parent,errorCaption,errorMessage,required);
				if (errorMessage == String.Empty) {
					errorMessage = String.Format(Localization.GetLocalizedAttributeString("ErrorMsg.MissingMandatoryValue"),caption);
				}
				foreach (string ignoreItem in ignoreList) {
					if (ignoreItem != String.Empty) {
						SimpleControlConstruct.AddDifferenceValidator(controlTableCell,this,parent,errorCaption,errorMessage,String.Empty,"not equals",ignoreItem,"string");
					}
				}
			}

			controlTableRow.Cells.Add(controlTableCell);
			// Add Control's Caption to Page's Table Row
			SimpleControlConstruct.AddCaption(controlTableRow,controlTableCell,caption,tip,required,captionWidth);
			if(!tip.Equals(String.Empty)) {
				this.ToolTip=tip;
			}

			// create a place holder in case we want to create a popup on this page
			// when we'll create a textbox to replace the drop down list

			textBox = new System.Web.UI.WebControls.TextBox();
			textBox.Enabled=true;
			textBox.Visible=false;
			textBox.ID = "replaceddl" + id;
			textBox.CssClass="formdropdown";
			if (size>=0) {
				textBox.Width = Unit.Pixel(size);
			}
	
			// wrap the control and its placeholder friend in a span	
			Control wrapper =  BaseControl.WrapControlInHtml(this,"controldropdownlist","simplecontrol" + id);
			wrapper.Controls.Add(textBox); 

			// Add Control to Page's Table Row
			controlTableCell = new TableCell();
			controlTableCell.Controls.Add(wrapper);
			controlTableRow.Cells.Add(controlTableCell);

			this.Visible = true;

			this.PreRender  += new System.EventHandler(this.MyPreRender);
		}
		#endregion

		#region Pre render
		/// <summary>
		/// convert  this drop down to a text box
		/// </summary>
		private void MyPreRender(object sender, System.EventArgs e)
		{
			if(!FindModalPopup(this.Page))
			{
				this.Visible=true;
				this.textBox.Visible = false;
			}
			else
			{
				this.Visible=false;
				this.textBox.Visible = true;
				if (this.SelectedIndex > 0 && this.SelectedIndex < this.Items.Count)
					this.textBox.Text = this.Items[this.SelectedIndex].Text;
			}
		}
		#endregion

		#region Find Modal Popup
		/// <summary>
		/// recursively find a visible modal popup box on this control
		/// </summary>
		/// <param name="owner"></param>
		/// <returns>whether a visible modal is on the page</returns>
		private bool FindModalPopup(Control owner)
		{
			if (owner != null && owner is PopupForm  && owner.Visible && !((PopupForm)owner).ShowAsDialogBox )
			{
				// Now need to determine whether the drop down list is part
				// of the popup hierarchy.
				DropDownList ddl = (DropDownList)SimpleControlConstruct.FindControlById(owner,this.ID);
				if (ddl == null)
					// It is, so return true to indicate that the drop down list doesn't
					// want to be converted to a textbox.
					return true;
				else
					return false;
			}
			else if (owner != null && owner.HasControls())
			{
				
				
				foreach (Control child in owner.Controls)
				{
					if (FindModalPopup(child))
					{
						 return true;
					};
				}
			}
			return false;
			
		}
		#endregion
	}
}