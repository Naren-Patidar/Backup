#region Using
//using Microsoft.ApplicationBlocks.ExceptionManagement;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Xml;
#endregion

namespace Fujitsu.eCrm.Generic.SharedUtils
{
    #region Header
    /// <department>Fujitsu, e-Innovation, eCRM</department>
    /// <copyright>(c) Fujitsu Consulting, 2002</copyright>
    /// 
    /// <summary>
    /// The Web.Config file should contain the following sections:
    /// 
    ///	<appSettings>
    ///	   <add key="trace_directory" value="c:\Trace\QuoteServer"/>
    ///	</appSettings>
    /// 
    ///  ... and ...
    ///
    ///  <system.diagnostics>
    ///		<switches>
    ///			<add name="QuoteService" value="3" />
    ///		</switches>
    ///	</system.diagnostics>
    ///
    /// where name is the tracename specified in the constructor and
    /// value is "1" for error messages only, "2" for errors and warnings,
    /// "3" gives more detailed error information, and "4" gives verbose 
    /// trace information
    ///
    ///
    /// NOTES
    ///
    /// PERFORMANCE
    /// The following timings were found during tests on my laptop (PIII 700MHz).
    ///		Time 1 (dummy loop) = 0.010 uS
    ///		Time 2 (Checking TraceLevel.TraceInfo) = 0.070 uS
    ///		Time 3 (WriteIf) = 0.854 uS
    ///		Time 4 (WriteInfo) = 0.867 uS
    ///		Time 5 (Queuing data) 15uS to write the data to the message queue
    ///     Time 6 (Writing data from queue to file) 600us - Runs asynchronously as a 
    ///				background thread.
    ///				
    ///  For best performance, check the trace flag, rather than calling the
    ///  WriteInfo and similar routines.
    ///	i.e. 
    ///		if (_trace.traceLevel.TraceInfo)
    ///			_trace.Write("Entering procedure GetCustomerName");
    ///
    ///
    /// WHY NOT USE .NET BUILT-IN TRACING?
    ///
    /// A number of problems were encountered when trying to use .NET built in tracing 
    /// with web services:
    ///
    /// 1. Before writing to the NT Event log, it is incessary to create the source.
    ///    This can be created manually with CreateEventSource or, if it doesnt exist,
    ///    it will be automatically created by EventLogTraceListener. Unfortunately,
    ///    WebServices by default don't have permissions to write to the registry.
    ///    Whilst this could be fixed by changing the security settings, this is a bad idea.
    ///    One way round this is to create the entries by creating an install class and
    ///    doing a proper install of the application, instead of an xcopy.
    ///
    /// 2. There sometimes seems to be duplicated entries in the event log. It is normal
    ///    for multiple instances of webclasses to exist, and this may be responsible.
    ///
    /// 3. Can't get file tracing to work properly - nothing seems to get written to the
    ///    log file, and after several runs, an error occurs stating the file is in use.
    ///
    /// As a result of these problems, It was thought to be less hassle if we produced
    /// our own tracing class that duplicates most of the .NET trace class functions.
    ///
    /// 
    /// MULTITHREADING
    /// 
    /// Multithreading is used to reduce the impact of tracing on the user applications.
    /// Messages to be written to the trace log are stored in a temporary message buffer
    /// so that control can be returned to the user as soon as possible (around 15uS).
    /// Should the buffer become full (i.e. trace data is generated faster than it can be
    /// written to the file), the extra data will be discarded and a warning message
    /// added to the trace log.
    /// 
    /// Each trace file has an associated thread, that writes all queued messages
    /// to the file. The trace write thread runs every two seconds.
    ///
    /// There may be more than one instance of the trace class writing to the same file
    /// (in fact .Nets garbage collection means that there are usually many old instances
    /// left lying about.) To limit the number of threads running, helper threads
    /// and message queues for the same file are shared between instances using 
    /// the global static variables 'htWriterThreads' and 'htMessageQueues'.
    /// </summary>
    /// <development> 
    ///    <version number="1.10" day="16" month="01" year="2003">
    ///			<developer>Tom Bedwell</developer>
    ///			<checker>Steve Lang</checker>
    ///			<work_packet>WP/Barcelona/046</work_packet>
    ///			<description>Namespaces conform to standards</description>
    ///	</version>
    ///		<version number="1.05" day="13" month="01" year="2003">
    ///			<developer>Mark Hart</developer>
    ///			<checker>Tom Bedwell</checker>
    ///			<work_packet>WP/Barcelona/045</work_packet>
    ///			<description>Changed disposal of Trace class.</description>
    ///		</version>
    ///		<version number="1.05" day="13" month="01" year="2003">
    ///			<developer>Mark Hart</developer>
    ///			<checker>Tom Bedwell</checker>
    ///			<work_packet>WP/Barcelona/045</work_packet>
    ///			<description>Changed disposal of Trace class.</description>
    ///		</version>
    ///		<version number="1.04" day="25" month="09" year="2002">
    ///			<developer>Mark Hart</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Place synchronisation lock round queue accessors.</description>
    ///		</version>
    ///		<version number="1.03" day="18" month="06" year="2002">
    ///			<developer>Gary Bleads</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Indentation reset to 0 following a call to writeError.</description>
    ///		</version>
    ///		<version number="1.02" day="13" month="06" year="2002">
    ///			<developer>Gary Bleads</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Fixed problem with indentation.</description>
    ///		</version>
    ///		<version number="1.01" day="12" month="06" year="2002">
    ///			<developer>Gary Bleads</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>
    ///				Added a write queue and seperate thread to write
    ///				the queue to each output file. Added trace level
    ///				bits, updated configuration file handling &amp;
    ///				added performance tweaks.
    ///			</description>
    ///		</version>
    ///		<version number="1.00" day="06" month="06" year="2002">
    ///			<developer>Gary Bleads</developer>
    ///			<checker></checker>
    ///			<work_packet></work_packet>
    ///			<description>Initial Implementation.</description>
    ///		</version>
    /// </development>
    #endregion
    #region Trace
    //public class Trace : IExceptionPublisher, ITrace {
    public class Trace : ITrace
    {

