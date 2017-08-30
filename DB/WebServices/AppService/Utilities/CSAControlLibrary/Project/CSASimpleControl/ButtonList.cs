#region Using
using System;
using System.Collections;
using System.Xml;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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
	/// Generate a list of buttons.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>RS2003</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion
	
	public class ButtonList : PlaceHolder, ISimpleControl
	{
		#region Constants
		private const string newLine = "\n";
		private const string btnSelectedClass = "button buttonlistbuttonselected";
		private const string btnClass = "button buttonlistbutton";
		private const string btnPrefixId = "buttonlistbutton";
		private const string hiddenControlId = "buttonlisthtmlinputhidden";
		#endregion

		#region Properties
		private bool enabled;
		/// <summary>
		/// Set the control to be read-only.
		/// </summary>
		public bool Enabled
		{
			set { this.enabled = value; }
			get { return this.enabled; }
		}
		#endregion

		#region Get Control Value
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{
			if (this.enabled) 
			{
				// Need to locate the hidden control which is storing
				// the selected value.
				HtmlInputHidden hidden = (HtmlInputHidden)SimpleControlConstruct.FindControlById(this.Page,hiddenControlId);
				// See if the hidden field has been found.
				if (hidden == null)
					// The hidden field hasn't been found. This means that the control
					// was never created in the first place i.e. the list of items to
					// generate the control was empty. Don't need to add anything to
					// the hashtable, so just return.
					return;
				else
					// The hidden field was found, so obtain the value selected and add
					// to the hashtable.
					ht.Add(this.ID,hidden.Value);
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Create a ButtonList.
		/// </summary>
		/// <param name="parent">The Dataform containing this control</param>
		/// <param name="controlTableRow">The table row containing this control</param>
		/// <param name="configXmlElement">The XML Node in the configuration file describing this control's layout</param>
		/// <param name="dataXmlElement">The XML Node from the CRM Server describing this control's data</param>
		public ButtonList(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement
			)
		{
			// Get configuration details
			string id, dataValue, caption,tip,format;
			ListItemCollection listItems;
			ArrayList ignoreList;
			parent.GetSettings(configXmlElement,dataXmlElement,out dataValue,
				out id,out caption,out tip,out format,out listItems, out ignoreList);
			bool readOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			int buttonWidth = BaseControl.GetAttributeInt(configXmlElement, "buttonwidth");
			int buttonHeight = BaseControl.GetAttributeInt(configXmlElement, "buttonheight");
			// Set the ID of the control.
			this.ID = id;
			// Check if the control is read-only.
			if (readOnly)
				this.enabled = false;
			else
				this.enabled = true;
			// Iterate over the list of items and create a new button for each.
			if (listItems != null) 
			{
				// Add the hidden field to store the selected value.
				HtmlInputHidden hidden = new HtmlInputHidden();
				hidden.ID = hiddenControlId;
				// Set the control to the data value the xpath currently has.
				if (!StringUtils.IsStringEmpty(dataValue))
					hidden.Value = dataValue;
				this.Controls.Add(hidden);
				// Also add the JavaScript which processes a button click event.
				HtmlGenericControl script =  new HtmlGenericControl("script");
				string javaScript = newLine +
					"function ButtonListOnClick(clickValue)" + newLine +
					"{" + newLine +
						// Set-up the regular expression to locate the hidden control.
						"var regexHidden = /^.*" + hiddenControlId + "$/;" + newLine +
						// Set-up the regular expression to locate the buttons.
						"var regexButton = /^.*" + btnPrefixId + ".*$/;" + newLine +
						"var i = -1;" + newLine +
						// Loop round until the hidden control is found and all buttons
						// have there class set to not selected.
						"while(true)" + newLine +
						"{" + newLine +
							"i++;" + newLine +
							// If the ID is null, then all elements have been searched.
							"if (document.forms[0].elements.length == i)" + newLine +
								"break;" + newLine +
							// Get the ID of the current control.
							"var id = document.forms[0].elements[i].id;" + newLine +
							// Attempt to match with the required hidden name.
							"var result = id.match(regexHidden);" + newLine +
							"if (result != null)" + newLine +
							"{" + newLine +
								// Set the hidden control to the clicked value.
								"document.forms[0].elements[i].value = clickValue;" + newLine +
								"continue;" + newLine +
							"}" + newLine +
							// Attempt to match with the button name.
							"result = id.match(regexButton);" + newLine +
							"if (result != null)" + newLine +
							"{" + newLine +
								// Set the button class name to not selected.
								"document.forms[0].elements[i].className = '" + btnClass + "';" + newLine +
								"continue;" + newLine +
							"}" + newLine +
						"}" + newLine +
						// Set the clicked button class to enable some styling to
						// be performed.
						"event.srcElement.className = '" + btnSelectedClass + "';" + newLine +
						"return false;" + newLine +
					"}";
				script.InnerText = javaScript;
				this.Controls.Add(script);
				// Create a table to hold the list.
				Table mainTable = new Table();
				mainTable.ID = "buttonlistmaintable";
				mainTable.CssClass = "buttonlistmaintable";
				// Create a row in the main table.
				TableRow mainRow = new TableRow();
				mainRow.ID = "buttonlistmaintablerow";
				mainRow.CssClass = "buttonlistmaintablerow";
				int i = 1;
				foreach (ListItem item in listItems) 
				{
					// If the list item's value (or text if the value is not supplied)
					// matches the initialValue parameter, select that item.
					bool isSelected = false;
					if ((item.Value == null) || (item.Value.Length == 0)) 
					{
						if (item.Text == dataValue) 
						{
							// Set this button to be highlighted.
							isSelected = true;
						}
					} 
					else if (item.Value == dataValue || item.Text == dataValue) 
					{
						isSelected = true;
					}
					// Add a button to the current list.
					mainRow.Cells.Add(this.CreateButtonCell(i.ToString(),isSelected,buttonWidth,buttonHeight
						,item.Text,item.Value));
					// Increment the counter.
					i++;
				}
				mainTable.Rows.Add(mainRow);
				this.Controls.Add(mainTable);
			}
			// Add the hyperlink to the pages row.
			TableCell controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlbuttonlist","simplecontrol" + id));
			controlTableRow.Cells.Add(controlTableCell);
		}
		#endregion

		#region Create Button Cell
		private TableCell CreateButtonCell(
			string idSuffix,
			bool isSelected,
			int buttonWidth,
			int buttonHeight,
			string buttonText,
			string buttonValue
			)
		{
			TableCell outerCell = new TableCell();
			outerCell.ID = "buttonlistmaintablecell" + idSuffix;
			outerCell.CssClass = "buttonlistmaintablecell buttonlistmaintablecell" + idSuffix;
			Table table = new Table();
			table.ID = "buttonlistsubtable" + idSuffix;
			table.CssClass = "buttonlistsubtable buttonlistsubtable" + idSuffix;
			TableRow tableRow = new TableRow();
			tableRow.ID = "buttonlistsubtablerow" + idSuffix;
			tableRow.CssClass = "buttonlistsubtablerow buttonlistsubtablerow" + idSuffix;
			TableCell innerCell = new TableCell();
			innerCell.ID = "buttonlistsubtablecell" + idSuffix;
			innerCell.CssClass = "buttonlistsubtablecell buttonlistsubtablecell" + idSuffix;
			// Create the new button.
			Button button = new Button();
			button.Attributes.Add("onclick","return ButtonListOnClick('" + buttonValue + "')");
			button.ID = btnPrefixId + idSuffix;
			button.Text = buttonText;
			// Check if the control has been specified as read-only.
			button.Enabled = this.enabled;
			if (isSelected)
				button.CssClass = btnSelectedClass;
			else
				button.CssClass = btnClass;
			if (buttonHeight > 0)
				button.Height = buttonHeight;
			if (buttonWidth > 0)
				button.Width = buttonWidth;
			innerCell.Controls.Add(button);
			tableRow.Cells.Add(innerCell);
			table.Rows.Add(tableRow);
			outerCell.Controls.Add(table);
			return outerCell;
		}
		#endregion
	}
}
