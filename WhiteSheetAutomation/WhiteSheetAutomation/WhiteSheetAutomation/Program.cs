using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data; 

namespace WhiteSheetAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Constants.strStartProcess);

            string strMsg = string.Empty;
            DataTable table = new DataTable("MyDataTable");

            Console.WriteLine(Constants.strStartInputFileParsing);
            table = DataImporter.ExtractDataFromInputFile(Utility.htCommon["InputFile"].ToString());
            Console.WriteLine(Constants.strEndInputFileParsing);

            if (table != null && table.Rows.Count > 0)
            {
                Console.WriteLine(Constants.strStartSQLQueryExecution);
                Dictionary<string, DataTable> TableDictionary = DataImporter.ProcessInputDataFile(table);
                Console.WriteLine(Constants.strEndSQLQueryExecution);

                Console.WriteLine(Constants.strStartOutputFileCreation);
                DataImporter.CreateAndLoadOutputFiles(TableDictionary);
                Console.WriteLine(Constants.strEndSQLQueryExecution);

                Console.WriteLine(Constants.strEndProcess);
            }

            else
            {
                Console.WriteLine("Invalid or empty input file, please check the error logs to get more information");
            }
            
        }
    }
}
