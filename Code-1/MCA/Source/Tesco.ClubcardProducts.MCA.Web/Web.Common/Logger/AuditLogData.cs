using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;   
namespace Tesco.ClubcardProducts.MCA.Web.Common.Logger
{
    public class AuditLogData : LogData
    {
        public string DotcomCustomerId { get; set; }
        public string HTTP_X_FORWARDED_FOR { get; set; }
        public string HTTP_REMOTE_ADDR { get; set; }
        public string HTTP_CLIENT_ID { get; set; }
        
        public override string Source 
        { 
            get
            {
                return String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                                    this._moduleName,
                                    this._currentClass,
                                    this._currentMethod,
                                    this.CustomerID,
                                    this.DotcomCustomerId,
                                    this.HTTP_CLIENT_ID,
                                    this.HTTP_X_FORWARDED_FOR,
                                    this.HTTP_REMOTE_ADDR);
            } }

        public AuditLogData() : base()
        {
            if (String.IsNullOrWhiteSpace(this.DotcomCustomerId))
            {
                this.DotcomCustomerId = MCACookie.Cookie[MCACookieEnum.DotcomCustomerID];

                if (GlobalCachingProvider.Instance.GetItem(AppConfigEnum.IsEnterpriceServiceCallsEnabled.ToString()).ToString() == "1")
                {
                    if (String.IsNullOrWhiteSpace(this.DotcomCustomerId))
                    {
                        var dcid = HttpContext.Current.Session["dcid"];
                        this.DotcomCustomerId = dcid != null ? dcid.ToString() : String.Empty;

                        if (String.IsNullOrWhiteSpace(this.DotcomCustomerId))
                        {
                            SecurityHelper sHelp = new SecurityHelper();
                            this.DotcomCustomerId = sHelp.GetSecurityItem("DotcomID");
                        }
                    }
                }
            }

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                SetTrueClientIP();
            }           
            this.LogEntries.Clear();            
        }

        private void SetTrueClientIP()
        {
            this.HTTP_X_FORWARDED_FOR = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? "NA";
            this.HTTP_CLIENT_ID = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_ID"] ?? "NA";
            this.HTTP_REMOTE_ADDR = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ?? "NA";
        }

        public override void RecordStep(string message)
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
                    Message = message
                });
            }
            catch { }
        }
    }
}
