using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using CompareConfigurations.Entities;
using CompareConfigurations.Classes;
using System.Data;
using System.Xml.Linq;

namespace CompareConfigurations
{
    class Program
    {
        static AllCultures Cultures = new AllCultures();
        static Culture CurrentCulture = new Culture();
        static string CurrentCountry = string.Empty;
        static string DefaultExcelFile = string.Empty;
        static List<DbConfigurationItem> ExcelConfigs = new List<DbConfigurationItem>();
        static List<DbConfigurationItem> XmlConfigs = new List<DbConfigurationItem>();

        static void Main(string[] args)
        {
            try
            {
                Cultures = Utility.LoadCultures();
                ReadCountry();
                ReadExcel();
                ReadXML();
                CompareConfigs();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
                Console.ReadKey();
            }
        }

        private static void CompareConfigs()
        {
            StringBuilder txtFile = new StringBuilder();
            List<DbConfigurationItem> itemsNotInExcel = XmlConfigs.FindAll(c => ExcelConfigs.FindAll(ec => c.ConfigurationType == ec.ConfigurationType && c.ConfigurationName == ec.ConfigurationName).Count == 0);
            Console.WriteLine("=================================================================");
            Console.WriteLine("Configuraitons not present in Excel File");
            Console.WriteLine("=================================================================");
            txtFile.Append("Configuraitons not present in Excel File" + Environment.NewLine);
            txtFile.Append("=================================================================" + Environment.NewLine);
            if (itemsNotInExcel.Count == 0)
            {
                WriteSuccess("There are no such configurations.");
                txtFile.Append("There are no such configurations." + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("ConfigurationType | ConfigurationName");
                Console.WriteLine("=================================================================");
                txtFile.Append("ConfigurationType | ConfigurationName" + Environment.NewLine + "=================================================================" + Environment.NewLine);
                foreach (DbConfigurationItem config in itemsNotInExcel)
                {
                    WriteError(string.Format("{0} | {1}", config.ConfigurationType, config.ConfigurationName));
                    txtFile.Append(string.Format("{0} | {1} , {2}", config.ConfigurationType, config.ConfigurationName, Environment.NewLine));
                }
                WriteError(string.Format("There are {0} such configs.", itemsNotInExcel.Count));
                txtFile.Append(string.Format("There are {0} such configs. {1}", itemsNotInExcel.Count, Environment.NewLine));
            }
            List<DbConfigurationItem> itemsNotInXML = ExcelConfigs.FindAll(c => XmlConfigs.FindAll(xc => c.ConfigurationType == xc.ConfigurationType && c.ConfigurationName == xc.ConfigurationName).Count == 0);
            Console.WriteLine("=================================================================");
            Console.WriteLine("Configuraitons not present in XML File");
            Console.WriteLine("=================================================================");
            txtFile.Append("Configuraitons not present in XML File" + Environment.NewLine);
            txtFile.Append("=================================================================" + Environment.NewLine);
            if (itemsNotInXML.Count == 0)
            {
                WriteSuccess("There are no such configurations.");
                txtFile.Append("There are no such configurations." + Environment.NewLine);
            }
            else
            {
                Console.WriteLine("ConfigurationType | ConfigurationName");
                Console.WriteLine("=================================================================");
                txtFile.Append("ConfigurationType | ConfigurationName" + Environment.NewLine + "=================================================================" + Environment.NewLine);
                foreach (DbConfigurationItem config in itemsNotInXML)
                {
                    WriteError(string.Format("{0} | {1}", config.ConfigurationType, config.ConfigurationName));
                    txtFile.Append(string.Format("{0} | {1} , {2}", config.ConfigurationType, config.ConfigurationName, Environment.NewLine));
                }
                WriteError(string.Format("There are {0} such configs.", itemsNotInXML.Count));
                txtFile.Append(string.Format("There are {0} such configs. {1}", itemsNotInXML.Count, Environment.NewLine));
            }
            List<DbConfigurationItem> itemsMismatched = (from t in XmlConfigs
                                                        join u in ExcelConfigs on
                                                        new
                                                        {
                                                            JoinProperty1 = t.ConfigurationName,
                                                            JoinProperty2 = t.ConfigurationType
                                                        }
                                                        equals
                                                        new
                                                        {
                                                            JoinProperty1 = u.ConfigurationName,
                                                            JoinProperty2 = u.ConfigurationType
                                                        }
                                                         where !t.ConfigurationValue1.Trim().EqualSpecial(u.ConfigurationValue1.Trim()) || !t.ConfigurationValue2.Trim().EqualSpecial(u.ConfigurationValue2) || !t.IsDeleted.EqualSpecial(u.IsDeleted)
                                                        select t).ToList(); 
            
            Console.WriteLine("=================================================================");
            Console.WriteLine("Configuraitons Values does not match");
            Console.WriteLine("=================================================================");
            txtFile.Append("Configuraitons Values does not match" + Environment.NewLine + "=================================================================" + Environment.NewLine);
            if (itemsMismatched.Count == 0)
            {
                WriteSuccess("There are no such configurations.");
            }
            else
            {
                Console.WriteLine("ConfigurationType | ConfigurationName | ConfigurationValue1 | ConfigurationValue2 | IsDeleted");
                Console.WriteLine("=================================================================");
                txtFile.Append("ConfigurationType | ConfigurationName | ConfigurationValue1 | ConfigurationValue2 | IsDeleted" + Environment.NewLine + "=================================================================" + Environment.NewLine);
                foreach (DbConfigurationItem config in itemsMismatched)
                {
                    WriteError(string.Format("Values from XML: {0} | {1} | {2} | {3} | {4}", config.ConfigurationType, config.ConfigurationName, config.ConfigurationValue1 , config.ConfigurationValue2, config.IsDeleted));
                    txtFile.Append(string.Format("{0} | {1} | {2} | {3} | {4} {5}", config.ConfigurationType, config.ConfigurationName,config.ConfigurationValue1, config.ConfigurationValue2, config.IsDeleted, Environment.NewLine));
                }
                WriteError(string.Format("There are {0} such configs.", itemsMismatched.Count));
                txtFile.Append(string.Format("There are {0} such configs. {1}", itemsMismatched.Count, Environment.NewLine));
            }

            string outFile = ConfigurationManager.AppSettings.AllKeys.Contains("OutFile") ? ConfigurationManager.AppSettings["OutFile"] : string.Empty;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(outFile, false))
            {
                file.WriteLine(txtFile.ToString());
            }

        }        

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void ReadCountry()
        {
            Console.Write("Enter the Country : ");
            CurrentCountry = Console.ReadLine();

            bool chkCountry = Cultures.Cultures.FindAll(c => c.Code.ToUpper() == CurrentCountry.ToUpper()).Count > 0;
            while (!chkCountry)
            {
                WriteError("Please enter valid country name. Choosse from Below List:");
                StringBuilder list = new StringBuilder();
                Cultures.Cultures.ForEach(c => list.Append(c.Code + Environment.NewLine));
                Console.Write(list.ToString());
                Console.Write("Enter the Country : ");
                CurrentCountry = Console.ReadLine();
                chkCountry = Cultures.Cultures.FindAll(c => c.Code.ToUpper() == CurrentCountry.ToUpper()).Count > 0;
            }
            CurrentCulture = Cultures.Cultures.Find(c => c.Code.ToUpper() == CurrentCountry.ToUpper());
        }

