using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Utilities;
using Tesco.Framework.Common.Reporting.HtmlGenerator;
using Tesco.Framework.Common.Reporting.Enums;

namespace Tesco.Framework.Common.Reporting.Facade
{
    public class ReportingFacade
    {
        #region PRIVATE MEMBERS

        private ILogger customLogs = null;
        private IHtmlGenerator generator = new HtmlGenerator.HtmlGenerator();

        #endregion

        public ReportingFacade()
        {
            Utilities.Utilities.InitializeLogger(ref customLogs, AppenderType.HTMLREPORT);
        }

        public Enums.Enums.ErrorCode GenerateReport(Enums.Enums.TestResultFileType fileType, string inputFilePath, string outputHtmlPath, string codeCoverageFilePath, DateTime runStartDateTime, bool sendEmail = false, string country = "NA", string browser = "NA")
        {
            Enums.Enums.ErrorCode returnFlag = Enums.Enums.ErrorCode.NOERROR;

            try
            {
                switch (fileType)
                {
                    case Enums.Enums.TestResultFileType.TRX:
                        {
                            returnFlag = generator.GenerateReportFromTRX(inputFilePath, outputHtmlPath, codeCoverageFilePath, runStartDateTime, sendEmail);
                            break;
                        }
                    case Enums.Enums.TestResultFileType.CSV:
                        {
                            //TO-DO:: Create & Call GenerateReportFromCSV Method
                            break;
                        }                    
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return returnFlag;
        }

        public string GenerateReportFromTRXFile(Enums.Enums.TestResultFileType fileType, string inputFilePath, string outputHtmlPath, string codeCoverageFilePath, DateTime runStartDateTime, bool sendEmail = false, string country = "NA", string browser = "NA")
        {
            string returnFlag = string.Empty;

            try
            {
                switch (fileType)
                {
                    case Enums.Enums.TestResultFileType.TRX:
                        {
                            returnFlag = generator.GenerateReportFromTRX(inputFilePath, outputHtmlPath, codeCoverageFilePath, runStartDateTime, sendEmail).ToString();
                            break;
                        }
                    case Enums.Enums.TestResultFileType.CSV:
                        {
                            //TO-DO:: Create & Call GenerateReportFromCSV Method
                            break;
                        }
                    case Enums.Enums.TestResultFileType.TRXFILE:
                        {
                            returnFlag = generator.GenerateReportFromTRXFile(inputFilePath, outputHtmlPath, codeCoverageFilePath, runStartDateTime, sendEmail, country, browser).ToString();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return returnFlag;
        }
    }
}
