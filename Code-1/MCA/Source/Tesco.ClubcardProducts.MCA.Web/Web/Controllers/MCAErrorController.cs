using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class MCAErrorController : AccountController
    {
        public ActionResult Home()
        {
            LogData _logData = new LogData();
            try
            {                
                _logData.RecordStep("Error encountered in some Controller Action.");
                _logger.Submit(_logData);
                return View("Error");
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while displaying Custom Error Page", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }
    }
}
