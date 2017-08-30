using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Diagnostics;
using System.IO;


namespace Fujitsu.eCrm.Seoul.PosSocketsService {
	
	/// <summary>
	/// Summary description for PosHandler.
	/// </summary>
	public class BangkokPosHandler : NgcPosHandler {

		private static Regex numericExpression = new Regex(@"\A\d+\Z",RegexOptions.Compiled);

		#region Constructor
		static BangkokPosHandler() {
			inBufferLength = 300;
		}

		public BangkokPosHandler(IPosListener parent, Socket connection) : base(parent,connection) {
		}
		#endregion

		#region Validate Connection
		protected override void Validate() {
		}
		#endregion

		#region Process Header
		protected override bool ProcessHeader(NgcPosDialog ngcPosDialog) {
			switch (ngcPosDialog.InBuffer[0]) {
				case 1: // Message 01 Request Pos Get
					ngcPosDialog.Length = 200;
					ngcPosDialog.Process = new ProcessDelegate(this.PosGetProcess);
					return true;
				case 12: // Message 12 Request Pos Set
					ngcPosDialog.Length = 300;
					ngcPosDialog.Process = new ProcessDelegate(this.PosSetProcess);
					return true;
				default:
					PublishDataValueError(ngcPosDialog,"Message Type",ngcPosDialog.InBuffer[0].ToString());
					return false;
			}
		}
		#endregion

		#region Process Message
		#region Process POS Get Request
		/// <summary>
		/// UC16 Read Customer Details
		/// Precondition: card number's length, format and check digit have been validated
		/// </summary>
		/// <param name="ngcPosDialog"></param>
		protected bool PosGetProcess(NgcPosDialog ngcPosDialog) {

			#region Read Data from Input
			string storeCode;
			if (!FromNChar(ngcPosDialog,"Store Code",1,5,out storeCode)) {
				return false;
			}
			int storeCodeNumber = Int32.Parse(storeCode);
			
			// Do not need to check store code aginst port number
			//if (!this.StoreCodes.Contains(storeCodeNumber)) 
			//{
			//	PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
			//	return false;
			//}
				
			string cardNo;
			if (!FromNChar(ngcPosDialog,"Card Number",20,18,out cardNo)) {
				return false;
			}
			#endregion

			#region Process Data
			string customerTitle = String.Empty;
			string custName = String.Empty;
			string postalCode = String.Empty;
			decimal accumulatedPoints = 0;
			decimal pointsConvertedBalance = 0;
			decimal welcomePoints = 0;
			string primaryCardAccountNumber = String.Empty;
			string uniqueNumber = String.Empty;
			decimal extraPoints1Balance = 0;
			decimal extraPoints2Balance = 0;
			decimal extraPoints3Balance = 0;


			string message = String.Empty;
			int resultCode = 0;

			bool success = this.PosGet(
				ngcPosDialog,
				2,
				cardNo,
				out customerTitle,
				out custName,
				out postalCode,
				out accumulatedPoints,
				out pointsConvertedBalance,
				out welcomePoints,
				out primaryCardAccountNumber,
				out uniqueNumber,
				out extraPoints1Balance,
				out extraPoints2Balance,
				out extraPoints3Balance,
				storeCode,
				out message,
				out resultCode);
			if (!success) {
				return false;
			}

			string statusCode = null;
			switch (resultCode) {
				case 0: // Normal
				case 1: // New SkeletonCard
				case 11: // Existing Skeleton
				case 12: // Address In Error
					statusCode = "NR";
					break;
				case 2: // Invalid Card Number
				case 3: // Closed Card
				case 6: // Banned Customer
				case 9: // Deceased Customer
				case 10: // Customer Left Scheme
					statusCode = "NF";
					break;
			}
			#endregion

			#region Write Data to Output
			ngcPosDialog.OutBuffer = new byte[200];
			// Message Type
			ngcPosDialog.OutBuffer[0] = 109;
			// Store Number ... Clubcard Issue Number
			Buffer.BlockCopy(ngcPosDialog.InBuffer,1,ngcPosDialog.OutBuffer,1,40);
			// Status Code
			ToChar(statusCode,ngcPosDialog.OutBuffer,41,2);
			// Messages To Follow
			ToChar("N",ngcPosDialog.OutBuffer,43,1);
			// Training Mode
			// OLA Version
			ngcPosDialog.OutBuffer[45] = 2;
			// Title
			ToCharUtf8(customerTitle,ngcPosDialog.OutBuffer,50,4);
			// Initials
			ToCharUtf8(String.Empty,ngcPosDialog.OutBuffer,54,2);
			// Surname
			ToCharUtf8(custName,ngcPosDialog.OutBuffer,56,25);
			// Post Code
			ToChar(postalCode,ngcPosDialog.OutBuffer,81,10);
			// Points Balance
			ToNCharLeft(accumulatedPoints,ngcPosDialog.OutBuffer,91,7);
			// Current Time
			ToChar(DateTime.Now.ToString("yyyyMMddHHmm"),ngcPosDialog.OutBuffer,98,12);
			// Deals Spend ... Deal Status
			ToChar("0      0      Y0    010    020    030    040    05  10   0    ",ngcPosDialog.OutBuffer,110,62);
			// Message 1
			if (message == String.Empty) {
				ToChar(String.Empty,ngcPosDialog.OutBuffer,172,2);
			} else {
				ToNChar(message,ngcPosDialog.OutBuffer,172,2);
			}
			// Message 2 ... Message 9
			ToChar(String.Empty,ngcPosDialog.OutBuffer,174,16);
			// Filler
			#endregion

			return true;
		}
		#endregion

