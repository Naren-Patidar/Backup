using System;
using System.Xml.Schema;

namespace Tesco.NGC.Utils{

	/// <summary>
	/// Used by XML Validate Reader to collect error messages
	/// </summary>
	public class XmlSyntaxError {

		private static string errorMessage = String.Empty;
		private static bool hasError = false;

		/// <summary>
		/// Called when an error is found
		/// </summary>
		/// <param name="x"></param>
		/// <param name="arg"></param>
		public static void Handle(object x, ValidationEventArgs arg) {
			if (XmlSyntaxError.hasError) {
				XmlSyntaxError.errorMessage += "\n";
			}
			XmlSyntaxError.errorMessage += arg.Message;
			XmlSyntaxError.hasError = true;
		}

		/// <summary>
		/// The Collection of Errors identified
		/// </summary>
		public static string ErrorMessage {
			get { return XmlSyntaxError.errorMessage; }
		}

		/// <summary>
		/// Is there any errors
		/// </summary>
		public static bool HasError {
			get { return XmlSyntaxError.hasError; }
		}
	}
}
