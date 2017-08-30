using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Fujitsu.eCrm.Generic.CrmService;
using Fujitsu.eCrm.Generic.SharedUtils;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	internal class PosListener : Socket, IPosListener {

		#region Attributes
		private readonly int ngcPort;
		private readonly string ngcIPAddress;
		private Hashtable storeTable;
		private ReaderWriterLock storeLock;
		private ArrayList messageList;	
		private ReaderWriterLock messageLock;
		private ArrayList handlerList;
		private ReaderWriterLock handlerLock;
		private AsyncCallback asyncCallback;
		#endregion

		#region Properties
		public ArrayList Messages {
			// MessageList needs to be synchronised, but the clone does not
			get { 
				ArrayList clone = null;
				messageLock.AcquireReaderLock(-1);
				try {
					clone = (ArrayList)this.messageList.Clone(); 
				} catch {
					clone = new ArrayList();
				} finally {
					messageLock.ReleaseReaderLock();
				}
				return clone;
			}
		}
		
		public ITrace Trace {
			get { return PosApplication.Trace; }
		}

		public EventLog PosSocketsEventLog {
			get { return PosApplication.PosSocketsEventLog; }
		}

		public string NgcIPAddress {
			get { return this.ngcIPAddress; }
		}

		public int NgcPort {
			get { return this.ngcPort; }
		}

		public int TimeoutPeriod {
			get { return PosApplication.TimeoutPeriod; }
		}

		public int RetryVoucherRedeemTime {
			get { return PosApplication.RetryVoucherRedeemTime; }
		}
		#endregion

		#region Constructor
		internal PosListener(IPEndPoint lep, int pendingBacklog, Hashtable storeTable, ArrayList messageList) : base(lep.Address.AddressFamily,SocketType.Stream,ProtocolType.Tcp) {
			ITraceState trState = PosApplication.Trace.StartProc("PosListener");
			try {
				// Setup listener
				this.ngcIPAddress = lep.Address.ToString();
				this.ngcPort = lep.Port;
				this.storeTable = storeTable;
				this.storeLock = new ReaderWriterLock();
				this.messageList = messageList;
				this.messageLock = new ReaderWriterLock();
				this.handlerList = new ArrayList();
				this.handlerLock = new ReaderWriterLock();
				this.asyncCallback = new AsyncCallback(this.CallbackAccept);
				this.Bind(lep);
				this.Listen(pendingBacklog);
				// Start listening
				PosApplication.PosSocketsEventLog.WriteEntry("Starting Listener on NGC Port "+this.ngcIPAddress+":"+this.ngcPort);
				PosApplication.Trace.WriteDebug("Starting Listener on NGC Port "+this.ngcIPAddress+":"+this.ngcPort);

				this.BeginAccept(this.asyncCallback,null);
			} catch (Exception e) {
				ExceptionManager.Publish(e);
			} finally {
				trState.EndProc();
			}
		}
		#endregion

		#region Handle Connection Request
		private void CallbackAccept(IAsyncResult ar) {
			ITraceState trState = PosApplication.Trace.StartProc("CallbackAccept");
			try {
				// Get the socket that handles the client connection
				Socket connection = this.EndAccept(ar);

				// Start listening for the next client connection
				this.BeginAccept(this.asyncCallback,null);

				string storeIPAddress = ((IPEndPoint)connection.RemoteEndPoint).Address.ToString();
				int storePort = ((IPEndPoint)connection.RemoteEndPoint).Port;
				PosApplication.Trace.WriteDebug("Listener "+this.ngcIPAddress+":"+this.ngcPort+" received connection request from "+storeIPAddress+":"+storePort);

				// Check the remote end point has a recognised Store IP Address
				this.storeLock.AcquireReaderLock(-1);
				bool isAcceptable;
				try {
					isAcceptable = this.storeTable.Contains(storeIPAddress);
				} finally {
					this.storeLock.ReleaseReaderLock();
				}

				// Close connection and throw exception if the remote end point
				// does not have a recognised Store IP Address
				if (!isAcceptable) {

					connection.Shutdown(SocketShutdown.Both);
					connection.Close();

					CrmServiceException ce = new CrmServiceException(
						"Client",
						"ConfigurationError",
						"PosSocketsService.UnSupportedStoreIPAddress",
						PosApplication.Trace,
						this.ngcIPAddress,
						this.ngcPort.ToString(),
						storeIPAddress,
						storePort.ToString());
					throw ce;
				}

				// Setup a connection handler
				this.handlerLock.AcquireWriterLock(-1);
				try {
					IPosHandler posHandler = PosApplication.ConstructIPosHandler(this,connection);
					this.handlerList.Add(posHandler);
				} finally {
					this.handlerLock.ReleaseWriterLock();
				}

			} catch (ObjectDisposedException) {
				// this object has been disposed by another thread
			} catch (SocketException e) {
				ExceptionManager.Publish(e);
				PosApplication.Trace.WriteDebug("Unexpected Socket Exception Closing Listener "+this.ngcIPAddress+":"+this.ngcPort);
				this.Dispose();
			} catch (Exception e) {
				ExceptionManager.Publish(e);
			} finally {
				trState.EndProc();
			}
		}
		#endregion

		#region Methods for PosListenerTable
		internal void Change(Hashtable storeTable, ArrayList messageList) {
			ITraceState trState = PosApplication.Trace.StartProc("Change");
			try {

				// The List of Supported Stores has changed.  Some of the existing
				// handles may be connected to stores that are no longer supported,
				// we need to dispose such handles
				// First lock the handle list and the store list to prevent any
				// further outside changes
				// Upgrade lock on store list to change to new version
				// Downgrade lock on store list, to allow other threads to read it
				// Compare contents of store list to handler list to create a list of
				// handles to dispose
				// Release lock on store list
				// If there are any handles to dispose then Upgrade lock on handle list,
				// and close the handles
				// Finally release locks on handle list
				this.handlerLock.AcquireReaderLock(-1);
				try {
					this.storeLock.AcquireReaderLock(-1);

					ArrayList disposeList = new ArrayList();
					try {

						LockCookie storeCookie = this.storeLock.UpgradeToWriterLock(-1);
						try {
							this.storeTable = storeTable;
						} finally {
							this.storeLock.DowngradeFromWriterLock(ref storeCookie);
						}

						foreach (IPosHandler posHandler in this.handlerList) {
							if (!this.storeTable.Contains(posHandler.StoreIPAddress)) {
								disposeList.Add(posHandler);								
							}
						}
					
					} finally {
						this.storeLock.ReleaseReaderLock();
					}

					if (disposeList.Count > 0) {
						LockCookie handlerCookie = this.handlerLock.UpgradeToWriterLock(-1);
						try {
							foreach (IPosHandler posHandler in disposeList) {
								this.handlerList.Remove(posHandler);
								posHandler.Dispose();
							}
						} finally {
							this.handlerLock.DowngradeFromWriterLock(ref handlerCookie);
						}
					}

				} finally {
					this.handlerLock.ReleaseReaderLock();
				}

				this.messageLock.AcquireWriterLock(-1);
				try {
					this.messageList = messageList;
				} finally {
					this.messageLock.ReleaseWriterLock();
				}

			} catch (Exception e) {
				ExceptionManager.Publish(e);
			} finally {
				trState.EndProc();
			}
		}
		#endregion

		#region Methods for PosHandlers
		#region Store Codes
		/// <summary>
		/// Return a list of store codes that the connection should only support
		/// </summary>
		/// <param name="storeIPAddress">The connection's Remote IP Address</param>
		/// <returns>A cloned array list of supported store codes</returns>
		public ArrayList StoreCodes(string storeIPAddress) {
			ArrayList clone = null;
			storeLock.AcquireReaderLock(-1);
			try {
				ArrayList storeCodeList = (ArrayList)this.storeTable[storeIPAddress];
				clone = (ArrayList)storeCodeList.Clone(); 
			} catch {
				clone = new ArrayList();
			} finally {
				storeLock.ReleaseReaderLock();
			}
			return clone;
		}
		#endregion

		#region Remove Handler
		/// <summary>
		/// The connection handler is closing, so remove the handler
		/// from the list of live connection handlers
		/// </summary>
		/// <param name="posHandler"></param>
		public void Remove(IPosHandler posHandler) {
			this.handlerLock.AcquireWriterLock(-1);
			try {
				this.handlerList.Remove(posHandler);
			} catch {
				// ignore
			} finally {
				this.handlerLock.ReleaseWriterLock();
			}
		}
		#endregion

		#region Pos Get
		/// <summary>
		/// PosGet receives card number, and responds with customer details
		/// </summary>
		/// <param name="interfaceId"></param>
		/// <param name="cardAccountNumber">The number of the card that has been swiped</param>
		/// <param name="customerTitle"></param>
		/// <param name="customerName">The name of the card's primary customer</param>
		/// <param name="postalCode"></param>
		/// <param name="pointsBalance">The points balance of the card's household</param>
		/// <param name="pointsConvertedBalance">The number of points converted since previous visit</param>
		/// <param name="welcomePoints">If this first time the card is used then specify x points to be added to the card account</param>
		/// <param name="primaryCardAccountNumber">The primary card of the householder</param>
		/// <param name="uniqueNumber">The customers unique number</param>
		/// <param name="extraPoints1Balance">The points balance for extra points type 1</param>
		/// <param name="extraPoints2Balance">The points balance for extra points type 1</param>
		/// <param name="extraPoints3Balance">The points balance for extra points type 1</param>
		/// <param name="storeCode">The store code that is sending the messge</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error or warning</param>
		/// <returns>Critical error has occurred</returns>
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosGets","Count of Requested CRM Server PosGets",PerformanceCounterType.NumberOfItems32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosGets","Time (Milliseconds) Taken During Requested CRM Server PosGets",PerformanceCounterType.NumberOfItems64)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosGets Per Second","Count of Requested CRM Server PosGets Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosGets","Average Time (Milliseconds) Taken During Requested CRM Server PosGets","_Number of Requested PosGets","",PerformanceCounterType.AverageCount64)]
		public bool PosGet(
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
			out int returnCode) 
		{

			// Set up timer
			HiResTimer timer = new HiResTimer();
			timer.Start();

			// Set up default values for output parameters
			customerTitle = String.Empty;
			customerName = String.Empty;
			postalCode = String.Empty;
			pointsBalance = 0;
			pointsConvertedBalance = 0;
			welcomePoints = 0;
			primaryCardAccountNumber = cardAccountNumber;
			uniqueNumber = "";
			extraPoints1Balance = 0;
			extraPoints2Balance = 0;
			extraPoints3Balance = 0;

			returnCode = 256;

			// Set up tracing
			ITraceState trState = PosApplication.Trace.StartProc("PosGet");

			try {

				if (PosApplication.PosGetSProc == null) {
					CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosGet");
					throw ce;
				}

				SqlConnection connection = new SqlConnection(Convert.ToString(XmlApplication.ConfigurationTable["AdminConnectionString"]));
				connection.Open();
				try {
					#region Execute SProc
					SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

					object[] paramList = {
											 interfaceId,
											 XmlApplication.ConfigurationTable["CultureDefault"],
											 cardAccountNumber,
											 PosApplication.InformConversionTxnThreshold,
											 null,	// place holder for out primaryCardAccountNumber
											 null,  // place holder for out welcomeFlag
											 null,  // place holder for out customerName
											 null,	// place holder for out customerTitle
											 null,	// place holder for out postalCode
											 null,	// place holder for out pointsBalance
											 null,  // place holder for out pointsConvertedBalance
											 null,	// place holder for out uniqueNumber
											 null,	// place holder for out welcomePoints
											 storeCode,
											 null,	// place holder for out extra points balance 1
											 null,	// place holder for out extra points balance 2
											 null};	// place holder for out extra points balance 3


					returnCode = PosApplication.PosGetSProc.Execute(
						transaction,
						ref paramList);

					// Commit uynconditionally - it is a read-only transaction

					transaction.Commit();

					// Handle SProc output parameters
					if (!(paramList[4] is System.DBNull)) {
						primaryCardAccountNumber = Convert.ToString(paramList[4]);
					}
					//if (!(paramList[5] is System.DBNull)) {
					//	welcomedFlag = Convert.ToString(paramList[5]);
					//}
					if (!(paramList[6] is System.DBNull)) {
						customerName = Convert.ToString(paramList[6]);
					}
					if (!(paramList[7] is System.DBNull)) {
						customerTitle = Convert.ToString(paramList[7]);
					}
					if (!(paramList[8] is System.DBNull)) {
						postalCode = Convert.ToString(paramList[8]);
					}
					if (!(paramList[9] is System.DBNull)) {
						pointsBalance = Convert.ToDecimal(paramList[9]);
					}
					if (!(paramList[10] is System.DBNull)) 
					{
						pointsConvertedBalance = Convert.ToDecimal(paramList[10]);
					}
					
					// Handle SProc output parameters
					if (!(paramList[11] is System.DBNull)) 
					{
						uniqueNumber = Convert.ToString(paramList[11]);
					}

					if (!(paramList[12] is System.DBNull)) 
					{
						welcomePoints = Convert.ToDecimal(paramList[12]);
					}
					
					// Store code input only in position 13

					if (!(paramList[14] is System.DBNull)) 
					{
						extraPoints1Balance = Convert.ToDecimal(paramList[14]);
					}

					if (!(paramList[15] is System.DBNull)) 
					{
						extraPoints2Balance = Convert.ToDecimal(paramList[15]);
					}

					if (!(paramList[16] is System.DBNull)) 
					{
						extraPoints3Balance = Convert.ToDecimal(paramList[16]);
					}
					#endregion
				} finally {
					connection.Close(); // rollback any pending transactions
				}

				return true;
			} catch (SqlException e) {
				if (e.Number == 11) {
					returnCode = 257;
					return true;
				}
				ExceptionManager.Publish(e);
				return false;
			} catch (Exception e) {
				ExceptionManager.Publish(e);
				return false;
			} finally {
				timer.Stop();
				XmlApplication.PerfMon.Increment("Overall Number of Requested PosGets");
				XmlApplication.PerfMon.IncrementBy("Overall Time(ms) of Requested PosGets",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("Requested PosGets Per Second");
				XmlApplication.PerfMon.IncrementBy("Average Time(ms) of Requested PosGets",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("_Number of Requested PosGets");
				trState.EndProc();
			}
		}
		#endregion

		#region Pos Set
		/// <summary>
		/// PosSet receives card and txn details, it updates card and household accounts
		/// </summary>
		/// <param name="interfaceId"></param>
		/// <param name="cardAccountNumber">The number of the card that has been swiped</param>
		/// <param name="customerName">The default nameto use when creatinga skeleton customer</param>
		/// <param name="txnSourceCode">The type of POS</param>
		/// <param name="txnTypeCode">The type of Transaction (normal, cancelled, returned)</param>
		/// <param name="storeCode">The store where the card is used</param>
		/// <param name="storeDate">The date &amp; time of the transaction</param>
		/// <param name="posId">The number of the POS terminal</param>
		/// <param name="txnNbr">The transaction number of the POS terminal</param>
		/// <param name="cashierId">The operator behind the POS</param>
		/// <param name="amountSpent">The amount spent by the customer</param>
		/// <param name="points">The number of standard points to award the customer</param>
		/// <param name="welcomePoints">The number of welcome points to award the customer, e.g. welcome points</param>
		/// <param name="skuPoints">The number of SKU points to award the customer, e.g. points for buying a product</param>
		/// <param name="extraPointsType1">The number of extra points (culture specific?) to award the customer, e.g. points for buying a product</param>
		/// <param name="extraPointsType2">The number of extra points (culture specific?) to award the customer, e.g. points for buying a product</param>
		/// <param name="extraPointsType3">The number of extra points (culture specific?) to award the customer, e.g. points for buying a product</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error</param>
		/// <returns>Critical error has occurred</returns>
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosSets","Count of Requested CRM Server PosSets",PerformanceCounterType.NumberOfItems32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosSets","Time (Milliseconds) Taken During Requested CRM Server PosSets",PerformanceCounterType.NumberOfItems64)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosSets Per Second","Count of Requested CRM Server PosSets Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosSets","Average Time (Milliseconds) Taken During Requested CRM Server PosSets","_Number of Requested PosSets","",PerformanceCounterType.AverageCount64)]
		public bool PosSet(
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

			// Set up timer
			HiResTimer timer = new HiResTimer();
			timer.Start();

			// Set up default values for output parameters
			returnCode = 256;

			txnNbr= txnNbr.PadLeft(5,'0');

			// Set up tracing
			ITraceState trState = PosApplication.Trace.StartProc("XmlApplication.PosSet");

			try {

				if (PosApplication.PosSetSProc == null) {
					CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosSet");
					throw ce;
				}

				SqlConnection connection = new SqlConnection(Convert.ToString(XmlApplication.ConfigurationTable["AdminConnectionString"]));
				connection.Open();
				try {
					#region Add Points
					SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

					object[] paramList = {
											 interfaceId,
											 PosApplication.UserId,
											 PosApplication.SessionId,
											 cardAccountNumber,
											 customerName,
											 txnSourceCode,
											 txnTypeCode,
											 storeCode,
											 storeDate,
											 PosApplication.FutureTime,
											 posId,
											 txnNbr,
											 cashierId,
											 amountSpent,
											 points,
											 welcomePoints,
											 skuPoints,
											 extraPointsType1,
											 extraPointsType2,
											 extraPointsType3};

					returnCode = PosApplication.PosSetSProc.Execute(
						transaction,
						ref paramList);

					// Handle return codes of SProc
					if (returnCode == 0) {
						// Successful returned details
						transaction.Commit();
					} else {
						transaction.Rollback();
					}
					#endregion
				} finally {
					connection.Close(); // rollback any pending transactions
				}
				return true;
			} catch (CrmServiceException e) {
				// Check if exception is caused by numeric overflow
				if (e.InnerException != null) {
					if (e.InnerException is SqlException) {
						SqlException se = (SqlException)e.InnerException;
						if (se.Number == 8115) {
							returnCode = 258;
							return true;
						}
					}
				}
				ExceptionManager.Publish(e);
				return false;
			} catch (SqlException e) {
				if (e.Number == 11) {
					returnCode = 257;
					return true;
				}
				ExceptionManager.Publish(e);
				return false;
			} catch (Exception e) {
				ExceptionManager.Publish(e);
				return false;
			} finally {
				timer.Stop();
				XmlApplication.PerfMon.Increment("Overall Number of Requested PosSets");
				XmlApplication.PerfMon.IncrementBy("Overall Time(ms) of Requested PosSets",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("Requested PosSets Per Second");
				XmlApplication.PerfMon.IncrementBy("Average Time(ms) of Requested PosSets",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("_Number of Requested PosSets");
				trState.EndProc();
			}
		}
		#endregion

		#region Pos Voucher Redeem
		/// <summary>
		/// Update Card Account and Customer with details of vouchers redeemed
		/// </summary>
		/// <param name="interfaceId"></param>
		/// <param name="storeCode">The store where the card is used</param>
		/// <param name="storeDate">The date at the store where the card is used</param>
		/// <param name="posId">The number of the POS terminal</param>
		/// <param name="txnNbr">The transaction number of the POS terminal</param>
		/// <param name="voucherValue">The value of the voucher being redeemed</param>
		/// <param name="voucherExpiryDate">The expiry on the voucher being redeemed</param>
		/// <param name="returnCode">The Status of PosGet, 0=success, otherwise an error</param>
		/// <returns>Critical error has occurred</returns>
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosVoucherRedeems","Count of Requested CRM Server PosVoucherRedeems",PerformanceCounterType.NumberOfItems32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosVoucherRedeems","Time (Milliseconds) Taken During Requested CRM Server PosVoucherRedeems",PerformanceCounterType.NumberOfItems64)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosVoucherRedeems Per Second","Count of Requested CRM Server PosVoucherRedeems Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
		[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosVoucherRedeems","Average Time (Milliseconds) Taken During Requested CRM Server PosVoucherRedeems","_Number of Requested PosVoucherRedeems","",PerformanceCounterType.AverageCount64)]
		public bool PosVoucherRedeem(
			int interfaceId,
			string storeCode,
			string storeDate,
			string posId,
			string txnNbr,
			string voucherValue,
			string voucherExpiryDate,
			out int returnCode) {

			// Set up timer
			HiResTimer timer = new HiResTimer();
			timer.Start();

			// Set up default values for output parameters
			returnCode = 256;
			txnNbr= txnNbr.PadLeft(5,'0');


			// Set up tracing
			ITraceState trState = PosApplication.Trace.StartProc("XmlApplication.PosVoucherRedeem");

			try {

				if (PosApplication.PosVoucherRedeemSProc == null) {
					CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosVoucherRedeem");
					throw ce;
				}

				SqlConnection connection = new SqlConnection(Convert.ToString(XmlApplication.ConfigurationTable["AdminConnectionString"]));
				connection.Open();
				try {
					#region Add Points
					SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

					object[] paramList = {
											 storeCode,
											 storeDate,
											 posId,
											 txnNbr,
											 voucherValue,
											 voucherExpiryDate};

					returnCode = PosApplication.PosVoucherRedeemSProc.Execute(
						transaction,
						ref paramList);

					// Handle return codes of SProc
					switch (returnCode) {
						case 0:
						case 3:
							transaction.Commit();
							break;
						default:
							transaction.Rollback();
							break;
					}
					#endregion
				} finally {
					connection.Close(); // rollback any pending transactions
				}

				return true;
			} catch (SqlException e) {
				if (e.Number == 11) {
					returnCode = 257;
					return true;
				}
				ExceptionManager.Publish(e);
				return false;
			} catch (Exception e) {
				ExceptionManager.Publish(e);
				return false;
			} finally {
				timer.Stop();
				XmlApplication.PerfMon.Increment("Overall Number of Requested PosVoucherRedeems");
				XmlApplication.PerfMon.IncrementBy("Overall Time(ms) of Requested PosVoucherRedeems",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("Requested PosVoucherRedeems Per Second");
				XmlApplication.PerfMon.IncrementBy("Average Time(ms) of Requested PosVoucherRedeems",Convert.ToInt64(timer.ElapsedMilliseconds));
				XmlApplication.PerfMon.Increment("_Number of Requested PosVoucherRedeems");
				trState.EndProc();
			}
		}
		#endregion
		#endregion

		#region Dispose Listener and Handler(s)
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~PosListener() {
			this.Dispose(false);
		}

		private new void Dispose(bool disposing) {
			try {
				PosApplication.Trace.WriteDebug("Closing Listener NGC Port "+this.ngcIPAddress+":"+this.ngcPort);
				PosApplication.PosSocketsEventLog.WriteEntry("Closing Listener on NGC Port "+this.ngcIPAddress+":"+this.ngcPort);
				if (disposing) {
					lock (this.handlerList) {
						foreach (IPosHandler posHandler in handlerList) {
							posHandler.Dispose();
						}
					}
				}
				this.Shutdown(SocketShutdown.Both);
				this.Close();
			} catch {
				// already shutdown and closed
			}
			base.Dispose(disposing);
		}
		#endregion

	}
}