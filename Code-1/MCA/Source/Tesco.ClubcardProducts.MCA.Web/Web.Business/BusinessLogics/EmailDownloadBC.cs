using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using System.IO;
using System.Web;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using System.Xml;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.VoucherandCouponDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    
    
    
    public class EmailDownloadBC : IEmailDownloadBC
    {
        IServiceAdapter _customerServiceAdapter;
        IServiceAdapter _smartvoucherAdapter;
        private readonly ILoggingService _logger;
        MCAResponse response;
        MCARequest request;
        public EmailDownloadBC(IServiceAdapter customerServiceAdapter, IServiceAdapter smartVoucherServiceAdapter, ILoggingService logger)
        {
            _customerServiceAdapter = customerServiceAdapter;
            _smartvoucherAdapter = smartVoucherServiceAdapter;
            this._logger = logger;
           
        }
    public  string GetCustomeridbyGUID(string guid)
        {
            string sCustomerId = string.Empty;
            LogData _logdata = new LogData();
            try
            {
               // response = _customerServiceAdapter.Get<GetCustomerIDbyGUID>(request);
                request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CUSTOMERID_BY_GUID);
                request.Parameters.Add(ParameterNames.CUSTOMER_GUID, guid);
                _logdata.CaptureData("Request object", request);
                response = _customerServiceAdapter.Get<string>(request);
                _logdata.RecordStep("Response received from adapter successfully");
                _logdata.CaptureData("response received", response);
                 sCustomerId = (string)response.Data;
                 _logdata.CustomerID = sCustomerId;
                 _logger.Submit(_logdata);
            }
            catch(Exception ex)
            {
                throw GeneralUtility.GetCustomException("", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
            }
            return sCustomerId;  
        }
    public List<HouseholdCustomerDetails> GetHouseholdDetailsofCustomer(long customerid, string culture)
     {
         List<HouseholdCustomerDetails> customerDetails=null;
         LogData _logdata = new LogData();
         try
         {
             request = new MCARequest();
             request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_HOUSEHOLD_DETAILS_BY_CUSTOMER);
             request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerid);
             request.Parameters.Add(ParameterNames.CULTURE, culture);
             
             response = _customerServiceAdapter.Get<List<HouseholdCustomerDetails>>(request);
             customerDetails = response.Data as List<HouseholdCustomerDetails>;
             _logdata.RecordStep("Response received from adapter successfully");
             
             _logger.Submit(_logdata);
         }
         catch(Exception ex)
         {
             throw GeneralUtility.GetCustomException("failed while getting household details of customer", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
         }

         return customerDetails;
     }
    public List<VoucherDetails> GetUnusedVoucherDeatils(long customerID, long cardNumber, string culture)
    {
        List<VoucherDetails> unusedvoucherdetails = null;
        LogData _logdata = new LogData();
        try
        {
            request = new MCARequest();
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_UNUSED_VOUCHER_DETAILS);
            request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
            request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, cardNumber);
            request.Parameters.Add(ParameterNames.CULTURE, culture);
            response = _smartvoucherAdapter.Get<List<VoucherDetails>>(request);
            unusedvoucherdetails = response.Data as List<VoucherDetails>;
            _logdata.RecordStep("Response received from adapter successfully");
            _logger.Submit(_logdata);
        }
        catch(Exception ex)
        {
            throw GeneralUtility.GetCustomException("failed while getting unused voucher details of customer", ex, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
        }
        return unusedvoucherdetails;
    }
    public bool RecordCouponAndVoucherPrintedDataSet(List<VoucherDetails> voucherDetailsList, List<CouponDetails> couponDetailsList, string customerID, string cardNumber, int typeofcall)
    {
      
        bool chk = false;
        LogData _logdata = new LogData();
        MCAResponse response = new MCAResponse();
        MCARequest request = new MCARequest();
        DataTable dtPrintDetail = new DataTable();
        try
        {
            
            string errorXml = string.Empty;
            DataSet dsPrintVoucherDetails = null;

            dtPrintDetail.TableName = "PrintDetails";
            dtPrintDetail.Columns.Add("CustomerID", typeof(Int64));
            dtPrintDetail.Columns.Add("PrintDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("Value", typeof(Decimal));
            dtPrintDetail.Columns.Add("VoucherID", typeof(string));
            dtPrintDetail.Columns.Add("VoucherType", typeof(string));
            dtPrintDetail.Columns.Add("ExpiryDate", typeof(DateTime));
            dtPrintDetail.Columns.Add("CCNumber", typeof(Int64));
            dtPrintDetail.Columns.Add("Flag", typeof(string));

            if (voucherDetailsList != null)
            {
                
                foreach (VoucherDetails voucherDetails in voucherDetailsList)
                {
                    DataRow dr = dtPrintDetail.NewRow();

                    dr["CustomerID"] = customerID;

                    dr["PrintDate"] = DateTime.Now;
                    dr["Value"] = voucherDetails.Value; // row["Value"];
                    dr["VoucherID"] = voucherDetails.BarCode;//  row["22DigitVoucher_Number"];

                    //Voucher type
                    if (voucherDetails.VoucherType.ToString() == "1") //row["Voucher Type"]
                    {
                        dr["VoucherType"] = "Clubcard";
                    }
                    else if (voucherDetails.VoucherType.ToString() == "4")
                    {
                        dr["VoucherType"] = "Bonus";
                    }
                    if (voucherDetails.VoucherType.ToString() == "5")
                    {
                        dr["VoucherType"] = "Top Up";
                    }

                    dr["ExpiryDate"] = voucherDetails.ExpiryDate;// row["Expiry Date"];
                    dr["CCNumber"] = cardNumber;
                    if (typeofcall == 0)
                    {
                        dr["Flag"] = "EV";
                    }
                    else
                    {
                        dr["Flag"] = "LV";
                    }

                    dtPrintDetail.Rows.Add(dr);
                }
            }
            if (couponDetailsList != null)
            {
                _logdata.CaptureData("CouponDetails", couponDetailsList);
                foreach (CouponDetails couponDetails in couponDetailsList) //   foreach (DataRow row in dsCoupons.Tables[0].Rows)
                {
                    DataRow dr = dtPrintDetail.NewRow();
                    dr["CustomerID"] = customerID;
                    dr["PrintDate"] = DateTime.Now;
                    if (typeofcall == 0)
                    {
                        dr["Flag"] = "EC";
                    }
                    else
                    {
                        dr["Flag"] = "LC";
                    }
                    dr["ExpiryDate"] = couponDetails.ExpiryDate; // row["ExpiryDate"].ToString();
                    dr["VoucherID"] = couponDetails.BarcodeNumber; // row["SmartBarcode"].ToString();
                    dr["CCNumber"] = cardNumber;
                    dtPrintDetail.Rows.Add(dr);
                }
            }

            StringWriter sw = new StringWriter();
            dtPrintDetail.WriteXml(sw, false);
            string strXML = sw.ToString();

            XmlDocument resulDoc = new XmlDocument();
            resulDoc.LoadXml(strXML);
            dsPrintVoucherDetails = new DataSet();
            dsPrintVoucherDetails.ReadXml(new XmlNodeReader(resulDoc));
            
            request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.RECORD_PRINT_AT_HOME_DETAILS);
            request.Parameters.Add(ParameterNames.DS_VOUCHER, dsPrintVoucherDetails);
            response = this._customerServiceAdapter.Set<VoucherandCouponDetails>(request);
            chk = response.Status;
            _logdata.CaptureData("requestobject", request);
            _logdata.CaptureData("Response status", chk);
            _logger.Submit(_logdata);
        }
        catch (Exception exception)
        {
            throw GeneralUtility.GetCustomException("failed while recording coupon and voucher details of customer", exception, new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, _logdata },
                                { ParameterNames.FRIENDLY_ERROR_MESSAGE, "" }
                            });
        }
        return chk;
    }

    
    }
}
