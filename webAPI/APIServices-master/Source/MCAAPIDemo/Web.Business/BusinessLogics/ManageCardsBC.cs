using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Activation = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using System.Globalization;



namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{

    class ManageCardsBC : IManageCardsBC
    {        
        ILoggingService _logger;
        IConfigurationProvider _Config;

        public ManageCardsBC(ILoggingService logger, IConfigurationProvider config)
        {
            this._logger = logger;
            this._Config = config;
        }

        public ManageCardsViewModel GetManageCardsModel(List<HouseholdCustomerDetails> households, Dictionary<string,string> resources)
        {
            LogData logData = new LogData();
           //  Seems to be sensitive data
          //   logData.CaptureData("Request Object", households);
            try
            {
              
                ManageCardsViewModel model = new ManageCardsViewModel();
                households.ForEach(c => c.Cards.ForEach(ca => ca.LastUsed = checkTransactionTIme(ca, resources["lclMore"])));
                households.ForEach(c => c.CustomerType = c.CustomerID == c.PrimaryCustomerID ? "M" : "A");
                households.ForEach(c => c.IsCustomerBanned = (c.CustomerUseStatusID == BusinessConstants.CUSTOMER_BANNED ||
                                                                c.CustomerUseStatusID == BusinessConstants.CUSTOMER_LEFTSCHEME ||
                                                                c.CustomerUseStatusID == BusinessConstants.CUSTOMER_DATAREMOVED));

                
                model.Households = households;
                //seems to be sensitive data
             //   logData.CaptureData("Response Object", households);
                _logger.Submit(logData);
                return model;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in ManageCardsBC while getting ManageCardsModel", ex,
               new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY,logData }
                            });
            }
        }

        protected List<Clubcard> RenderCardsSectionByCustomers(List<HouseholdCustomerDetails> houseHoldCustomers, string culture)
        {
            LogData logData = new LogData();
            //  Seems to be sensitive data
         //   logData.CaptureData("Request Object", houseHoldCustomers);
            try
            {
                logData.RecordStep(string.Format("culture: {0}", culture));

                List<Clubcard> clubcardsDetails = new List<Clubcard>();
                foreach (HouseholdCustomerDetails customer in houseHoldCustomers)
                {
                    if (!this.IsCustomerBanned(customer))
                    {
                        customer.CustomerType = (customer.CustomerID == customer.PrimaryCustomerID) ? "M" : "A";
                        customer.MsgCardHolder = HttpContext.GetLocalResourceObject("~/Views/ManageCards/ViewMyCards.cshtml", "CardHolder", System.Globalization.CultureInfo.CurrentCulture).ToString();

                        //clubcardsDetails = this.GetClubcardsCustomerData(customer.CustomerID, culture);
                    }
                }

                string strSeparator = HttpContext.GetLocalResourceObject("~/Views/ManageCards/ViewMyCards.cshtml", "AndSeprator", System.Globalization.CultureInfo.CurrentCulture).ToString();//GetLocalResourceObject("AndSeprator").ToString();
                //manageCardsModel.getAllCustomerNames = GetAllCustomerNames(houseHoldCustomers, strSeparator);// GetAllCustomerNames(customers, strSeparator);

                //  Seems to be sensitive data
           //     logData.CaptureData("Response Object", houseHoldCustomers);
                _logger.Submit(logData);
                return clubcardsDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in ManageCardsBC while getting RenderCardsSectionByCustomers", ex,
              new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY,houseHoldCustomers }
                            });
            }
        }

        private bool IsCustomerBanned(HouseholdCustomerDetails customer)
        {
            LogData logData = new LogData();
            //  Seems to be sensitive data
            //  logData.CaptureData("Request Object", customer);
            try
            {
                //  Seems to be sensitive data
                //  logData.CaptureData("Response Object", customer);
                _logger.Submit(logData);
                return (customer.CustomerUseStatusID == BusinessConstants.CUSTOMER_BANNED ||
                        customer.CustomerUseStatusID == BusinessConstants.CUSTOMER_LEFTSCHEME ||
                        customer.CustomerUseStatusID == BusinessConstants.CUSTOMER_DATAREMOVED);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in ManageCardsBC while getting IsCustomerBanned status", ex,
            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY,customer }
                            });
            }
          
        }


        public bool IsHidden(string key)
        {
            LogData _logData = new LogData();
            bool isHidden = false;
            try
            {
                DbConfigurationItem isTitleHideConfig = _Config.GetConfigurations(DbConfigurationTypeEnum.ChinaHiddenFunctionality, key);
                isHidden = !isTitleHideConfig.IsDeleted && string.IsNullOrEmpty(isTitleHideConfig.ConfigurationValue1) && isTitleHideConfig.ConfigurationValue1.Equals("1");
                _logData.CaptureData("isHidden", isHidden);
                _logger.Submit(_logData);                

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ManageCardsBC while getting Is hidden Status.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return isHidden;
        }

        private string checkTransactionTIme(Clubcard card, string msg)
        {
            LogData _logData = new LogData();
             // Seems to be Sensitive Data
            //  logData.CaptureData("Request Object", card);
            try
            {
                string lastUsed = string.Empty;
                string transactionType = _Config.GetStringAppSetting(AppConfigEnum.Transaction_type);
                _logData.RecordStep(string.Format("Transaction_type: {0}", transactionType));

                string dateFormat = _Config.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);
                _logData.RecordStep(string.Format("dateFormat: {0}", dateFormat));

                if (card.TransactionDateTime < DateTime.Now.AddMonths(-6))
                {
                    lastUsed = msg;
                }
                else if (card.TransactionType.HasValue && card.TransactionType == transactionType.TryParse<Int32>())
                {
                    lastUsed = "-";
                }
                else if (card.TransactionDateTime.HasValue)
                {
                    lastUsed = card.TransactionDateTime.Value.ToString(dateFormat,CultureInfo.InvariantCulture);
                }
            //Response is not captured as it seems to be sensitive data
                _logger.Submit(_logData);
                return lastUsed;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in ManageCardsBC while Checking Transaction Time", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
        }

    }
}

