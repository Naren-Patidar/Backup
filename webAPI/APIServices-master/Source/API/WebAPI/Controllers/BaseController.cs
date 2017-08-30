using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using Tesco.ClubcardProducts.MCA.API.Logger;

namespace Tesco.ClubcardProducts.MCA.API.Controllers
{
    public class BaseController : ApiController
    {
        protected LogData _LogData = null;
        protected LogData _AuditLogData = null;

        public static ILoggingService _GeneralLoggingService = new LoggingService();
        public static ILoggingService _AuditLoggingService = new AuditLoggingService();

        public BaseController()
        {
            this._LogData = new LogData();
            this._AuditLogData = new AuditLogData();
        }
    }
}
