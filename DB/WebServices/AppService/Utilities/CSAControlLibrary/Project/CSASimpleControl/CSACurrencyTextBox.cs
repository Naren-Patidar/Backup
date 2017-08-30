using System;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Xml;

namespace Fujitsu.eCrm.Generic.ControlLibrary.SimpleControl {

	/// <summary>
	/// Summary description for CurrencyTextBox.
	/// </summary>
	public class CurrencyTextBox : TextBox, ISimpleControl {

		/// <summary>
		/// Construct a control containing a text box with currency info
		/// </summary>
		/// <param name="parent">The ControlTable Web Page</param>
		/// <param name="controlTableRow">The ControlTable Current Row</param>
		/// <param name="configXmlElement"></param>
		/// <param name="dataXmlElement"></param>
		public CurrencyTextBox(
			ControlTable parent, 
			TableRow controlTableRow,
			XmlElement configXmlElement, 
			XmlElement dataXmlElement) : base(parent,controlTableRow,configXmlElement,dataXmlElement) {

			Double xx;
			// Try to reformat the amount into a currency amount (i.e. 2 digits after the decimal point)
			if (Double.TryParse(
					this.Text,
					System.Globalization.NumberStyles.Float,
					new CultureInfo("en-GB"), // The Database's Culture
					out xx)) {
				this.Text = xx.ToString("F2");
			}
			
		}
	}
}
