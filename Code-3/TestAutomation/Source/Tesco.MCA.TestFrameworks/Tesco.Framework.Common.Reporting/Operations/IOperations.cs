using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.Common.Reporting.Enums;
using Tesco.Framework.Common.Reporting.Entities;

namespace Tesco.Framework.Common.Reporting.Operations
{
    interface IOperations
    {
        bool VerifyInputs(string input, string output, string codecoverage, out string error);
        void InitOutput(string output);        
        string[] GetTestDlls(XmlDocument doc);
        string[] GetTestNames(XmlDocument doc, string testDll);
        string[] GetTestDescription(XmlDocument doc, string testDll);
        Structs.Structs.Summary GetSummary(XmlDocument doc);
        Structs.Structs.Summary GetActualSummary(XmlDocument doc);
        void GetTestPassFails(XmlDocument doc, ref Structs.Structs.TestProjectDetails testProjectDetails);
        Structs.Structs.Test[] GetTestDetails(XmlNode rootNode, Enums.Enums.ResultType result);
        int GetResultCount(XmlDocument doc, Enums.Enums.ResultType result);
        XmlNode GetOrderedTestsRoot(XmlDocument doc, string testName);
        Structs.Structs.Module[] GetCCData(string codecoverageXML);
        void CreateSummaryTable(Structs.Structs.Summary summary);
        void TestProjectSummaryTable(Structs.Structs.TestProjectDetails[] allDetails);
        void TestDetailsByDllTable(Structs.Structs.TestProjectDetails[] allDetails, Dictionary<string, string> testGroups);
        void TestDetailsTable(Structs.Structs.Test[] tests, string tableName);
        void CreateCCTable(Structs.Structs.Module[] modules);
        void CloseStreamWriter();
        FileInfo FetchLatestFile(string directoryPath, string fileExtension);
        void SendEmailReport(string outputHtmlPath, DateTime runStartTime);
        Dictionary<string, string> GetTestGroups(XmlDocument doc);
        ResultSummary GetResultSummary(XmlDocument doc);
        Times GetTimes(XmlDocument doc);
        void CreateSummary(ResultSummary summary, Times times);
    }
}
