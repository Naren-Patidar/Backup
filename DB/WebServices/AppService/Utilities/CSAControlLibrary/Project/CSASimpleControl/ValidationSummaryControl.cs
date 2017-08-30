using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Fujitsu.eCrm.Generic.ControlLibrary {

	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Customized summary control which presents itself as a draggable,minimizable pop-up window on top of the form
	/// (references javascript ,'drag.js',stylesheets and minimize,close and restore button images to complete the illusion of a window)
	/// </summary>
	/// 
	/// <development> 
	///		<version number="1.11" day="05" month="02" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Seoul/047</work_packet>
	///			<description>Moved Validation summary to main panel</description>
	///		</version>
	///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	/// 	<version number="1.00" day="02" month="12" year="2002">
	///			<developer> Tom Bedwell </developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet>WP/Barcelona/025</work_packet>
	///			<description>New class</description>
	///		</version>
	/// </development>
	#endregion

	public class ValidationSummaryControl : System.Web.UI.WebControls.ValidationSummary  {

		private string validationSummaryMsg;
		private bool useDefault;

		/// <summary>
		/// Customized summary control which presents itself as a draggable,minimizable window
		/// Requires the stylesheets, button images and scripts are installed on the web server
		/// </summary>
		public ValidationSummaryControl(ControlCollection parent, bool useDefault) {

			this.useDefault = useDefault;
			if (useDefault) {
				this.ShowMessageBox = true;
				this.ShowSummary = false;
			} else {
				this.ID="ValidationSummary1";
				this.CssClass="validationsummary";
				this.Style.Add("z-index","10");
				this.DisplayMode=ValidationSummaryDisplayMode.BulletList;
				// this sets up the html for the draggable bar and the minimize,restore and close buttons
				this.HeaderText=
					"<div onmousedown='onDragMouseDown()' class='validationSummaryBar'>&nbsp;" +
				
					"<div class='validationSummaryButtons'>"+
					"<img src='images/minimize.bmp' id='minimizeimage' onClick='onMinimizeClick()' onmousedown='ignoreEvent()'  alt='minimize'/>"+
					"<img src='images/restore.bmp' id='restoreimage' onClick='onRestoreClick()' onmousedown='ignoreEvent()' style='display:none;' alt='restore'/>"+
					"<img src='images/close.bmp' id='closeimage' onClick='onCloseClick()' onmousedown='ignoreEvent()' alt='close'/>"+
					"</div> </div>";
				this.ShowMessageBox=false;
				this.ShowSummary=true;

				HtmlGenericControl script =  new HtmlGenericControl("script");
				script.Attributes.Add("src","scripts/drag.js");
				script.InnerText="/* */";
				parent.Add(script);
			}
			parent.Add(this);
		}


		/// <summary>
		/// text that appears in the validation summary header
		/// </summary>
		public string ValidationSummaryMsg {
			get	{
				if (this.useDefault) {
					return this.HeaderText;
				} else {
					return validationSummaryMsg;
				}
			}
			set	{
				if (this.useDefault) {
					this.HeaderText = value;
				} else {
					validationSummaryMsg=value;
					this.HeaderText += 
						"<div class='validationSummaryErrorBar'><img id='warningimage' src='images/Warn.gif' alt='warning'/>"+
						validationSummaryMsg +
						"</div>";
				}
			}
		}

	
	}
}