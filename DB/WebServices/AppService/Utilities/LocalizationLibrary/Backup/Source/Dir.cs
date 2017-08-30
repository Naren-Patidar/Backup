#region Using
using System;
using System.IO;
using System.Collections;
#endregion

namespace Fujitsu.eCrm.Generic.LocalizationLibrary
{
	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// 
	/// <summary>
	/// Process a directory and determine the files that make
	/// it up.
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
	
	internal class Dir
	{
		#region Attributes
		private int currentDepthLevel;
		private Stack currentDir;
		#endregion

		#region Constants
		private const string backSlash = @"\";
		#endregion

		#region Enumerations
		/// <summary>
		/// The format of files to be reported.
		/// </summary>
		internal enum DirType
		{
			/// <summary>
			/// The full path of the file will be returned.
			/// </summary>
			Full,
			/// <summary>
			/// The relative path of the file from the starting
			/// directory will be returned.
			/// </summary>
			Relative
		}

		/// <summary>
		/// The case in which the file list will be constructed.
		/// </summary>
		internal enum DirCase
		{
			/// <summary>
			/// Do not change - whatever is on the disk.
			/// </summary>
			Normal,
			/// <summary>
			/// All lowercase.
			/// </summary>
			Lower,
			/// <summary>
			/// All uppercase.
			/// </summary>
			Upper
		}
		#endregion

		#region Properties
		private string directoryPath;
		/// <summary>
		/// The full path of the directory to investigate.
		/// </summary>
		internal string DirectoryPath
		{
			get { return this.directoryPath; }
			set { this.directoryPath = value; }
		}

		private string fileFilter;
		/// <summary>
		/// The file extensions to search for e.g. '*.cs *.txt'
		/// </summary>
		internal string FileFilter
		{
			get { return this.fileFilter; }
			set { this.fileFilter = value; }
		}

