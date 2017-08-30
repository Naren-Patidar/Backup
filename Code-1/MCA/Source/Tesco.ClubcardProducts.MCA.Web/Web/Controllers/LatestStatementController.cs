using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using System;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Collections.Generic;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class LatestStatementController : BaseController
    {
        IMyLatestStatementBC _mylateststatementProvider;
        string sRewardMilesRateFlag = string.Empty;

        public LatestStatementController()
        {
            this._mylateststatementProvider = ServiceLocator.Current.GetInstance<IMyLatestStatementBC>();
        }

        /// <summary>
        /// Gets the MLS ViewModel details for the MLS page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        [SecurityCheck(Order = 3)]
        [PageAuthorization(Order = 4)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            try
            {
                string customerID = logData.CustomerID = this.CustomerId;

                MyLatestStatementViewModel myLatestStatementViewModel = new MyLatestStatementViewModel();

                myLatestStatementViewModel = _mylateststatementProvider.GetMLSViewDetails(customerID.TryParse<Int64>());

                _logger.Submit(logData);
                return View(myLatestStatementViewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while getting MLSViewDetails", ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }
    }
}
