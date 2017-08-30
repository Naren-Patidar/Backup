using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.API.ServiceManager;

namespace Tesco.ClubcardProducts.MCA.API.Logger
{
    [Serializable]
    public class LogData
    {
        public bool _overrideLevel = false;
        public string _clientID = String.Empty;
        public string _serviceRequested = String.Empty;
        public string _operationRequested = String.Empty;
        public string _assemblyInvoked = String.Empty;
        public string _typeInvoked = String.Empty;
        public string _operationInvoked = String.Empty;
        public string _overallduration = String.Empty;
        public string _identifier = String.Empty;
        public string _clientTransactionID = String.Empty;

        LoggingLevel _level = LoggingLevel.none;
        protected List<LogEvent> _logEntries = null;
        List<string> _blackLists = new List<string>();

        [JsonIgnore]
        public List<string> BlackLists
        {
            get { return _blackLists; }
            set { _blackLists = value; }
        }

        public LogData()
        {
            try
            {
                Enum.TryParse<LoggingLevel>(GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.LoggingLevel), out this._level);
                this._identifier = Guid.NewGuid().ToString();
                this.RecordEntry("Starting...", false, String.Empty);
            }
            catch { }
        }

        public List<LogEvent> LogEntries { get { return this._logEntries; } }

        public virtual string Source
        {
            get
            {
                return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                                    this._identifier,
                                    this._clientTransactionID,
                                    this._overallduration,
                                    this._clientID,
                                    this._serviceRequested,
                                    this._operationRequested,
                                    this._assemblyInvoked,
                                    this._typeInvoked,
                                    this._operationInvoked);
            }
        }

        public LoggingLevel Level { get { return this._level; } }

        public virtual void RecordStep(string message, bool isError = false)
        {
            this.RecordStep(message, String.Empty, isError);
        }

        public virtual void RecordStep(string message, string errorCode, bool isError)
        {
            try
            {
                if (this._level != LoggingLevel.none)
                {
                    this.RecordEntry(this.CleanupBlackLists(message), isError, errorCode);
                }
            }
            catch { }
        }

        public void CaptureData(string name, object dataObject) 
        {
            try
            {
                if (this._level == LoggingLevel.high)
                {
                    this.RecordEntry(this.CleanupBlackLists(new { name, dataObject }.JsonText()), false, String.Empty);
                }
            }
            catch { }
        }

        protected virtual void RecordEntry(string message, bool isError, string errorCode)
        {
            try
            {
                if (this.Level == LoggingLevel.none)
                {
                    return;
                }

                if (this._logEntries == null)
                {
                    this._logEntries = new List<LogEvent>();
                }

                this._logEntries.Add(new LogEvent
                {
                    DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-ffff"),
                    Message = this.CleanupBlackLists(message),
                    EntryType = isError ?  LogEntryType.error : LogEntryType.info,
                    ErrorCode = errorCode
                });
            }
            catch { }
        }

        private string CleanupBlackLists(string message)
        {
            try
            {
                if (this.BlackLists != null && this.BlackLists.Count > 0)
                {
                    foreach (string s in this.BlackLists)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            message = message.Replace(s, "*************");
                        }
                    }
                }
            }
            catch { }
            return message;
        }
    }

    public class LogEvent
    {
        public string DateTime { get; set; }
        public string Message { get; set; }
        public LogEntryType EntryType { get; set; }
        public string ErrorCode { get; set; }
    }

    public class LogDataAndEvent
    {
        public LogEvent LogEntry { get; set; }
        public LogData LogData { get; set; }
    }

    public enum LoggingLevel
    {
        none,
        low,
        high,
        error
    }

    public enum LogEntryType
    {
        info,
        error
    }
}
