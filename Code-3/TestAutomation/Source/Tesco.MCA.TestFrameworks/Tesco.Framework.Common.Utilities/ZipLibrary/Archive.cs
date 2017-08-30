using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.ZipLibrary
{
    // FACTORY CLASS
    public abstract class Archive
    {
        internal String strPath;
        internal List<String> lError;
        internal List<String> lFiles;
        public String[] ErrorList
        {
            get
            {
                return lError.ToArray();
            }
        }
        public Archive()
        {
            lFiles = new List<string>();
            lError = new List<string>();
        }
        public Boolean AddFile(String strFile)
        {
            lFiles.Add(strFile);
            return true;
        }
        public abstract int SaveArchive();
    }
    // CREATOR CLASS
    public abstract class ArchiveCreator
    {
        internal String strPath;
        public abstract Archive GetArchieve();
    }
}
