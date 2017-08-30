using System;
using System.Threading;
using System.Net.Sockets;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	public delegate bool ProcessDelegate(NgcPosDialog ngcPosDialog);

	public class NgcPosDialog {

		private byte[] inBuffer;
		private byte[] outBuffer;
		private int offset;
		private int length;
		private Timer receiveTimeoutTimer;
		private Timer sendTimeoutTimer;
		private ProcessDelegate process;

		public byte[] InBuffer {
			get { return this.inBuffer; }
			set { this.inBuffer = value; }
		}

		public byte[] OutBuffer {
			get { return this.outBuffer; }
			set { this.outBuffer = value; }
		}

		public int Offset {
			get { return this.offset; }
			set { this.offset = value; }
		}

		public int Length {
			get { return this.length; }
			set { this.length = value; }
		}

		public Timer ReceiveTimeoutTimer {
			get { return this.receiveTimeoutTimer; }
			set { this.receiveTimeoutTimer = value; }
		}

		public Timer SendTimeoutTimer {
			get { return this.sendTimeoutTimer; }
			set { this.sendTimeoutTimer = value; }
		}
		
		public ProcessDelegate Process {
			get { return this.process; }
			set { this.process = value; }
		}

	}
}
