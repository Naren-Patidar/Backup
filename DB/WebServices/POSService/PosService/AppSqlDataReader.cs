using Fujitsu.eCrm.Generic.SharedUtils;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Fujitsu.eCrm.Generic.PosService {

	#region Header
	///
	/// <department>Fujitsu, e-Innovations, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2001</copyright>
	/// 
	/// <summary>
	/// Class contains SqlDataReader, SqlCommand and SqlConnection
	/// Performs standard actions using three classes
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.01" day="15" month="03" year="2002">
	///			<developer>Andy Kirk</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Add 'GetName' and 'FieldCount' methods</description>
	///		</version>
	///		<version number="1.00" day="19" month="11" year="2001">
	///			<developer>Stephen Lang</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class AppSqlDataReader {

		/// <summary>
		/// The instance's SQL connection
		/// </summary>
		private SqlConnection ownSqlConnection;

		/// <summary>
		/// The instance's SQL command
		/// </summary>
		private SqlCommand ownSqlCommand;

		/// <summary>
		/// The instance's SQL Data Reader using ownSqlCommand and ownSqlConnection
		/// </summary>
		private SqlDataReader ownSqlDataReader;

		/// <summary>
		/// Open a SqlDataReader based on the command and on the connection string
		/// </summary>
		/// <param name="cmdText">SQL Select Statement</param>
		/// <param name="connectionString">DB Connection String</param>
		public AppSqlDataReader(string cmdText, string connectionString) {
			try {
				ownSqlConnection = new SqlConnection(connectionString);
				ownSqlConnection.Open();
			} catch (Exception e) {
				CrmServiceException ce = new CrmServiceException(
					"Server",
					"SqlError",
					"ConnectionError",e,connectionString);
				throw ce;
			}
			try {
				ownSqlCommand = new SqlCommand(cmdText, ownSqlConnection);
				ownSqlDataReader = ownSqlCommand.ExecuteReader();
			} catch (Exception e) {
				HandleSqlExecutionException(e,cmdText);
			}
		}

		/// <summary>
		/// Return name of a field (indexed by its position) from the current record
		/// </summary>
		/// <param name="i">Field index number</param>
		/// <returns>Field name</returns>
		public string GetName (int i) { return ownSqlDataReader.GetName(i); }

		/// <summary>
		/// Return number of fields of the current record
		/// </summary>
		/// <returns>Number of fields</returns>
		public int FieldCount () { return ownSqlDataReader.FieldCount; }

		/// <summary>
		/// Retrieves the next record
		/// </summary>
		/// <returns>Success of retrieving a record</returns>
		public bool Read() { return this.ownSqlDataReader.Read(); }

		/// <summary>
		/// Return a field (indexed by its position) from the current record
		/// </summary>
		public object this [int i] { get { return ownSqlDataReader[i]; } }

		/// <summary>
		/// Return a field (indexed by its name) from the current record
		/// </summary>
		public object this [string name] { get { return ownSqlDataReader[name]; } }

		/// <summary>
		/// End of instance, class its resources
		/// </summary>
		public void Close() {
			this.ownSqlDataReader.Close();
			this.ownSqlConnection.Close();
		}

        /// <param name="sqlStatement">Strings to include in the CrmServiceException</param>
		public static void HandleSqlExecutionException(Exception e, string sqlStatement) {
			HandleSqlExecutionException(e, new Fujitsu.eCrm.Generic.SharedUtils.Trace(), sqlStatement);
		}
		

        /// <summary>
		/// Process the Exception and throw a CrmServiceException
		/// </summary>
		/// <param name="e">The SqlException</param>
		/// <param name="trace">The Trace file</param>
		/// <param name="sqlStatement">Strings to include in the CrmServiceException</param>
		public static void HandleSqlExecutionException(Exception e, ITrace trace, string sqlStatement) {

			string actor = "Server";
			string message = "ExecutionError";

			if (e is SqlException) {
				switch (((SqlException)e).Number) {
					case -2:
//						// Not sure if -2 is only ever given for time outs, check text as well
						Regex timeoutSeeker = new Regex("^Timeout expired");
						if (timeoutSeeker.IsMatch(e.Message))
						{
							actor = "Server";
							message = "TimeOut";
						}
						break;
					case 2601:
						actor = "Client";
						message = "UniqueConstraint";
						break;
					default:
						break;
				}
			}

			CrmServiceException e2 = new CrmServiceException(
				actor,"SqlError",message,e,trace,sqlStatement);
			throw e2;
		}

	}
}