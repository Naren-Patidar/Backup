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
using System.Threading;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary 
{

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <summary>
	/// Generate asp from config files.
	/// </summary>
	/// 
	/// <development>
	///		<version number="1.14" day="25" month="03" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/072</work_packet>
	///			<description>Added conditional links in select column.</description>
	///		</version>
	///		<version number="1.13" day="19" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/052</work_packet>
	///			<description>Added message to be displayed on no data
	///			to be set via property.</description>
	///		</version>
	///		<version number="1.12" day="04" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added display message when no data is returned.</description>
	///		</version>
	///		<version number="1.11" day="23" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added scrolling tables.</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.07" day="20" month="11" year="2002">
	///			<developer>Stuart Forbes</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Add styles</description>
	///		</version>
	/// 	<version number="1.06" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	/// 	<version number="1.06" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed CreateTable to wrap control in SPAN tags.</description>
	///		</version>
	/// 	<version number="1.05" day="06" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stephen Lang</checker>
	///			<work_packet>WP/Barcelona/031</work_packet>
	///			<description>Changed storing of dataview in Session to become the
	///			DataTable. We are now using Out-Of-Proc Session and DataView isn't
	///			serialisable.</description>
	///		</version>
	/// 	<version number="1.04" day="18" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Tidy</work_packet>
	///			<description>Removed unused language string and moved header to
	///			correct space to prevent warning messages.</description>
	///		</version>
	/// 	<version number="1.03" day="17" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/barcelona/015</work_packet>
	///			<description>
	///			DataTable.CreateChildControls() method uses the ConfigDoc property 
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
	///			<work_packet>wp/DOGON/01</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	///
	#endregion

	public class DataTable : BaseControl 
	{
		#region Properties
		//public System.Web.UI.Page _page = null;

		/// <summary>Get/Set maximum number of lines that can be displayed on a single page.</summary>
		private int LinesPerPage {
			get {
				Object o = ViewState["linesperpage"];
				if (o == null) {
					return 20;
				} else {
					return (int)o;
				}
			}
			set { ViewState["linesperpage"] = value;}
		}
		private string tableXPath = null;
		/// <summary>The XPath used to search from the data element</summary>
		public string TableXPath {
			get { return this.tableXPath; }
			set { this.tableXPath = value; }
		}

		private string noDataMessage;
		/// <summary>
		/// Set the message to be displayed when there is no data. This
		/// will override the setting in the configuration file, if there
		/// is one.
		/// </summary>
		public string NoDataMessage 
		{
			get { return this.noDataMessage; }
			set { this.noDataMessage = value; }
		}


		//Added for points partner..
		private string alternativeField;
		/// <summary>
		/// Set the alternative field to be displayed.
		/// </summary>
		public string AlternativeField
		{
			get { return this.alternativeField; }
			set { this.alternativeField = value; }
		}
		private string alternativeLinkText;
		/// <summary>
		/// Set the alternative link text to be displayed
		/// </summary>
		public string AlternativeLinkText
		{
			get { return this.alternativeLinkText; }
			set { this.alternativeLinkText = value; }
		}
		private string alternativeHref;
		/// <summary>
		/// Set the alternative href to be displayed
		/// </summary>
		public string AlternativeHref
		{
			get { return this.alternativeHref; }
			set { this.alternativeHref = value; }
		}

		private string alternateValueCheck;
		/// <summary>
		/// Set the alternative href to be displayed
		/// </summary>
		public string AlternateValueCheck
		{
			get { return this.alternateValueCheck; }
			set { this.alternateValueCheck= value; }
		}

		private string alternateFieldCheck;
		/// <summary>
		/// Set the alternative href to be displayed
		/// </summary>
		public string AlternateFieldCheck
		{
			get { return this.alternateFieldCheck; }
			set { this.alternateFieldCheck = value; }
		}

		private string alternateValue;
		/// <summary>
		/// Set the alternative href to be displayed
		/// </summary>
		public string AlternateValue
		{
			get { return this.alternateValue; }
			set { this.alternateValue = value; }
		}

		private string idfieldName;
		/// <summary>
		/// Set the alternative href to be displayed
		/// </summary>
		public string IDFieldName
		{
			get { return this.idfieldName; }
			set { this.idfieldName = value; }
		}
		

		private string conditionalFieldName;
		/// <summary>
		/// Set the conditionalfieldname
		/// </summary>
		public string ConditionalFieldName
		{
			get { return this.conditionalFieldName; }
			set { this.conditionalFieldName = value; }
		}
		

		private string conditionFieldName;
		/// <summary>
		/// Set the conditionfieldname
		/// </summary>
		public string ConditionFieldName
		{
			get { return this.conditionFieldName; }
			set { this.conditionFieldName = value; }
		}
		

		private string conditionValue;
		/// <summary>
		/// Set the conditionvalue
		/// </summary>
		public string ConditionValue
		{
			get { return this.conditionValue; }
			set { this.conditionValue = value; }
		}
       //End of addition.
		#endregion

		#region HTML Methods
		private void CreateTable( ) 
		{
			XmlElement steeringXml = this.ConfigXmlDoc.DocumentElement;
			XmlElement dataXml = this.DataXmlElement;
			string[] sortOrderArray = new string[10];
			//points partner
			bool doBind=false;
			//end.
			
			ITraceState trState = Trace.StartProc("DataTable.CreateTable");

			// Check that the top-level element is table.
			if (steeringXml.LocalName != "table") {
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.MissingTableElement", Trace, steeringXml.OuterXml);
				throw ce;
			}

			// Get the xpath attribute of the top-level XML element for each 
			// row of table data from the steering files "table" element.
			if (this.TableXPath == null) 
			{
				this.TableXPath = GetAttributeString(steeringXml, "xpath");
			}
			if (tableXPath.Length==0) {
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "ControlLibrary.MissingXPathAttribute", Trace, steeringXml.OuterXml);
				throw ce;
			}
			this.tableXPath = "descendant-or-self::" + this.tableXPath;
			// Get a set of elements, one for each row in the table.
			XmlNodeList tableNodeList = null;
			if (dataXml != null) {
				tableNodeList = dataXml.SelectNodes(this.tableXPath);
			}
			// Get the width and height elements, if there are any,
			// out of the steering file.
			string heightValue = BaseControl.GetAttributeString(steeringXml,"height");
			//Added for points partner
			string alternateField=BaseControl.GetAttributeString(steeringXml,"alternatefield");
			string alternateLinkText=BaseControl.GetAttributeString(steeringXml,"alternatelinktext");
			string alternateHref=BaseControl.GetAttributeString(steeringXml,"alternatehref");
			string alternateValueCheck=BaseControl.GetAttributeString(steeringXml,"alternatevaluecheck");
			string alternateFieldCheck=BaseControl.GetAttributeString(steeringXml,"alternatefieldcheck");
			string alternateValue=BaseControl.GetAttributeString(steeringXml,"alternatevalue");
			string idfieldName=BaseControl.GetAttributeString(steeringXml,"idfieldname");
			string conditionalField=BaseControl.GetAttributeString(steeringXml,"conditionalfield");
			string conditionField=BaseControl.GetAttributeString(steeringXml,"conditionfield");
			string conditionValue=BaseControl.GetAttributeString(steeringXml,"conditionvalue");
			//end of addition.
			UnitType heightUnitType = UnitType.Pixel;
			int height = -1;
			
				height = CheckAndExtractDimension(heightValue,"height",out heightUnitType);
			// Create a datagrid control & set its styles.
			DataGrid dg = new DataGrid();
			
			//Added for pointspartner
			if((alternateField!="") &&(alternateField!=null))
			{
				this.AlternativeField=LocalizationLibrary.Localization.GetLocalizedAttributeString(alternateField);
				this.AlternativeHref=alternateHref;
				this.AlternativeLinkText=LocalizationLibrary.Localization.GetLocalizedAttributeString(alternateLinkText);
				this.AlternateValue=alternateValue;
				this.AlternateFieldCheck=alternateFieldCheck;
				this.AlternateValueCheck=alternateValueCheck;
				this.IDFieldName=idfieldName;
				doBind=true;
			}
			if((conditionField!="")&&(conditionField!=null))
			{
				this.ConditionalFieldName=LocalizationLibrary.Localization.GetLocalizedAttributeString(conditionalField);
				this.ConditionFieldName=conditionField;
				this.ConditionValue=conditionValue;
				doBind=true;
			}
			if(doBind)
			{
				dg.ItemDataBound+=new DataGridItemEventHandler(DataGrid_ItemDataBound);
			}
			//end of addition.
			dg.ID = this.GenerateControlId("gridData",steeringXml);
			// Set the flag to indicate whether a height has been supplied for
			// the table. If it has then the table will need splitting into
			// two. This enables a scrollable table with fixed column headings.
			bool truncate = false;
			if (height > -1)
				truncate = true;
			dg.AutoGenerateColumns = false;
			dg.ShowHeader = true;
			Panel p = null;
			if (truncate)
			{
				// A height has been specified for the grid. The grid will
				// need to be wrapped in a panel. This will enable the
				// overflow-y:auto style to be applied and hence, make
				// a scrollbar appear when the height is exceeded. Create
				// the panel first.
				p = new Panel();
				// Create the style to enable the scrollbars to appear.

				if ((tableNodeList == null) || (tableNodeList.Count < 1))
				{
					// There wasn't any data, so don't add the overflow-y to the panel
					// we don't need the scrollbars if there's no data.
					p.Attributes["style"] = "border-collapse:collapse;";
				} 
				else 
				{
					p.Attributes["style"] = "overflow-y:auto;";
				}
					// Set the height of the panel.
					p.Height = new Unit(height, heightUnitType);
					// Make the datagrid heading disapear.
					dg.ShowHeader = false;
					dg.Attributes["style"] = "table-layout:fixed;border-collapse:collapse";
				
			}
			dg.CssClass = "griddata";
			dg.ItemStyle.CssClass = "gridrow";
			dg.AlternatingItemStyle.CssClass = "gridalternaterow";
			dg.HeaderStyle.CssClass = "gridheader";
			dg.PageSize = this.LinesPerPage;
			dg.PagerStyle.HorizontalAlign =	HorizontalAlign.Right;
			dg.PagerStyle.NextPageText = Localization.GetLocalizedAttributeString("NextPage");
			dg.PagerStyle.PrevPageText = Localization.GetLocalizedAttributeString("PrevPage");
			dg.PagerStyle.CssClass = "gridheader";
			dg.PagerStyle.Mode = PagerMode.NextPrev;
			//dg.PageIndexChanged += new DataGridPageChangedEventHandler(DataGridPageChange);

			// Create a dataset to hold the data for binding to the datagrid
			DataSet ds = new DataSet();
			System.Data.DataTable dt = ds.Tables.Add("DataTable");

			// Create an ADO DataSet and ASP DataGrid to contain the table's data
			ArrayList xpathList = new ArrayList();
			ArrayList dataTypeList = new ArrayList();
			ArrayList keyLists = new ArrayList();
			ArrayList textLists = new ArrayList();
			// If a height has been supplied, then we need a seperate table.
			Table headerTable = null;
			TableRow headerRow = null;
			if (truncate)
			{
				headerTable = new Table();
				headerTable.GridLines = GridLines.Both;
				headerTable.CssClass="gridheadertable";
				headerTable.Attributes["style"] = "table-layout:fixed;border-collapse:collapse";
				headerRow = new TableRow();
				headerRow.CssClass = "gridheader";
			}
			int numberColumns = 0;
			int columnNumber = 0;
			Hashtable csInfo = new Hashtable();
			foreach(XmlElement el in steeringXml.SelectNodes("//field"))
			{
				// Get the rest of the field details.
				string actualXPath,xPathItemName,id,caption,tip,overrideFormat;
				GetFieldDetails(el, out actualXPath, out xPathItemName,
					out id, out caption,out tip,out overrideFormat);

				// Attempt to get the size of the column.
				string sizeValue = BaseControl.GetAttributeString(el,"size");
				int size = -1;
				UnitType unitType = UnitType.Pixel;
				
				size = CheckAndExtractDimension(sizeValue,"size",out unitType);
				
				// If a height has been specified, then add to the header row.
				if (truncate)
				{
					// Need to check that a size has been specified for this column
					// otherwise throw an exception.
					if (size > -1)
					{
						// Create a new cell with the supplied width.
						TableCell headerCell = new TableCell();
						// Check to see if this is the last column. If it is,
						// then add 30 to the width to encompass the scroll
						// bar.
						headerCell.Width = new Unit(size,unitType);
						headerCell.Text = caption;
						headerCell.ToolTip = tip;
						// Add the cell to the row.
						headerRow.Cells.Add(headerCell);
						numberColumns++;
					}
					else
					{
						// Size has not been supplied, so throw an exception.
						CrmServiceException ce = new CrmServiceException("Library",
							"Data","CSAControlLibrary.MissingElement","size","field");
						throw ce;
					}
				}

				string type = GetAttributeString(el,"type");
				string href = GetAttributeString(el,"href");
				string linkText = GetAttributeString(el, "linktext");
				//Creates hyperlink under customer name(view reward group) to view customer detail
				string showValue = GetAttributeString(el, "showValue");
				string showCrmid = GetAttributeString(el, "showCrmid");
				if (StringUtils.IsStringEmpty(linkText))
					// No value supplied, so use the default.
					linkText = Localization.GetLocalizedAttributeString("Select");
			

				// save the sorting order number and direction in an array of column names.
				int sortOrder = GetAttributeInt(el,"sortorder");
				if ((sortOrder > 0) && (sortOrder < sortOrderArray.GetUpperBound(0))) 
				{
					sortOrderArray[sortOrder] = caption;
					if (GetAttributeBool(el,"sortreverse")) 
					{
						sortOrderArray[sortOrder] += " DESC";
					}
				}

				// Add the column to the ADO datatable
				xpathList.Add(actualXPath);
				dataTypeList.Add(type); 

				// Determine the correct format for the ADO dataTable and ASP datagrid
				string colType = "System.String";

				string format = String.Empty;
				HorizontalAlign align;
				switch (type) 
				{
					case "date":
						colType = "System.DateTime";
						align=HorizontalAlign.Center;
						format=" {0:dd/MMM/yyy} ";
						break;
					case "enddate":
						colType = "System.DateTime";
						align=HorizontalAlign.Center;
						format=" {0:dd/MMM/yyy} ";
						break;
					case "startdate":
						colType = "System.DateTime";
						align=HorizontalAlign.Center;
						format=" {0:dd/MMM/yyy} ";
						break;
					case "currency":
						colType = "System.Double";
						align=HorizontalAlign.Right;
						format=" {0:f2} ";
						break;
					case "integer":
						colType = "System.Int64";
						align=HorizontalAlign.Right;
						break;
					case "double":
						colType = "System.Double";
						align=HorizontalAlign.Right;
						format=" {0:f} ";
						break;
					case "select":
						colType = "System.String";
						align=HorizontalAlign.Center;
						format=String.Empty;
						break;
					default:
						colType = "System.String";
						align = HorizontalAlign.Left;
						format=String.Empty;
						break;
				}

				if (overrideFormat != String.Empty) 
				{
					format = "{0:"+overrideFormat+"}";
				}

				DataColumn dc = new DataColumn(caption, System.Type.GetType(colType));
				dt.Columns.Add(dc);

				// Get Options Data
				XmlNode optionsNode = el.SelectSingleNode("./options");
				if (optionsNode != null) 
				{
					ArrayList keyList;
					ArrayList textList;
					ArrayList orderList;	// has no effect for Data Tables
					ArrayList ignoreList;	// has no effect for Data Tables
					GetOptionsData(optionsNode, out keyList, out textList, out orderList, out ignoreList);
					keyLists.Add(keyList);
					textLists.Add(textList);
				} 
				else 
				{
					keyLists.Add(null);
					textLists.Add(null);
				}

				switch (type)
				{
					case "select":
						// Hyperlink column
						HyperLinkColumn selectColumn = new HyperLinkColumn();
						selectColumn.HeaderText = "&nbsp;" + caption + "&nbsp;";
						//Creates hyperlink under customer name(view reward group) to view customer detail
						if (StringUtils.IsStringEmpty(showValue))
							selectColumn.Text = linkText;
						else
							selectColumn.DataTextField = caption;

						selectColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
						selectColumn.ItemStyle.CssClass="gridrowlink";
						// Check to see if a specific link text has been supplied.
						//Creates hyperlink under customer name(view reward group) to view customer detail
						if (StringUtils.IsStringEmpty(showCrmid))
							selectColumn.DataNavigateUrlField = caption;
						else
							selectColumn.DataNavigateUrlField = showCrmid;

						selectColumn.DataNavigateUrlFormatString = href;
						if (size > -1)
							selectColumn.ItemStyle.Width = new Unit(size,unitType);
						dg.Columns.Add(selectColumn);
						break;
					case "conditionalselect":
						// Get the conditional xpath. This is used to determine whether
						// a link will placed into the row.
						string condXpath = GetAttributeString(el, "conditionalxpath");
						// A conditional select. Need to keep track of these in the array and
						// the 
						csInfo[columnNumber] = new string[] {linkText,href,condXpath,tip};
						// list, as checks need to be made later.
						BoundColumn condSelectColumn = new BoundColumn();
						condSelectColumn.HeaderText = "&nbsp;" + caption + "&nbsp;";
						condSelectColumn.DataField = caption;
						condSelectColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
						if (size > -1)
							condSelectColumn.ItemStyle.Width = new Unit(size,unitType);
						dg.Columns.Add(condSelectColumn);
						break;
					default:
						// Ordinary Databound column - Add it to the ASP datagrid
						BoundColumn normalColumn = new BoundColumn();
						normalColumn.HeaderText = "&nbsp;" + caption + "&nbsp;";
						normalColumn.DataField = caption;
						normalColumn.DataFormatString = format;
						normalColumn.ItemStyle.HorizontalAlign = align;
						if (size > -1)
							normalColumn.ItemStyle.Width = new Unit(size,unitType);
						dg.Columns.Add(normalColumn);
						break;
				}
				columnNumber++;
			}

			// If a height has been specified, then add the tablerow to the
			// table.
			if (truncate)
				headerTable.Rows.Add(headerRow);

			// Populate the DataTable from the data XML
			if (tableNodeList != null) 
			{
				foreach (XmlNode rowNode in tableNodeList) 
				{
					// Create a new data row
					DataRow dr = dt.NewRow();

					for (int i=0; i<xpathList.Count; i++) 
					{
						// Get the xpath
						string xp = (string)xpathList[i];
						// Locate the data
						XmlNode selectedNode = rowNode.SelectSingleNode(xp);
						// Get the data value, either from an element or an atribute
						string val = null;
						if (selectedNode != null) {
							val = GetNodeValue(selectedNode);
						}

						// Map the data onto the reference data.
						if (keyLists[i] != null) {
							ArrayList keyList = (ArrayList)keyLists[i];
							ArrayList textList = (ArrayList)textLists[i];
							int index = keyList.IndexOf(val);
							if (index != -1) {
								val = (string)textList[index];
							}
						}

						// See if this is a conditional select column.
						if (csInfo.ContainsKey(i))
						{
							// Get the conditional xpath. This determines whether
							// a link will be placed in the column.
							string condXpath = ((string[])csInfo[i])[2];
							XmlNode condNode = rowNode.SelectSingleNode(condXpath);
							string condValue = null;
							if (condNode != null)
								// The node exists, so get the value.
								condValue = GetNodeValue(condNode);
							// Inspect the value and determine whether a link will
							// be required.
							if (!StringUtils.IsStringEmpty(condValue))
							{
								// A link is required, so get the linktext and
								// the URL.
								string linkText = ((string[])csInfo[i])[0];
								string href = ((string[])csInfo[i])[1];
								string tip = ((string[])csInfo[i])[3];
								if (StringUtils.IsStringEmpty(tip))
									tip = String.Empty;
								// Replace the placeholder in the URL with the
								// data value.
								href = href.Replace("{0}",val);
								// Create the hyperlink.
								val = "<a id=\"datatableconditionalselect" + i.ToString() + "\" title=\"" +  tip +
									"\" href=\"" + href + "\">" + linkText + "</a>";
							}
							else
								// The condition has failed, so no link is required.
								val = null;
						}
						// Store the data in the DataTable
						string type = dt.Columns[i].DataType.ToString();
						if ((val!=null) && (val.Length>0)) 
						{
							try 
							{
								switch (type) 
								{
									case "System.Int64":
										dr[i] = Int64.Parse(val);
										break;
									case "System.Double":
										Double xx;
										Boolean tf;
										tf = Double.TryParse(val,System.Globalization.NumberStyles.Float,new CultureInfo("en-GB"),out xx);
										dr[i] = xx;
										break;
									case "System.DateTime":
										dr[i] = DateTime.Parse(val);
										break;
									default:
										dr[i] = val;
										break;
								}
							} 
							catch (Exception) 
							{
								dr[i] = DBNull.Value;
								dr[i] += "###";
							}
						} 
						else 
						{
							dr[i] = DBNull.Value;
						}
					}
					dt.Rows.Add(dr);
				}

				// Create a dataview, and apply any sorting required.
				DataView dv = new DataView(dt);

				// build a sort string in the format "LastName,Firstname DESC"
				string sortStr = String.Empty;

				for (int i=sortOrderArray.GetLowerBound(0); i<=sortOrderArray.GetUpperBound(0); i++) 
				{
					if ((sortOrderArray[i] != null) && (sortOrderArray[i].Length>0)) 
					{
						if (sortStr.Length > 0) 
						{
							sortStr += "," + sortOrderArray[i];
						} 
						else 
						{
							sortStr += sortOrderArray[i];
						}
					}
				}
				// apply the sort to the data view
				dv.Sort = sortStr;

				// Store the sort string in the Session.
				Page.Session[CacheTagPrefix + "DATASORT"] = sortStr;

				// Save the DataTable to the page, so that it is available
				// for multi-page tables. Cannot store the DataView using
				// SQLServer Session state management.
				Page.Session[CacheTagPrefix + "DATATABLE"] = dt;
				dg.DataSource = dv;
				dg.DataBind();
			} 
			else 
			{
				// There was no data, so display a message accordingly, using
				// a table structure.

			}
			// If there is no data, create an empty table to display a message.
			Table noDataTable = null;
			TableRow noDataRow = null;
			TableCell noDataCell =  null;
			if ((tableNodeList == null) || (tableNodeList.Count < 1))
			{
				// Create the table.
				noDataTable = new Table();
				// Add a style to the table.
				
				noDataTable.CssClass = "gridnodatatable";
				noDataTable.GridLines = GridLines.Both;
				noDataTable.Attributes["style"] = "table-layout:fixed;border-collapse:collapse";
				// Create a new row.
				noDataRow = new TableRow();
				noDataRow.CssClass = "gridnodatarow";
				// Create a new cell.
				noDataCell = new TableCell();
				if (numberColumns > 0)
					noDataCell.ColumnSpan = numberColumns;
				noDataCell.CssClass = "gridnodatacell";
	
				// Check if there has been a message set via the
				// property, as this will override the configuration
				// file setting.
				string noDataText = String.Empty;
				if (this.NoDataMessage == null)
				{
					// It hasn't been set, so attempt to get from
					// the configuration file.
						noDataText = BaseControl.GetAttributeString(steeringXml,"nodatamessage");
				}
				else 
					// It has been set, so use it.
					noDataText = this.NoDataMessage;
				noDataCell.Text = noDataText;
				noDataCell.HorizontalAlign = HorizontalAlign.Center;
				// Add the cell to the row.
				noDataRow.Cells.Add(noDataCell);
				// Add the row to the table.
				noDataTable.Rows.Add(noDataRow);				
			}

			// Check if a panel was created to enable scroll bars. If it was,
			// then need to add the datagrid as a child control.
			if (truncate)
			{
				// Check to see if there was any data.
				if ((tableNodeList == null) || (tableNodeList.Count < 1))
				{
					// There wasn't any data, so add the message to the
					// table saying so. 
					Table noDataFoundTable = new Table();
					noDataFoundTable.CellPadding=0;
					noDataFoundTable.CellSpacing=0;
					

					p.Controls.Add (noDataFoundTable);
					noDataFoundTable.Rows.Add(noDataRow);
				}
				// A height was specified, so add the header and datagrid to it.
				Controls.Add(headerTable);
				// Only add the datagrid if there is any data.
				if ((tableNodeList != null) && (tableNodeList.Count > 0))
				{
					p.Width = new Unit(1,UnitType.Pixel);
					p.Controls.Add(dg);
					// Now add the panel to the controls.
					Controls.Add(WrapControlInHtml(p,"controldatatable",this.ID));
				}
				else
					// Still want the height of the panel to be present
					p.CssClass = "nodatatablepanel";
					Controls.Add(p);
			}
			else
			{
				// Check if there was any data.
				if ((tableNodeList == null) || (tableNodeList.Count < 1))
					// There wasn't any data, so add the noDataTable to the
					// datagrid.
					dg.Controls.Add(noDataTable);
			
                // Now wrap the datagrid in SPAN tags.
				Controls.Add(WrapControlInHtml(dg,"controldatatable",this.ID));
			}	
			Trace.EndProc(trState);
		}
		#endregion
		
		#region Control Methods
		
		/// <summary>
		/// Overrides Control.CreateChildControls.
		/// Notifies this control to create any child nodes
		/// </summary>
		protected override void CreateChildControls() 
		{
			ITraceState trState = Trace.StartProc("DataTable.CreateChildControls");
			
			//Create the data table
			CreateTable();

			Trace.EndProc(trState);
		}

		private string GetNodeValue(XmlNode node)
		{
			string val = null;
			if (node.NodeType == XmlNodeType.Element) 
			{
				val = ((XmlElement)node).InnerText;
			} 
			else if (node.NodeType == XmlNodeType.Attribute) 
			{
				val = ((XmlAttribute)node).Value;
			}
			return val;
		}


		//--------------------------------------------------------
		// DataGridPageChange event handler
		// The user has changed the page of a multi-page grid.
		//
		// TODO : For some reason, this event never gets called. Fix it!
		//--------------------------------------------------------
		private void DataGridPageChange(Object sender, DataGridPageChangedEventArgs e) {
 
			// Set CurrentPageIndex to the page the user clicked.
			DataGrid itemsGrid = (DataGrid)sender;
			itemsGrid.CurrentPageIndex = e.NewPageIndex;

			// Rebind the data. 
			string sortStr = (string)Page.Session[CacheTagPrefix + "DATASORT"];
			System.Data.DataTable dt = (System.Data.DataTable)Page.Session[CacheTagPrefix + "DATATABLE"];
			DataView dv = new DataView(dt);
			if (!StringUtils.IsStringEmpty(sortStr))
				dv.Sort = sortStr;
			itemsGrid.DataSource = dv;
			itemsGrid.DataBind();
		}

		private void PrerenderEvent(Object sender, System.EventArgs e) {
			CrmServiceException ce = new CrmServiceException("Library", "Unknown", "Prerender", Trace, null);
			throw ce;
			//throw new Exception("prerender Exception");
		}
		#endregion

		//Addition of points partner

		#region DataGrid_ItemDataBound
		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			int j=-1;
			int k=-1;
			int l=-1;
			int m=-1;
			int p=-1;
			string conditionCellValue="";
			DataGrid myGrid=(DataGrid)sender;
			for(int i=0;i<myGrid.Columns.Count;i++)
			{
				if((this.AlternativeField!=null)&&(this.AlternativeField!="")) 
				{
					if(myGrid.Columns[i].HeaderText.IndexOf(this.AlternativeField)>-1)
					{
						j=i;
					}
					else if(myGrid.Columns[i].HeaderText.IndexOf(this.AlternateFieldCheck)>-1)
					{
						k=i;
					}
					else if(myGrid.Columns[i].HeaderText.IndexOf(this.AlternateValue)>-1)
					{
						l=i;
					}
				}
				if((this.ConditionalFieldName!=null)&&(this.ConditionalFieldName!="")) 
				{
					if(myGrid.Columns[i].HeaderText.IndexOf(this.ConditionalFieldName)>-1)
					{
						m=i;
					}
					else if(myGrid.Columns[i].HeaderText.IndexOf(this.ConditionFieldName)>-1)
					{
						p=i;
					}
				}
			}
			if((e.Item.ItemType!=ListItemType.Header)&&(e.Item.ItemType!=ListItemType.Footer))
			{
				if(k>-1)
				{
					if(e.Item.Cells[2].Text=="Agency")
					{
						if(l>-1 && j>-1)
						{
							HyperLink hl = new HyperLink(); 
							hl.Text = this.AlternativeLinkText;
							hl.NavigateUrl = this.AlternativeHref+this.idfieldName+"="+e.Item.Cells[l].Text; 
							e.Item.Cells[j].Controls.Clear();
							e.Item.Cells[j].Controls.Add(hl); 
						}
					}
					if(e.Item.Cells.Count>=k+2)
					{
						e.Item.Cells[k+1].Text="";
					}
				}
				if(p>-1)
				{
					conditionCellValue=e.Item.Cells[p].Text;
					conditionCellValue=conditionCellValue.Replace("&nbsp;","");
					if((conditionCellValue!="")&&(conditionCellValue!=null)) 
					{
						if(conditionCellValue!=this.ConditionValue)
						{
							if(m>-1 && e.Item.Cells.Count>=m+1)
							{
								e.Item.Cells[m].Controls.Clear();
							}
						}	
					}
				}
			}
		}
		#endregion
		//End of addition.
	}
}