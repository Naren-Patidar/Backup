#region Using
using System;
using System.IO;
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
	/// Open a localization type file and processes.
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

	internal class LocalizationFile
	{
		#region Properties
		private string filePath;
		/// <summary>
		/// The full path of the file.
		/// </summary>
		internal string FilePath { get { return this.filePath; } }

		private Localization.FileType type;
		/// <summary>
		/// The type of the localization file.
		/// </summary>
		internal Localization.FileType Type { get { return this.type;} }

		private XmlDocument localizationDoc;
		/// <summary>
		/// The processed file loaded into an XML document.
		/// </summary>
		internal XmlDocument LocalizationDoc { get { return this.localizationDoc; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Initialise an instance of the class and set the
		/// file to process.
		/// </summary>
		/// <param name="file">The full path of the file to process.</param>
		/// <param name="fileType">The FileType enumeration entry of this file.</param>
		internal LocalizationFile(
			string file,
			Localization.FileType fileType)
		{
			if (File.Exists(file))
			{
				this.type = fileType;
				this.filePath = file;
				// Load the file into the XML document.
				this.localizationDoc = new XmlDocument();
				try 
				{
					this.localizationDoc.Load(file);
				}
				catch (Exception exception)
				{
					throw new Exception(Localization.GetMessage("InvalidXml",file),exception);
				}
			}
		}
		#endregion
	}
}
