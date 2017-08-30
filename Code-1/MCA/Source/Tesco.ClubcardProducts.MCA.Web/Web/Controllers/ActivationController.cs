using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class ActivationController : BaseController
    {
        private IActivationBC _activationProvider;
        ILoggingService _AuditLoggerService = null;

        public ActivationController()
        {
            _activationProvider = ServiceLocator.Current.GetInstance<IActivationBC>();
            this._AuditLoggerService = new AuditLoggingService();
            ModelState.Clear();
        }

        public ActivationController(IActivationBC _IActivationBC, IAccountBC _IAccountBC, ILoggingService _auditlogger, IConfigurationProvider configProvider)
            : base(_auditlogger, _IAccountBC, configProvider)
        {
            this._activationProvider = _IActivationBC;
            _AuditLoggerService = _auditlogger;
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult Home(string from)
        {
            LogData logData = new LogData(); 
            ClubcardCustomer model = new ClubcardCustomer();
            _logger.Submit(logData);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult Home(ClubcardCustomer customerEntity, string from)
        {
            LogData logData = new LogData();
            LogData logDataAudit = new AuditLogData();
            List<string> blackListFields = new List<string>();
            try
            {
                string dotComID = this.DotcomCustomerId;
                string customerID = logData.CustomerID = logDataAudit.CustomerID = this.CustomerId;

                if (ModelState.IsValid)
                {
                    customerEntity.Address.PostCode = GeneralUtility.FormatPostalCode(customerEntity.Address.PostCode);
                    logData.RecordStep(string.Format("DotcomCustomerId :{0}", dotComID));
                    ActivationRequestStatusEnum requestStatus = _activationProvider.ProcessActivationRequest(
                        dotComID, customerEntity.Clubcard.ClubcardNumber.TryParse<long>(), customerEntity);

                    logData.CaptureData("requestStatus", requestStatus);

                    blackListFields.Add(customerEntity.Clubcard.ClubcardNumber.TryParse<string>());
                    blackListFields.Add(customerEntity.FirstName.TryParse<string>());
                    blackListFields.Add(customerEntity.Surname.TryParse<string>());
                    blackListFields.Add(customerEntity.ContactDetail.EmailAddress.TryParse<string>());
                    blackListFields.Add(customerEntity.ContactDetail.MobileContactNumber.TryParse<string>());

                    logData.BlackLists = blackListFields;
                    logData.CaptureData("customerEntity", customerEntity);
                    _logger.Submit(logData);
                    switch (requestStatus)
                    {
                        case ActivationRequestStatusEnum.ErrorMessage:
                            logData.RecordStep(string.Format("ErrorMessage", ActivationRequestStatusEnum.ErrorMessage));
                            _logger.Submit(logData);
                            throw new Exception("ProcessActivationRequest failed");

                        case ActivationRequestStatusEnum.DuplicateDotcomID:
                            logData.RecordStep(string.Format("DuplicateDotcomID", ActivationRequestStatusEnum.DuplicateDotcomID));
                            ViewBag.Name = "DuplicateDotcomID";
                            break;
                        //Merging UK MCa Hot fix code into 3F branch
                        case ActivationRequestStatusEnum.CustomerIDalready:
                            logData.RecordStep(string.Format("CustomerIDalready", ActivationRequestStatusEnum.CustomerIDalready));
                            ViewBag.Name = "CustomerIDalready";
                            break;

                        case ActivationRequestStatusEnum.ClubcardDetailsDoesntMatch:
                            logData.RecordStep(string.Format("ClubcardDetailsDoesntMatch", ActivationRequestStatusEnum.ClubcardDetailsDoesntMatch));
                            ViewBag.Name = "ClubcardDetailsDoesntMatch";
                            logDataAudit.RecordStep(String.Format("False:{0}|{1}", ActivationRequestStatusEnum.ClubcardDetailsDoesntMatch,
                                  new
                                  {
                                    ClubcardNumber = (customerEntity.Clubcard.ClubcardNumber.Length > 4) ?
                                    customerEntity.Clubcard.ClubcardNumber.Substring(customerEntity.Clubcard.ClubcardNumber.Length - 4) : customerEntity.Clubcard.ClubcardNumber,
                                    FirstName = customerEntity.FirstName.TryParse<string>(),
                                    Surname = customerEntity.Surname.TryParse<string>(),
                                    Postcode = customerEntity.Address.PostCode.TryParse<string>()

                                  }.JsonText()));
                            break;

                        case ActivationRequestStatusEnum.ActivationFailed:
                            logData.RecordStep(string.Format("Activation Failed", ActivationRequestStatusEnum.ActivationFailed));
                            _logger.Submit(logData);
                            throw new Exception("Activation failed" + " - DotcomID:" + dotComID.ToString());

                        case ActivationRequestStatusEnum.ActivationSuccessful:
                            //after details are update, user will redirected to Thank you screen
                            logData.RecordStep(string.Format("Activation Successful", ActivationRequestStatusEnum.ActivationSuccessful));
                            logDataAudit.RecordStep(String.Format("True:{0}", ActivationRequestStatusEnum.ActivationSuccessful));
                            // Update base controller's Activation status property as ACTIVATED
                            MCACookie.Cookie.Remove(MCACookieEnum.DotcomCustomerID);
                            MCACookie.Cookie.Add(MCACookieEnum.Activated, "Y");
                            _logger.Submit(logData);
                            if (!string.IsNullOrEmpty(from))
                            {
                                this.SecurityAfterVerification(1, 1);
                                return RedirectToAction("RedirectTo", "Account", new { from = from });
                            }
                            else
                            {
                                return RedirectToAction("Confirmation");
                            }
                        default:
                            break;

                    }
                    _logger.Submit(logData);

                }

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
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
            return View("~/Views/Activation/Home.cshtml", customerEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser]
        public ActionResult ConfirmActivation()
        {
            LogData logData = new LogData();
            try
            {
                logData.CustomerID = this.GetCustomerId().ToString();
                this.SecurityAfterVerification(1, 1);
                _logger.Submit(logData);
                return RedirectToAction("Home", "Home");   
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult Confirmation()
        {
            LogData _logData = new LogData();
            _logger.Submit(_logData);
            return View("~/Views/shared/_ActivationConfirmDetails.cshtml");
        }

        [HttpGet]
        [AuthorizeUser]
        public ActionResult Pending()
        {
            LogData _logData = new LogData();
            ViewBag.Name = "Pending";
            _logger.Submit(_logData);
            return View("~/Views/Activation/ActivationStatus.cshtml");
        }
        
        [HttpGet]
        [AuthorizeUser]
        public ActionResult BannedHousehold()
        {
            LogData _logData = new LogData();
            ViewBag.Name = "Banned";
            _logger.Submit(_logData);
            return View("~/Views/Activation/ActivationStatus.cshtml");
        }
        
        [HttpGet]
        [AuthorizeUser]
        public ActionResult LeftScheme()
        {
            LogData _logData = new LogData();
            ViewBag.Name = "LeftScheme";
            _logger.Submit(_logData);
            return View("~/Views/Activation/ActivationStatus.cshtml");
        }

        private void SecurityAfterVerification(int customerMailStatus, int customerUseStatus)
        {
            LogData logData = new LogData();
            long customerId = this.CustomerId.TryParse<Int64>();
            logData.CustomerID = customerId.ToString();

            bool showSecurityAfterActivation = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                [AppConfigEnum.EnableSecurityAfterActivation.ToString()].ConfigurationValue1 == "1";

            logData.RecordStep(string.Format("Show Security After Activation: {0}", showSecurityAfterActivation));
            
            if (!showSecurityAfterActivation)
            {
                string dotcomid = this.DotcomCustomerId;
                GeneralUtility generalUtility = new GeneralUtility();
                if (this.IsEnterpriceServiceCallsEnabled)
                {
                    CustomerActivationStatusdetails ca = new CustomerActivationStatusdetails()
                    {
                        Activated = 'Y',
                        CustomerId = customerId,
                        CustomerMailStatus = customerMailStatus,
                        CustomerUseStatus = customerUseStatus
                    };

                    this.SetTokenPayload(ca, dotcomid, true, default(long));

                    //This is just a temporary code. It should go away once boost team develops an alternate logic to determine
                    //if the customer cleared the security or not.
                    generalUtility.CreateSecurityClearedCookie(IsDotcomEnvironmentEnabled, dotcomid, customerId);
                }
                else
                {
                    generalUtility.CreateSecurityClearedCookie(IsDotcomEnvironmentEnabled, dotcomid, customerId);
                }
            }
            _logger.Submit(logData);
        }
    }

}
