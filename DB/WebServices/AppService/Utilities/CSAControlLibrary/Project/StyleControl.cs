namespace Fujitsu.eCrm.Generic.ControlLibrary
{
	#region using
		using System;
		using System.Web;
		using System.Web.UI;
		using System.Web.UI.WebControls;
		using System.ComponentModel;
		using System.Resources;
		using System.Globalization;
		using System.Reflection;
		using System.Threading;
		using Fujitsu.eCrm.Generic.SharedUtils;
		using Fujitsu.eCrm.Generic.LocalizationLibrary;
	#endregion

	#region Header
	/// <summary>
	/// Summary description for StyleControl.
	/// The Style Control obtains a value from a resource file using name 'stylesheet'
	/// This value is then used as a lookup in the Style resource in Localization.
	/// </summary>
	/// <development> 
	///		<version number="1.00" day="22" month="01" year="2003">
	///			<developer>Bill Curtis</developer>
	///			<checker>Mark Hart</checker>
	///			<work_packet></work_packet>
	///			<description>Added new method to get localised validation string.</description>
	///		</version>
	/// </development> 
	#endregion

	[DefaultProperty("Text"), 
		ToolboxData("<{0}:StyleControl runat=server></{0}:StyleControl>")]
	public class StyleControl : System.Web.UI.WebControls.WebControl
	{
	
		/// <summary>
		/// Returns the required HTML LINK lines to be rendered
		/// </summary>
		private string SetStyleText()
		{
			string styleText = "";
			ResourceManager resourceManager;
			// Attempt to get the culture info from the Session.
			string culture = (string)Page.Session[WebSessionIndexes.Culture()];

			// Now set the culture info for this page.
			CultureInfo cultureInfo = new CultureInfo(culture);

			// Set-up the resource file. Need to know the owning assembly for the resource.
			resourceManager = 
				new ResourceManager (
				this.ResourceLocation + "." + this.PageName,
				owningAssembly);
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			string styleType = resourceManager.GetString("stylesheet");
			if (styleType == null) return styleText;

			string styleString = Localization.GetLocalizedStyleString(styleType);

			if (styleString == null) return styleText;

			// now extract the stylesheet filenames separated by semi-colons
			// and create a <LINK> line for each stylesheet

			string lastPart = styleString; //Part equals the whole
			string firstPart = "";
			char [] separator = {';',','};
			int posn = lastPart.IndexOfAny(separator);
			if (posn == -1)
			{
				// only one file, so no separator needed but is optional
				//<LINK href="filename" type="text/css" rel="stylesheet">
				styleText = "<LINK href=\"" + styleString + "\" type=\"text/css\" rel=\"stylesheet\">";
			}
			
			while (posn > 0)
			{
				// may be more than one get the firstpart
				firstPart = lastPart.Substring(0,posn-1);
				styleText = styleText + "\n" +
				"<LINK href=\"" + firstPart + "\" type=\"text/css\" rel=\"stylesheet\">";

				lastPart = lastPart.Substring(posn+1,lastPart.Length-posn-1);
				posn = lastPart.IndexOfAny(separator);
				if ((posn == -1) & (lastPart.Length > 0))
				{
					styleText = styleText + "\n" +
					"<LINK href=\"" + lastPart + "\" type=\"text/css\" rel=\"stylesheet\">";
				}
			}

			return styleText;
		}


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			output.Write(SetStyleText());
		}

		#region Properties
		private string pageName = "";
		/// <summary>
		/// Defines the Resource file which contains the name/value for stylesheet
		/// </summary>
		public string PageName
		{
			get
			{
				return pageName;
			}
			set
			{
				pageName = value;
			}
		}

		private string resourceLocation = "";

		/// <summary>
		/// The base location of the resource files
		/// </summary>
		public string ResourceLocation
		{
			get
			{
				return resourceLocation;
			}
			set
			{
				resourceLocation = value;
			}
		}

		/// <summary>
		/// The Assembly that is using the Style Control
		/// </summary>
		private static Assembly owningAssembly;

		/// <summary>
		/// The Assembly to use for Resources
		/// </summary>
		public Assembly Assembly
		{
			get
			{
				return owningAssembly;
			}
			set
			{
				owningAssembly = value;
			}
		}
		#endregion

	}
}
