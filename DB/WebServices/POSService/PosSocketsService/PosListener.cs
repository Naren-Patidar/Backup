using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using Fujitsu.eCrm.Generic.CrmService;
using Fujitsu.eCrm.Generic.SharedUtils;
//using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;



namespace Fujitsu.eCrm.Seoul.PosSocketsService
{

    internal class PosListener : Socket, IPosListener
    {

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
        public ArrayList Messages
        {
            // MessageList needs to be synchronised, but the clone does not
            get
            {
                ArrayList clone = null;
                messageLock.AcquireReaderLock(-1);
                try
                {
                    clone = (ArrayList)this.messageList.Clone();
                }
                catch
                {
                    clone = new ArrayList();
                }
                finally
                {
                    messageLock.ReleaseReaderLock();
                }
                return clone;
            }
        }

        public ITrace Trace
        {
            get { return PosApplication.Trace; }
        }

        public EventLog PosSocketsEventLog
        {
            get { return PosApplication.PosSocketsEventLog; }
        }

        public string NgcIPAddress
        {
            get { return this.ngcIPAddress; }
        }

        public int NgcPort
        {
            get { return this.ngcPort; }
        }

        public int TimeoutPeriod
        {
            get { return PosApplication.TimeoutPeriod; }
        }

        public int RetryVoucherRedeemTime
        {
            get { return PosApplication.RetryVoucherRedeemTime; }
        }
        #endregion