		private int depthLevel;
		/// <summary>
		/// The depth to search into the subdirectories. The level
		/// 0 is the current directory.
		/// </summary>
		internal int DepthLevel
		{
			get { return this.depthLevel; }
			set { this.depthLevel = value; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Intializes a new instance of the Directory class.
		/// </summary>
		/// <param name="directory">The full path of the directory to process.</param>
		/// <param name="filter">Exclude any files with these extensions.</param>
		/// <param name="level">The depth to search into the subdirectories.
		/// The level 0 is the current directory. Specify -1 for all
		/// sub-directories.</param>
		internal Dir(
			string directory,
			string filter,
			int level)
		{
			// Check that the directory exists.
			if (Directory.Exists(directory))
			{
				this.directoryPath = directory;
				if (filter != null)
					this.fileFilter = filter;
				this.depthLevel = level;
				this.currentDir = new Stack();
			}
			else 
			{
				// The directory doesn't exist, so throw an exception.
				throw new Exception(Localization.GetMessage("MissingRootDirectory",directory));
			}
		}

		/// <summary>
		/// Intializes a new instance of the Directory class.
		/// </summary>
		/// <param name="directory">The full path of the directory to process.</param>
		internal Dir(string directory) : this(directory,null,-1) {}

		/// <summary>
		/// Intializes a new instance of the Directory class.
		/// </summary>
		/// <param name="directory">The full path of the directory to process.</param>
		/// <param name="filter">Exclude any files with these extensions.</param>
		private Dir(string directory,string filter) : this(directory,filter,-1) {}
		#endregion

		#region Methods

		#region GetAllFiles
		/// <summary>
		/// Recursively retrieve all files that make up the directory and
		/// all sub-directories. 
		/// </summary>
		/// <param name="dirType">The type file path name to be stored.</param>
		/// <param name="dirCase">The case the list of files is required in.</param>
		/// <returns>An arraylist of files located.</returns>
		internal ArrayList GetAllFiles(DirType dirType,DirCase dirCase)
		{
			// Set the current depth level back to 0;
			this.currentDepthLevel = 0;
			// Set the current directory as root.
			this.currentDir.Push(String.Empty);
			return this.ChangeArrayCase(this.GetAllFiles(this.directoryPath,dirType),dirCase);
		}

		/// <summary>
		/// Recursively retrieve all files that make up the directory and
		/// all sub-directories. 
		/// </summary>
		/// <param name="dirType">The type file path name to be stored.</param>
		/// <returns>An arraylist of files located.</returns>
		internal ArrayList GetAllFiles(DirType dirType)
		{
			return this.GetAllFiles(dirType,DirCase.Normal);
		}
		#endregion

		#region GetFiles
		/// <summary>
		/// Get the files contained in the current directory.
		/// </summary>
		/// <param name="dirType">The type file path name to be stored.</param>
		/// <param name="dirCase">The case the list of files is required in.</param>
		/// <returns>An arraylist of the files located.</returns>
		internal ArrayList GetFiles(DirType dirType,DirCase dirCase)
		{
			return this.ChangeArrayCase(this.GetFiles(this.directoryPath,dirType),dirCase);
		}

		/// <summary>
		/// Get the files contained in the current directory.
		/// </summary>
		/// <param name="dirType">The type file path name to be stored.</param>
		/// <returns>An arraylist of the files located.</returns>
		internal ArrayList GetFiles(DirType dirType)
		{
			return this.GetFiles(dirType,DirCase.Normal);
		}
		#endregion

		#endregion

		#region Private Methods

		#region Change Array Case
		/// <summary>
		/// Change the case of an arraylist.
		/// </summary>
		/// <param name="arrayList">The arraylist to process.</param>
		/// <param name="dirCase">The case required.</param>
		/// <returns>The updated arraylist.</returns>
		private ArrayList ChangeArrayCase(
			ArrayList arrayList,
			DirCase dirCase)
		{
			ArrayList al = new ArrayList();
			foreach(string entry in arrayList)
			{
				switch (dirCase)
				{
					case DirCase.Lower:
						al.Add(entry.ToLower());
						break;
					case DirCase.Upper:
						al.Add(entry.ToUpper());
						break;
					default:
						al.Add(entry);
						break;
				}
			}
			return al;
		}
		#endregion

		#region Get Files
		/// <summary>
		/// Get the files that are contained only within the directory
		/// specified.
		/// </summary>
		/// <returns>An arraylist of files located.</returns>
		private ArrayList GetFiles(string directory,DirType dirType)
		{
			// Check that the directory exists.
			if (Directory.Exists(directory))
			{
				// Get the files that make up the directory.
				DirectoryInfo dirInfo = new DirectoryInfo(directory);
				FileInfo[] filesInfo = dirInfo.GetFiles(this.fileFilter == null ? "*" : this.fileFilter);
				// Create the arraylist to store the files.
				ArrayList fileNameList = new ArrayList();
				// Loop round the files and store.
				foreach (FileInfo fi in filesInfo)
				{
					switch(dirType)
					{
						case DirType.Full:
							fileNameList.Add(fi.FullName);
							break;
						case DirType.Relative:
							fileNameList.Add(fi.Name);
							break;
					}
				}
				return fileNameList;
			}
			else
			{
				// The directory doesn't exist, so throw an exception.
				throw new Exception(Localization.GetMessage("MissingDirectory",directory));
			}
		}
		#endregion

		#region GetAllFiles
		/// <summary>
		/// Get all files contained in the current directory and sub
		/// directories.
		/// </summary>
		/// <param name="startingDir">The directory to start looking.</param>
		/// <param name="dirType">The type of file list to be produced i.e.
		/// full path or short path.</param>
		/// <returns>An arraylist containing all files.</returns>
		private ArrayList GetAllFiles(string startingDir,DirType dirType)
		{
			// Get the files of the current directory.
			ArrayList fileNameList = new ArrayList();
			// Check that we're not too deep.
			if ((this.depthLevel >= this.currentDepthLevel) && (this.depthLevel != -1))
			{
				fileNameList = this.GetFiles(startingDir,dirType);
				// Get the list of directories that make up this folder.
				DirectoryInfo dirInfo = new DirectoryInfo(startingDir);
				DirectoryInfo[] dirsInfo = dirInfo.GetDirectories();
				// Loop round all the directories
				foreach(DirectoryInfo di in dirsInfo)
				{
					// If we're in here, then we must be in a directory and
					// need to set the current dir.
					if (this.currentDepthLevel > 1)
						this.currentDir.Push((string)this.currentDir.Peek() + backSlash + di.Name);
					else
						this.currentDir.Push(di.Name);
					// Increase the depth.
					this.currentDepthLevel++;
					// Get a list of files within this directory.
					ArrayList diFiles = this.GetAllFiles(di.FullName,dirType);
					// Get the name of the current directory.
					string dir = (string)this.currentDir.Pop();
					// Decrease the depth.
					this.currentDepthLevel--;
					// Tag this list onto the current arraylist.
					foreach(string fileName in diFiles)
					{
						switch (dirType)
						{
							case DirType.Full:
								fileNameList.Add(fileName);
								break;
							case DirType.Relative:
								fileNameList.Add(dir + backSlash + fileName);
								break;
						}
					}
				}
			}
			return fileNameList;
		}
		#endregion

		#endregion
	}
}