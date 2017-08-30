#region Using
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.ControlLibrary.CSAScript;
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
	/// Generate a texbox.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.13" day="20" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Steve Lang</developer>
	///			<work_packet>WP/Seoul/052</work_packet>
	///			<description>Add translations for populating text boxes. This only
	///			makes sense to do when populating the textbox i.e. 0 translated
	///			to Female. When scraping the values from the screen, the actual
	///			contents entered will be used. This change is primarily being
	///			used to stop using listboxes to display readonly reference data.
	///			</description>
	///		</version>
	///		<version number="1.12" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added captionWidth element to config syntax</description>
	///		</version>
	/// 	<version number="1.11" day="22" month="01" year="2003">
	///			<developer>Bill Curtis</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet></work_packet>
	///			<description>
	///			TextBox gets any regularexpression using a name for localization lookup.
	///			</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.03" day="28" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>add DifferenceValidator</description>
	///		</version>
	/// 	<version number="1.02" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	/// 	<version number="1.01" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed CssClass names to be more consistent.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class TextBox : CSAGenericTextBox, ISimpleControl 
	{
		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.Enabled) {
				ht.Add(this.ID,this.Text);
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Construct a control containing a text box
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public TextBox(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) : base(parent,configXmlElement) {

			// Get configuration details.
			string id, dataValue, caption,tip, format;
			parent.GetSettings(configXmlElement,dataXmlElement,out dataValue,out id,out caption,out tip,out format);
			int captionWidth = BaseControl.GetAttributeInt(configXmlElement,"captionWidth");
			bool calculated = BaseControl.GetAttributeBool(configXmlElement,"calculated");
			// Set the error caption.
			string errorCaption;
			if (this.ErrorCaption == String.Empty) {
				errorCaption = caption;
			} else {
				errorCaption = this.ErrorCaption;
			}
			// Get any reference data that may be required. Only do this translation
			// if the box is in readonly.
			if (this.ReadOnly || this.Disabled)
			{
				XmlNode optionsNode = configXmlElement.SelectSingleNode("./options");
				if (optionsNode != null) 
				{
					// Obtain the reference data.
					ArrayList keyList;
					ArrayList textList;
					ArrayList orderList;	// has no effect on TextBoxes
					ArrayList ignoreList;	// has no effect on TextBoxes
					parent.GetOptionsData(optionsNode,out keyList, out textList, out orderList, out ignoreList);
					// Attempt to translate the current value. See if
					// the key exists and translate if it does.
					int index = keyList.IndexOf(dataValue);
					if (index != -1) {
						dataValue = (string)textList[index];
					}
				}
			}
			// Set the ID of the control.
			this.ID = id;
			// Format the data depending upon the datatype.
			if ((dataValue != null) && (dataValue.Length > 0)) 
			{
				if (format != null) {
					if (this.DataType == "date") {
						dataValue = DateTime.Parse(dataValue).ToString(format);
					} else if (this.DataType == "integer") {
						dataValue = Int32.Parse(dataValue).ToString(format);
					} else if (this.DataType == "currency") {
						dataValue = Decimal.Parse(dataValue).ToString(format);
					}
				}
				this.Text = dataValue;
			} 
			else 
			{
				this.Text = String.Empty;
			}
			// See if focus is required on this textbox.
			if (this.SetFocus)
				parent.SetFocus = CSAJavaScript.SetFocusOnControl(this.ID,parent.currentLocation);
			// Set the tooltip of the control.
			if(!tip.Equals(String.Empty))
				this.ToolTip = tip;
			// Add the validators.
			TableCell controlTableCell = new TableCell();
			if (!this.ReadOnly || calculated) 
			{
				SimpleControlConstruct.AddRequiredFieldValidator(controlTableCell,this,parent,errorCaption,this.ErrorMessage,this.Required);
				if (this.DifferenceWith.Length == 0 || this.MaxValue.Length > 0 || this.MinValue.Length > 0)
					// There is a range to check or there might be a datatype not checked by the difference validator.
					SimpleControlConstruct.AddRangeValidator(controlTableCell,this,parent,errorCaption,this.ErrorMessage,this.MaxValue,this.MinValue,this.DataType);
				SimpleControlConstruct.AddRegularExpressionValidator(controlTableCell,this,parent,errorCaption,this.ErrorMessage,this.Regularexpression);
				SimpleControlConstruct.AddDifferenceValidator(controlTableCell,this,parent,errorCaption,this.ErrorMessage,this.DifferenceWith,this.DifferenceOperator,this.DifferenceValue,this.DataType);
			}
			controlTableRow.Cells.Add(controlTableCell);
			// Add Control's Caption to Page's Table Row
			SimpleControlConstruct.AddCaption(controlTableRow,controlTableCell,caption,tip,this.Required,captionWidth);
			// Add Control to Page's Table Row
			controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controltextbox","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion
	}
}