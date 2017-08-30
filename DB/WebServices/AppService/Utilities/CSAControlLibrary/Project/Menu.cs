#region Using
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text.RegularExpressions;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary 
{

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate asp from config files.
	/// </summary>
	/// 
	/// <development>
	///		<version number="1.12" day="23" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Moved CheckAndExtractWidth into common BaseControl class.</description>
	///		</version>
	///		<version number="1.11" day="22" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added functionality for vertical menus.</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.10" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip to menu items</description>
	///		</version>
	/// 	<version number="1.09" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed to wrap control in HTML SPAN tags.</description>
	///		</version>
	/// 	<version number="1.08" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed CssClass name to lowercase.</description>
	///		</version>
	/// 	<version number="1.07" day="01" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Bug Fix</work_packet>
	///			<description>Fixed problem with Xpath when selecting submenu.</description>
	///		</version>
	/// 	<version number="1.06" day="18" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>General enhancement.</work_packet>
	///			<description>Removed name of trace log from Trace constructor.</description>
	///		</version>
	/// 	<version number="1.05" day="18" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Tidy</work_packet>
	///			<description>Moved header to correct space to prevent warning messages.</description>
	///		</version>
	/// 	<version number="1.04" day="17" month="10" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/barcelona/015</work_packet>
	///			<description>
	///			Menu class inherits from CSAControl and gets a new, inherited property, ConfigFile, 
	///			to match the new control property. 
	///			(The old XmlFile property is retained for compatibility, mapped onto ConfigFile, but is deprecated).
	///			
	///			Menu uses XML data sets for configuration  rather than XML documents. 
	///			Change it to use an XML document to be in line with the classes and the new ConfigDoc property.
	///
	///			Menu.CreateChildControls() method uses the ConfigDoc property 
	///			instead of handling the ConfigFile directly.
	///			</description>
	///		</version>
	/// 	<version number="1.02" day="18" month="04" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker>Steve Lang</checker>
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

	public class Menu : BaseControl 
	{
		#region Attributes
		private Trace trace = new Trace();
		//private LocalizationLibrary ll = new LocalizationLibrary();
		private string caption = String.Empty;
		private string logo = String.Empty;
		private const int numberVerticalColumns = 2;
		#endregion

		#region Properties
		/// <summary>Deprecated - use ConfigFile instead</summary>
		public string XmlFilename {
			get	{ return this.ConfigFile; }
			set	{ this.ConfigFile = value; }
		}

		/// <summary>The title to appear in the menu bar</summary>
		public string Caption {
			get	{ return this.caption; }
			set	{ this.caption = value; }
		}

		/// <summary>filename of the Retailer's small Logo JPG or GIF.</summary>
		public string Logo {
			get	{ return this.logo; }
			set	{ this.logo = value; }
		}
		#endregion

		#region Build Menu

		#region In Sub Menu?
		/// <summary>
		///  Looks for a SubmenuItem with an URL that matches currentURL
		/// </summary>
		/// <param name="menuItem">MenuItem to search</param>
		/// <param name="currentURL">URL to seek</param>
		/// <returns>true if found</returns>
		private bool InSubmenu(XmlNode menuItem, string currentURL) 
		{
			// use xpath to find the sought element.
			return (menuItem.SelectSingleNode("SubmenuItem[URL='" + currentURL.ToLower() +"']") != null); 
		}
		#endregion

		#region Horizontal Menu
		/// <summary>
		/// Builds the HTML menu as defined in the ConfigFile
		/// </summary>
		private void BuildHorizontalMenu() 
		{
			ITraceState trState = this.trace.StartProc("Menu.BuildHorizontalMenu()");

			XmlElement configXmlElement = this.ConfigXmlDoc.DocumentElement;
			
			Table menuTable1 = new Table();
			Table menuTable2 = new Table();
			menuTable1.BorderWidth=0;
			menuTable1.CssClass="menutable1";
			menuTable1.Width = Unit.Percentage(100);

			menuTable2.BorderWidth=0;
			menuTable2.CssClass="menutable2";
			menuTable2.Width = Unit.Percentage(100);

			TableRow menuRow;
			TableCell menuCell;

			string info = String.Empty;
		
			// Get the URL of the current page
			string currentURL = Page.Request.ServerVariables["URL"];
			currentURL = currentURL.Substring(currentURL.LastIndexOf("/") + 1);

			// Blank spacer on the left hand side.
			menuCell = new TableCell();
			menuCell.CssClass = "menuitem";
			menuCell.Text = "&nbsp;";
			menuCell.Width = Unit.Pixel(20);
			menuRow = new TableRow();
			menuRow.Cells.Add(menuCell);

			// Add the top-level menu items
			XmlNodeList menuItemList = configXmlElement.SelectNodes("MenuItem");
			foreach (XmlNode menuItem in menuItemList) 
			{
				menuCell = new TableCell();
				menuCell.HorizontalAlign = HorizontalAlign.Center;
				string urlNode = BaseControl.GetAttributeString(menuItem,"URL");
				string nameNode = BaseControl.GetAttributeString(menuItem,"Name");
				string tipNode = BaseControl.GetAttributeString(menuItem,"tip");
				string infoNode = BaseControl.GetAttributeString(menuItem,"Info");
				// the ID of the hyperlink controls is based upon the unlocalised name.
				string idName = menuItem.SelectSingleNode("Name/text()").Value;

				this.trace.WriteDebug("menu item " + nameNode );
				
				// Decide the class names.
				string className;
				if ((currentURL.ToLower() == urlNode) || (this.InSubmenu(menuItem, currentURL))) 
				{
					// Selected (currently active) menu item - Highlight it in a different colour.
					menuCell.CssClass = "menuitemactive";
					className = "menuitemactivelink";
					info = infoNode ;
				} 
				else 
				{
					// Non-selected menu item
					menuCell.CssClass = "menuitem";
					className = "menuitemlink";
				}

				// Build the menu cell text.
				menuCell.Text = "&nbsp;&nbsp;<a id='" + idName + "' class='" + className + "' href='" + urlNode + "' " + 
					"title='"+ tipNode
					+"'>" + nameNode + "</a>&nbsp;&nbsp;";

				menuCell.Width = Unit.Pixel(160);
				menuRow.Cells.Add(menuCell);
			}
			

			// Blank spacer on the right hand side.
			menuCell = new TableCell();
			menuCell.CssClass = "menuitem";
			menuCell.Text = "&nbsp;";
			menuRow.Cells.Add(menuCell);

			menuTable1.Rows.Add(menuRow);
			Controls.Add(WrapControlInHtml(menuTable1,"controlmenu",this.ID));

			// Add a second table containing the page title and logo.
			menuRow = new TableRow();

			// title
			menuCell = new TableCell();
			menuCell.Style.Add("Width", "100%");
			menuCell.HorizontalAlign = HorizontalAlign.Center;
			menuCell.CssClass = "menuitemactive";
			if (Caption.Length > 0) {
				menuCell.Text = Caption;
			} else {
				menuCell.Text = info;
			}
			menuRow.Cells.Add(menuCell);

			// add the company logo
			menuCell = new TableCell();
			menuCell.Style.Add("Width", "0");
			menuCell.HorizontalAlign = HorizontalAlign.Right;
			menuCell.CssClass = "menuitemactive";
			string logoFilename = Localization.GetLocalizedImageString(logo);
			menuCell.Text = "<img id=\"menulogo\" class=\"menulogo\" src='" + logoFilename + "'/>";
			menuRow.Cells.Add(menuCell);
			menuTable2.Rows.Add(menuRow);

			// Add the submenu (if any) to the second table
			foreach (XmlNode menuItem in configXmlElement.SelectNodes("MenuItem"))
			{
				XmlNode urlNode = menuItem.SelectSingleNode("URL/text()");

				if ((currentURL.ToLower() == urlNode.Value) || (this.InSubmenu(menuItem, currentURL))) 
				{
					string submenuText = this.BuildHorizontalSubMenu(menuItem, currentURL);
					if (submenuText.Length > 0)
					{
						menuRow = new TableRow();
						menuCell = new TableCell();
						menuCell.CssClass = "menuitemactive";
						menuCell.ColumnSpan = 2;
						menuCell.Text = "<hr size=1 noshade color='black'/>" + submenuText;
						menuRow.Cells.Add(menuCell);
						menuTable2.Rows.Add(menuRow);
					}
				}
			}

			Controls.Add(WrapControlInHtml(menuTable2,"controlmenu",this.ID));

			this.trace.EndProc(trState);
		}
		#endregion

		#region Horizontal Sub Menu
		/// <summary>
		/// Builds an HTML submenu of the menuItem
		/// </summary>
		/// <param name="menuItem">owner of the submenu items</param>
		/// <param name="currentURL">determines whether the menu item is displayed as the active one or not</param>
		/// <returns>
		///</returns>
		private string BuildHorizontalSubMenu(XmlNode menuItem, string currentURL) 
		{
	
			ITraceState trState = this.trace.StartProc("Menu.BuildHorizontalSubMenu()");
			
			XmlNodeList submenus = menuItem.SelectNodes("SubmenuItem[URL]");
			String txt = String.Empty;

			foreach (XmlNode submenu in submenus)
			{
				// Get the node values.
				string urlNodeValue = BaseControl.GetAttributeString(submenu,"URL");
				string tipNodeValue = BaseControl.GetAttributeString(submenu,"tip");
				string nameNodeValue = BaseControl.GetAttributeString(submenu,"Name");
				bool newlineAtt = BaseControl.GetAttributeBool(submenu,"Newline");
				// the ID of the hyperlink controls is based upon the unlocalised name.
				string idName = submenu.SelectSingleNode("Name/text()").Value;

				this.trace.WriteDebug("sub menu item " + nameNodeValue );

				// Determine the classname of the menu item.
				string className;
				if (currentURL.ToLower() == urlNodeValue.ToLower()) 
					className = "submenuitemactivelink";
				else 
					className = "submenuitemlink";
		
				// Create the menu item now the class is decided.
				txt = txt + "&nbsp;&nbsp;<a id='" + idName + "' class='" + className  + "' href='" + urlNodeValue + "' title='" +
					tipNodeValue + "'>" + nameNodeValue + "</a>&nbsp;&nbsp;";

				// if the xml includes a "Newline" attribute, force a line break
				if (newlineAtt ) 
					txt = txt + "<br />";
			};

			this.trace.EndProc(trState);
			return txt;
		}
		#endregion

		#region Vertical Menu
		/// <summary>
		/// Builds a vertical menu bar as defined in the configuration file.
		/// </summary>
		private void BuildVerticalMenu()
		{
			ITraceState trState = this.trace.StartProc("Menu.BuildVerticalMenu()");
			// Get the configuration file.
			XmlElement configXmlElement = this.ConfigXmlDoc.DocumentElement;
			// Get the URL of the current page
			string currentUrl = Page.Request.ServerVariables["URL"];
			currentUrl = currentUrl.Substring(currentUrl.LastIndexOf("/") + 1);
			this.trace.WriteDebug("Current URL is '" + currentUrl + "'");
			// Create the table for the menu.
			Table menuTable = new Table();
			// Set the class of the table.
			menuTable.CssClass = "menutable";
			menuTable.Style.Clear();
			// The table is made up of two columns. The widths for the columns
			// or the overall width of the table can be specified. Check to see
			// if these have been supplied.
			string tableWidthNode = BaseControl.GetAttributeString( configXmlElement,"tablewidth");
			int tableWidth = -1;
			UnitType unitType = UnitType.Pixel;
			if (tableWidthNode != string.Empty )
				// Get the width
				tableWidth = CheckAndExtractDimension(tableWidthNode,"tablewidth",out unitType);
			// See if the table width needs to be set.
			if (tableWidth > -1)
				// It does need to be set, so do it.
				menuTable.Width = new Unit(tableWidth,unitType);
			// Attempt to no get the row height, if there is one.
			string rowHeightNode = BaseControl.GetAttributeString(configXmlElement,"rowheight");
			int rowHeight = -1;
			UnitType rowHeightUnitType = UnitType.Pixel;
			if (rowHeightNode != string.Empty )
				// Get the height
				rowHeight = CheckAndExtractDimension(rowHeightNode,"rowheight",out rowHeightUnitType);
			// Attempt to now get the column widths.
			ArrayList columnWidths = new ArrayList();
			ArrayList columnUnits = new ArrayList();
			// Loop round the number of columns and attempt to obtain.
			int i;
			for (i = 1; i <= numberVerticalColumns; i++)
			{
				// Derive the name of the column.
				string columnName = "col" + i.ToString() + "width";
				// Get the inner text of this element, if it exists.
				string columnWidth = BaseControl.GetAttributeString(configXmlElement,columnName);
				// Get the actual width of the column, if it exists.
				if (columnWidth != String.Empty) 
				{
					// Add the width and the unit type to the array lists.
					columnWidths.Add(CheckAndExtractDimension(BaseControl.GetAttributeString(configXmlElement,columnName),columnName,out unitType));
					columnUnits.Add(unitType.ToString());
				} 
				else
				{
					// There was no element for this column, so set the width to
					// an arbitary value.
					columnWidths.Add(-1);
					columnUnits.Add(String.Empty);
				}
			}
			// Place the logo at the top of the table. Create a new
			// image object and assign the logo to it.
			Image image = new Image();
			// Set the URL of the image if it exists.
			if (!StringUtils.IsStringEmpty(this.Logo))
				image.ImageUrl = Localization.GetLocalizedImageString(this.Logo);
			image.CssClass = "menuimage";
			image.ID = "menuimage";
			// Create a row and cell for this logo.
			TableRow menuRow = CreateTableRow(rowHeight,rowHeightUnitType);
			menuRow.CssClass = "menulogorow logo";
			TableCell menuCell = CreateTableCell(rowHeight,rowHeightUnitType,numberVerticalColumns,-1);
			menuCell.CssClass = "menucell";
			// Add the image to the table cell.
			menuCell.Controls.Add(image);
			// Add the cell to the row.
			menuRow.Cells.Add(menuCell);
			// Add the row to the table.
			menuTable.Rows.Add(menuRow);
			// Now add the menu items and sub menu items to the table.
			foreach (XmlNode menuItem in configXmlElement.SelectNodes("MenuItem"))
			{
				// Get any menu settings.
				string urlNode = BaseControl.GetAttributeString(menuItem,"URL");

				string name =BaseControl.GetAttributeString(menuItem,"Name");
				string tipNode = BaseControl.GetAttributeString(menuItem,"tip");
				string infoNode = BaseControl.GetAttributeString(menuItem,"Info");
				// the stylesheet class is based on on the unlocalised name;
				string cssClass = menuItem.SelectSingleNode("Name/text()").Value;
				// Do some tracing stuff for debugging.
				this.trace.WriteDebug("Menu item is '" + name + "'");
				// Check to see if a url has been supplied for this.
				string menuItemUrl = String.Empty;
				bool currentPage = false;
				if (urlNode != null)
				{
					// A url has been supplied, so get it and determine whether
					// it is the current page.
					menuItemUrl = urlNode;
					if ((currentUrl.ToLower() == name) || (this.InSubmenu(menuItem,currentUrl)))
						currentPage = true;
					else
						currentPage = false;
				}
				// Create the new table row.
				menuRow = CreateTableRow(rowHeight,rowHeightUnitType);
				if (currentPage)
					menuRow.CssClass = "menurow active " + ReplaceSpacesWithUnderscore(cssClass);
				else
					menuRow.CssClass = "menurow " + ReplaceSpacesWithUnderscore(cssClass);
				// Each menu item will span all of the columns that make up
				// a row. Create the new cell and span it.
				menuCell = CreateTableCell(rowHeight,rowHeightUnitType,numberVerticalColumns,-1);
				menuCell.CssClass = "menucell";
				if (StringUtils.IsStringEmpty(menuItemUrl)) 
				{
					// Add the text to the cell.
					menuCell.Text = name.Replace(" ","&nbsp;");
				}
				else
				{
					// Add the text to the cell as a hyperlink.
					HyperLink hyperLink = new HyperLink();
					hyperLink.NavigateUrl = urlNode;
					hyperLink.ID = ReplaceSpacesWithUnderscore(cssClass);
					hyperLink.Text = name.Replace(" ","&nbsp;");
					hyperLink.CssClass = "menulink";
					// Add the hyperlink to the cell.
					menuCell.Controls.Add(hyperLink);
				}
				// Add the tooltip.
				menuCell.ToolTip = tipNode;
				// Add the cell to the row.
				menuRow.Cells.Add(menuCell);
				// Add the row to the table.
				menuTable.Rows.Add(menuRow);
				// Collate the information about the sub menu items. This has to be done
				// first to find out how many rows will be occupied. This enables the 
				// row span of column 1 to be set.
				ArrayList subMenuUrls = new ArrayList();
				ArrayList subMenuNames = new ArrayList();
				ArrayList subMenuClasses = new ArrayList();
				ArrayList subMenuTips = new ArrayList();
				ArrayList subMenuNewLines = new ArrayList();
				foreach (XmlNode subMenuItem in menuItem.SelectNodes("SubmenuItem"))
				{
					// Get the potential elements for a sub menu.
					subMenuUrls.Add(BaseControl.GetAttributeString (subMenuItem,"URL"));
					subMenuNames.Add(BaseControl.GetAttributeString(subMenuItem,"Name"));
					subMenuClasses.Add(subMenuItem.SelectSingleNode("Name/text()").Value);// don't localise
					subMenuTips.Add(BaseControl.GetAttributeString(subMenuItem,"tip"));
					// Check to see if a newline has been specified.
                    XmlNode newLine = subMenuItem.SelectSingleNode("Newline");
					if (newLine == null)
						// No newline required.
						subMenuNewLines.Add(false);
					else
						// A newline is required.
						subMenuNewLines.Add(true);
				}
				// Now build the sub menu structure.
				ArrayList subMenuRows = this.BuildVerticalSubMenu(subMenuUrls,subMenuNames,subMenuClasses,subMenuTips,
					subMenuNewLines,columnWidths,columnUnits,rowHeight,rowHeightUnitType,currentUrl,ReplaceSpacesWithUnderscore(cssClass));
				// Loop round and add rows to the table.
				foreach(TableRow subMenuRow in subMenuRows)
				{
					menuTable.Rows.Add(subMenuRow);
				}
			}

			Controls.Add(WrapControlInHtml(menuTable,"controlmenu",this.ID));
			this.trace.EndProc(trState);
		}
		#endregion

		#region Vertical Sub Menu
		/// <summary>
		/// Build an ArrayList containing a collection of TableRows that make up
		/// a sub-menu group.
		/// </summary>
		/// <param name="subMenuUrls">An ArrayList containing the list of the URLs of
		/// each of the sub-menu item. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="subMenuNames">An ArrayList containing the list of names of
		/// each of the sub-menu item. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="subMenuClasses">An ArrayList containing the list of stylesheet classes of
		/// each of the sub-menu item. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="subMenuTips">An ArrayList containing the list of tips of
		/// each sub-menu item. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="subMenuNewLines">An ArrayList containing the list of newlines
		/// for each sub-menu item. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="columnWidths">An ArrayList containing the column widths of the
		/// row. If a element is not present for a particular sub-menu item, then the
		/// ArrayList will contain an empty string.</param>
		/// <param name="columnWidthUnits">An ArrayList containing the units that each of the
		/// column widths are defined in. If a element is not present for a particular
		/// sub-menu item, then the ArrayList will contain an empty string.</param>
		/// <param name="rowHeight">The height of each row.</param>
		/// <param name="rowHeightUnitType">The UnitType that the row height has been
		/// supplied in.</param>
		/// <param name="currentUrl">The current URL of the page.</param>
		/// <param name="menuItemName">The name of the menu item</param>
		/// <returns></returns>
		private ArrayList BuildVerticalSubMenu(
			ArrayList subMenuUrls,
			ArrayList subMenuNames,
			ArrayList subMenuClasses,
			ArrayList subMenuTips,
			ArrayList subMenuNewLines,
			ArrayList columnWidths,
			ArrayList columnWidthUnits,
			int rowHeight,
			UnitType rowHeightUnitType,
			string currentUrl,
			string menuItemName
			)
		{
			ITraceState trState = this.trace.StartProc("Menu.BuildVerticalSubMenu()");
			ArrayList subMenuRows = new ArrayList();
			// Loop round the arraylists and build each of the rows.
			bool firstRow = true;
			int i;
			for (i = 0; i < subMenuUrls.Count; i++)
			{
				// Determine whether this is the current page.
				bool currentPage = false;
				// Get the sub-menu item values.
				string url = (string)subMenuUrls[i];
				string name = (string)subMenuNames[i];
				string lName = name;
				string cssClass = (string)subMenuClasses[i];
				string tip = (string)subMenuTips[i];
				string lTip = tip;
				// We are always one behind with the newline because
				// when specified, it applied to the next item.
				bool newLine = false;
				if (!firstRow)
					newLine = (bool)subMenuNewLines[i-1];
				if (currentUrl == url)
					currentPage = true;
				this.trace.WriteDebug("Submenu item. url=" + url + " name=" + name + " tip=" +
					tip + " newline=" + newLine.ToString());
				// Create a row and cell for this logo.
				TableRow menuRow = CreateTableRow(rowHeight,rowHeightUnitType);
				if (currentPage)
					menuRow.CssClass = "submenurow active " + ReplaceSpacesWithUnderscore(cssClass) + 
										" sub_" + menuItemName;
				else
					menuRow.CssClass = "submenurow " + ReplaceSpacesWithUnderscore(cssClass) +
										" sub_" + menuItemName;
				// If a newline was required, then we'll have to add this TableRow
				// to the collection.
				TableCell menuCell = null;
				if (newLine) 
				{
					// A newline is required, so add some cells to the row and
					// then add the row to the collection.
					TableRow nlRow = CreateTableRow(rowHeight,rowHeightUnitType);
					nlRow.CssClass = "submenurow newline";
					TableCell nlCell = CreateTableCell(rowHeight,rowHeightUnitType,numberVerticalColumns,-1);
					nlCell.CssClass = "submenucell";
					nlRow.Cells.Add(nlCell);
					subMenuRows.Add(nlRow);
				}
				// Loop round and create all the columns.
				int j;
				for (j = 0; j < columnWidths.Count; j++)
				{
					// Get the values of the widths.
					int width = (int)columnWidths[j];
					// Check if we need to get the unittype.
					UnitType unit = UnitType.Pixel;
					if (width > -1)
					{
						switch((string)columnWidthUnits[j])
						{
							case "Percentage":
								unit = UnitType.Percentage;
								break;
							case "Pixel":
								unit = UnitType.Pixel;
								break;
							default:
								unit = UnitType.Pixel;
								break;
						}
					}
					// Use a switch to easily assign usage of each column.
					bool skipAdd = false;
					switch(j)
					{
						case 0:
							// This is the first column, which is highlighted
							// when a the sub-menu item is active. Create the
							// cell.
							menuCell = CreateTableCell(rowHeight,rowHeightUnitType,width,unit);
							menuCell.CssClass = "submenucell1";
							menuCell.Text="&nbsp;";
							break;
						case 1:
							// This is the second column which contains the name
							// of the sub-menu item.
							menuCell = CreateTableCell(rowHeight,rowHeightUnitType,width,unit);
							menuCell.CssClass = "submenucell2" + (currentPage?" active":" inactive");
							// Create the hyperlink.
							HyperLink hyperLink = new HyperLink();
							hyperLink.NavigateUrl = url;
							hyperLink.ID = ReplaceSpacesWithUnderscore(cssClass);
							hyperLink.Text = lName.Replace(" ","&nbsp;");
							// Check if a tip is required.
							if (!StringUtils.IsStringEmpty(tip))
								// A tip is required.
								hyperLink.ToolTip = lTip;
							// Add the hyperlink to the cell.
							menuCell.Controls.Add(hyperLink);
							hyperLink.CssClass = "submenulink ";
							break;
						default:
							CrmServiceException ce = new CrmServiceException("Library",
								"Data","CSAControlLibrary.ColumnNotBeingUsed",j.ToString());
							throw ce;
					}
					// Add the TableCell to the TableRow. Check if this is needed.
					if (!skipAdd) 
						menuRow.Cells.Add(menuCell);
				}
				// This row is finished, so add to the ArrayList.
				subMenuRows.Add(menuRow);
				// Set the flag, so the span cell is not processed for the rest of
				// the sub-menu items.
				firstRow = false;
			}
			this.trace.EndProc(trState);
			return subMenuRows;
		}
		#endregion

		#endregion

		#region Helper Methods
		/// <summary>
		/// Replace all occurences of spaces with an underscore. Also make all
		/// lowercase.
		/// </summary>
		/// <param name="replaceString">The string to be checked and amended.</param>
		/// <returns>The string with all spaces replaced with an underscore.</returns>
		private static string ReplaceSpacesWithUnderscore(string replaceString)
		{
			return replaceString.Replace(" ","_").Replace(".","_").ToLower();
		}

		/// <summary>
		/// Create a table row, checking if particular attributes
		/// need to be set.
		/// </summary>
		/// <param name="height">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="heightUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <returns></returns>
		private static TableRow CreateTableRow(
			int height,
			UnitType heightUnitType
			)
		{
			TableRow tableRow = new TableRow();
			if (height > -1) 
				tableRow.Height = new Unit(height,heightUnitType);
			return tableRow;
		}

		/// <summary>
		/// Create a table cell, checking if particular attributes need
		/// to be set.
		/// </summary>
		/// <param name="cellHeight">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellHeightUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <param name="cellWidth">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellWidthUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <param name="cellColumnSpan">The number of columns to span. Supplying -1
		/// causes no span to be set.</param>
		/// <param name="cellRowSpan">The number of rows to span. Supplying -1
		/// causes no span to be set.</param>
		/// <returns>The new TableCell with the supplied attributes.</returns>
		private static TableCell CreateTableCell(
			int cellHeight,
			UnitType cellHeightUnitType,
			int cellWidth,
			UnitType cellWidthUnitType,
			int cellColumnSpan,
			int cellRowSpan
			)
		{
			TableCell tableCell = new TableCell();
			if (cellHeight > -1) 
				tableCell.Height = new Unit(cellHeight,cellHeightUnitType);
			if (cellWidth > -1) 
				tableCell.Width = new Unit(cellWidth,cellWidthUnitType);
			if (cellColumnSpan > -1)
				tableCell.ColumnSpan = cellColumnSpan;
			if (cellRowSpan > -1)
				tableCell.RowSpan = cellRowSpan;
			return tableCell;
		}

		/// <summary>
		/// Create a table cell, checking if particular attributes need
		/// to be set.
		/// </summary>
		/// <param name="cellHeight">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellHeightUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <param name="cellWidth">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellWidthUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <returns>The new TableCell with the supplied attributes.</returns>
		private static TableCell CreateTableCell(
			int cellHeight,
			UnitType cellHeightUnitType,
			int cellWidth,
			UnitType cellWidthUnitType
			)
		{
			TableCell tableCell = new TableCell();
			if (cellHeight > -1) 
				tableCell.Height = new Unit(cellHeight,cellHeightUnitType);
			if (cellWidth > -1) 
				tableCell.Width = new Unit(cellWidth,cellWidthUnitType);
			return tableCell;
		}

		/// <summary>
		/// Create a table cell, checking if particular attributes need
		/// to be set.
		/// </summary>
		/// <param name="cellHeight">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellHeightUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <param name="cellColumnSpan">The number of columns to span. Supplying -1
		/// causes no span to be set.</param>
		/// <param name="cellRowSpan">The number of rows to span. Supplying -1
		/// causes no span to be set.</param>
		/// <returns>The new TableCell with the supplied attributes.</returns>
		private static TableCell CreateTableCell(
			int cellHeight,
			UnitType cellHeightUnitType,
			int cellColumnSpan,
			int cellRowSpan
			)
		{
			return CreateTableCell(cellHeight,cellHeightUnitType,-1,UnitType.Pixel,cellColumnSpan,cellRowSpan);
		}

		/// <summary>
		/// Create a table cell, checking if particular attributes need
		/// to be set.
		/// </summary>
		/// <param name="cellHeight">The height of the TableRow. Supplying -1
		/// causes no height to be set.</param>
		/// <param name="cellHeightUnitType">The UnitType to use to set the height of
		/// the TableRow.</param>
		/// <param name="cellRowSpan">The number of rows to span. Supplying -1
		/// causes no span to be set.</param>
		/// <returns>The new TableCell with the supplied attributes.</returns>
		private static TableCell CreateTableCell(
			int cellHeight,
			UnitType cellHeightUnitType,
			int cellRowSpan
			)
		{
			return CreateTableCell(cellHeight,cellHeightUnitType,-1,UnitType.Pixel,-1,cellRowSpan);
		}

		/// <summary>
		/// Get the inner text of a particular node.
		/// </summary>
		/// <param name="element">The top XmlElement of the configuration file.</param>
		/// <param name="elementName">The name of the element to have the inner text extracted.</param>
		/// <returns>The inner text of the required element.</returns>
		private static string GetInnerText(
			XmlElement element,
			string elementName
			)
		{
			XmlNode node = element.SelectSingleNode("//" + elementName + "/text()");
			string text = String.Empty;
			if (node != null)
			{
				text = node.Value;
			}
			return text;
		}

		/// <summary>
		/// Get the text of a particular node.
		/// </summary>
		/// <param name="node">The starting XML node for the search.</param>
		/// <param name="nodeName">The name of the element to have the text extracted.</param>
		/// <returns>The text value of the required element.</returns>
		private static string GetInnerText(
			XmlNode node,
			string nodeName
			)
		{
			XmlNode requiredNode = node.SelectSingleNode(nodeName + "/text()");
			string text = String.Empty;
			if (requiredNode != null)
			{
				text = requiredNode.Value;
			}
			return text;
		}
		#endregion

		#region Control Methods
		/// <summary>
		/// Overrides Control.CreateChildControls.
		/// Notifies this control to create any child nodes
		/// </summary>
		protected override void CreateChildControls() 
		{
			ITraceState trState = this.trace.StartProc("Menu.CreateChildControls()");
			// Create the main menu. Need to determine whether this is a
			// vertical or horizontal menu. Look for an 'alignment' element.
			XmlElement configXmlElement = this.ConfigXmlDoc.DocumentElement;
			string alignmentNode = BaseControl.GetAttributeString(configXmlElement,"alignment");
			string menuType = String.Empty;
			if (alignmentNode != string.Empty )
			{
				// An alignment element has been specified. Check that the
				// content is correct i.e. vertical or horizontal
				string alignmentContent = alignmentNode.ToLower();
				switch (alignmentContent)
				{
					case "vertical":
						// A vertical menu is required.
						this.BuildVerticalMenu();
						break;
					case "horizontal":
						// A horizontal menu is required, so call the method.
						this.BuildHorizontalMenu();
						break;
					default:
						CrmServiceException ce = new CrmServiceException("Library",
							"Data","CSAControlLibrary.InvalidElementContent",alignmentContent,"alignment");
						throw ce;
				}
			} 
			else 
				// No alignment element has been supplied, so default to
				// a horizontal type.
				this.BuildHorizontalMenu();
			this.trace.EndProc(trState);
		}
		#endregion
	}
}