        #region Constants
        // Strings inserted into trace log for errors etc.
        const string TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        #endregion

        #region Static Attributes
        // Static Variables shared by all instances of this class
        // These are used so that multiple instances of the same trace log can
        // share the same message queues and helper threads.

        // Shared Writer threads, one per trace file
        private static Hashtable traceQueueTable = new Hashtable(21);
        private static bool disposed = false;
        private static string dateString = String.Empty;
        private static DateTime nextDt = new DateTime(1900, 1, 1);

        // Instance counters, record how many instances are using each trace file.
        // Used to control the creation and deletion of threads.
        //private static Hashtable htInstanceCount = new Hashtable(20);
        #endregion

        #region Attributes
        // Global variables private to each instance of the class
        private TraceQueue traceQueue;	// Current writer class
        private int indentLevel;		// text indent level 
        private TraceFlags traceLevel;	// tracing level flags
        #endregion

        #region Properties
        /// <summary>
        /// Returns the level of tracing supported by this
        /// </summary>
        public ITraceFlags TraceLevel { get { return this.traceLevel; } }

        /// <summary>
        /// Change the current procedure nesting IndentLevel.
        /// The identString is a string containing a number of space characters
        /// added to the front of each trace message.
        /// </summary>
        public int Indent
        {
            get { return this.indentLevel; }
            set
            {
                if (value > 0)
                {
                    this.indentLevel = value;
                }
            }
        }

