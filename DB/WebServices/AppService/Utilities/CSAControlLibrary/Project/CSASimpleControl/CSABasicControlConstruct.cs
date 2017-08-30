#region Using
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Globalization;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	///	Base class for controls.
	/// </summary>
	/// <development>
	///		<version number="1.05" day="05" month="02" year="2003">
	///			<checker>Mark Hart</checker>
	///			<developer>Tom Bedwell</developer>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Added captionWidth element to config syntax</description>
	///		</version>
	/// 	<version number="1.04" day="02" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>
	///			Move FindControlById to CSABasicControlConstruct.
	///			New method AddValidationSummary.
	///			</description>
	///		</version>
	/// 	<version number="1.04" day="28" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>add DifferenceValidator</description>
	///		</version>
	///		<version number="1.03" day="16" month="01" year="2003">
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
	/// </development>
	#endregion

	public class SimpleControlConstruct {

		#region Statics
		private static string VALIDATION_ERROR_TEXT = Localization.GetLocalizedAttributeString("ValidationErrorText");
		private static string MANDATORY_FIELD_MARKER = Localization.GetLocalizedAttributeString("MandatoryFieldMarker");
		#endregion

		#region Add Caption
		/// <summary>
		/// Add a Simple Control's Caption
		/// </summary>
		/// <param name="controlTableRow">The table row to add the tip on.</param>
		/// <param name="controlTableCell">The table cell to add the caption to.</param>
		/// <param name="caption">The caption to be added to the table cell.</param>
		/// <param name="required">Is the input to this control mandatory.</param>
		/// <param name="tip">The tooltip text to be added to the table cell.</param>
		public static void AddCaption(
			TableRow controlTableRow,
			TableCell controlTableCell,
			string caption,
			string tip,
			bool required) {
			AddCaption
				(
				controlTableRow,
				controlTableCell,
				caption,
				tip,
				required,
				0
				);
		}

		/// <summary>
		/// Add a Simple Control's Caption
		/// </summary>
		/// <param name="controlTableRow">The table row to add the tip on.</param>
		/// <param name="controlTableCell">The table cell to add the caption to.</param>
		/// <param name="caption">The caption to be added to the table cell.</param>
		/// <param name="required">Is the input to this control mandatory.</param>
		/// <param name="tip">The tooltip text to be added to the table cell.</param>
		/// <param name="captionWidth">The width of the cell that the caption will be placed in.</param>
		public static void AddCaption(
			TableRow controlTableRow,
			TableCell controlTableCell,
			string caption,
			string tip,
			bool required,
			int captionWidth) {

			// Add the caption
			controlTableCell.CssClass = "formlabel";
			System.Web.UI.WebControls.Label captionLabel =  new System.Web.UI.WebControls.Label();
			if(caption==".")
				caption = " ";

			captionLabel.Text = caption;
			if (captionWidth > 0)
				controlTableCell.Width = captionWidth;
			if(required) {
				System.Web.UI.WebControls.Label mfm = new System.Web.UI.WebControls.Label();
				mfm.CssClass="formrequiredmarker";
				mfm.Text = MANDATORY_FIELD_MARKER;
				controlTableCell.Controls.Add(mfm);
			}

			controlTableCell.Controls.Add(captionLabel);

			if(!tip.Equals(String.Empty)) {
				controlTableCell.ToolTip = tip;
			}
		}
		#endregion

		#region Field Validators

		#region Required Field
		/// <summary>
		/// Add a required validator to a simple control
		/// </summary>
		/// <param name="textControl"></param>
		/// <param name="parent"></param>
		/// <param name="caption"></param>
		/// <param name="errorMessage"></param>
		/// <param name="required"></param>
		/// <param name="baseControl"></param>
		public static void AddRequiredFieldValidator(
			Control textControl,
			Control parent,
			BaseControl baseControl,
			string caption,
			string errorMessage,
			bool required) {

			if (required) {
				RequiredFieldValidator ctrl = new RequiredFieldValidator();
				ctrl.CssClass = "formrequiredvalidationerror";
				ctrl.ControlToValidate = parent.ID;
				ctrl.Text = VALIDATION_ERROR_TEXT;
				if (errorMessage != String.Empty) {
					ctrl.ErrorMessage = errorMessage;
				} else {
					ctrl.ErrorMessage = String.Format(Localization.GetLocalizedAttributeString("ErrorMsg.MissingMandatoryValue"),caption);
				}
				ctrl.ID = baseControl.GenerateControlId("requiredFieldValidator");
				textControl.Controls.Add(ctrl);

			}
		}
		#endregion

		#region Range
		/// <summary>
		/// Add a range validator to a simple control
		/// </summary>
		/// <param name="textControl"></param>
		/// <param name="parent"></param>
		/// <param name="caption"></param>
		/// <param name="errorMessage"></param>
		/// <param name="maxValue"></param>
		/// <param name="minValue"></param>
		/// <param name="baseControl"></param>
		/// <param name="datatype"></param>
		public static void AddRangeValidator(
			Control textControl,
			Control parent,
			BaseControl baseControl,
			string caption,
			string errorMessage,
			string maxValue,
			string minValue,
			string datatype) {
			
			//string[] args;

			// Add any range validation controls 
			// (exclude date controls, cos we are going to validate these ourselves
			if ((maxValue.Length >0) || (minValue.Length>0) || (datatype.Length>0)) {
				RangeValidator ctrl = new RangeValidator();
				ctrl.ID = baseControl.GenerateControlId("rangeValidator");
				ctrl.ControlToValidate = parent.ID;

				// set the maximum and minimum values
				if (minValue.Length>0) {
					ctrl.MinimumValue = minValue;
				} else {
					ctrl.MinimumValue = int.MinValue.ToString();
				}

				if (maxValue.Length>0) {
					ctrl.MaximumValue = maxValue;
				} else {
					ctrl.MaximumValue = int.MaxValue.ToString();
				}	

				string validationType = string.Empty;
				switch (datatype.ToLower()) {
					case "integer":
						ctrl.Type = ValidationDataType.Integer;
						validationType = "whole number";
						break;
					case "date":
						ctrl.Type = ValidationDataType.Date;
						validationType = "date";
						break;
					case "double":
						ctrl.Type = ValidationDataType.Double;
						validationType = "decimal number";
						break;
					case "currency":
						ctrl.Type = ValidationDataType.Currency;
						validationType = "monetary value";
						break;
					case "string":
						ctrl.Type = ValidationDataType.String;
						validationType = "character string";
						break;
					default:
						ctrl.Type = ValidationDataType.Integer;
						validationType = "whole number";
						break;
				}

				// Create the error message for the validation control in the form
				// 'Card Issue is invalid. It should be a whole number between 0 and 999'
				if (errorMessage == String.Empty) {
					if ((minValue.Length>0) && (maxValue.Length==0)) {
						errorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.Minval",caption,validationType,minValue);
					} else if ((minValue.Length==0) && (maxValue.Length>0)) {
						errorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.Maxval",caption,validationType,maxValue);
					} else if ((minValue.Length>0) && (maxValue.Length>0)) {
						errorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.BetweenMinAndMax",caption,validationType,minValue,maxValue);
					} else {
						errorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.IsInvalid", caption);
					}
				}
				ctrl.CssClass = "formrangevalidationerror";
				ctrl.Text = VALIDATION_ERROR_TEXT;
				ctrl.ErrorMessage = errorMessage;

				textControl.Controls.Add(ctrl);
			}
		}
		#endregion

		#region Regular Expression
		/// <summary>
		/// Add a regular expression validator to a simple control
		/// </summary>
		/// <param name="textControl"></param>
		/// <param name="parent"></param>
		/// <param name="caption"></param>
		/// <param name="errorMessage"></param>
		/// <param name="regularexpression"></param>
		/// <param name="baseControl"></param>
		public static void AddRegularExpressionValidator(
			Control textControl,
			Control parent,
			BaseControl baseControl,
			string caption,
			string errorMessage,
			string regularexpression) {

			// add any regular Expression validator conrols
			if ((regularexpression != null) && (regularexpression != String.Empty)) {
				RegularExpressionValidator ctrl = new RegularExpressionValidator();
				ctrl.ID = baseControl.GenerateControlId("regularExpressionValidator");
				ctrl.ControlToValidate = parent.ID;
				ctrl.ValidationExpression = Localization.GetLocalizedValidationString(regularexpression);
				ctrl.CssClass = "formregularexpressionvalidationerror";
				ctrl.Text = VALIDATION_ERROR_TEXT;
				if (errorMessage != String.Empty) {
					ctrl.ErrorMessage = errorMessage;
				} else {
					ctrl.ErrorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.IsInvalid",caption);
				}
				textControl.Controls.Add(ctrl);

			}
		}
		#endregion

		#region Difference
		/// <summary>
		/// Add a difference (based on compare) validator to a simple control
		/// Takes two control values or a control value and a constant and checks than their difference 
		/// </summary>
		/// <param name="textControl"></param>
		/// <param name="parent">Control with value to be validated</param>
		/// <param name="baseControl">Control with the xml containing the data to be compared against</param>
		/// <param name="caption">Informs user of invalid value</param>
		/// <param name="errorMessage"></param>
		/// <param name="differenceWith">Id of the control to be subtracted from the validated control value. Empty string indicates that the contant value should be used</param>
		/// <param name="differenceOperator">GreaterThan, GreaterThanEqual, LessThan, LessThanEqual, NotEqual, Equal    </param>
		/// <param name="differenceValue">Value to compare with the control value difference</param>
		/// <param name="datatype"></param>
		public static void AddDifferenceValidator(
			Control textControl,
			Control parent,
			BaseControl baseControl,
			string caption,
			string errorMessage,
			string differenceWith,
			string differenceOperator,
			string differenceValue,
			string datatype) {

			if ((differenceWith == String.Empty) && (differenceValue == String.Empty)) {
				return;
			}

			ValidationCompareOperator comparison;
			switch (differenceOperator) {
				case "greater than":
					comparison = ValidationCompareOperator.GreaterThan;
					break;
				case "not greater than":
					comparison = ValidationCompareOperator.GreaterThanEqual;
					break;
				case "less than":
					comparison = ValidationCompareOperator.LessThan;
					break;
				case "not less than":
					comparison = ValidationCompareOperator.LessThanEqual;
					break;
				case "equals":
					comparison = ValidationCompareOperator.Equal;
					break;
				case "not equals":
					comparison = ValidationCompareOperator.NotEqual;
					break;
				default:
					return;
			}

			ValidationDataType type;
			switch (datatype) {
				case "currency":
					type = ValidationDataType.Currency;
					break;
				case "date":
					type = ValidationDataType.Date;
					break;
				case "double":
					type = ValidationDataType.Double;
					break;
				case "integer":
					type = ValidationDataType.Integer;
					break;
				case "string":
					type = ValidationDataType.String;
					break;
				default:
					return;
			}

			if ((differenceWith != String.Empty) && (differenceValue != String.Empty)) {
				AddServerDifferenceValidator(
					textControl,
					parent,
					baseControl,
					caption,
					differenceWith,
					comparison,
					differenceValue,
					type);
				return;
			}

			CompareValidator ctrl = new CompareValidator();
			ctrl.ID = baseControl.GenerateControlId("differenceValidator");
			ctrl.ControlToValidate = parent.ID;
			ctrl.Operator = comparison;
			ctrl.Type = type;
			ctrl.CssClass = "formdifferencevalidationerror";
			ctrl.Text = VALIDATION_ERROR_TEXT;

			string target;
			if (differenceWith != String.Empty) {

				XmlNode differenceNode = baseControl.ConfigXmlDoc.SelectSingleNode("//*[xpath='"+differenceWith+"']");
				if (differenceNode == null) {
					return;
				}
				string differenceID, dummy1, dummy2, dummy3, dummy4;
				baseControl.GetFieldDetails((XmlElement)differenceNode, out dummy1, out dummy2, out differenceID, out target, out dummy3, out dummy4);
				ctrl.ControlToCompare = differenceID;

			} else {
				ctrl.ValueToCompare = differenceValue;
				target = differenceValue;
			}

			if (errorMessage == String.Empty) {
				errorMessage = Localization.GetLocalizedAttributeString(
						"ErrorMsg.InvalidDifference",
						caption,
						Localization.GetLocalizedAttributeString(ctrl.Type.ToString()),
						Localization.GetLocalizedAttributeString(ctrl.Operator.ToString()),
						target);
			}
			ctrl.ErrorMessage = errorMessage;

			textControl.Controls.Add(ctrl);
		}

		/// <summary>
		/// Add a difference (based on compare) validator to a simple control
		/// Takes two control values or a control value and a constant and checks than their difference 
		/// </summary>
		/// <param name="textControl"></param>
		/// <param name="parent">Control with value to be validated</param>
		/// <param name="dataControl">Control with the xml containing the data to be compared against</param>
		/// <param name="caption">Informs user of invalid value</param>
		/// <param name="differenceWith">Id of the control to be subtracted from the validated control value. Empty string indicates that the contant value should be used</param>
		/// <param name="comparison">GreaterThan, GreaterThanEqual, LessThan, LessThanEqual, NotEqual, Equal    </param>
		/// <param name="differenceValue">Value to compare with the control value difference</param>
		/// <param name="type"></param>
		public static void AddServerDifferenceValidator(
			Control textControl,
			Control parent,
			BaseControl dataControl,
			string caption,
			string differenceWith,
			ValidationCompareOperator comparison,
			string differenceValue,
			ValidationDataType type) {
			
			// Add compare validation controls 
			if (differenceValue.Length > 0) {
				DifferenceValidator ctrl = new DifferenceValidator();
				ctrl.ID = dataControl.GenerateControlId("differenceValidator");
				ctrl.ControlToValidate = parent.ID;
				ctrl.ControlToCompare = differenceWith;
				ctrl.Operator = comparison;
				ctrl.Type = type;
				ctrl.DataForm = dataControl;
				ctrl.ValueToCompare = differenceValue;
				ctrl.Caption=caption;
				ctrl.CssClass = "formdifferencevalidationerror";
				ctrl.Text = VALIDATION_ERROR_TEXT;
				ctrl.ErrorMessage  = "Invalid value"; // this gets replaced when the validation criteria are calculated
				textControl.Controls.Add(ctrl);
			}
		}

		#endregion

		#endregion

		#region Find Control By Id
		/// <summary>
		/// Search the control hierachy for a control with a given ID
		/// </summary>
		/// <param name="owner">Owning control</param>
		/// <param name="id">ID of the sought control</param>
		/// <returns>Control with id or null</returns>
		public static  Control FindControlById(Control owner,string id)
			// TODO investigate moving this to SharedUtils
		{
			Control c = owner.FindControl(id);
			if (c == null && owner.HasControls())
			{
				foreach (Control child in owner.Controls)
				{
					c = FindControlById(child,id);
					if (c != null)
					{
						return c;
					}

				}
			}
			return c;
		}
		#endregion

	}
}