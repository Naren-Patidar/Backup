using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;

namespace CSVToExcelConverter
{
    public class CSVToDataSet
    {

        #region Public Methods

        public void ProcessDirectory(string inputFilePath)
        {
            try
            {
                string outputFilePath = ConfigurationSettings.AppSettings["OutputFilePath"].ToString();

                //Process the list of files found in the directory.
                string[] inputFiles = Directory.GetFiles(inputFilePath);

                foreach (string inputFile in inputFiles)
                {
                    DataSetToExcel oDataSetToExcel = new DataSetToExcel();

                    int inputFilePathLength = Convert.ToInt32(ConfigurationSettings.AppSettings["InputFilePathLength"].ToString());

                    string outputFile = outputFilePath + ConfigurationSettings.AppSettings["OutputFileName"].ToString();

                    oDataSetToExcel.ExportToExcel(ConvertCsvToDataTable(inputFile, true), outputFile);

                    File.Move(inputFile, ConfigurationSettings.AppSettings["ProcessedFilePath"].ToString() + inputFile.Substring(inputFilePathLength));
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion Public Methods

        #region Private Methods

        private DataSet ConvertCsvToDataTable(string inputFilePath, bool isRowOneHeader)
        {
            try
            {
                DataSet csvDataSet = new DataSet();

                csvDataSet.Tables.Add();

                //no try/catch - add these in yourselfs or let exception happen
                String[] csvData = File.ReadAllLines(inputFilePath);

                //if no data in file ‘manually’ throw an exception
                if (csvData.Length == 0)
                {
                    throw new Exception("CSV File Appears to be Empty");
                }

                String[] headings = csvData[0].Split(',');

                int index = 0; //will be zero or one depending on isRowOneHeader

                if (isRowOneHeader) //if first record lists headers
                {
                    index = 1; //so we won’t take headings as data

                    //for each heading
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //replace spaces with underscores for column names
                        headings[i] = headings[i].Replace(" ", "_");

                        //add a column for each heading
                        csvDataSet.Tables[0].Columns.Add(headings[i], typeof(string));
                    }
                }
                else //if no headers just go for col1, col2 etc.
                {
                    for (int i = 0; i < headings.Length; i++)
                    {
                        //create arbitary column names
                        csvDataSet.Tables[0].Columns.Add("col" + (i + 1).ToString(), typeof(string));
                    }
                }

                //populate the DataTable
                for (int i = index; i < csvData.Length; i++)
                {
                    //create new rows
                    DataRow row = csvDataSet.Tables[0].NewRow();

                    for (int j = 0; j < headings.Length; j++)
                    {
                        //fill them
                        row[j] = csvData[i].Split(',')[j];
                    }

                    //add rows to over DataTable
                    csvDataSet.Tables[0].Rows.Add(row);
                }

                //return the CSV DataTable
                return csvDataSet;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion Private Methods

    }
}
