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
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class BoostsAtTescoController : BaseController
    {
        private IBoostsAtTescoBC _boostsAtTescoProvider;
        private string _boostPage = "BoostsAtTesco";
        IPDFGenerator _pdfProvider;
        IConfigurationProvider _configProvider = null;
        
        public BoostsAtTescoController()
        {
            _boostsAtTescoProvider = ServiceLocator.Current.GetInstance<IBoostsAtTescoBC>();
            _pdfProvider = ServiceLocator.Current.GetInstance<IPDFGenerator>();
            _configProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
        }

        public BoostsAtTescoController(IBoostsAtTescoBC _IboostsattescoBC, IPDFGenerator pdfGenerator, IConfigurationProvider configProvider)
        {
            this._boostsAtTescoProvider = _IboostsattescoBC;
            this._pdfProvider = pdfGenerator;
            this._configProvider = configProvider;
        }

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [SecurityCheck(Order = 2)]
        [PageAuthorization(Order = 3)]
        public ActionResult Home()
        {
            LogData logData = new LogData();
            try
            {
                long customerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = customerID.ToString();
                BoostsViewModel boostsViewModel = new BoostsViewModel();
                bool bExchangeEnabled = _boostsAtTescoProvider.IsExchangeEnabled();
                logData.CaptureData("IsExchangeEnabled", bExchangeEnabled);

                if (!bExchangeEnabled)
                {
                    logData.RecordStep(string.Format("IsExchangeEnabled {0}:", bExchangeEnabled));
                    Exception ex = new Exception();
                    ex.Data.Add(ParameterNames.FRIENDLY_ERROR_MESSAGE, "Boost is not enabled");
                    throw ex;
                }
                boostsViewModel.isCurrenltyBCVEPeriod = _boostsAtTescoProvider.IsCurrenltyBCVEPeriod();
                logData.CaptureData("isCurrenltyBCVEPeriod {0}:", boostsViewModel.isCurrenltyBCVEPeriod);
                if (boostsViewModel.isCurrenltyBCVEPeriod)
                {
                    ViewBag.pExpiryDesc = false;
                    boostsViewModel.rewardAndToken = _boostsAtTescoProvider.GetRewardAndTokens(customerID);
                    logData.CaptureData("boostsViewModel.rewardAndToken {0}:", boostsViewModel.rewardAndToken);
                }
                _logger.Submit(logData);
                return View(boostsViewModel);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in Boost Controller GET", ex, 
                    new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [AuthorizeUser(Order = 2)]
        [ActivationCheck(Order = 3)]
        [SecurityCheck(Order = 4)]
        [PageAuthorization(Order = 5)]
        public ActionResult Home(List<Token> rtmodel)
        {
            LogData logData = new LogData();         
            bool bPdfGenerated = false;
            BoostsViewModel boostsViewModel = new BoostsViewModel();
            List<Token> tokenModel = null;  
            List<Token> tList;   
            List<Token> selectedTokens = null;
            RewardAndToken rewardandtoken = new RewardAndToken();
            long customerID = default(long);

            try
            {
                customerID = this.CustomerId.TryParse<long>();
                logData.CustomerID = customerID.ToString();
                logData.CaptureData("rtmodel", rtmodel);
                ModelState.Clear();
                boostsViewModel.rewardAndToken = _boostsAtTescoProvider.GetRewardAndTokens(customerID);
                tokenModel = boostsViewModel.rewardAndToken.Tokens;
                tList = tokenModel;
                logData.CaptureData("tList", tList);
                selectedTokens = GetSelectedTokens(tList, rtmodel);
                logData.RecordStep(string.Format("selectedTokens Count {0}:", selectedTokens.Count));
                if (selectedTokens.Count > 0)
                {                 
                    BoostBackgroundTemplate boostBackGroundTemplate = GetBoostBackgroundTemplate();
                    using (MemoryStream document = _pdfProvider.GetPDFDocumentStream<Token, BoostBackgroundTemplate>(selectedTokens, boostBackGroundTemplate, null))
                    {
                        bPdfGenerated = true;
                        logData.RecordStep(string.Format("bPdfGenerated {0}:", bPdfGenerated));
                        _logger.Submit(logData);
                        return File(document.ToArray(), "application/pdf", Server.UrlEncode("Tokens.pdf"));
                    }
                }
                else
                {
                    ModelState.AddModelError("TokensNotSelected",
                        HttpContext.GetLocalResourceObject("~/Views/BoostsAtTesco/Home.cshtml",
                                                            "lblShowErrorMsgResource",
                                                            System.Globalization.CultureInfo.CurrentCulture).ToString());
                    _logger.Submit(logData);
                    return RedirectToAction("Home", "BoostsAtTesco", new { error = "noTokens" });
                }

              }
            finally
            {
                if (bPdfGenerated)
                {
                    logData.RecordStep(string.Format("bPdfGenerated true {0}:", bPdfGenerated));
                    this.RecordPrintStatus(selectedTokens, customerID, "T");
                }            
                rewardandtoken.Tokens = tokenModel;
                logData.CaptureData("tokenModel", tokenModel);
                _logger.Submit(logData);
            }
        }

        private void RecordPrintStatus(List<Token> selectedTokens, long customerID, string tokenFlag)
        {
            LogData logData = new LogData();
            try
            {
                this._boostsAtTescoProvider.RecordRewardTokenPrintDetails(selectedTokens, customerID, tokenFlag);
            }
            catch (Exception ex)
            {
                logData.RecordStep(String.Format("Failed to record reward token print details. More info - {0}", ex.ToString()));
            }
            finally
            {
                _logger.Submit(logData);
            }
        }

        private List<Token> GetSelectedTokens(List<Token> tokenList, List<Token> selectedTokens)
        {
            // Return a list of voucherdetails of the selected vouchers
            LogData _logData = new LogData();
            _logData.CaptureData("tokenList", tokenList);
            _logData.CaptureData("selectedTokens", selectedTokens);
            var IDs = (from v in selectedTokens where v.Selected select v).ToList();
            var result = tokenList.Where(v => IDs.Any(v2 => CryptoUtility.DecryptTripleDES(v2.TokenIDEncr).TryParse<Int64>() == v.TokenID));

            _logData.CaptureData("result", result.ToList());
            _logger.Submit(_logData);
            return result.ToList();
        }

        private BoostBackgroundTemplate GetBoostBackgroundTemplate()
        {
            LogData _logData = new LogData();
            BoostBackgroundTemplate template = new BoostBackgroundTemplate();
            try
            {
                template.ReplaceClubcardPrefix = this._configProvider.GetStringAppSetting(AppConfigEnum.ReplaceClubcardPrefix); 
                template.PrintBGImagePath = HttpContext.Server.MapPath(this._configProvider.GetStringAppSetting(AppConfigEnum.PrintBGImagePath));
                template.FontPath = this._configProvider.GetStringAppSetting(AppConfigEnum.FontPath);
                template.IsAlphaCodeRequired = this._configProvider.GetStringAppSetting(AppConfigEnum.IsAlphaCodeRequired);
                template.CultureDefaultloc = this._configProvider.GetStringAppSetting(AppConfigEnum.Culture);

                template.lblstrCurrencySymbol = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF", "lblstrCurrencySymbol").ToString(); 
                template.lblstrClubcardBoostatTesco = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrClubcardBoostatTesco").ToString();
                template.lblstrCustomerPrintedClubcardBoostToken = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrCustomerPrintedClubcardBoostToken").ToString();
                template.lblstrDateFormat = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrDateFormat").ToString();
                template.lblstrDatePrinted = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrDatePrinted").ToString();

                template.lblstrInaSingleTransaction = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrInaSingleTransaction").ToString();
                template.lblstrLine1 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine1").ToString();
                template.lblstrLine2 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine2").ToString();
                template.lblstrLine3 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine3").ToString();
                template.lblstrLine4 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine4").ToString();
                template.lblstrLine5 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine5").ToString();

                template.lblstrLine6 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine6").ToString();
                template.lblstrLine7 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine7").ToString();
                template.lblstrLine8 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine8").ToString();

                template.lblstrLine9 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine9").ToString();
                template.lblstrLine10 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine10").ToString();
                template.lblstrLine11 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine11").ToString();
                template.lblstrLine12 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine12").ToString();
                template.lblstrLine13 = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrLine13").ToString();

                template.lblstrOFF = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrOFF").ToString();
                template.lblstrOrMoreOn = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrOrMoreOn").ToString();
                template.lblstrSerialNumber = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrSerialNumber").ToString();
                template.lblstrToken = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrToken").ToString();
                template.lblstrTokenFileName = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrTokenFileName").ToString();

                template.lblstrValidUntil = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrValidUntil").ToString();
                template.lblstrWhenYouSpend = HttpContext.GetGlobalResourceObject("GenerateVouchersPDF","lblstrWhenYouSpend").ToString();

                _logData.CaptureData("template", template);
                _logger.Submit(_logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException(ex.Message, ex,
                              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, Resources.Messages.ApplicationError}
                            });
            }          
            _logger.Submit(_logData);
            return template;
        }

        /// <summary>
        ///  This method will check if any unspent voucher is available with user
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public bool IsUnSpentBoostTokensAvailable(long customerID)
        {
            LogData _logData = new LogData();
            try
            {
                _logger.Submit(_logData);
                return _boostsAtTescoProvider.IsUnSpentBoostTokensAvailable(customerID);
            }
            catch (Exception exp)
            {
                RedirectToAction("Index", "Error");
                return false;
            }           
        }
    }
}
