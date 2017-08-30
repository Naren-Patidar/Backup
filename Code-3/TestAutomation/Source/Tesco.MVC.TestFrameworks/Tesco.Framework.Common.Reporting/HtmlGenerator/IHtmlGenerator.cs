using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Reporting.HtmlGenerator
{
    interface IHtmlGenerator
    {
        Enums.Enums.ErrorCode GenerateReportFromTRX(string inputTrxPath, string outputHtmlPath, string codecoverageXML, DateTime runStartDateTime, bool sendEmail);
        string GenerateReportFromTRXFile(string trxFile, string outputHtmlPath, string codecoverageXML, DateTime runStartDateTime, bool sendEmail, string country, string browser);
    }
}