        #region Constructor
        internal PosListener(IPEndPoint lep, int pendingBacklog, Hashtable storeTable, ArrayList messageList)
            : base(lep.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        {
            ITraceState trState = PosApplication.Trace.StartProc("PosListener");
            try
            {

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
                PosApplication.PosSocketsEventLog.WriteEntry("Starting Listener on NGC Port " + this.ngcIPAddress + ":" + this.ngcPort);
                PosApplication.Trace.WriteDebug("Starting Listener on NGC Port " + this.ngcIPAddress + ":" + this.ngcPort);


                this.BeginAccept(this.asyncCallback, null);
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb= new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
            }
            finally
            {
                trState.EndProc();
            }
        }
        #endregion

        #region Handle Connection Request
        private void CallbackAccept(IAsyncResult ar)
        {
            ITraceState trState = PosApplication.Trace.StartProc("CallbackAccept");
            try
            {
                // Get the socket that handles the client connection
                Socket connection = this.EndAccept(ar);

                // Start listening for the next client connection
                this.BeginAccept(this.asyncCallback, null);

                string storeIPAddress = ((IPEndPoint)connection.RemoteEndPoint).Address.ToString();
                int storePort = ((IPEndPoint)connection.RemoteEndPoint).Port;
                PosApplication.Trace.WriteDebug("Listener " + this.ngcIPAddress + ":" + this.ngcPort + " received connection request from " + storeIPAddress + ":" + storePort);


                // Check the remote end point has a recognised Store IP Address
                this.storeLock.AcquireReaderLock(-1);
                bool isAcceptable;
                try
                {
                    isAcceptable = this.storeTable.Contains(storeIPAddress);
                }
                finally
                {
                    this.storeLock.ReleaseReaderLock();
                }

                // Close connection and throw exception if the remote end point
                // does not have a recognised Store IP Address
                if (!isAcceptable)
                {

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
                try
                {

                    IPosHandler posHandler = PosApplication.ConstructIPosHandler(this, connection);
                    this.handlerList.Add(posHandler);
                }
                finally
                {
                    this.handlerLock.ReleaseWriterLock();
                }

            }
            catch (ObjectDisposedException)
            {
                // this object has been disposed by another thread
            }
            catch (SocketException e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                PosApplication.Trace.WriteDebug("Unexpected Socket Exception Closing Listener " + this.ngcIPAddress + ":" + this.ngcPort);
                this.Dispose();
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
               //ExceptionManager.Publish(e);
            }
            finally
            {
                trState.EndProc();
            }
        }
        #endregion

        #region Methods for PosListenerTable
        internal void Change(Hashtable storeTable, ArrayList messageList)
        {
            ITraceState trState = PosApplication.Trace.StartProc("Change");
            try
            {

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
                try
                {
                    this.storeLock.AcquireReaderLock(-1);

                    ArrayList disposeList = new ArrayList();
                    try
                    {

                        LockCookie storeCookie = this.storeLock.UpgradeToWriterLock(-1);
                        try
                        {
                            this.storeTable = storeTable;
                        }
                        finally
                        {
                            this.storeLock.DowngradeFromWriterLock(ref storeCookie);
                        }

                        foreach (IPosHandler posHandler in this.handlerList)
                        {
                            if (!this.storeTable.Contains(posHandler.StoreIPAddress))
                            {
                                disposeList.Add(posHandler);
                            }
                        }

                    }
                    finally
                    {
                        this.storeLock.ReleaseReaderLock();
                    }

                    if (disposeList.Count > 0)
                    {
                        LockCookie handlerCookie = this.handlerLock.UpgradeToWriterLock(-1);
                        try
                        {
                            foreach (IPosHandler posHandler in disposeList)
                            {
                                this.handlerList.Remove(posHandler);
                                posHandler.Dispose();
                            }
                        }
                        finally
                        {
                            this.handlerLock.DowngradeFromWriterLock(ref handlerCookie);
                        }
                    }

                }
                finally
                {
                    this.handlerLock.ReleaseReaderLock();
                }

                this.messageLock.AcquireWriterLock(-1);
                try
                {
                    this.messageList = messageList;
                }
                finally
                {
                    this.messageLock.ReleaseWriterLock();
                }

            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
            }
            finally
            {
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
        public ArrayList StoreCodes(string storeIPAddress)
        {
            ArrayList clone = null;
            storeLock.AcquireReaderLock(-1);
            try
            {
                ArrayList storeCodeList = (ArrayList)this.storeTable[storeIPAddress];
                clone = (ArrayList)storeCodeList.Clone();
            }
            catch
            {
                clone = new ArrayList();
            }
            finally
            {
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
        public void Remove(IPosHandler posHandler)
        {
            this.handlerLock.AcquireWriterLock(-1);
            try
            {
                this.handlerList.Remove(posHandler);
            }
            catch
            {
                // ignore
            }
            finally
            {
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
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosGets","Count of Requested CRM Server PosGets",PerformanceCounterType.NumberOfItems32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosGets","Time (Milliseconds) Taken During Requested CRM Server PosGets",PerformanceCounterType.NumberOfItems64)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosGets Per Second","Count of Requested CRM Server PosGets Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosGets","Average Time (Milliseconds) Taken During Requested CRM Server PosGets","_Number of Requested PosGets","",PerformanceCounterType.AverageCount64)]
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
            string welcomedFlag = string.Empty;

            long cardAccNumber = long.Parse(cardAccountNumber);
            int tescoStoreID = int.Parse(storeCode);


            returnCode = 256;

            // Set up tracing
            ITraceState trState = PosApplication.Trace.StartProc("PosGet");

            try
            {

                //if (PosApplication.PosGetSProc == null) {
                //    CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosGet");
                //    throw ce;
                //}

                SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
                connection.Open();
                try
                {
                    #region Execute SProc
                    //SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                    string culture = ConfigurationSettings.AppSettings["CultureDefault"];

                    //object[] paramList = {
                    //                         interfaceId,
                    //                         XmlApplication.ConfigurationTable["CultureDefault"],
                    //                         cardAccountNumber,
                    //                         PosApplication.InformConversionTxnThreshold,
                    //                         null,	// place holder for out primaryCardAccountNumber
                    //                         null,  // place holder for out welcomeFlag
                    //                         null,  // place holder for out customerName
                    //                         null,	// place holder for out customerTitle
                    //                         null,	// place holder for out postalCode
                    //                         null,	// place holder for out pointsBalance
                    //                         null,  // place holder for out pointsConvertedBalance
                    //                         null,	// place holder for out uniqueNumber
                    //                         null,	// place holder for out welcomePoints
                    //                         storeCode,
                    //                         null,	// place holder for out extra points balance 1
                    //                         null,	// place holder for out extra points balance 2
                    //                         null};	// place holder for out extra points balance 3


                    //returnCode = PosApplication.PosGetSProc.Execute(
                    //    transaction,
                    //    ref paramList);

                    //Changes done to suit the new Database Model -- Avoiding Barcelona Architecture
                    returnCode = 0;
                    SqlParameter[] paramList = new SqlParameter[14];

                    #region Execute SProc
                    SqlCommand cmdObj = new SqlCommand();

                    cmdObj.CommandType = CommandType.StoredProcedure;
                    cmdObj.CommandText = "USP_POS_GET";
                    paramList[0] = BuildParameter("@InterfaceCode", SqlDbType.Int, 50, ParameterDirection.Input, interfaceId);
                    paramList[1] = BuildParameter("@ISOLanguageCode", SqlDbType.VarChar, 4, ParameterDirection.Input, culture);
                    paramList[2] = BuildParameter("@ClubcardID", SqlDbType.VarChar, 20, ParameterDirection.Input, cardAccNumber);
                    //paramList[3] = BuildParameter("@TescoStoreID", SqlDbType.Int, 20, ParameterDirection.Input, PosApplication.InformConversionTxnThreshold);
                    paramList[3] = BuildParameter("@PrimaryClubcardID", SqlDbType.BigInt, 20, ParameterDirection.Output, primaryCardAccountNumber);
                    //paramList[5] = BuildParameter("@customer_welcomed_flag", SqlDbType.VarChar, 1, ParameterDirection.Output, welcomedFlag);
                    paramList[4] = BuildParameter("@Name1", SqlDbType.NVarChar, 100, ParameterDirection.Output, customerName);
                    paramList[5] = BuildParameter("@LookupTitlePhrase", SqlDbType.NVarChar, 50, ParameterDirection.Output, customerTitle);
                    paramList[6] = BuildParameter("@MailingAddressPostCode", SqlDbType.NVarChar, 20, ParameterDirection.Output, postalCode);
                    paramList[7] = BuildParameter("@TotalPointsBalance", SqlDbType.Int, 15, ParameterDirection.Output, pointsBalance);
                    paramList[8] = BuildParameter("@PointsConvertedToRewards", SqlDbType.BigInt, 15, ParameterDirection.Output, pointsConvertedBalance);
                    paramList[9] = BuildParameter("@WelcomePointsQty", SqlDbType.Int, 15, ParameterDirection.Output, welcomePoints);
                    paramList[10] = BuildParameter("@TescoStoreID", SqlDbType.Int, 5, ParameterDirection.Input, tescoStoreID);
                    paramList[11] = BuildParameter("@BonusPointsQty", SqlDbType.Decimal, 15, ParameterDirection.Output, extraPoints1Balance);
                    //paramList[15] = BuildParameter("@extra_points_2_balance", SqlDbType.Decimal, 15, ParameterDirection.Output, extraPoints2Balance);
                    paramList[12] = BuildParameter("@PartnerPointsBalanceQty", SqlDbType.Decimal, 15, ParameterDirection.Output, extraPoints3Balance);
                    paramList[13] = BuildParameter("@UniqueNumber", SqlDbType.VarChar, 18, ParameterDirection.Output, uniqueNumber);
                    cmdObj.Connection = connection;
                    //cmdObj.Transaction = transaction;

                    cmdObj.Parameters.Clear();
                    foreach (SqlParameter param in paramList)
                    {
                        cmdObj.Parameters.Add(param);
                    }
                    SqlParameter returnParameter = cmdObj.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    try
                    {
                        cmdObj.ExecuteNonQuery();
                        returnCode = int.Parse(cmdObj.Parameters["@return_value"].Value.ToString());
                    }
                    catch (Exception e)
                    {
                        Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                        wb.ProcessException(e);
                        //ExceptionManager.Publish(e);
                    }
                    #endregion

                    //End of Changes

                    // Commit uynconditionally - it is a read-only transaction

                    //transaction.Commit();

                    // Handle SProc output parameters
                    if (!(paramList[3] is System.DBNull))
                    {
                        primaryCardAccountNumber = Convert.ToString(paramList[3].Value);
                    }
                    //if (!(paramList[5] is System.DBNull)) {
                    //	welcomedFlag = Convert.ToString(paramList[5]);
                    //}
                    if (!(paramList[4] is System.DBNull))
                    {
                        customerName = Convert.ToString(paramList[4].Value);
                    }
                    if (!(paramList[5] is System.DBNull))
                    {
                        customerTitle = Convert.ToString(paramList[5].Value);
                    }
                    if (!(paramList[6] is System.DBNull))
                    {
                        postalCode = Convert.ToString(paramList[6].Value);
                    }
                    if (!(paramList[7] is System.DBNull))
                    {
                        pointsBalance = Convert.ToDecimal(paramList[7].Value);
                    }
                    if (!(paramList[8] is System.DBNull))
                    {
                        pointsConvertedBalance = Convert.ToDecimal(paramList[8].Value);
                    }

                    // Handle SProc output parameters
                    if (!(paramList[13] is System.DBNull))
                    {
                        uniqueNumber = Convert.ToString(paramList[13].Value);
                    }

                    if (!(paramList[9] is System.DBNull))
                    {
                        welcomePoints = Convert.ToDecimal(paramList[9].Value);
                    }

                    // Store code input only in position 13

                    if (!(paramList[11] is System.DBNull))
                    {
                        extraPoints1Balance = Convert.ToDecimal(paramList[11].Value);
                    }

                    if (!(paramList[12] is System.DBNull))
                    {
                        extraPoints3Balance = Convert.ToDecimal(paramList[12].Value);
                    }
                    #endregion
                }
                finally
                {
                    connection.Close(); // rollback any pending transactions
                }

                return true;
            }
            catch (SqlException e)
            {
                if (e.Number == 11)
                {
                    returnCode = 257;
                    return true;
                }
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
               // ExceptionManager.Publish(e);
                return false;
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            finally
            {
                timer.Stop();
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
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosSets","Count of Requested CRM Server PosSets",PerformanceCounterType.NumberOfItems32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosSets","Time (Milliseconds) Taken During Requested CRM Server PosSets",PerformanceCounterType.NumberOfItems64)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosSets Per Second","Count of Requested CRM Server PosSets Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosSets","Average Time (Milliseconds) Taken During Requested CRM Server PosSets","_Number of Requested PosSets","",PerformanceCounterType.AverageCount64)]
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

            txnNbr = txnNbr.PadLeft(5, '0');

            //Parse the Parameters
            long cardAccNumber = long.Parse(cardAccountNumber);
            int userID = 1;
            int transactionType = int.Parse(txnTypeCode);
            int tescoStoreID = int.Parse(storeCode);
            int sourcePOSID = int.Parse(posId);
            int sourceSystemTransactionID = int.Parse(txnNbr);
            decimal amtSpent = decimal.Parse(amountSpent);
            int totalPointsQty = int.Parse(points);
            int welcomePointsQty = int.Parse(welcomePoints);
            int skuPointsQty = int.Parse(skuPoints);
            int bonusPointsQty = int.Parse(extraPointsType1);
            int partnerPointsBalanceQty = int.Parse(extraPointsType3);

            System.DateTime txnDt = DateTime.Parse(storeDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat);
            //End of addition

            // Set up tracing
            ITraceState trState = PosApplication.Trace.StartProc("XmlApplication.PosSet");

            try
            {

                //if (PosApplication.PosSetSProc == null) {
                //    CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosSet");
                //    throw ce;
                //}

                SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
                connection.Open();
                try
                {
                    #region Add Points
                    //SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

                    //object[] paramList = {
                    //                         interfaceId,
                    //                         PosApplication.UserId,
                    //                         PosApplication.SessionId,
                    //                         cardAccountNumber,
                    //                         customerName,
                    //                         txnSourceCode,
                    //                         txnTypeCode,
                    //                         storeCode,
                    //                         storeDate,
                    //                         PosApplication.FutureTime,
                    //                         posId,
                    //                         txnNbr,
                    //                         cashierId,
                    //                         amountSpent,
                    //                         points,
                    //                         welcomePoints,
                    //                         skuPoints,
                    //                         extraPointsType1,
                    //                         extraPointsType2,
                    //                         extraPointsType3};

                    //returnCode = PosApplication.PosSetSProc.Execute(
                    //    transaction,
                    //    ref paramList);

                    returnCode = 0;
                    SqlParameter[] paramList = new SqlParameter[18];

                    #region Execute SProc
                    SqlCommand cmdObj = new SqlCommand();
                    //Fetch the Default Data Protection Preference --V3.1[Req ID - 007]
                    Int16 defaultDataProtPref = Convert.ToInt16(ConfigurationSettings.AppSettings["DefaultDataProtectionPreference"].ToString());

                    cmdObj.CommandType = CommandType.StoredProcedure;
                    cmdObj.CommandText = "USP_POS_SET";
                    paramList[0] = BuildParameter("@InterfaceCode", SqlDbType.Int, 50, ParameterDirection.Input, interfaceId);
                    paramList[1] = BuildParameter("@UserId", SqlDbType.SmallInt, 7, ParameterDirection.Input, userID);
                    //paramList[2] = BuildParameter("@session_crmid", SqlDbType.VarChar, 20, ParameterDirection.Input, PosApplication.SessionId);
                    paramList[2] = BuildParameter("@ClubcardId", SqlDbType.BigInt, 20, ParameterDirection.Input, cardAccNumber);
                    paramList[3] = BuildParameter("@CustomerName", SqlDbType.VarChar, 100, ParameterDirection.Input, customerName);
                    // paramList[5] = BuildParameter("@txn_source_code", SqlDbType.VarChar, 1, ParameterDirection.Input, txnSourceCode);
                    paramList[4] = BuildParameter("@TransactionType", SqlDbType.TinyInt, 50, ParameterDirection.Input, transactionType);
                    paramList[5] = BuildParameter("@TescoStoreID", SqlDbType.Int, 50, ParameterDirection.Input, tescoStoreID);
                    paramList[6] = BuildParameter("@TransactionDateTime", SqlDbType.DateTime, 6, ParameterDirection.Input, txnDt);
                    paramList[7] = BuildParameter("@FutureTime", SqlDbType.Int, 15, ParameterDirection.Input, PosApplication.FutureTime);
                    paramList[8] = BuildParameter("@SourcePOSID", SqlDbType.SmallInt, 15, ParameterDirection.Input, sourcePOSID);
                    paramList[9] = BuildParameter("@SourceSystemTransactionID", SqlDbType.Int, 18, ParameterDirection.Input, sourceSystemTransactionID);
                    paramList[10] = BuildParameter("@CashierID", SqlDbType.NVarChar, 20, ParameterDirection.Input, cashierId);
                    paramList[11] = BuildParameter("@AmountSpent", SqlDbType.Decimal, 17, ParameterDirection.Input, amtSpent);
                    paramList[12] = BuildParameter("@TotalPointsQty", SqlDbType.Int, 15, ParameterDirection.Input, totalPointsQty);
                    paramList[13] = BuildParameter("@WelcomePointsQty", SqlDbType.Int, 15, ParameterDirection.Input, welcomePointsQty);
                    paramList[14] = BuildParameter("@SKUPointsQty", SqlDbType.Int, 15, ParameterDirection.Input, skuPointsQty);
                    //paramList[17] = BuildParameter("@extra_points_1", SqlDbType.Decimal, 15, ParameterDirection.Input, extraPointsType1);
                    paramList[15] = BuildParameter("@BonusPointsQty", SqlDbType.Int, 15, ParameterDirection.Input, bonusPointsQty);
                    paramList[16] = BuildParameter("@PartnerPointsBalanceQty", SqlDbType.Int, 15, ParameterDirection.Input, partnerPointsBalanceQty);
                    paramList[17] = BuildParameter("@DefaultDataProtection", SqlDbType.Int, 15, ParameterDirection.Input, defaultDataProtPref);
                    cmdObj.Connection = connection;
                    //cmdObj.Transaction = transaction;

                    cmdObj.Parameters.Clear();
                    foreach (SqlParameter param in paramList)
                    {
                        cmdObj.Parameters.Add(param);
                    }
                    SqlParameter returnParameter = cmdObj.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    try
                    {
                        cmdObj.ExecuteNonQuery();
                        // Retrieve return value
                        returnCode = int.Parse(cmdObj.Parameters["@return_value"].Value.ToString());
                        //returnCode = (int)returnParameter.Value;
                    }
                    catch (Exception e)
                    {
                        Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                        wb.ProcessException(e);
                        //ExceptionManager.Publish(e);
                        //transaction.Rollback();
                        //return false;
                    }
                    #endregion
                    // Handle return codes of SProc
                    if (returnCode == 0)
                    {
                        // Successful returned details
                        //transaction.Commit();
                    }
                    else
                    {
                        //transaction.Rollback();
                    }
                    #endregion
                }
                finally
                {
                    connection.Close(); // rollback any pending transactions
                }
                return true;
            }
            catch (CrmServiceException e)
            {
                // Check if exception is caused by numeric overflow
                if (e.InnerException != null)
                {
                    if (e.InnerException is SqlException)
                    {
                        SqlException se = (SqlException)e.InnerException;
                        if (se.Number == 8115)
                        {
                            returnCode = 258;
                            return true;
                        }
                    }
                }
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            catch (SqlException e)
            {
                if (e.Number == 11)
                {
                    returnCode = 257;
                    return true;
                }
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            finally
            {
                timer.Stop();
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
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Number of Requested PosVoucherRedeems","Count of Requested CRM Server PosVoucherRedeems",PerformanceCounterType.NumberOfItems32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Overall Time(ms) of Requested PosVoucherRedeems","Time (Milliseconds) Taken During Requested CRM Server PosVoucherRedeems",PerformanceCounterType.NumberOfItems64)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Requested PosVoucherRedeems Per Second","Count of Requested CRM Server PosVoucherRedeems Per Second",PerformanceCounterType.RateOfCountsPerSecond32)]
        //[PerfMonCounter("Barcelona CRMServer:Seoul:POS","Average Time(ms) of Requested PosVoucherRedeems","Average Time (Milliseconds) Taken During Requested CRM Server PosVoucherRedeems","_Number of Requested PosVoucherRedeems","",PerformanceCounterType.AverageCount64)]
        public bool PosVoucherRedeem(
            int interfaceId,
            string storeCode,
            string storeDate,
            string posId,
            string txnNbr,
            string voucherValue,
            string voucherExpiryDate,
            out int returnCode)
        {

            // Set up timer
            HiResTimer timer = new HiResTimer();
            timer.Start();

            // Set up default values for output parameters
            returnCode = 256;
            txnNbr = txnNbr.PadLeft(5, '0');


            // Set up tracing
            ITraceState trState = PosApplication.Trace.StartProc("XmlApplication.PosVoucherRedeem");

            try
            {

                //if (PosApplication.PosVoucherRedeemSProc == null) {
                //    CrmServiceException ce = new CrmServiceException("Server","SqlError","SProcUnknown",PosApplication.Trace,"PosVoucherRedeem");
                //    throw ce;
                //}

                SqlConnection connection = new SqlConnection(Convert.ToString(ConfigurationSettings.AppSettings["AdminConnectionString"]));
                connection.Open();
                try
                {
                    #region Add Points
                    SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);

                    //object[] paramList = {
                    //                         storeCode,
                    //                         storeDate,
                    //                         posId,
                    //                         txnNbr,
                    //                         voucherValue,
                    //                         voucherExpiryDate};

                    //returnCode = PosApplication.PosVoucherRedeemSProc.Execute(
                    //    transaction,
                    //    ref paramList);

                    returnCode = 0;
                    SqlParameter[] paramList = new SqlParameter[6];

                    #region Execute SProc
                    SqlCommand cmdObj = new SqlCommand();

                    cmdObj.CommandType = CommandType.StoredProcedure;
                    cmdObj.CommandText = "sp_pos_voucher_redeem";
                    paramList[0] = BuildParameter("@store_code", SqlDbType.Int, 50, ParameterDirection.Input, storeCode);
                    paramList[1] = BuildParameter("@txn_date", SqlDbType.DateTime, 7, ParameterDirection.Input, storeDate);
                    paramList[2] = BuildParameter("@pos_id", SqlDbType.VarChar, 6, ParameterDirection.Input, posId);
                    paramList[3] = BuildParameter("@txn_nbr", SqlDbType.VarChar, 5, ParameterDirection.Input, txnNbr);
                    paramList[4] = BuildParameter("@voucher_value", SqlDbType.Decimal, 17, ParameterDirection.Input, voucherValue);
                    paramList[5] = BuildParameter("@voucher_expiry_date", SqlDbType.DateTime, 1, ParameterDirection.Input, voucherExpiryDate);

                    cmdObj.Connection = connection;
                    cmdObj.Transaction = transaction;

                    cmdObj.Parameters.Clear();
                    foreach (SqlParameter param in paramList)
                    {
                        cmdObj.Parameters.Add(param);
                    }
                    SqlParameter returnParameter = cmdObj.Parameters.Add("@return_value", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    try
                    {
                        cmdObj.ExecuteNonQuery();
                        // Retrieve return value
                        returnCode = int.Parse(cmdObj.Parameters["@return_value"].Value.ToString());
                        //returnCode = (int)returnParameter.Value;
                    }
                    catch (Exception e)
                    {
                        Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                        wb.ProcessException(e);
                       // ExceptionManager.Publish(e);
                        return false;
                    }
                    #endregion

                    // Handle return codes of SProc
                    switch (returnCode)
                    {
                        case 0:
                        case 3:
                            transaction.Commit();
                            break;
                        default:
                            transaction.Rollback();
                            break;
                    }
                    #endregion
                }
                finally
                {
                    connection.Close(); // rollback any pending transactions
                }

                return true;
            }
            catch (SqlException e)
            {
                if (e.Number == 11)
                {
                    returnCode = 257;
                    return true;
                }
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                return false;
            }
            finally
            {
                timer.Stop();
                trState.EndProc();
            }
        }
        #endregion
        #endregion

        #region Dispose Listener and Handler(s)
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PosListener()
        {
            this.Dispose(false);
        }

        private new void Dispose(bool disposing)
        {
            try
            {
                PosApplication.Trace.WriteDebug("Closing Listener NGC Port " + this.ngcIPAddress + ":" + this.ngcPort);
                PosApplication.PosSocketsEventLog.WriteEntry("Closing Listener on NGC Port " + this.ngcIPAddress + ":" + this.ngcPort);
                if (disposing)
                {
                    lock (this.handlerList)
                    {
                        foreach (IPosHandler posHandler in handlerList)
                        {
                            posHandler.Dispose();
                        }
                    }
                }
                this.Shutdown(SocketShutdown.Both);
                this.Close();
            }
            catch
            {
                // already shutdown and closed
            }
            base.Dispose(disposing);
        }
        #endregion

        #region BuildParameter Method

        /// <summary>
        /// Method to Build Parameter, set its direction and value
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="dbType">Parameter Type</param>
        /// <param name="size">Parameter Size</param>
        /// <param name="dir">Parameter Direction</param>
        /// <param name="paramVal">Parameter Value</param>
        /// <returns></returns>

        private SqlParameter BuildParameter(string paramName, SqlDbType dbType, int size, ParameterDirection dir, object paramVal)
        {
            SqlParameter param = null;
            Result result = new Result();
            try
            {
                param = new SqlParameter(paramName, dbType, size);
                param.Direction = dir;
                param.Value = paramVal;
            }
            catch (Exception e)
            {
                Fujitsu.eCrm.Generic.SharedUtils.WebException wb = new Fujitsu.eCrm.Generic.SharedUtils.WebException();
                wb.ProcessException(e);
                //ExceptionManager.Publish(e);
                result.Add(e);
            }
            finally
            {


            }
            return param;
        }

        #endregion BuildParameter Method

    }
}