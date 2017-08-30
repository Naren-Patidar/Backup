#region Using
using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
#endregion

namespace Fujitsu.eCrm.Generic.LocalizationLibrary
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <summary>
	/// Create the hashtable required for storing the localization
	/// information.
	/// </summary>
	/// 
	/// <development>
	///		<version number="1.00" day="05" month="06" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/058</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	#endregion

	public class LocalizationTable
	{
		#region Attributes
		private Hashtable locTabHash;
		#endregion

		#region Constants
		/// <summary>
		/// [culturedir].[type].[culture].resX
		/// </summary>
		private const string regEx1 = @"^.*(?<culturedir>\w{2}-\w{2})\\{0}\.(?<culture>\w{2}-\w{2})\.resx$";
		/// <summary>
		/// [culturedir].[type].resX
		/// </summary>
		private const string regEx2 = @"^.*(?<culturedir>\w{2}-\w{2})\\{0}\.resx$";
		/// <summary>
		/// [type].[culture].resX
		/// </summary>
		private const string regEx3 = @"^.*{0}\.(?<culture>\w{2}-\w{2})\.resx$";
		/// <summary>
		/// [type].resX
		/// </summary>
		private const string regEx4 = @"^.*{0}\.resx$";
		/// <summary>
		/// The key to use for the reference culture.
		/// </summary>
		private const string baseKey = "base";
		#endregion

		#region Constructor
		/// <summary>
		/// Create a new instance of the class.
		/// </summary>
		internal LocalizationTable()
		{
			this.locTabHash = new Hashtable();
		}
		#endregion

		#region Methods

		#region Store
		/// <summary>
		/// Add an entry to the current LocalizationTable.
		/// </summary>
		/// <param name="nodesXpath">The xpath to use to locate the nodes to extract
		/// the key-value information.</param>
		/// <param name="keyXpath">The xpath to use to apply to the node to extract
		/// the key value.</param>
		/// <param name="valueXpath">The xpath to use to apply to the node to extract
		/// the value to store with the key.</param>
		/// <param name="file">The file object to process.</param>
		internal void Store(
			string nodesXpath,
			string keyXpath,
			string valueXpath,
			LocalizationFile file
			)
		{
			// Check that the XML document isn't null, else return.
			if (file.LocalizationDoc == null)
				return;
			// Get the list of nodes to process.
			XmlNodeList nodes = file.LocalizationDoc.SelectNodes(nodesXpath);
			// Check that we have some nodes, else return.
			if (nodes.Count < 1)
				return;
			// Create a new hashtable
			Hashtable ht = new Hashtable();
			// Process each of the nodes and extract the values.
			foreach (XmlNode node in nodes)
			{
				// Get the value to use as the key
				XmlNode keyNode = node.SelectSingleNode(keyXpath);
				// If the node is null, then just continue with the next
				// value as the key of the hastable must have a value.
				if (keyNode == null)
					continue;
				string keyValue = keyNode.Value;
				// Check that the key doesn't already exist. If it does
				// then just continue onto the next.
				if (ht.ContainsKey(keyValue))
					continue;
				// Get the value.
				XmlNode valueNode = node.SelectSingleNode(valueXpath);
				// If the value node is null then set to empty
				// string else use value.
				string valueValue;
				if (valueNode == null)
					valueValue = String.Empty;
				else
					valueValue = valueNode.Value;
				// Now store this value in the hashtable.
				ht.Add(keyValue,valueValue);
			}
			// Now determine the key to use
			string level1Key,level2Key;
			this.GetKeys(file.FilePath,file.Type,out level1Key,out level2Key);
			// Store the new hashtable.
			this.StoreHashtable(ht,level1Key,level2Key);
		}
		#endregion

		#region Get
		/// <summary>
		/// Get an entry from the LocalizationTable.
		/// </summary>
		/// <param name="level1Key">The level 1 key to use.</param>
		/// <param name="level2Key">The level 2 key to use.</param>
		/// <param name="name">The name of the neutral localization key.</param>
		/// <returns>The matched string, otherwise if no match found, the key.</returns>
		internal string Get(
			string level1Key,
			string level2Key,
			string name
			)
		{
			// Need to search through the hashtables, dropping through
			// each of the levels if a key isn't found.
			string match;

			// Try <level1Key><level2Key>
			if (this.GetValue(level1Key,level2Key,name,out match))
				// Match found, return string.
				return match;

			// Try <level1Key><'base'>
			if (this.GetValue(level1Key,baseKey,name,out match))
				// Match found, return string.
				return match;

			// Try <'base'><level2Key>
			if (this.GetValue(baseKey,level2Key,name,out match))
				// Match found, return string.
				return match;

			// Try <'base'><'base'>
			if (this.GetValue(baseKey,baseKey,name,out match))
				// Match found, return string.
				return match;

			// No match found in any of the files, so just return the
			// key.
			return name;
		}
		#endregion

		#endregion

		#region Private Methods

		#region Get Value
		/// <summary>
		/// Obtain a value from the LocalizationTable depending
		/// upon the supplied key.
		/// </summary>
		/// <param name="level1Key">The level 1 key to use.</param>
		/// <param name="level2Key">The level 2 key to use.</param>
		/// <param name="name">The name of key to look for.</param>
		/// <param name="match">The value obtained if a entry was found.</param>
		/// <returns>Rteurn true if a match has been found.</returns>
		private bool GetValue(
			string level1Key,
			string level2Key,
			string name,
			out string match
			)
		{
			// Intialise the out parameters.
			match = null;
			// Check if the level 1 key exists.
			if (this.locTabHash.ContainsKey(level1Key))
			{
				// The level 1 key did exist, so extract the
				// next level hashtable.
				Hashtable subHash = (Hashtable)this.locTabHash[level1Key];
				// See if the level 2 key exists.
				if (subHash.ContainsKey(level2Key))
				{
					// The level 2 key does exist, so extract the actual
					// localization hashtable.
					Hashtable locHash = (Hashtable)subHash[level2Key];
					// Now see if the neutral key supplied exists.
					if (locHash.ContainsKey(name))
					{
						// The neutral key does exist, so get the value
						// and return true;
						match = (string)locHash[name];
						return true;
					}
					else
					{
						// The neutral key didn't exist, so return false.
						return false;
					}
				}
				else
				{
					// The level 2 key didn't exist, so return false.
					return false;
				}
			}
			else
			{
				// The level 1 key didn't exist, so return false.
				return false;
			}
		}
		#endregion

		#region Get Keys
		/// <summary>
		/// Generate the key needed to store a hashtable in the
		/// current structure.
		/// </summary>
		/// <param name="filePath">The filepath from where the document
		/// came from.</param>
		/// <param name="fileType">The FileType enumeration entry that this
		/// filePath represents</param>
		/// <param name="level1Key">The level 1 key generated from the file path.</param>
		/// <param name="level2Key">The level 2 key generated from the file path.</param>
		private void GetKeys (
			string filePath,
			Localization.FileType fileType,
			out string level1Key,
			out string level2Key
			)
		{
			// Default keys.
			level1Key = baseKey;
			level2Key = baseKey;
			// Set-up the list of strings to be used for regular expressions.
			string[] regExs = new string[4] {regEx1,regEx2,regEx3,regEx4};
			// Recurse through each of the regular expressions and try to
			// obtain a match.
			foreach (string regEx in regExs)
			{
				// Replace the placeholder with the file
				string repRegEx = regEx.Replace(Localization.placeHolder0,
					Enum.GetName(typeof(Localization.FileType),fileType));
				Regex r = new Regex(repRegEx,RegexOptions.Compiled|RegexOptions.IgnoreCase);
				// Now attempt to match with the file path.
				Match m = r.Match(filePath);
				if (m.Success)
				{
					// A match has been found, so attempt to locate the required
					// strings i.e. culturedir and culture. Only set the key value
					// if a value has been returned.
					string cultureDir = m.Groups["culturedir"].Value;
					if (!IsStringEmpty(cultureDir))
						level1Key = cultureDir;
					string culture = m.Groups["culture"].Value;
					if (!IsStringEmpty(culture))
						level2Key = culture;
					// We've found the required values, so stop the search.
					break;
				}
			}
		}
		#endregion

		# region Is String Empty?
		private bool IsStringEmpty(
			string s
			)
		{
			if ((s == null) || (s == "") || (s.Length == 0))
				return true;
			else
				return false;
		}
		#endregion

		#region Store Hashtable
		/// <summary>
		/// Store a sub-hashtable in the LocalizationTable.
		/// </summary>
		/// <param name="hashtable">The hashtable object to store.</param>
		/// <param name="level1Key">The level 1 key to use.</param>
		/// <param name="level2Key">The level 2 key to use.</param>
		private void StoreHashtable(
			Hashtable hashtable,
			string level1Key,
			string level2Key
			)
		{
			// Check to see if the level 1 key exists.
			if (this.locTabHash.ContainsKey(level1Key))
			{
				// The level 1 key does exist, so get the sub
				// hashtable out.
				Hashtable ht = (Hashtable)this.locTabHash[level1Key];
				// Now check if the hastable being stored exists
				// already.
				if (ht.ContainsKey(level2Key))
					// The level 2 hashtable already exists, so
					// overwrite with the new hashtable.
					ht[level2Key] = hashtable;
				else
					// The level 2 hashtable does not yet exist,
					// so add it.
					ht.Add(level2Key,hashtable);
				// Now place back the level 2 holding hashtable.
				this.locTabHash[level1Key] = ht;
			}
			else
			{
				// The key doesn't yet exist, so create the sub
				// hashtable.
				Hashtable ht = new Hashtable();
				// Store the supplied hashtable in this new hashtable
				// using level 2 key.
				ht.Add(level2Key,hashtable);
				// Now store the new hashtable in the main hashtable using
				// the level 1 key.
				this.locTabHash.Add(level1Key,ht);
			}
		}
		#endregion

		#endregion
	}
}