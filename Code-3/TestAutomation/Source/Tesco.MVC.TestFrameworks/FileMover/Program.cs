using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace FileCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string source = ConfigurationManager.AppSettings["source"];
                string destination = ConfigurationManager.AppSettings["destination"];

                List<DirectoryInfo> countries = new List<DirectoryInfo>();
                DirectoryInfo sourceDir = new DirectoryInfo(source);
                DirectoryInfo destinationDir = new DirectoryInfo(destination);
                countries = sourceDir.GetDirectories().ToList();
                countries.RemoveAll(c => c.Name.StartsWith("Archived"));
                Console.WriteLine("List of countries loaded.");
                WriteLog("List of countries loaded.");
                foreach (DirectoryInfo country in countries)
                {
                    Console.WriteLine(string.Format("Moving report for country : {0} , from : '{1}'.", country.Name, country.FullName));
                    WriteLog(string.Format("======================Moving report for country : {0} , from : '{1}'.=======================", country.Name, country.FullName));                    
                    List<DirectoryInfo> categories = new List<DirectoryInfo>();
                    categories = country.GetDirectories().ToList();
                    foreach (DirectoryInfo category in categories)
                    {
                        Console.WriteLine(string.Format("Moving report for category : {0}, from : {1}.", category.Name, category.FullName));
                        WriteLog(string.Format("+++++++++++++++Moving report for category : {0}, from : {1}.++++++++++++++++", category.Name, category.FullName));
                        List<FileInfo> reports = category.GetFiles().ToList();
                        reports = (from f in reports
                                  where f.LastWriteTime > DateTime.Now.AddDays(-1)
                                  select f).ToList();
                        Console.WriteLine(string.Format("Reports Count : {0}", reports.Count));
                        WriteLog(string.Format("Reports Count : {0}", reports.Count));
                        foreach (FileInfo report in reports)
                        {
                            //clear destination directory
                            string destinationDirectory = Path.Combine(destination, country.Name, category.Name);
                            DirectoryInfo d = new DirectoryInfo(destinationDirectory);
                            if (d.Exists)
                            {
                                WriteLog("------------------------Starting ClearDirectory---------------------------");
                                ClearDirectory(d, country.Name, category.Name);
                                WriteLog(string.Format("------------------------Directory '{0}' Cleared---------------------------", d.FullName));
                            }
                            else
                            {
                                WriteLog("------------------------Creating ClearDirectory---------------------------");
                                d.Create();
                                WriteLog(string.Format("-------------------------Directory '{0}' Created----------------------------",d.FullName));
                            }

                            string destinationFile = Path.Combine(destination, country.Name, category.Name, report.Name);
                            Console.WriteLine(string.Format("Copeying {0} to {1}.", report.FullName, destinationFile));
                            WriteLog(string.Format("Copeying {0} to {1}.", report.FullName, destinationFile));
                            try
                            {
                                report.CopyTo(destinationFile);
                            }
                            catch(Exception ex) 
                            {
                                WriteLog(ex.Message + ex.StackTrace);
                            }
                        }
                    }
                }
                Console.WriteLine("All Done");
                WriteLog("All Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press Enter key to exit.");
                WriteLog(ex.Message + ex.StackTrace);                
            }
        }

        public static void ClearDirectory(DirectoryInfo dir, string country, string category)
        {
            try
            {
                List<FileInfo> allFiles = dir.GetFiles().ToList();
                WriteLog(string.Format("For Country {0}, category {1}, there are {2} files present in virtual directory", country, category, allFiles.Count));
                foreach (FileInfo f in allFiles)
                {
                    string ArchivedDir = ConfigurationManager.AppSettings["ArchivedReports"];
                    WriteLog(string.Format("Directory '{0}' - checking", ArchivedDir));
                    if (!string.IsNullOrEmpty(ArchivedDir))
                    {
                        DirectoryInfo adi = new DirectoryInfo(ArchivedDir);
                        if (!adi.Exists)
                        {
                            WriteLog(string.Format("Directory '{0}' - Not existed", adi.FullName));
                            adi.Create();
                            WriteLog(string.Format("Directory '{0}' - Created", adi.FullName));
                        }
                        adi = new DirectoryInfo(Path.Combine(adi.FullName, country));
                        if (!adi.Exists)
                        {
                            WriteLog(string.Format("Directory '{0}' - Not existed", adi.FullName));
                            adi.Create();
                            WriteLog(string.Format("Directory '{0}' - Created", adi.FullName));
                        }
                        adi = new DirectoryInfo(Path.Combine(adi.FullName, category));
                        if (!adi.Exists)
                        {
                            WriteLog(string.Format("Directory '{0}' - Not existed", adi.FullName));
                            adi.Create();
                            WriteLog(string.Format("Directory '{0}' - Created", adi.FullName));
                        }
                        try
                        {
                            WriteLog(string.Format("Moving file {0} to Archieve {1}", f.FullName, adi.FullName));
                            f.MoveTo(Path.Combine(adi.FullName, f.Name));
                        }
                        catch
                        {
                            try
                            {
                                f.Delete();
                                WriteLog(string.Format("file {0} is deleted", f.FullName));
                            }
                            catch { }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message + ex.StackTrace);
            }
        }

        public static void WriteLog(string message)
        {
            try
            {
                FileInfo logFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"));
                if(logFile.LastWriteTime.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    logFile.Delete();
                }
                message = string.Format("{0}{1} - {2}", Environment.NewLine, DateTime.Now, message);
                File.AppendAllText(logFile.FullName, message);
            }
            catch { }
        }
    }
}
