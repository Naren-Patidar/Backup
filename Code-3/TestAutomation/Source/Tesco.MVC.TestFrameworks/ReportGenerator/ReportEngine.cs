using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.Common.Reporting.Structs;
using Tesco.Framework.Common.Reporting.Enums;
using Tesco.Framework.Common.Reporting.Facade;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Utilities;
using System.Configuration;
using System.Globalization;

namespace Tesco.Framework.ReportGenerator
{
    class ReportEngine
    {
        private static ILogger customLogs = null;
        private static bool _isSendMail = false;
        private static DateTime _runStartTime;

        public ReportEngine()
        {
            //Utilities.InitializeLogger(ref customLogs, AppenderType.HTMLREPORT);
        }

        static void Main(string[] args)
        {
            try
            {
                ReportingFacade report = new ReportingFacade();
                Utilities.InitializeLogger(ref customLogs, AppenderType.HTMLREPORT);
                Enums.ErrorCode returnCode = Enums.ErrorCode.NOERROR;
                string trxFileLocation = string.Empty;
                string OutputFileLocation = string.Empty;
                string Country = string.Empty;
                string Browser = string.Empty;
                if (args.Length == 0)
                {
                    trxFileLocation = ConfigurationManager.AppSettings["trxFileLocation"];
                    OutputFileLocation = ConfigurationManager.AppSettings["OutputFileLocation"];
                    returnCode = report.GenerateReport(Enums.TestResultFileType.TRX, trxFileLocation, OutputFileLocation, null, _runStartTime, false);
                }
                else
                {
                    trxFileLocation = args.Length > 0 ? args[0] : string.Empty;
                    OutputFileLocation = args.Length > 1 ? args[1] : string.Empty;
                    Country = args.Length > 2 ? args[2] : string.Empty;
                    Browser = args.Length > 3 ? args[3] : string.Empty;
                    string error = report.GenerateReportFromTRXFile(Enums.TestResultFileType.TRXFILE, trxFileLocation, OutputFileLocation, null, _runStartTime, false, Country, Browser);
                    Console.WriteLine(error);
                }
                switch (returnCode)
                {
                    case Enums.ErrorCode.NOERROR:
                        Console.WriteLine("HTML Test Report Generated Successfully!");
                        if (_isSendMail)
                            Console.WriteLine("Report is also e-mailed.");

                        customLogs.LogInformation("HTML Test Report Generated Successfully!");

                        break;
                    case Enums.ErrorCode.REPORT_GEN_ERROR:
                        Console.WriteLine("Sorry, something went wrong! HTML Test Report could not be generated.");
                        customLogs.LogInformation("Sorry, something went wrong! HTML Test Report could not be generated. Most probable cause will be incorrect arguments.");
                        break;
                    case Enums.ErrorCode.SEND_MAIL_ERROR:
                        Console.WriteLine("HTML Test Report generated successfully.");
                        Console.WriteLine("Error Occured while e-mailing test report.");
                        customLogs.LogInformation("HTML Test Report generated successfully. Error Occured while e-mailing test report.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured, Report Engine Terminated!");
                Console.ReadLine();
                customLogs.LogError(ex);
            }
            return;
        }

        private static Enums.ErrorCode CheckArgumentsValidity(string[] args)
        {
            string msg = String.Empty;
            Enums.ErrorCode code = 0;

            /*Minimum two parameters are expected - UI/API and Run Start DateTime*/
            if (args.GetLength(0) < 2)
            {
                msg = "Minimum two parameters are expected - UI/API and Run Start DateTime";
                code = Enums.ErrorCode.REPORT_GEN_ERROR;
            }
            /*Maximum three parameters are allowed - UI/API, Run Start DateTime & Send Email*/
            else if (args.GetLength(0) > 3)
            {
                msg = "Maximum three parameters are allowed - UI/API, Run Start DateTime & Send Email";
                code = Enums.ErrorCode.REPORT_GEN_ERROR;
            }
            else
            {
                switch (1)
                {
                    case 1:
                        //Check if the first patam is either UI or API
                        if (args[0] != "UI" && args[0] != "API")
                        {
                            msg = "Only UI or API is a valid parameter 1";
                            code = Enums.ErrorCode.REPORT_GEN_ERROR;
                            break;
                        }
                        //Check if the second param is a valid datetime value
                        try
                        {
                            _runStartTime = DateTime.ParseExact(args[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            msg = "Only DateTime in (dd/MM/yyyy HH:mm:ss) format is a valid parameter 2";
                            code = Enums.ErrorCode.REPORT_GEN_ERROR;
                            break;
                        }
                        //Check if the third optional param is provided, it is either true or false
                        if (args.GetLength(0) == 3)
                        {
                            if (!(args[2].Equals("true", StringComparison.OrdinalIgnoreCase) || args[2].Equals("false", StringComparison.OrdinalIgnoreCase)))
                            {
                                msg = "Only true or false is a valid parameter 3";
                                code = Enums.ErrorCode.REPORT_GEN_ERROR;
                                break;
                            }
                            else
                                _isSendMail = Convert.ToBoolean(args[2].ToLower());
                        }
                        else
                        {
                            _isSendMail = false;
                        }

                        break;
                }
            }

            Console.WriteLine(msg);

            return code;
        }
    }
}