        /// <summary>
        /// Returns the current time as a string i.e. "12:34:56 12/06/02"
        ///
        /// DateTime.Now.ToString(...) is a relatively slow statement (15uS/call),
        /// and was having a significant impact on trace performance. 
        ///
        /// This routine provides a faster way of getting the time string. 
        /// The first time it is called, it keeps a local copy of the time string. 
        /// The next time, it returns the local copy, unless the time has changed
        /// by more than one second.
        /// </summary>
        private static string DateString
        {
            get
            {
                if (DateTime.Now.CompareTo(Trace.nextDt) > 0)
                {
                    Trace.nextDt = DateTime.Now.AddSeconds(1);
                    Trace.dateString = DateTime.Now.ToString(TIME_FORMAT);
                }
                return Trace.dateString;
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Constructor, default to trace file "log"
        /// </summary>
        public Trace()
        {
            Initialise("log");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tracename">A string containing the name of the application. This is used 
        /// to create the trace file in the output directory and to give the trace 
        /// switch a unique identifier and description</param>
        public Trace(string tracename)
        {
            Initialise(tracename);
        }

        private void Initialise(string tracename)
        {

            try
            {

                if (Thread.CurrentThread.Name == null)
                {
                    Thread.CurrentThread.Name = Guid.NewGuid().ToString();
                }

                // Set the trace level. It Defaults to "ALL"
                string traceLevelStr = (string)ConfigurationSettings.AppSettings[tracename + "TraceLevel"];
                if (traceLevelStr == null)
                {
                    traceLevelStr = (string)ConfigurationSettings.AppSettings["traceLevel"];
                }
                if (traceLevelStr == null)
                {
                    traceLevelStr = "ALL";
                }
                this.traceLevel = new TraceFlags();
                SetTraceLevel(traceLevelStr);

                // Set the trace indentation
                this.indentLevel = 0;

                lock (typeof(Trace))
                {
                    if (!disposed)
                    {
                        // Is an instance of this trace log already running?
                        if (traceQueueTable.ContainsKey(tracename))
                        {
                            // Get the existing writer thread.
                            this.traceQueue = (TraceQueue)traceQueueTable[tracename];
                        }
                        else
                        {
                            // No. Create a new writer thread.					

                            // Get the name of the trace directory from the config file
                            string traceDirectory = (string)ConfigurationSettings.AppSettings[tracename + "TraceDirectory"];
                            if (traceDirectory == null)
                            {
                                traceDirectory = (string)ConfigurationSettings.AppSettings["traceDirectory"];
                            }

                            // Default to c:\ if no directory name is specified
                            if (traceDirectory == null)
                            {
                                traceDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            }

                            // add a "\" to the end of the directory name if its not already there.
                            if ((!traceDirectory.EndsWith(@"\")) && (!traceDirectory.EndsWith(@"/")))
                            {
                                traceDirectory += @"\";
                            }

                            this.traceQueue = new TraceQueue(traceDirectory, tracename);

                            // Store a pointer to the thread and set the instance count to 1
                            traceQueueTable.Add(tracename, this.traceQueue);
                        }
                    }
                }
            }
            catch
            {
                // Suppress all exceptions, allow the code to continue in silence
            }
        }
        #endregion

        #region Dispose
        /// <summary>
        /// The Trace object is finished with for good. Remove all writers
        /// from the hashtable and close.
        /// </summary>
        public static void Close()
        {

            // Lock Trace Class to prevent any further trace queues being
            // creating and added to traceQueueTable
            lock (typeof(Trace))
            {
                disposed = true;

                // Loop round all entries in the static hashtable and close
                // and remove writers.
                foreach (string key in traceQueueTable.Keys)
                {
                    // Close the writer stream.
                    ((TraceQueue)traceQueueTable[key]).Dispose();
                }
                // Now remove all elements from the hashtable.
                traceQueueTable.Clear();
                traceQueueTable = null;
            }
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Parses the traceLevel string (as read from the applications web.config file)
        /// and sets the relevant bits in the traceLevel global variable.
        /// The tracelevel string comprises of one or more fields, seperated by commas, ampersand or + 
        /// characters. Fields are processed from left to right. Each field may contain:-
        ///   "ALL" - Sets all flags
        ///   "{flagname}" - Sets the specified flag, i.e. "procedure"
        ///   "NO{flagname}" - Sets the specified flag, i.e. "noprocedure"
        ///   "{hexvalue}" - Sets the flags according to the hex value i.e. "FFFA"
        /// </summary>
        ///
        /// <param name="traceLevelStr">
        /// The desired trace level
        /// </param>
        public void SetTraceLevel(string traceLevelStr)
        {
            // Get an array containing the name of each trace flag.
            Type t = typeof(TraceFlags);
            PropertyInfo[] properties = t.GetProperties();

            // Initially clear all trace flags
            foreach (PropertyInfo pi in properties)
            {
                pi.SetValue(traceLevel, false, null);
            }

            // tokenize the trace level string by splitting on comma, + or & characters.
            // Convert to upper case and throw away any spaces.
            string[] tokens = traceLevelStr.ToUpper().Replace(" ", String.Empty).Split(new char[] { ',', '&', '+' });

            foreach (string token in tokens)
            {
                if ((token == null) || (token.Length == 0))
                {
                    // skip empty fields
                }
                else if ((token == "ALL") || (token == "EVERYTHING"))
                {
                    // Turn on all trace levels
                    foreach (PropertyInfo pi in properties)
                        pi.SetValue(traceLevel, true, null);
                }
                else if ((token == "NONE") || (token == "NOTHING"))
                {
                    // Turn on all trace levels
                    foreach (PropertyInfo pi in properties)
                    {
                        pi.SetValue(traceLevel, false, null);
                    }
                }
                else
                {
                    // Is the supplied value the name of a trace level, i.e. "ERROR"
                    // or "NOERROR"
                    string tokenName;
                    bool tokenValue;
                    bool found = false;
                    if (token.StartsWith("NO"))
                    {
                        // Set the trace flag off
                        tokenName = token.Substring(2);
                        tokenValue = false;
                    }
                    else
                    {
                        // turn the trace flag on
                        tokenName = token;
                        tokenValue = true;
                    }

                    // Set the appropriate flag in the traceLevel class
                    foreach (PropertyInfo pi in properties)
                    {
                        if (pi.Name.ToUpper() == tokenName)
                        {
                            pi.SetValue(traceLevel, tokenValue, null);
                            found = true;
                            break;
                        }
                    }

                    // The flag was not found. Was the supplied value a hex number?
                    if (!found)
                    {
                        try
                        {
                            // Parse the HEX string into an unsigned 32-bit int.
                            uint bitmap = uint.Parse(token, System.Globalization.NumberStyles.AllowHexSpecifier);

                            // mask to select 1 bit from the hex value
                            uint mask = 1;
                            for (int i = properties.GetUpperBound(0); i >= properties.GetLowerBound(0); i--)
                            {
                                if ((bitmap & mask) == 0)
                                { // bitwise AND 
                                    properties[i].SetValue(traceLevel, false, null);
                                }
                                else
                                {
                                    properties[i].SetValue(traceLevel, true, null);
                                }
                                mask <<= 1;	// Shift mask 1 place left
                            }
                        }
                        catch
                        {
                            // Unknown trace message. Build a helpfull error message 
                            // containing a list of all legal value.
                            StringBuilder msg = new StringBuilder("Unrecognised trace level in web.config file : '", 1000);
                            msg.Append(token);
                            msg.Append("'.\nAllowed values are : 'ALL' 'NONE'");
                            foreach (PropertyInfo pi in properties)
                            {
                                msg.Append(" '");
                                msg.Append(pi.Name.ToUpper());
                                msg.Append("' 'NO");
                                msg.Append(pi.Name.ToUpper());
                                msg.Append("' ");
                            }
                            msg.Append("or a Hexadecimal number");
                            throw new ApplicationException(msg.ToString());
                        }
                    }
                }
            }
        }
        #endregion

        #region Write
        /// <summary>
        /// Writes a message to the trace log
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="type">The message type</param>
        //private void Write(string msg, string type) 
        //{
        //    if (this.traceQueue != null)
        //    {
        //        this.traceQueue.Enqueue(String.Format(
        //            "<message thread=\"{0}\" level=\"{1}\" time=\"{2}\" type=\"{3}\">{4}</message>",
        //            Thread.CurrentThread.Name,
        //            this.Indent,
        //            Trace.DateString,
        //            //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff"),
        //            type,
        //            Escape(msg)));
        //    }
        private void Write(string msg, string type)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Message = msg + ":" + type;
            Logger.Write(logEntry);
        }


        private string Escape(string orginalMsg)
        {
            string msg = orginalMsg.Replace("&", "&amp;");
            msg = msg.Replace("<", "&lt;");
            msg = msg.Replace(">", "&gt;");
            msg = msg.Replace("'", "&apos;");
            msg = msg.Replace("\"", "&quot;");
            return msg;
        }

        /// <summary>
        /// Write details of new thread, the current thread id the parent thread
        /// </summary>
        /// <param name="childThreadName"></param>
        public void WriteThread(string childThreadName)
        {
            try
            {
                if (this.traceLevel.Debug)
                {
                    if (this.traceQueue != null)
                    {
                        this.traceQueue.Enqueue(String.Format(
                            "<thread name=\"{0}\" parent=\"{1}\"/>",
                            childThreadName,
                            Thread.CurrentThread.Name));
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Called by Microsoft.ApplicationBlock.ExceptionManager, to publish details of an exception
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <param name="additionalInfo">The context of the exception</param>
        /// <param name="configSettings">Configuration details for this publisher</param>
        void HandleException(Exception exception, NameValueCollection additionalInfo, NameValueCollection configSettings)
        {

            int oldIndent = this.Indent;

            this.WriteError("Exception");
            this.Indent++;

            // Display Additional Information, i.e. the Context of the exception
            if (additionalInfo != null)
            {
                this.WriteError("General Information");
                this.Indent++;
                foreach (string i in additionalInfo)
                {
                    this.WriteError(i + ": " + additionalInfo.Get(i));
                }
                this.Indent--;
            }

            Exception currentException = exception;	// Temp variable to hold InnerException object during the loop.
            do
            {
                // Write title information for the exception object.
                this.WriteError("Exception Information");
                this.Indent++;
                // Loop through the public properties of the exception object and record their value.
                PropertyInfo[] aryPublicProperties = currentException.GetType().GetProperties();
                NameValueCollection currentAdditionalInfo;
                foreach (PropertyInfo p in aryPublicProperties)
                {
                    // Do not log information for the InnerException or StackTrace. This information is 
                    // captured later in the process.
                    if (p.Name != "InnerException" && p.Name != "StackTrace")
                    {
                        if (p.GetValue(currentException, null) == null)
                        {
                            this.WriteError(p.Name + ": NULL");
                        }
                        //code convergence changes
                        //else if (p.Name == "AdditionalInformation" && currentException is BaseApplicationException)
                        else if (p.Name == "AdditionalInformation")
                        {
                            // Verify the collection is not null.
                            if (p.GetValue(currentException, null) != null)
                            {
                                // Cast the collection into a local variable.
                                currentAdditionalInfo = (NameValueCollection)p.GetValue(currentException, null);
                                // Check if the collection contains values.
                                if (currentAdditionalInfo.Count > 0)
                                {
                                    // Loop through the collection adding the information to the string builder.
                                    for (int i = 0; i < currentAdditionalInfo.Count; i++)
                                    {
                                        this.WriteError(currentAdditionalInfo.GetKey(i) + ": " + currentAdditionalInfo[i]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.WriteError(p.Name + ": " + p.GetValue(currentException, null));
                        }
                    }
                }

                // Record the StackTrace with separate label.
                if (currentException.StackTrace != null)
                {
                    this.WriteError("StackTrace Information");
                    this.Indent++;
                    this.WriteError(currentException.StackTrace);
                    this.Indent--;
                }

                // Reset the temp exception object and iterate the counter.
                currentException = currentException.InnerException;
            } while (currentException != null);

            this.indentLevel = oldIndent;
        }

        /// <summary>
        /// Writes an error message to the trace log if the 
        /// trace level is error
        /// </summary>
        /// <param name="msg">The error message</param>
        public void WriteError(string msg)
        {
            try
            {
                if (this.traceLevel.Error)
                {
                    this.Write(msg, "Error");
                }

                // Reset the indent following an error, cos we'll probably crash out
                // of any nested procedures without calling endproc().
                this.Indent = 0;
            }
            catch { }
        }

        /// <summary>
        /// Writes an warning message to the trace log if the 
        /// trace level is warning
        /// </summary>
        /// <param name="msg">The warning message</param>
        public void WriteWarning(string msg)
        {
            try
            {
                if (this.traceLevel.Warning)
                {
                    this.Write(msg, "Warning");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is debug
        /// </summary>
        /// <param name="msg">The message</param>
        public void WriteDebug(string msg)
        {
            try
            {
                if (this.traceLevel.Debug)
                {
                    this.Write(msg, "Debug");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is sql
        /// </summary>
        /// <param name="msg">The message</param>
        public void WriteSql(string msg)
        {
            try
            {
                if (this.traceLevel.Sql)
                {
                    this.Write(msg, "Sql");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is info
        /// </summary>
        /// <param name="msg">The message</param>
        public void WriteInfo(string msg)
        {
            try
            {
                if (this.traceLevel.Info)
                {
                    this.Write(msg, "Info");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is XML
        /// </summary>
        /// <param name="msg">The message</param>
        public void WriteXML(string msg)
        {
            try
            {
                if (this.traceLevel.Xml)
                {
                    this.Write(msg, "Xml");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is Request
        /// </summary>
        /// <param name="msg">The message</param>
        public void WriteRequest(string msg)
        {
            try
            {
                if (this.traceLevel.Request)
                {
                    this.Write(msg, "Request");
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes a plain message to the trace log if the 
        /// trace level is Procedure
        /// </summary>
        /// <param name="msg">The message</param>
        /// <param name="type">Start or End</param>
        public void WriteProcedure(string msg, string type)
        {
            try
            {
                if (this.traceLevel.Procedure)
                {
                    this.Write(msg, type);
                }
            }
            catch { }
        }

        /// <summary>
        /// Put a call to StartProc at the begining of every procedure 
        /// to write the name of the procedure to the log with auto indent. 
        /// and start the procedure timer. See also EndProc.
        /// </summary>
        /// <param name="procedureName">The name of the procedure</param>
        /// <returns>The state of tracing the procedure</returns>
        public ITraceState StartProc(string procedureName)
        {
            return new TraceState(this, procedureName);
        }

        /// <summary>
        /// Put a call to EndProc at the end of each procedure or function.
        /// Dont forget to add before any "return"s inside the function as well.
        /// Use with StartProc.
        /// </summary>
        /// <param name="trState"></param>
        public void EndProc(ITraceState trState)
        {
            try
            {
                trState.EndProc();
            }
            catch { }
        }
        #endregion
    }
    #endregion

    #region Class TraceState
    /// <summary>
    /// Encapulate a method
    /// </summary>
    public class TraceState : ITraceState
    {

        #region Attributes
        private string procName;
        private HiResTimer timer;
        private ITrace trace;
        #endregion

        #region Constructor
        /// <summary>
        /// Start tracing a method
        /// </summary>
        /// <param name="trace">The trace destination</param>
        /// <param name="procName">The method being traced</param>
        public TraceState(ITrace trace, string procName)
        {
            try
            {
                this.timer = new HiResTimer();
                this.timer.Start();
                this.trace = trace;
                this.procName = procName;
                this.trace.WriteProcedure("Entered Procedure " + this.procName, "Start");
                this.trace.Indent++;
            }
            catch
            {
                // Suppress all exceptions, allow the code to continue in silence
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// End tracing a method
        /// </summary>
        public void EndProc()
        {
            try
            {
                this.timer.Stop();
                this.trace.WriteProcedure("Exited Procedure " + this.procName + ". Time=" + this.timer.ElapsedMilliseconds.ToString("F3") + " ms", "End");
                this.trace.Indent--;  // do this after writing the message
            }
            catch
            {
                // Suppress all exceptions, allow the code to continue in silence
            }
        }
        #endregion
    }
    #endregion

    #region Class TraceFlags
    // TracingLevel
    //
    // A set of flags which can be used to set or view the current
    // tracing level. They are normally loaded from the web.config file, but
    // can also be read and written by the application.
    //
    // You can add new fields to this class without having to modify the rest
    // of the trace class. Trace class uses C# reflection to automatically 
    // discover the contents of the TracingLevel class. There can be up to
    // 32 tracing level flags.
    //
    // The default tracing level is Error & Warning.
    //
    // The trace level can be set in the application's Web.Config file by adding the
    // following :-
    //
    // <appSettings>
    //		<add key="traceDirectory" value="c:\Trace\"/>
    //		<add key="traceLevel" 
    //			value="noSQL,noXML,Error,Warning,Info,noDebug,noProcedure,Request"/>
    // </appSettings>
    //
    // traceLevel can be either "ALL", "NONE", the name of any of the tracing level
    // fields, 'NO' + the field name or a Hexadecimal number, such as FFA0.
    // Fields can be seperated by comma, & or + characters.
    //
    // We have to use a class, 'cos reflection doesn't work properly with a struct.

    /// <summary>
    /// A set of flags which can be used to set or view the current
    /// tracing level. They are normally loaded from the web.config file, but
    /// can also be read and written by the application. See the documentation
    /// for details of the web.Config file
    /// </summary>
    public class TraceFlags : ITraceFlags
    {

        private bool sql = true;
        private bool xml = true;
        private bool error = true;
        private bool warning = true;
        private bool info = true;
        private bool debug = true;
        private bool procedure = true;
        private bool request = true;

        /// <summary>Trace SQL</summary>
        public bool Sql
        {
            get { return this.sql; }
            set { this.sql = value; }
        }

        /// <summary>Trace XML</summary>
        public bool Xml
        {
            get { return this.xml; }
            set { this.xml = value; }
        }

        /// <summary>Trace Errors</summary>
        public bool Error
        {
            get { return this.error; }
            set { this.error = value; }
        }

        /// <summary>Trace Warnings</summary>
        public bool Warning
        {
            get { return this.warning; }
            set { this.warning = value; }
        }

        /// <summary>Trace Info</summary>
        public bool Info
        {
            get { return this.info; }
            set { this.info = value; }
        }

        /// <summary>Trace Debug Comments</summary>
        public bool Debug
        {
            get { return this.debug; }
            set { this.debug = value; }
        }

        /// <summary>Trace Procedures</summary>
        public bool Procedure
        {
            get { return this.procedure; }
            set { this.procedure = value; }
        }

        /// <summary>Trace Requests</summary>
        public bool Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

    }
    #endregion

    #region Class TraceQueue
    /// <summary>
    ///  This is the "Worker" thread that writes the contents of the
    ///  message queue to the trace file.
    ///
    ///  There should be one thread running per trace file.
    ///  
    ///  The message queue has to be locked because multiple threads access the queue (and the disposed flag)
    ///  The file stream does not have to be locked because only one thread accesses it, the one running HandleThread()
    /// </summary>
    internal class TraceQueue : Queue, IDisposable
    {

        #region Constants
        private const ThreadPriority WRITER_THREAD_PRIORITY = ThreadPriority.BelowNormal;
        private const int THREAD_SLEEP_MILLISECONDS = 2500; // 2.5 secs
        private const int QUEUE_SIZE = 5000;
        private const int QUEUE_BLOCK_SIZE = QUEUE_SIZE - 10;
        private const int QUEUE_UNBLOCK_SIZE = QUEUE_SIZE - 25;
        private const int TRACE_FILE_MB = 50;
        private const long MAX_FILE_SIZE = TRACE_FILE_MB * 1024L * 1024L;		// max filesize = 50MB
        #endregion

        #region Attributes
        // access to read/write attributes need to be via locks
        private DateTime traceFileDate;
        private FileStream traceStream;
        private StreamWriter fileWriter;
        private bool queueBlock;

        private readonly string traceDirectory;
        private readonly string traceName;
        private readonly Thread dequeueThread;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="traceDirectory">The NT path of the directory to hold the trace logs</param>
        /// <param name="traceName">The name of the trace log, as supplied to the Trace constructor</param>
        internal TraceQueue(string traceDirectory, string traceName)
            : base(QUEUE_SIZE)
        {

            this.traceDirectory = traceDirectory;
            this.traceName = traceName;

            // Setup file to write to.  Do here and not in dequeue thread as the dequeue thread
            // might be aborted before it has had chance to open up a file
            this.OpenFile(false);

            // Setup thread to read from queue and write to file
            this.dequeueThread = new Thread(new ThreadStart(this.HandleThread));
            this.dequeueThread.Name = "Barcelona Trace " + traceName;
            this.dequeueThread.Priority = WRITER_THREAD_PRIORITY;
            this.dequeueThread.Start();

        }
        #endregion

        #region Dispose
        /// <summary>
        /// Destructor. Called by the Garbage Collector because
        /// the class is derived from IDisposable.
        /// 
        /// See Wrox Professional C# Second Edition Pages 142-146
        /// </summary>
        ~TraceQueue()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implementation of IDisposable.Dispose()
        /// User call to tidily shut down the tracing thread.
        /// It is not essential to call this, as the Garbage Collector
        /// will eventually tidy up.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // No longer any need for the garbage collector to call the destructor
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Final read from queue and write to stream
        /// Close down stream
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // Lock to ensure no conflicting enqueues or dequeues
            lock (this)
            {
                dequeueThread.Abort();
                this.DequeueAllToFile();
                this.CloseFile();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a string to the trace message queue for later writing to the trace file.
        /// If the trace file is full, a warning message will be added to the log and
        /// future messages discarded until the queue has some more space.
        /// Only add a string to the queue if there is a trace log, i.e. not in silent mode
        /// </summary>
        /// <param name="s">The string to write to the trace log</param>
        internal void Enqueue(string s)
        {
            try
            {
                // Lock the message queue to ensure no conflict with the dequeue process
                // in the HandleThread method, or a dispose method
                lock (this)
                {
                    if (this.queueBlock)
                    {
                        // The queue was full. Has it now got room for some more entries?
                        if (this.Count < (QUEUE_UNBLOCK_SIZE))
                        {
                            // Yes. We've now got some room
                            this.queueBlock = false;
                        }
                    }
                    else if (this.Count >= (QUEUE_BLOCK_SIZE))
                    {
                        // Remember that we're now full so we don't write the 
                        // overflow message again
                        this.queueBlock = true;
                    }

                    if (!this.queueBlock)
                    {
                        base.Enqueue(s);
                    }
                }
            }
            catch
            {
                // Suppress all exceptions
            }
        }

        /// <summary>
        /// Remove all strings from the memory queue and write them to a trace file
        /// </summary>
        internal void DequeueAllToFile()
        {
            try
            {
                // Check there is a file to dequeue to
                if (this.fileWriter != null)
                {
                    // Output any messages in the message queue
                    while (this.Count > 0)
                    {
                        // We use peek & dequeue seperately to ensure we don't
                        // loose a message if the file is in use & writeline fails.
                        string msg = (string)this.Peek();
                        this.fileWriter.WriteLine(msg);
                        this.Dequeue();
                    }
                }
            }
            catch
            {
                // Suppress all exceptions
            }
        }

        /// <summary>
        /// Runs as a background thread. 
        /// Every second or two it checks if there are any items in the messageQueue,
        /// and if so, writes them all to the trace log.
        ///
        /// If an error occurs when writing to the file (i.e. it's in use elsewhere),
        /// it tries again later.
        /// </summary>
        private void HandleThread()
        {

            try
            {

                while (true)
                {

                    // Lock this to make sure there is no conflict from an Enqueue or a Dispose
                    lock (this)
                    {

                        if (this.Count > 0)
                        {
                            // Check that the file is not too large, and that the date is still
                            // the same when we created the file.
                            if (this.fileWriter == null)
                            {
                                this.OpenFile(false);
                            }
                            else
                            {
                                // If the file is too big or the date has changed since the 
                                // last file was created, create a new file.
                                if ((this.traceStream.Position >= MAX_FILE_SIZE) || (this.traceFileDate != DateTime.Now.Date))
                                {
                                    // Close the current stream
                                    this.CloseFile();
                                    // Open a new stream
                                    this.OpenFile(true);
                                }
                            }
                            this.DequeueAllToFile();
                        }
                    }

                    // Sleep, let some writes occur
                    Thread.Sleep(THREAD_SLEEP_MILLISECONDS);
                }
            }
            catch
            {
                // Suppress all exceptions
            }
        }


        /// <summary>
        /// Creates the full trace path name, based on the tracename, the 
        /// trace directory, the date and the lowest free serial number.
        /// </summary>
        /// <param name="createNewFile">If true, always creates a new file with the next</param>
        private void OpenFile(bool createNewFile)
        {

            try
            {

                this.traceFileDate = DateTime.Now.Date;
                string traceDateString = this.traceFileDate.ToString("yyyyMMdd");

                // Find the existing file with the highest serial number (path1), and the 
                // next vacant file (path2).

                if (!Directory.Exists(this.traceDirectory))
                {
                    Directory.CreateDirectory(this.traceDirectory);
                }

                string[] existingFiles = Directory.GetFiles(this.traceDirectory, this.traceName + "_" + traceDateString + "_*");

                int nextNumber = 1;
                string traceFilePath = null;

                if (existingFiles.Length > 0)
                {
                    // File(s) already exist. Get the file with the highest serial number
                    Array.Sort(existingFiles);
                    traceFilePath = existingFiles[existingFiles.GetUpperBound(0)];
                    string serial = traceFilePath.Substring(traceFilePath.Length - 8, 4);
                    nextNumber = int.Parse(serial);

                    // Is the old file usable, if not then create a new file
                    if (!createNewFile)
                    {
                        // Get the file's size, is it too big
                        FileInfo fi = new FileInfo(traceFilePath);
                        if (fi.Length >= MAX_FILE_SIZE)
                        {
                            createNewFile = true;
                        }
                    }
                    // If creating new file then must have a new number (1 higher than the previous high number)
                    if (createNewFile)
                    {
                        nextNumber++;
                    }
                }
                else
                {
                    createNewFile = true;
                }

                if (createNewFile)
                {
                    traceFilePath = this.traceDirectory + this.traceName + "_" + traceDateString + "_" + nextNumber.ToString("0000") + ".xml";
                }

                // Open a File Stream, with Read Access and allow others to have Read/Write Access
                this.traceStream = new FileStream(traceFilePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                // Open a writer to the file stream
                this.fileWriter = new StreamWriter(this.traceStream);
                this.fileWriter.AutoFlush = true;

            }
            catch
            {
                // Suppress all exceptions
                this.fileWriter = null;
            }
        }

        /// <summary>
        /// Try opening a file stream
        /// </summary>
        private void CloseFile()
        {
            try
            {
                if (this.fileWriter != null)
                {
                    this.fileWriter.Close();
                }
                // the stream is closed when the writer is closed
            }
            catch
            {
                // Suppress all exceptions
            }
            finally
            {
                this.fileWriter = null;
            }
        }
        #endregion
    }
    #endregion
}