		#region POS Set Request
		/// <summary>
		/// UC03 Apply Points to Account
		/// </summary>
		/// <param name="ngcPosDialog"></param>
		protected bool PosSetProcess(NgcPosDialog ngcPosDialog) {

			#region Read Data from Input
			string storeCode;
			if (!FromNChar(ngcPosDialog,"Store Code",1,5,out storeCode)) {
				return false;
			}

            int storeCodeNumber = Int32.Parse(storeCode);
			// Do not need to check store code aginst port number
			//if (!this.StoreCodes.Contains(storeCodeNumber)) 
			//{
			//	PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
			//	return false;
			//}

            if (!EventLog.SourceExists("PosHandler"))
            {
                EventLog.CreateEventSource("PosHandler", "Application");
            }
            EventLog evLog = new EventLog();
            evLog.Source = "PosHandler";

            string data = Encoding.ASCII.GetString(ngcPosDialog.InBuffer).ToString();
            Fujitsu.eCrm.Generic.SharedUtils.Trace trace = new Fujitsu.eCrm.Generic.SharedUtils.Trace();
            trace.WriteInfo("Data: " + data);

            StreamWriter sw = new StreamWriter("C:\\Test.txt", true);
            sw.WriteLine(data);
            sw.Close();
           
			string posId;
			if (!FromFixNChar(ngcPosDialog,"POS Number",6,3,out posId)) {
				return false;
			}

			string txnNbr;
			if (!FromFixNChar(ngcPosDialog,"Transaction Number",9,5,out txnNbr)) {
				return false;
			}
				
			string posTxnType;
			if (!FromChar(ngcPosDialog,"Transaction Type",16,1,out posTxnType)) {
				return false;
			}
			string ngcTxnType;
			switch (posTxnType) {
				case "R":
					ngcTxnType = "3";
					break;
				case "U":
					ngcTxnType = "1";
					break;
				default:
					PublishDataValueError(ngcPosDialog,"Transaction Type",posTxnType);
					return false;
			}

			string cardNo;
			if (!FromNChar(ngcPosDialog,"Card Number",20,18,out cardNo)) {
				return false;
			}

			string posStoreDate;
			if (!FromChar(ngcPosDialog,"Transaction Date",50,14,out posStoreDate)) {
				return false;
			}
			string ngcStoreDate = null;
			try {
				DateTime temp = DateTime.ParseExact(posStoreDate,"yyyyMMddHHmmss",null);
				ngcStoreDate = temp.ToString("yyyy-MM-dd HH:mm:ss");
			} catch {
				PublishDataValueError(ngcPosDialog,"Transaction Date",posStoreDate);
				return false;
			}

			string points;
			if (!FromNChar(ngcPosDialog,"Points Awarded",64,7,out points)) {
				return false;
			}
				
			string amountSpent;
			if (!FromNChar(ngcPosDialog,"Total Spend",71,9,out amountSpent)) 
			{
				return false;
			}

			// From NChar has trimmed and raise exception for non numerics
			double dblAmountSpent = Convert.ToDouble(amountSpent)/100;
			amountSpent = dblAmountSpent.ToString("0.00");
		
			#endregion

            #region Process Data
			int resultCode;
			bool success = this.PosSet(
				ngcPosDialog,
				2,
				cardNo,
				defaultCustomerName,
				"1",
				ngcTxnType,
				storeCode,
				ngcStoreDate,
				posId,
				txnNbr,
				"Unknown",
				amountSpent,
				points,
				"0",
				"0",
				"0",
				"0",
				"0",
				out resultCode);
			if (!success) {
				return false;
			}
			#endregion

			#region Write Data to Output
			ngcPosDialog.OutBuffer = new byte[200];
			// Message Type
			ngcPosDialog.OutBuffer[0] = 112;
			// Store Number ... Clubcard Issue Number
			Buffer.BlockCopy(ngcPosDialog.InBuffer,1,ngcPosDialog.OutBuffer,1,40);
			// Status Code ... Messages To Follow
			ToChar("OKN",ngcPosDialog.OutBuffer,41,3);
			// Training Mode
			// OLA Version
			ngcPosDialog.OutBuffer[45] = 2;
			// Body Filler
			ToChar(String.Empty,ngcPosDialog.OutBuffer,50,150);
			#endregion

			return true;
		}
		#endregion
		#endregion

