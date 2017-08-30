using System;
using System.Configuration;
using System.Reflection;
using System.Resources;
using System.Threading;
using Fujitsu.eCrm.Generic.CrmService;
using Fujitsu.eCrm.Generic.CrmService.Security;
using Fujitsu.eCrm.Generic.LocalizationLibrary;
using Fujitsu.eCrm.Generic.SharedUtils;
using System.Globalization;


namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	internal class PosApplication {

		#region Constants
		private const string nameSpace = "Fujitsu.eCrm.Seoul.PosSocketsService";
		private const string iPosHandlerName = nameSpace+".IPosHandler";
		private const string errorResourceName = nameSpace+".Resources.ErrorMessages";
				
		private const string posTimeout = "POSTimeout";
		private const string posRetryVoucherRedeemTime = "POSRetryVoucherRedeemTime";
		private const string posFutureTimeSetting = "POSFutureTime";
		private const string posInformConversionTxnThresholdSetting = "POSInformConversionTxnThreshold";
		private const string posRefreshFrequencySetting = "POSRefreshFrequency";

		private const string retryConnectionTimeSetting = "RetryConnectionTime";
		private const string posListenerBacklogSizeSetting = "PosListenerBacklogSize";
		private const string posHandlerDllSetting = "PosHandlerDll";
		private const string posAuditUserSetting = "PosAuditUser";
		private const string cultureDefaultSetting = "CultureDefault";
		private const string localizationDirectorySetting = "LocalizationDirectory";
		#endregion

		#region Static Attributes
		#region Configured Attributes
		/// <summary>Maximum time to receive and send data</summary>
		private static int timeoutPeriod = 30000;

		/// <summary>When voucher redemption fails due to missing transaction, the Time(ms) to wait before try again</summary>
		private static int retryVoucherRedeemTime = 1000;

		/// <summary>The maximum future time allowed for transactions</summary>
		private static string futureTimeString = String.Empty;

		/// <summary>Number of times to inform customer of points converted to rewards</summary>
		private static string informConversionTxnThreshold = String.Empty;

		/// <summary>Frequency of refreshing store data and changing sockets.  Defaults to 600000 milliseconds (10 minutes)</summary>
		private static long refreshFrequency = 600000;
		
		/// <summary>The listener's queue size of connection waiting acceptance</summary>
		private static int pendingBacklog = 10;

		/// <summary>The DLL to dynamically load for handling POS messages</summary>
		private static string posHandlerDll;
		#endregion

		private static ConstructorInfo iPosHandlerConstructor;
		private static PosListenerTable posListenerTable;
		private static SProc posGetSProc;
		private static SProc posSetSProc;
		private static SProc posVoucherRedeemSProc;
		private static Trace trace;
		private static System.Diagnostics.EventLog posSocketsEventLog;
		private static string userId;
		private static string sessionId;
		#endregion

		#region Properties
		internal static int TimeoutPeriod { get { return PosApplication.timeoutPeriod; } }
		internal static int RetryVoucherRedeemTime { get { return PosApplication.retryVoucherRedeemTime; } }
		internal static string FutureTime { get { return PosApplication.futureTimeString; }	}
		internal static string InformConversionTxnThreshold { get { return PosApplication.informConversionTxnThreshold; }	}
		internal static SProc PosGetSProc { get { return PosApplication.posGetSProc; }	}
		internal static SProc PosSetSProc { get { return PosApplication.posSetSProc; }	}
		internal static SProc PosVoucherRedeemSProc { get { return PosApplication.posVoucherRedeemSProc; }	}
		internal static Trace Trace { get { return PosApplication.trace; }	}
		internal static System.Diagnostics.EventLog PosSocketsEventLog { get { return PosApplication.posSocketsEventLog; } }
		internal static string UserId { get { return PosApplication.userId; } }
		internal static string SessionId { get { return PosApplication.sessionId; } }
		#endregion

		#region Start NGC POS Server
		/// <summary>
		/// Call Initialise CRM Server (repeatedly until the server starts successfully
		/// Initialise CRM Server will also call Initialise POS Server.
		/// </summary>
		internal static void OnStart(System.Diagnostics.EventLog posSocketsEventLog) {

			PosApplication.posSocketsEventLog = posSocketsEventLog;
			if (Thread.CurrentThread.Name == null) {
				Thread.CurrentThread.Name = Guid.NewGuid().ToString();
			}

			// Localise Date for Trace File First
			string defaultCulture = ConfigurationSettings.AppSettings[cultureDefaultSetting];
			CultureInfo cultureInfo = new CultureInfo(defaultCulture);
			// set the gregorian (western) calendar
			cultureInfo.DateTimeFormat.Calendar=new GregorianCalendar(GregorianCalendarTypes.Localized) ;			
			Thread.CurrentThread.CurrentCulture = cultureInfo;



			PosApplication.trace = new Trace();
			ITraceState trState = PosApplication.trace.StartProc("PosApplication.OnStart");

			// Get frequency for retrying to initialise POS Socket Service.  
			// Defaults to 60000 milliseconds (1 minute)
			string retryFrequencyString = ConfigurationSettings.AppSettings[retryConnectionTimeSetting];
			int retryFrequency;
			try {
				retryFrequency = Int32.Parse(retryFrequencyString);
			} catch {
				retryFrequency = 60000;
			}

			// Try to initialise
			while (!XmlApplication.Initialise(
				new XmlApplication.InitialiseMethod(PosApplication.InitialiseServer),
				new XmlApplication.CloseMethod(PosApplication.FinaliseServer))) {

				Thread.Sleep(retryFrequency);
			}

			PosApplication.trace.EndProc(trState);
		}

		public static void InitialiseServer() {

			#region Configuration Settings
			#region Timeout Period
			// the future time determines how far in the future a transaction can be yet still be allowed
			string timeoutPeriodString = (string)XmlApplication.ConfigurationTable[posTimeout];
			try {
				// does the string represent an integer
				timeoutPeriod = Int32.Parse(timeoutPeriodString);
			} catch {
				// default timeoutPeriod = 30000;
			}
			#endregion

			#region Retry Voucher Redeem Time
			// the future time determines how far in the future a transaction can be yet still be allowed
			string retryVoucherRedeemTimeString = (string)XmlApplication.ConfigurationTable[posRetryVoucherRedeemTime];
			try {
				// does the string represent an integer
				retryVoucherRedeemTime = Int32.Parse(retryVoucherRedeemTimeString);
			} catch {
				// default retryVoucherRedeemTime = 1000;
			}
			#endregion

			#region Future Time
			// the future time determines how far in the future a transaction can be yet still be allowed
			futureTimeString = (string)XmlApplication.ConfigurationTable[posFutureTimeSetting];
			try {
				// does the string represent an integer
				Int32.Parse(futureTimeString);
			} catch {
				futureTimeString = "2";
			}
			#endregion

			#region Conversion Threshold
			// The number of transaction for inform customers of points converted
			informConversionTxnThreshold = (string)XmlApplication.ConfigurationTable[posInformConversionTxnThresholdSetting];
			try {
				// does the string represent an integer
				Int32.Parse(informConversionTxnThreshold);
			} catch {
				informConversionTxnThreshold = "5";
			}
			#endregion

			#region Refresh Store Data
			// The Frequency(ms) the POS Server checkes for changes to store data
			string refreshFrequencyString = (string)XmlApplication.ConfigurationTable[posRefreshFrequencySetting];
			try {
				refreshFrequency = Int64.Parse(refreshFrequencyString);
			} catch {
				// default refreshFrequency = 600000;
			}
			#endregion

			#region Listener Backlog
			try {
				pendingBacklog = Int32.Parse(ConfigurationSettings.AppSettings[posListenerBacklogSizeSetting]);
			} catch {
				// default pendingBacklog = 10;
			}
			#endregion

			#region Audit User
			string auditUser = ConfigurationSettings.AppSettings[posAuditUserSetting];
			#endregion
			#endregion

			#region POS Handler
			posHandlerDll = ConfigurationSettings.AppSettings[posHandlerDllSetting];
			// Dynamically Construct IPosHandler to handle requests
			Assembly posHandlerAssembly = Assembly.LoadWithPartialName(posHandlerDll);
			if (posHandlerAssembly != null) {
				System.Reflection.TypeFilter iPosHandlerFilter = new System.Reflection.TypeFilter(IPosHandlerFilter);
				foreach (Type posHandlerDllType in posHandlerAssembly.GetTypes()) {
					Type[] interfaces = posHandlerDllType.FindInterfaces(iPosHandlerFilter,iPosHandlerName);
					if (interfaces.Length > 0) {
						iPosHandlerConstructor = posHandlerDllType.GetConstructors()[0];
						break;
					}
				}
			}
			if (iPosHandlerConstructor == null) {
				throw (new Exception());
			}
			#endregion

			#region SPROC
			posGetSProc = (SProc)XmlApplication.SProcTable["sp_pos_get"];
			posSetSProc = (SProc)XmlApplication.SProcTable["sp_pos_set"];
			posVoucherRedeemSProc = (SProc)XmlApplication.SProcTable["sp_pos_voucher_redeem"];
			#endregion

			#region Miscellaneous
			// Localise
			string defaultCulture = ConfigurationSettings.AppSettings[cultureDefaultSetting];
			string localiseDirectory = ConfigurationSettings.AppSettings[localizationDirectorySetting];
			Localization.Initialize(defaultCulture,localiseDirectory);

			// Point to POS Error Messages
			CrmServiceException.AddResourceManager(
				new ResourceManager(errorResourceName,Assembly.GetExecutingAssembly()));

			// Create Session for auditing creation of customer skeletons
			try {
				Session dummySession = new Session(PosApplication.trace,auditUser,defaultCulture);
				userId = dummySession.UserId;
				sessionId = dummySession.SessionId;
			} catch {
				userId = null;
				sessionId = null;
			}

			// Create Listeners, this must be the last action in Initialise
			posListenerTable = new PosListenerTable(refreshFrequency,pendingBacklog);
			#endregion
		}

		/// <summary>
		/// Find Type implementing the interface specified by criteriaObj
		/// </summary>
		/// <param name="typeObj"></param>
		/// <param name="criteriaObj"></param>
		/// <returns></returns>
		private static bool IPosHandlerFilter(Type typeObj, Object criteriaObj) {
			return (typeObj.ToString() == criteriaObj.ToString());
		}
		#endregion

		#region Dynamically Construct Handler
		internal static IPosHandler ConstructIPosHandler(IPosListener posListener, System.Net.Sockets.Socket socket) {
			return (IPosHandler)iPosHandlerConstructor.Invoke(new object[2] {posListener,socket});
		}
		#endregion

		#region Stop NGC POS Server
		private static void FinaliseServer() {
			posListenerTable.Dispose();
		}
		#endregion
	}
}