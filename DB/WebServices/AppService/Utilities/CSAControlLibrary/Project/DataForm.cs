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
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate a data form from config files.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added grid syntax to data form</description>
	///		</version>
	///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	/// 	<version number="1.07" day="02" month="12" year="2002">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Added Validation summary popup to the dataform</description>
	///		</version>
	/// 	<version number="1.06" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Changed CreateChildControls to wrap control in a SPAN
	///			HTML control.</description>
	///		</version>
	/// 	<version number="1.05" day="11" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/024</work_packet>
	///			<description>Added new CssClass properties to tables.</description>
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
	///			DataForm. CreateChildControls() method uses the ConfigDoc property 
	///			instead of handling the ConfigFile directly.
	///			</description>
	///		</version>
	/// 	<version number="1.02" day="18" month="04" year="2002">
	///			<developer>GSB</developer>
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

	public class DataForm : ControlTable 
	{

		/// <summary>
		/// Create the main Form. 
		/// This comprises of a Header Row and a main section
		/// containing the Form Table. The Form Table contains
		/// one or more Column Tables.
		/// </summary>
		/// <returns></returns>
		private Control CreateMainTable( ) {

			ITraceState trState = Trace.StartProc("DataForm1.CreateMainTable");
			XmlElement configXmlElement = (XmlElement)this.ConfigXmlDoc.SelectSingleNode("descendant::form");
			if ( configXmlElement.SelectSingleNode("grid") != null)
			{
				// new style form config

				Control grid = this.CreateGrid(configXmlElement,true);
				Trace.EndProc(trState);
				return grid;


			}
			else
			{
				// the old style ( this can be deleted when we've converted all the old forms )
		
				XmlElement dataXmlElement = this.DataXmlElement;

				
				//this.LinkPage = Page;

				Table mainTable = new Table();
				mainTable.BorderWidth=1;
				mainTable.GridLines=GridLines.Both;
				mainTable.CssClass="formcontainertable";

				//mainTable.Width = Unit.Percentage(100);

				Table formTable = new Table();
				//formTable.Width = Unit.Percentage(100);
				formTable.GridLines = GridLines.None;
				formTable.BorderWidth = 0;
				formTable.CssClass = "formtable";

				TableRow formTableRow = new TableRow();
				formTableRow.VerticalAlign = VerticalAlign.Top;
				formTable.Rows.Add(formTableRow);


				// Display the Form Header (if any)
				XmlNodeList elemList = configXmlElement.GetElementsByTagName("header");
				if (elemList.Count>0) 
				{
					Trace.WriteDebug("Header = " + elemList[0].InnerXml);
					mainTable.Rows.Add(this.GenerateHeaderRow(elemList[0].InnerText ));
				}


				// Create a new table for each column within the form and
				// populate it
				elemList = configXmlElement.GetElementsByTagName("column");

				foreach (XmlElement elem in elemList) 
				{
					Table columnTable = this.CreateControlTable(elem, dataXmlElement);		
					TableCell formTableCell = new TableCell();
					formTableCell.Controls.Add(columnTable);
					formTableRow.Cells.Add(formTableCell);
				}

				// Add the FormTable to the bottom of the MainTable
				TableCell mainTableCell = new TableCell();
				mainTableCell.Controls.Add(formTable);
				ValidationSummaryControl vsc = new ValidationSummaryControl(mainTableCell.Controls,true);
				XmlNode validationSummaryMsg = configXmlElement.SelectSingleNode("//validationSummaryMsg/text()");
				if (validationSummaryMsg != null) {
					vsc.ValidationSummaryMsg = validationSummaryMsg.Value;
				}
				TableRow mainTableRow = new TableRow();
				mainTableRow.Cells.Add(mainTableCell);
				mainTable.Rows.Add(mainTableRow);

				Trace.EndProc(trState);
				return mainTable;
			}
		}
		/// <summary>
		/// Overrides Control.CreateChildControls.
		/// Notifies this control to create any child nodes
		/// </summary>
		protected override void CreateChildControls() 
		{
			ITraceState trState = Trace.StartProc("DataForm1.CreateChildControls");

			this.currentLocation = InClass.DataForm;

			// Create the dataform and wrap the control in SPAN tags.
			Controls.Add(WrapControlInHtml(CreateMainTable(),"controldataform",this.ID));
			// Also add the script to set the focus.
			if (this.SetFocus != null)
				Controls.Add(this.SetFocus);
			Trace.EndProc(trState);
		}
	}
}