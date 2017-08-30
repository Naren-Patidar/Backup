#region Using
using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using System.Xml;
using Microsoft.ApplicationBlocks.ExceptionManagement;
#endregion

namespace Fujitsu.eCrm.Generic.LocalizationLibrary
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <development>
	///		<version number="1.00" day="05" month="06" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/058</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	/// <summary>
	/// Read data from localization files. To add new files, the
	/// application using this assembly must be restarted for
	/// them to be detected and parsed.
	/// 
	/// If a new file type is required, then add a new entry to
	/// the FileType enumeration and also add a new access method
	/// in a similar vain to GetLocalizedAttributeString().
	/// Everything else should hopefully then fall into place!!
	/// </summary>

	#endregion
	
	public class Localization
	{
		#region Statics
		/// <summary>
		/// The installed culture of the application.
		/// </summary>
		private static string iCulture;
		/// <summary>
		/// A hashtable holding all the localizaed files.
		/// </summary>
		private static Hashtable localize;
		/// <summary>
		/// A hashtable to hold the complete set of files processed for each
		/// type.
		/// </summary>
		private static Hashtable allFilePaths;
		/// <summary>
		/// The resource manager holding the error messages.
		/// </summary>
		private static ResourceManager errorMessages;
		#endregion

		#region Properties
		private static bool initialized;
		/// <summary>
		/// Has the localization files been processed yet?
		/// </summary>
		public static bool Intialized { get { return initialized; } }
		#endregion
        
		#region Enumeration
		/// <summary>
		/// A list of all the file types.
		/// </summary>
		internal enum FileType
		{
			language,
			style,
			validation,
			format,
			image
		}
		#endregion

		#region Constants
		internal const string placeHolder0 = "{0}";
		internal const string fileExtension = "*resx";
		private const string nodesXpath = "//data";
		private const string keyXpath = "@name";
		private const string valueXpath = "value/text()";
		#endregion

		#region Constructor
		static Localization() 
		{
			// Set the initialized flag to it's default value.
			initialized = false;
			// Intialise the storage for the localized string.
			localize = new Hashtable();
			// Intialise the storage for the file list.
			allFilePaths = new Hashtable();
			// Create the error messages resource manager.
			errorMessages = new ResourceManager("Fujitsu.eCrm.Generic.LocalizationLibrary.Resources.ErrorMessages",
					Assembly.GetExecutingAssembly());
		}
		#endregion

		#region Methods

		#region Intialize
		/// <summary>
		/// Intialize the hashtables for each of the file types. This
		/// method should only be called once during it's lifetime. A
		/// check will be made to ensure that this happens.
		/// </summary>
		/// <param name="installedCulture">The installed culture of the application.</param>
		/// <param name="localizationDirectory">The root directory of the files to be
		/// processed to create the hashtables.</param>
		public static void Initialize(
			string installedCulture,
			string localizationDirectory
			)
		{
			// Check that the hashtables haven't already been created.
			if (!initialized)
			{
				// Check that the localization directory has a '\' on the end.
				// add a "\" to the end of the directory name if its not already there.
				if ((!localizationDirectory.EndsWith(@"\")) && (!localizationDirectory.EndsWith(@"/"))) 
					localizationDirectory += @"\";
				// Process each of the file types.
				foreach (FileType type in Enum.GetValues(typeof(FileType)))
				{
					// Convert the file type into a string.
					string fileTypeName = Enum.GetName(typeof(FileType),type);
					// Set-up the wildcard.
					string wildCard = fileTypeName + fileExtension;
					// Obtain the list of files to be processed.
					Dir localDir = new Dir(localizationDirectory,wildCard,1);
					// Obtain the relative file list and store in the hashtable.
					ArrayList filePaths = localDir.GetAllFiles(Dir.DirType.Full,Dir.DirCase.Lower);
					// Add these to the controlling hashtable.
					foreach (string filePath in filePaths)
						allFilePaths.Add(filePath.ToLower(),type);
					// Set-up the localization table.
					LocalizationTable locTable = new LocalizationTable();
					// Now process each of the files
					foreach(string filePath in filePaths)
					{
						// Attempt to load the file into an XML document. If it fails,
						// then just move onto the next
						try 
						{
							LocalizationFile file = new LocalizationFile(filePath,type);
							locTable.Store(nodesXpath,keyXpath,valueXpath,file);
						}
						catch (Exception exception)
						{
							// Publish the exception and then carry onto the next
							// file.
							ExceptionManager.Publish(exception);
							continue;
						}
					}
					// The LocalizationTable has now been constructed for this file type,
					// so store in the main hashtable.
					localize.Add(type,locTable);
				}
				// Need to now set-up a file system watcher to see if any of these
				// files are updated, as we'll have to re-process them.
				FileSystemWatcher watcher = new FileSystemWatcher(localizationDirectory,fileExtension);
				// Set-up the events to occur when a file is changed.
				watcher.NotifyFilter = NotifyFilters.CreationTime;
				// Ensure the sub-directories are included.
				watcher.IncludeSubdirectories = true;
				// Enable the events.
				watcher.EnableRaisingEvents = true;
				// Set the file changed event.
				watcher.Changed += new FileSystemEventHandler(FileChange);
				// Check that the culture supplied is valid.
				try { CultureInfo ci = new CultureInfo(installedCulture); }
				catch (Exception exception) { throw new Exception(GetMessage("InvalidCulture",installedCulture),exception); }
				// Set the installed culture
				iCulture = installedCulture.ToLower();
				// Set the intialized flag to be true.
				initialized = true;
			}
		}
		#endregion

		#region Get Localized Attribute String
		/// <summary>
		/// Translates the provided attribute using the Language.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedAttributeString(
			string name
			)
		{
			return GetAttribute(FileType.language,name,null);
		}

		/// <summary>
		/// Translates the provided attribute using the Language.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <param name="args">The values to insert into the template message.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedAttributeString(
			string name,
			params string[] args
			)
		{
			return GetAttribute(FileType.language,name,args);
		}
		#endregion

		#region Get Localized Style String
		/// <summary>
		/// Translates the provided attribute using the Style.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedStyleString(
			string name
			)
		{
			return GetAttribute(FileType.style,name,null);
		}

		/// <summary>
		/// Translates the provided attribute using the Style.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <param name="args">The values to insert into the template message.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedStyleString(
			string name,
			params string[] args
			)
		{
			return GetAttribute(FileType.style,name,args);
		}
		#endregion

		#region Get Localized Validation String
		/// <summary>
		/// Translates the provided attribute using the Validation.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedValidationString(
			string name
			)
		{
			return GetAttribute(FileType.validation,name,null);
		}

		/// <summary>
		/// Translates the provided attribute using the Validation.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <param name="args">The values to insert into the template message.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedValidationString(
			string name,
			params string[] args
			)
		{
			return GetAttribute(FileType.validation,name,args);
		}
		#endregion

		#region Get Localized Image String
		/// <summary>
		/// Translates the provided attribute using the Image.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedImageString(
			string name
			)
		{
			return GetAttribute(FileType.image,name,null);
		}
		/// <summary>
		/// Translates the provided attribute using the Image.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <param name="args">The values to insert into the template message.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedImageString(
			string name,
			params string[] args
			)
		{
			return GetAttribute(FileType.image,name,args);
		}
		#endregion

		#region Get Localized Format String
		/// <summary>
		/// Translates the provided attribute using the Format.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedFormatString(
			string name
			)
		{
			return GetAttribute(FileType.format,name,null);
		}

		/// <summary>
		/// Translates the provided attribute using the Format.resx language variant.
		/// </summary>
		/// <param name="name">The neutral name of a phrase.</param>
		/// <param name="args">The values to insert into the template message.</param>
		/// <returns>The localized phrase.</returns>
		public static string GetLocalizedFormatString(
			string name,
			params string[] args
			)
		{
			return GetAttribute(FileType.format,name,args);
		}
		#endregion

		#endregion

		#region Private Methods

		#region Get Attribute
		private static string GetAttribute(
			FileType type,
			string name,
			string[] args
			)
		{
			// Because an application will be running, if an exception
			// is encountered, then we don't want to bring the whole
			// thing down, so surround in a try catch block.
			try 
			{
				// If the name is null or String.Empty, then return an empty
				// string.
				if ((name == null) || (name == String.Empty))
					return String.Empty;
				// Check that a Localization tables exists for the required
				// filetype.
				if (localize.ContainsKey(type))
				{
					string l;
					lock (localize)
					{
						// Get the LocalizationTable from the main hashtable.
						LocalizationTable locTable = (LocalizationTable)localize[type];
						// Get the current culture from the thread.
						string cCulture = Thread.CurrentThread.CurrentCulture.Name.ToLower();
						// Get the string from the LocalizationTable.
						l = locTable.Get(iCulture,cCulture,name);
					}
					if (args == null)
						return l;
					else
						return String.Format(l,args);
				}
				else
				{
					// A LocalizationTable doesn't exist for the file type,
					// so just return the orignal neutral key.
					return name;
				}
			}
			catch (Exception exception)
			{
				// Something happened that stopped the localized string
				// from being obtained, so publish the event and just
				// the key.
				ExceptionManager.Publish(exception);
				return name;
			}
		}
		#endregion

		#region File Change
		// Define the event handlers.
		private static void FileChange(
			object source, FileSystemEventArgs e
			)
		{
			Refresh(e.FullPath);
		}
		#endregion

		#region Refresh
		private static void Refresh(
			string filePath
			)
		{
			// Because an application will be running, if a bad file
			// is encountered durong a refresh, then we don't want
			// an exception to occur, so surround in a try catch
			// block.
			try
			{
				// Check that the file is one of the files parsed during
				// the initialise and get the type. If it isn't then just
				// do nothing.
				string filePathLower = filePath.ToLower();
				if (allFilePaths.ContainsKey(filePathLower))
				{
					// Get the type.
					FileType type = (FileType)allFilePaths[filePathLower];
					// Create a LocalizationFile object.
					LocalizationFile file = new LocalizationFile(filePathLower,type);
					// Get the LocalizationTable object for this type.
					LocalizationTable locTable = (LocalizationTable)localize[type];
					// Store the new file into the localizationTable.
					locTable.Store(nodesXpath,keyXpath,valueXpath,file);
					// Re-store the LocalizationTable in the main table. Lock the hashtable
					// before storing.
					lock (localize)
                        localize[type] = locTable;
				}
			}
			catch (Exception exception)
			{
				// Something has happened, so just publish the exception.
				ExceptionManager.Publish(exception);
			}
		}
		#endregion

		#region Get Message
		/// <summary>
		/// Get an error message out of the ErrorMessages resource
		/// file.
		/// </summary>
		/// <param name="key">The key used to locate the message.</param>
		/// <param name="replace">The string to replace the placeholder with.</param>
		/// <returns>A localizased string.</returns>
		internal static string GetMessage(
			string key,
			string replace
			)
		{
			return errorMessages.GetString(key).Replace(placeHolder0,replace);
		}
		#endregion

		#endregion
	}
}
