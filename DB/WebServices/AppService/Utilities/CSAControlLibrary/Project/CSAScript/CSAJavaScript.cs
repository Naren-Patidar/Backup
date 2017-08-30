#region Using
using System;
using System.Web.UI.HtmlControls;
#endregion

namespace Fujitsu.eCrm.Generic.ControlLibrary.CSAScript
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// Generate Javascript.
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
	
	public class CSAJavaScript
	{
		#region Constants
		private const string newLine = "\n";
		#endregion

		#region Static Methods

		#region Search For Control
		/// <summary>
		/// Generate the JavaScript required to search for a control
		/// with a particular ID. The ID of the control will be the
		/// pre-generated i.e. the ID assigned in the code. The script
		/// will search through the HTML document looking for the first
		/// match with the .NET ID and will then use this control. Once
		/// the control is found, an action can be performed.
		/// </summary>
		/// <param name="controlId">The control ID to search for.</param>
		/// <returns>The generated JavaScript string.</returns>
		private static string SearchForControl(
			string controlId
			)
		{
			return

				// Set-up the regular expression to locate the control.
				"var locatedControl;" + newLine +
				"var regexControl = /^.*" + controlId + "$/;" + newLine +
				"var i = -1;" + newLine +
				// Loop round until the control is found.
				"while(true)" + newLine +
				"{" + newLine +
					"i++;" + newLine +
					// If number of elements in the document equals the current count
					// then all elements have been searched and the control hasn't
					// been found. Stop the loop.
					"if (document.forms[0].elements.length == i)" + newLine +
						"break;" + newLine +
					// Get the ID of the current control.
					"var id = document.forms[0].elements[i].id;" + newLine +
					// Attempt to match with the required control name.
					"var result = id.match(regexControl);" + newLine +
					"if (result != null)" + newLine +
					"{" + newLine +
						// The control has been found, so return the object.
						"locatedControl = document.forms[0].elements[i];" + newLine +
						"break;" + newLine +
					"}" + newLine +
				"}";
		}
		#endregion

		#region Set Focus On Control
		/// <summary>
		/// Generate a JavaScript function to search for an element
		/// in the HTML document and if found, set the focus. The
		/// match is against the .NET generated ID, which can have
		/// a lot of garbage on the start, before the string assigned
		/// in the code is found. This is done using regular expressions.
		/// </summary>
		/// <param name="controlId">The ID of the control to search for.</param>
		/// <param name="inClass">The current location when this script is placed
		/// in the control.</param>
		/// <returns>An HtmlgenericControl object with the JavaScript.</returns>
		internal static HtmlGenericControl SetFocusOnControl(
			string controlId,
			BaseControl.InClass inClass
			)
		{
			HtmlGenericControl script =  new HtmlGenericControl("script");
			string javaScript = newLine +

				// Declare the variable to hold the last executed set focus method.
				"var lastFocusExecution;" + newLine +

				// Add to onload actions.
				"if (typeof(onloadFunctions) != 'undefined')" + newLine +
					"onloadFunctions[onloadFunctions.length] = SetFocusOnControl;" + newLine +

				// Define the function name.
				"function SetFocusOnControl()" + newLine +
				"{" + newLine +

					// Determine if this needs to be run. Only run if
					//
					// (1) It is the first set focus.
					// (2) The last set focus was from a DataForm and this is
					//     a DataForm or PopupForm.
					// (3) The last set focus was from a PopupForm and this is
					//     a PopupForm.
					"if (lastFocusExecution != null)" + newLine +
					"{" + newLine;

			if (inClass == BaseControl.InClass.DataForm)
			{
				javaScript +=

						// Check if the last execution was a PopupForm. If it was
						// then we don't won't to perform this focus, as it is a
						// DataForm.
						"if (lastFocusExecution == 'PopupForm')" + newLine +
							"return;" + newLine;
			}
				javaScript +=

					// End the if lastFocusExecution.						
					"}" + newLine +

			// Need to check if we are in a dataform.
			"var foundFogBox;" + newLine;

			if (inClass == BaseControl.InClass.DataForm)
			{
				// Need to check if the fogbox is present. If it
				// is then need to check if the fogbox is present,
				// because if it is, then no focus is required.
				javaScript +=

					// Check whether the fogbox is on the page.
					"foundFogBox = document.getElementById('bp_forceField');" + newLine;
			}

				javaScript +=

					// Insert code to search for control to set focus on.
					SearchForControl(controlId) + newLine +

					// Set the focus on the located control, as long as it
					// 
					// (1) isn't null
					// (2) tab index isn't -1
					// (3) if it is a dataform and a fogbox isn't present
					//
					"if (locatedControl != null)" + newLine +
					"{" + newLine +
					    "if (locatedControl.tabIndex != -1)" + newLine +
						"{" + newLine +
							"if (foundFogBox == null)" + newLine +
							"{" + newLine +
								"locatedControl.focus()" + newLine +
								"lastFocusExecution = '" + inClass.ToString() + "';" + newLine +
							"}" + newLine +
						"}" + newLine +
					"}" + newLine +
				"}" + newLine;

			// Set the inner text of the control.
			script.InnerText = javaScript;
			return script;
		}
		#endregion

		#endregion
	}
}