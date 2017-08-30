using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CSVToExcelConverter
{
    public class DataSetToExcel
    {

        #region Member Variables

        //Declare a Hashtable object.
        Hashtable htExcelProcesses;

        //Declare a Application object.
        Application excel;

        //Declare a _Workbook object.
        _Workbook workbook;

        //Declare a _Worksheet object.
        _Worksheet worksheet;

        //Declare a Range object.
        Range range;

        #endregion Member Variables

        #region Public Methods

        /// <summary>
        /// Method to export customer details to excel.
        /// </summary>
        /// <param name="dsCustomersDetails"></param>
        /// <param name="outputPath">C:\TSMAutomationLog\SlovakiaRewardedCustomer.xls</param>
        /// <returns></returns>
        public Boolean ExportToExcel(DataSet dsCustomersDetails, string outputPath)
        {
            //Declare a boolean variable.
            Boolean customerDetailsExportedSuccessfully = true;

            try
            {
                //Check excel processes.
                CheckExcellProcesses();

                //Initialize Application object.
                excel = new Application();

                //Set excel visibility to false.
                excel.Visible = false;

                //Get a new workbook.
                workbook = (_Workbook)(excel.Workbooks.Add(Missing.Value));

                //Get a new worksheet.
                worksheet = (_Worksheet)workbook.ActiveSheet;

                //Declare a int variable.
                int row = 2;

                //Loop through the details.
                for (int j = 0; j < dsCustomersDetails.Tables[0].Columns.Count; j++)
                {
                    //Add to the worksheet cells.
                    worksheet.Cells[1, j + 1] = dsCustomersDetails.Tables[0].Columns[j].ColumnName;
                }

                //For each row, print the values of each column.
                for (int rowNo = 0; rowNo < dsCustomersDetails.Tables[0].Rows.Count; rowNo++)
                {
                    //Loop through the details.
                    for (int colNo = 0; colNo < dsCustomersDetails.Tables[0].Columns.Count; colNo++)
                    {
                        //Add to the worksheet cells.
                        worksheet.Cells[row, colNo + 1] = "'" + dsCustomersDetails.Tables[0].Rows[rowNo][colNo].ToString();
                    }
                    //Increment row count by 1.
                    row++;
                }

                //Initialize Range object.
                range = worksheet.get_Range("A1", "IV1");

                //Set AutoFit.
                range.EntireColumn.AutoFit();

                //Set excel visibility to false.
                excel.Visible = false;

                //Set excel UserControl to false.
                excel.UserControl = false;

                //Check if file exist.
                if (File.Exists(outputPath))
                {
                    //Delete file if exist.
                    File.Delete(outputPath);
                }

                //Save workbook.
                workbook.SaveAs(outputPath, XlFileFormat.xlWorkbookNormal, null, null, false, false, XlSaveAsAccessMode.xlShared, false, false, null, null, null);
            }

            //Catch the exception.
            catch (Exception ex)
            {
                //Set false to return variable.
                customerDetailsExportedSuccessfully = false;

            }
            finally
            {
                //Need all following code to clean up and remove all references.
                //Make null to workbook.
                workbook.Close(null, null, null);

                //Close workbook.
                excel.Workbooks.Close();

                //Quit excel.
                excel.Quit();

                //Release Range Com object.
                Marshal.ReleaseComObject(range);

                //Release Excel Com object.
                Marshal.ReleaseComObject(excel);

                //Release Worksheet Com object.
                Marshal.ReleaseComObject(worksheet);

                //Release Workbook Com object.
                Marshal.ReleaseComObject(workbook);

                //Kill the excel.
                KillExcel();

                //Force final cleanup.
                GC.Collect();
            }
            //Return true or false.
            return customerDetailsExportedSuccessfully;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Method to check excel processes.
        /// </summary>
        private void CheckExcellProcesses()
        {
            Process[] AllProcesses = Process.GetProcessesByName("EXCEL");

            htExcelProcesses = new Hashtable();

            int count = 0;

            foreach (Process ExcelProcess in AllProcesses)
            {
                htExcelProcesses.Add(ExcelProcess.Id, count);

                count = count + 1;
            }
        }

        /// <summary>
        /// Method to kill excel.
        /// </summary>
        private void KillExcel()
        {
            //Declare Process array.
            Process[] AllProcesses = Process.GetProcessesByName("EXCEL");

            //Check to kill the right process
            foreach (Process ExcelProcess in AllProcesses)
            {
                if (htExcelProcesses.ContainsKey(ExcelProcess.Id) == false)
                {
                    //Kill excel process.
                    ExcelProcess.Kill();
                }
            }
            //Set null to AllProcesses array.
            AllProcesses = null;
        }

        #endregion Private Methods

    }
}
