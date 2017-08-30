using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Diagnostics;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {
	
	/// <summary>
	/// Summary description for PosHandler.
	/// </summary>
	public class SeoulPosHandler : NgcPosHandler {

		private static Regex numericExpression = new Regex(@"\A\d+\Z",RegexOptions.Compiled);
		private string messageType;

		#region Constructor
		static SeoulPosHandler() {
			inBufferLength = 512;
		}

		public SeoulPosHandler(IPosListener parent, Socket connection) : base(parent,connection) {
		}
		#endregion

		#region Validate Connection
		protected override void Validate() {

			// Identify the message(s) supported by this port
			System.Collections.ArrayList messageList = this.Parent.Messages;
			if (messageList != null) {
				if (messageList.Count == 1) {
					this.messageType = (string)this.Parent.Messages[0];
					switch (this.messageType) {
						case "IRT":
							this.Parent.Trace.WriteDebug("Port "+this.Parent.NgcIPAddress+":"+this.Parent.NgcPort+" supports IRT");
							return;
						case "TRAN":
							this.Parent.Trace.WriteDebug("Port "+this.Parent.NgcIPAddress+":"+this.Parent.NgcPort+" supports TRAN");
							return;
					}
				}
			}

			CrmServiceException ce = new CrmServiceException(
				"Server",
				"ConfigurationError",
				"PosSocketsService.UnknownMessageType",
				this.Parent.Trace,
				this.Parent.NgcIPAddress,
				this.Parent.NgcPort.ToString(),
				this.StoreIPAddress,
				this.StorePort.ToString());
			throw ce;

		}
		#endregion

		#region Process Header
		protected override bool ProcessHeader(NgcPosDialog ngcPosDialog) {
			if (ngcPosDialog.InBuffer[0] == 0x38) // Ascii 8 which is either 8000 or 8100
				ngcPosDialog.Length = 180;
			else
				ngcPosDialog.Length = 512;
			ngcPosDialog.Process = new ProcessDelegate(this.ProcessMessage);
			return true;
		}
		#endregion

		#region Process Message
		#region Identify Message Type
		protected bool ProcessMessage(NgcPosDialog ngcPosDialog) {

			switch (this.messageType) {
				case "IRT":
					return this.IrtProcess(ngcPosDialog);
				case "TRAN":
					string id;
					if (!ToFixAsciiToString(ngcPosDialog,"Coupon Code",0,4,out id)) {
						return false;
					}
					switch (id) {
						case "8000":
							return this.TranPointsProcess(ngcPosDialog);
						case "8100":
							string couponCode;
							if (!ToNumAsciiToString(ngcPosDialog,"Coupon Code",169,2,out couponCode)) {
								return false;
							}
							switch (couponCode) {
								case "78": // Cash Coupon
									return this.TranAmountProcess(ngcPosDialog);
								case "79": // Product Coupon
									return true;
								default:
									PublishDataValueError(ngcPosDialog,"Coupon Code",couponCode);
									return false;
							}
						default:
							PublishDataValueError(ngcPosDialog,"ID",id);
							return false;
					}
				default:
					return false;
			}
		}
		#endregion

		#region Process Irt Request
		/// <summary>
		/// UC16 Read Customer Details
		/// Precondition: card number's length, format and check digit have been validated
		/// </summary>
		/// <param name="ngcPosDialog"></param>
		internal bool IrtProcess(NgcPosDialog ngcPosDialog) {

			#region Read Data from Input
			int vli;
			if (!ToBigEndianToInt16(ngcPosDialog,"VLI",0,out vli)) {
				return false;
			}
			if (vli != 75) {
				PublishDataValueError(ngcPosDialog,"VLI",vli.ToString());
				return false;
			}

			int classification;
			if (!ToBigEndianToInt16(ngcPosDialog,"Classification",3,out classification)) {
				return false;
			}
			if (classification != 0x5009) {
				PublishDataValueError(ngcPosDialog,"Classification",classification.ToString());
				return false;
			}

			string storeCode;
			if (!ToBcdToString(ngcPosDialog,"Store Code",11,2,out storeCode)) {
				return false;
			}
			int storeCodeNumber;
			try {
				storeCodeNumber = Int32.Parse(storeCode);
			} catch {
				PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
				return false;
			}
			
			// Do not need to check Store Code against Port Number
			//if (!this.StoreCodes.Contains(storeCodeNumber)) {
			//	PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
			//	return false;
			//}

			string cardNo;
			if (!ToVarAsciiToString(ngcPosDialog,"Card Number",42,20,out cardNo)) {
				return false;
			}
			try {
				Decimal.Parse(cardNo);
			} catch {
				PublishDataValueError(ngcPosDialog,"Card Number",cardNo);
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
				1,
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

			short status = 0;
			switch (resultCode) {
				case 0: // Normal
					status = 0;
					break;
				case 1: // New SkeletonCard
				case 11: // Existing Skeleton
				case 12: // Address In Error
					status = 3;
					break;
				case 2: // Invalid Card Number
				case 3: // Card Account Closed
				case 6: // Banned Customer
				case 9: // Deceased Customer
				case 10: // Customer Left Scheme
					status = 4;
					break;
			}
			#endregion

			#region Write Data to Output
			ngcPosDialog.OutBuffer = new byte[512];
			// VLI
			ToBigEndianToBinary((short)229,ngcPosDialog.OutBuffer,0);
			// Kind
			Buffer.BlockCopy(ngcPosDialog.InBuffer,2,ngcPosDialog.OutBuffer,2,1);
			// Classification
			ToBigEndianToBinary((short)0x5509,ngcPosDialog.OutBuffer,3);
			// MD ... Port
			Buffer.BlockCopy(ngcPosDialog.InBuffer,5,ngcPosDialog.OutBuffer,5,34);
			// ID
			ToBigEndianToBinary((short)0x5509,ngcPosDialog.OutBuffer,39);
			// Status
			ToBigEndianToBinary(status,ngcPosDialog.OutBuffer,41);
			// Card Number ... Social Security Number
			Buffer.BlockCopy(ngcPosDialog.InBuffer,42,ngcPosDialog.OutBuffer,43,33);
			// Customer Name
			if (status != 4) {
				ToKoreanToBinary(custName,ngcPosDialog.OutBuffer,76, 20);
			}
			// Rank
			// Accumulated Points ... Welcomed Points
			if (status != 4) {
				accumulatedPoints = Decimal.Floor(accumulatedPoints);	// round down to a whole number
				if (accumulatedPoints < 0) {
					accumulatedPoints = 0;
				}
				ToNumAsciiToBinary(accumulatedPoints,ngcPosDialog.OutBuffer,98,9,"000000000");

				welcomePoints = Decimal.Floor(welcomePoints);		// to a whole number
				if (welcomePoints < 0) {
					welcomePoints = 0;
				}
				ToNumAsciiToBinary(welcomePoints,ngcPosDialog.OutBuffer,107,5,"00000");

				extraPoints1Balance = Decimal.Floor(extraPoints1Balance);		// to a whole number
				if (extraPoints1Balance < 0) 
				{
					extraPoints1Balance = 0;
				}
				ToNumAsciiToBinary(extraPoints1Balance,ngcPosDialog.OutBuffer,135,5,"00000");

			}

			// Amount .. Points
			// Message
			if ((status == 3) || (status == 4)) {
				ToKoreanToBinary(message,ngcPosDialog.OutBuffer,140,60);
			}
			// Representative Card Number ... Coupon Issue Points
			if (status != 4) {
				//ToAsciiToBinary(primaryCardAccountNumber,ngcPosDialog.OutBuffer,198,20);
				ToAsciiToBinary(uniqueNumber,ngcPosDialog.OutBuffer,200,20);

				decimal roundedPointsConvertedBalance = Decimal.Floor(pointsConvertedBalance);	// to a whole number
				if (roundedPointsConvertedBalance != pointsConvertedBalance) {
					roundedPointsConvertedBalance++;
				}
				if (roundedPointsConvertedBalance < 0) {
					pointsConvertedBalance = 0;
				} else {
					pointsConvertedBalance = roundedPointsConvertedBalance;
				}
				ToNumAsciiToBinary(pointsConvertedBalance,ngcPosDialog.OutBuffer,220,9,"000000000");
			}
			#endregion

			return true;
		}
		#endregion

		#region Process Points
		/// <summary>
		/// UC03 Apply Points to Account
		/// </summary>
		/// <param name="ngcPosDialog"></param>
		internal bool TranPointsProcess(NgcPosDialog ngcPosDialog) {

			#region Read Data from Input
			string posTxnType;
			if (!ToFixAsciiToString(ngcPosDialog,"Transaction Type",4,4,out posTxnType)) {
				return false;
			}
			string ngcTxnType;
			switch (posTxnType) {
				case "0100":
					ngcTxnType = "1";
					break;
				case "0200":
					ngcTxnType = "2";
					break;
				case "0300":
					ngcTxnType = "3";
					break;
				default:
					PublishDataValueError(ngcPosDialog,"Transaction Type",posTxnType);
					return false;
			}

			string storeCode;
			if (!ToNumAsciiToString(ngcPosDialog,"Store Code",18,4, out storeCode)) {
				return false;
			}
			int storeCodeNumber = Int32.Parse(storeCode);
			
			// Do not need to check store code aginst port number
			//if (!this.StoreCodes.Contains(storeCodeNumber)) 
			//{
			//	PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
			//	return false;
			//}

			string posStoreDate;
			if (!ToFixAsciiToString(ngcPosDialog,"Date",22,8,out posStoreDate)) {
				return false;
			}
			try {
				DateTime.ParseExact(posStoreDate,"yyyyMMdd",null);
			} catch {
				PublishDataValueError(ngcPosDialog,"Date",posStoreDate);
				return false;
			}

			string posStoreTime;
			if (!ToFixAsciiToString(ngcPosDialog,"Time",30,6,out posStoreTime)) {
				return false;
			}
			string ngcStoreDate;
			try {
				DateTime temp = DateTime.ParseExact(posStoreDate+posStoreTime,"yyyyMMddHHmmss",null);
				ngcStoreDate = temp.ToString("yyyy-MM-dd HH:mm:ss");
			} catch {
				PublishDataValueError(ngcPosDialog,"Time",posStoreTime);
				return false;
			}

			string posId;
			if (!ToFixNumAsciiToString(ngcPosDialog,"POS Number",36,6,out posId)) {
				return false;
			}

			string txnNbr;
			if (!ToFixNumAsciiToString(ngcPosDialog,"Transaction Number",42,4,out txnNbr)) {
				return false;
			}

			string cashierId;           			
            if (!ToNumAsciiToString(ngcPosDialog,"Cashier Number",48,14,out cashierId)) {
				return false;
			}
           			
			string cardAccountNo;
			if (!ToVarAsciiToString(ngcPosDialog,"Card Number",86,20,out cardAccountNo)) {
				return false;
			}
			try {
				Decimal.Parse(cardAccountNo);
			} catch {
				PublishDataValueError(ngcPosDialog,"Card Number",cardAccountNo);
				return false;
			}

			string posType;
			if (!ToNumAsciiToString(ngcPosDialog,"POS Flag",106,1,out posType)) {
				return false;
			}
			if ((posType != "1") && (posType != "2") && (posType != "3")) {
				PublishDataValueError(ngcPosDialog,"POS Flag",posType);
				return false;
			}

			string amountSpent;
			if (!ToNumAsciiToString(ngcPosDialog,"Total Sales Amount",107,9,out amountSpent)) {
				return false;
			}

			string points;
			if (!ToNumAsciiToString(ngcPosDialog,"Issue Points",125,9,out points)) {
				return false;
			}

			string welcomePoints;
			if (!ToNumAsciiToString(ngcPosDialog,"Welcome Points",134,9,out welcomePoints)) {
				return false;
			}

			string hscPoints;
			if (!ToNumAsciiToString(ngcPosDialog,"HSC Points",143,9,out hscPoints)) {
				return false;
			}

			string skuPoints;
			if (!ToNumAsciiToString(ngcPosDialog,"SKU Points",152,9,out skuPoints)) {
				return false;
			}
			#endregion

			#region Process Data
			int resultCode;
			bool success = this.PosSet(
				ngcPosDialog,
				1,
				cardAccountNo,
				defaultCustomerName,
				posType,
				ngcTxnType,
				storeCode,
				ngcStoreDate,
				posId,
				txnNbr,
				cashierId,
				amountSpent,
				points,
				welcomePoints,
				skuPoints,
				hscPoints,
				"0",
				"0",
				out resultCode);
			#endregion

			# region Generate reply
			ngcPosDialog.OutBuffer = new byte[180];
			ToNumAsciiToBinary( 8050,ngcPosDialog.OutBuffer,0,4,"0000");
			Buffer.BlockCopy(ngcPosDialog.InBuffer,4,ngcPosDialog.OutBuffer,4,58);
			
			ToAsciiToBinary("OK",ngcPosDialog.OutBuffer,62,2);
			ToAsciiToBinary(new string(' ',116),ngcPosDialog.OutBuffer,64,116);
			#endregion

			return success;
		}
		#endregion

		#region Process Vouchers
		/// <summary>
		/// UC05 Capture Use of Reward Voucher
		/// </summary>
		/// <param name="ngcPosDialog"></param>
		internal bool TranAmountProcess(NgcPosDialog ngcPosDialog) {

			#region Read Data
			string storeCode;
			if (!ToNumAsciiToString(ngcPosDialog,"Store Code",18,4,out storeCode)) {
				return false;
			} 
			int storeCodeNumber = Int32.Parse(storeCode);
			// Do not need to check store code aginst port number
			//if (!this.StoreCodes.Contains(storeCodeNumber)) {
			//	PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
			//	return false;
			//}

			string posStoreDate;
			if (!ToFixAsciiToString(ngcPosDialog,"Date",22,8, out posStoreDate)) {
				return false;
			}
			try {
				DateTime.ParseExact(posStoreDate,"yyyyMMdd",null);
			} catch {
				PublishDataValueError(ngcPosDialog,"Date",posStoreDate);
				return false;
			}

			string posStoreTime;
			if (!ToFixAsciiToString(ngcPosDialog,"Time",30,6,out posStoreTime)) {
				return false;
			}
			string ngcStoreDate = null;
			try {
				DateTime temp = DateTime.ParseExact(posStoreDate+posStoreTime,"yyyyMMddHHmmss",null);
				ngcStoreDate = temp.ToString("yyyy-MM-dd HH:mm:ss");
			} catch {
				PublishDataValueError(ngcPosDialog,"Time",posStoreTime);
				return false;
			}

			string posId;
			if (!ToFixNumAsciiToString(ngcPosDialog,"POS Number",36,6,out posId)) {
				return false;
			}

			string txnNbr;
			if (!ToFixNumAsciiToString(ngcPosDialog,"Transaction Number",42,4,out txnNbr)) {
				return false;
			}

			string voucherValue;
			if (!ToNumAsciiToString(ngcPosDialog,"Voucher Amount",119,9,out voucherValue)) {
				return false;
			}

			string posExpiryYear;
			if (!ToFixAsciiToString(ngcPosDialog,"Voucher Expiry Year",151,1,out posExpiryYear)) {
				return false;
			}
			string posExpiryMonthDay;
			if (!ToFixAsciiToString(ngcPosDialog,"Voucher Expiry Month",152,4, out posExpiryMonthDay)) {
				return false;
			}
			string ngcExpiryDate = null;
			try {
				// Determine the voucher's expiry date
				// It can be no more than 6 years difference between now and the expiry date
				// Only use year in the calculate because it is not safe to append 29-Feb to any year
				DateTime now = DateTime.Now;

				int nowYear = now.Year % 10;
				int posYear = Int32.Parse(posExpiryYear);
				int diffYear = nowYear - posYear;

				int currentDecade = now.Year/(int)10;
				int assumedDecade = currentDecade;
				if (diffYear <= -5) {
					assumedDecade--;
				} else if (diffYear > 5) {
					assumedDecade++;
				}

				DateTime temp = DateTime.ParseExact(assumedDecade+posExpiryYear+posExpiryMonthDay,"yyyyMMdd",null);
				ngcExpiryDate = temp.ToString("yyyy-MM-dd");
			} catch {
				PublishDataValueError(ngcPosDialog,"Voucher Expiry Date",posExpiryYear+posExpiryMonthDay);
				return false;
			}
			#endregion

			#region Process Data
			int resultCode;
			bool success = this.PosVoucherRedeem(
				ngcPosDialog,
				1,
				storeCode,
				ngcStoreDate,
				posId,
				txnNbr,
				voucherValue,
				ngcExpiryDate,
				out resultCode);
			#endregion

			# region Generate reply
			ngcPosDialog.OutBuffer = new byte[180];
			ToNumAsciiToBinary( 8150,ngcPosDialog.OutBuffer,0,4,"0000");
			Buffer.BlockCopy(ngcPosDialog.InBuffer,4,ngcPosDialog.OutBuffer,4,58);
			
			ToAsciiToBinary("OK",ngcPosDialog.OutBuffer,62,2);
			ToAsciiToBinary(new string(' ',116),ngcPosDialog.OutBuffer,64,116);
			#endregion

			return success;
		}
		#endregion
		#endregion

		#region Buffer Methods
		#region From Binary
		internal bool ToBigEndianToInt16(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, out int value) {
			try {
				byte[] dst = new Byte[2];
				Buffer.BlockCopy(ngcPosDialog.InBuffer,startIndex,dst,0,2);
				if (BitConverter.IsLittleEndian) {
					byte temp = dst[0];
					dst[0] = dst[1];
					dst[1] = temp;
				}
				value = BitConverter.ToInt16(dst,0);
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = 0;
				return false;
			}
		}

		/// <summary>
		/// Converts a byte array in Binary Coded Decimal format
		/// into a string, checks that values are 0-9, else empty string.
		/// </summary> 
		internal bool ToBcdToString(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {			
			try {
				StringBuilder str = new StringBuilder();
				for (int index=startIndex; index<startIndex+length; index++) {
					int upperQuartet = ngcPosDialog.InBuffer[index] >> 4;
					int lowerQuartet = ngcPosDialog.InBuffer[index] & 0xf;
					if ((upperQuartet > 9) || (lowerQuartet > 9)) {
						PublishDataFormatError(ngcPosDialog,fieldName);
						value = null;
						return false;
					}
					str.Append(upperQuartet);
					str.Append(lowerQuartet);
				}
				value = str.ToString();
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal bool ToFixAsciiToString(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal bool ToNumAsciiToString(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				value = value.TrimStart('0');
				if (value == String.Empty) {
					value = "0";
				} else if (!numericExpression.IsMatch(value)) {
					PublishDataValueError(ngcPosDialog,fieldName,value);
					value = null;
					return false;
				}
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}

		internal bool ToFixNumAsciiToString(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,length);
				if (!numericExpression.IsMatch(value)) {
					PublishDataValueError(ngcPosDialog,fieldName,value);
					value = null;
					return false;
				}
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}
		
		internal bool ToVarAsciiToString(NgcPosDialog ngcPosDialog, string fieldName, int startIndex, int length, out string value) {
			try {
				int count = 0;
				for (int index=startIndex; index<startIndex+length; index++) {
					if (ngcPosDialog.InBuffer[index] == 0) {
						break;
					}
					count++;
				}
				value = Encoding.ASCII.GetString(ngcPosDialog.InBuffer,startIndex,count);
				return true;
			} catch {
				PublishDataFormatError(ngcPosDialog,fieldName);
				value = null;
				return false;
			}
		}
		#endregion
		
		#region To Binary
		internal static bool ToAsciiToBinary(string value, byte[] dst, int dstOffset, int count) {
			try {
				if (value.Length < count) {
					count = value.Length;
				}
				Encoding.ASCII.GetBytes(value,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}

		internal static bool ToNumAsciiToBinary(decimal value, byte[] dst, int dstOffset, int count, string format) {
			try {
				string temp = value.ToString(format);
				if (temp.Length > count) {
					temp = new String('9',count);
				}
				Encoding.ASCII.GetBytes(temp,0,count,dst,dstOffset);
				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Converts an integer into a two byte field at the specified in a hex array 
		/// </summary>
		/// <param name="i"></param>
		/// <param name="array"></param>
		/// <param name="index"></param>
		internal static bool ToBigEndianToBinary(short value, byte[] dst, int dstOffset) {
			try {
				byte[] src = BitConverter.GetBytes(value);
				if (BitConverter.IsLittleEndian) {
					byte temp = src[0];
					src[0] = src[1];
					src[1] = temp;
				}
				Buffer.BlockCopy(src,0,dst,dstOffset,2);
				return true;
			} catch {
				return false;
			}
		}

		/// <summary>
		/// Selects the Unicode characters from a string and converts them to 
		/// Korean (single-byte characters are returned unchanged).
		/// </summary>
		internal static bool ToKoreanToBinary(string value, byte[] dst, int dstOffset, int count) {
			try {
				byte[] uniByteArray = Encoding.Unicode.GetBytes(value);
				byte[] encodedBytes = Encoding.Convert(Encoding.Unicode,Encoding.GetEncoding(949),uniByteArray);
				if (encodedBytes.Length < count) {
					count = encodedBytes.Length;
				}

				Buffer.BlockCopy(encodedBytes,0,dst,dstOffset,count);
				return true;
			} catch {
				return false;
			}
		}


		#endregion
		#endregion
		
	}
}