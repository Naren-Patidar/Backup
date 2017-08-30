using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.API.Logger
{
    public class AuditLogData : LogData
    {
        public string HTTP_X_FORWARDED_FOR { get; set; }
        public string HTTP_REMOTE_ADDR { get; set; }
        public string HTTP_CLIENT_ID { get; set; }
        
        public override string Source 
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
                                    this.HTTP_CLIENT_ID,
                                    this.HTTP_X_FORWARDED_FOR,
                                    this.HTTP_REMOTE_ADDR);

            } }

        public AuditLogData() :  base()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                this.SetTrueClientIP();
            }
            if (this.LogEntries != null)
            {
                this.LogEntries.Clear();
            }
        }

        private void SetTrueClientIP()
        {
            this.HTTP_X_FORWARDED_FOR = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? "NA";
            this.HTTP_CLIENT_ID = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_ID"] ?? "NA";
            this.HTTP_REMOTE_ADDR = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ?? "NA";
        }

        public override void RecordStep(string message, bool isError = false)
        {
            try
            {
                if (this._logEntries == null)
                {
                    this._logEntries = new List<LogEvent>();
                }

                this._logEntries.Add(new LogEvent
                {
                    DateTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH-mm-ss-ffff"),
                    Message = message,
                    EntryType = isError ? LogEntryType.error : LogEntryType.info
                });
            }
            catch { }
        }
    }
}
