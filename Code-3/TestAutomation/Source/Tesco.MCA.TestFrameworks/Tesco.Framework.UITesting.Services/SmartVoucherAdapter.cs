using System;
using System.Collections.Generic;
using Tesco.Framework.UITesting.Services.SmartVoucherService;
using Tesco.Framework.UITesting;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;


namespace Tesco.Framework.UITesting.Services
{
    public class SmartVoucherAdapter :BaseAdaptor
    {

        //To check for the active voucher
        public bool GetUnUsedVoucher(string clubcard)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);

            using (SmartVoucherServicesClient client = new SmartVoucherServicesClient())
            {
                client.GetUnusedVoucherDtls(clubcard);
                DataSet dsUnusedVoucherDetails = new DataSet();
                GetUnusedVoucherDtlsRsp response = client.GetUnusedVoucherDtls(clubcard);
                if (response != null)
                {
                    if (response.dsResponse != null)
                    {
                        if (response.ErrorMessage != null)
                        {
                            throw new Exception(response.ErrorMessage);
                        }

                        dsUnusedVoucherDetails = response.dsResponse;

                    }
                }
                if (dsUnusedVoucherDetails.Tables["Table"].Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        }

        //To check for the Redeemed voucher
        public bool GetUsedVoucher(string clubcard)
        {
            StackTrace stackTrace = new StackTrace();
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            using (SmartVoucherServicesClient client = new SmartVoucherServicesClient())
            {
                client.GetUsedVoucherDtls(clubcard);
                DataSet dsUsedVoucherDetails = new DataSet();
                GetUsedVoucherDtlsRsp response = client.GetUsedVoucherDtls(clubcard);
                if (response != null)
                {
                    if (response.dsResponse != null)
                    {
                        if (response.ErrorMessage != null)
                        {
                            throw new Exception(response.ErrorMessage);
                        }

                        dsUsedVoucherDetails = response.dsResponse;

                    }
                }
                if (dsUsedVoucherDetails.Tables["Table"].Rows.Count > 0)
                    return true;
                else
                    return false;
            }

        }

        //To check for top up or bonus voucher
        public bool GetUnUsedVouchertype(string clubcard, string VoucherType)
        {
            EnumerableRowCollection<DataRow> rows = null;
            using (SmartVoucherServicesClient client = new SmartVoucherServicesClient())
            {
                client.GetUnusedVoucherDtls(clubcard);
                DataSet dsUnusedVoucherDetails = new DataSet();

                GetUnusedVoucherDtlsRsp response = client.GetUnusedVoucherDtls(clubcard);
                if (response != null)
                {
                    if (response.dsResponse != null)
                    {
                        if (response.ErrorMessage != null)
                        {
                            throw new Exception(response.ErrorMessage);
                        }

                        dsUnusedVoucherDetails = response.dsResponse;

                    }
                }
                switch (VoucherType)
                {
                    case "TopUp":
                        rows = dsUnusedVoucherDetails.Tables["Table"].AsEnumerable()
                      .Where(r => r.Field<Byte>("Voucher Type") == 5);
                        break;
                    case "Bonus":
                        rows = dsUnusedVoucherDetails.Tables["Table"].AsEnumerable()
                                     .Where(r => r.Field<Byte>("Voucher Type") == 5);
                        break;
                }
                if (rows.Count() > 0)
                    return true;
                else
                    return false;


            }

        }

        public DataSet GetCustomerVoucherValCPSDataset(string cardNumber, string stDate, string enDate)
        {
            GetVoucherValAllCPSRsp response = null;
            DataSet voucherDataSet = new DataSet();
            SmartVoucherServicesClient smartVoucherServicesClient = new SmartVoucherServicesClient();
            try
            {
               

                //Get the Clubcard voucher value from the webservice.
                response = new GetVoucherValAllCPSRsp();
                response = smartVoucherServicesClient.GetVoucherValCPS(cardNumber, stDate, enDate);
                if (response != null && response.dsResponse != null)
                {
                    voucherDataSet = response.dsResponse;
                }
                return voucherDataSet;
            }
            catch (Exception exp)
            {

                throw exp;
            }
            finally
            {
               
                response = null;

            }
        }
        public string GetAvailableVouchersCount(string cardNumber, string culture)
        {
            float Voucher = 0.0f; string VouchersAvailable = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty;

            SmartVoucherServicesClient svsc = new SmartVoucherServicesClient();
            GetRewardDtlsRsp resp = svsc.GetRewardDtls(cardNumber);
            string xml = resp.dsResponse.GetXml();
            if (!xml.ToUpper().Equals("<NEWDATASET />"))
            {
                XDocument xDoc = new XDocument(xml);
                List<float> availableVouchers = (from t in xDoc.Descendants("Table")
                                                  select t.Element("Reward_Left_Over").GetValue<float>()
                                                ).ToList();

                Voucher = availableVouchers.FirstOrDefault();
                VouchersAvailable = (string.Format("{0:0.00}", Voucher)).ToString();
            }
            else
            {
                 VouchersAvailable = (string.Format("{0:0.00}", Voucher)).ToString();
            }
            return VouchersAvailable;
        }

    }
}


