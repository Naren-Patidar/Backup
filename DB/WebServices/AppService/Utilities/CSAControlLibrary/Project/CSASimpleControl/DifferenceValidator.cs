using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Threading;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.LocalizationLibrary;

namespace Fujitsu.eCrm.Generic.ControlLibrary
{
	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Validates the difference between a control value and either another control value or a data xml value
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	/// 	<version number="1.03" day="02" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>Moved FindControlById to CSABasicControlConstruct</description>
	///		</version>
	/// 	<version number="1.02" day="25" month="11" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>New class</description>
	///		</version>
	/// </development>
	#endregion

	public class DifferenceValidator : CompareValidator
	{
		#region constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public DifferenceValidator()
		{
			this.Load += new EventHandler(this.DifferenceValidationLoad);
			this.PreRender += new EventHandler(this.DifferenceValidationPreRender);
			
		}
		#endregion

		#region pre render event handler
		/// <summary>
		/// remembers the original value of control to compare
		/// </summary>
		protected string origControlToCompare;
		/// <summary>
		/// remembers the original value of value to compare
		/// </summary>
		protected string origValueToCompare;
		/// <summary>
		/// remembers the original value of operator
		/// </summary>
		protected ValidationCompareOperator origOperator;
		/// <summary>
		/// event handler, which occurs after client side validation, to  restore the original values
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DifferenceValidationPreRender(object sender, EventArgs e) 
		{
			DifferenceValidator diffValidator = (DifferenceValidator) sender;
			if (diffValidator.origControlToCompare != null)
			{
				diffValidator.ControlToCompare = diffValidator.origControlToCompare;
				diffValidator.ValueToCompare  = diffValidator.origValueToCompare;
				diffValidator.Operator  = diffValidator.origOperator;
			}
		}
		#endregion

		#region load event handler
		/// <summary>
		/// Event handler to perform difference calculation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DifferenceValidationLoad(object sender, EventArgs e) 
		{
			DifferenceValidator diffValidator = (DifferenceValidator) sender;
			string compareToXpath = diffValidator.ControlToCompare;
			if(!compareToXpath.Equals(String.Empty))
			{
				string compareVal = diffValidator.ValueToCompare;
				Control controlToCompare = SimpleControl.SimpleControlConstruct.FindControlById(Page,compareToXpath);
				string compareText;
				if (controlToCompare != null)
				{
					// there is a control on the form to compare against				
					compareText =   ((TextBox)controlToCompare).Text;
					// There is no client script to perform difference validation so leave it to the server
					diffValidator.EnableClientScript = false;

					// remember the original values to restore them after the validation
					diffValidator.origControlToCompare=compareToXpath;
					diffValidator.origValueToCompare = compareVal;
					diffValidator.origOperator = diffValidator.Operator;

				}
				else
				{
					// find the value from the data xml using the xpath in controlToCompare
					XmlNode  dataNode = ControlTable.GetDataNode( this.DataForm.XmlDoc.DocumentElement ,ControlTable.GetActualXPath(compareToXpath));
					compareText = dataNode.Value;	
				}
				try 
				{	string errMsgVal =  String.Empty;
					switch(diffValidator.Type)
					{
						case ValidationDataType.Date:
							DateTimeFormatInfo dfi = new DateTimeFormatInfo();
							dfi.ShortDatePattern = "yyyy-MM-dd";
							DateTime diffDate = DateTime.ParseExact(compareText,"d",dfi,DateTimeStyles.AllowWhiteSpaces );
							int unitStart = -1;
							
							if ((unitStart = compareVal.ToLower().IndexOf("month")) >= 0 )
							{
								diffDate = diffDate.AddMonths(int.Parse(compareVal.Substring(0,unitStart)));
							}
							else if ((unitStart = compareVal.ToLower().IndexOf("year")) >= 0 )
							{
								diffDate = diffDate.AddYears(int.Parse(compareVal.Substring(0,unitStart)));

							}
							else if ((unitStart = compareVal.ToLower().IndexOf("day")) >= 0 )
							{
								diffDate = diffDate.AddDays(int.Parse(compareVal.Substring(0,unitStart)));

							}
							else
							{
								// time in format [-]d.hh:mm:ss
								diffDate = diffDate.Add(TimeSpan.Parse(compareVal));
							}

						
							diffValidator.ValueToCompare = diffDate.ToString().Substring(0,10);
							errMsgVal = diffDate.ToString("d",dfi);
							break;
						case  ValidationDataType.Double:
							double diffDouble = double.Parse(compareText) + double.Parse(compareVal);
							diffValidator.ValueToCompare = diffDouble.ToString();
							errMsgVal = diffValidator.ValueToCompare;
							break;
						case ValidationDataType.Currency:
							decimal diffCurrency = decimal.Parse((decimal.Parse(compareText)).ToString("C",Thread.CurrentThread.CurrentUICulture),NumberStyles.Currency,Thread.CurrentThread.CurrentUICulture) 
													+ decimal.Parse((decimal.Parse(compareVal)).ToString("C",Thread.CurrentThread.CurrentUICulture),NumberStyles.Currency,Thread.CurrentThread.CurrentUICulture);
							diffValidator.ValueToCompare = diffCurrency.ToString();
							errMsgVal = diffValidator.ValueToCompare;

							break;
						case  ValidationDataType.Integer:
							int diffInt = int.Parse(compareText) + int.Parse(compareVal);
							diffValidator.ValueToCompare = diffInt.ToString();
							errMsgVal = diffValidator.ValueToCompare;

							break;
					
					}
					
					
					// Create the error message for the validation control in the form
					string[] args = new string[] {
													 diffValidator.Caption,
													 Localization.GetLocalizedAttributeString(diffValidator.Type.ToString()),
													 Localization.GetLocalizedAttributeString(diffValidator.Operator.ToString()),
													 errMsgVal
												 }; 
					diffValidator.ErrorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.InvalidDifference",args);
					// prevent comparison against the Control's value.
					// The value calculated above will be used for comparison
					diffValidator.ControlToCompare = String.Empty;
				}
				catch (FormatException)
				{
					// Couldn't parse one of the input values
					//  so check the data type only
					diffValidator.Operator= ValidationCompareOperator.DataTypeCheck;
					
					// change the error message here.
					string[] args = new string[] {
													 diffValidator.Caption,
													 Localization.GetLocalizedAttributeString(diffValidator.Type.ToString())
												 };
					diffValidator.ErrorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.InvalidType",args);
				}
			}

		}
		# endregion
 
		#region properties		
		private BaseControl dataForm;
		/// <summary>
		/// reference to the form which contains this control
		/// </summary>
		public BaseControl DataForm
		{
			get
			{
				return this.dataForm;
			}
			set
			{
				this.dataForm=value;
			}
		}
		private string caption;
		/// <summary>
		/// caption of the control that is to be validated
		/// </summary>
		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption=value;
			}
		}
		
		#endregion

		
	}
}
