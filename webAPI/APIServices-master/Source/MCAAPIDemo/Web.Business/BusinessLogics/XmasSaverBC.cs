using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;


namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class XmasSaverBC : IXmasSaverBC
    {
        ILoggingService _logger = null;
        IServiceAdapter _clubcardServiceAdapter;
        IServiceAdapter _customerServiceAdapter;
        IServiceAdapter _smartVoucherAdapter;
        IAccountBC _accountProvider;

        XmasSaverViewModel xmasSaverModel = new XmasSaverViewModel();
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();
        string xmasSaverYear = string.Empty;
        MCARequest request;
        MCAResponse response;

        public XmasSaverBC(IServiceAdapter clubcardServiceAdapter,
                            IServiceAdapter customerServiceAdapter,
                            IServiceAdapter smartVoucherServiceAdapter,
                            ILoggingService logger,
                            IAccountBC accountProvider)
        {
            this._clubcardServiceAdapter = clubcardServiceAdapter;
            this._customerServiceAdapter = customerServiceAdapter;
            this._smartVoucherAdapter = smartVoucherServiceAdapter;
            this._logger = logger;
            this._accountProvider = accountProvider;
        }

        #region Public

        public bool CheckCustomerIsXmasClubMember(string customerId, string culture)
        {
            LogData logData = new LogData();
            


            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.IS_XMAS_CLUBMEMBER);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                response = _clubcardServiceAdapter.Get<bool>(request);

                _logger.Submit(logData);
                return response.Data.TryParse<Boolean>();

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while checking Is the customer is XmasClubMember.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        public XmasSaverViewModel GetXmasSaverViewModel(string CustomerID, string Cluture)
        {

            LogData logData = new LogData();
            

            decimal totalSumXmasVoucher = 0;
            try
            {
                this.SetXmasDates();
                xmasSaverModel.IsOptionsAndBenefitsEnabled = this.IsOptionAndBenifitEnabled();
                xmasSaverModel.XmasSaverYear = xmasSaverYear;
                xmasSaverModel.xmasSaverTopUpModel = this.GetXmasSaverTopUpDetails(CustomerID, Cluture);
                xmasSaverModel.xmasSaverVoucherSavedModel = this.GetXmasVoucherSavedSoFar(CustomerID, Cluture, startDate, endDate);
                this.CalculateBonusAndGetXmasSaverSummary(out totalSumXmasVoucher);
                xmasSaverModel.xmasSaverHeaderModel = this.GetXmasSaverHeaderValues(xmasSaverYear, totalSumXmasVoucher);
                
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting all the details to populate XmasSaverViewModel.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return xmasSaverModel;
        }

        #endregion

        #region Private

        private void SetXmasDates()
        {

            LogData logData = new LogData();
            

            try
            {
                DbConfiguration dbConfigs = _accountProvider.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, System.Globalization.CultureInfo.CurrentCulture.Name);
                DbConfigurationItem xsConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverCurrDates);
                DbConfigurationItem xsNextConfig = dbConfigs.ConfigurationItems.Find(c => c.ConfigurationName == DbConfigurationItemNames.XmasSaverNextDates);

                DateTime strXmasCurrStartDate = xsConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                DateTime strXmasCurrEndDate = xsConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();

                DateTime strXmasNextStartDate = xsNextConfig.ConfigurationValue1.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();
                DateTime strXmasNextEndDate = xsNextConfig.ConfigurationValue2.TryParse<DateTime>().ToShortDateString().TryParse<DateTime>();

                if (DateTime.Now.Date < strXmasNextStartDate)
                {
                    startDate = strXmasCurrStartDate;
                    endDate = strXmasCurrEndDate;
                    xmasSaverYear = (DateTime.Now.Year).ToString();
                }
                else if (DateTime.Now.Date >= strXmasNextStartDate)
                {
                    startDate = strXmasNextStartDate;
                    endDate = strXmasNextEndDate;
                    xmasSaverYear = (DateTime.Now.Year + 1).ToString();
                }

                logData.RecordStep(string.Format("Xmas Start date: {0} , Xmas End date: {1}, xmas saver year : {2}", startDate, endDate, xmasSaverYear));
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting all the start and end date of the xmas saver.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private XmasSaverVoucherSavedViewModel GetXmasVoucherSavedSoFar(string customerID, string culture, DateTime startDate, DateTime endDate)
        {

            LogData logData = new LogData();
            


            XmasSaverVoucherSavedViewModel xmasVoucherSavedModel = new XmasSaverVoucherSavedViewModel();
            string stDate = startDate.ToString("yyyyMMdd");
            string enDate = endDate.ToString("yyyyMMdd");
            int remainders = 0;
            int rewardPointsForCP = 0;
            int prevCPPnts = 0;
            string voucherValue = string.Empty;
            decimal totalSumXmasVoucher = 0;

            logData.RecordStep(string.Format("Customer ID: {0} , Xmas start date: {1}, xmas endDate : {2}", customerID, startDate, endDate));
            try
            {
                string cardNumber = GetCustomerClubcard(customerID, culture);
                

                List<RewardAndPoints> rewardAndPointsList = GetRewardAndPointsList(cardNumber, stDate, enDate);

                

                if (rewardAndPointsList != null && rewardAndPointsList.Count > 0)
                {
                    foreach (RewardAndPoints rewardAndPoints in rewardAndPointsList)
                    {

                        if (!string.IsNullOrEmpty(rewardAndPoints.Reward_Points.ToString()))
                        {
                            rewardPointsForCP = (Convert.ToInt32(rewardAndPoints.Reward_Points.ToString()) - prevCPPnts) + remainders;
                            prevCPPnts = Convert.ToInt32(rewardAndPoints.Reward_Points.ToString());
                            voucherValue = VoucherDisplay(rewardPointsForCP, out remainders);

                            totalSumXmasVoucher = totalSumXmasVoucher + Convert.ToDecimal(voucherValue, CultureInfo.InvariantCulture);

                            xmasVoucherSavedModel.StatementDate.Add(rewardAndPoints.StatementDate.ToString());
                            xmasVoucherSavedModel.VoucherValue.Add(voucherValue);
                        }
                    }
                }
                xmasVoucherSavedModel.TotalVoucherSaver = totalSumXmasVoucher;
                logData.RecordStep(string.Format("Total sum of xmas voucher : {0}", totalSumXmasVoucher));

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting voucher collected so far for the customer", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return xmasVoucherSavedModel;

        }

        private XmasSaverHeaderViewModel GetXmasSaverHeaderValues(string xmasSaverYear, decimal totalSumXmasVoucher)
        {
            LogData logData = new LogData();
            
            XmasSaverHeaderViewModel xmasHeaderModel = new XmasSaverHeaderViewModel();
            try
            {
                xmasHeaderModel.TotalVoucherSaver = totalSumXmasVoucher;
                xmasHeaderModel.XmasSaverYear = xmasSaverYear;

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while assigning total sum of xmas saaver voucher.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

            return xmasHeaderModel;

        }

        private XmasSaverTopUpViewModel GetXmasSaverTopUpDetails(string customerId, string Cluture)
        {

            LogData logData = new LogData();
            


            XmasSaverTopUpViewModel xmasSaverTopUpModel = new XmasSaverTopUpViewModel();
            try
            {
                xmasSaverTopUpModel.ChristmasSaverSummaryList = this.GetXmasSummaryDetails(customerId.TryParse<Int64>(), Cluture);
                xmasSaverTopUpModel.sumTtlToppedUpMoney = (xmasSaverTopUpModel.ChristmasSaverSummaryList != null) ? xmasSaverTopUpModel.ChristmasSaverSummaryList.Sum(m => Convert.ToDecimal(m.AmountSpent)) : 0;
                logData.CaptureData("Xmans saver TopUp details", xmasSaverTopUpModel);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting the values for Xmas saver topup model", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return xmasSaverTopUpModel;
        }

        private List<RewardAndPoints> GetRewardAndPointsList(string cardNumber, string stDate, string enDate)
        {
            LogData logData = new LogData();
            

            try
            {

                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMERVOUCHERVAL_CPS);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, cardNumber);
                request.Parameters.Add(ParameterNames.START_DATE, stDate);
                request.Parameters.Add(ParameterNames.END_DATE, enDate);

                response = _smartVoucherAdapter.Get<RewardAndPoints>(request);

                
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting rewards and points list from service.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return (List<RewardAndPoints>)response.Data;
        }

        private List<ChristmasSaverSummary> GetXmasSummaryDetails(long customerId, string Cluture)
        {
            LogData logData = new LogData();
            

            try
            {
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CHRISTMAS_SAVER_SUMMARY);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.START_DATE, startDate);
                request.Parameters.Add(ParameterNames.END_DATE, endDate);
                request.Parameters.Add(ParameterNames.CULTURE, Cluture);
                response = _clubcardServiceAdapter.Get<List<ChristmasSaverSummary>>(request);
                List<ChristmasSaverSummary> summaryList = response.Data as List<ChristmasSaverSummary>;
                logData.CaptureData("Response of  Christmas saver summary details", response);
                _logger.Submit(logData);

                return summaryList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting xmas saver summary details.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            finally
            {
                _logger.Dispose();
            }
        }

        private string VoucherDisplay(int totalPoints, out int residual)
        {
            LogData logData = new LogData();
            
            try
            {
                if (totalPoints < BusinessConstants.REWARDEE_LIMIT)
                {
                    residual = totalPoints;
                    return "0.0";
                }
                int remd = totalPoints % 50;
                residual = remd;
                int correctedPoints = totalPoints - remd;
                float dispVal = ((float)(correctedPoints)) / 100;
                string strDispVal = dispVal.ToString();
                if (strDispVal.Contains("."))
                {
                    string temp = strDispVal.Substring(strDispVal.Length - 2, 1);
                    if (temp != "0")
                        strDispVal += "0";
                }
                else
                {
                    strDispVal += ".00";
                }

                _logger.Submit(logData);
                return strDispVal;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while calculating the voucher value to be displayed.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        private void CalculateBonusAndGetXmasSaverSummary(out decimal totalSumXmasVoucher)
        {
            LogData logData = new LogData();
            

            try
            {

                string TopupRange = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.TopupRange].ConfigurationValue1.TryParse<string>();
                string BonusVooucher = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.BonusVoucher].ConfigurationValue1.TryParse<string>();
                string MaxBonusVoucher = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.MaxBonusVoucher].ConfigurationValue1.TryParse<string>();
                string Topuptoreceivemaxbonus = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.Topuptoreceivemaxbonus].ConfigurationValue1.TryParse<string>();



                decimal sumTtlToppedUpMoney = xmasSaverModel.xmasSaverTopUpModel.sumTtlToppedUpMoney;
                decimal sumVoucherSavedSoFar = xmasSaverModel.xmasSaverVoucherSavedModel.TotalVoucherSaver;

                totalSumXmasVoucher = 0;
                XmasSaverSummaryViewModel xmasSaverSummaryModel = new XmasSaverSummaryViewModel();


                xmasSaverSummaryModel.displayBonusVocuherRow = false;
                xmasSaverSummaryModel.displayCongratesMsgForMaxBonus = false;
                string[] topupRange = TopupRange.Split(',');
                string[] bonusVoucher = BonusVooucher.Split(',');
                xmasSaverSummaryModel.displayCongratesMsgForMinBonus = false;
                xmasSaverModel.xmasSaverTopUpModel.displayMsgForTopupBonus = false;

                xmasSaverSummaryModel.toppedupVouchersSavedSofar = sumTtlToppedUpMoney;
                xmasSaverSummaryModel.clubcardVouchersSavedSofar = sumVoucherSavedSoFar;

                int countoftopuprange = (topupRange.Length) - 1;
                for (int i = 0; i < countoftopuprange; i++)
                {
                    if ((sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i], CultureInfo.InvariantCulture))
                           && (sumTtlToppedUpMoney < Convert.ToDecimal(topupRange[i + 1], CultureInfo.InvariantCulture)))
                    {
                        xmasSaverSummaryModel.displayBonusVocuherRow = true;
                        xmasSaverSummaryModel.bonusVouchersSavedSofar = Convert.ToDecimal(bonusVoucher[i], CultureInfo.InvariantCulture);
                        sumVoucherSavedSoFar = sumVoucherSavedSoFar + sumTtlToppedUpMoney + Convert.ToDecimal(bonusVoucher[i], CultureInfo.InvariantCulture);
                        xmasSaverSummaryModel.CongratsSectionValues.recievedBonusVocuher = Convert.ToDecimal(bonusVoucher[i], CultureInfo.InvariantCulture);

                        if (bonusVoucher[i] == "0")
                        {
                            xmasSaverModel.xmasSaverTopUpModel.displayMsgForTopupBonus = true;
                            xmasSaverModel.xmasSaverTopUpModel.topUpYourAccountUpto = topupRange[i + 1];
                            decimal val = Math.Round(Convert.ToDecimal(0), 2);
                            xmasSaverModel.xmasSaverTopUpModel.rewardedBonus = Convert.ToDecimal(bonusVoucher[i + 1], CultureInfo.InvariantCulture);
                            xmasSaverModel.xmasSaverTopUpModel.topupWithOverValue = Convert.ToDecimal(topupRange[i + 2], CultureInfo.InvariantCulture);
                            xmasSaverModel.xmasSaverTopUpModel.bonuVoucherinReturn = Convert.ToDecimal(bonusVoucher[i + 2], CultureInfo.InvariantCulture);

                        }
                        else
                        {
                            xmasSaverSummaryModel.displayCongratesMsgForMinBonus = true;
                            xmasSaverSummaryModel.CongratsSectionValues.bonusValueFor50 = Convert.ToDecimal(bonusVoucher[i], CultureInfo.InvariantCulture);
                            xmasSaverSummaryModel.CongratsSectionValues.extraValueNeedToBeSaved = Convert.ToDecimal(topupRange[i + 1], CultureInfo.InvariantCulture) - sumTtlToppedUpMoney;
                            xmasSaverSummaryModel.CongratsSectionValues.requiredValueToMakeBonusVoucher = Convert.ToDecimal(bonusVoucher[i + 1], CultureInfo.InvariantCulture);
                        }

                    }
                    else if (sumTtlToppedUpMoney >= Convert.ToDecimal(topupRange[i + 1], CultureInfo.InvariantCulture) && sumTtlToppedUpMoney >= Convert.ToDecimal(Topuptoreceivemaxbonus, CultureInfo.InvariantCulture))
                    {
                        if (i == 0)
                        {
                            xmasSaverSummaryModel.displayBonusVocuherRow = true;
                            xmasSaverSummaryModel.bonusVouchersSavedSofar = Convert.ToDecimal(MaxBonusVoucher, CultureInfo.InvariantCulture);
                            sumVoucherSavedSoFar = sumVoucherSavedSoFar + Convert.ToDecimal(MaxBonusVoucher, CultureInfo.InvariantCulture) + sumTtlToppedUpMoney;
                            xmasSaverSummaryModel.CongratsSectionValues.recievedBonusVocuher = Convert.ToDecimal(MaxBonusVoucher, CultureInfo.InvariantCulture);
                            xmasSaverSummaryModel.displayCongratesMsgForMaxBonus = true;
                        }
                    }

                }

                xmasSaverSummaryModel.totalVouchersSavedSofar = sumVoucherSavedSoFar;
                totalSumXmasVoucher = sumVoucherSavedSoFar;

                xmasSaverModel.xmasSaverSummaryModel = xmasSaverSummaryModel;

                logData.CaptureData("Total voucher saved so far:", sumVoucherSavedSoFar);
                logData.CaptureData("Total xmas saaver summary model", xmasSaverSummaryModel);
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while Calculating the Bonus And Getting the XmasSaverSummary.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        private string GetCustomerClubcard(string customerId, string culture)
        {
            LogData logData = new LogData();
            
            
            AccountDetails customerAccountDetails = new AccountDetails();
            string clubcardnumber = string.Empty;
            try
            {

                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMER_ACCOUNT_DETAILS);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);

                response = _clubcardServiceAdapter.Get<AccountDetails>(request);
                customerAccountDetails = (AccountDetails)response.Data;
                if (response.Status)
                {
                    if (customerAccountDetails != null)
                    {
                        clubcardnumber = customerAccountDetails.ClubcardID.TryParse<string>();
                        if (string.IsNullOrEmpty(clubcardnumber))
                        {
                            logData.RecordStep("Clubcard number is not available");
                        }
                    }
                }

                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting clubcard number for the xmas saver customer.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return clubcardnumber;
        }

        private bool IsOptionAndBenifitEnabled()
        {

            LogData logData = new LogData();
            
            try
            {
                _logger.Submit(logData);
                return ValidatorUtility.AuthorizePage("MYACCOUNTDETAILS", "OPTIONSANDBENEFITS");
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while checking if optionandbenefit page is enablde or not.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        #endregion
    }
}
