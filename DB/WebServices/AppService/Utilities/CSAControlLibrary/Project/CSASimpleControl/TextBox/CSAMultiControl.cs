using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
using Fujitsu.eCrm.Generic.ControlLibrary.CSAScript;
namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	/// <summary>
	/// Valid elements that can appear in the config file
	/// field[type='multi']
	///		xpath					- the xpath for searching the data document
	///		caption					- text on the page, to the left of the control
	///		captionWidth			- the width given to the caption
	///		tip						- tool tip appearing when mouse pointer over multi control
	///		datatype				- data type of data, used for formatting
	///		minvalue				- the min value of data value
	///		maxvalue				- the max value of data value
	///		format					- format, how to display the data
	///		required				- one or more subfields contain text
	///		readonly				- is it possible to change data
	///		output					- how to combine the subfield values to generate the field value
	///		input					- how to split the field value to generate the subfield values
	///		sequence				- comma separated list of sub fields which alters the order in which they appear on the page
	///		regularexpression		- pattern that the field value (after combining using output) must match, regularexpression works only if required is true
	///		difference				- set up a difference validator
	///			with				- the id of the control that this control is being compared with
	///			operator			- how to compare the two controls
	///			value				- the threshold of the comparison (not implemented)
	///		subfield[type='label']	- a label subfield
	///			subfield			-	the name of the subfield as mentioned in the field's sequence
	///			text				- text to appear in the label
	///			size				- the width of the label in pixels
	///		subfield[type='textbox']- a textbox subfield
	///			tip					- tool tip appearing when mouse pointer over subfield
	///			maxlength			- the max number of characters in the textbox
	///			size				- the width of the textbox in pixels
	///			mode				- password/multiline
	///			fieldpart			- the name of the subfield as mentioned in the field's input, output and sequence
	///			caption				- caption, used in the validation messages
	///			datatype			- the data typem used for setting default min and max values
	///			minvalue			- the min value of subfield value
	///			maxvalue			- the max value of subfield value
	///			required			- should the subfields contain text
	///			regularexpression	- pattern that the subfield value must match
	///			cleanse				- regular expression for removing sub-patterns
	///			default				- text value to use if the textbox is empty
	///			difference			- set up a difference validator
	///				with
	///				operator
	///				value
	/// </summary>
	internal class MultiControl : PlaceHolder, ISimpleControl {

		#region Attributes
		private bool readOnly = true;
		private bool required = false;
		private bool disabled = false;
		private bool gotText = false;
		private string outputFormat = null;
		private string caption = null;
		private string errorCaption = null;
		private string errorMessage = null;
		private string tip = null;
		private string pattern = String.Empty;
		private string text = null;
		private string format = null;
		private string datatype = null;
		private string minvalue = null;
		private string maxvalue = null;
		private string differenceWith = null;
		private string differenceOperator = null;
		//private string differenceValue = null;
		private TableCell captionCell = null;
		private TableCell controlCell = null;
		private ControlTable controlTable = null;
		private ArrayList parts = new ArrayList();
		#endregion

		#region Properties
		internal bool Disabled { get { return this.disabled; } }
		internal bool ReadOnly { get { return this.readOnly; } }
		internal string Caption { get { return this.caption; } }
		internal TableCell CaptionCell { get { return this.captionCell; } }
		internal ControlTable ControlTable { get { return this.controlTable; } }
		internal ArrayList Parts { get { return this.parts; } }
		internal string Pattern { get { return this.pattern; } }

		/// <summary>
		/// Extract values from the subfields and format into a field
		/// level value
		/// </summary>
		internal string Text {
			get {
				if (!this.gotText) {

					this.gotText = true;
					this.text = String.Empty;

					string[] subValue = new string[this.Parts.Count];
					string adjustedOutputFormat = this.outputFormat;
					bool isEmpty = true;

					if (!this.disabled) {
						for (int i=0; i<this.Controls.Count; i++) {
							Control subControl = (Control)this.Parts[i];
							if (subControl is MultiControl.TextBox) {
								MultiControl.TextBox subTextBoxControl = (MultiControl.TextBox)subControl;
								subValue[i] = subTextBoxControl.Text;;
								// Only check for an empty texbox.
								if (subValue[i] != String.Empty) {
									isEmpty = false; 
								} else if (subTextBoxControl.DefaultText != null) {
									subValue[i] = subTextBoxControl.DefaultText;
								}
								if (subTextBoxControl.FieldPart != null) {
								
								adjustedOutputFormat = adjustedOutputFormat.Replace(subTextBoxControl.FieldPart ,i.ToString());
							}

							} else if (subControl is MultiControl.Label) {
								subValue[i] = ((MultiControl.Label)subControl).Text;
							}
						}
						if (!isEmpty) {
							this.text = String.Format(adjustedOutputFormat, subValue);
							if (this.datatype == "date") {
								try {
									DateTime dateValue;
									if (this.format != String.Empty) {
										dateValue = DateTime.ParseExact(this.text,this.format,null);
									} else {
										dateValue = DateTime.Parse(this.text);
									}
									this.text = dateValue.ToString("s");
								} catch {}
							}
						} else {
							if (this.datatype == "date") {
								this.text = null;
							}
						}
					}
				}
				return this.text;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parent">The Dataform containing this control</param>
		/// <param name="controlTableRow">The table row containing this control</param>
		/// <param name="configXmlElement">The XML Node in the configuration file describing this control's layout</param>
		/// <param name="dataXmlElement">The XML Node from the CRM Server describing this control's data</param>
		public MultiControl(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) {

			this.controlTable = parent;

			// Get Configuration Details
			string id, dataValue;
			parent.GetSettings(
				configXmlElement,
				dataXmlElement, 
				out dataValue, 
				out id, 
				out this.caption,
				out this.tip,
				out this.format);

			this.ID = id;

			// Standard configuration elements
			this.required = BaseControl.GetAttributeBool(configXmlElement, "required");
			this.readOnly = (parent.ReadOnly || BaseControl.GetAttributeBool(configXmlElement, "readonly"));
			this.disabled = BaseControl.GetAttributeBool(configXmlElement, "disabled");
			int size = BaseControl.GetAttributeInt(configXmlElement, "size");
			this.datatype = BaseControl.GetAttributeString(configXmlElement, "datatype");
			this.minvalue = BaseControl.GetAttributeString(configXmlElement, "minvalue");
			this.maxvalue = BaseControl.GetAttributeString(configXmlElement, "maxvalue");

			// Difference Comparison
			this.differenceWith = BaseControl.GetAttributeString(configXmlElement,"difference/with");
			this.differenceOperator = BaseControl.GetAttributeString(configXmlElement,"difference/operator");
			//this.differenceValue = BaseControl.GetAttributeString(configXmlElement,"difference/value");

			// Multi configuration elements
			this.outputFormat = BaseControl.GetAttributeString(configXmlElement, "output");

			// error caption
			string errorCaptionStr = BaseControl.GetAttributeString(configXmlElement, "errorCaption");
			if (errorCaptionStr==String.Empty)
				this.errorCaption = this.caption;
			else
				this.errorCaption = errorCaptionStr;
			this.errorMessage = BaseControl.GetAttributeString(configXmlElement,"errormessage");


			// Process the data value
			// 1. Re-format the value
			// 2. Split the value into parts
			if (dataValue == null) {
				dataValue = String.Empty;
			}
			if ((dataValue != String.Empty) && (this.format != String.Empty)) {
				if (this.datatype == "date") {
					dataValue = DateTime.Parse(dataValue).ToString(this.format);
				} else if (this.datatype == "int") {
					dataValue = Int32.Parse(dataValue).ToString(this.format);
				} else if (this.datatype == "currency") {
					dataValue = Decimal.Parse(dataValue).ToString(this.format);
				}
			}

			// Add Control's Caption to Page's Table Row
			this.captionCell = new TableCell();
			controlTableRow.Cells.Add(this.captionCell);

			// Add Control to Page's Table Row
			this.controlCell = new TableCell();
			if (size>=0) {
				this.controlCell.Width = Unit.Pixel(size);
			}
			this.controlCell.Controls.Add(BaseControl.WrapControlInHtml(this,"multi","simplecontrol" + id));
			controlTableRow.Cells.Add(this.controlCell);


			string input = BaseControl.GetAttributeString(configXmlElement, "input");
			Regex inputRegex = new Regex(input);
			Match inputMatch = inputRegex.Match(dataValue);

			if (!this.disabled) {
				this.pattern = BaseControl.GetAttributeString(configXmlElement, "regularexpression");
				this.pattern = Localization.GetLocalizedValidationString(this.pattern);
			
			}
			string sequence = BaseControl.GetAttributeString(configXmlElement,"sequence");
			if (StringUtils.IsStringEmpty(sequence)) {
				// create the subfields in the order in which they are defined
				foreach (XmlNode subfieldNode in configXmlElement.SelectNodes("./subfield")) {
					createSubfield(subfieldNode,inputMatch);
				}
			}
			else {
				// there is a comma separated sequence of subfields which re-arranges the order in which the subfields appear
				foreach (string fieldpart in sequence.Split(new Char[]{','})) {
					XmlNode subfieldNode = configXmlElement.SelectSingleNode("subfield[fieldpart='" + fieldpart + "']");
					if (subfieldNode != null)
						createSubfield(subfieldNode, inputMatch);

				}

			}
			if ((this.required) || 
				(this.datatype == "date") || 
				(this.pattern != String.Empty) || 
				(this.differenceWith != String.Empty) ||
				(this.minvalue != String.Empty) ||
				(this.maxvalue != String.Empty) ) {
				CustomValidator serverValidator = ConstructValidationControl();
				serverValidator.ServerValidate += new ServerValidateEventHandler(ServerValidation);
			}

			// Add caption to the caption cell
			int captionWidth = BaseControl.GetAttributeInt(configXmlElement,"captionWidth");
			SimpleControlConstruct.AddCaption(controlTableRow,this.captionCell,caption,this.tip,required,captionWidth);

		}
		#endregion

		#region Methods
		internal CustomValidator ConstructValidationControl() {
			CustomValidator serverValidator = new CustomValidator();
			serverValidator.CssClass = "formregularexpressionvalidationerror";
			serverValidator.Text = Localization.GetLocalizedAttributeString("ValidationErrorText");
			if (this.errorMessage != String.Empty) {
				serverValidator.ErrorMessage = this.errorMessage;
			} else {
				serverValidator.ErrorMessage = Localization.GetLocalizedAttributeString("ErrorMsg.IsInvalid",this.errorCaption);
			}
			serverValidator.EnableClientScript = false;
			this.captionCell.Controls.Add(serverValidator);
			return serverValidator;
		}

		/// <summary>
		/// Compare the field value against various conditions
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		internal void ServerValidation(object source, ServerValidateEventArgs args) {
			try {
				// innocent until proven guilty
				args.IsValid = true;

				string valueToCompare = null;
				if (this.differenceWith != String.Empty) {
					Control controlToCompare = SimpleControl.SimpleControlConstruct.FindControlById(this.Page,this.differenceWith);
					if (controlToCompare is MultiControl) {
						valueToCompare = ((MultiControl)controlToCompare).Text;
					} else if (controlToCompare is TextBox) {
						valueToCompare = ((TextBox)controlToCompare).Text;
					}
				}

				if (this.datatype == "date") {
					#region Date
					if (this.Text == null) {
						if (this.required) {
							args.IsValid = false;
						}
						return;
					}

					// an exception is thrown if the string cannot be parsed into a string
					DateTime thisValue = DateTime.ParseExact(this.Text,"s",null);

					if (this.minvalue != String.Empty) {
						DateTime minValue;
						if (this.minvalue.ToLower() == "today") {
							minValue = DateTime.Today;
						} else {
							minValue = DateTime.ParseExact(this.minvalue,this.format,null);
						}
						if (thisValue < minValue) {
							args.IsValid = false;
							return;
						}
					}

					if (this.maxvalue != String.Empty) {
						DateTime maxValue;
						if (this.maxvalue.ToLower() == "today") {
							maxValue = DateTime.Today;
						} else {
							maxValue = DateTime.ParseExact(this.maxvalue,this.format,null);
						}
						if (thisValue > maxValue) {
							args.IsValid = false;
							return;
						}
					}

					// compare values
					if (valueToCompare != null) {
						try {
							DateTime thatValue = DateTime.ParseExact(valueToCompare,"s",null);
								
							switch (this.differenceOperator) {
								case "equals":
									args.IsValid = (thisValue == thatValue);
									break;
								case "not equals":
									args.IsValid = (thisValue != thatValue);
									break;
								case "greater than":
									args.IsValid = (thisValue > thatValue);
									break;
								case "not greater than":
									args.IsValid = (thisValue <= thatValue);
									break;
								case "less than":
									args.IsValid = (thisValue < thatValue);
									break;
								case "not less than":
									args.IsValid = (thisValue >= thatValue);
									break;
								default:
									break;
							}
						} catch {
							// suppress
						}
					}
					#endregion
				} else {
					#region String
					if (this.Text == String.Empty) {
						if (this.required) {
							args.IsValid = false;
						}
						return;
					}
					
					if (this.pattern != String.Empty) {
						Regex inputRegex = new Regex(this.pattern);
						if (!inputRegex.IsMatch(this.Text)) {
							args.IsValid = false;
							return;
						}
					}

					if (this.minvalue != String.Empty) {
						if (this.Text.CompareTo(this.minvalue) < 0) {
							args.IsValid = false;
							return;
						}
					}

					if (this.maxvalue != String.Empty) {
						if (this.Text.CompareTo(this.maxvalue) > 0) {
							args.IsValid = false;
							return;
						}
					}

					// compare text values
					if (valueToCompare != null) {
						switch (this.differenceOperator) {
							case "equals":
								args.IsValid = (this.Text == valueToCompare);
								break;
							case "not equals":
								args.IsValid = (this.Text != valueToCompare);
								break;
							case "greater than":
								args.IsValid = (this.Text.CompareTo(valueToCompare) > 0);
								break;
							case "not greater than":
								args.IsValid = (this.Text.CompareTo(valueToCompare) <= 0);
								break;
							case "less than":
								args.IsValid = (this.Text.CompareTo(valueToCompare) < 0);
								break;
							case "not less than":
								args.IsValid = (this.Text.CompareTo(valueToCompare) >= 0);
								break;
							default:
								break;
						}
					}
					#endregion
				}

			} catch {
				args.IsValid = false;
			}
		}

		/// <summary>
		/// Return the value of the field
		/// </summary>
		/// <param name="ht"></param>
		public void GetValue(Hashtable ht) {
			if (!this.disabled) {
				ht.Add(this.ID,this.Text);
			}
		}
		#endregion

		#region Subfields
		/// <summary>
		/// A Label, useful for separating textboxes
		/// </summary>
		internal class Label : System.Web.UI.WebControls.Label {

			internal Label(
				MultiControl parent,
				XmlElement configXmlElement) {

				if (parent.disabled) {
					this.CssClass = "formmultilabel";
					this.Enabled = false;
				} else {
					this.CssClass = "formmultilabel";
				}


				this.Text = BaseControl.GetAttributeString(configXmlElement, "text");

				int size = BaseControl.GetAttributeInt(configXmlElement, "size");
				if (size > 0) {
					this.Width = Unit.Pixel(size);
				}

			}
		}

		/// <summary>
		/// A Textbox
		/// </summary>
		internal class TextBox : CSAGenericTextBox {
			
			private string defaultText;
			private string fieldPartName;

			internal string DefaultText { get { return this.defaultText; } }
			internal string FieldPart {get {return this.fieldPartName;}}

			internal TextBox(
				MultiControl parent,
				XmlElement configXmlElement,
				Match inputMatch) : base(parent,configXmlElement) {
				
				// Generate and set the ID of the control.
				string fieldpart = BaseControl.GetAttributeString(configXmlElement,"fieldpart");
				this.ID = parent.ID + "/" + fieldpart;
				this.fieldPartName = fieldpart;
				// Set the text of the control.
				this.Text = inputMatch.Groups[fieldpart].Value;
				// Check if this textbox should have focus.
				if (this.SetFocus)
					parent.ControlTable.SetFocus = CSAJavaScript.SetFocusOnControl(this.ID,parent.ControlTable.currentLocation);
				// Set the caption.
				string caption = BaseControl.GetAttributeString(configXmlElement,"caption");
				if (caption == String.Empty) {
					caption = parent.Caption + " " + fieldpart;
				}
				// Set the error caption.
				string errorCaption;
				if (this.ErrorCaption == String.Empty)
					errorCaption = caption;
				else
					errorCaption = this.ErrorCaption;
				// Set the tip.
				string tip = BaseControl.GetAttributeString(configXmlElement, "tip");
				if (tip==String.Empty)
					tip=parent.tip;
				if (tip != String.Empty) 
					this.ToolTip = tip;
				// Set any difference validators.
				if (!parent.ReadOnly && !parent.disabled) {
					// Set the default text.
					this.defaultText = BaseControl.GetAttributeString(configXmlElement, "default");
					// If there is validation at the field level then do not display
					// Mandatory Text when the sub fields are invalid.  Achieve this
					// by writting the mandatory text to the textbox instead of the
					// caption.  Yuck!
					Control textControl = (parent.Pattern == String.Empty) ? (Control)parent.CaptionCell : (Control)this;
					SimpleControlConstruct.AddRequiredFieldValidator(textControl,this,parent.ControlTable,errorCaption,this.ErrorMessage,this.Required);
					// If max and min are not set, then only try to add a default range validator if there is no difference validator
					if (this.DifferenceWith == String.Empty || this.MaxValue != String.Empty || this.MinValue != String.Empty)
						SimpleControlConstruct.AddRangeValidator(textControl,this,parent.ControlTable,errorCaption,this.ErrorMessage,this.MaxValue,this.MinValue,this.DataType);
					SimpleControlConstruct.AddRegularExpressionValidator(textControl,this,parent.ControlTable,errorCaption,this.ErrorMessage,this.Regularexpression);
					SimpleControlConstruct.AddDifferenceValidator(textControl,this,parent.ControlTable,errorCaption,this.ErrorMessage,this.DifferenceWith,this.DifferenceOperator,this.DifferenceValue,this.DataType);
				}
			}
		}
		#endregion

		#region Create Sub Field
		private void createSubfield(XmlNode subfieldNode,Match inputMatch)
		{
			string type = BaseControl.GetAttributeString(subfieldNode, "type");
			Control subControl;
			switch (type) 
			{
				case "label":
					subControl = new MultiControl.Label(this,(XmlElement)subfieldNode);
					this.parts.Add(subControl);
					break;
				default:
					Control subSubControl = new MultiControl.TextBox(this,(XmlElement)subfieldNode,inputMatch);
					this.parts.Add(subSubControl);
					subControl = BaseControl.WrapControlInHtml(subSubControl,"multi","textbox" + subSubControl.ID);
					break;
			}
			this.Controls.Add(subControl);			
		}
		#endregion
	}
}