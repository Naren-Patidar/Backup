using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	/// <summary>
	/// Returns a control containing just a blank
	/// line, for use as a spacer to seperate 
	/// different sections of the form
	/// </summary>
	public class BlankLine : Literal, ISimpleControl {

		/// <summary>
		/// Extract the Control Values and insert it into a Hashtable
		/// </summary>
		/// <param name="ht">The Hashtable of values</param>
		public void GetValue(Hashtable ht) {
		}

		/// <summary>
		/// Set Literal Text to Blank Line
		/// </summary>
		public BlankLine(
			ControlTable parent, 
			TableRow controlTableRow) {

			// Add Control to Page's Table Row
			TableCell controlTableCell = new TableCell();
			controlTableCell.Controls.Add(BaseControl.WrapControlInHtml(this,"controlblankline","controlblankline"));
			controlTableRow.Cells.Add(controlTableCell);

			// Setup Control
			this.Text = "&nbsp";
		}
	}
}