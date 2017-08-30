using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.Common.Reporting.Operations;
using Tesco.Framework.Common.Reporting.Structs;
using Tesco.Framework.Common.Reporting.Enums;
using Tesco.Framework.Common.Logging.Logger;
using System.Linq;
using Tesco.Framework.Common.Reporting.Entities;

namespace Tesco.Framework.Common.Reporting.HtmlGenerator
{
    class HtmlGenerator : IHtmlGenerator
    {  
        private IOperations operations = new Operations.Operations();
        private ILogger customLogs = null;
        private string tempXml = string.Empty;
        public static string Browser = string.Empty;
        public static string Country = string.Empty;

        public HtmlGenerator()
        {
            Utilities.Utilities.InitializeLogger(ref customLogs, Utilities.AppenderType.HTMLREPORT);
        }

        /// <summary>
        /// Method to Generate Report HTML
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputHtmlPath"></param>
        /// <param name="codecoverageXML"></param>
        /// <returns></returns>
        public Enums.Enums.ErrorCode GenerateReportFromTRX(string inputTrxPath, string outputHtmlPath, string codecoverageXML, DateTime runStartDateTime, bool sendEmail)
        {
            string errorMessage = string.Empty;
            Enums.Enums.ErrorCode errCode = Enums.Enums.ErrorCode.NOERROR; 
            try
            {
                if (operations.VerifyInputs(inputTrxPath, outputHtmlPath, codecoverageXML, out errorMessage))
                {
                    //Get the Latest TRX file
                    var trxFile = operations.FetchLatestFile(inputTrxPath, "trx"); 
                    //Get the CodeCoverage XML File
                    //var codeCoverageFile = operations.FetchLatestFile(codecoverageXML, "xml");

                    if (File.Exists(trxFile.FullName) != true)
                    {
                        errCode = Enums.Enums.ErrorCode.REPORT_GEN_ERROR;
                        return errCode;
                    }

                    //Create Output File Object
                    string htmlFile = Path.GetFileNameWithoutExtension(trxFile.FullName) + ".html";
                    outputHtmlPath = Path.Combine(outputHtmlPath, htmlFile);
                    operations.InitOutput(outputHtmlPath);

                    //change the first tag to have no attributes - VSTS2008
                    tempXml = Path.GetTempPath() + "\\temp.xml";
                    File.Copy(trxFile.FullName, tempXml, true);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(tempXml);
                    XmlElement root = doc.DocumentElement;
                    root.Attributes.RemoveAll();
                    doc.Save(tempXml);

                    string file = File.ReadAllText(tempXml);
                    if (file.Contains("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2006\""))
                    {
                        file = file.Replace("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2006\"", "");
                    }
                    else if (file.Contains("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2010\""))
                    {
                        file = file.Replace("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2010\"", "");
                    }

                    File.WriteAllText(tempXml, file);

                    doc = new XmlDocument();
                    doc.Load(tempXml);

                    // Summary Table                    
                    Structs.Structs.Summary s = operations.GetActualSummary(doc);
                    operations.CreateSummaryTable(s);

                    // Code coverage table
                    //if (codeCoverageFile != null && codeCoverageFile.Length > 0)
                    //{
                    //    Structs.Structs.Module[] m = operations.GetCCData(codeCoverageFile.FullName);
                    //    if (m != null)
                    //    {
                    //        operations.CreateCCTable(m);
                    //    }
                    //}

                    // Unit test reports:
                    // Some unit tests may be run as part of an Ordered test
                    if (operations.GetResultCount(doc, Enums.Enums.ResultType.UnitTestResult) > 0)
                    {
                        // Get all test dlls
                        string[] testDlls = operations.GetTestDlls(doc);
                        if (testDlls.Length > 0)
                        {
                            // Get details for each test dlls
                            Structs.Structs.TestProjectDetails[] allDetails = new Structs.Structs.TestProjectDetails[testDlls.Length];
                            for (int i = 0; i < allDetails.Length; i++)
                            {
                                allDetails[i].testDll = testDlls[i];
                                string[] testNames = operations.GetTestNames(doc, testDlls[i]);

                                string[] testDescriptions = operations.GetTestDescription(doc, testDlls[i]);
                                allDetails[i].tests = new Structs.Structs.Test[testNames.Length];
                                allDetails[i].count = testNames.Length;
                                for (int j = 0; j < testNames.Length; j++)
                                {
                                    allDetails[i].tests[j].Name = testNames[j];
                                    allDetails[i].tests[j].Description = testDescriptions[j];
                                }
                                operations.GetTestPassFails(doc, ref allDetails[i]);
                            }

                            //Get Test Groups
                            var testGroups = operations.GetTestGroups(doc);

                            // Summary table for test dlls
                            operations.TestProjectSummaryTable(allDetails);
                            // Details for individual unit tests in each test dll
                            operations.TestDetailsByDllTable(allDetails, testGroups);
                        }
                    }

                    // WebTest reports:
                    Structs.Structs.Test[] webTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.WebTestResult); // GetWebTestDetails(doc);
                    if (webTests.Length > 0)
                    {
                        // Add Table for web tests:
                        operations.TestDetailsTable(webTests, "Web Tests:");
                        //TestDetailsWebTests(webTests);
                    }

                    // Manual test reports:
                    Structs.Structs.Test[] manualTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.ManualTestResult); //.GetManualTestDetails(doc);
                    if (manualTests.Length > 0)
                    {
                        // Add Table for manual tests:
                        //TestDetailsManualTests(manualTests);
                        operations.TestDetailsTable(manualTests, "Manual Tests:");
                    }

                    // Generic test reports:
                    Structs.Structs.Test[] genericTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.GenericTestResult);
                    if (genericTests.Length > 0)
                    {
                        operations.TestDetailsTable(genericTests, "Generic tests:");
                    }

