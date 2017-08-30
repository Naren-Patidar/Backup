using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Reporting.Enums
{
    public static class Enums
    {
        public enum ResultType { ManualTestResult, WebTestResult, GenericTestResult, UnitTestResult, LoadTestResult, TestResultAggregation, TestResultAggregationSubTests };
        public enum TestResultFileType
        {
            TRX = 1, 
            CSV = 2,
            TRXFILE
        };

        public enum ErrorCode
        {
            NOERROR = 0,
            REPORT_GEN_ERROR = 1,
            SEND_MAIL_ERROR = 2
        };
    }
}
