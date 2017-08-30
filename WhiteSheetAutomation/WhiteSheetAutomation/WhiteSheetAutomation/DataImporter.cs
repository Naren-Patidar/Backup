using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace WhiteSheetAutomation
{
    public class DataImporter
    {
        public static DataTable ExtractDataFromInputFile(string Path)
        {
            Utility.WriteApplicationSummaryLogs(Constants.strStartProcess);
            Utility.WriteApplicationSummaryLogs(Constants.strStartInputFileParsing);
            DataTable table;
            int LineNo = 1;
            try
            {
                StreamReader reader = new StreamReader(Path);
                string inputLine = "";
                string[] values = null;

                //DataTable table = new DataTable("MyDataTable");
                table = new DataTable("MyDataTable");
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("Printer", typeof(string));
                table.Columns.Add("Section", typeof(string));
                table.Columns.Add("BaseStock", typeof(string));
                table.Columns.Add("Action", typeof(string));
                table.Columns.Add("ConditionToBeChecked", typeof(string));
                table.Columns.Add("Query", typeof(string));

                while ((inputLine = reader.ReadLine()) != null)
                {
                    values = inputLine.Split('|');
                    if (values.Length == 6)
                    {
                        string[] arrBaseStock = values[2].Split(',');
                        foreach (string str in arrBaseStock)
                        {
                            if (str.Length > 0)
                            {
                                table.Rows.Add(LineNo, values[0], values[1], str, values[3], values[4], values[5]);
                            }
                        }
                        //for (int i = 0; i < values.Length; i++)
                        //{
                        //    if (i < values.Length && i == 3)
                        //    {
                        //        table.Rows.Add(LineNo, values[i]);
                        //    }

                        //}
                    }
                    else
                    {
                        Utility.WriteFailureLogs("Error:: Line no:" + LineNo.ToString() + ", File: " + Path);
                    }
                    LineNo++;

                }
                reader.Close();
                Utility.WriteApplicationSummaryLogs(Constants.strEndInputFileParsing);
                return table; 
               
            }
            catch (Exception ex)
            {
                Utility.WriteApplicationSummaryLogs(Constants.strError);
                Utility.WriteFailureLogs("Error in File: " + Path + "Description:: " + ex.Message);
                return new DataTable("MyDataTable"); 
            }


        }
        public static Dictionary<string,DataTable> ProcessInputDataFile(DataTable table)
        {
            int i = 0;
            Dictionary<string, DataTable> TableDictionary = new Dictionary<string, DataTable>();
            try
            {
                int NoOfRecord = table.Rows.Count;    
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.AppConfig))
                {
                    //DataTable table = new DataTable("MyDataTable");
                    DataTable tablePDWS = new DataTable("tablePDWS");
                    tablePDWS.Columns.Add("ID", typeof(int));
                    tablePDWS.Columns.Add("HouseHoldID", typeof(int));
                    tablePDWS.Columns.Add("Cards", typeof(string));
                    tablePDWS.Columns.Add("ConditionToBeChecked", typeof(string));
                    tablePDWS.Columns.Add("BaseStock", typeof(string));
                    tablePDWS.Columns.Add("Section", typeof(string));
                    tablePDWS.Columns.Add("Printer", typeof(string));
                    tablePDWS.Columns.Add("Action", typeof(string));

                    //DataTable table = new DataTable("MyDataTable");
                    DataTable tableMDWS = new DataTable("tableMDWS");
                    tableMDWS.Columns.Add("ID", typeof(int));
                    tableMDWS.Columns.Add("Cards", typeof(string));
                    tableMDWS.Columns.Add("ConditionToBeChecked", typeof(string));
                    tableMDWS.Columns.Add("BaseStock", typeof(string));
                    tableMDWS.Columns.Add("Section", typeof(string));
                    tableMDWS.Columns.Add("Printer", typeof(string));
                    tableMDWS.Columns.Add("Action", typeof(string));

                    con.Open();
                    DataTable dt;
                    //int j = 0;
                    Utility.WriteApplicationSummaryLogs(Constants.strStartSQLQueryExecution);
                    for (i = 0; i < NoOfRecord; i++)
                    {
                        SqlCommand cmd = new SqlCommand(table.Rows[i]["Query"].ToString(), con);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = Convert.ToInt32(Utility.htCommon["SQLCommandTimeout"].ToString());// Sql command timeout set to 10 minits.  
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            tablePDWS.Rows.Add(i + 1, dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), table.Rows[i]["ConditionToBeChecked"].ToString(), table.Rows[i]["BaseStock"].ToString(), table.Rows[i]["Section"].ToString(), table.Rows[i]["Printer"].ToString(), table.Rows[i]["Action"].ToString());

                        }
                        else
                        {
                            tableMDWS.Rows.Add(i + 1, "", table.Rows[i]["ConditionToBeChecked"].ToString(), table.Rows[i]["BaseStock"].ToString(), table.Rows[i]["Section"].ToString(), table.Rows[i]["Printer"].ToString(), table.Rows[i]["Action"].ToString());
                        }
                    }
                    Utility.WriteApplicationSummaryLogs(Constants.strEndSQLQueryExecution);

                   
                    TableDictionary.Add("tablePDWS", tablePDWS);
                    TableDictionary.Add("tableMDWS", tableMDWS);
                }
                return TableDictionary; 

            }
            catch (SqlException sqlex)
            {
                Utility.WriteFailureLogs("SQL connection or Query syntex error:Description:: " + sqlex.Message + ", Query:: " + table.Rows[i]["Query"].ToString());
                return TableDictionary; 
            }
            catch (Exception ex)
            {
                Utility.WriteApplicationFailureLogs("Process failed, details:" + ex.Message);
                return TableDictionary; 
            }
        }
        public static void CreateAndLoadOutputFiles(Dictionary<string, DataTable> TableDictionary)
        {

            DataTable tablePDWS = TableDictionary["tablePDWS"];
            DataTable tableMDWS = TableDictionary["tableMDWS"];

            //--This is Root path for both mdws and pdws files. 
            string FilePath = Properties.Settings.Default.PDWS;
            //--Validate path
            string strOutput = FilePath.Substring(FilePath.Length - 1, 1);
            if (strOutput.Equals("\\"))
            {
                FilePath = FilePath.Substring(0, FilePath.Length - 1);
            }

            Utility.WriteApplicationSummaryLogs(Constants.strStartOutputFileCreation);

            if (tablePDWS.Rows.Count > 0)
            {
                ApplyConfiguration(tablePDWS, FilePath, "pdws");
                // ExportDataTableToExcel(tablePDWS, FilePath + "template_pdws");
            }
            if (tableMDWS.Rows.Count > 0)
            {
                ApplyConfiguration(tableMDWS, FilePath, "mdws");
                //ExportDataTableToExcel(tableMDWS, FilePath + "tempalete_mdws");
            }
            Utility.WriteApplicationSummaryLogs(Constants.strEndOutputFileCreation);
            Utility.WriteApplicationSummaryLogs(Constants.strEndProcess);
        }
        private static void ApplyConfiguration(DataTable dt, string strRootPath, string strFileType)
        {
            //--Sort the data
            DataView dv = dt.DefaultView;
            dv.Sort = "Printer desc, Section desc, BaseStock desc";
            dt = dv.ToTable();

            //--Filter data and write into excel
            DataTable tblExcelData = new DataTable("tblExcelData");
            tblExcelData.Columns.Add("ID", typeof(int));
            tblExcelData.Columns.Add("Cards", typeof(string));
            tblExcelData.Columns.Add("ConditionToBeChecked", typeof(string));
            tblExcelData.Columns.Add("BaseStock", typeof(string));
            //--Variables
            string strPrinter, strSection, strBaseStock = string.Empty;
            string strFilePath, strFileName, strMailingFileName, strMailingFilePath, strFileDirectory, strAction = string.Empty;
            strPrinter = strSection = strBaseStock;
            int iCount = 0;
            StringBuilder sb = new StringBuilder();

            foreach (DataRow dr in dt.Rows)
            {
                //--Get the household id's and converted into string
                if (sb.Length == 0 && strFileType.ToUpper().Equals("PDWS"))
                {
                    sb.Append(dr["HouseHoldID"].ToString());
                }
                else if (sb.Length > 0 && strFileType.ToUpper().Equals("PDWS"))
                {
                    sb.Append(", " + dr["HouseHoldID"].ToString());
                }
                else
                {
                    sb.Append("");
                }

                if (string.IsNullOrEmpty(strPrinter) || string.IsNullOrEmpty(strSection) || string.IsNullOrEmpty(strBaseStock))
                {
                    //--First row, Put data into table
                    AddNewRowToDataTable(tblExcelData, ref strPrinter, ref strSection, ref strBaseStock, ref iCount, dr, ref strAction);
                }
                else if (strPrinter == dr["Printer"].ToString() && strSection == dr["Section"].ToString() && strBaseStock == dr["BaseStock"].ToString())
                {
                    //--New row
                    AddNewRowToDataTable(tblExcelData, ref strPrinter, ref strSection, ref strBaseStock, ref iCount, dr, ref strAction);
                }
                else
                {
                    //--data is changed so send data to excel and clear datatable
                    //--1). configure file path(BasePath + Printer folder + Section folder + fileName  
                    strFileName = strBaseStock + "." + strSection + "." + strFileType + ".xls";
                    strFilePath = strRootPath + "\\" + strPrinter + "\\" + strSection + "\\" + strFileName;
                    strFileDirectory = strRootPath + "\\" + strPrinter + "\\" + strSection;
                    //--2). Push data to excel
                    ExportDataTableToExcel(tblExcelData, strFilePath, strFileDirectory, strFileType, strPrinter, strSection, strBaseStock, strAction);

                    //--2.1) Generate mailing file if data is present                   
                    if (strFileType.ToUpper().Equals("PDWS"))
                    {
                        strMailingFileName = strBaseStock + "." + strSection + "." + "data.txt";
                        strMailingFilePath = strRootPath + "\\" + strPrinter + "\\" + strSection + "\\" + strMailingFileName;
                        GenerateMailingFile(sb.ToString(), strMailingFilePath);
                    }

                    //--3). Clear table
                    tblExcelData.Rows.Clear();
                    iCount = 0;
                    sb.Length = 0;

                    //--First row, Put data into table
                    AddNewRowToDataTable(tblExcelData, ref strPrinter, ref strSection, ref strBaseStock, ref iCount, dr, ref strAction);
                }

            }
            //--Push the last datarows row to excel
            //--1). configure file path(BasePath + Printer folder + Section folder + fileName  
            strFileName = strBaseStock + "." + strSection + "." + strFileType + ".xls";
            strFilePath = strRootPath + "\\" + strPrinter + "\\" + strSection + "\\" + strFileName;
            strFileDirectory = strRootPath + "\\" + strPrinter + "\\" + strSection;
            //--2). Push data to excel
            ExportDataTableToExcel(tblExcelData, strFilePath, strFileDirectory, strFileType, strPrinter, strSection, strBaseStock, strAction);

            //--2.1) Generate mailing file if data is present         

            if (strFileType.ToUpper().Equals("PDWS"))
            {
                strMailingFileName = strBaseStock + "." + strSection + "." + "data.txt";
                strMailingFilePath = strRootPath + "\\" + strPrinter + "\\" + strSection + "\\" + strMailingFileName;
                GenerateMailingFile(sb.ToString(), strMailingFilePath);
            }

            //--3). Clear table
            tblExcelData.Rows.Clear();
            tblExcelData = null;
            iCount = 0;
        }
        private static void GenerateMailingFile(string strHouseHoldId, string strDestinationFilePath)
        {
            //--Input file path
            string strFilePath = System.Configuration.ConfigurationSettings.AppSettings.Get("GenericQueryPath");

            //--Read the query from the text file
            StreamReader reader = new StreamReader(strFilePath);

            string strGenericQuery = reader.ReadToEnd();

            //--Appent the household id into the query
            strGenericQuery = strGenericQuery.Replace("@HouseholdID", strHouseHoldId);

            //--Execute query and get data
            DataTable dt = Utility.GetDataTable(strGenericQuery);

            if (dt != null && dt.Rows.Count > 0)
            {
                //--Generate output file and dump data
                StreamWriter sw = new StreamWriter(strDestinationFilePath, false);

                foreach (DataRow dr in dt.Rows)
                {
                    sw.Write(dr[0].ToString());

                }
                sw.Flush();
                sw.Close();
                sw = null;
            }

            Utility.WriteSuccessLogs("Data Imported successfully into file :: " + strDestinationFilePath);

        }
        private static void AddNewRowToDataTable(DataTable tblExcelData, ref string strPrinter, ref string strSection, ref string strBaseStock, ref int iCount, DataRow dr, ref string strAction)
        {
            DataRow mydr = tblExcelData.NewRow();
            iCount += 1;
            mydr["ID"] = iCount.ToString();
            mydr["Cards"] = dr["Cards"].ToString();
            mydr["ConditionToBeChecked"] = dr["ConditionToBeChecked"].ToString();
            mydr["BaseStock"] = dr["BaseStock"].ToString();
            strPrinter = dr["Printer"].ToString();
            strSection = dr["Section"].ToString();
            strBaseStock = dr["BaseStock"].ToString();
            strAction = dr["Action"].ToString();
            tblExcelData.Rows.Add(mydr);
        }
        private static bool ExportDataTableToExcel(DataTable dt, string filepath, string fileDirectory, string strFileType, string strPrinterName, string strSectionName, string strBaseStock, string strAction)
        {

            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            Excel.Range oRange;

            try
            {
                //--Validate excel file
                ValidateFileStructure(filepath, fileDirectory, strFileType);
                // Start Excel and get Application object. 
                oXL = new Excel.Application();
                // Set some properties 
                oXL.Visible = false;
                oXL.DisplayAlerts = false;

                //Open a workbook. 
                oWB = oXL.Workbooks.Open(filepath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
                //xlWBook = xlApp.Workbooks.Open(@"E:\temp.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                // Get the Active sheet 
                oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                // oSheet.Name = "Data";
                //--Populate header information: PaperPosition,CheckNamePosition,ActionPosition
                Excel.Range cell = (Excel.Range)oSheet.Evaluate(Utility.htCommon["PaperPosition"].ToString());
                if (cell != null) cell.Value2 = strBaseStock + "(" + strPrinterName + ")";
                cell = (Excel.Range)oSheet.Evaluate(Utility.htCommon["CheckNamePosition"].ToString());
                if (cell != null) cell.Value2 = strSectionName;
                cell = (Excel.Range)oSheet.Evaluate(Utility.htCommon["ActionPosition"].ToString());
                if (cell != null) cell.Value2 = strAction;

                int rowCount = Convert.ToInt32(Utility.htCommon["ExcelStartRowIndexForData"]);
                int ColumnCount = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    ColumnCount = Convert.ToInt32(Utility.htCommon["ExcelStartColumnIndexForData"]);
                    for (int i = 1; i <= dt.Columns.Count; i++)
                    {
                        // Add the header the first time through 
                        if (rowCount == 2)
                        {
                            // oSheet.Cells[17, i] = dt.Columns[i - 1].ColumnName;
                        }
                        oSheet.Cells[rowCount, ColumnCount] = dr[i - 1].ToString();
                        ColumnCount += 1;
                    }
                    rowCount += 1;
                }

                // Resize the columns 
                //Excel.Range C1 = oSheet.Cells[1, 1];
                //20130808: Changes from Narendra
                Excel.Range C1 = (Excel.Range)oSheet.Cells[Convert.ToInt32(Utility.htCommon["ExcelStartRowIndexForData"]), Convert.ToInt32(Utility.htCommon["ExcelStartColumnIndexForData"])];
                Excel.Range C2 = (Excel.Range)oSheet.Cells[rowCount, ColumnCount];

                oRange = oSheet.get_Range(C1, C2);
                oRange.EntireColumn.AutoFit();

                // Save the sheet and close 
                oSheet = null;
                oRange = null;
                oWB.Save();
                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oWB = null;

                oXL.Quit();
                oXL = null;
                Utility.WriteSuccessLogs("Data Imported successfully into file :: " + filepath);
            }
            catch (Exception ex)
            {
                Utility.WriteApplicationFailureLogs("Process failed, details:" + ex.Message);
            }
            finally
            {
                // Clean up 
                // NOTE: When in release mode, this does the trick 
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return true;

        }
        private static void ValidateFileStructure(string strFilePath, string strFileDirectory, string strFileType)
        {
            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            Excel.Range oRange;

            try
            {

                //1. Start Excel and get Application object. 
                oXL = new Excel.Application();
                // Set some properties 
                oXL.Visible = false;
                oXL.DisplayAlerts = false;

                //2.Open a Template workbook. 
                //2.1 Get the path of template file
                string strTemplateFilePath = string.Empty;
                if (strFileType.ToUpper().Equals("PDWS"))
                {
                    strTemplateFilePath = System.Configuration.ConfigurationSettings.AppSettings.Get("PDWSFileTemplatePath");
                }
                else
                {
                    strTemplateFilePath = System.Configuration.ConfigurationSettings.AppSettings.Get("MDWSFileTemplatePath");
                }

                oWB = oXL.Workbooks.Open(strTemplateFilePath, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);

                // Get the Active sheet 
                oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                oSheet.Name = "Data";
                //3. Check folder is exists or not
                bool flag = Directory.Exists(strFileDirectory);
                if (!flag)
                {
                    Directory.CreateDirectory(strFileDirectory);
                }
                //4. Save the new file
                oWB.SaveAs(strFilePath, Excel.XlFileFormat.xlWorkbookNormal,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Excel.XlSaveAsAccessMode.xlExclusive,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value);
                //oWB.Save();
                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oWB = null;
                oXL.Quit();
                oXL = null;
            }
            catch (Exception ex)
            {
                Utility.WriteApplicationFailureLogs("Process failed, details:" + ex.Message);
            }
            finally
            {
                // Clean up 
                // NOTE: When in release mode, this does the trick 
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }
    }
}
