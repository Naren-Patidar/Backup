using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Shell32;

namespace Tesco.Framework.Common.Utilities.ZipLibrary
{
    class ZipShell : Archive
    {
        private ZipShell() { }
        public ZipShell(String sPath)
        {
            strPath = sPath;
        }
        public override int SaveArchive()
        {
            CreateZip(strPath);
            foreach (String strFile in lFiles)
                ZipCopyFile(strFile);
            return 1;
        }
        public bool CreateZip(String strZipFile)
        {
            try
            {
                ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
                byte[] baHeader = Encoding.ASCII.GetBytes(("PK" + (char)5 + (char)6).PadRight(22, (char)0));

                /*Overwrite an existing zip file's CreationTime and delete it*/
                if (File.Exists(strZipFile))
                {
                    File.SetCreationTime(strZipFile, DateTime.Now.ToLocalTime());
                    File.Delete(strZipFile);
                }

                /*Create new Zip File*/
                FileStream fs = System.IO.File.Create(strZipFile);                

                fs.Write(baHeader, 0, baHeader.Length);
                fs.Flush();
                fs.Close();
                fs = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        private bool ZipCopyFile(String strFile)
        {
            Shell Shell = new Shell();
            int iCnt = Shell.NameSpace(strPath).Items().Count;
            Shell.NameSpace(strPath).CopyHere(strFile, 0); // Copy file in Zip
            System.Threading.Thread.Sleep(2000);
            if (Shell.NameSpace(strPath).Items().Count == (iCnt + 1))
            {
                System.Threading.Thread.Sleep(100);
            }
            return true;
        }
    }
    public class ZipShellCreator : ArchiveCreator
    {
        private ZipShellCreator() { }
        public ZipShellCreator(String sPath)
        {
            strPath = sPath;
        }
        public override Archive GetArchieve()
        {
            return new ZipShell(strPath);
        }
    }
}