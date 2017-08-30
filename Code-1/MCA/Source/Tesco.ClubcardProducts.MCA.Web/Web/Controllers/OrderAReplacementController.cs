using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Collections.Generic;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using System.Linq;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class OrderAReplacementController : BaseController
    {
        private IOrderAReplacementBC _orderReplacementProvider;
        ILoggingService _AuditLoggerService = null;

        public OrderAReplacementController()
        {
            _orderReplacementProvider = ServiceLocator.Current.GetInstance<IOrderAReplacementBC>();
            _AuditLoggerService = new AuditLoggingService();
        }

        public OrderAReplacementController(IOrderAReplacementBC _IOrderReplacementBC, ILoggingService _auditlogger, IAccountBC _IAccountBC, IConfigurationProvider _IConfigurationProvider)
            : base(_auditlogger, _IAccountBC, _IConfigurationProvider)
        {
            _orderReplacementProvider = _IOrderReplacementBC;
            _AuditLoggerService = _auditlogger;
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [ActivationCheck(Order = 2)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            
            List<string> blackListFields = new List<string>();
            try
            {
                long customerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = customerID.ToString();
                OrderAReplacementModel orderAReplacementModel = new OrderAReplacementModel();
                orderAReplacementModel = _orderReplacementProvider.GetOrderAReplacementModel(customerID, CurrentCulture);
                blackListFields.Add(orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString());
                logData.BlackLists = blackListFields;
                logData.CaptureData("Is Order in Process? ", orderAReplacementModel.IsInProcess);
                _logger.Submit(logData);
                return View(orderAReplacementModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed while loading Order Replacement page", ex,
                new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        public ActionResult Home(OrderAReplacementModel orderAReplacementModel)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();

            try
            {
                long customerID = this.GetCustomerId();
                logData.CustomerID = logDataAudit.CustomerID = customerID.ToString();

                orderAReplacementModel.OrderReplacementModel.ClubcardNumber = _orderReplacementProvider.GetOrderAReplacementModel(
                                                    customerID, CurrentCulture).OrderReplacementModel.ClubcardNumber;

                if (orderAReplacementModel.OrderReplacementModel.Reason != null)
                {
                    logData.RecordStep(string.Format("Processing Order Replacement Request: {0}", orderAReplacementModel.OrderReplacementModel.Reason));
                    orderAReplacementModel.IsInProcess = _orderReplacementProvider.ProcessOrderReplacementRequest(orderAReplacementModel.OrderReplacementModel);
                    logData.CaptureData("Request Reason for Replacement: ", orderAReplacementModel.OrderReplacementModel.Reason.ToString());
                    logDataAudit.RecordStep(String.Format("Order replacement|{0}",
                           new
                           {
                               Reason = orderAReplacementModel.Reasons[orderAReplacementModel.OrderReplacementModel.Reason].ToString(),
                               CCLastDigits = orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString().Substring(
                               orderAReplacementModel.OrderReplacementModel.ClubcardNumber.ToString().Length - 4)
                           }.JsonText()));
                }
                else
                {
                    orderAReplacementModel.errorMsg = true;
                    logData.RecordStep(string.Format("Error Message: {0}", orderAReplacementModel.errorMsg));
                }
                _logger.Submit(logData);
                return View(orderAReplacementModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed  while processing Order Replacement request", ex,
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });

            }
            finally
            {
                if (_AuditLoggerService != null)
                {
                    _AuditLoggerService.Submit(logDataAudit);
                }
            }
        }
    }
}
