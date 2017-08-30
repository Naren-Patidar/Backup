#region Using
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.ControlLibrary;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// As there are 2 implementations of the textbox (CSATextBox and
	/// CSAMultiControl) some areas are duplicated and hence the need
	/// for the generic textbox.
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.00" day="18" month="01" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Tom Bedwell</checker>
	///			<work_packet>Generic Enhancement</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class CSAGenericTextBox : System.Web.UI.WebControls.TextBox {
		#region Attributes
		private bool notrim;
		private string cleanse;
		private string onChange = null;
		private string onkeypress = null;
		private string onblur = null;
		private string mode;
		private string minValue;
		private string maxValue;
		private string dataType;
		private string errorCaption;
		private string errorMessage;
		private string regularexpression;
		private string differenceWith;
		private string differenceOperator;
		private string differenceValue;
		private bool setFocus;
		private bool required;
		private bool disabled;
		private bool readOnly;
		private int maxLength;
		private int size;
		private int height;
		private BaseControl.InClass currentLocation;

		#endregion

		#region Properties
		/// <summary>Get regular expression to be used for any validation.</summary>
		internal string Regularexpression { get { return this.regularexpression; } }

		/// <summary>Get the difference with.</summary>
		internal string DifferenceWith { get { return this.differenceWith; } }

		/// <summary>Get the difference operator.</summary>
		internal string DifferenceOperator { get { return this.differenceOperator; } }

		/// <summary>Get the difference value.</summary>
		internal string DifferenceValue { get { return this.differenceValue; } }

		/// <summary>Get the error caption defined for the textbox.</summary>
		internal string ErrorCaption { get { return this.errorCaption; } }

		/// <summary>Get the error message defined for the textbox.</summary>
		internal string ErrorMessage { get { return this.errorMessage; } }

		/// <summary>Get the datatype defined for the textbox.</summary>
		internal string DataType { get { return this.dataType; } }

		/// <summary>Get the max value allowed for the texbox.</summary>
		internal string MaxValue { get { return this.maxValue; } }

		/// <summary>Get the min value allowed for the texbox.</summary>
		internal string MinValue { get { return this.minValue; } }

		/// <summary>Is notrim required on this textbox?</summary>
		internal bool NoTrim { get { return this.notrim; } }

		/// <summary>Is the texbox disabled?</summary>
		internal bool Disabled { get { return this.disabled; } }

		internal BaseControl.InClass CurrentLocation { get { return this.currentLocation; } }

		/// <summary>Is focus required on this textbox?</summary>
		internal bool SetFocus { get { return this.setFocus; } }

		/// <summary>Is data entry required in this textbox?</summary>
		internal bool Required { get { return this.required; } }

		/// <summary>Is the texbox readonly?.</summary>
		public override bool ReadOnly {
			get { return this.readOnly; }
			set {
				this.readOnly = value;
				base.ReadOnly = value;
			}
		}

		/// <summary>Get the max length defined for this textbox.</summary>
		public override int MaxLength { get { return this.maxLength; } }

		/// <summary>Get the text currently assigned to the textbox.</summary>
		public override string Text {
			get {
				if ((this.cleanse != String.Empty) && (base.Text != String.Empty)) {
					Regex cleanseExpression = new Regex(this.cleanse);
					Match cleanseMatch = cleanseExpression.Match(base.Text);
					if (cleanseMatch.Success) {
						Group lastCleanseGroup = cleanseMatch.Groups[cleanseMatch.Groups.Count-1];
						StringBuilder cleanseText = new StringBuilder();
						foreach (Capture cleanseCapture in lastCleanseGroup.Captures) {
							cleanseText.Append(cleanseCapture.Value);
						}
						base.Text = cleanseText.ToString();
					}
				}
				if (!this.notrim) base.Text = base.Text.Trim();

				return base.Text;
			}
		}
		#endregion

		#region Constructor
		internal CSAGenericTextBox(
			object parent,
			XmlElement configXmlElement) {
			// Get the format options.
			this.cleanse = Localization.GetLocalizedValidationString(BaseControl.GetAttributeString(configXmlElement,"cleanse"));
			this.notrim = BaseControl.GetAttributeBool(configXmlElement,"notrim");
			this.setFocus = BaseControl.GetAttributeBool(configXmlElement,"setfocus");
			this.required = BaseControl.GetAttributeBool(configXmlElement,"required");
			// Set the max length.
			this.maxLength = BaseControl.GetAttributeInt(configXmlElement,"maxlength");
			if (this.maxLength >= 0) 
				base.MaxLength = this.maxLength;
			// Set the width.
			this.size = BaseControl.GetAttributeInt(configXmlElement,"size");
			if (this.size >= 0) 
				this.Width = Unit.Pixel(size);
			// Set the onchange event.
			this.onChange = BaseControl.GetAttributeString(configXmlElement,"onchange");
			// Set the keypress event.
			this.onkeypress  = BaseControl.GetAttributeString(configXmlElement,"onkeypress");
			// Set the OnBlur event.
			this.onblur  = BaseControl.GetAttributeString(configXmlElement,"onblur");
			// Set the mode.
			this.mode = BaseControl.GetAttributeString(configXmlElement,"mode");;
			if (this.mode.ToLower() == "password") 
				this.TextMode = TextBoxMode.Password;
			else if (this.mode.ToLower() == "multiline") {
				this.TextMode = TextBoxMode.MultiLine;
				this.height = BaseControl.GetAttributeInt(configXmlElement,"height");
				if (this.height > 0) 
					this.Height = this.height;
			}
			// Some things are determined differently depending what class
			// the parent parameter is.
			if (parent is MultiControl) {
				MultiControl mc = (MultiControl)parent;
				this.currentLocation = mc.ControlTable.currentLocation;
				this.disabled = mc.Disabled;
				this.readOnly = mc.ReadOnly;
			}
			else {
				ControlTable ct = (ControlTable)parent;
				this.currentLocation = ct.currentLocation;
				this.disabled = BaseControl.GetAttributeBool(configXmlElement,"disabled");
				this.readOnly = (ct.ReadOnly || BaseControl.GetAttributeBool(configXmlElement,"readonly"));
			}
			// Get the error caption.
			this.errorCaption = BaseControl.GetAttributeString(configXmlElement,"errorCaption");
			this.errorMessage = BaseControl.GetAttributeString(configXmlElement,"errormessage");
			// Get the datatype.
			this.dataType = BaseControl.GetAttributeString(configXmlElement,"datatype");
			// Get the validators settings.
			this.minValue = BaseControl.GetAttributeString(configXmlElement,"minvalue");
			this.maxValue = BaseControl.GetAttributeString(configXmlElement,"maxvalue");
			this.regularexpression = BaseControl.GetAttributeString(configXmlElement,"regularexpression");
			this.differenceWith = BaseControl.GetAttributeString(configXmlElement,"difference/with");
			this.differenceOperator = BaseControl.GetAttributeString(configXmlElement,"difference/operator");
			this.differenceValue = BaseControl.GetAttributeString(configXmlElement,"difference/value");
			// Set the class of the texbox.
			if (this.disabled) {
				this.CssClass = "formdisabled";
				this.Enabled = false;
			}
			else if (this.readOnly) {
				this.CssClass = "formreadonly";
				this.ReadOnly = true;
			} 
			else if (this.required) {
				this.CssClass = "forminputrequired";
			} 
			else {
				this.CssClass = "forminput";
			}
			// Check for valid parameter sets.
			if (this.maxLength<=0 && !(this.readOnly||this.disabled)) {
				CrmServiceException ce = new CrmServiceException("Library", "XML Error", "CSAControlLibrary.MissingElement","MaxLength",errorCaption);
				throw ce;
			}
		}
		#endregion

		#region Add Attribute
		/// <summary> Adds an onchange attribute to the control
		/// </summary>
		/// <param name="writer">The Html Text writer used to render the control</param>
		protected override void AddAttributesToRender(HtmlTextWriter writer) {
			if (this.onChange != null && !this.onChange.Equals(String.Empty)) {
				writer.AddAttribute(HtmlTextWriterAttribute.Onchange,this.onChange);
			}
			if (this.onkeypress != null && !this.onkeypress.Equals(String.Empty)) {
				writer.AddAttribute("onkeypress",this.onkeypress);
			}
			//To trigger textbox lostfocus event in Add Manual Transation(CSD) page
			if (this.onblur != null && !this.onblur.Equals(String.Empty)) 
			{
				writer.AddAttribute("onblur",this.onblur);
			}
			base.AddAttributesToRender(writer);
		}
		#endregion	

	}
}