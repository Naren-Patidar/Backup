#region Using
using System;
using System.Diagnostics;
#endregion

namespace PollPartnerBatchService
{
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public class Logger
	{
		public Logger()
		{
			
		}

		#region WriteToEventLog
		/// <summary>
		/// Write a message to the eventlog
		/// </summary>
		/// <param name="filename">The event source</param>
		/// <param name="message">The message to be written</param>
		public static void WriteToEventLog(string filename,string message)
		{
			//Check whether a soure already exists.If not create it
			if(!EventLog.SourceExists(filename))
			{
				EventLog.CreateEventSource(filename,"Application");
			}
			EventLog.WriteEntry(filename,message);
		}
		#endregion
	}
}
