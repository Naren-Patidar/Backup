#region Using
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using Fujitsu.eCrm;
using Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using System.Threading;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.ControlLibrary.CSAScript;

#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary 
{

	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// ControlLibrary.ControlTable Class.
	/// </summary>
	/// <development>
	/// 	<version number="1.15" day="19" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Seoul/052</work_packet>
	///			<description>Added checkbox control.</description>
	///		</version>
	/// 	<version number="1.14" day="17" month="02" year="2003">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Added extra style</description>
	///		</version>
	/// 	<version number="1.13" day="13" month="02" year="2003">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Added extra style</description>
	///		</version>
	///		<version number="1.12" day="11" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/060</work_packet>
	///			<description>Added image control.</description>
	///		</version>
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added grid syntax to data form</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.09" day="20" month="11" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker></checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add GetDataNode </description>
	///		</version>
	/// 	<version number="1.08" day="12" month="11" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	/// 	<version number="1.07" day="06" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Add more CssClass detail to table etc.</description>
	///		</version>
	/// 	<version number="1.06" day="06" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>WP/Barcelona/031</work_packet>
	///			<description>Changed Session index to use SharedUtils class WebSessionIndexes.</description>
	///		</version>
	/// 	<version number="1.05" day="24" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Bug fix</work_packet>
	///			<description>Changes to CrmServer mean web references need updating.</description>
	///		</version>
	/// 	<version number="1.04" day="24" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>Bug fix</work_packet>
	///			<description>Radio button lists displayed the reference value as well as the option</description>
	///		</version>
	///		<version number="1.03" day="24" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Bug fix</work_packet>
	///			<description>Fixed the CrmServiceException argument parameter.</description>
	///		</version>
	///		<version number="1.02" day="18" month="04" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Changed namespace to Fujitsu.eCrm.CSAControlLibrary</description>
	///		</version>
	///		<version number="1.01" day="06" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Internationalisation and standardisation
	///			of error handling. Change getReferenceData so we can
	///			select to retrieve data pertaining to all customers
	///			or just the current customer.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet>WP/DOGON/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class ControlTable : BaseControl 
	{

		#region Extract Page Configuration 
		/// <summary>
		/// Extract Page Configuration for a Simple Control
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		/// <param name="dataValue"></param>
		/// <param name="id"></param>
		/// <param name="caption"></param>
		/// <param name="tip">tooltip text</param>
		/// <param name="format">format for displaying text</param>
		/// <param name="listItems"></param>
		/// <param name="ignoreList"></param>
		/// 
		public void GetSettings(
			XmlElement configXmlElement, 
			XmlElement dataXmlElement,
			out string dataValue,
			out string id,
			out string caption,
			out string tip,
			out string format,
			out ListItemCollection listItems,
			out ArrayList ignoreList) {

			GetSettings(configXmlElement,dataXmlElement,out dataValue, out id, out caption,out tip,out format);

			// Get any listbox, radiobutton or ticklist options
			listItems = null;
			ignoreList = null;
			foreach (XmlNode n in configXmlElement.ChildNodes) {
				if (n.Name=="options") {
					listItems = this.GetListItems(n,dataValue, out ignoreList);
				}
			}
		}

		/// <summary>
		/// Extract Page Configuration for a Simple Control
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		/// <param name="dataValue"></param>
		/// <param name="id"></param>
		/// <param name="caption"></param>
		/// <param name="tip">tooltip text</param>
		/// <param name="format">format for displaying text</param>
		public void GetSettings(
			XmlElement configXmlElement, 
			XmlElement dataXmlElement,
			out string dataValue,
			out string id,
			out string caption,
			out string tip,
			out string format) 
		{

			string actualXPath, xPathItemName;
			XmlNode selectedNode = null;

			id = String.Empty;
			dataValue = String.Empty;
			caption = String.Empty;

			string defaultValue = GetAttributeString(configXmlElement, "default");

			GetFieldDetails(configXmlElement, out actualXPath, out xPathItemName, out id, out caption, out tip, out format);

			selectedNode = GetDataNode(dataXmlElement, actualXPath);
				
			// If no data available, use the default value
			if (selectedNode==null) {
				dataValue = defaultValue;

				// if the default is a Session Variable, load it.
				if ((dataValue.Length > 1) && (dataValue.StartsWith("$"))) {
					string sessionVariableName = dataValue.Substring(1);
					object sessionVar = this.Page.Session[sessionVariableName];
					if (sessionVar != null) {
						dataValue = sessionVar.ToString().Trim();
					} else {
						dataValue = String.Empty;
					}
				}
			} else {
				dataValue = selectedNode.Value.Trim();
			}
		}

		
		/// <summary>
		/// Navigates to a node from element
		/// </summary>
		/// <param name="dataXmlElement">element to navigate</param>
		/// <param name="xpath">Navigation path</param>
		/// <returns>the required node</returns>
		public static XmlNode GetDataNode(XmlElement dataXmlElement, string xpath)
		{
			XmlNode selectedNode = null;
			if (dataXmlElement != null) 
			{
				// Get the data value from the relevant part of the DataElement
				try	
				{
					selectedNode = dataXmlElement.SelectSingleNode(xpath);
					if (selectedNode != null) {
						if (selectedNode.NodeType.Equals(XmlNodeType.Element)) {
							selectedNode = selectedNode.SelectSingleNode("text()");
						}
					}
 

				} 
				catch (Exception e) 
				{
					string[] args;
					args = new string[] {xpath, dataXmlElement.OuterXml};  
					CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.UnsatisfiedXPath",e ,args);
					throw ce;
				}
			}
			return selectedNode;

		}

		private ListItemCollection GetListItems(XmlNode node, string dataValue, out ArrayList ignoreList) {

			ArrayList keyList;
			ArrayList textList;
			ArrayList orderList;
			GetOptionsData(node, out keyList, out textList, out orderList, out ignoreList);

			#region Remove ignore keys if they do not match the dataValue
			ArrayList ignoreIndexList = new ArrayList();
			foreach (string ignoreItem in ignoreList) {
				if (ignoreItem != dataValue) {
					int ignoreIndex = keyList.IndexOf(ignoreItem);
					if (ignoreIndex != -1) {
						ignoreIndexList.Add(ignoreIndex);
					}
				}
			}
			ignoreIndexList.Sort();
			ignoreIndexList.Reverse();
			foreach (int ignoreIndex in ignoreIndexList) {
				keyList.RemoveAt(ignoreIndex);
				textList.RemoveAt(ignoreIndex);
				orderList.RemoveAt(ignoreIndex);
			}
			#endregion

			#region Sort Lists
			object[] allKeys = keyList.ToArray();
			object[] allTexts = textList.ToArray();
			object[] allOrders1 = orderList.ToArray();
			object[] allOrders2 = orderList.ToArray();
			IComparer comparer = GetComparer(node);
			Array.Sort(allOrders1,allKeys,comparer);
			Array.Sort(allOrders2,allTexts,comparer);
			#endregion

			#region Create ListItemCollection
			ListItemCollection listItems = new ListItemCollection();
			// copy key-value pairs by order into the list item collection
			for (int i=0; i<allKeys.Length; i++) {
				ListItem listItem = new ListItem((string)allTexts[i], (string)allKeys[i]);
				listItems.Add(listItem);
			}
			#endregion

			return listItems;
		}

		private IComparer GetComparer(XmlNode optionsNode) {

			CSAComparer.ICSAComparer comparer;

			string ordertype =  GetAttributeString(optionsNode,"ordertype");
			switch (ordertype) {
				case "integer":
					comparer = new CSAComparer.IntegerComparer();
					break;
				case "string":
				default:
					comparer = new CSAComparer.StringComparer();
					break;
			}

			string orderby =  GetAttributeString(optionsNode,"orderby");
			if (orderby == "desc") {
				comparer.Descending = true;
			}

			return comparer;
		}

		#endregion

		#region Write Page

		//private bool validationSummaryCreated = false;
		/// <summary>
		/// creates the layout tables for a data control. This will call itself recursively for tables within tables
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="header">true if the header caption is to be displayed</param>
		/// <returns>a layout table containing the controls</returns>
		protected Control CreateGrid(
			XmlElement configXmlElement,
			bool header)
		{	
			return CreateGrid(configXmlElement,header,true);
		}

		//private bool validationSummaryCreated = false;
		/// <summary>
		/// creates the layout tables for a data control. This will call itself recursively for tables within tables
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="header">true if the header caption is to be displayed</param>
		/// <param name="topLevel">true if this is the top level grid</param>
		/// <returns>a layout table containing the controls</returns>
		protected Control CreateGrid(
			XmlElement configXmlElement,
			bool header, bool topLevel)
		{
			Control returnControl = null;
			Table gridTable = new Table();
			gridTable.ID = this.GenerateControlId("gridtable",configXmlElement);

			TableRow gridRow = new TableRow();
			TableCell gridCell;
			int rows = 1;
			int cols = 1;
			XmlElement form  =  configXmlElement;

			// Get the fieldset legend early, so it cab be added to the
			// class of the table.
			XmlNode legendNode = form.SelectSingleNode("header/text()"); //localize this later - it's used for class name and the Legend
			string legendText = null;
			if (legendNode != null)
				legendText = legendNode.Value;

			if (topLevel)
				gridTable.CssClass="DataFormTable " + (legendText!=null?legendText.Replace(".",""):String.Empty);
			else
			{
				gridTable.CssClass=(legendText!=null?legendText.Replace(".",""):String.Empty);
				gridTable.CellSpacing = 0;
			}

			// Need to try and calculate an ID for the 

			XmlElement grid =  (XmlElement)form.SelectSingleNode("grid");
			if ( grid != null)
			{
				// calculate the rows and columns for this form 
				if ((rows = BaseControl.GetAttributeInt(grid,"rows")) == -1)
				{
					// the grid has not specified new row coordinates
					// so use the form's row span to inherit the same grid coords
					rows = BaseControl.GetAttributeInt(form,"height");
				};
				if ((cols = BaseControl.GetAttributeInt(grid,"cols")) == -1)
				{
					// the grid has not specified new col coordinates
					// so use the form's col span to inherit the same grid coords
					cols = BaseControl.GetAttributeInt(form,"width");
				};
				// check for no specification of rows and columns in this case
				if (rows == -1) rows = 1;
				if (cols == -1) cols = 1;

				string headerText = BaseControl.GetAttributeString(form,"header") ;
				// declare 2-dimensional array to use as a map of the table 
				// It will be used to keep track of the grid cells used

				int[,] gridMap = new int[rows,cols];
				gridMap.Initialize();
				
				if (form.SelectSingleNode("fieldGroup") != null) {

					// this table should be surrounded by a fieldSet
					HtmlGenericControl fieldSet = new HtmlGenericControl("fieldset");
					string cleanedLegend = legendText.Replace(".","");
					string extraClassFieldSet = "dffs"+cleanedLegend;
					fieldSet.Attributes["class"] = "dataformfieldset "+extraClassFieldSet;

					if (legendText != null) {
						HtmlGenericControl legend =  new HtmlGenericControl("legend");
						string extraClassLegend = "dffl"+cleanedLegend;
						legend.Attributes["class"] = "dataformfieldlegend "+extraClassLegend;

						legend.InnerText =  Localization.GetLocalizedAttributeString(legendText);
						fieldSet.Controls.Add(legend);
					}
					
					fieldSet.Controls.Add(gridTable);
					returnControl =  fieldSet;

				}
				else if (headerText != string.Empty ) {
					if (header) {
						// put a header row in the table. It is not included in the grid
						TableRow headerRow = new TableRow();
						gridTable.Rows.Add(this.GenerateHeaderRow(headerText));
						gridTable.Rows[0].Cells[0].ColumnSpan=cols;
					}
					returnControl =  gridTable;
				} else {
					returnControl =  gridTable;
				}
				
				// find all the sub forms or fields in this grid
				// slot them into the grid into the first empty space
				XmlNodeList gridBricks =  grid.SelectNodes("form|field|column|empty|placeholder|button");
				int brickIndex = 0; // > 0 values used to complete the gridMap
				foreach (XmlNode brick in gridBricks)
				{	
					brickIndex++;
					// determine its width and height
					int height = 1;
					int width = 1;
					height = BaseControl.GetAttributeInt(brick,"height");
					width = BaseControl.GetAttributeInt(brick,"width");
					// reset defaults if necessary
					if ( height == -1)
						height = 1;

					if ( width == -1)
						width = 1;
					
					
					// calculate its position in the grid
					int top = 0 , left = 0;
					for (top = 0; top <= gridMap.GetUpperBound(0); top++)
					{
						for (left = 0; left <= gridMap.GetUpperBound(1); left++)
						{
							if (gridMap[top,left]==0) break; 
						}
						if (left <= gridMap.GetUpperBound(1) && gridMap[top,left]==0) break;


					}
					
					if (left == 0 || gridMap[top,left-1] < brickIndex - 1) // new row or previous grid entry not populated by the previous brick
					{
						// start a new row
						gridRow = new TableRow();
						gridTable.Rows.Add(gridRow);

					}

					// mark the cells used in the map
					for (int mRow = top;mRow <= gridMap.GetUpperBound(0) && mRow < top + height;mRow++)
					{
						for (int mCol = left;mCol <= gridMap.GetUpperBound(1) && mCol < left + width;mCol++)
						{
							gridMap[mRow,mCol] = brickIndex;
						}
					}

					gridCell = new TableCell();
					gridCell.VerticalAlign = VerticalAlign.Top;
					gridRow.Cells.Add(gridCell);
					gridCell.ColumnSpan = width;
					gridCell.RowSpan = height;

					Table controlTable;
					switch (brick.Name)
					{
						case "form":
							// recursively create an inner form
							gridCell.Controls.Add(CreateGrid((XmlElement)brick,true,false));
							break;
						case "column":
							int columns = BaseControl.GetAttributeInt(brick,"colWidth");
							if (columns == -1) columns = 1;

							controlTable = new Table();
							controlTable.BorderWidth=0;
							controlTable.ID = this.GenerateControlId("controlTable",brick);
							controlTable.GridLines=GridLines.None;
							controlTable.CssClass="controltable";
							controlTable.Width = Unit.Percentage(100);

							int fields=0;
							TableRow tableRow=null;
							foreach (XmlElement elem in brick.SelectNodes("field"))
							{
								if (fields++ % columns==0)
								{
									tableRow=new TableRow();
									controlTable.Rows.Add(tableRow);
								}
								GenerateFieldRow(elem, this.DataXmlElement,tableRow);
							}
							gridCell.Controls.Add(controlTable);
							break;
						case "field":
							//field so go create the controls
							controlTable = new Table();
						
							controlTable.BorderWidth=0;
							controlTable.GridLines=GridLines.None;
							controlTable.CssClass="controltable";
							controlTable.ID = this.GenerateControlId("controlTable",brick);
							controlTable.Width = Unit.Percentage(100);
							controlTable.Rows.Add(this.GenerateFieldRow((XmlElement)brick,this.DataXmlElement,null));
							gridCell.Controls.Add(controlTable);
	
							break;
						case "empty":
							gridCell.Text = "&nbsp;";
							// See what has been specified
							XmlNodeList brickChildren = brick.ChildNodes;
							foreach (XmlNode brickChild in brickChildren)
							{
								switch (brickChild.Name)
								{
									case "emptywidth":
										// Get the value of the node and set the width of
										// the cell.
										gridCell.Width = new Unit(int.Parse(brickChild.InnerText),UnitType.Pixel);
										break;
									default:
										break;
								}
							}
							break;
						case "placeholder":
							PlaceHolder placeholder = new PlaceHolder();
							XmlNode placeholderid = brick.SelectSingleNode("id/text()"); // don't localize
							if (placeholderid != null)
								placeholder.ID = placeholderid.Value;
							gridCell.Controls.Add(placeholder);
							break;
						case "button":
							Button button = new Button();
							XmlNode buttonid = brick.SelectSingleNode("id/text()"); // don't localize
							
							
							button.CssClass="button";
							if (buttonid != null)
								button.ID = buttonid.Value;
							
							button.Text  = BaseControl.GetAttributeString(brick,"text");
						
							button.ToolTip   = BaseControl.GetAttributeString(brick,"tip");
							gridCell.Controls.Add(button);
							break;

						default:
							CrmServiceException ce = new CrmServiceException("Library","Configuration",
								"CSAControlLibrary.InvalidElement",brick.Name);
							throw ce;
					}
				}
			}
			return returnControl;
		}
			
		/// <summary>
		/// Returns a TableRow containing a single control and its caption, 
		/// as specified by the current XML element - or adds the control to
		/// the parent row
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		/// <param name="parentRow"></param>
		/// <returns></returns>
		public TableRow GenerateFieldRow(XmlElement configXmlElement, XmlElement dataXmlElement,TableRow parentRow) {

			// Add the input field
			if (configXmlElement.Name != "field") {
				CrmServiceException ce = new CrmServiceException("Library", "Configuration", "ControlLibrary.MissingFieldNode", configXmlElement.OuterXml);
				throw ce;
			}

			string controlType = GetAttributeString(configXmlElement, "type");
			if (controlType.Length == 0) {
				controlType = "textbox";
			}

			TableRow controlTableRow;
			if (parentRow==null)
				controlTableRow= new TableRow();
			else
				controlTableRow=parentRow;

			switch (controlType) {
				case "textbox" :
					new SimpleControl.TextBox(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "multi" :
					new SimpleControl.MultiControl(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "currency" :
					new SimpleControl.CurrencyTextBox(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "listbox" :
					new SimpleControl.DropDownList(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "radiobuttons" :
					new SimpleControl.RadioButtonList(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "ticklist" :
					new SimpleControl.CheckBoxList(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "date" :
					new SimpleControl.DatePlaceHolder(this,controlTableRow,configXmlElement,dataXmlElement, false, false);
					break;
				case "startdate" :
					new SimpleControl.DatePlaceHolder(this,controlTableRow,configXmlElement,dataXmlElement, true, false);
					break;
				case "enddate" :
					new SimpleControl.DatePlaceHolder(this,controlTableRow,configXmlElement,dataXmlElement, false, true);
					break;
				case "blankline" :
					new SimpleControl.BlankLine(this,controlTableRow);
					break;
				case "image" :
					new SimpleControl.Image(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "label" :
					new SimpleControl.Label(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "hyperlink" :
					new SimpleControl.HyperLink(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "checkbox" :
					new SimpleControl.CheckBox(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "report" :
					new SimpleControl.ReportControl(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				case "buttonlist" :
					new SimpleControl.ButtonList(this,controlTableRow,configXmlElement,dataXmlElement);
					break;
				default :
					string[] args = {configXmlElement.Name,controlType};
					CrmServiceException ce = new CrmServiceException("Library", "Configuration", "ControlLibrary.InvalidControlTypeAttribute", args);
					throw ce;
			}

			return controlTableRow;
		}


		/// <summary>
		/// Returns a table Row, containing the header specified by the current XML elelemt.
		/// </summary>
		/// <param name="headerText"> the header's text</param>
		/// <returns></returns>
		public TableRow GenerateHeaderRow(string headerText) {

			//	<tr>
			//	  <td class="formheader">
			//		Customer Details
			//	  </td>
			//  </tr>

			TableRow row = new TableRow();
			TableCell cell =  new TableCell();

			cell.CssClass = "formheader";
			cell.Text = headerText;
			row.Cells.Add(cell);

			return row;
		}

		/// <summary>
		/// Returns an HTML table, containing all the controls specified
		/// within the current XML elelemt.
		/// </summary>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		/// <returns></returns>
		public Table CreateControlTable(XmlElement configXmlElement, XmlElement dataXmlElement) {

			Table controlTable = new Table();
			controlTable.BorderWidth=0;
			controlTable.GridLines=GridLines.None;
			controlTable.CssClass="controltable";
			controlTable.ID = this.GenerateControlId("controlTable",dataXmlElement);
			controlTable.Width = Unit.Percentage(100);

			foreach (XmlElement elem in configXmlElement.ChildNodes) {
				controlTable.Rows.Add(GenerateFieldRow(elem, dataXmlElement,null));
			}
			return controlTable;
		}
		#endregion

		#region SetFocus
		/// <summary>
		/// Set input focus on a textbox or multi control.
		/// </summary>
		/// <param name="id">The ID of the control to be given focus</param>
		public void SetControlFocus(string id)
		{
			// Find the new control to take focus
			Control ctrl = SimpleControlConstruct.FindControlById(this, id);

			if (ctrl is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.TextBox) 
			{
				CSAGenericTextBox tb = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.TextBox)ctrl;
				if (!tb.ReadOnly)
				{
					HtmlGenericControl javaCtrl = CSAJavaScript.SetFocusOnControl(tb.ID, tb.CurrentLocation);
					this.Controls.Add(javaCtrl);
				}
			}
			else if (ctrl is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl) 
			{
				Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl mc = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl)ctrl;
				// Get the parts.
				ArrayList parts = mc.Parts;
				foreach (object part in parts) 
				{
					if (part is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox) 
					{
						Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox tb = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox)part;
						if (!tb.ReadOnly)
						{
							HtmlGenericControl javaCtrl = CSAJavaScript.SetFocusOnControl(tb.ID, tb.CurrentLocation);
							this.Controls.Add(javaCtrl);
						}
					}
				}
			}

		}
		#endregion

		#region Delete Control Contents
		/// <summary>
		/// Delete all text from a control.
		/// </summary>
		/// <param name="id">The ID of the control that requires to be
		/// emptied.</param>
		public void RemoveDataValues(string id)
		{
			this.DeleteControlValues(this.Controls,id);
		}
		/// <summary>
		/// Delete all text from control hierarchy.
		/// </summary>
		public void RemoveDataValues()
		{
			this.DeleteControlValues(this.Controls,null);
		}

		/// <summary>
		/// Recursive routine to walk through the control hierarchy and
		/// delete data values.
		/// </summary>
		/// <param name="ctrls"></param>
		/// <param name="id"></param>
		private void DeleteControlValues(ControlCollection ctrls,string id) 
		{

			foreach (Control ctrl in ctrls) 
			{
				// Check to see if an ID has been supplied.
				bool found = false;
				bool check = false;
				if (!StringUtils.IsStringEmpty(id))
				{
					// Set the flag to indicate a check is required.
					check = true;
					// It has been supplied, so check to see if this control
					// matches the ID.
					if (ctrl.ID == id)
						// This isn't the one we're looking for, so move onto
						// the next.
						found = true;
				}
				if (check == false || (check == true && found == true))
				{
					if (ctrl.GetType().Namespace == "Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl") 
					{

						if (ctrl is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.TextBox) 
						{
							Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.TextBox tb = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.TextBox)ctrl;
							if (tb.ReadOnly == false)
								tb.Text = String.Empty;
						}
						else if (ctrl is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl) 
						{
							Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl mc = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl)ctrl;
							// Get the parts.
							ArrayList parts = mc.Parts;
							foreach (object part in parts) 
							{
								if (part is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox) 
								{
									Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox tb = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.MultiControl.TextBox)part;
									if (tb.ReadOnly == false)
										tb.Text = String.Empty;
								}
							}
						}
						else if (ctrl is Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.CheckBox) 
						{
							Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.CheckBox cb = (Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl.CheckBox)ctrl;
							cb.Checked = false;
						}
					}
				}
				if (ctrl.HasControls() && found == false) 
				{
					this.DeleteControlValues(ctrl.Controls,id);
				} 
			}
		}
		#endregion

		#region Read Information
		/// <summary>
		/// Returns a hashtable containing the XPath and value of each 
		/// data input field.
		/// </summary>
		/// <returns></returns>
		public Hashtable GetDataValues() 
		{

			Hashtable ht = new Hashtable(51);		

			this.WalkControls(Controls, ht);
			return ht;
		}

		/// <summary>
		/// Recursive routine to walk through the Control Hierarchy and
		/// extract their xpath and value, then write them into a
		/// Hashtable for sending to the CRM Server.
		/// </summary>
		/// <param name="ctrls"></param>
		/// <param name="ht"></param>
		private void WalkControls(ControlCollection ctrls, Hashtable ht) {

			foreach (Control ctrl in ctrls) {
				// If the control has any sub-controls, recurse through them
				if (ctrl.GetType().Namespace == "Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl") {
					((ISimpleControl)ctrl).GetValue(ht);
				} else if (ctrl.HasControls()) {
					this.WalkControls(ctrl.Controls, ht);
				} 
			}
		}
		#endregion
	}
}