using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using PdfSharp.Pdf;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class CouponBC : ICouponBC
    {
        private ILoggingService _logger;
        private IServiceAdapter _clubcardCouponServiceAdapter;
        private IServiceAdapter _customerServiceAdapter;
        private IConfigurationProvider _Config;

        public CouponBC(IServiceAdapter clubcardCouponServiceAdapter, IServiceAdapter customerServiceAdapter, ILoggingService logger, IConfigurationProvider config)
        {
            this._clubcardCouponServiceAdapter = clubcardCouponServiceAdapter;
            this._customerServiceAdapter = customerServiceAdapter;
            this._logger = logger;
            this._Config = config;
        }

        /// <summary>
        /// Method to get the Available/ Unused coupons of the customer
        /// </summary>
        /// <param name="householdId">Household ID</param>
        /// <returns></returns>
        public List<CouponDetails> GetAvailableCoupons(out int totalCoupons, long householdId)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("householdId: {0}", householdId));
            List<CouponDetails> couponDetails = new List<CouponDetails>();
            totalCoupons = 0;
            try
            {
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_AVAILABLE_COUPONS);
                request.Parameters.Add(ParameterNames.HOUSEHOLD_ID, householdId);
                response = this._clubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);
                totalCoupons = response.OutParameters[ParameterNames.TOTAL_COUPONS_VALUE].TryParse<Int32>();
                couponDetails = (List<CouponDetails>)response.Data;
                var appsettings = _Config.GetStringAppSetting(AppConfigEnum.CouponImageFolder);
                _logData.CaptureData("appsettings", appsettings);
                couponDetails.ForEach(c => c.ThumbnailImageName = Path.Combine(appsettings, c.ThumbnailImageName));
                couponDetails.ForEach(c => c.HasImage = File.Exists(HttpContext.Current.Server.MapPath(Path.Combine(appsettings, c.FullImageName))));
                _logger.Submit(_logData);                
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CouponBC while getting Available Coupons.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });
            }
            return couponDetails;
        }

        /// <summary>
        /// Method to get the Redeemed Coupons for the customer
        /// </summary>
        /// <param name="householdId">Household ID</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public List<CouponDetails> GetRedeemedCoupons(long householdId, string culture, Dictionary<string, string> resources)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("householdId: {0}, culture: {1}", householdId, culture));
            List<CouponDetails> couponDetails = new List<CouponDetails>();
            try
            {
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_REDEEMED_COUPONS);
                request.Parameters.Add(ParameterNames.HOUSEHOLD_ID, householdId);
                request.Parameters.Add(ParameterNames.CULTURE, culture);
                _logData.CaptureData("Input Data for GetRedeemedCoupons", request);
                response = this._clubcardCouponServiceAdapter.Get<List<CouponDetails>>(request);
                couponDetails = (List<CouponDetails>)response.Data;
                couponDetails.ForEach(c => c.CouponStatus = c.CouponStatus.ToUpper().Equals("REDEEM") ? resources["strCouponUsed"] : c.CouponStatus.ToUpper().Equals("UNREDEEM") ? resources["strCouponVoided"] : string.Empty);
                couponDetails.ForEach(c => c.RedemptionCount = string.Format("{0} {1} {2}", c.RedemptionCount, resources["strOf"], c.TotalRedemption));
                _logData.CaptureData("Coupon Details", couponDetails);
                _logger.Submit(_logData);     
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CouponBC while getting Redeemed Coupons.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });               
            }
            return couponDetails;
        }

        /// <summary>
        /// Method to get the coupon background template
        /// </summary>
        /// <param name="title"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public PdfBackgroundTemplate GetCouponBackgroungTemplate(string title, string culture)
        {
            LogData _logData = new LogData();
            _logData.RecordStep(string.Format("title: {0}, culture: {1}", title, culture));
            PdfBackgroundTemplate template = new PdfBackgroundTemplate();
            try
            {
                template.lblstrTitleCoupon = title;
                template.IsHideCustomerName = _Config.GetStringAppSetting(AppConfigEnum.IsHideCustomerName);
                template.PrintBGImagePath = HttpContext.Current.Server.MapPath(_Config.GetStringAppSetting(AppConfigEnum.CouponImageFolder));
                template.FontName = _Config.GetStringAppSetting(AppConfigEnum.FontName);
                template.DateFormat = _Config.GetStringAppSetting(AppConfigEnum.DisplayDateFormat);
                template.Culture = culture;
                template.ItemPerPage = 6;
                template.DocumentWidth = 1200;
                template.Left = 30;
                template.Top = 10;
               
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CouponBC while getting Coupon Background Template.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });        
            }
            _logData.CaptureData("Coupon Background Template", template);
            _logger.Submit(_logData);     
            return template;
        }


        public bool RecordPrintAtHomeDetails(List<CouponDetails> couponList, string customerId, AccountDetails customerAccountDetails)
        {
            bool chk = false;
            DataTable dtPrintDetail = new DataTable();
            DataSet dsCouponsToInsert = new DataSet("DocumentElement");
            string errorXml = string.Empty;
            LogData _logData = new LogData();
          //  _logData.RecordStep(string.Format("CustomerID: {0}", customerId));
            _logData.CaptureData("Coupon Details", couponList);
            try
            {
                MCAResponse response = new MCAResponse();
                MCARequest request = new MCARequest();

                dtPrintDetail.TableName = "PrintDetails";
                dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
                dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
                dtPrintDetail.Columns.Add("Flag", typeof(Char));
                dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
                dtPrintDetail.Columns.Add("VoucherID", typeof(String));
                dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));                
                foreach (CouponDetails coupon in couponList)
                {
                    DataRow dr = dtPrintDetail.NewRow();
                    dr["CustomerID"] = customerId;
                    dr["PrintDate"] = DateTime.Now;
                    dr["CCNumber"] = customerAccountDetails.ClubcardID;
                    dr["Flag"] = "C";
                    dr["ExpiryDate"] = coupon.ExpiryDate;
                    dr["VoucherID"] = coupon.BarcodeNumber;
                    dtPrintDetail.Rows.Add(dr);
                }                
                dsCouponsToInsert.Tables.Add(dtPrintDetail);
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
                request.Parameters.Add(ParameterNames.DS_COUPONS, dsCouponsToInsert);
                response = this._customerServiceAdapter.Set<CouponDetails>(request);
                chk = response.Status;
                _logData.RecordStep(string.Format("Response Status: {0}", chk));
                _logger.Submit(_logData);     
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Failed in CouponBC while getting status for RecordPrintatHomeDetails.", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logData }
                            });        
            }
            return chk;
        }
    
    }
}
