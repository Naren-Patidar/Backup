using System;

namespace Fujitsu.eCrm.Generic.SharedUtils {

	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// <development>
	/// 	<version number="1.00" day="14" month="01" year="2003">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet>WP/Barcelona/042</work_packet>
	///			<description>SharedUtil's Trace Interface definition </description>
	///		</version>
	/// </development>
	/// <summary>
	///	Trace Interface definition
	/// </summary>
	#endregion

	public interface ITraceFlags {
		/// <summary>Trace SQL</summary>
		bool Sql { get; set; }
		/// <summary>Trace XML</summary>
		bool Xml { get; set; }
		/// <summary>Trace Errors</summary>
		bool Error { get; set; }
		/// <summary>Trace Warnings</summary>
		bool Warning { get; set; }
		/// <summary>Trace Info</summary>
		bool Info { get; set; }
		/// <summary>Trace Debug Comments</summary>
		bool Debug { get; set; }
		/// <summary>Trace Procedures</summary>
		bool Procedure { get; set; }
		/// <summary>Trace Requests</summary>
		bool Request { get; set; }
	}

	public interface ITraceState {
		/// <summary>
		/// End tracing a method
		/// </summary>
		void EndProc();
	}

	public interface ITrace {

		#region Properties
		/// <summary>
		/// Returns the level of tracing supported by this
		/// </summary>
		ITraceFlags TraceLevel { get; }

		/// <summary>
		/// Change the current procedure nesting IndentLevel.
		/// The identString is a string containing a number of space characters
		/// added to the front of each trace message.
		/// </summary>
		int Indent { get; set; }
		#endregion Properties

		#region Write
		/// <summary>
		/// Allows the trace flags to be changed on the fly
		/// </summary>
		/// <param name="traceLevelStr"></param>
		void SetTraceLevel(string traceLevelStr);

		/// <summary>
		/// Write details of new thread, the current thread id the parent thread
		/// </summary>
		/// <param name="childThreadName"></param>
		void WriteThread(string childThreadName);

		/// <summary>
		/// Writes an error message to the trace log if the 
		/// trace level is error
		/// </summary>
		/// <param name="msg">The error message</param>
		void WriteError(string msg);

		/// <summary>
		/// Writes an warning message to the trace log if the 
		/// trace level is warning
		/// </summary>
		/// <param name="msg">The warning message</param>
		void WriteWarning(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is debug
		/// </summary>
		/// <param name="msg">The message</param>
		void WriteDebug(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is sql
		/// </summary>
		/// <param name="msg">The message</param>
		void WriteSql(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is info
		/// </summary>
		/// <param name="msg">The message</param>
		void WriteInfo(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is XML
		/// </summary>
		/// <param name="msg">The message</param>
		void WriteXML(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is Request
		/// </summary>
		/// <param name="msg">The message</param>
		void WriteRequest(string msg);

		/// <summary>
		/// Writes a plain message to the trace log if the 
		/// trace level is Procedure
		/// </summary>
		/// <param name="msg">The message</param>
		/// <param name="type">Start or End</param>
		void WriteProcedure(string msg, string type);

		/// <summary>
		/// Put a call to StartProc at the begining of every procedure 
		/// to write the name of the procedure to the log with auto indent. 
		/// and start the procedure timer. See also EndProc.
		/// </summary>
		/// <param name="procedureName">The name of the procedure</param>
		/// <returns>The state of tracing the procedure</returns>
		ITraceState StartProc(string procedureName);

		/// <summary>
		/// Put a call to EndProc at the end of each procedure or function.
		/// Dont forget to add before any "return"s inside the function as well.
		/// Use with StartProc.
		/// </summary>
		/// <param name="trState"></param>
		void EndProc(ITraceState trState);
		#endregion
	}
}