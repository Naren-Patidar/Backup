#region Using
using System;
using System.Data;
using System.Xml;
using System.Drawing;
using System.Configuration;
using System.Web.UI;
using System.IO;
using System.Reflection;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.SessionState;
//using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils
{
    #region Header
    /// <department>Fujitsu, e-Innovation, eCRM</department>
    /// <copyright>(c) Fujitsu Consulting, 2002</copyright>
    /// <development> 
    ///		<version number="1.11" day="03" month="02" year="2003">
    ///			<developer>Steve Lang</developer>
    ///			<checker>Tom Bedwell</checker>
    ///			<work_packet>WP/Seoul/051</work_packet>
    ///			<description>Do not use SharedUtils resources.
    ///			Use the resource supplied by the Web Application via
    ///			the constructor</description>
    ///		</version>
    ///		<version number="1.10" day="16" month="01" year="2003">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Steve Lang</checker>
    ///			<work_packet>WP/Barcelona/046</work_packet>
    ///			<description>Namespaces conform to standards</description>
    ///		</version>
    ///		<version number="1.01" day="06" month="11" year="2002">
    ///			<developer>Mark Hart</developer>
    ///			<checker>Stephan Lang</checker>
    ///			<work_packet>WP/Barcelona/031</work_packet>
    ///			<description>Corrected namespace.</description>
    ///		</version>
    ///		<version number="1.00" day="29" month="10" year="2002">
    ///			<developer>Mark Hart</developer>
    ///			<checker>Lawrie Griffiths</checker>
    ///			<work_packet>Bugzilla #60</work_packet>
    ///			<description>Initial Implementation</description>
    ///		</version>
    /// </development>
    /// <summary>
    /// A class to handle and process different kinds of exceptions
    /// and errors generated and captured by the wen user control
    /// panels.
    /// </summary>
    #endregion

    public class WebException
    {
        #region Declarations
        private ITrace trace = new Trace();
        private const string errorConfigTable = "action";
        private const string columnActor = "actor";
        private const string columnCategory = "category";
        private const string columnName = "name";
        private const string columnPage = "page";
        private const string columnPublish = "publish";
        private const string columnRedirectUrl = "redirecturl";
        private const string columnResourceIndex = "resourceindex";
        private const string na = "GeneralError.NA";
        #endregion

        #region Properties

        private Exception exceptionObj;
        /// <summary>
        /// Get or set the current exception for this error.
        /// </summary>
        public Exception ExceptionObj
        {
            get { return exceptionObj; }
            set { exceptionObj = value; }
        }

        private string actor;
        /// <summary>
        /// Set the actor of the exception, if there is one.
        /// </summary>
        public string Actor
        {
            get { return this.actor; }
            set
            {
                // Check to see if we have a value, otherwise, set
                // to a default.
                if (StringUtils.IsStringEmpty(value))
                    // No value supplied, so default.
                    this.actor = "default";
                else
                    // A value has been supplied, so use it,
                    // ensuring that lowercase is used.
                    this.actor = value.ToLower();
            }
        }

        private string category;
        /// <summary>
        /// Set the category of the exception, if there is one.
        /// </summary>
        public string Category
        {
            get { return this.category; }
            set
            {
                // Check to see if we have a value, otherwise, set
                // to a default.
                if (StringUtils.IsStringEmpty(value))
                    // No value supplied, so default.
                    this.category = "default";
                else
                    // A value has been supplied, so use it.
                    this.category = value.ToLower();
            }
        }

        private string name;
        /// <summary>
        /// Get or set the result XML generated from a failed
        /// call to a Crm Server web service.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                // Check to see if we have a value, otherwise, set
                // to a default.
                if (StringUtils.IsStringEmpty(value))
                    // No value supplied, so default.
                    this.name = "default";
                else
                    // A value has been supplied, so use it.
                    this.name = value.ToLower();
            }
        }

        private string resultXml;
        /// <summary>
        /// Get or set the result XML generated from a failed
        /// call to a Crm Server web service.
        /// </summary>
        public string ResultXml
        {
            get { return resultXml; }
            set { resultXml = value; }
        }

        private string errorMessage;
        /// <summary>
        /// Get or set the result XML generated from a failed
        /// call to a Crm Server web service.
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        private string pageName;
        /// <summary>
        /// Get or set the name of the page that has generated
        /// the exception..
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set
            {
                // Check to see if we have a value, otherwise, set
                // to a default.
                if (StringUtils.IsStringEmpty(value))
                    // No value supplied, so default.
                    pageName = "default";
                else
                    // A value has been supplied, so use it.
                    pageName = value;
            }
        }

        private Page page;
        /// <summary>
        /// Get or set the page that has generated the
        /// error or exception.
        /// </summary>
        public Page Page
        {
            get { return page; }
            set
            {
                page = value;
                // Also set the pageName whilst we're here.
                string crude = page.ToString().ToLower();
                string[] bits = crude.Split(new Char[] { '.' });
                bits = bits[1].Split(new Char[] { '_' });
                this.PageName = bits[0] + "." + bits[1];
            }
        }

        private HttpContext httpContext;
        /// <summary>
        /// Get or set the current HTTP context of the web application.
        /// </summary>
        public HttpContext HttpContext
        {
            get { return httpContext; }
            set { this.httpContext = value; }
        }

        private string pageRedirect;
        /// <summary>
        /// Get or set the page that has generated the
        /// error or exception.
        /// </summary>
        public string PageRedirect
        {
            get { return pageRedirect; }
            set { this.pageRedirect = value; }
        }

        private string errorId;
        /// <summary>
        /// Get or set the page that has generated the
        /// error or exception.
        /// </summary>
        public string ErrorId
        {
            get { return errorId; }
            set { this.errorId = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the WebException class.
        /// </summary>
        public WebException()
        {
        }
        #endregion

        #region Public Methods

        #region Process Exception Methods
        /// <summary>
        /// Process the exception with the current property
        /// settings.
        /// </summary>
        public void ProcessException()
        {
        }
        public void ProcessException(Exception ex)
        {
            this.PublishException(ex);
        }
        /// <summary>
        /// Process the exception. The current property settings
        /// will be ignored and the method parameters used
        /// instead.
        /// </summary>
        /// <param name="control">The current control.</param>
        /// <param name="exception">The exception raised by the web page.</param>
        public void ProcessException(
            Control control,
            Exception exception
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.ProcessException1");
                // Set the properties with the supplied parameters.
                this.Page = control.Page;
                this.ExceptionObj = exception;
                // Remove all sub controls to prevent rendering which might cause further errors
                control.Controls.Clear();
                // Process the exception.
                this.ErrorMessage = this.CheckException(this.exceptionObj);
                this.HandleException();
            }
            catch { }
            finally
            {
                ts.EndProc();
            }
        }

        /// <summary>
        /// Process the exception. The current property settings
        /// will be ignored and the method parameters used
        /// instead.
        /// </summary>
        /// <param name="control">The current page.</param>
        /// <param name="resultXml">The result XML generated from the Crm Server
        /// web service call.</param>
        /// 

        // Keep this overload for backward compatibility
        // Always remove the controls by default
        public void ProcessException(
            Control control,
            string resultXml
            )
        {
            ProcessException(control, resultXml, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="resultXml"></param>
        /// <param name="removeControls"></param>
        public void ProcessException(
            Control control,
            string resultXml,
            bool removeControls
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.ProcessException2");
                // Set the properties with the supplied parameters.
                this.Page = control.Page;
                this.ResultXml = resultXml;
                // Remove all sub controls to prevent rendering which might cause further errors
                if (removeControls)
                {
                    control.Controls.Clear();
                }
                // Process the exception.
                //this.ErrorMessage = this.CheckResultXml(this.resultXml);

                //Convergence changes
                this.ErrorMessage = this.resultXml;
                this.HandleException();
            }
            catch { }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        /// <summary>
        /// Process the exception. The current property settings
        /// will be ignored and the method parameters used
        /// instead.
        /// </summary>
        /// <param name="exception">The exception raised by the web page.</param>
        /// <param name="context">The current context of the web application.</param>
        public void ProcessException(
            Exception exception,
            HttpContext context
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.ProcessException2");
                this.ExceptionObj = exception;
                this.httpContext = context;
                // Process the exception.
                this.ErrorMessage = this.CheckException(this.exceptionObj);
                this.HandleException();
            }
            catch { }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        #region Process Error Methods
        /// <summary>
        /// Reset the current object properties to original settings.
        /// </summary>
        public void ResetProperties()
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.ResetProperties");

                this.ExceptionObj = null;
                this.Page = null;
                this.ResultXml = null;
            }
            catch { }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        #region Private Methods

        #region Handle Exception
        /// <summary>
        /// Process the current property setting i.e. the current
        /// exception.
        /// </summary>
        private void HandleException()
        {

            // Do the tracing stuff.
            ITraceState ts = trace.StartProc("WebException.HandleException");
            try
            {

                // Check if the Error.config should be parsed for this
                // error.
                string exceptionMessage = this.errorMessage;
                bool publish = false;

                // Yes it should, so see if a match can be found with
                // the error message.
                string redirectUrl = null;
                string messageIndex = null;

                if (this.CheckErrorConfig(this.actor, this.category, this.pageName,
                    out redirectUrl, out messageIndex, out publish))
                {

                    // A match has been found, so check if a message is required
                    // to be set.
                    if (StringUtils.IsStringEmpty(messageIndex))
                    {
                        // No message index was available, so use the exception
                        // message instead.
                        exceptionMessage = this.errorMessage;
                    }
                    else
                    {
                        // A index was found, so use this instead.
                        exceptionMessage = messageIndex;
                    }

                    // Check if we need to re-driect.
                    if ((StringUtils.IsStringEmpty(redirectUrl)) && (this.page == null))
                    {
                        redirectUrl = System.Configuration.ConfigurationSettings.AppSettings["GeneralErrorPage"];
                    }

                    // Check if we need to re-driect.
                    if (!StringUtils.IsStringEmpty(redirectUrl))
                    {

                        StringBuilder uri = new StringBuilder(redirectUrl);
                        uri.AppendFormat("?{0}={1}", WebSessionIndexes.ExceptionMessage(), exceptionMessage);
                        uri.AppendFormat("&{0}={1}", WebSessionIndexes.ExceptionErrorId(), this.errorId);
                        this.pageRedirect = uri.ToString();

                        // Check what to use to store into the session. If page attribute
                        // has not been set, then use the context.
                        if (this.page != null)
                        {
                            if (this.page.Response != null)
                            {
                                this.page.Response.Redirect(this.pageRedirect, false);
                            }
                        }
                        else if (this.httpContext != null)
                        {
                            // Try to remove contents of response
                            if (this.httpContext.Response != null)
                            {
                                try
                                {
                                    this.httpContext.Response.ClearContent();
                                }
                                catch
                                {
                                }
                            }
                            if (this.httpContext.Server != null)
                            {
                                this.httpContext.Server.Transfer(this.pageRedirect, true);
                            }
                        }
                    }
                }

                // Publish the exception if it needs it.
                if (publish)
                {
                    this.PublishException(this.exceptionObj);
                }
            }
            catch (Exception e)
            {
                if (this.exceptionObj != null)
                {
                    //ExceptionManager.Publish(this.exceptionObj);
                    // changes as part of Convergence below logger class will be using Enterprise Library 2.0
                    bool logstatus = ExceptionPolicy.HandleException(exceptionObj, "Log Only Policy");
                }
                //ExceptionManager.Publish(e);
                // changes as part of Convergence below logger class will be using Enterprise Library 2.0
                bool status = ExceptionPolicy.HandleException(e, "Log Only Policy");

                // Failed to redirect/transfer, as last resort send
                // status code 500
                try
                {
                    if (this.page != null)
                    {
                        this.page.Response.StatusCode = 500;
                        this.page.Response.End();
                    }
                    else if (this.httpContext != null)
                    {
                        if (this.httpContext.Response != null)
                        {
                            this.httpContext.Response.StatusCode = 500;
                            this.httpContext.Response.End();
                        }
                    }
                }
                catch
                {
                }

            }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        #region Check Exception
        /// <summary>
        /// Get the message from the exception.
        /// </summary>
        /// <param name="exception">The exception to be investigated.</param>
        /// <returns>The message from the exception.</returns>
        private string CheckException(Exception exception)
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            string rawMessage = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.CheckException");
                // Try and get a message id by looking at the start of
                // the exception.
                rawMessage = exception.Message;
                this.DetermineErrorId(rawMessage);
                return rawMessage;
            }
            catch
            {
                return rawMessage;
            }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion


        //Convergence Changes
        #region Check Result Xml
        /// <summary>
        /// Get the message, actor and category out of the result
        /// message.
        /// </summary>
        /// <param name="resultXml">The result XML returned from a CRM
        /// server web service call.</param>
        /// <returns>The error message.</returns>
        //private string CheckResultXml(string resultXml) 
        //{
        //    // Do the tracing stuff.
        //    ITraceState ts = null;
        //    try 
        //    {
        //        // Do the tracing stuff.
        //        ts = trace.StartProc("WebException.CheckresultXml");
        //        trace.WriteDebug("Checking result XML " + resultXml);
        //        // Create a new result and get the message.
        //        //Convergence change
        //        Result r = new Result();
        //        r.LoadXml(resultXml);
        //        r.MoveToTopResult();
        //        string uiMessage;
        //        r.GetResultElementByName("ui_message",out uiMessage);
        //        this.DetermineErrorId(uiMessage);
        //        r.MoveToTopResult();
        //        string actor, category, name;
        //        r.GetResultElementByName("actor",out actor);
        //        r.MoveToTopResult();
        //        r.GetResultElementByName("category",out category);
        //        r.MoveToTopResult();
        //        r.GetResultElementByName("name",out name);
        //        this.Actor = actor;
        //        this.Category = category;
        //        this.Name = name;
        //        trace.WriteDebug("Checked result XML and found ui_message=" +
        //            uiMessage + ", actor=" + actor + " and category=" + category);
        //        // Convert into an exception before leaving.
        //        this.ExceptionObj = r.ToException();
        //        return uiMessage;
        //    }
        //    catch 
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        ts.EndProc();
        //    }

        //}

        private void DetermineErrorId(string message)
        {
            Regex r = new Regex(@"^(?<errorid>\d+)");
            Match m = r.Match(message);
            if (m.Success)
                // There is an id, so set it.
                this.errorId = m.Groups["errorid"].Value;
            else
                this.errorId = na;
        }
        #endregion

        #region Publish Exception
        /// <summary>
        /// Publish an exception to the Microsoft Exception handling
        /// mechanism.
        /// </summary>
        /// <param name="exception">The exception to publish.</param>
        private void PublishException(
            Exception exception
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.PublishException");
                // Publish the exception.

                //ExceptionManager.Publish(exception);
                // changes as part of Convergence below ExceptionPolicy class will be using Enterprise Library 2.0
                bool logstatus = ExceptionPolicy.HandleException(exception, "Log Only Policy");
            }
            catch (Exception e)
            {
                bool logstatus = ExceptionPolicy.HandleException(e, "Log Only Policy");
            }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        #region Check Error.config File
        /// <summary>
        /// Use the supplied error message and attempt to find a
        /// match in the Error.config file. If a match is found,
        /// then return the name of the page to re-direct to.
        /// </summary>
        /// <param name="actor">The actor to check for.</param>
        /// <param name="category">The category to check for.</param>
        /// <param name="pageName">The pagename to check for.</param>
        /// <param name="resourceIndex">The resource file index name for the error message.</param>
        /// <param name="redirectUrl">The page to re-driect to if a match is found.</param>
        /// <param name="publish">Whether the exception needs to be published.</param>
        /// <returns>True if a match has been found.</returns>
        private bool CheckErrorConfig(
            string actor,
            string category,
            string pageName,
            out string redirectUrl,
            out string resourceIndex,
            out bool publish
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            HiResTimer timer = null;
            redirectUrl = null;
            resourceIndex = null;
            DataSet ds = null;
            bool matchFound = false;
            publish = false;
            XmlTextReader reader = null;

            try
            {
                // Do the tracing and timer stuff.
                ts = trace.StartProc("WebException.CheckErrorConfig");
                timer = new HiResTimer();
                // Get the name of the current directory.
                string rootDir = null;
                try { rootDir = ConfigurationSettings.AppSettings["RootDirectory"]; }
                catch { }
                // Add a "\" to the end of the directory name if its not already there.
                if ((!rootDir.EndsWith(@"\")) && (!rootDir.EndsWith(@"/")))
                    rootDir += @"\";
                // Set the full name of the Error.config file.
                string errorFile = rootDir + "Error.config";
                // Record the filename to the trace log.
                trace.WriteDebug("Error.config file is " + errorFile);
                // Check to see if the file exists.
                if (File.Exists(errorFile))
                {
                    // Yes, the file does exist, so load it into an XML document
                    // Time the event.
                    timer.Start();
                    // Open the Error.config file to read.
                    reader = new XmlTextReader(errorFile);
                    reader.WhitespaceHandling = WhitespaceHandling.None;
                    // Create the dataset to store the Error.config
                    // file.
                    ds = this.CreateErrorConfigDataSet();
                    // Load the dataset with the contents of the Error.config
                    // file.
                    string xmlValue;
                    bool insideError = false;
                    DataRow dr = null;
                    trace.WriteDebug("Starting parse of Error.config file");
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                // Check if this is an element inside the <error>
                                // element.
                                if (insideError)
                                {
                                    // Yes it is, so determine action this element.
                                    trace.WriteDebug("Inside error element.");
                                    switch (reader.Name)
                                    {
                                        case "actor":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString().ToLower();
                                            dr[columnActor] = xmlValue;
                                            trace.WriteDebug("Element actor=" + xmlValue);
                                            break;
                                        case "category":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString().ToLower();
                                            dr[columnCategory] = xmlValue;
                                            trace.WriteDebug("Element category=" + xmlValue);
                                            break;
                                        case "pagename":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString();
                                            dr[columnPage] = xmlValue;
                                            trace.WriteDebug("Element pagename=" + xmlValue);
                                            break;
                                        case "publish":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString();
                                            dr[columnPublish] = xmlValue;
                                            trace.WriteDebug("Element publish=" + xmlValue);
                                            break;
                                        case "redirecturl":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString();
                                            dr[columnRedirectUrl] = xmlValue;
                                            trace.WriteDebug("Element redirecturl=" + xmlValue);
                                            break;
                                        case "resourceindex":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString();
                                            dr[columnResourceIndex] = xmlValue;
                                            trace.WriteDebug("Element resourceindex=" + xmlValue);
                                            break;
                                        case "name":
                                            // Move to the value.
                                            reader.Read();
                                            xmlValue = reader.Value.ToString().ToLower();
                                            dr[columnName] = xmlValue;
                                            trace.WriteDebug("Element name=" + xmlValue);
                                            break;
                                        default:
                                            // Not an expected element, so do nothing.
                                            break;
                                    }
                                }
                                else
                                {
                                    trace.WriteDebug("Not inside error element.");
                                    // Check if this element is the start of an error.
                                    if (reader.Name == "error")
                                    {
                                        // it is, so create a new datarow and set the
                                        // flag.
                                        trace.WriteDebug("Found starting error element.");
                                        dr = this.CreateNewDataRowInErrorConfigTable(ds);
                                        insideError = true;
                                    }
                                }
                                break;
                            case XmlNodeType.EndElement:
                                // Only interested in the end of error elements
                                // i.e. </error>
                                if (reader.Name == "error")
                                {
                                    // This is the end of this current error, so
                                    // add the datarow to the table and set the
                                    // flag.
                                    trace.WriteDebug("Found ending error element.");
                                    try
                                    {
                                        trace.WriteDebug("Adding current datarow to datatable.");
                                        ds.Tables[errorConfigTable].Rows.Add(dr);
                                    }
                                    catch (Exception ex)
                                    {
                                        trace.WriteInfo("Unable to write row to datatable because" + ex.Message);
                                    }
                                    insideError = false;
                                }
                                break;
                            default:
                                // Not bothered about any other areas, so just
                                // ignore.
                                break;
                        }
                    }
                    // Close the reader.
                    reader.Close();
                    // Stop the timer and record the outcome.
                    timer.Stop();
                    trace.WriteInfo("Error.config loaded in " + timer.ElapsedMilliseconds.ToString("F3") +
                        "ms");
                    // Start the timer again.
                    timer.Start();
                    // For the supplied actor, category and page, attempt
                    // to find the redirectUrl and resourceindex if there
                    // is one.
                    if (this.FindRowInErrorConfigDataTable(ds, actor, category,
                        pageName, name, out redirectUrl, out resourceIndex, out publish))
                        // A match has been found for the parameters passed,
                        // so return true.
                        return true;
                }
            }
            catch (Exception ex)
            {
                trace.WriteInfo("Problem parsing Error.config file because" + ex.Message);
                return false;
            }
            finally
            {
                // Ensure that the reader is closed.
                if (reader != null)
                    reader.Close();
                // Stop the timer, even if it doesn't exist.
                timer.Stop();
                ts.EndProc();
            }
            return matchFound;
        }
        #endregion

        #region Error.config DataSet
        /// <summary>
        /// Create a dataset to store the contents of the Error.config
        /// file.
        /// </summary>
        /// <returns>The dataset to store the Error.config file.</returns>
        private DataSet CreateErrorConfigDataSet()
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            DataSet ds = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.CreateErrorConfigDataSet");
                // Create the dataset to store the information.
                ds = new DataSet();
                // Create the table.
                DataTable dt = new DataTable(errorConfigTable);
                // Create the columns.
                DataColumn[] primaryKeys = new DataColumn[4];
                DataColumn dc = new DataColumn(columnActor, Type.GetType("System.String"));
                primaryKeys[0] = dc;
                dt.Columns.Add(dc);
                dc = new DataColumn(columnCategory, Type.GetType("System.String"));
                primaryKeys[1] = dc;
                dt.Columns.Add(dc);
                dc = new DataColumn(columnName, Type.GetType("System.String"));
                primaryKeys[2] = dc;
                dt.Columns.Add(dc);
                dc = new DataColumn(columnPage, Type.GetType("System.String"));
                primaryKeys[3] = dc;
                dt.Columns.Add(dc);
                dc = new DataColumn(columnRedirectUrl, Type.GetType("System.String"));
                dt.Columns.Add(dc);
                dc = new DataColumn(columnResourceIndex, Type.GetType("System.String"));
                dt.Columns.Add(dc);
                dc = new DataColumn(columnPublish, Type.GetType("System.String"));
                dt.Columns.Add(dc);
                dt.PrimaryKey = primaryKeys;
                ds.Tables.Add(dt);
                // Return the dataset.
                return ds;
            }
            catch
            {
                return ds;
            }
            finally
            {
                ts.EndProc();
            }
        }

        /// <summary>
        /// Create a new row in the error config table within a
        /// dataset.
        /// </summary>
        /// <param name="ds">The dataset with the error config table in.</param>
        /// <returns>A new datarow.</returns>
        private DataRow CreateNewDataRowInErrorConfigTable(
            DataSet ds
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.CreateNewDataRowInErrorConfigTable");
                return ds.Tables[errorConfigTable].NewRow();
            }
            catch
            {
                return null;
            }
            finally
            {
                ts.EndProc();
            }
        }

        /// <summary>
        /// Find a row in the Error.config dataset table 'action' for
        /// a particular actor, category and page. If a row can't be
        /// found, then the default entry for this actor and category
        /// will be found. If there is no row here, then the default
        /// entry for the actor will be found.
        /// </summary>
        /// <param name="ds">The dataset containing the 'action' table i.e.
        /// the dataset with the loaded Error.config file.</param>
        /// <param name="actor">The actor to search for.</param>
        /// <param name="category">The category to search for.</param>
        /// <param name="pageName">The name of the page to search for.</param>
        /// <param name="name">The name of the error.</param>
        /// <param name="redirectUrl">The name of redirect URL obtained from a located row.</param>
        /// <param name="resourceIndex">The resource index obtained from a located row.</param>
        /// <param name="publish">Whether the error should be published.</param>
        /// <returns></returns>
        private bool FindRowInErrorConfigDataTable(
            DataSet ds,
            string actor,
            string category,
            string pageName,
            string name,
            out string redirectUrl,
            out string resourceIndex,
            out bool publish
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            redirectUrl = null;
            resourceIndex = null;
            publish = false;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.FindRowInErrorConfigDataTable");
                // Attempt to find the required row.
                object[] findTheseValues = new object[4];
                findTheseValues[0] = actor;
                findTheseValues[1] = category;
                findTheseValues[2] = name;
                findTheseValues[3] = pageName;
                trace.WriteDebug("Looking for actor='" + actor + "', category='" +
                    category + "', pagename='" + pageName + "' and name='" + name + "'");
                if (this.FindRow(ds, findTheseValues, out redirectUrl, out resourceIndex, out publish))
                    // A match was found, so return.
                    return true;
                // A match wasn't found, so now try the actor, category and name
                // with pagename as default.
                findTheseValues = new object[4];
                findTheseValues[0] = actor;
                findTheseValues[1] = category;
                findTheseValues[2] = name;
                findTheseValues[3] = "default";
                trace.WriteDebug("Looking for actor='" + actor + "', category='" +
                    category + "', pagename='" + pageName + "' and name='" + name + "'");
                if (this.FindRow(ds, findTheseValues, out redirectUrl, out resourceIndex, out publish))
                    // A match was found, so return.
                    return true;
                // A match wasn't found here either, so now try just the
                // actor and category with others as default.
                findTheseValues = new object[4];
                findTheseValues[0] = actor;
                findTheseValues[1] = category;
                findTheseValues[2] = "default";
                findTheseValues[3] = "default";
                trace.WriteDebug("Looking for actor='" + actor + "', category='" +
                    category + "', pagename='" + pageName + "' and name='" + name + "'");
                if (this.FindRow(ds, findTheseValues, out redirectUrl, out resourceIndex, out publish))
                    // A match was found, so return.
                    return true;
                // No match was found, so try just actor with others as default.
                findTheseValues = new object[4];
                findTheseValues[0] = actor;
                findTheseValues[1] = "default";
                findTheseValues[2] = "default";
                findTheseValues[3] = "default";
                trace.WriteDebug("Looking for actor='" + actor + "', category='" +
                    category + "', pagename='" + pageName + "' and name='" + name + "'");
                if (this.FindRow(ds, findTheseValues, out redirectUrl, out resourceIndex, out publish))
                    // A match was found, so return.
                    return true;
                // No match was found, so try all defaults.
                findTheseValues = new object[4];
                findTheseValues[0] = "default";
                findTheseValues[1] = "default";
                findTheseValues[2] = "default";
                findTheseValues[3] = "default";
                trace.WriteDebug("Looking for actor='" + actor + "', category='" +
                    category + "', pagename='" + pageName + "' and name='" + name + "'");
                if (this.FindRow(ds, findTheseValues, out redirectUrl, out resourceIndex, out publish))
                    // A match was found, so return.
                    return true;
                // No match found, so return false.
                return false;
            }
            catch (Exception ex)
            {
                trace.WriteInfo("Problem finding row in Error.config table because" + ex.Message);
                return false;
            }
            finally
            {
                ts.EndProc();
            }
        }

        /// <summary>
        /// Find a row in the Error.config table
        /// </summary>
        /// <param name="ds">The dataset containing the Error.config table.</param>
        /// <param name="findTheseValues">An object containing the search criteria.</param>
        /// <param name="redirectUrl">A string containing the redirect URL, if there is one.</param>
        /// <param name="resourceIndex">A string containing the resource index, if there is one.</param>
        /// <param name="publish">A bool containing whether this error should be published.</param>
        /// <returns>True if a row was found.</returns>
        private bool FindRow(
            DataSet ds,
            object[] findTheseValues,
            out string redirectUrl,
            out string resourceIndex,
            out bool publish
            )
        {
            // Do the tracing stuff.
            ITraceState ts = null;
            redirectUrl = null;
            resourceIndex = null;
            publish = false;
            try
            {
                // Do the tracing stuff.
                ts = trace.StartProc("WebException.FindRow");
                // Instantiate a data row using the supplied values.
                DataRow dr = ds.Tables[errorConfigTable].Rows.Find(findTheseValues);
                // Check to see if a match has been found.
                if (dr != null)
                {
                    // Yes a match has been found, so set the output
                    // parameters and return.
                    try { redirectUrl = dr[columnRedirectUrl].ToString(); }
                    catch { }
                    try { resourceIndex = dr[columnResourceIndex].ToString(); }
                    catch { }
                    try { publish = bool.Parse(dr[columnPublish].ToString()); }
                    catch { }
                    trace.WriteDebug("Match found, obtained redirecturl='" + redirectUrl +
                        "', resourceindex='" + resourceIndex + "' and publish='" + publish.ToString() + "'");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                trace.WriteInfo("Problem finding row in Error.config table because" + ex.Message);
                return false;
            }
            finally
            {
                ts.EndProc();
            }
        }
        #endregion

        #endregion
    }
}
