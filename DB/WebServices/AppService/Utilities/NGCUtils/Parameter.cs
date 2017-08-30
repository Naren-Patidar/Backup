using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Tesco.NGC.Utils
{

	#region Header
	///
	/// <summary>
	/// AppArrayList is derived from the class ArrayList
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.00" day="09" month="08" year="2002">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class Parameter {

		private const string parameterQuote = "@";

		private string name;
		private Regex regex;

		/// <summary>
		/// Construct a parameter
		/// </summary>
		/// <param name="name">The name of the parameter</param>
		public Parameter(
			string name) {

			this.name = parameterQuote + name + parameterQuote;
			this.regex = new Regex(this.name);
		}

		/// <summary>
		/// Replace reference to the parameter in the sentence with a value
		/// </summary>
		/// <param name="originalSentence">The sentence</param>
		/// <param name="parameterValue">The value</param>
		/// <returns>The sentence with the value</returns>
		public string Replace(
			string originalSentence,
			string parameterValue) {

			try {
				// Replace the view Id parameter with the view Object Id
				string sentence = this.regex.Replace(originalSentence,parameterValue);
				// Return the amended SQL statement
				return sentence;
			} catch (Exception e) {
				CrmServiceException e2 = new CrmServiceException(
					"Server",
					"InternalError",
					"Parameter.Replace",e,this.name,originalSentence,parameterValue);
				throw e2;
			}
		}

		/// <summary>
		/// Return the parameter's name
		/// </summary>
		public string Name { get { return this.name; } }
	}
}