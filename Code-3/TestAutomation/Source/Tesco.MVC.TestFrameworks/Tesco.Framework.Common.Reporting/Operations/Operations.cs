using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Tesco.Framework.Common.Reporting.Structs;
using Tesco.Framework.Common.Logging.Logger;
using Tesco.Framework.Common.Reporting.Enums;
using System.Linq;
using Tesco.Framework.Common.Utilities.Entity;
using System.Net.Mail;
using System.IO.Compression;
using Tesco.Framework.Common.Utilities.ZipLibrary;
using System.Globalization;
using System.Text.RegularExpressions;
using Tesco.Framework.Common.Reporting.Entities;
using System.Xml.Serialization;
using System.Configuration;

namespace Tesco.Framework.Common.Reporting.Operations
{
    class Operations : IOperations
    {
        private StreamWriter writer = null;
        private ILogger customLogs = null;
        private int _failed;
        private string outputHtmlFile = string.Empty;
        private string tempHTML = string.Empty;        

        public int Failed
        {
            get
            {
                return _failed;
            }
            set
            {
                _failed = value;
            }
        }

        public Operations()
        {
            Utilities.Utilities.InitializeLogger(ref customLogs, Utilities.AppenderType.HTMLREPORT);
        }

        /// <summary>
        /// Method to Verify Inputs
        /// </summary>       
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="codecoverage"></param>       
        /// <returns></returns>
        public bool VerifyInputs(string input, string output, string codecoverage, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (input != string.Empty && output != string.Empty && codecoverage != string.Empty)
                {
                    if (Directory.Exists(input) == false)
                    {
                        errorMessage = "Could not find Input file path: " + input;
                        return false;
                    }
                    if (Directory.Exists(output) == false)
                    {
                        errorMessage = "Could not find Output file path: " + output;
                        return false;
                    }
                    //if (Directory.Exists(codecoverage) == false)
                    //{
                    //    errorMessage = "Could not find Code-Coverage file path: " + codecoverage;
                    //    return false;
                    //}
                }
                else
                {
                    errorMessage = "Test File Paths cannot be empty";
                    return false;
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initialize Output
        /// </summary>
        /// <param name="outputFile"></param>
        public void InitOutput(string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            /*Copy Output HTML File's Full Path*/
            outputHtmlFile = outputFile;            

            writer = new StreamWriter(outputFile);
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("<style type=\"text/css\">");
            writer.WriteLine("body { font:normal 80% verdana,arial,helvetica; color:#000000; }");
            writer.WriteLine("span.info { font-size:smaller; font-weight:normal; color:#545454; font-style:normal; font-family:Arial;}");
            writer.WriteLine("span.covered { background: #2D9320; display: inline-block;height: 9px;}");
            writer.WriteLine("span.uncovered { background: #CA261D; display: inline-block; height: 9px;}");
            writer.WriteLine("span.ignored { background: #FFFF50; display: inline-block; height: 9px;}");
            writer.WriteLine("td.covered { background-color: #2D9320; height: 9px;}");
            writer.WriteLine("td.uncovered { background-color: #CA261D; height: 9px;}");
            writer.WriteLine("td.ignored { background-color: #FFFF50; height: 9px;}");
            writer.WriteLine("td { FONT-SIZE: 80%; BORDER-BOTTOM: #dcdcdc 1px solid; BORDER-RIGHT: #dcdcdc 1px solid;}");
            writer.WriteLine("td.section { FONT-SIZE: 80% !important; BORDER-BOTTOM: 0px !important; BORDER-RIGHT: 0px !important;}");
            writer.WriteLine("p {	line-height:1.5em; margin-top:0.5em; margin-bottom:1.0em;}");
            writer.WriteLine("h1 { MARGIN: 0px 0px 5px; FONT: 165% verdana,arial,helvetica;}");
            writer.WriteLine("h2 { MARGIN-TOP: 1em; MARGIN-BOTTOM: 0.5em; FONT: bold 125% verdana,arial,helvetica;}");
            writer.WriteLine("h3 { MARGIN-BOTTOM: 0.5em; FONT: bold 115% verdana,arial,helvetica; }");
            writer.WriteLine("h4 { MARGIN-BOTTOM: 0.5em; FONT: bold 100% verdana,arial,helvetica; color:#441106; }");
            writer.WriteLine("h5 { MARGIN-BOTTOM: 0.5em; FONT: bold 100% verdana,arial,helvetica;}");
            writer.WriteLine("h6 { MARGIN-BOTTOM: 0.5em; FONT: bold 100% verdana,arial,helvetica;}");            
            writer.WriteLine(".Error { font-weight:bold;}");
            writer.WriteLine(".Failure { font-weight:bold; color:#CA261D;}");
            writer.WriteLine(".Success { font-weight:bold; color:#2D9320;}");
            writer.WriteLine(".Blue { font-weight:bold; color:#1F38B6;}");
            writer.WriteLine(".FailureTest { color:#373E36;}");
            writer.WriteLine(".SuccessTest { color:#373E36;}");
            writer.WriteLine(".Ignored { font-weight:bold;}");
            writer.WriteLine(".FailureDetail { font-size: -1; padding-left: 2.0em; background:#cdcdcd;}");
            writer.WriteLine(".Pass { padding-left:2px;}");
            writer.WriteLine(".TableHeader { background: #efefef; color: #000; font-weight: bold; horizontal-align: center; }");
            writer.WriteLine("a:visited { color: #0000ff;} a { color: #0000ff;}");
            writer.WriteLine("a:active { color: #800000;} a.summarie { color:#000; text-decoration: none;}");
            writer.WriteLine("a.summarie:active { color:#000; text-decoration: none;}");
            writer.WriteLine("a.summarie:visited { color:#000; text-decoration: none;}");
            writer.WriteLine(".description { margin-top:1px; padding:3px; background-color:#dcdcdc; color:#000; font-weight:normal;}");
            writer.WriteLine(".method{ color:#000; font-weight:normal; padding-left:5px;}");
            writer.WriteLine("a.method{ text-decoration: none; color:#000; font-weight:normal; padding-left:5px;}");
            writer.WriteLine("a.Failure { font-weight:bold; color:red; text-decoration: none;}");
            writer.WriteLine("a.Failure:visited { font-weight:bold; color:red; text-decoration: none;}");
            writer.WriteLine("a.Failure:active { font-weight:bold; color:red; text-decoration: none;}");
            writer.WriteLine("a.error { font-weight:bold; color:red;}");
            writer.WriteLine("a.error:visited { font-weight:bold; color:red;}");
            writer.WriteLine("a.error:active { font-weight:bold; color:red;}");
            writer.WriteLine("a.ignored { font-weight:bold; text-decoration: none; padding-left:5px;}");
            writer.WriteLine("a.ignored:visited { font-weight:bold; text-decoration: none; padding-left:5px;}");
            writer.WriteLine("a.ignored:active { font-weight:bold; text-decoration: none; padding-left:5px;}");
            writer.WriteLine("</style>");

            AddToggleScript();
        }

        /// <summary>
        /// Method to Add Toggle Script in HTML Report
        /// </summary>
        private void AddToggleScript()
        {
            writer.WriteLine("<script language=\"JavaScript\">");
            writer.WriteLine("function Toggle(id) {");
            writer.WriteLine("var element = document.getElementById(id);");
            writer.WriteLine("if ( element.style.display == \"none\" )");
            /*Changed the below line to fix Chrome issue with HTML*/
            writer.WriteLine("element.style.display = '';");
            writer.WriteLine("else ");
            writer.WriteLine("element.style.display = \"none\";");
            writer.WriteLine("}");
            writer.WriteLine("</script>");
        }

        public string[] GetTestDlls(XmlDocument doc)
        {
            string AllTestDlls = "";
            XmlNodeList nodes = doc.SelectNodes("//TestDefinitions/UnitTest");

            if (nodes.Count <= 0)
            {
                return new string[] { };
            }

            foreach (XmlNode node in nodes)
            {
                string text = node.SelectSingleNode("TestMethod/@codeBase").InnerText;

                int index = text.LastIndexOf("/");
                if (index < text.LastIndexOf("\\"))
                {
                    index = text.LastIndexOf("\\");
                }
                if (index < 0)
                {
                    index = 0;
                }

                string codeBase = text.Substring(index + 1, text.Length - index - 1);

                if (AllTestDlls.IndexOf(codeBase) >= 0)
                {
                    continue;
                }

                AllTestDlls += codeBase + ";";
            }

            AllTestDlls = AllTestDlls.Substring(0, Math.Max(AllTestDlls.LastIndexOf(";"), 0));
            string[] testDlls = AllTestDlls.Split(new char[] { ';' });
            return testDlls;
        }

        public string[] GetTestNames(XmlDocument doc, string testDll)
        {
            try
            {
                string AllTestNames = "";
                XmlNodeList nodes = doc.SelectNodes("//TestDefinitions/UnitTest");

                foreach (XmlNode node in nodes)
                {
                    string sCodeBase = node.SelectSingleNode("TestMethod/@codeBase").InnerText;

                    if (sCodeBase != null)
                    {
                        int index = sCodeBase.LastIndexOf("/");
                        if (index < sCodeBase.LastIndexOf("\\"))
                        {
                            index = sCodeBase.LastIndexOf("\\");
                        }
                        if (index < 0)
                        {
                            index = 0;
                        }

                        string codeBase = sCodeBase.Substring(index + 1, sCodeBase.Length - index - 1);

                        if (codeBase == testDll)
                        {
                            string testName = node.SelectSingleNode("TestMethod/@name").InnerText;
                            AllTestNames += testName + ";";

                        }
                    }
                }

                AllTestNames = AllTestNames.Substring(0, AllTestNames.LastIndexOf(";"));
                string[] testNames = AllTestNames.Split(new char[] { ';' });
                return testNames;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return null;
            }
        }

        public string[] GetTestDescription(XmlDocument doc, string testDll)
        {
            string[] AllTestDescriptions = null;

            try
            {
                XmlNodeList nodes = doc.SelectNodes("//TestDefinitions/UnitTest");
                AllTestDescriptions = new string[nodes.Count];
                int i = 0;

                foreach (XmlNode node in nodes)
                {
                    string sCodeBase = node.SelectSingleNode("TestMethod/@codeBase").InnerText;

                    if (sCodeBase != null)
                    {
                        int index = sCodeBase.LastIndexOf("/");
                        if (index < sCodeBase.LastIndexOf("\\"))
                        {
                            index = sCodeBase.LastIndexOf("\\");
                        }
                        if (index < 0)
                        {
                            index = 0;
                        }

                        string codeBase = sCodeBase.Substring(index + 1, sCodeBase.Length - index - 1);
                        if (codeBase == testDll)
                        {
                            if (node.InnerXml.Contains("<Description>"))
                            {
                                AllTestDescriptions[i] = node.SelectSingleNode("Description").InnerText;

                            }
                            else
                            {
                                AllTestDescriptions[i] = "NA";
                            }
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return AllTestDescriptions;
        }

        public Structs.Structs.Summary GetSummary(XmlDocument doc)
        {
            Structs.Structs.Summary summary = new Structs.Structs.Summary();

            try
            {
                summary.total = -1;
                summary.executed = -1;
                summary.passed = -1;

                XmlNode nTotal = doc.SelectNodes("//Counters/@total").Item(0);
                summary.total = Convert.ToInt32(nTotal.InnerText);
                XmlNode nPass = doc.SelectNodes("//Counters/@passed").Item(0);
                summary.passed = Convert.ToInt32(nPass.InnerText);
                XmlNode nExecuted = doc.SelectNodes("//Counters/@executed").Item(0);
                summary.executed = Convert.ToInt32(nExecuted.InnerText);

                ////condition to check if the file has been modified to have elapsedtime attribute
                ////elapsed time attribute will be set when 2 trx files are combined
                if (doc.SelectNodes("//Times/@elapsedtime").Count != 0)
                {
                    XmlNode nElapsed = doc.SelectNodes("//Times/@elapsedtime").Item(0);
                    summary.time = Convert.ToDouble(nElapsed.InnerText);
                }
                else
                {
                    DateTime start;
                    DateTime end0;
                    XmlNode nStart = doc.SelectNodes("//Times/@start").Item(0);
                    start = Convert.ToDateTime(nStart.InnerText);
                    XmlNode nEnd = doc.SelectNodes("//Times/@finish").Item(0);
                    end0 = Convert.ToDateTime(nEnd.InnerText);
                    summary.time = (end0 - start).TotalSeconds;
                }

                //get execution date
                XmlNode dtDate = doc.SelectNodes("//Times/@creation").Item(0);
                summary.execDate = Convert.ToDateTime(dtDate.InnerText);
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return summary;
        }

        public Structs.Structs.Summary GetActualSummary(XmlDocument doc)
        {
            Structs.Structs.Summary summary = new Structs.Structs.Summary();

            try
            {
                summary.total = 0;
                summary.executed = 0;
                summary.passed = 0;

                XmlNodeList nodes = doc.SelectNodes("//Results/UnitTestResult");
                summary.total = summary.executed = nodes.Count;

                foreach (XmlNode node in nodes)
                {
                    if (node.SelectSingleNode("@outcome").InnerText.Equals("Passed"))
                    {
                        summary.passed = summary.passed + 1;
                    }
                }

                ////condition to check if the file has been modified to have elapsedtime attribute
                ////elapsed time attribute will be set when 2 trx files are combined

                /*Added extra condition for 'false' to ensure that @elapsedtime is not considered*/
                bool isTrue = false;
                if (isTrue && doc.SelectNodes("//Times/@elapsedtime").Count != 0)
                {
                    XmlNode nElapsed = doc.SelectNodes("//Times/@elapsedtime").Item(0);
                    summary.time = Convert.ToDouble(nElapsed.InnerText);
                }
                else
                {
                    /*Uncomment the below region, if you need to use @start & @finish attributes*/
                    #region Old For Sequential Execution

                    //DateTime start;
                    //DateTime end0;
                    //XmlNode nStart = doc.SelectNodes("//Times/@start").Item(0);
                    //start = Convert.ToDateTime(nStart.InnerText);
                    //XmlNode nEnd = doc.SelectNodes("//Times/@finish").Item(0);
                    //end0 = Convert.ToDateTime(nEnd.InnerText);
                    //summary.time = (end0 - start).TotalSeconds;

                    #endregion

                    /*region for calculating time for parallel execution*/
                    #region For Parallel Execution

                    XmlNodeList peNodes = doc.SelectNodes("//Results/UnitTestResult");

                    List<DateTime> startTime = new List<DateTime>();
                    List<DateTime> endTime = new List<DateTime>();

                    foreach (XmlNode node in peNodes)
                    {
                        //Check for StartTime
                        if (!(node.SelectSingleNode("@startTime") == null))
                        {
                            startTime.Add(DateTime.Parse(node.SelectSingleNode("@startTime").InnerText, CultureInfo.InvariantCulture));
                        }

                        //Check for EndTime
                        if (!(node.SelectSingleNode("@endTime") == null))
                        {
                            endTime.Add(DateTime.Parse(node.SelectSingleNode("@endTime").InnerText, CultureInfo.InvariantCulture));
                        }  
                    }

                    if (startTime.Count == endTime.Count)
                    {
                        summary.time = (endTime.Max() - startTime.Min()).TotalSeconds;
                    }
                    else
                    {
                        throw new Exception("@StartTime and @EndTime attribute counts for 'UnitTestResult' are not same, check the TRX File");
                    }

                    #endregion
                }

                //get execution date
                XmlNode dtDate = doc.SelectNodes("//Times/@creation").Item(0);
                summary.execDate = Convert.ToDateTime(dtDate.InnerText);
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return summary;
        }

        public void GetTestPassFails(XmlDocument doc, ref Structs.Structs.TestProjectDetails testProjectDetails)
        {
            try
            {
                XmlNodeList nodes = doc.SelectNodes("//Results/UnitTestResult");                
                    
                foreach (XmlNode node in nodes)
                {                    
                    for (int i = 0; i < testProjectDetails.tests.Length; i++)
                    {
                        // Same test may be run twice
                        if ((testProjectDetails.tests[i].Name == node.SelectSingleNode("@testName").InnerText) && (testProjectDetails.tests[i].Status == null))
                        {
                            // Get & Set Execution ID for each Test
                            if (!(string.IsNullOrEmpty(node.SelectSingleNode("@executionId").InnerText)))
                            {
                                testProjectDetails.tests[i].ExecutionID = node.SelectSingleNode("@executionId").InnerText;
                            }

                            // Get outcome, error message and update failed count if required
                            if (node.SelectSingleNode("@outcome").InnerText == "Passed")
                            {
                                testProjectDetails.tests[i].Status = "PASS";

                                if (node.SelectSingleNode("@resultType") != null && node.SelectSingleNode("@resultType").InnerText != "")
                                {
                                    if (node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message") != null && node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText != "")
                                    {
                                        testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText;
                                    }
                                }
                                else
                                {
                                    if (node.SelectSingleNode("Output/ErrorInfo/Message") != null && node.SelectSingleNode("Output/ErrorInfo/Message").InnerText != "")
                                    {
                                        testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("Output/ErrorInfo/Message").InnerText;
                                    }
                                }
                            }
                            else if (node.SelectSingleNode("@outcome").InnerText == "Inconclusive")
                            {
                                testProjectDetails.tests[i].Status = "Inconclusive";

                                if (node.SelectSingleNode("@resultType") != null && node.SelectSingleNode("@resultType").InnerText != "")
                                {
                                    if (node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message") != null && node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText != "")
                                    {
                                        testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText;
                                    }
                                }
                                else
                                {
                                    if (node.SelectSingleNode("Output/ErrorInfo/Message") != null && node.SelectSingleNode("Output/ErrorInfo/Message").InnerText != "")
                                    {
                                        testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("Output/ErrorInfo/Message").InnerText;
                                    }
                                }
                                testProjectDetails.inconclusive++;
                            }
                            else if (node.SelectSingleNode("@outcome").InnerText == "PassedButRunAborted")
                            {
                                testProjectDetails.tests[i].Status = "PASSEDBUTABORTED";

                            }
                            else if (node.SelectSingleNode("@outcome").InnerText == "NotExecuted")
                            {
                                testProjectDetails.tests[i].Status = "NOTRUN";
                            }
                            else
                            {
                                if (node.SelectSingleNode("@outcome").InnerText == "Aborted")
                                {
                                    testProjectDetails.tests[i].Status = "ABORTED";
                                }
                                else
                                {
                                    testProjectDetails.tests[i].Status = "FAIL";

                                    if (node.SelectSingleNode("@resultType") != null && node.SelectSingleNode("@resultType").InnerText != "")
                                    {
                                        if (node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message") != null && node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText != "")
                                        {
                                            testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("InnerResults/UnitTestResult/Output/ErrorInfo/Message").InnerText;
                                        }
                                        /*Fix: Sometimes a test with resultType may not have InnerResults section*/
                                        else if (node.SelectSingleNode("Output/ErrorInfo/Message") != null && node.SelectSingleNode("Output/ErrorInfo/Message").InnerText != "")
                                        {
                                            testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("Output/ErrorInfo/Message").InnerText;
                                        }
                                    }
                                    else
                                    {
                                        if (node.SelectSingleNode("Output/ErrorInfo/Message") != null && node.SelectSingleNode("Output/ErrorInfo/Message").InnerText != "")
                                        {
                                            testProjectDetails.tests[i].ErrorMessage = node.SelectSingleNode("Output/ErrorInfo/Message").InnerText;
                                        }
                                    }
                                }

                                testProjectDetails.fail++;
                            }

                            /*Fix: When duration attribute is absent for test(s) that are not executed)*/
                            if (!(node.SelectSingleNode("@duration") == null))
                            {
                                string[] time = node.SelectSingleNode("@duration").InnerText.Split(new char[] { ':' });
                                testProjectDetails.tests[i].Time = Math.Round((Convert.ToInt32(time[0]) * 60 * 60) + (Convert.ToInt32(time[1]) * 60) + Convert.ToDouble(time[2]), 3);
                            }
                            else
                            {
                                testProjectDetails.tests[i].Time = 0.0;
                            }

                            #region Handle Execution Time For Parallel Execution                            

                            //Check for StartTime
                            if (!(node.SelectSingleNode("@startTime") == null))
                            {
                                testProjectDetails.tests[i].StartTime = DateTime.Parse(node.SelectSingleNode("@startTime").InnerText, CultureInfo.InvariantCulture);
                            }                            

                            //Check for EndTime
                            if (!(node.SelectSingleNode("@endTime") == null))
                            {
                                testProjectDetails.tests[i].EndTime = DateTime.Parse(node.SelectSingleNode("@endTime").InnerText, CultureInfo.InvariantCulture);
                            }                            

                            #endregion

                            break;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        public Dictionary<string, string> GetTestGroups(XmlDocument doc)
        {
            try
            {
                string testGroup = string.Empty;
                Dictionary<string, string> testGroups = new Dictionary<string, string>();

                XmlNodeList unitTests = doc.SelectNodes("//TestDefinitions/UnitTest");

                foreach (XmlNode test in unitTests)
                {
                    string executionID = test.SelectSingleNode("Execution/@id").InnerText;
                    string testName = test.SelectSingleNode("@name").InnerText;
                    string[] seperators = { "_" };

                    if (!(string.IsNullOrEmpty(testName)))
                    {
                        string[] nameKeywords = testName.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

                        if (nameKeywords.Length > 0)
                        {
                            testGroup = nameKeywords[0];
                        }
                    }

                    if (string.IsNullOrEmpty(testGroup) || string.IsNullOrEmpty(executionID))
                    {
                        return null;
                    }

                    testGroups.Add(executionID, testGroup);
                }

                return testGroups;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return null;
            }
        }

        public Structs.Structs.Test[] GetTestDetails(XmlNode rootNode, Enums.Enums.ResultType result)
        {
            Structs.Structs.Test[] tests = null;

            try
            {
                string selectResults;
                if (result == Enums.Enums.ResultType.TestResultAggregationSubTests)
                {
                    selectResults = "innerResults/element";
                }
                else
                {
                    selectResults = result.ToString();
                }

                XmlNodeList nodes = rootNode.SelectNodes("//" + selectResults);
                if (nodes.Count <= 0)
                {
                    return new Structs.Structs.Test[] { };
                }

                tests = new Structs.Structs.Test[nodes.Count];
                int index = 0;

                foreach (XmlNode node in nodes)
                {
                    // Same test may be run twice
                    // special handling for manual test cases
                    if (selectResults == "ManualTestResult")
                    {
                        XmlNodeList mT = rootNode.SelectNodes("//ManualTest");

                        foreach (XmlNode test in mT)
                        {

                            if (test.SelectSingleNode("@name").InnerText == node.SelectSingleNode("@testName").InnerText)
                            {
                                if (test.SelectSingleNode("Comments") != null)
                                {
                                    tests[index].Description = test.SelectSingleNode("Description").InnerText;
                                    break;
                                }
                                else if (test.SelectSingleNode("Description") != null)
                                {
                                    tests[index].Description = test.SelectSingleNode("Description").InnerText;
                                    break;
                                }
                                else
                                {
                                    tests[index].Description = "Description not set";
                                    break;
                                }
                            }
                        }
                    }

                    tests[index].Name = node.SelectSingleNode("@testName").InnerText;
                    XmlNodeList lst = rootNode.SelectNodes("//TestList");
                    foreach (XmlNode bnode in lst)
                    {
                        if (node.SelectSingleNode("@testListId").InnerText == bnode.SelectSingleNode("@id").InnerText)
                        {
                            tests[index].TestListID = bnode.SelectSingleNode("@name").InnerText;
                        }
                    }

                    // Get outcome, error message and update failed count if required
                    if (node.SelectSingleNode("@outcome").InnerText == "Passed")
                    {
                        tests[index].Status = "PASS";

                    }
                    else if (node.SelectSingleNode("@outcome").InnerText == "PassedButRunAborted")
                    {
                        tests[index].Status = "PASSEDBUTABORTED";

                    }
                    else if (node.SelectSingleNode("@outcome").InnerText == "NotExecuted")
                    {
                        tests[index].Status = "NOTRUN";
                    }
                    else
                    {
                        if (node.SelectSingleNode("@outcome").InnerText == "Aborted")
                        {
                            tests[index].Status = "ABORTED";

                        }
                        else
                        {
                            tests[index].Status = "FAIL";
                        }
                        if (null != node.SelectSingleNode("Output/ErrorInfo/Message") && node.SelectSingleNode("Output/ErrorInfo/Message").InnerText.Length > 0)
                        {
                            tests[index].ErrorMessage = node.SelectSingleNode("Output/ErrorInfo/Message").InnerText;
                        }

                    }
                    if (null != node.SelectSingleNode("Comments") && node.SelectSingleNode("Comments").InnerText.Length > 0)
                    {
                        tests[index].ErrorMessage = node.SelectSingleNode("Comments").InnerText;
                    }

                    string[] time = node.SelectSingleNode("@duration").InnerText.Split(new char[] { ':' });
                    tests[index].Time = (Convert.ToInt32(time[0]) * 60 * 60) + (Convert.ToInt32(time[1]) * 60) + Convert.ToDouble(time[2]);                    

                    index++;
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return tests;
        }

        public int GetResultCount(XmlDocument doc, Enums.Enums.ResultType result)
        {
            try
            {
                return doc.SelectNodes("//Results/" + result.ToString()).Count;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return 0;
            }
        }

        public XmlNode GetOrderedTestsRoot(XmlDocument doc, string testName)
        {
            try
            {
                foreach (XmlNode n in doc.SelectNodes("//TestDefinitions/UnitTest"))
                {
                    if (n.SelectSingleNode("@name").InnerText.ToLower() == testName.ToLower())
                    {
                        return n;
                    }
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }

            return null;
        }

        public Structs.Structs.Module[] GetCCData(string codecoverageXML)
        {
            try
            {
                XmlDocument oCC = new XmlDocument();
                oCC.Load(codecoverageXML);

                XmlNodeList modules = oCC.SelectNodes("//Module");
                Structs.Structs.Module[] oModule = new Structs.Structs.Module[modules.Count];
                int index = 0;

                foreach (XmlNode module in modules)
                {
                    oModule[index].moduleName = module.SelectSingleNode("ModuleName").InnerText;
                    oModule[index].linesCovered = module.SelectSingleNode("LinesCovered").InnerText;
                    oModule[index].linesNotCovered = module.SelectSingleNode("LinesNotCovered").InnerText;
                    oModule[index].linesPartiallyCovered = module.SelectSingleNode("LinesPartiallyCovered").InnerText;
                    oModule[index].blocksCovered = module.SelectSingleNode("BlocksCovered").InnerText;
                    oModule[index].blocksNotCovered = module.SelectSingleNode("BlocksNotCovered").InnerText;

                    XmlNodeList spaces = module.SelectNodes("NamespaceTable");
                    oModule[index].namespaces = new Structs.Structs.NamespaceTable[spaces.Count];
                    int s = 0;

                    foreach (XmlNode n in spaces)
                    {
                        oModule[index].namespaces[s].blocksCovered = n.SelectSingleNode("BlocksCovered").InnerText;
                        oModule[index].namespaces[s].blocksNotCovered = n.SelectSingleNode("BlocksNotCovered").InnerText;
                        oModule[index].namespaces[s].linesCovered = n.SelectSingleNode("LinesCovered").InnerText;
                        oModule[index].namespaces[s].linesNotCovered = n.SelectSingleNode("LinesNotCovered").InnerText;
                        oModule[index].namespaces[s].linesPartiallyCovered = n.SelectSingleNode("LinesPartiallyCovered").InnerText;
                        oModule[index].namespaces[s].moduleName = n.SelectSingleNode("ModuleName").InnerText;
                        oModule[index].namespaces[s].key = n.SelectSingleNode("NamespaceKeyName").InnerText;
                        oModule[index].namespaces[s].name = n.SelectSingleNode("NamespaceName").InnerText;

                        XmlNodeList classes = n.SelectNodes("Class");
                        oModule[index].namespaces[s].classes = new Structs.Structs.ClassNode[classes.Count];
                        int p = 0;

                        foreach (XmlNode cls in classes)
                        {
                            oModule[index].namespaces[s].classes[p].blocksCovered = cls.SelectSingleNode("BlocksCovered").InnerText;
                            oModule[index].namespaces[s].classes[p].blocksNotCovered = cls.SelectSingleNode("BlocksNotCovered").InnerText;
                            oModule[index].namespaces[s].classes[p].linesCovered = cls.SelectSingleNode("LinesCovered").InnerText;
                            oModule[index].namespaces[s].classes[p].linesNotCovered = cls.SelectSingleNode("LinesNotCovered").InnerText;
                            oModule[index].namespaces[s].classes[p].linesPartiallyCovered = cls.SelectSingleNode("LinesPartiallyCovered").InnerText;
                            oModule[index].namespaces[s].classes[p].className = cls.SelectSingleNode("ClassName").InnerText;
                            oModule[index].namespaces[s].classes[p].namespaceKeyName = cls.SelectSingleNode("NamespaceKeyName").InnerText;
                            oModule[index].namespaces[s].classes[p].classKeyName = cls.SelectSingleNode("ClassKeyName").InnerText;

                            p++;
                        }
                        s++;

                    }
                    index++;
                }
                return oModule;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Method to create Summary table for Report
        /// </summary>
        /// <param name="summary"></param>
        public void CreateSummaryTable(Structs.Structs.Summary summary)
        {
            try
            {
                /*Get the local time zone*/
                string timeZone = TimeZone.CurrentTimeZone.StandardName;

                writer.WriteLine("</head>");
                writer.WriteLine("<body>");
                writer.WriteLine("<h2 id=\"top\" class=\"title\">"+ string.Format("Test Results - Country : {0}, Browser : {1}",  HtmlGenerator.HtmlGenerator.Country, HtmlGenerator.HtmlGenerator.Browser) 
                    +"<span class=\"info\"> (" + summary.execDate + " " + timeZone + ")</span></h2>");
                writer.WriteLine("<hr width=\"85%\" align=\"left\"/>");

                int failed = summary.executed - summary.passed;
                // set the property value
                this.Failed = failed;

                double successRate = Math.Round(summary.passed * 100.0 / summary.total, 2);

                int covered = (int)Math.Round(successRate * 2);
                int uncovered = (int)(Math.Round((summary.executed - summary.passed) * 100.0 / summary.total) * 2);
                int ignored = 200 - covered - uncovered;

                writer.WriteLine("<h3>Summary: " + "" + "</h3>");
                writer.WriteLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" width=\"60%\" style=\"border: #dcdcdc 1px solid;\">");
                writer.WriteLine("<tr valign=\"top\" class=\"TableHeader\">");
                writer.WriteLine("<td width=\"50px\"><b>Tests</b></td>");
                writer.WriteLine("<td width=\"70px\"><b>Executed</b></td>");
                writer.WriteLine("<td width=\"70px\"><b>Passed</b></td>");
                writer.WriteLine("<td width=\"70px\"><b>Failed</b></td>");
                writer.WriteLine("<td colspan=\"2\"><b>Success Rate</b></td>");
                writer.WriteLine("<td width=\"10%\" nowrap=\"nowrap\"><b>Total Time(hh:mm:ss)</b></td></tr>");
                writer.WriteLine("<tr valign=\"top\" class=\"Blue\">");
                writer.WriteLine("<td>" + summary.total.ToString() + " </td>");
                writer.WriteLine("<td>" + summary.executed.ToString() + " </td>");
                writer.WriteLine("<td>" + summary.passed.ToString() + " </td>");
                writer.WriteLine("<td>" + failed.ToString() + "</td>");
                writer.WriteLine("<td nowrap=\"nowrap\" width=\"70px\">" + successRate + "%</td>");
                writer.WriteLine("<td>");
                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                writer.WriteLine("<tr>");
                if (covered != 0)
                {
                    writer.WriteLine("<td class=\"covered\" style=\"width:" + covered + "px\"></td>");
                }
                if (uncovered != 0)
                {
                    writer.WriteLine("<td class=\"uncovered\" style=\"width:" + uncovered + "px\"></td>");
                }
                if (ignored != 0)
                {
                    writer.WriteLine("<td class=\"ignored\" style=\"width:" + ignored + "px\"></td>");
                }
                writer.WriteLine("</tr>");
                writer.WriteLine("</table>");
                writer.WriteLine("</td>");
                writer.WriteLine("<td>" + TimeSpan.FromSeconds(summary.time).ToString(@"hh\:mm\:ss") + "</td>");
                writer.WriteLine("</tr></table>");
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        /// <summary>
        /// Method for Test Project Summary Table
        /// </summary>
        /// <param name="allDetails"></param>
        public void TestProjectSummaryTable(Structs.Structs.TestProjectDetails[] allDetails)
        {
            try
            {
                writer.WriteLine("<h2>Test Container Summary</h2>");
                writer.WriteLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" width=\"85%\">");
                writer.WriteLine("<tr class=\"TableHeader\" valign=\"top\">");
                writer.WriteLine("<td width=\"30%\" colspan=\"1\"><b>Project Name</b></td>");
                writer.WriteLine("<td width=\"40%\" colspan=\"2\"><b>Success Rate</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Tests</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Success</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Inconclusive</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Failures</b></td>");
                writer.WriteLine("<td width=\"10%\" nowrap=\"nowrap\"><b>Total Execution Time(hh:mm:ss)</b></td></tr>");

                foreach (Structs.Structs.TestProjectDetails tdetail in allDetails)
                {
                    double passRate = Math.Round((tdetail.count - tdetail.fail) * 100.0 / tdetail.count, 2);
                    int success = tdetail.count - tdetail.fail - tdetail.inconclusive;
                    int inconclusive = tdetail.inconclusive;
                    int covered = (int)passRate * 2;
                    int uncovered = 200 - covered;

                    // CSS for test dlls w/ errors
                    if (tdetail.fail == 0)
                    {
                        writer.WriteLine("<tr valign=\"top\" class=\"Success\"><td width=\"25%\">");
                        writer.WriteLine(tdetail.testDll + "</td>");
                    }
                    else
                    {
                        writer.WriteLine("<tr valign=\"top\" class=\"Failure\"><td width=\"25%\">");
                        writer.WriteLine(tdetail.testDll + "</td>");
                    }
                    writer.WriteLine("<td nowrap=\"nowrap\" width=\"6%\" align=\"right\"><b>" + passRate + " %</b></td>");
                    writer.WriteLine("<td width=\"20%\" height=\"9px\">");
                    writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                    writer.WriteLine("<tr>");
                    if (covered != 0)
                    {
                        writer.WriteLine("<td class=\"covered\" style=\"width:" + covered + "px\"></td>");
                    }
                    if (uncovered != 0)
                    {
                        writer.WriteLine("<td class=\"uncovered\" style=\"width:" + uncovered + "px\"></td>");
                    }
                    writer.WriteLine("</tr>");
                    writer.WriteLine("</table>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td>" + tdetail.count + "</td><td>" + success + "</td><td>" + tdetail.inconclusive + "</td><td>" + tdetail.fail + "</td>");                    

                    #region Handle Time for Parallel Execution

                    //Extract the Minimun StartTime and Maximum EndTime from Tests
                    DateTime minStartTime = tdetail.tests.ToList().Min(strttm => strttm.StartTime);
                    DateTime maxEndTime = tdetail.tests.ToList().Max(endtm => endtm.EndTime);                    

                    //Calculate the Difference in TimeSpan                    
                    var execTime = (maxEndTime - minStartTime);

                    //Bind the difference to the Execution Time Column
                    writer.WriteLine("<td>" + execTime.ToString(@"hh\:mm\:ss") + "</td></tr>");

                    #endregion
                }
                writer.WriteLine("</table>");

                /*Close and Dispose the StreamWriter so that Temp File can be created*/
                writer.Close();
                writer.Dispose();

                #region CREATE TEMP HTML FOR EMAIL BODY

                try
                {
                    if (!(string.IsNullOrEmpty(outputHtmlFile)))
                    {
                        if (File.Exists(outputHtmlFile))
                        {
                            tempHTML = Path.GetTempPath() + "\\temp.html";

                            if (File.Exists(tempHTML))
                            {
                                File.Delete(tempHTML);
                            }
                            File.Copy(outputHtmlFile, tempHTML, true);                            

                            /*Append to the Temp HTML File*/
                            FileStream fileAppend = File.Open(tempHTML, FileMode.Append);
                            StreamWriter sWriter = new StreamWriter(fileAppend);

                            sWriter.WriteLine("<h4><u>Note</u> - This is only the test summary, please open the attached HtmlReport.zip file for detailed report.</h3></body></html>");

                            sWriter.Close();
                            sWriter.Dispose();                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                #endregion

                /*Append to the Test Result HTML File*/
                FileStream fappend = File.Open(outputHtmlFile, FileMode.Append);
                writer = new StreamWriter(fappend);

                writer.WriteLine("<hr size=\"1\" width=\"85%\" align=\"left\"></hr>");
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        /// <summary>
        /// Method for Test Details for All DLLs
        /// </summary>
        /// <param name="allDetails"></param>
        public void TestDetailsByDllTable(Structs.Structs.TestProjectDetails[] allDetails, Dictionary<string, string> testGroups)
        {
            try
            {
                foreach (Structs.Structs.TestProjectDetails tdetail in allDetails)
                {
                    for (int j = 0; j < tdetail.tests.Length; j++)
                    {
                        //Compare ExecutionID of each test with Dictionary Key and Assign TestGroup (Dictionary Value) to it
                        foreach (KeyValuePair<string, string> group in testGroups)
                        {
                            if (tdetail.tests[j].ExecutionID == group.Key)
                            {
                                tdetail.tests[j].TestGroup = group.Value;
                                break;
                            }
                        }
                    }

                    TestDetailsTable(tdetail.tests, tdetail.testDll);
                }
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        /// <summary>
        /// Metod to create Test Details for Report
        /// </summary>
        /// <param name="tests"></param>
        /// <param name="tableName"></param>
        public void TestDetailsTable(Structs.Structs.Test[] tests, string tableName)
        {
            try
            {
                writer.WriteLine("<h3 id=" + tableName + ">" + tableName + "</h3>");
                writer.WriteLine("<table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"85%\">");
                writer.WriteLine("<tr class=\"TableHeader\" valign=\"top\"><td width=\"30%\" style=\"word-break: break-all\"><b>Method Name</b></td><td width=\"20%\"><b>Description</b></td>");
                writer.WriteLine("<td width=\"5%\">Status</td><td width=\"32%\" nowrap=\"nowrap\"><b>Remarks</b></td><td width=\"10%\" nowrap=\"nowrap\"><b>Execution Time(s)</b></td></tr>");
                int index = 0;

                //Use TestGroup member to group Tests
                var testsByGroup = from test in tests orderby test.Name group test by test.TestGroup;
                //Loop over groups
                foreach (var group in testsByGroup)
                {
                    //Add row for Test Group
                    writer.WriteLine("<tr><td height=\"2px\" colspan=\"5\"></td></tr>");

                    writer.WriteLine("<tr style=\"color: #1947A3\"><td width=\"20%\" style=\"font-family:Copperplate Gothic;font-size:78%\" bgcolor=\"#E3F7F8\"><a href=\"javascript:Toggle('" + group.Key + "')\" style=\"text-decoration:none\"><b>" + "Test Group :: " + group.Key + "</b></a></td>");
                    writer.WriteLine("<td bgcolor=\"#E3F7F8\"></td>");
                    writer.WriteLine("<td width=\"10%\" style=\"padding-left:3px\" height=\"9px\" bgcolor=\"#E3F7F8\">");
                    writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                    writer.WriteLine("<tr>");
                    writer.WriteLine("<td style=\"width:100px\" bgcolor=\"#E3F7F8\"></td>");
                    writer.WriteLine("</tr>");
                    writer.WriteLine("</table>");
                    writer.WriteLine("</td>");
                    writer.WriteLine("<td width=\"10%\" bgcolor=\"#E3F7F8\"></td>");
                    writer.WriteLine("<td bgcolor=\"#E3F7F8\"></td></tr>");

                    writer.WriteLine("<tr><td class=\"section\" colspan=\"5\"><table border=\"0\" cellpadding=\"1\" cellspacing=\"1\" width=\"100%\" style=\"display: none !important;\" id=\"" + group.Key + "\">");

                    //Loop over tests in each test group
                    foreach (var test in group)
                    {
                        if (test.Status == "PASS" || test.Status == "COMPLETED")
                        {
                            writer.WriteLine("<tr valign=\"top\" class=\"Pass\"><td width=\"29%\" class=\"SuccessTest\" style=\"word-break: break-all\">");
                            if (null != test.ErrorMessage)
                            {                                                         
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"covered\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\">" + Regex.Replace((test.ErrorMessage.Replace("\r\n", "<br/>")), @"[<>]", "\"") + "</td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            }
                            else
                            {
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"covered\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\"> NA </td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            }
                        }
                        else if (test.Status == "NOTRUN")
                        {
                            writer.WriteLine("<tr valign=\"top\" class=\"Ignored\"><td width=\"29%\" style=\"word-break: break-all\">" + test.Name + "</td>");
                            writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                            writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                            writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                            writer.WriteLine("<tr>");
                            writer.WriteLine("<td class=\"ignored\" style=\"width:100px\"></td>");
                            writer.WriteLine("</tr>");
                            writer.WriteLine("</table>");
                            writer.WriteLine("</td>");
                            writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\"> NA </td>");
                            writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                        }
                        else if (test.Status.ToUpper() == "INCONCLUSIVE")
                        {

                            writer.WriteLine("<tr valign=\"top\" ><td width=\"29%\" class=\"Inconclusive\" style=\"word-break: break-all\">");
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"Inconclusive\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\">" + Regex.Replace((test.ErrorMessage.Replace("\r\n", "<br/>")), @"[<>]", "\"") + "</td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            
                        }
                        else
                        {
                            if (null != test.ErrorMessage)
                            {
                                writer.WriteLine("<tr valign=\"top\" ><td width=\"29%\" class=\"FailureTest\" style=\"word-break: break-all\">");
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"uncovered\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\">" + Regex.Replace((test.ErrorMessage.Replace("\r\n", "<br/>")), @"[<>]", "\"") + "</td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            }
                            else if (null != test.Description)
                            {
                                writer.WriteLine("<tr valign=\"top\" ><td width=\"29%\" class=\"FailureTest\" style=\"word-break: break-all\">");
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"uncovered\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\">" + (test.Comments == null ? "NA" : test.Comments) + "</td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            }
                            else
                            {
                                writer.WriteLine("<tr valign=\"top\" ><td width=\"29%\" class=\"FailureTest\" style=\"word-break: break-all\">");
                                writer.WriteLine(test.Name + "</td>");
                                writer.WriteLine("<td width=\"19%\">" + test.Description + "</td>");
                                writer.WriteLine("<td width=\"9%\" style=\"padding-left:3px\" height=\"9px\">");
                                writer.WriteLine("<table cellspacing=\"0\" cellpadding=\"0\">");
                                writer.WriteLine("<tr>");
                                writer.WriteLine("<td class=\"uncovered\" style=\"width:100px\"></td>");
                                writer.WriteLine("</tr>");
                                writer.WriteLine("</table>");
                                writer.WriteLine("</td>");
                                writer.WriteLine("<td width=\"345px\" style=\"word-break: break-all\">" + (test.Comments == null ? "NA" : test.Comments) + "</td>");
                                writer.WriteLine("<td>" + test.Time.ToString() + "</td></tr>");
                            }
                        }
                        index++;
                    }

                    writer.WriteLine("</table></td></tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("<a href=\"#top\">Back to top</a>");
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        /// <summary>
        /// Method to create CC table for Report
        /// </summary>
        /// <param name="modules"></param>
        public void CreateCCTable(Structs.Structs.Module[] modules)
        {
            try
            {
                ////calculate percentages
                double pNotCoveredBlocks;
                double pCoveredBlocks;

                writer.WriteLine("<h3>Code Coverage Summary</h3>");
                writer.WriteLine("<table border=\"0\" cellpadding=\"2\" cellspacing=\"0\" width=\"95%\">");
                writer.WriteLine("<tr class=\"TableHeader\" valign=\"top\">");
                writer.WriteLine("<td width=\"40%\"><b>Name</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Not Covered (Blocks)</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>%Not Covered (Blocks)</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>Covered (Blocks)</b></td>");
                writer.WriteLine("<td width=\"10%\"><b>%Covered (Blocks)</b></td>");
                writer.WriteLine("</tr>");

                foreach (Structs.Structs.Module module in modules)
                {
                    pNotCoveredBlocks = Convert.ToDouble((Convert.ToInt32(module.blocksNotCovered) * 100)) / Convert.ToDouble((Convert.ToInt32(module.blocksCovered) + Convert.ToInt32(module.blocksNotCovered)));
                    pCoveredBlocks = Convert.ToDouble((Convert.ToInt32(module.blocksCovered) * 100)) / Convert.ToDouble((Convert.ToInt32(module.blocksCovered) + Convert.ToInt32(module.blocksNotCovered)));

                    writer.WriteLine("<tr valign=\"top\">");
                    writer.WriteLine("<td width=\"40%\"><b>" + module.moduleName + "</b></td>");
                    writer.WriteLine("<td width=\"10%\">" + module.blocksNotCovered + "</td>");
                    writer.WriteLine("<td width=\"10%\">" + Math.Round(pNotCoveredBlocks, 2) + "%" + "</td>");
                    writer.WriteLine("<td width=\"10%\">" + module.linesNotCovered + "</td>");
                    writer.WriteLine("<td width=\"10%\">" + Math.Round(pCoveredBlocks, 2) + "%" + "</td>");

                    foreach (Structs.Structs.NamespaceTable space in module.namespaces)
                    {
                        pNotCoveredBlocks = (Convert.ToDouble(module.blocksNotCovered) * 100) / (Convert.ToDouble(module.blocksCovered) + Convert.ToDouble(module.blocksNotCovered));
                        pCoveredBlocks = (Convert.ToDouble(module.blocksCovered) * 100) / (Convert.ToDouble(module.blocksCovered) + Convert.ToDouble(module.blocksNotCovered));

                        writer.WriteLine("<tr valign=\"top\">");
                        writer.WriteLine("<td width=\"40%\"><b>" + space.name + "</b></td>");
                        writer.WriteLine("<td width=\"10%\">" + space.blocksNotCovered + "</td>");
                        writer.WriteLine("<td width=\"10%\">" + Math.Round(pNotCoveredBlocks, 2) + "%" + "</td>");
                        writer.WriteLine("<td width=\"10%\">" + space.linesNotCovered + "</td>");
                        writer.WriteLine("<td width=\"10%\">" + Math.Round(pCoveredBlocks, 2) + "%" + "</td>");

                        writer.WriteLine("</tr>");

                    }
                    writer.WriteLine("</tr>");
                }
                writer.WriteLine("</table>");
                writer.WriteLine("<hr size=\"1\" width=\"95%\" align=\"left\"></hr>");
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }

        public void CloseStreamWriter()
        {
            if (writer != null)
            {
                if (writer.BaseStream != null)
                {
                    writer.WriteLine("</body>");
                    writer.WriteLine("</html>");
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public DirectoryInfo FetchLatestDirectory(string directoryPath)
        {
            try
            {
                var directory = new DirectoryInfo(directoryPath);

                var TRXfile = directory.GetDirectories("*", SearchOption.TopDirectoryOnly).OrderByDescending(f => f.LastWriteTime).First();

                return TRXfile;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return null;
            }
        }

        public FileInfo FetchLatestFile(string directoryPath, string fileExtension)
        {
            try
            {
                var directory = new DirectoryInfo(directoryPath);
                string extension = "*." + fileExtension;
                var TRXfile = (from f in directory.GetFiles(extension) orderby f.LastWriteTime descending select f).First();

                return TRXfile;
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
                return null;
            }
        }

        public void SendEmailReport(string outputHtmlPath, DateTime runStartTime)
        {
            string fileContents = string.Empty;
            string zipHTMLPath = string.Empty;
            string zipPath = string.Empty;

            if ((!string.IsNullOrEmpty(tempHTML)) && (new FileInfo(tempHTML).Length > 0))
            {
                fileContents = File.ReadAllText(tempHTML);
            }
            else
            {
                fileContents = File.ReadAllText(outputHtmlPath);
            }            

            SMTPDetails smtpDetails = new SMTPDetails
            {
                SmtpHost = Utilities.Utilities.EnvironmentSettings["SMTPClient"],
                SmtpPort = Convert.ToInt32(Utilities.Utilities.EnvironmentSettings["SMTPPort"]),
                IsSSLEnabled = false,

                /* Use the below line for specifying Delivery Method */
                //DeliveryMethod = SmtpDeliveryMethod.Network,

                SmtpDomain = Utilities.Utilities.EnvironmentSettings["SMTP_domain"],
                SmtpUserName = Utilities.Utilities.EnvironmentSettings["SMTP_username"],
                SmtpPassword = Utilities.Utilities.EnvironmentSettings["SMTP_password"]

            };

            List<MailAddress> lstMailto = new List<MailAddress>();

            string mailTo = Utilities.Utilities.EnvironmentSettings["MailTo"];

            foreach (var curr_address in mailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                MailAddress mytoAddress = new MailAddress(curr_address);
                lstMailto.Add(mytoAddress);
            }

            EMailMessage emailMsg = new EMailMessage
            {
                MailFrom = Utilities.Utilities.EnvironmentSettings["MailFrom"],
                MailTo = lstMailto,
                IsBodyHtml = true,
                Body = fileContents,
                Subject = "Test Run Report for " + DateTime.Now.ToLocalTime().ToShortDateString()
            };

            /*Zip File for HTML Report*/
            zipHTMLPath = Utilities.Utilities.EnvironmentSettings["UITestHtmlReportPath"] + @"\" + "HtmlReport.zip";
            ArchiveCreator archiveHTMLCreator = new ZipShellCreator(zipHTMLPath);
            Archive htmlArchive = archiveHTMLCreator.GetArchieve();
            htmlArchive.AddFile(outputHtmlPath);
            htmlArchive.SaveArchive();

            /*Object for Email Attachment(s)*/
            Attachment[] attachments = null;
            Attachment attachZipHtml = new Attachment(zipHTMLPath);
            Attachment attachZipScreenshots = null;

            if (this.Failed > 0)
            {
                var screenShots = Utilities.Utilities.EnvironmentSettings["ScreenShotPath"];

                #region For Parallel Execution
                
                var directoryScreenShots = new DirectoryInfo(screenShots);
                DateTime currentTimeStamp = DateTime.ParseExact(DateTime.Now.ToLocalTime().ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                /*Write current time & zone to log file*/
                customLogs.LogInformation("Current Date-Time is " + currentTimeStamp.ToString() + " and Time Zone is " + TimeZone.CurrentTimeZone.StandardName);

                string fileExtension = "*.jpeg";
                
                /*Fetch all the directories created by the latest parallel run*/
                var errorSnapShotDir = directoryScreenShots.GetDirectories()
                  .Where(dir => dir.LastWriteTime >= runStartTime && dir.LastWriteTime <= currentTimeStamp);

                List<FileInfo> lstErrorScreenShots = new List<FileInfo>();                

                /*Fetch all the error snapshots from each directory*/
                foreach (var folder in errorSnapShotDir)
                {
                    lstErrorScreenShots.AddRange(folder.GetFiles(fileExtension));                    
                }

                /*Write Screenshot Count to HTML Log*/
                customLogs.LogInformation("Total ErrorScreenshots for " + runStartTime.ToString() + " Test Run - " + (lstErrorScreenShots == null? "0" : lstErrorScreenShots.Count.ToString()));

                /*Create a Zip Folder*/
                zipPath = screenShots + @"\" + "ErrorScreenshots.zip";
                ArchiveCreator archiveCreator = new ZipShellCreator(zipPath);
                Archive archive = archiveCreator.GetArchieve();

                /*Add each error screenshot to Zip Folder*/
                foreach (var file in lstErrorScreenShots)
                {
                    archive.AddFile(file.FullName);
                    
                    /*Write name of each Error Screenshot file to HTML Log*/
                    customLogs.LogInformation("File added to archive - " + file.FullName);
                }

                /*Save the Zip Folder*/
                archive.SaveArchive();

                attachments = new Attachment[2];
                //Attach ErrorScreenShot Zipped File
                attachments[1] = attachZipScreenshots = new Attachment(zipPath);
                //Attach HTML Report File
                attachments[0] = attachZipHtml;

                #endregion                
            }
            else
            {
                attachments = new Attachment[1];
                //Attach HTML Report File
                attachments[0] = attachZipHtml;
            }

            Utilities.Utilities.SendMail(smtpDetails, emailMsg, attachments);
            
            attachZipHtml.Dispose();

            if (attachZipScreenshots != null)
                attachZipScreenshots.Dispose();

            /*Delete the temp.html file*/
            if (File.Exists(tempHTML))
            {
                File.Delete(tempHTML);
            }

            /*Delete the HTML Zip file*/
            if (File.Exists(zipHTMLPath))
            {
                File.Delete(zipHTMLPath);
            }

            /*Delete the Zipped Screenshots file*/
            if (File.Exists(zipPath))
            {                
                File.Delete(zipPath);
            }
        }

        public ResultSummary GetResultSummary(XmlDocument doc)
        {
            ResultSummary summary = new ResultSummary();
            XmlNode sNode = doc.SelectSingleNode("//ResultSummary");
            XmlSerializer serializer = new XmlSerializer(typeof(ResultSummary));
            using (var stringReader = new System.IO.StringReader(sNode.OuterXml))
            {
                object xmlData = serializer.Deserialize(stringReader);
                summary = (ResultSummary)xmlData;
            }
            return summary;

        }

        public Times GetTimes(XmlDocument doc)
        {
            Times times = new Times();
            XmlNode tNode = doc.SelectSingleNode("//Times");
            XmlSerializer serializer = new XmlSerializer(typeof(Times));
            using (var stringReader = new System.IO.StringReader(tNode.OuterXml))
            {
                object xmlData = serializer.Deserialize(stringReader);
                times = (Times)xmlData;
            }
            return times;
        }


        public void CreateSummary(ResultSummary summary, Times times)
        {
            try
            {                
                string cssText = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["theme"]);
                cssText = string.Format("<style>{0}</style>", cssText);
                writer.WriteLine(cssText);
                writer.WriteLine("</head>");
                writer.WriteLine("<body>");

                /*Get the local time zone*/
                string timeZone = TimeZone.CurrentTimeZone.StandardName;
                // Read Html file
                string summaryText = System.IO.File.ReadAllText(ConfigurationManager.AppSettings["summaryHTML"]);
                summaryText = string.Format(summaryText, new object[] {
                string.Format("Test Results - Country : {0}, Browser : {1}",  HtmlGenerator.HtmlGenerator.Country, HtmlGenerator.HtmlGenerator.Browser),
                string.Format("{0} {1}",times.Creation.ToString("dd-MMM-yyyy hh:mm") , timeZone),
                summary.Outcome,
                summary.ExecutionCounters.PendingTestCases,
                summary.ExecutionCounters.InProgressTestCases,
                summary.ExecutionCounters.CompletedTestCases,
                summary.ExecutionCounters.Warnings,
                summary.ExecutionCounters.Disconnected,
                summary.ExecutionCounters.NotExecuted,
                summary.ExecutionCounters.NotRunnable,
                summary.ExecutionCounters.PassedButRunAborted,
                summary.ExecutionCounters.Inconclusive,
                summary.ExecutionCounters.Aborted,
                summary.ExecutionCounters.Timeout,
                summary.ExecutionCounters.Failed,
                summary.ExecutionCounters.Error,
                summary.ExecutionCounters.Passed,
                summary.ExecutionCounters.Executed,
                summary.ExecutionCounters.Total,
                summary.ExecutionCounters.Passed
                });
                writer.WriteLine(summaryText);
            }
            catch (Exception ex)
            {
                customLogs.LogError(ex);
            }
        }
    }
}
