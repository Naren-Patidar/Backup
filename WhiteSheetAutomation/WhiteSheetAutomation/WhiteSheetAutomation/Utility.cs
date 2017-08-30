using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO; 
using System.Collections;

namespace WhiteSheetAutomation
{
    public class Utility
    {
        public static Hashtable htCommon = new Hashtable();

        static Utility()
        {
            SetConfigurableValues();
        }     
        public static DataTable GetDataTable(string strQuery)
        {
            SqlConnection conn = new SqlConnection();
            DataTable dt = new DataTable();
            try
            {
                conn = new SqlConnection(Properties.Settings.Default.AppConfig);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = Convert.ToInt32(Utility.htCommon["SQLCommandTimeout"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
               // MessageBox.Show(ex.Message);
                return dt;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                //return dt;
            }
           
        }
        public static void WriteSuccessLogs(string msg)
        {
            //string strFilePath = Application.StartupPath + "\\SuccessLogs.txt";
            StreamWriter writer = File.AppendText(htCommon["ApplicationLogs"].ToString() + "\\" + Constants.strFileSuccesLogs);
            writer.WriteLine(System.DateTime.Now + " : INFO : " + msg);
            writer.Close();
            writer.Dispose();  
        }
        public static void WriteFailureLogs(string msg)
        {
          //  string strFilePath = Application.StartupPath + "\\FailureLogs.txt";
            StreamWriter writer = File.AppendText(htCommon["ApplicationLogs"].ToString() + "\\" + Constants.strFileFailureLogs);
            writer.WriteLine(System.DateTime.Now + " : INFO : " + msg);
            writer.Close();
            writer.Dispose();  
        }
        public static void WriteApplicationFailureLogs(string msg)
        {
           // string strFilePath = Application.StartupPath + "\\ApplicationFailureLogs.txt";
            StreamWriter writer = File.AppendText(htCommon["ApplicationLogs"].ToString() + "\\" + Constants.strFileApplicationFailureLogs);
            writer.WriteLine(System.DateTime.Now + " : INFO : " + msg);
            writer.Close();
            writer.Dispose();  
        }
        public static void WriteApplicationSummaryLogs(string msg)
        {           
            StreamWriter writer = File.AppendText(htCommon["ApplicationLogs"].ToString() + "\\" + Constants.strFileApplicationSummary);
            writer.WriteLine(System.DateTime.Now + " : INFO : " + msg);
            writer.Close();
            writer.Dispose();
        }
        private static void SetConfigurableValues()
        {
            string RootPathForOutputFiles = System.Configuration.ConfigurationSettings.AppSettings.Get("RootPathForOutputFiles");
            MyProperties mp = new MyProperties();
            mp.MDWS = RootPathForOutputFiles;
            Properties.Settings.Default["MDWS"] = mp.MDWS;
            mp.PDWS = RootPathForOutputFiles;
            Properties.Settings.Default["PDWS"] = mp.PDWS;
            mp.ApplicationConfig = System.Configuration.ConfigurationSettings.AppSettings.Get("ConnectionString"); 
            Properties.Settings.Default["AppConfig"] = mp.ApplicationConfig;

            Properties.Settings.Default.Save();

            htCommon.Add("PaperPosition", System.Configuration.ConfigurationSettings.AppSettings.Get("PaperPosition"));
            htCommon.Add("CheckNamePosition", System.Configuration.ConfigurationSettings.AppSettings.Get("CheckNamePosition"));
            htCommon.Add("ActionPosition", System.Configuration.ConfigurationSettings.AppSettings.Get("ActionPosition"));
            htCommon.Add("ExcelStartRowIndexForData", System.Configuration.ConfigurationSettings.AppSettings.Get("ExcelStartRowIndexForData"));
            htCommon.Add("ExcelStartColumnIndexForData", System.Configuration.ConfigurationSettings.AppSettings.Get("ExcelStartColumnIndexForData"));
            htCommon.Add("InputFile", System.Configuration.ConfigurationSettings.AppSettings.Get("InputFile"));
            htCommon.Add("ApplicationLogs", ValidateFilePath(System.Configuration.ConfigurationSettings.AppSettings.Get("ApplicationLogs").ToString()));
            htCommon.Add("IsUserInterfaceRequired", System.Configuration.ConfigurationSettings.AppSettings.Get("IsUserInterfaceRequired").ToString());
            htCommon.Add("SQLCommandTimeout", System.Configuration.ConfigurationSettings.AppSettings.Get("SQLCommandTimeout").ToString());
           
            //htCommon.Add("FailureLogs", System.Configuration.ConfigurationSettings.AppSettings.Get("FailureLogs"));
            //htCommon.Add("ApplicationFailureLogs", System.Configuration.ConfigurationSettings.AppSettings.Get("ApplicationFailureLogs"));


        }
        public static string ValidateFilePath(string strPath)
        {
            //--Validate path
            string strOutput = strPath.Substring(strPath.Length - 1, 1);
            if (strOutput.Equals("\\"))
            {
                strPath = strPath.Substring(0, strPath.Length - 1);
            }          
            //2. Check folder is exists or not
            bool flag = Directory.Exists(strPath);
            if (!flag)
            {
                Directory.CreateDirectory(strPath);
            }

            return strPath; 
        }
    }
}
