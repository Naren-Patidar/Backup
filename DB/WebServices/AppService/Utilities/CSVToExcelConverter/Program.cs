using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;

namespace CSVToExcelConverter
{
    class Program
    {

        #region Main Program

        static void Main(string[] args)
        {
            string inputFilePath = ConfigurationSettings.AppSettings["InputFilePath"].ToString();

            CSVToDataSet oCSVToDataSet = new CSVToDataSet();

            oCSVToDataSet.ProcessDirectory(inputFilePath);
        }

        #endregion Main Program

    }
}
