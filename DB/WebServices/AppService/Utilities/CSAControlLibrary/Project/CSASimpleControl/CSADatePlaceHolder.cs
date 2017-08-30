#region Using
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.RegularExpressions;
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
	/// Returns a placeholder containing drop down lists of day, month and year.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Changed required field marker to use localization library.</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.02" day="11" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Add tool tip</description>
	///		</version>
	///		<version number="1.01" day="07" month="11" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Bug Fix</work_packet>
	///			<description>Problem with generated Java script using a prefix
	///			which is actually an xpath.</description>
	///		</version>
	///		<version number="1.00" day="18" month="01" year="2002">
	///			<developer>Gary Bleads</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion
	public class DatePlaceHolder : PlaceHolder, ISimpleControl 
	{
		private string parseFormat = "d/M/yyyy";

		#region Get Control Values
		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) 
		{

			if (this.yearList.Enabled) {
				string dateYear = this.yearList.SelectedItem.Text;
				string dateMonth = this.monthList.SelectedItem.Value;
				string dateDay = DATE_NULL_TEXT;

				if (forceStartOfMonth) {
					dateDay = "1";
				} else if (forceEndOfMonth) {
					try {
						DateTime dt = DateTime.ParseExact("1" + "/" + dateMonth + "/" + dateYear,parseFormat,null);
						int lastDayinMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
						dateDay = lastDayinMonth.ToString(); 
					} catch {}
				} else {
					dateDay = this.dayList.SelectedItem.Text;
				}

				if ((dateYear != DATE_NULL_TEXT) && (dateMonth != DATE_NULL_TEXT) && (dateDay != DATE_NULL_TEXT)) {
					try {
						DateTime ddt = DateTime.ParseExact(dateDay + "/" + dateMonth + "/" + dateYear,parseFormat,null);
						string dateStr = ddt.ToString("s");
						ht.Add(this.ID,dateStr);
					} catch {
						throw (new Exception(dateDay + "/" + dateMonth + "/" + dateYear));
					}
				} else {
					ht.Add(this.ID,null);
				}
			}
		}
		#endregion

		#region Declarations
		private static string VALIDATION_ERROR_TEXT = Localization.GetLocalizedAttributeString("ValidationErrorText");
		private const string DATE_NULL_TEXT = "---";
		private bool forceStartOfMonth;
		private bool forceEndOfMonth;
		System.Web.UI.WebControls.DropDownList yearList;
		System.Web.UI.WebControls.DropDownList monthList;
		System.Web.UI.WebControls.DropDownList dayList;
		#endregion

		#region Constructor
		/// <summary>
		/// Returns a control containing the date as dropdown lists (day,month,year)
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		/// <param name="forceStartOfMonth">if set, the day is always forced to 1st (useful for startdates of creditcards)</param>
		/// <param name="forceEndOfMonth">if set, the day is always forced to the last day in the month. It is silly to set both forceStart and forceEnd</param>
		public DatePlaceHolder(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement,
			bool forceStartOfMonth, bool forceEndOfMonth) {

			this.forceStartOfMonth = forceStartOfMonth;
			this.forceEndOfMonth = forceEndOfMonth;

			// Get Configuration Details
			string id, dataValue, caption,tip,format;
			parent.GetSettings(configXmlElement,dataXmlElement, out dataValue, out id, out caption,out tip,out format);
			string minValue = BaseControl.GetAttributeString(configXmlElement, "minvalue");
			string maxValue = BaseControl.GetAttributeString(configXmlElement, "maxvalue");
			bool required = BaseControl.GetAttributeBool(configXmlElement, "required");
			bool rdOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			
			String errorCaption;
			string errorCaptionStr = BaseControl.GetAttributeString(configXmlElement, "errorCaption");
			if (errorCaptionStr==String.Empty)
				errorCaption = caption;
			else
				errorCaption = errorCaptionStr;

			// Setup Control according to configuration details
			this.yearList = new System.Web.UI.WebControls.DropDownList();
			this.monthList = new System.Web.UI.WebControls.DropDownList();
			this.dayList = new System.Web.UI.WebControls.DropDownList();

			if (rdOnly) {
				this.yearList.Enabled = false;
				this.monthList.Enabled = false;
				this.dayList.Enabled = false;
				this.yearList.CssClass = "formreadonly";
				this.monthList.CssClass = "formreadonly";
				this.dayList.CssClass = "formreadonly";
			}
			this.ID = id;
			this.yearList.ID = id + "$year";
			this.monthList.ID = id + "$month";

			DateTime initialDate;
			string initDay = String.Empty;
			string initMonth = String.Empty;
			string initYear = String.Empty;
			if ((dataValue != null) && (dataValue.Length > 0)) {
				initialDate = this.ParseDate(dataValue);
				initDay = initialDate.Day.ToString();
				initMonth = initialDate.Month.ToString();
				initYear = initialDate.Year.ToString();
			}

			DateTime startDate = this.ParseDate(parent,minValue,"01/01/1900");
			DateTime endDate = this.ParseDate(parent,maxValue,"01/01/2015");
			if (forceStartOfMonth || forceEndOfMonth) {
				// Plant a hidden field on the form "id$day", with value of "1" or "end"
//				Literal dayLit = new Literal();
//				dayLit.ID = id + "$day";
//				dayLit.Visible = false;
//
//				if (forceStartOfMonth) {
//					dayLit.Text = "1";
//				} else {
//					dayLit.Text = "end";
//				}
//
//				this.Controls.Add(dayLit);

				// Force the day of the start and end dates to the beginning and 
				// end of the month so that the client-side validation scripts work properly.
				startDate = new DateTime(startDate.Year, startDate.Month, 1);
				endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
			} else {
				// Add a dropdown list to the form.
				this.dayList.ID = id + "$day";
			
				if (!required) {
					this.dayList.Items.Add(DATE_NULL_TEXT);
				}

				for (Int32 day=1; day<=31; day++) {
					this.dayList.Items.Add(day.ToString());
				}

				foreach (ListItem i in this.dayList.Items) {
					if (i.Text == initDay) {
						i.Selected = true;	
						break;
					}
				}
				this.Controls.Add(BaseControl.WrapControlInHtml(this.dayList,"controlplaceholderdaylist","simplecontrol" + id));
			}

			// Add a blank entry to allow "null" values to be selected if the field isn't mandatory
			if (!required) {
				this.monthList.Items.Add(DATE_NULL_TEXT);
			}
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("January"),"1"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("February"),"2"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("March"),"3"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("April"),"4"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("May"),"5"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("June"),"6"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("July"),"7"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("August"),"8"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("September"),"9"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("October"),"10"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("November"),"11"));
			this.monthList.Items.Add(new ListItem(Localization.GetLocalizedAttributeString("December"),"12"));

			// Select the initial Month
			foreach (ListItem i in this.monthList.Items) {
				if (i.Value == initMonth) {
					i.Selected = true;	
					break;
				}
			}
			if (!required) {
				this.yearList.Items.Add(DATE_NULL_TEXT);
			}
			for (Int32 year=startDate.Year; year<=endDate.Year; year++) {
				this.yearList.Items.Add(year.ToString());
			}

			// Select the initial Year
			foreach (ListItem i in this.yearList.Items) {
				if (i.Text == initYear) {
					i.Selected = true;	
					break;
				}
			}
			this.Controls.Add(BaseControl.WrapControlInHtml(this.monthList,"controlplaceholdermonthlist","simplecontrol" + id));
			this.Controls.Add(BaseControl.WrapControlInHtml(this.yearList,"controlplaceholderyearlist","simplecontrol" + id));

			// Create a little client-side validation function in JavaScript.
			Literal jscript = new Literal();
			// The id may have come back as an xpath. If this is the case, then
			// these characters need to be removed.
			Regex regex = new Regex("/");
			string newid = regex.Replace(id,"");

			string validationFunctionName = newid + "ClientValidator";
			jscript.Text = 
				"\n<script language=\"JavaScript\"><!--" +
				"\n  function " + validationFunctionName + "(objSource, objArgs) {";

			// If the StartDate is forced, set day to 1, otherwise get it from the dropdown list.
			if (forceStartOfMonth || forceEndOfMonth) {
				jscript.Text += 
					"\n    var dd = 1; ";
			} else {
				jscript.Text += 
					"\n    var dd = document.all[\"" + parent.ClientID + "_" + this.dayList.ClientID + "\"].value; ";
			}
			jscript.Text += 
				"\n    var mm = document.all[\"" + parent.ClientID + "_" + this.monthList.ClientID + "\"].value; " + 
				"\n    var yyyy = document.all[\"" + parent.ClientID + "_" + this.yearList.ClientID + "\"].value; " + 
				"\n    var ok = true" +
				"\n    var dat = new Date (yyyy,mm-1,dd); ";

			// jscript.Text += "\n\n    alert(dd + \"-\" + mm + \"-\" + yyyy);\n\n";

			// Allow null values if the field is not mandatory
			if (!required) {
				if (forceStartOfMonth || forceEndOfMonth) {
					jscript.Text += 
						"\n    if ((yyyy == \"" + DATE_NULL_TEXT + "\") && (mm == \"" + DATE_NULL_TEXT + "\"))" +
						"\n    {" + 
						"\n        objArgs.IsValid = true;" +	 
						"\n        return; " +
						"\n     };" ;
				} else {
					jscript.Text += 
						"\n    if ((yyyy == \"" + DATE_NULL_TEXT + "\") && (mm == \"" + DATE_NULL_TEXT + "\") && (dd == \"" + DATE_NULL_TEXT + "\"))" +
						"\n    {" + 
						"\n        objArgs.IsValid = true;" +	 
						"\n        return; " +
						"\n     };" ;
				}
			}

			jscript.Text +=
				"\n    if ((yyyy != dat.getFullYear()) || (mm-1 != dat.getMonth()) || (dd != dat.getDate())) {" +
				"\n      ok = false; " +
				"\n    } ";

			// Check that the date is not before the minimum.
			if (minValue.Length > 0) {
				jscript.Text +=
					"\n    if (dat < new Date(" + startDate.Year + "," + (startDate.Month-1) + "," + startDate.Day + ")) {" +
					"\n      ok = false;" +
					"\n    } ";
			}

			// Check that the date is not after the maximum.
			if (maxValue.Length > 0) {
				jscript.Text +=
					"\n    if (dat > new Date(" + endDate.Year + "," + (endDate.Month-1) + "," + endDate.Day + ")) {" +
					"\n      ok = false;" +
					"\n    } ";
			}

			jscript.Text += 
				"\n    objArgs.IsValid = ok;" +	 
				"\n    return;" +
				"\n  }" +
				"\n//--></script>";

			this.Controls.Add(jscript);


			// Add a custom validation control
			TableCell controlTableCell = new TableCell();
			CustomValidator validationCtrl = new CustomValidator();
			validationCtrl.ClientValidationFunction = validationFunctionName;
			validationCtrl.ControlToValidate = this.yearList.ID;
			validationCtrl.Text = VALIDATION_ERROR_TEXT;
			validationCtrl.ErrorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.IsInvalid",errorCaption);
			controlTableCell.Controls.Add(validationCtrl);
			controlTableRow.Cells.Add(controlTableCell);

			// Add Control's Caption to Page's Table Row
			SimpleControlConstruct.AddCaption(controlTableRow,controlTableCell,caption,tip,required);

			// Add Control to Page's Table Row
			controlTableCell = new TableCell();
			controlTableCell.Controls.Add(this);
			controlTableRow.Cells.Add(controlTableCell);

			//this.Controls.Add(validationCtrl);
		}
		#endregion

		#region Parse Date
		/// <summary>
		/// Returns a DataTime parsed from the supplied datestr string.
		/// DateStr may be formatted "25/DEC/2001" or "TODAY".
		///
		/// If datestr is empty, the defaultdate will be used instead.
		/// </summary>
		/// <param name="parent">The Page</param>
		/// <param name="dateStr"></param>
		/// <param name="defaultDateStr"></param>
		/// <returns></returns>
		private DateTime ParseDate(
			ControlTable parent, 
			string dateStr, 
			string defaultDateStr) {

			DateTime dat = new DateTime();
			string theDateStr;
			string culture = "";

			// Attempt to get the culture from the Session state
			// dictionary.
			try 
			{
				culture = (string)parent.Page.Session[WebSessionIndexes.Culture()];
			}
				// Do nothing here, as we don't won't to throw
				// an exception for this.
			catch {}
			// Check that the culture was available from the Session
			// state dictionary.
			if (StringUtils.IsStringEmpty(culture)) 
			{
				CrmServiceException ce = new CrmServiceException("Library",
					"Data","ControlLibrary.UnableToDetermineCulture",WebSessionIndexes.Culture());
				throw ce;
			}
			CultureInfo cultureInfo  = new CultureInfo(culture);
			Thread.CurrentThread.CurrentUICulture = cultureInfo;

			if (dateStr.Length==0) {
				theDateStr = defaultDateStr;
			} else {
				theDateStr = dateStr;
			}

			try {
				if (theDateStr.ToLower() == "today") {
					dat = DateTime.Today;
				} else {
					//dat = DateTime.Parse(theDateStr,new CultureInfo(CULTURE_CODE));
					dat = DateTime.Parse(theDateStr,Thread.CurrentThread.CurrentUICulture);
				}
			} catch (Exception e) {
				CrmServiceException ce = new CrmServiceException("Library", "Data", "DateStringParseFailure", e, theDateStr);
				throw ce;
			}

			return dat;
		}

		/// <summary>
		/// Returns a DataTime parsed from the supplied datestr string.
		/// DateStr may be formatted "25/DEC/2001" or "TODAY".
		///
		/// If datestr is empty, null will be returned.
		/// </summary>
		/// <param name="dateStr"></param>
		/// <returns></returns>
		private DateTime ParseDate(string dateStr) {
			DateTime dat = new DateTime();
			try {
				if ((dateStr==null) || (dateStr.Length==0) || (dateStr.ToLower()=="today")) {
					dat = DateTime.Today;
				} else {
					//dat = DateTime.Parse(dateStr, new CultureInfo(CULTURE_CODE));
					dat = DateTime.Parse(dateStr, Thread.CurrentThread.CurrentUICulture); 
				}			
			} catch (Exception) {
				CrmServiceException ce = new CrmServiceException("Library", "Data", "DateStringParseFailure", dateStr);
				throw ce;
			}

			return dat;
		}
		#endregion
	}
}