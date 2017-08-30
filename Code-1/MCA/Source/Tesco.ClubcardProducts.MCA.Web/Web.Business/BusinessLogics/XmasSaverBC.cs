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
using System.Threading.Tasks;


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

        public XmasSaverViewModel GetXmasSaverViewModel(string CustomerID, string culture)
        {
            LogData logData = new LogData();
            decimal totalSumXmasVoucher = 0;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            try
            {
                XmasSaverDates dates = this.GetXmasDates();
                startDate = dates.StartDate;
                endDate = dates.EndDate;
                xmasSaverModel.IsOptionsAndBenefitsEnabled = this.IsOptionAndBenifitEnabled();
                xmasSaverModel.XmasSaverYear = dates.Year;
                xmasSaverModel.xmasSaverTopUpModel = this.GetXmasSaverTopUpDetails(CustomerID, culture, startDate, endDate);
                AccountDetails myAccountDetails = _accountProvider.GetMyAccountDetail(CustomerID.TryParse<Int64>(), culture);
                string cardNumber = myAccountDetails != null ? myAccountDetails.ClubcardID.TryParse<string>() : string.Empty;
                xmasSaverModel.xmasSaverVoucherSavedModel = this.GetXmasVoucherSavedSoFar(cardNumber, culture, startDate, endDate);
                xmasSaverModel = this.CalculateBonus(xmasSaverModel);
                totalSumXmasVoucher = xmasSaverModel.xmasSaverSummaryModel == null ? 0 : xmasSaverModel.xmasSaverSummaryModel.totalVouchersSavedSofar;
                xmasSaverModel.xmasSaverHeaderModel = this.GetXmasSaverHeaderValues(dates.Year, totalSumXmasVoucher);                
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

        public string GetVouchersForXmasSaverCustomer(string custId, string culture)
        {
            LogData logData = new LogData();
            string voucherValue = string.Empty;
            try
            {                
                Task<object> task1 = new Task<object>(() => GetXmasDates());
                Task<object> task2 = new Task<object>(() => _accountProvider.GetMyAccountDetail(custId.TryParse<long>(), culture));
                task1.Start();
                task2.Start();
                Task.WaitAll(new Task[] { task1, task2 });

                XmasSaverDates xmasDates = task1.Result as XmasSaverDates;
                AccountDetails accountDetails = task2.Result as AccountDetails;
                string cardNumber = accountDetails != null ? accountDetails.ClubcardID.TryParse<string>() : string.Empty;

                Task<object> task3 = new Task<object>(() => GetXmasSaverTopUpDetails(custId, culture, xmasDates.StartDate, xmasDates.EndDate));
                Task<object> task4 = new Task<object>(() => GetXmasVoucherSavedSoFar(cardNumber, culture, xmasDates.StartDate, xmasDates.EndDate));
                task3.Start();
                task4.Start();
                Task.WaitAll(new Task[] { task3, task4 });

                XmasSaverViewModel xmasModel = new XmasSaverViewModel();
                xmasModel.XmasSaverYear = xmasDates.Year;
                xmasModel.xmasSaverTopUpModel = task3.Result as XmasSaverTopUpViewModel;
                xmasModel.xmasSaverVoucherSavedModel = task4.Result as XmasSaverVoucherSavedViewModel;
                xmasModel = CalculateBonus(xmasModel);
                voucherValue = xmasModel.xmasSaverSummaryModel.totalVouchersSavedSofar.ToString();            
                _logger.Submit(logData);
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting total vouchers saved", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
            return voucherValue;
        }

        public XmasSaverTopUpViewModel GetXmasSaverTopUpDetails(string customerId, string Cluture, DateTime startDate, DateTime endDate)
        {

            LogData logData = new LogData();
            XmasSaverTopUpViewModel xmasSaverTopUpModel = new XmasSaverTopUpViewModel();
            try
            {
                xmasSaverTopUpModel.ChristmasSaverSummaryList = this.GetXmasSummaryDetails(customerId.TryParse<Int64>(), Cluture, startDate, endDate);
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

        public XmasSaverDates GetXmasDates()
        {
            LogData logData = new LogData();
            XmasSaverDates xmasDates = new XmasSaverDates();
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
                    xmasDates.StartDate = strXmasCurrStartDate;
                    xmasDates.EndDate = strXmasCurrEndDate;
                    xmasDates.Year = (DateTime.Now.Year).ToString();
                }
                else if (DateTime.Now.Date >= strXmasNextStartDate)
                {
                    xmasDates.StartDate = strXmasNextStartDate;
                    xmasDates.EndDate = strXmasNextEndDate;
                    xmasDates.Year = (DateTime.Now.Year + 1).ToString();
                }

                logData.RecordStep(string.Format("Xmas Start date: {0} , Xmas End date: {1}, xmas saver year : {2}", xmasDates.StartDate, xmasDates.EndDate, xmasDates.Year));
                _logger.Submit(logData);
                return xmasDates;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while getting all the start and end date of the xmas saver.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        public XmasSaverVoucherSavedViewModel GetXmasVoucherSavedSoFar(string cardNumber, string culture, DateTime startDate, DateTime endDate)
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

            logData.RecordStep(string.Format("Xmas start date: {0}, xmas endDate : {1}", startDate, endDate));
            try
            {
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

        public XmasSaverViewModel CalculateBonus(XmasSaverViewModel model)
        {
            LogData logData = new LogData();
            try
            {

                string TopupRange = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.TopupRange].ConfigurationValue1.TryParse<string>();
                string BonusVooucher = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.BonusVoucher].ConfigurationValue1.TryParse<string>();
                string MaxBonusVoucher = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.MaxBonusVoucher].ConfigurationValue1.TryParse<string>();
                string Topuptoreceivemaxbonus = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings][DbConfigurationItemNames.Topuptoreceivemaxbonus].ConfigurationValue1.TryParse<string>();
                
                decimal sumTtlToppedUpMoney = model.xmasSaverTopUpModel.sumTtlToppedUpMoney;
                decimal sumVoucherSavedSoFar = model.xmasSaverVoucherSavedModel.TotalVoucherSaver;
                
                XmasSaverSummaryViewModel xmasSaverSummaryModel = new XmasSaverSummaryViewModel();

                xmasSaverSummaryModel.displayBonusVocuherRow = false;
                xmasSaverSummaryModel.displayCongratesMsgForMaxBonus = false;
                string[] topupRange = TopupRange.Split(',');
                string[] bonusVoucher = BonusVooucher.Split(',');
                xmasSaverSummaryModel.displayCongratesMsgForMinBonus = false;
                model.xmasSaverTopUpModel.displayMsgForTopupBonus = false;

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
                            model.xmasSaverTopUpModel.displayMsgForTopupBonus = true;
                            model.xmasSaverTopUpModel.topUpYourAccountUpto = topupRange[i + 1];
                            decimal val = Math.Round(Convert.ToDecimal(0), 2);
                            model.xmasSaverTopUpModel.rewardedBonus = Convert.ToDecimal(bonusVoucher[i + 1], CultureInfo.InvariantCulture);
                            model.xmasSaverTopUpModel.topupWithOverValue = Convert.ToDecimal(topupRange[i + 2], CultureInfo.InvariantCulture);
                            model.xmasSaverTopUpModel.bonuVoucherinReturn = Convert.ToDecimal(bonusVoucher[i + 2], CultureInfo.InvariantCulture);

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
                model.xmasSaverSummaryModel = xmasSaverSummaryModel;

                logData.CaptureData("Total voucher saved so far:", sumVoucherSavedSoFar);
                logData.CaptureData("Total xmas saaver summary model", xmasSaverSummaryModel);
                _logger.Submit(logData);
                return model;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in XmassaverBC while Calculating the Bonus And Getting the XmasSaverSummary.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        #endregion

        #region Private

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

        private List<ChristmasSaverSummary> GetXmasSummaryDetails(long customerId, string Cluture, DateTime startDate, DateTime endDate)
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
                logData.RecordStep("got the response from service call.");
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
