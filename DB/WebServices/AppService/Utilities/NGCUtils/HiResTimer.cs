using System;
using System.Runtime.InteropServices;

namespace Tesco.NGC.Utils
{

	#region Header
	///
	/// <summary>
	/// Hi-resoultion timer for performance measurement
	///
	/// from www.GotDotNet.com
	///
	/// Usage:
	///    HiResTimer tim = new HiResTimer();
	///    tim.Start();
	///		.... time consuming code ..
	///    tim.Stop();
	///    trace.writeln("Time = " + tim.ElapsedSeconds + " seconds");
	/// </summary>
	/// 
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.00" day="06" month="03" year="2002">
	///			<developer>Gary Blead</developer>
	///			<checker></checker>
	///			<work_packet>WP/Dogon/001</work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header
	public class HiResTimer {

		[DllImport("kernel32.dll")]
		private static extern int QueryPerformanceFrequency(out long frequency);
	
		[DllImport("kernel32.dll")]
		private static extern int QueryPerformanceCounter(out long tick);
	
		private long start;
		private long stop;
		private long frequency;

		/// <summary>
		/// Construct a timer
		/// </summary>
		public HiResTimer() {
			QueryPerformanceFrequency(out this.frequency);
			//if (_frequency == 0) throw new InvalidOperationException("Computer does not support high resolution timing.");
		}

		/// <summary>
		/// Returns the time between the Start and Stop function calls in
		/// Seconds. If the hardware doesn't support the high-resolution 
		/// performance counter, it returns zero.
		/// </summary>
		public decimal ElapsedSeconds {
			get	{
				if (this.frequency > 0) {
					return ((decimal)this.ElapsedTicks / (decimal)this.frequency);
				} else {
					return 0M;
				}
			}
		}

		/// <summary>
		/// Like ElapsedSeconds, but returns the time in Microseconds.
		/// </summary>
		public decimal ElapsedMicroseconds { get { return this.ElapsedSeconds * 1000 * 1000; } }
	
		/// <summary>
		/// Like ElapsedSeconds, but returns the time in Milliseconds.
		/// </summary>
		public decimal ElapsedMilliseconds { get { return this.ElapsedSeconds * 1000; } }

		/// <summary>
		/// Returns the number of "ticks" between the start and stop 
		/// function callls. Call TicksPerSecond() to get the duration of a tick.
		/// </summary>
		public long ElapsedTicks { get {return (this.stop - this.start);} }
	
		/// <summary>
		/// Returns the number of timer "ticks" per second. Gives an 
		/// indication of timer resolution. Returns 0 if the hardware doesnt
		/// support a high-resolution performance timer.
		/// </summary>
		public long TicksPerSecond { get {return this.frequency;} }
	
		/// <summary>
		/// Starts the timer
		/// </summary>
		public void Start() {
			QueryPerformanceCounter(out this.start);
		}
	
		/// <summary>
		/// Stops the timer
		/// </summary>
		/// <returns>The duration the timer was running</returns>
		public long Stop() {
			QueryPerformanceCounter(out this.stop);
			return this.ElapsedTicks;
		}
	}
}
