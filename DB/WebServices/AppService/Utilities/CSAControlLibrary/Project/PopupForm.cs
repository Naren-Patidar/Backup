#region Using
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.Caching;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using System.Web.UI.HtmlControls;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary 
{

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate Popup form from config files.
	/// </summary>
	/// 
	/// <development>
	/// 	<version number="1.11" day="19" month="03" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Popup forms are now modal</description>
	///		</version>
	/// 	<version number="1.11" day="18" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Add a new Show permutation of ShowForMessageCancel.</description>
	///		</version>
	/// 	<version number="1.11" day="18" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Enable setting of the button text to be specified
	///			via properties.</description>
	///		</version>
	/// 	<version number="1.10" day="18" month="02" year="2003">
	///			<checker>Stuart Forbes</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added grid syntax to data form</description>
	///		</version>
	///		<version number="1.09" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added grid syntax to data form</description>
	///		</version>
	///		<version number="1.09" day="04" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added functionality for a standard popup message
	///			with an ok button.</description>
	///		</version>
	///		<version number="1.08" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	///  	<version number="1.07" day="21" month="11" year="2002">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Add button style to all pop-up buttons</description>
	///		</version>
	/// 	<version number="1.06" day="20" month="11" year="2002">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Add styles</description>
	///		</version>
	/// 	<version number="1.05" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed CssClass requiredforminput to forminputrequired
	///			to be more consistent with naming convention.</description>
	///		</version>
	/// 	<version number="1.04" day="18" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Tidy</work_packet>
	///			<description>Moved header to correct space to prevent warning messages.</description>
	///		</version>
	/// 	<version number="1.03" day="17" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/barcelona/015</work_packet>
	///			<description>
	///			PopupForm.CreateChildControls() methods use the ConfigDoc property 
	///			instead of handling the ConfigFile directly.
	///			</description>
	///		</version>
	/// 	<version number="1.02" day="18" month="04" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Changed namespace to Fujitsu.eCrm.CSAControlLibrary.</description>
	///		</version>
	///		<version number="1.01" day="06" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Internationalisation and standardisation of error handling.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	///
	#endregion

	public class PopupForm : ControlTable 
	{
		#region Properties
		/// <summary>
		/// Gets or sets whether this control is rendered as UI, 
		/// and if so masks out the rest of the fields on the page to make the popup modal
		/// </summary>
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set 
			{
				base.Visible = value;
				this.ModalPopup(value);	
				
			}

		}

		/// <summary>
		/// Messages may be shown as modal dialog boxes instead of absolutely positioned divs
		/// </summary>
		internal bool ShowAsDialogBox 
		{
			get 
			{
				Object o = ViewState["showAsDialogBox"];
				if (o==null) 
				{
					return false;
				} 
				else 
				{
					return (bool)o;
				}
			}
			set { ViewState["showAsDialogBox"] = value; }
		}

		private bool ShowCancel 
		{
			get 
			{
				Object o = ViewState["showCancel"];
				if (o==null) 
				{
					return true;
				} 
				else 
				{
					return (bool)o;
				}
			}
			set { ViewState["showCancel"] = value; }
		}

		private bool ShowInsert 
		{
			get {
				Object o = ViewState["showInsert"];
				if (o==null) {
					return false;
				} else {
					return (bool)o;
				}
			}
			set { ViewState["showInsert"] = value; }
		}

		private bool ShowUpdate {
			get {
				Object o = ViewState["showUpdate"];
				if (o==null) {
					return false;
				} else {
					return (bool)o;
				}
			}
			set { ViewState["showUpdate"] = value; }
		}

		private bool ShowMessage 
		{
			get 
			{
				Object o = ViewState["showMessage"];
				if (o==null) 
				{
					return false;
				} 
				else 
				{
					return (bool)o;
				}
			}
			set { ViewState["showMessage"] = value; }
		}

		private bool ShowDelete {
			get {
				Object o = ViewState["showDelete"];
				if (o==null) {
					return false;
				} else {
					return (bool)o;
				}
			}
			set { ViewState["showDelete"] = value; }
		}

		private string showAsDialogRedirectUrl;
		/// <summary>
		/// Manually set the text to be displayed on the OK button
		/// of a popup.
		/// </summary>
		public string ShowAsDialogRedirectUrl
		{
			get { return this.showAsDialogRedirectUrl; }
			set { this.showAsDialogRedirectUrl = value; }
		}

		private string buttonTextOk;
		/// <summary>
		/// Manually set the text to be displayed on the OK button
		/// of a popup.
		/// </summary>
		public string ButtonTextOk
		{
			get { return this.buttonTextOk; }
			set { this.buttonTextOk = value; }
		}
		private string buttonTextClose;
		/// <summary>
		/// Manually set the text to be displayed on the close button
		/// of a popup.
		/// </summary>
		public string ButtonTextClose
		{
			get { return this.buttonTextClose; }
			set { this.buttonTextClose = value; }
		}
		private string buttonTextInsert;
		/// <summary>
		/// Manually set the text to be displayed on the insert button
		/// of a popup.
		/// </summary>
		public string ButtonTextInsert
		{
			get { return this.buttonTextInsert; }
			set { this.buttonTextInsert = value; }
		}
		private string buttonToolTipInsert;
		/// <summary>
		/// Manually set the tooltip to be displayed on the insert button
		/// of a popup.
		/// </summary>
		public string ButtonToolTipInsert
		{
			get { return this.buttonToolTipInsert; }
			set { this.buttonToolTipInsert = value; }
		}

		private string buttonTextUpdate;
		/// <summary>
		/// Manually set the text to be displayed on the update button
		/// of a popup.
		/// </summary>
		public string ButtonTextUpdate
		{
			get { return this.buttonTextUpdate; }
			set { this.buttonTextUpdate = value; }
		}
		private string buttonToolTipUpdate;
		/// <summary>
		/// Manually set the tooltip to be displayed on the update button
		/// of a popup.
		/// </summary>
		public string ButtonToolTipUpdate
		{
			get { return this.buttonToolTipUpdate; }
			set { this.buttonToolTipUpdate = value; }
		}

		
		
		private string buttonTextDelete;
		/// <summary>
		/// Manually set the text to be displayed on the delete button
		/// of a popup.
		/// </summary>
		public string ButtonTextDelete
		{
			get { return this.buttonTextDelete; }
			set { this.buttonTextDelete = value; }
		}
		private string buttonToolTipDelete;
		/// <summary>
		/// Manually set the tooltip to be displayed on the delete button
		/// of a popup.
		/// </summary>
		public string ButtonToolTipDelete
		{
			get { return this.buttonToolTipDelete; }
			set { this.buttonToolTipDelete = value; }
		}

		private string buttonTextCancel;
		/// <summary>
		/// Manually set the text to be displayed on the cancel button
		/// of a popup.
		/// </summary>
		public string ButtonTextCancel
		{
			get { return this.buttonTextCancel; }
			set { this.buttonTextCancel = value; }
		}
		private string buttonToolTipCancel;
		/// <summary>
		/// Manually set the tooltip to be displayed on the cancel button
		/// of a popup.
		/// </summary>
		public string ButtonToolTipCancel
		{
			get { return this.buttonToolTipCancel; }
			set { this.buttonToolTipCancel = value; }
		}

		private bool buttonDisabledOk;
		/// <summary>
		/// Disable the OK button on the popup.
		/// </summary>
		public bool ButtonDisabledOk
		{
			get { return this.buttonDisabledOk; }
			set { this.buttonDisabledOk = value; }
		}
		private string buttonToolTipOk;
		/// <summary>
		/// Manually set the tooltip to be displayed on the OK button
		/// of a popup.
		/// </summary>
		public string ButtonToolTipOk
		{
			get { return this.buttonToolTipOk; }
			set { this.buttonToolTipOk = value; }
		}

		private bool buttonDisabledClose;
		/// <summary>
		/// Disable the Close button on the popup.
		/// </summary>
		public bool ButtonDisabledClose
		{
			get { return this.buttonDisabledClose; }
			set { this.buttonDisabledClose = value; }
		}
		private bool buttonDisabledInsert;
		/// <summary>
		/// Disable the Insert button on the popup.
		/// </summary>
		public bool ButtonDisabledInsert
		{
			get { return this.buttonDisabledInsert; }
			set { this.buttonDisabledInsert = value; }
		}
		private bool buttonDisabledUpdate;
		/// <summary>
		/// Disable the Update button on the popup.
		/// </summary>
		public bool ButtonDisabledUpdate
		{
			get { return this.buttonDisabledUpdate; }
			set { this.buttonDisabledUpdate = value; }
		}
		private bool buttonDisabledDelete;
		/// <summary>
		/// Disable the Delete button on the popup.
		/// </summary>
		public bool ButtonDisabledDelete
		{
			get { return this.buttonDisabledDelete; }
			set { this.buttonDisabledDelete = value; }
		}
		private bool buttonDisabledCancel;
		/// <summary>
		/// Disable the Cancel button on the popup.
		/// </summary>
		public bool ButtonDisabledCancel
		{
			get { return this.buttonDisabledCancel; }
			set { this.buttonDisabledCancel = value; }
		}
		private string popupHeaderText;
		/// <summary>
		/// Manually set the text to be displayed on the cancel button
		/// of a popup.
		/// </summary>
		public string PopupHeaderText
		{
			get { return this.popupHeaderText; }
			set { this.popupHeaderText = value; }
		}
		#endregion

		#region Event Handlers
		/// <summary></summary>
		public event EventHandler OkClick;
		/// <summary></summary>
		public event EventHandler CloseClick;
		/// <summary></summary>
		public event EventHandler InsertClick;
		/// <summary></summary>
		public event EventHandler UpdateClick;
		/// <summary></summary>
		public event EventHandler DeleteClick;
		/// <summary></summary>
		public event EventHandler CancelClick;

		private void OwnCloseClick(object sender, System.EventArgs e) 
		{
			this.Visible = false;
			// Propogate the event to the web page.
			if (CloseClick != null) 
			{
				CloseClick(this,e);
			}
		}

		private void OwnOkClick(object sender, System.EventArgs e) 
		{
			this.Visible = false;

			// Propogate the event to the web page.
			if (OkClick != null) 
			{
				OkClick(this,e);
			}
		}

		private void OwnInsertClick(object sender, System.EventArgs e) {
			this.Visible = false;
			// Propogate the event to the web page.
			if (InsertClick != null) 
			{
				InsertClick(this,e);
			}
		}

		private void OwnUpdateClick(object sender, System.EventArgs e) {
			this.Visible = false;
			// Propogate the event to the web page.
			if (UpdateClick != null) 
			{ 
				UpdateClick(this,e);
			}
		}

		private void OwnDeleteClick(object sender, System.EventArgs e) {
			this.Visible = false;
			// Propogate the event to the web page.
			if (DeleteClick != null) 
			{
				DeleteClick(this,e);
			}
		}

		private void OwnCancelClick(object sender, System.EventArgs e) 
		{
			this.Visible = false;
			// Propogate the cancel event to the web page.
			if (CancelClick != null) 
			{
				CancelClick(this,e);
			}
		}
		private Table formTable;
		#endregion

		#region HTML
		
		/// <summary>
		/// Creates a table containing the main Popup table.
		///
		/// This comprises of a Header Row, a section
		/// containing the Form Table and a set of buttons
		/// at the bottom.
		/// </summary>
		private Table CreatePopupTable() 
		{

			ITraceState trState = Trace.StartProc("PopupForm.CreatePopupTable");
			XmlDocument xmlDoc = this.ConfigXmlDoc;

			//this.LinkPage = Page;

			// Create the outer table (This just produces a gray border)
			//<table align="center" border=1 bordercolor=#efefef cellSpacing=0 cellPadding=0>
			Table outerTable = new Table();
			outerTable.BorderWidth=2;
			outerTable.HorizontalAlign = HorizontalAlign.Center;

			// Create the main table 
			//	<table align="center" class="popup" border="0" width="450"
			//    cellspacing="2" cellpadding="4">
			Table mainTable = new Table();
			mainTable.CssClass = "popup";
			mainTable.BorderWidth=0;
			//mainTable.Width = Unit.Pixel(800);

			// insert the main table into the outer table
			TableRow outerRow = new TableRow();
			TableCell outerCell = new TableCell();
			outerCell.Controls.Add(mainTable);
			outerRow.Cells.Add(outerCell);
			outerTable.Rows.Add(outerRow);

			// Display the Form Header (if any). Check if supplied via
			// the property first.
			string headerCaption = String.Empty;
			XmlNodeList elemList = null;
			if (StringUtils.IsStringEmpty(this.popupHeaderText))
			{
				headerCaption =  BaseControl.GetAttributeString(xmlDoc.DocumentElement,"header");
			} 
			else
			{
				headerCaption = Localization.GetLocalizedAttributeString(this.popupHeaderText);
			}
			Trace.WriteDebug("Form Header = " + headerCaption);
			// Need to construct a header with events to enable
			// the dragging of the popup and closure. Do this
			// with a button. Create a new tablerow.
			Table headerTable = new Table();
			headerTable.CssClass = "popupheadertable";
			headerTable.Width = Unit.Percentage(100);
			TableRow headerRow = new TableRow();
			headerRow.CssClass = "popupheaderrow";
			TableCell headerCell = new TableCell();
			// Add the caption to the cell.
			headerCell.Text =headerCaption;
			// Make the table draggable when clicked on this cell
			headerCell.Attributes.Add("onmousedown","onDragMouseDown()");
			headerCell.Attributes.Add("unselectable","on");
			headerCell.CssClass = "popupheadercaptioncell";
			// Add the cell to the row.
			headerRow.Cells.Add(headerCell);
			// Create a new header cell.
			headerCell = new TableCell();
			// Create a new button to act as the close button.
			Button headerCloseButton = new Button();
			headerCloseButton.Style.Add("background-image","url(images/popupclose.bmp)");
			headerCloseButton.Click += new System.EventHandler(OwnCloseClick);
			headerCloseButton.CausesValidation = false;
			headerCloseButton.CssClass = "popupclosebutton";
			headerCloseButton.ID = "closebutton";
			// Make the button a square.
			headerCloseButton.Width = new Unit(16,UnitType.Pixel);
			headerCloseButton.Height = new Unit(14,UnitType.Pixel);
			// Add the button to the header cell.
			headerCell.Controls.Add(headerCloseButton);
			headerCell.HorizontalAlign = HorizontalAlign.Right;
			// Add this cell to the row.
			headerRow.Cells.Add(headerCell);
			// Add the row to the table.
			headerTable.Rows.Add(headerRow);

			// Add the headerTable to the mainTable
			TableCell mainTableCell = new TableCell();
			mainTableCell.Controls.Add(headerTable);
			TableRow mainTableRow = new TableRow();
			mainTableRow.Cells.Add(mainTableCell);
			mainTable.Rows.Add(mainTableRow);

			// Create the formTable to hold the form's columns
			 formTable = new Table();
			formTable.Width = Unit.Percentage(100);
			formTable.GridLines = GridLines.None;
			formTable.CssClass= "popuptable";

			TableRow formTableRow = new TableRow();
			formTableRow.VerticalAlign = VerticalAlign.Top;
			formTable.Rows.Add(formTableRow);

			if (xmlDoc.SelectSingleNode("//grid") == null)
			{
				// Create a new table for each column within the form and
				// populate it
			
				elemList = xmlDoc.GetElementsByTagName("column");

				foreach (XmlElement elem in elemList) 
				{
					Table columnTable = this.CreateControlTable(elem, this.DataXmlElement);		
					TableCell formTableCell = new TableCell();
					formTableCell.Controls.Add(columnTable);
					formTableRow.Cells.Add(formTableCell);
				}
			}
			else
			{
				Control grid = this.CreateGrid(xmlDoc.DocumentElement ,false);
				TableCell formTableCell = new TableCell();
				formTableCell.Controls.Add(grid);
				formTableRow.Cells.Add(formTableCell);
			}

			// Add the FormTable to the bottom of the MainTable
			mainTableCell = new TableCell();
			mainTableCell.Controls.Add(formTable);
			mainTableRow = new TableRow();
			mainTableRow.Cells.Add(mainTableCell);
			mainTable.Rows.Add(mainTableRow);

			// See if the pop-up is for showing a message. If it is, then display it
			if (this.ShowMessage)
			{
				// Create the table, row and cell.
				Table messageTable = new Table();
				messageTable.CssClass = "popupmessagetable";
				TableRow messageTableRow = new TableRow();
				messageTableRow.CssClass = "popupmessagerow";
				TableCell messageTableCell = new TableCell();
				messageTableCell.CssClass = "popupmessagecell";
				// Assign the text to the cell.
				messageTableCell.Text = this.showForMessageText;
				// Add the cell to the row.
				messageTableRow.Cells.Add(messageTableCell);
				// Add the row to the table.
				messageTable.Rows.Add(messageTableRow);
				// Add the table to the main table.
				mainTableRow = new TableRow();
				mainTableCell = new TableCell();
				mainTableCell.Controls.Add(messageTable);
				mainTableRow.Cells.Add(mainTableCell);
				mainTable.Rows.Add(mainTableRow);
			}

			// Add the buttons as a new table at the bottom of the main table.
			Table buttonTable = new Table();
			buttonTable.Width = Unit.Percentage(100);
			TableRow buttonRow = new TableRow();
			buttonRow.HorizontalAlign = HorizontalAlign.Center;
			buttonTable.CssClass = "popupbuttons";

			if (this.ShowInsert) {
				Button butt = new Button();
				// Check if the button text has been supplied manually through
				// a property.
				string textToUse;
				string toolTipToUse;
				if (this.ButtonTextInsert != null)
					textToUse = this.ButtonTextInsert;
				else
					textToUse = "Button.Insert";

				if (this.ButtonToolTipInsert != null)
					toolTipToUse = this.ButtonToolTipInsert;
				else
					toolTipToUse = "Button.Insert.ToolTip";

				butt.Text = Localization.GetLocalizedAttributeString(textToUse);
				butt.ToolTip = Localization.GetLocalizedAttributeString(toolTipToUse);

				butt.Click += new System.EventHandler(OwnInsertClick);
				butt.CssClass="button";
				butt.ID = "insertbutton";
				// Check if the button should be disabled.
				if (this.buttonDisabledInsert)
					butt.Enabled = false;
				TableCell cell = new TableCell();
				cell.Controls.Add(butt);
				buttonRow.Cells.Add(cell);				
			}

			if (this.ShowUpdate) {
				Button butt = new Button();
				// Check if the button text has been supplied manually through
				// a property.
				string textToUse;
				string toolTipToUse;

				if (this.ButtonTextUpdate != null)
					textToUse = this.ButtonTextUpdate;
				else
					textToUse = "Button.Update";

				if (this.ButtonToolTipUpdate != null)
					toolTipToUse = this.ButtonToolTipUpdate;
				else
					toolTipToUse = "Button.Update.ToolTip";

				
				butt.Text = Localization.GetLocalizedAttributeString(textToUse);
				butt.ToolTip = Localization.GetLocalizedAttributeString(toolTipToUse);

				butt.Click += new System.EventHandler(OwnUpdateClick);
				butt.CssClass="button";
				butt.ID = "updatebutton";
				// Check if the button should be disabled.
				if (this.buttonDisabledUpdate)
					butt.Enabled = false;
				TableCell cell = new TableCell();
				cell.Controls.Add(butt);
				buttonRow.Cells.Add(cell);	
			}

			if (this.ShowDelete) {
				Button butt = new Button();
				// Check if the button text has been supplied manually through
				// a property.
				string textToUse;
				string toolTipToUse;
				if (this.ButtonTextDelete != null)
					textToUse = this.ButtonTextDelete;
				else
					textToUse = "Button.Delete";

				if (this.ButtonToolTipDelete != null)
					toolTipToUse = this.ButtonToolTipDelete;
				else
					toolTipToUse = "Button.Delete.ToolTip";

				butt.Text = Localization.GetLocalizedAttributeString(textToUse);
				butt.ToolTip = Localization.GetLocalizedAttributeString(toolTipToUse);
				butt.Click += new System.EventHandler(OwnDeleteClick);
				butt.CausesValidation = false;
				butt.CssClass="button";
				butt.ID = "deletebutton";
				// Check if the button should be disabled.
				if (this.buttonDisabledDelete)
					butt.Enabled = false;
				TableCell cell = new TableCell();
				cell.Controls.Add(butt);
				buttonRow.Cells.Add(cell);				
			}

			if (this.ShowMessage) 
			{
				Button butt = new Button();
				// Check if the button text has been supplied manually through
				// a property.
				string textToUse;
				string toolTipToUse;

				if (this.ButtonTextOk != null)
					textToUse = this.ButtonTextOk;
				else
					textToUse = "Button.Ok";

				if (this.ButtonToolTipOk != null)
					toolTipToUse = this.ButtonToolTipOk;
				else
					toolTipToUse = "Button.Ok.ToolTip";

				
				
				butt.Text = Localization.GetLocalizedAttributeString(textToUse);
				butt.ToolTip = Localization.GetLocalizedAttributeString(toolTipToUse);
				butt.Click += new System.EventHandler(OwnOkClick);
				butt.CausesValidation = false;
				butt.CssClass="button";
				butt.ID = "okbutton";
				// Check if the button should be disabled.
				if (this.buttonDisabledOk)
					butt.Enabled = false;
				TableCell cell = new TableCell();
				cell.Controls.Add(butt);
				buttonRow.Cells.Add(cell);				
			}

			if (this.ShowCancel) {
				Button butt = new Button();
				// Check if the button text has been supplied manually through
				// a property.
				string textToUse;
				string toolTipToUse;
				if (this.ButtonTextCancel != null)
					textToUse = this.ButtonTextCancel;
				else
					textToUse = "Button.Cancel";
				
				if (this.ButtonToolTipCancel != null)
					toolTipToUse = this.ButtonToolTipCancel;
				else
					toolTipToUse = "Button.Cancel.ToolTip";
				

				butt.Text = Localization.GetLocalizedAttributeString(textToUse);
				butt.ToolTip = Localization.GetLocalizedAttributeString(toolTipToUse);
				butt.Click += new System.EventHandler(OwnCancelClick);
				butt.CausesValidation = false;
				butt.CssClass="button";
				butt.ID = "cancelbutton";
				// Check if the button should be disabled.
				if (this.buttonDisabledCancel)
					butt.Enabled = false;
				TableCell cell = new TableCell();
				cell.Controls.Add(butt);
				buttonRow.Cells.Add(cell);
			}

			buttonTable.Rows.Add(buttonRow);

			// Add the buttons table to the main table
			mainTableRow = new TableRow();
			mainTableCell = new TableCell();
			mainTableCell.Controls.Add(buttonTable);
			mainTableRow.Cells.Add(mainTableCell);
			mainTable.Rows.Add(mainTableRow);

			Trace.EndProc(trState);
			return outerTable;
		}

		#endregion

		#region Modal
		//make this popup modal by placing a semi transparent DIV over the page to mask out all controls
		private void ModalPopup (bool show)
		{
			if (!this.ShowAsDialogBox)
			{
				if (this.Page != null && this.Page.HasControls())
				{
				
					Control forceField = SimpleControl.SimpleControlConstruct.FindControlById(this.Page,"forceField");
					if (forceField == null)
					{
						forceField=new Panel();
						((Panel)forceField).CssClass = "fogBox";
						forceField.ID = "forceField";
						this.Page.Controls.Add(forceField);
					}
					forceField.Visible=show;
				}
				ReplaceDropDowns(this.Page,show);
			}
		}
		
		// Disable all web controls except those that are in this popup
		// create textboxes to appear on the screen instead of drop downs which float on top 
		// of popups
		private void ReplaceDropDowns(Control owner,bool showText)
		{
			
			if (owner != null && owner.HasControls())
			{
				
				
				foreach (Control child in owner.Controls)
				{
					// Avoid disabling the controls on the popup itself
					if (!child.Equals(this))
					{
						// check that this control can be disabled
						if (child is WebControl)
						{
							WebControl wc = (WebControl)child;
							// disable it from the tab sequence so you can't use the tab key to get to it
							short oldtabindex = wc.TabIndex;
							
							if ((showText && oldtabindex >= 0) ||(!showText && oldtabindex < 0))
							{
								wc.TabIndex = (short)(-1 - oldtabindex);
							}
							// If you click on a button under the force field it gets the focus
							// Avoid this by  disabling it
							if (wc is Button)
							wc.Enabled=!showText;

						}
						if ( child is  Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.DropDownList)
						{
							Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.DropDownList dropDown =  
								(Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.DropDownList)child;
							if (showText)
							{
								// drop down lists are 'windowed' controls which means they float on top of any
								// absolute postioned divs. Convert them into text boxes which contain the currently selected value
								// of the drop down
								TextBox textBox=dropDown.textBox;
								int selectedIndex = dropDown.SelectedIndex;
								if (selectedIndex > 0 && selectedIndex < dropDown.Items.Count)
									textBox.Text = dropDown.Items[dropDown.SelectedIndex].Text;

								textBox.Visible=true;
								dropDown.Enabled=false;
								dropDown.Visible=false;
							}
							else
							{
								// reinstate the drop down and hide the text place holder
								dropDown.textBox.Visible=false;
								dropDown.Visible = true;
								dropDown.Enabled = true;
							}
				
						}
						// recurse round the children of this control
						ReplaceDropDowns(child,showText);
					}
				}
			}

		}
	


		#endregion

		#region Control Methods
		/// <summary></summary>
		public void ShowForMessage() 
		{
			this.ShowCancel = false;
			this.ShowInsert = false;
			this.ShowDelete = false;
			this.ShowUpdate = false;
			this.ShowMessage = true;
			this.ShowAsDialogBox = true;
			this.Visible = true;
		}

		/// <summary>
		/// Show the popup with a panel in it
		/// </summary>
		public void ShowForPanel() 
		{
			this.ShowCancel = false;
			this.ShowInsert = false;
			this.ShowDelete = false;
			this.ShowUpdate = false;
			this.ShowMessage = false;
			if (this.formTable != null)
				this.formTable.CssClass="popuptableforpanel";
			this.Visible = true;
		}

		/// <summary></summary>
		public void ShowForMessageCancel() 
		{
			this.ShowCancel = true;
			this.ShowInsert = false;
			this.ShowDelete = false;
			this.ShowUpdate = false;
			this.ShowMessage = true;
			this.Visible = true;
		}
		
		/// <summary></summary>
		public void ShowForViewing() {
			this.ShowCancel = true;
			this.ShowInsert = false;
			this.ShowDelete = false;
			this.ShowUpdate = false;
			this.ShowMessage = false;
			this.Visible = true;
		}

		/// <summary></summary>
		public void ShowForInsert() {
			this.XmlDoc = null;			// Don't display any old data
			this.ShowCancel = true;
			this.ShowInsert = true;
			this.ShowDelete = false;
			this.ShowUpdate = false;
			this.ShowMessage = false;
			this.Visible = true;
		}

		/// <summary></summary>
		public void ShowForUpdate() {
			this.ShowCancel = true;
			this.ShowInsert = false;
			this.ShowDelete = false;
			this.ShowUpdate = true;
			this.ShowMessage = false;
			this.Visible = true;
		}

		/// <summary></summary>
		public void ShowForUpdateDelete() {
			this.ShowCancel = true;
			this.ShowInsert = false;
			this.ShowDelete = true;
			this.ShowUpdate = true;
			this.ShowMessage = false;
			this.Visible = true;
		}

		/// <summary></summary>
		public void ShowForDelete() {
			this.ShowCancel = true;
			this.ShowInsert = false;
			this.ShowDelete = true;
			this.ShowUpdate = false;
			this.ShowMessage = false;
			this.Visible = true;
		}

		private string showForMessageText;
		/// <summary>
		/// The text to be displayed for a standard message pop-up with
		/// an ok button. Only works when used in conjunction with
		/// ShowForMessage method.
		/// </summary>
		public string ShowForMessageText 
		{
			set { this.showForMessageText = value; }
		}

		/// <summary>
		/// Overrides Control.CreateChildControls.
		/// Notifies this control to create any child nodes 
		/// </summary>
		protected override void CreateChildControls() 
		{
			ITraceState trState = Trace.StartProc("PopupForm.CreateChildControls");

			this.currentLocation = InClass.PopupForm;
			
			if (this.Visible) 
			{
				if (this.ShowAsDialogBox)
				{
					if (this.showForMessageText != null && this.showForMessageText.Length > 0)
					{
						// It's a dialog box popup with a message to show.
						HtmlGenericControl script =  new HtmlGenericControl("script");
						// Create some script to add the alert function to the onbodyload list
						string popupID = this.UniqueID.Replace(":","_");
						string jscript =
							"function popup_" + popupID + "() {alert('" + 
							// Adjust message so it displays as a javascript alert.
							// Also escape single quote char and replace <br> by new line.
							this.showForMessageText.Replace("'",@"\'").Replace("<br>",@"\n") + "'); ";
						// Check if a redirect is required afterwards.
						if (!StringUtils.IsStringEmpty(this.showAsDialogRedirectUrl))
							jscript += "window.navigate('" + this.showAsDialogRedirectUrl + "');";
						// End of function.
						jscript += "}" + " if(typeof(onloadFunctions) != 'undefined')"	+ "{" +
							"onloadFunctions[onloadFunctions.length] = popup_" + popupID +";"+ "}";
						script.InnerText = jscript;
						Controls.Add(script);
					}
				}
				else
				{
					// Add the JScript to enable dragging of the pop-up.
					HtmlGenericControl script =  new HtmlGenericControl("script");
					script.Attributes.Add("src","scripts/drag.js");
					Controls.Add(script);

					// Create the popup (DIV) window
					Panel popupPanel = new Panel();
					popupPanel.CssClass = "popuphidden";
					popupPanel.Style.Add("visibility","visible");

					// Add the form to the popup window
					popupPanel.Controls.Add(CreatePopupTable());
 
					// Add the drag window to the form
					Controls.Add(WrapControlInHtml(popupPanel,"controlpopup",this.ID));

					// Also add the script to set the focus.
					if (this.SetFocus != null)
						Controls.Add(this.SetFocus);
				}
			}

			Trace.EndProc(trState);
		}
		#endregion
	}
}