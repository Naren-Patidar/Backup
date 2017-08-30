using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	public abstract class NgcPosHandler : IPosHandler {

		#region Statics
		protected internal static readonly string defaultCustomerName;
		protected internal static readonly string newSkeletonCardString;			// POS.NewSkeleton
		protected internal static readonly string invalidCardNoString;				// POS.InvalidCard
		protected internal static readonly string closedCardString;				// POS.ClosedCard
		protected internal static readonly string bannedCustomerString;			// POS.BannedCustomer
		protected internal static readonly string deceasedCustomerString;			// POS.DeceasedCustomer
		protected internal static readonly string customerLeftSchemeString;		// POS.LeftScheme
		protected internal static readonly string existingSkeletonString;			// POS.ExistingSkeleton
		protected internal static readonly string addressErrorString;				// POS.AddressError
		protected internal static int inBufferLength;

		private const string cultureDefaultSetting = "CultureDefault";
		private const string userCultureDefaultSetting = "UserCultureDefault";

		static NgcPosHandler() {

			CultureInfo[] cultureList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			string installCultureString = ConfigurationSettings.AppSettings[cultureDefaultSetting];
			string userCultureString = ConfigurationSettings.AppSettings[userCultureDefaultSetting];
			foreach (CultureInfo specificCulture in cultureList) {
				if (specificCulture.Name == userCultureString) {
					// set the thread to the user culture stop looking
					Thread.CurrentThread.CurrentCulture = specificCulture;
					break;
				} else if (specificCulture.Name == installCultureString) {
					// set the thread to the installed culture but continue to look for the user culture
					Thread.CurrentThread.CurrentCulture = specificCulture;
				}
			}

			defaultCustomerName = Localization.GetLocalizedAttributeString("POS.DefaultCustomerName");
			newSkeletonCardString  = Localization.GetLocalizedAttributeString("POS.NewSkeleton");
			invalidCardNoString  = Localization.GetLocalizedAttributeString("POS.InvalidCard");
			closedCardString  = Localization.GetLocalizedAttributeString("POS.ClosedCard");
			bannedCustomerString  = Localization.GetLocalizedAttributeString("POS.BannedCustomer");
			deceasedCustomerString  = Localization.GetLocalizedAttributeString("POS.DeceasedCustomer");
			customerLeftSchemeString  = Localization.GetLocalizedAttributeString("POS.LeftScheme");
			existingSkeletonString  = Localization.GetLocalizedAttributeString("POS.ExistingSkeleton");
			addressErrorString  = Localization.GetLocalizedAttributeString("POS.AddressError");
		}
		#endregion

		#region Attributes
		internal Socket connection;
		internal IPosListener parent;
		internal readonly string storeIPAddress;
		internal readonly int storePort;
		#endregion

		#region Properties
		protected Socket Connection { get { return this.connection; } }
		protected IPosListener Parent {	get { return this.parent; } }
		public string StoreIPAddress { get { return this.storeIPAddress; } }
		public int StorePort { get { return this.storePort; } }
		protected ArrayList StoreCodes { get { return this.parent.StoreCodes(this.storeIPAddress); } }
		#endregion

		#region Constructor
		protected NgcPosHandler(IPosListener parent, Socket connection) {
			ITraceState trState = parent.Trace.StartProc("NgcPosHandler");
			try {
				this.parent = parent;
				this.connection = connection;
				this.storeIPAddress = ((IPEndPoint)connection.RemoteEndPoint).Address.ToString();
				this.storePort = ((IPEndPoint)connection.RemoteEndPoint).Port;
				this.parent.Trace.WriteDebug("Starting Connection between NGC Port "+this.parent.NgcIPAddress+":"+this.parent.NgcPort+" and Store Port "+this.storeIPAddress+":"+this.storePort);
				this.parent.PosSocketsEventLog.WriteEntry("Starting Connection between NGC Port "+this.parent.NgcIPAddress+":"+this.parent.NgcPort+" and Store Port "+this.storeIPAddress+":"+this.storePort);

				ITraceState trState2 = this.parent.Trace.StartProc("Validate");
				try {
					this.Validate();
				} finally {
					trState2.EndProc();
				}

				this.ReceiveHeader();

			} catch (CrmServiceException e) {
				this.Dispose();
				throw e;
			} catch (SocketException e) {
				CrmServiceException ce = new CrmServiceException(
					"Network",
					"NetworkError",
					"PosSocketsService.SocketException",
					e,
					this.parent.Trace,
					this.parent.NgcIPAddress,
					this.parent.NgcPort.ToString(),
					this.storeIPAddress,
					this.storePort.ToString());
				this.Dispose();
				throw ce;
			} catch (Exception e) {
				CrmServiceException ce = new CrmServiceException(
					"Server",
					"SystemError",
					"PosSocketsService.UnexpectedException",
					e,
					this.parent.Trace,
					this.parent.NgcIPAddress,
					this.parent.NgcPort.ToString(),
					this.storeIPAddress,
					this.storePort.ToString());
				this.Dispose();
				throw ce;
			} finally {
				trState.EndProc();
			}
		}
		#endregion

		#region IO Methods
		protected abstract void Validate();

		private void ReceiveHeader() {
			ITraceState trState = this.Parent.Trace.StartProc("ReceiveHeader");
			bool isOkay = false;
			try {
				// Setup Header Read
				NgcPosDialog ngcPosDialog = new NgcPosDialog();
				ngcPosDialog.InBuffer = new byte[inBufferLength];

				// Start Header Read
				this.Connection.BeginReceive(
					ngcPosDialog.InBuffer,
					0,
					1,
					0,
					new AsyncCallback(this.CallbackHeaderReceive),
					ngcPosDialog);

				isOkay = true;

			} catch (ObjectDisposedException) {
				// this has been disposed in another thread
				isOkay = true;
			} catch (SocketException) {
				// not really an error as a message is not being processed
			} catch (Exception e) {
				PublishSystemException(e);
			} finally {
				if (!isOkay) {
					this.Close();
				}
				trState.EndProc();
			}
		}

		private void CallbackHeaderReceive(IAsyncResult ar) {
			ITraceState trState = this.Parent.Trace.StartProc("CallbackHeaderReceive");
			bool isOkay = false;
			try {
				this.parent.Trace.WriteDebug("Get dialog object");
				NgcPosDialog ngcPosDialog = (NgcPosDialog)ar.AsyncState;
				this.parent.Trace.WriteDebug("Receive data");

				int requestByteSize = this.Connection.EndReceive(ar);
				this.parent.Trace.WriteDebug("Received " + requestByteSize + "bytes" );
				if (requestByteSize >= 1)
				{
					this.WriteDebugBuffer(ngcPosDialog.InBuffer,requestByteSize);
				}


				if (requestByteSize == 1) {
					this.WriteDebugBuffer(ngcPosDialog.InBuffer,1);

					ITraceState trState2 = this.parent.Trace.StartProc("ProcessHeader");
					try {
						if (!this.ProcessHeader(ngcPosDialog)) {
							return;
						}
					} finally {
						trState2.EndProc();
					}

					// Limit the time waiting to read the remainder of the message
					ngcPosDialog.ReceiveTimeoutTimer = new Timer(new TimerCallback(this.ReceiveTimeout),ngcPosDialog,this.Parent.TimeoutPeriod,Timeout.Infinite);

					// Read Body
					ngcPosDialog.Offset = 1;
					this.Connection.BeginReceive(
						ngcPosDialog.InBuffer,
						ngcPosDialog.Offset,
						ngcPosDialog.Length-ngcPosDialog.Offset,
						0,
						new AsyncCallback(this.CallbackMessageReceive),
						ngcPosDialog);
				} else {
					// client is closing connection
					return;
				}

				isOkay = true;

			} catch (ObjectDisposedException) {
				// this has been disposed in another thread
				isOkay = true;
			} catch (SocketException) {
				// not really an error as a message is not being processed
			} catch (Exception e) {
				PublishSystemException(e);
			} finally {
				if (!isOkay) {
					this.Close();
				}
				trState.EndProc();
			}
		}

		private void ReceiveTimeout(Object arg) {
			ITraceState trState = this.parent.Trace.StartProc("ReceiveTimeout");
			try {
				NgcPosDialog ngcPosDialog = (NgcPosDialog)arg;
				lock (ngcPosDialog) {
					if (ngcPosDialog.ReceiveTimeoutTimer == null) { 
						// CallbackMessageReceive has been called, data received, do not
						// do anything
						this.parent.Trace.WriteDebug("Must have received data so do not time out");
						return;
					}
					ngcPosDialog.ReceiveTimeoutTimer.Dispose();
					ngcPosDialog.ReceiveTimeoutTimer = null;
					PublishTimeoutDataReceived(ngcPosDialog);
					this.Close();
				}
			} finally {
				trState.EndProc();
			}
		}

		protected abstract bool ProcessHeader(NgcPosDialog ngcPosDialog);

		private void CallbackMessageReceive(IAsyncResult ar) {
			ITraceState trState = this.Parent.Trace.StartProc("CallbackMessageReceive");
			bool isOkay = false;
			try {
				NgcPosDialog ngcPosDialog = (NgcPosDialog)ar.AsyncState;
				lock (ngcPosDialog) {
					if (ngcPosDialog.ReceiveTimeoutTimer == null) { 
						// ReceiveTimeout has been called, connection is closed, do not
						// do anything
						return;
					}

					int requestByteSize = this.Connection.EndReceive(ar);
					// Write the request buffer to the trace file
					this.WriteDebugBuffer(ngcPosDialog.InBuffer,ngcPosDialog.Offset+requestByteSize);

					// Has the whole message been received
					if (requestByteSize == ngcPosDialog.Length-ngcPosDialog.Offset) {
						#region Process the Message
						// Stop the timeout timer
						ngcPosDialog.ReceiveTimeoutTimer.Dispose();
						ngcPosDialog.ReceiveTimeoutTimer = null;

						// Start waiting from the next message
						this.ReceiveHeader();

						// Write the request buffer to the trace file
						this.WriteDebugBuffer(ngcPosDialog.InBuffer,ngcPosDialog.Length);

						// Process the message
						ITraceState trState2 = this.Parent.Trace.StartProc("ProcessMessage");
						try {
							if (!ngcPosDialog.Process(ngcPosDialog)) {
								return;
							}
						} finally {
							trState2.EndProc();
						}

						// Send the response
						if (ngcPosDialog.OutBuffer != null) {
							this.WriteDebugBuffer(ngcPosDialog.OutBuffer,ngcPosDialog.OutBuffer.Length);

							// Limit the time waiting to read the remainder of the message
							ngcPosDialog.SendTimeoutTimer = new Timer(new TimerCallback(this.SendTimeout),ngcPosDialog,this.Parent.TimeoutPeriod,Timeout.Infinite);

							this.Connection.BeginSend(
								ngcPosDialog.OutBuffer,
								0,
								ngcPosDialog.OutBuffer.Length,
								0,
								new AsyncCallback(this.CallbackSend),
								ngcPosDialog);

						}
						#endregion
					} else {
						#region Read the Remaining Message
						// This should not happen unless the client is sending the message
						// in packets
						if (this.parent.Trace.TraceLevel.Debug) 
						{
							this.parent.Trace.WriteDebug("Read " + requestByteSize + " bytes. Waiting for next packet");
						}
						ngcPosDialog.Offset = ngcPosDialog.Offset+requestByteSize;
						this.Connection.BeginReceive(
							ngcPosDialog.InBuffer,
							ngcPosDialog.Offset,
							ngcPosDialog.Length-ngcPosDialog.Offset,
							0,
							new AsyncCallback(this.CallbackMessageReceive),
							ngcPosDialog);
						#endregion
					}
				}
				isOkay = true;

			} catch (ObjectDisposedException) {
				// this has been disposed in another thread
				isOkay = true;
			} catch (SocketException e) {
				PublishNetworkException(e);
			} catch (Exception e) {
				PublishSystemException(e);
			} finally {
				if (!isOkay) {
					this.Close();
				}
				trState.EndProc();
			}
		}

		private void SendTimeout(Object arg) {
			ITraceState trState = this.parent.Trace.StartProc("ReceiveTimeout");
			try {
				NgcPosDialog ngcPosDialog = (NgcPosDialog)arg;
				lock (ngcPosDialog) {
					if (ngcPosDialog.SendTimeoutTimer == null) { 
						// CallbackSend has been called, data sent, do not
						// do anything
						this.parent.Trace.WriteDebug("Must have sent data so do not time out");
						return;
					}
					ngcPosDialog.SendTimeoutTimer.Dispose();
					ngcPosDialog.SendTimeoutTimer = null;
					PublishTimeoutDataSent(ngcPosDialog);
					this.Close();
				}
			} finally {
				trState.EndProc();
			}
		}

		private void CallbackSend(IAsyncResult ar) {
			ITraceState trState = this.Parent.Trace.StartProc("CallbackSend");
			bool isOkay = false;
			try {
				NgcPosDialog ngcPosDialog = (NgcPosDialog)ar.AsyncState;
				lock (ngcPosDialog) {
					if (ngcPosDialog.SendTimeoutTimer == null) { 
						// SendTimeout has been called, connection is closed, do not
						// do anything
						return;
					}

					// Stop the timeout timer
					ngcPosDialog.SendTimeoutTimer.Dispose();
					ngcPosDialog.SendTimeoutTimer = null;

					int responseByteSize = this.Connection.EndSend(ar);

					// Has the whole message been sent successfully
					if (responseByteSize != ngcPosDialog.OutBuffer.Length) {
						PublishIncompleteDataSent(ngcPosDialog,responseByteSize);
						return;
					}
				}

				isOkay = true;

			} catch (ObjectDisposedException) {
				// this has been disposed in another thread
				isOkay = true;
			} catch (SocketException e) {
				PublishNetworkException(e);
			} catch (Exception e) {
				PublishSystemException(e);
			} finally {
				if (!isOkay) {
					this.Close();
				}
				trState.EndProc();
			}
		}
		#endregion

		#region Process Request
		protected bool PosGet(
			NgcPosDialog ngcPosDialog,
			int interfaceId,
			string cardAccountNumber,
			out string customerTitle,
			out string customerName,
			out string postalCode,
			out decimal pointsBalance,
			out decimal pointsConvertedBalance,
			out decimal welcomePoints,
			out string primaryCardAccountNumber,
			out string uniqueNumber,
			out decimal extraPoints1Balance,
			out decimal extraPoints2Balance,
			out decimal extraPoints3Balance,
			string storeCode,
			out string message,
			out int returnCode) {

			message = String.Empty;

			bool success = this.Parent.PosGet(
				interfaceId,
				cardAccountNumber,
				out customerTitle,
				out customerName,
				out postalCode,
				out pointsBalance,
				out pointsConvertedBalance,
				out welcomePoints,
				out primaryCardAccountNumber,
				out uniqueNumber,
				out extraPoints1Balance,
				out extraPoints2Balance,
				out extraPoints3Balance,
				storeCode,
				out returnCode);
			if (!success) {
				return false;
			}

			switch (returnCode) {
				case 0:
					return true;
				case 1: // New SkeletonCard
					message = newSkeletonCardString;
					customerName = defaultCustomerName; 
					return true;
				case 2: // Invalid Card Number
					message = invalidCardNoString;
					return true;
				case 3: // Card Account Closed
					message = closedCardString;
					customerName = String.Empty; 
					customerTitle = String.Empty;
					postalCode = String.Empty;
					return true;
				case 6: // Banned Customer
					message = bannedCustomerString;
					return true;
				case 9: // Deceased Customer
					message = deceasedCustomerString;
					return true;
				case 10: // Customer Left Scheme
					message = customerLeftSchemeString;
					return true;
				case 11: // Existing Skeleton
					message = existingSkeletonString;
					return true;
				case 12: // Address In Error
					message = addressErrorString;
					postalCode = String.Empty;
					return true;
				case 15: // Store Code Unknown
					PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
					return false;
				case 257: // Database Unavailable
					CrmServiceException ce257 = new CrmServiceException(
						"Server",
						"SqlError",
						"PosSocketsService.DatabaseUnavailable",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce257);
					return false;
				default:
					PublishDatabaseError(ngcPosDialog,"PosGet",returnCode);
					return false;
			}
		}

		protected bool PosSet(
			NgcPosDialog ngcPosDialog,
			int interfaceId,
			string cardAccountNumber,
			string customerName,
			string txnSourceCode,
			string txnTypeCode,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string cashierId,
			string amountSpent,
			string points,
			string welcomePoints,
			string skuPoints,
			string extraPointsType1,
			string extraPointsType2,
			string extraPointsType3,
			out int returnCode) 
		{

			bool success = this.Parent.PosSet(
				interfaceId,
				cardAccountNumber,
				customerName,
				txnSourceCode,
				txnTypeCode,
				storeCode,
				storeDate,
				posId,
				txnNbr,
				cashierId,
				amountSpent,
				points,
				welcomePoints,
				skuPoints,
				extraPointsType1,
				extraPointsType2,
				extraPointsType3,
				out returnCode);
			if (!success) {
				return false;
			}

			switch (returnCode) {
				case 0:	// Success
					return true;
				case 1: // Transaction already added
					CrmServiceException ce1 = new CrmServiceException(
						"Client",
						"ConfigurationError",
						"PosSocketsService.PosSet.DuplicateTransaction",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						storeCode,
						storeDate,
						posId,
						txnNbr,
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce1);
					return true;
				case 5: // Tried and failed to register new card account 
					CrmServiceException ce5 = new CrmServiceException(
						"Client",
						"ConfigurationError",
						"PosSocketsService.PosSet.InvalidCardNumber",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						cardAccountNumber,
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce5);
					return false;
				case 7: // Transaction Date is in the future
					CrmServiceException ce7 = new CrmServiceException(
						"Client",
						"ConfigurationError",
						"PosSocketsService.PosSet.FutureTransaction",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						storeCode,
						storeDate,
						posId,
						txnNbr,
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce7);
					return false;
				case 8: // Store Code Unknown
					PublishDataValueError(ngcPosDialog,"Store Code",storeCode);
					return false;
				case 257: // Database Unavailable
					CrmServiceException ce257 = new CrmServiceException(
						"Server",
						"SqlError",
						"PosSocketsService.DatabaseUnavailable",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce257);
					return false;
				case 258: // Points Limit Exceeded
					CrmServiceException ce258 = new CrmServiceException(
						"Server",
						"SqlError",
						"PosSocketsService.PosSet.PointsExceeded",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						cardAccountNumber,
						points,
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce258);
					return true;
				default:
					PublishDatabaseError(ngcPosDialog,"PosSet",returnCode);
					return false;
			}
		}


		protected bool PosVoucherRedeem(
			NgcPosDialog ngcPosDialog,
			int interfaceId,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string voucherValue,
			string voucherExpiryDate,
			out int returnCode) {

			int attempts = 1;
			int maxAttempts = 2;
			returnCode = 0;

			while (true) {

				bool success = this.Parent.PosVoucherRedeem(
					interfaceId,
					storeCode,
					storeDate,
					posId,
					txnNbr,
					voucherValue,
					voucherExpiryDate,
					out returnCode);

				if (!success) {
					return false;
				}

				if ((returnCode != 1) || (attempts == maxAttempts)) {
					break;
				}

				// Transaction is unknown this may be because the PosSet has not 
				// finished.  Wait and try once more.
				Thread.Sleep(parent.RetryVoucherRedeemTime);
				attempts++;
			}
	
			switch (returnCode) {
				case 0:	// Success
				case 3: // No Matching Voucher
					return true;
				case 1: // Transaction is unknown
					CrmServiceException ce = new CrmServiceException(
						"Client",
						"ConfigurationError",
						"PosSocketsService.PosVoucherRedeem.UnknownTransaction",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						storeCode,
						storeDate,
						posId,
						txnNbr,
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce);
					return false;
				case 257: // Database Unavailable
					CrmServiceException ce257 = new CrmServiceException(
						"Server",
						"SqlError",
						"PosSocketsService.DatabaseUnavailable",
						this.Parent.Trace,
						this.Parent.NgcIPAddress,
						this.Parent.NgcPort.ToString(),
						this.StoreIPAddress,
						this.StorePort.ToString(),
						FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
					ExceptionManager.Publish(ce257);
					return false;
				default: // Unknown Error
					PublishDatabaseError(ngcPosDialog,"PosVoucherRedeem",returnCode);
					return false;
			}
		}

		#endregion

		#region Buffer Methods
		private void WriteDebugBuffer(byte[] value, int length) {
			if (this.parent.Trace.TraceLevel.Debug) {
				this.parent.Trace.WriteDebug(FromHex(value,length));
			}
		}

		private static string FromHex(byte[] value, int length) {
			try {
				StringBuilder message = new StringBuilder();
				int column=20;
				int index=0;
				foreach (byte val in value) {

					if (index == length) {
						break;
					}
					index++;

					if (column==20) {
						message.Append("\n");
						column=0;
					} else {
						message.Append("-");
					}

					message.Append(val.ToString("x2"));
					column++;

				}
				return message.ToString();
			} catch {
				return String.Empty;
			}
		}
		#endregion

		#region Publish Methods
		private void PublishNetworkException(Exception e) {
			CrmServiceException ce = new CrmServiceException(
				"Network",
				"NetworkError",
				"PosSocketsService.SocketException",
				e,
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString());
			ExceptionManager.Publish(ce);
		}

		private void PublishTimeoutDataReceived(NgcPosDialog ngcPosDialog) {
			CrmServiceException ce = new CrmServiceException(
				"Network",
				"NetworkError",
				"PosSocketsService.TimeoutDataReceived",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				this.parent.TimeoutPeriod.ToString());
			ExceptionManager.Publish(ce);
		}

		private void PublishTimeoutDataSent(NgcPosDialog ngcPosDialog){
			CrmServiceException ce = new CrmServiceException(
				"Network",
				"NetworkError",
				"PosSocketsService.TimeoutDataSent",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				this.parent.TimeoutPeriod.ToString(),
				FromHex(ngcPosDialog.OutBuffer,ngcPosDialog.OutBuffer.Length));
			ExceptionManager.Publish(ce);
		}

		private void PublishIncompleteDataSent(NgcPosDialog ngcPosDialog, int responseByteSize){
			CrmServiceException ce = new CrmServiceException(
				"Network",
				"NetworkError",
				"PosSocketsService.IncompleteDataSent",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				ngcPosDialog.OutBuffer.Length.ToString(),
				responseByteSize.ToString(),
				FromHex(ngcPosDialog.OutBuffer,ngcPosDialog.OutBuffer.Length));
			ExceptionManager.Publish(ce);
		}

		protected void PublishDataFormatError(NgcPosDialog ngcPosDialog, string dataItem) {
			CrmServiceException ce = new CrmServiceException(
				"Client",
				"NetworkError",
				"PosSocketsService.DataFormatError",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				dataItem,
				FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
			ExceptionManager.Publish(ce);
		}

		protected void PublishDataValueError(NgcPosDialog ngcPosDialog, string dataItem, string dataValue) {
			CrmServiceException ce = new CrmServiceException(
				"Client",
				"ConfigurationError",
				"PosSocketsService.DataValueError",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				dataItem,
				dataValue,
				FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
			ExceptionManager.Publish(ce);
		}

		protected void PublishDatabaseError(NgcPosDialog ngcPosDialog, string sProcName, int returnCode) {
			CrmServiceException ce = new CrmServiceException(
				"Server",
				"SystemError",
				"PosSocketsService.DatabaseError",
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString(),
				sProcName,
				returnCode.ToString(),
				FromHex(ngcPosDialog.InBuffer,ngcPosDialog.Length));
			ExceptionManager.Publish(ce);
		}

		private void PublishSystemException(Exception e) {
			CrmServiceException ce = new CrmServiceException(
				"Server",
				"SystemError",
				"PosSocketsService.UnexpectedException",
				e,
				this.parent.Trace,
				this.parent.NgcIPAddress,
				this.parent.NgcPort.ToString(),
				this.storeIPAddress,
				this.storePort.ToString());
			ExceptionManager.Publish(ce);
		}
		#endregion

		#region Dispose
		private void Close() {
			try {
				this.parent.Remove(this);
				this.Dispose();
			} catch {
				// ignore
			}
		}

		public void Dispose() {
			try {
				this.Dispose(true);
				GC.SuppressFinalize(this);
			} catch {
				// ignore
			}
		}

		~NgcPosHandler() {
			try {
				this.Dispose(false);
			} catch {
				// ignore
			}
		}

		private void Dispose(bool disposing) {
			this.parent.Trace.WriteDebug("Closing Connection between NGC Port "+this.parent.NgcIPAddress+":"+this.parent.NgcPort+" and Store Port "+this.storeIPAddress+":"+this.storePort);
			this.parent.PosSocketsEventLog.WriteEntry("Closing Connection between NGC Port "+this.parent.NgcIPAddress+":"+this.parent.NgcPort+" and Store Port "+this.storeIPAddress+":"+this.storePort);
			this.Connection.Shutdown(SocketShutdown.Both);
			this.Connection.Close();
		}
		#endregion
	}
}