		#region Buffer Methods
		internal bool FromChar(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				value = value.TrimEnd(' ');
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal bool FromNChar(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				value = value.TrimStart('0');
				value = value.TrimEnd(' ');
				if (value == String.Empty) {
					value = "0";
				} else if (!numericExpression.IsMatch(value)) {
					PublishDataValueError(ngcPosDialog,fieldName,value);
					return false;
				}

				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal bool FromFixNChar(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				value = value.TrimEnd(' ');
				if (value == String.Empty) {
					value = "0";
				} else if (!numericExpression.IsMatch(value)) {
					PublishDataValueError(ngcPosDialog,fieldName,value);
					return false;
				}

				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal static bool ToNCharLeft(decimal value, byte[] dst, int dstOffset, int count) {
			try {
				string output;
				if (value <= 0) {
					output = "0".PadRight(count,' ');
				} else {
					output = value.ToString();
					if (output.Length > count) {
						output = String.Empty.PadLeft(count,'9');
					} else {
						output = output.PadRight(count,' ');
					}
				}
				Encoding.ASCII.GetBytes(output,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}

		internal static bool ToNChar(string value, byte[] dst, int dstOffset, int count) {
			try {

				if (value.Length > count) {
					value = String.Empty.PadLeft(count,'9');
				} else {
					value = value.PadLeft(count,'0');
				}
				Encoding.ASCII.GetBytes(value,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}
		
		internal static bool ToChar(string value, byte[] dst, int dstOffset, int count) {
			try {
				value = value.PadRight(count,' ');
				Encoding.ASCII.GetBytes(value,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}

		internal static bool ToCharUtf8(string value, byte[] dst, int dstOffset, int count) {
			try {
				value = value.PadRight(count,' ');
				Encoding.UTF8.GetBytes(value,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}
		#endregion
	}
}