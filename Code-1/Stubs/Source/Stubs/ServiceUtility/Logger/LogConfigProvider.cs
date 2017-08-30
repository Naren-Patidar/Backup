using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Web.Configuration;

namespace ServiceUtility
{
    public class LogConfigProvider
    {
        public const string DEFAULTLOGLEVEL = "default";
        public const string EXCLOGDATAKEY = "logdata";
        
        private static LogConfigProvider _logConfigProvider = null;
        private static object _monitor = new object();        
        private Dictionary<string, LoggingLevel> _logConfiguration = null;

        private LogConfigProvider()
        {

        }

        public static LogConfigProvider Instance
        {
            get
            {
                lock (_monitor)
                {
                    if (LogConfigProvider._logConfigProvider == null)
                    {
                        LogConfigProvider._logConfigProvider = new LogConfigProvider();

                        if (WebConfigurationManager.AppSettings["LoggingConfiguration"] == null)
                        {
                            LogConfigProvider.Instance._logConfiguration = new Dictionary<string, LoggingLevel>() 
                                                                            {
                                                                               { LogConfigProvider.DEFAULTLOGLEVEL, LoggingLevel.low }
                                                                            };
                        }
                        else
                        {
                            LogConfigProvider.Instance._logConfiguration = JsonConvert.DeserializeObject<Dictionary<string, LoggingLevel>>
                                                                            (
                                                                                WebConfigurationManager.AppSettings["LoggingConfiguration"]
                                                                            );
                        }
                    }
                }
                return LogConfigProvider._logConfigProvider;
            }
        }

        public LoggingLevel this[string module]
        {
            get
            {
                LoggingLevel logLevel = this._logConfiguration[LogConfigProvider.DEFAULTLOGLEVEL];

                if (this._logConfiguration.ContainsKey(module.ToLower()))
                {
                    logLevel = this._logConfiguration[module.ToLower()];
                }

                return logLevel;
            }
        }
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