                    // Load test reports:
                    Structs.Structs.Test[] loadTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.LoadTestResult);
                    if (loadTests.Length > 0)
                    {
                        operations.TestDetailsTable(loadTests, "Load tests:");
                    }

                    // Ordered test reports:
                    Structs.Structs.Test[] orderedTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.TestResultAggregation);
                    if (orderedTests.Length > 0)
                    {
                        operations.TestDetailsTable(orderedTests, "Ordered tests:");
                        // List results of sub-tests for each ordered test:
                        foreach (Structs.Structs.Test orderedTest in orderedTests)
                        {
                            // Get the parent orderedTest node:
                            XmlNode orderedParent = operations.GetOrderedTestsRoot(doc, orderedTest.Name);
                            if (null != orderedParent)
                            {
                                Structs.Structs.Test[] orderedSubTests = operations.GetTestDetails(orderedParent, Enums.Enums.ResultType.TestResultAggregationSubTests);
                                if (orderedSubTests.Length > 0)
                                {
                                    operations.TestDetailsTable(orderedSubTests, "Individual tests in " + orderedTest.Name + " (ordered test):");
                                }
                            }
                        }
                    }

                    operations.CloseStreamWriter();
                    
                    /*Delete the temp.xml file*/
                    if (File.Exists(tempXml))
                    {
                        File.Delete(tempXml);
                    }
                }
                else
                {
                    throw new Exception(errorMessage);
                }

                // Send Email Report
                try
                {
                    if (sendEmail)
                    {
                        operations.SendEmailReport(outputHtmlPath, runStartDateTime);
                    }
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    customLogs.LogInformation(string.Format("Error Occured! Details - \r\nMessage: {0}, \r\nInnerException: {1}, \r\nStatus Code: {2}, \r\nData: {3}", ex.Message, ex.InnerException, ex.StatusCode));
                    //customLogs.LogError(ex);
                    errCode = Enums.Enums.ErrorCode.SEND_MAIL_ERROR;
                }
                catch (Exception ex)
                {
                    customLogs.LogError(ex);
                    errCode = Enums.Enums.ErrorCode.SEND_MAIL_ERROR;
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                errCode = Enums.Enums.ErrorCode.REPORT_GEN_ERROR;
            }
            finally
            {
                
                operations.CloseStreamWriter();

                /*Delete the temp.xml file*/
                if (File.Exists(tempXml))
                {
                    File.Delete(tempXml);
                }
            }

            return errCode;
        }

        public string GenerateReportFromTRXFile(string trxFile, string outputHtmlPath, string codecoverageXML, DateTime runStartDateTime, bool sendEmail, string country, string browser)
        {
            string errorMessage = string.Empty;
            Browser = browser;
            Country = country;            
            try
            {       if (File.Exists(trxFile) != true)
                    {                        
                        errorMessage = string.Format("{0} file not found", trxFile);
                        return errorMessage;
                    }
                    if (Directory.Exists(outputHtmlPath) == false)
                    {
                        try {
                            Directory.CreateDirectory(outputHtmlPath);
                        }
                        catch {
                            errorMessage = string.Format("'{0}' Unable to create output html directory", outputHtmlPath);
                            return errorMessage;
                        }
                    }

                    //Create Output File Object
                    string htmlFile =string.Format("{0}_{1}_{2}.{3}", Country, Browser, Path.GetFileNameWithoutExtension(trxFile) , "html");
                    outputHtmlPath = Path.Combine(outputHtmlPath, htmlFile);
                    operations.InitOutput(outputHtmlPath);

                    //change the first tag to have no attributes - VSTS2008
                    tempXml = Path.GetTempPath() + "\\temp.xml";
                    File.Copy(trxFile, tempXml, true);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(tempXml);
                    XmlElement root = doc.DocumentElement;
                    root.Attributes.RemoveAll();
                    doc.Save(tempXml);

                    string file = File.ReadAllText(tempXml);
                    if (file.Contains("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2006\""))
                    {
                        file = file.Replace("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2006\"", "");
                    }
                    else if (file.Contains("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2010\""))
                    {
                        file = file.Replace("xmlns=\"http://microsoft.com/schemas/VisualStudio/TeamTest/2010\"", "");
                    }

                    File.WriteAllText(tempXml, file);

                    doc = new XmlDocument();
                    doc.Load(tempXml);

                    // Summary Table
                    ResultSummary summary = operations.GetResultSummary(doc);
                    Times times = operations.GetTimes(doc);
                    
                    operations.CreateSummary(summary, times);

                    // Unit test reports:
                    // Some unit tests may be run as part of an Ordered test
                    if (operations.GetResultCount(doc, Enums.Enums.ResultType.UnitTestResult) > 0)
                    {
                        // Get all test dlls
                        string[] testDlls = operations.GetTestDlls(doc);
                        if (testDlls.Length > 0)
                        {
                            // Get details for each test dlls
                            Structs.Structs.TestProjectDetails[] allDetails = new Structs.Structs.TestProjectDetails[testDlls.Length];
                            for (int i = 0; i < allDetails.Length; i++)
                            {
                                allDetails[i].testDll = testDlls[i];
                                string[] testNames = operations.GetTestNames(doc, testDlls[i]);

                                string[] testDescriptions = operations.GetTestDescription(doc, testDlls[i]);
                                allDetails[i].tests = new Structs.Structs.Test[testNames.Length];
                                allDetails[i].count = testNames.Length;
                                for (int j = 0; j < testNames.Length; j++)
                                {
                                    allDetails[i].tests[j].Name = testNames[j];
                                    allDetails[i].tests[j].Description = testDescriptions[j];
                                }
                                operations.GetTestPassFails(doc, ref allDetails[i]);
                            }

                            //Get Test Groups
                            var testGroups = operations.GetTestGroups(doc);

                            // Summary table for test dlls
                            operations.TestProjectSummaryTable(allDetails);
                            // Details for individual unit tests in each test dll
                            operations.TestDetailsByDllTable(allDetails, testGroups);
                        }
                    }

                    // WebTest reports:
                    Structs.Structs.Test[] webTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.WebTestResult); // GetWebTestDetails(doc);
                    if (webTests.Length > 0)
                    {
                        // Add Table for web tests:
                        operations.TestDetailsTable(webTests, "Web Tests:");
                        //TestDetailsWebTests(webTests);
                    }

                    // Manual test reports:
                    Structs.Structs.Test[] manualTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.ManualTestResult); //.GetManualTestDetails(doc);
                    if (manualTests.Length > 0)
                    {
                        // Add Table for manual tests:
                        //TestDetailsManualTests(manualTests);
                        operations.TestDetailsTable(manualTests, "Manual Tests:");
                    }

                    // Generic test reports:
                    Structs.Structs.Test[] genericTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.GenericTestResult);
                    if (genericTests.Length > 0)
                    {
                        operations.TestDetailsTable(genericTests, "Generic tests:");
                    }

                    // Load test reports:
                    Structs.Structs.Test[] loadTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.LoadTestResult);
                    if (loadTests.Length > 0)
                    {
                        operations.TestDetailsTable(loadTests, "Load tests:");
                    }

                    // Ordered test reports:
                    Structs.Structs.Test[] orderedTests = operations.GetTestDetails(doc.DocumentElement, Enums.Enums.ResultType.TestResultAggregation);
                    if (orderedTests.Length > 0)
                    {
                        operations.TestDetailsTable(orderedTests, "Ordered tests:");
                        // List results of sub-tests for each ordered test:
                        foreach (Structs.Structs.Test orderedTest in orderedTests)
                        {
                            // Get the parent orderedTest node:
                            XmlNode orderedParent = operations.GetOrderedTestsRoot(doc, orderedTest.Name);
                            if (null != orderedParent)
                            {
                                Structs.Structs.Test[] orderedSubTests = operations.GetTestDetails(orderedParent, Enums.Enums.ResultType.TestResultAggregationSubTests);
                                if (orderedSubTests.Length > 0)
                                {
                                    operations.TestDetailsTable(orderedSubTests, "Individual tests in " + orderedTest.Name + " (ordered test):");
                                }
                            }
                        }
                    }

                    operations.CloseStreamWriter();

                    /*Delete the temp.xml file*/
                    if (File.Exists(tempXml))
                    {
                        File.Delete(tempXml);
                    }
                

                // Send Email Report
                try
                {
                    if (sendEmail)
                    {
                        operations.SendEmailReport(outputHtmlPath, runStartDateTime);
                    }
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    customLogs.LogInformation(string.Format("Error Occured! Details - \r\nMessage: {0}, \r\nInnerException: {1}, \r\nStatus Code: {2}, \r\nData: {3}", ex.Message, ex.InnerException, ex.StatusCode));
                    //customLogs.LogError(ex);                    
                    errorMessage = string.Format("{0} | {1}", ex.Message, ex.StackTrace);
                }
                catch (Exception ex)
                {
                    customLogs.LogError(ex);                    
                    errorMessage = string.Format("{0} | {1}", ex.Message, ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);                
                errorMessage = string.Format("{0} | {1}", ex.Message, ex.StackTrace);
            }
            finally
            {

                operations.CloseStreamWriter();

                /*Delete the temp.xml file*/
                if (File.Exists(tempXml))
                {
                    File.Delete(tempXml);
                }
            }

            return errorMessage;
        }  
    }
}

