#region Using
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
#endregion

namespace Tesco.NGC.Utils
{
	#region Header
	///
	/// <summary>
	/// A class to parse command line arguments and return a string
	/// dictionary with parameter and value.
	/// </summary>
	/// <development> 
	///		<version number="1.00" day="23" month="09" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header

	public class ConsoleParameterParser
	{
		#region Enumeration
		/// <summary>
		/// A list of allowed parameter types.
		/// </summary>
		public enum ParameterType
		{
			/// <summary>Parameter is System.Int64</summary>
			IsLong,
			/// <summary>Parameter is System.Int32</summary>
			IsInt,
			/// <summary>Parameter is System.String</summary>
			IsString
		};
		#endregion

		#region Attributes
		private string[] arguments;
		private StringDictionary parameters;
		#endregion

		#region Properties
		/// <summary>Get the args passed from the console application.</summary>
		public string[] Arguments { get { return this.arguments; } }
		/// <summary>Get the StringDictionary option and value pairs.</summary>
		public StringDictionary Parameters { get { return this.parameters; } }
		#endregion

		#region Statics
		private static Regex argumentExpression = new Regex(@"\-(?<option>[\w|\?|\.|\\|\:])\s*(?<value>[\w|\?|\.|\\|\:]*)");
		#endregion

		#region Constructor
		/// <summary>
		/// Instantiate a new instance of the object.
		/// </summary>
		public ConsoleParameterParser(string[] args)
		{
			this.arguments = args;
			this.parameters = new StringDictionary();
			string argument = String.Join(" ",args);
			foreach (Match argumentMatch in argumentExpression.Matches(argument)) 
			{
				string argOption = argumentMatch.Groups["option"].Value;
				string argValue = argumentMatch.Groups["value"].Value;
				this.parameters.Add(argOption,argValue);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get the StringDictionary containing the argument option and value
		/// pairs.
		/// </summary>
		/// <returns></returns>
		public StringDictionary GetParameters()
		{
			return this.parameters;
		}

		/// <summary>
		/// Get a particular parameter value in it's requested type.
		/// </summary>
		/// <param name="parameterOption">The parameter option to get.</param>
		/// <param name="checkEmpty">Check whether the parameter has had a
		/// value returned.</param>
		/// <param name="parameterValue">The value of the parameter being
		/// requested.</param>
		/// <param name="parameterType">Check parameter is off this type
		/// and parse if it is.</param>
		/// <returns>True if the parameter is obtained and checks have been
		/// passed.</returns>
		public bool GetParameter(
			string parameterOption,
			ParameterType parameterType,
			bool checkEmpty,
			out object parameterValue)
		{
			parameterValue = null;
			if (!this.parameters.ContainsKey(parameterOption))
				return false;
			string p = this.parameters[parameterOption];
			if (checkEmpty)
			{
				if (StringUtils.IsStringEmpty(p))
					return false;
			}
			try 
			{
				switch (parameterType)
				{
					case ParameterType.IsInt:
						parameterValue = Convert.ToInt32(p);
						break;
					case ParameterType.IsLong:
						parameterValue = Convert.ToInt64(p);
						break;
					default:
						parameterValue = p;
						break;
				}
			} catch { return false; }
			return true;
		}

		/// <summary>
		/// Get a particular string parameter value.
		/// </summary>
		/// <param name="parameterOption">The parameter option to get.</param>
		/// <param name="parameterValue">The value of the parameter being
		/// requested.</param>
		/// <returns>True if the parameter is obtained and checks have been
		/// passed.</returns>
		public bool GetParameter(
			string parameterOption,
			out object parameterValue)
		{
			return this.GetParameter(parameterOption,ParameterType.IsString,false,out parameterValue);
		}

		/// <summary>
		/// Get a particular parameter value in it's requested type.
		/// </summary>
		/// <param name="parameterOption">The parameter option to get.</param>
		/// <param name="parameterValue">The value of the parameter being
		/// requested.</param>
		/// <param name="parameterType">Check parameter is off this type
		/// and parse if it is.</param>
		/// <returns>True if the parameter is obtained and checks have been
		/// passed.</returns>
		public bool GetParameter(
			string parameterOption,
			ParameterType parameterType,
			out object parameterValue)
		{
			return this.GetParameter(parameterOption,parameterType,false,out parameterValue);
		}

		/// <summary>
		/// Get a particular string parameter value.
		/// </summary>
		/// <param name="parameterOption">The parameter option to get.</param>
		/// <param name="checkEmpty">Check whether the parameter has had a
		/// value returned.</param>
		/// <param name="parameterValue">The value of the parameter being
		/// requested.</param>
		/// <returns>True if the parameter is obtained and checks have been
		/// passed.</returns>
		public bool GetParameter(
			string parameterOption,
			bool checkEmpty,
			out object parameterValue)
		{
			return this.GetParameter(parameterOption,ParameterType.IsString,checkEmpty,out parameterValue);
		}

		/// <summary>
		/// Check if a parameter has been supplied.
		/// </summary>
		/// <param name="parameterOption">The parameter option to get.</param>
		/// <returns>True if the parameter has been supplied.</returns>
		public bool GetParameter(
			string parameterOption
			)
		{
			object parameterValue = null;
			return this.GetParameter(parameterOption,ParameterType.IsString,false,out parameterValue);
		}
		#endregion
	}
}
