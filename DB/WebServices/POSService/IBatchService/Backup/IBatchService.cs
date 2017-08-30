using System;
using Fujitsu.eCrm.Generic.SharedUtils;

namespace Fujitsu.eCrm.Generic.BatchService {

	#region Header
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2003</copyright>
	/// <development>
	///		<version number="1.11" day="12" month="06" year="2003">
	///			<developer>Mark Hart</developer>
	///			<checker>Stuart Forbes</checker>
	///			<work_packet>WP/Barcelona/059</work_packet>
	///			<description>Removed parameter 'culture' from the Batch method.</description>
	///		</version>
	///		<version number="1.10" day="16" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker>Steve Lang</checker>
	///			<work_packet>WP/Barcelona/046</work_packet>
	///			<description>Namespaces conform to standards</description>
	///		</version>
	/// 	<version number="1.00" day="07" month="01" year="2003">
	///			<developer>Tom Bedwell</developer>
	///			<checker></checker>
	///			<work_packet>WP/Barcelona/042</work_packet>
	///			<description>Batch Interface definition </description>
	///		</version>
	/// </development>
	/// <summary>
	///	Batch Interface definition
	/// </summary>
	#endregion	

	public delegate void Output(string message);
	/// <summary>
	/// Batch service interface
	/// </summary>
	public interface IBatchService 
	{

		/// <summary>
		/// Discover which scripts are available
		/// </summary>
		/// <param name="fileName">returned list of file names</param>
		/// <param name="resultXml">returned result</param>
		/// <returns>success</returns>
		bool BatchList(out string[] fileName, out string resultXml);

		/// <summary>
		/// run a script
		/// </summary>
		/// <param name="scriptName"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="args"></param>
		/// <param name="logFile"></param>
		/// <param name="resultXml"></param>
		/// <returns></returns>
		bool Batch(string scriptName, 
				string userName, 
				string password, 
				string[] args,
				out string logFile, 
				out string resultXml);

	}	
	/// <summary>
	/// Batch service 
	/// </summary>
	public interface ItBatchService 
		{

		/// <summary>
		/// Discover which scripts are available
		/// </summary>
		/// <param name="fileName">returned list of file names</param>
		/// <param name="resultXml">returned result</param>
		/// <returns>success</returns>
		bool BatchList(out string[] fileName, out string resultXml);

		/// <summary>
		/// run a script
		/// </summary>
		/// <param name="trace"></param>
		/// <param name="scriptName"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="args"></param>
		/// <param name="standardOutput"></param>
		/// <param name="errorOutput"></param>
		/// <param name="resultXml"></param>
		/// <returns></returns>
		bool Batch(
			ITrace trace,			
			string scriptName,
			string userName,
			string password,
			string[] args,
			Output standardOutput,
			Output errorOutput,
			out string resultXml);

		}
}