        private static void ReadExcel()
        {
            DefaultExcelFile = (ConfigurationManager.AppSettings.AllKeys.Contains("DefaultExcelFile")) ? ConfigurationManager.AppSettings["DefaultExcelFile"] : string.Empty;
            DefaultExcelFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultExcelFile));
            DataTable dtConfigs = Utility.exceldata(DefaultExcelFile, CurrentCulture.WorksheetName);
            ExcelConfigs = dtConfigs.AsEnumerable()
            .Select(dr =>
                new DbConfigurationItem
                {
                    ConfigurationType = dr.GetValue<string>("ConfigurationType"),
                    ConfigurationName = dr.GetValue<string>("ConfigurationName"),
                    ConfigurationValue1 = dr.GetValue<string>("ConfigurationValue1"),
                    ConfigurationValue2 = dr.GetValue<string>("ConfigurationValue2"),
                    IsDeleted = dr.GetValue<string>("IsDeleted")
                }).ToList();
        }

        private static void ReadXML()
        {
            string outFolder = (ConfigurationManager.AppSettings.AllKeys.Contains("OutputDirectory")) ? ConfigurationManager.AppSettings["OutputDirectory"] : string.Empty;
            outFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outFolder));
            string configurationFileName = (ConfigurationManager.AppSettings.AllKeys.Contains("ConfigurationFileName")) ? ConfigurationManager.AppSettings["ConfigurationFileName"] : string.Empty;
            configurationFileName = Path.Combine(outFolder, CurrentCulture.Country, configurationFileName);
            string xml = Utility.LoadXMLFile(configurationFileName);
            XDocument xDoc = XDocument.Parse(xml);
            XmlConfigs = (from t in xDoc.Descendants("ActiveDateRangeConfig")
                               select new DbConfigurationItem
                               {
                                   ConfigurationType = t.Element("ConfigurationType").GetValue<string>(),
                                   ConfigurationName = t.Element("ConfigurationName").GetValue<string>(),
                                   ConfigurationValue1 = t.Element("ConfigurationValue1").GetValue<string>(),
                                   ConfigurationValue2 = t.Element("ConfigurationValue2").GetValue<string>(),
                                   IsDeleted = t.Element("IsDeleted").GetValue<string>()
                               }).ToList();
        }
    }
}
