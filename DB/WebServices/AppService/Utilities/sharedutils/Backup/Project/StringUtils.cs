#region Using
using System;
using System.Collections;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
 	///		<version number="1.01" day="09" month="12" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet>WP/Barcelona/022</work_packet>
	///			<description>Add a routine to extract field from a string with start
	///			positions and lengths. Useful for fixed length records.</description>
	///		</version>
	///		<version number="1.00" day="07" month="10" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker>Lawrie Griffiths</checker>
	///			<work_packet>WP/Barcelona/014</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// <summary>
	/// A class to provide useful methods for manipulation of strings.
	/// </summary>

	#endregion

	public class StringUtils
	{
		#region Static

		#region Is String Empty?
		/// <summary>
		/// Check whether a string is null.
		/// </summary>
		/// <param name="checkString">The string to be checked.</param>
		/// <returns>Returns true if the string is null or empty.</returns>
		public static bool IsStringEmpty (string checkString) 
		{
			if (checkString == null || checkString == "")
				return true;
			else
				return false;
		}
		#endregion

		#region Extract String
		/// <summary>
		/// Extract string from another string depending upon start and length values. If
		/// a failure occurs, then the strings extracted so far will be returned and the
		/// result will be false.
		/// </summary>
		/// <param name="startPositions">An array list containing the list of start positions.</param>
		/// <param name="lengths">An array list containing the list of lengths.</param>
		/// <param name="processString">The string to have the bits extracted.</param>
		/// <param name="strings">The extracted strings.</param>
		/// <param name="extractionMessage">A message will be placed here if there are
		/// any problems.</param>
		/// <param name="zeroIndex">Treat start positions as beginning from 0 and not
		/// 1.</param>
		/// <returns>Return true if successful.</returns>
		public static bool ExtractStrings (
			ArrayList startPositions,
			ArrayList lengths,
			string processString,
			bool zeroIndex,
			out string[] strings,
			out string extractionMessage)
		{
			int i;
			// Loop round all elements. This exit value is less than
			// the number in the ArrayList as this starts from 1 and
			// not 0.
			string delimiter = "|";
			strings = null;
			extractionMessage = String.Empty;
			char charDelimiter = Convert.ToChar(delimiter);
			string delimitedString = String.Empty;
			bool first = true;
			bool result = true;
			// Check if we need to make any adjustments due to index
			// start.
			int adjust = 0;
			if (!zeroIndex)
				// Need to subtract 1 from every start position.
				adjust = 1;
			for (i = 0; i < startPositions.Count; i++)
			{
				// Get the start and length of each string. Subtract
				int startPosition = (int)startPositions[i] - adjust;
				int length = (int)lengths[i];
				// Extract the string and add to new string.
				try 
				{
					if (first)
					{
						delimitedString += processString.Substring(startPosition,length);
						first = false;
					}
					else
					{
						delimitedString += delimiter + processString.Substring(startPosition,length);
					}
				}
				catch (Exception exception)
				{
					// There's been an error, set flag and break.
					result = false;
					extractionMessage = exception.Message;
					break;
				}
			}
			// Set the strings array.
			strings = delimitedString.Split(charDelimiter);
			return result;
		}
		#endregion

		#region Sort Array
		/// <summary>
		/// Sort the contents of an array and return.
		/// </summary>
		/// <param name="array">The array to be sorted.</param>
		/// <returns>The sorted array.</returns>
		public static string[] SortArray(string[] array)
		{
			// If the array is empty, just return.
			int length = array.GetUpperBound(0);
			if (length < 0)
				// The array is empty.
				return array;
			string[] newArray = new string[length + 1];
			// Copy the array into an array list
			ArrayList a = new ArrayList(array);
			a.Sort();
			a.CopyTo(newArray);
			return newArray;
		}
		#endregion

		#region Remove Leading Zeros
		/// <summary>
		/// Remove leading zeros from a string.
		/// </summary>
		/// <param name="s">The string to be processed.</param>
		/// <returns>A string with the leading zeros removed.</returns>
		public static string RemoveLeadingZeros(
			string s
			)
		{
			// Check that we have a value to start with.
			if (StringUtils.IsStringEmpty(s))
			{
				// The value is empty, so just return an empty string.
				return String.Empty;
			}
			else
			{
				// The value isn't empty, so remove the leading zeros.
				string chopped = s.TrimStart(new char[]{'0'});
				// Check that we haven't chopped all the characters if the
				// string was all zeros.
				if (chopped == String.Empty)
					chopped = "0";
				return chopped;
			}
		}
		#endregion

		#region Trim Leading Spaces
		/// <summary>
		/// Remove any leading whitespace from a string.
		/// </summary>
		/// <param name="s">The string to be processed.</param>
		/// <returns>A string with the leading spaces removed.</returns>
		public static string TrimLeadingSpaces(
			string s)
		{
			return s.TrimStart(new char[] {' '});
		}
		#endregion

		#region Trim Trailing Spaces
		/// <summary>
		/// Remove any trailing whitespace from a string.
		/// </summary>
		/// <param name="s">The string to be processed.</param>
		/// <returns>A string with the trailing spaces removed.</returns>
		public static string TrimTrailingSpaces(
			string s)
		{
			return s.TrimEnd(new char[] {' '});
		}
		#endregion

		#region Remove Strings
		/// <summary>
		/// Remove a supplied list of strings from a string.
		/// </summary>
		/// <param name="removeString">The string to be amended.</param>
		/// <param name="removeList">The list of strings to check for and remove.</param>
		/// <returns>The amended string.</returns>
		public static string RemoveStrings(
			string removeString,
			params string[] removeList
			)
		{
			string amended = removeString;
			foreach (string remove in removeList)
				amended = amended.Replace(remove,"");
			return amended;
		}

		#endregion

		#endregion
	}
}
