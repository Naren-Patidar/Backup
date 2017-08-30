using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Web.Routing;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Logger
{
    [Serializable]
    public class LogData
    {
        LoggingLevel _level = LoggingLevel.none;
        protected string _moduleName = String.Empty;
        protected string _currentMethod = String.Empty;
        protected string _currentClass = String.Empty;
        protected List<LogEvent> _logEntries = null;
        List<string> _blackLists = new List<string>();

        public string CustomerID { get; set; }

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
                this.DetermineCurrentModule();
                this._level = LogConfigProvider.Instance[this._moduleName];
                this.DetermineMethodInfo();

                if (String.IsNullOrWhiteSpace(this.CustomerID))
                {
                    this.CustomerID = MCACookie.Cookie[MCACookieEnum.CustomerID];

                    if (GlobalCachingProvider.Instance.GetItem(AppConfigEnum.IsEnterpriceServiceCallsEnabled.ToString()).ToString() == "1")
                    {
                        if (String.IsNullOrWhiteSpace(this.CustomerID))
                        {
                            var cid = HttpContext.Current.Session["cid"];
                            this.CustomerID = cid != null ? cid.ToString() : String.Empty;

                            if (String.IsNullOrWhiteSpace(this.CustomerID))
                            {
                                SecurityHelper sHelp = new SecurityHelper();
                                this.CustomerID = sHelp.GetSecurityItem("CustomerID");
                            }
                        }
                    }
                }

                this.RecordEntry("Starting...");
            }
            catch { }
        }

        internal LogData(bool skipCustIDLogging)
        {
            try
            {
                this.DetermineCurrentModule();
                this._level = LogConfigProvider.Instance[this._moduleName];
                this.DetermineMethodInfo();
                this.RecordEntry("Starting...");
            }
            catch { }
        }

        public List<LogEvent> LogEntries { get { return this._logEntries; } }

        public virtual string Source
        {
            get
            {
                return String.Format("{0}|{1}|{2}|{3}",
                                    this._moduleName,
                                    this._currentClass,
                                    this._currentMethod,
                                    this.CustomerID);
            }
        }

        public LoggingLevel Level { get { return this._level; } }

        public virtual void RecordStep(string message)
        {
            try
            {
                if (this._level != LoggingLevel.none)
                {
                    this.RecordEntry(this.CleanupBlackLists(message));
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
                    this.RecordEntry(this.CleanupBlackLists(new { name, dataObject }.JsonText()));
                }
            }
            catch { }
        }

        protected void DetermineCurrentModule()
        {
            try
            {
                string currentModule = LogConfigProvider.DEFAULTLOGLEVEL;

                if (HttpContext.Current != null)
                {
                    RouteData _currentRoute = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

                    if (_currentRoute != null)
                    {
                        currentModule = _currentRoute.GetRequiredString("controller");

                        if (String.IsNullOrEmpty(currentModule))
                        {
                            currentModule = LogConfigProvider.DEFAULTLOGLEVEL;
                        }
                    }
                }
                this._moduleName = currentModule;
            }
            catch { }
        }

        protected void DetermineMethodInfo()
        {
            try
            {
                StackFrame frame = new StackFrame(2);
                var method = frame.GetMethod();
                if (method.IsConstructor)
                {
                    frame = new StackFrame(3);
                    method = frame.GetMethod();
                }
                _currentClass = method.DeclaringType.ToString();
                this._currentMethod = method.Name;
            }
            catch { }
        }

        protected virtual void RecordEntry(string message)
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
                                            EntryType = LogEntryType.info
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
    }

    public class LogDataAndEvent
    {
        public LogEvent LogEntry { get; set; }
        public LogData LogData { get; set; }
    }
}
