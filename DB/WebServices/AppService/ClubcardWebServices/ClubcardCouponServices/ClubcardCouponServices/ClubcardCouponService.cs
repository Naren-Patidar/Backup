using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ClubcardCouponServices.CouponEnquiryService;
using System.Data;
using System.Xml;
//using ClubcardCouponServices.ClubcardService;
using NGCTrace;
using System.Configuration;
using Tesco.NGC.Loyalty.EntityServiceLayer;
namespace Tesco.com.ClubcardCouponService
{
    // NOTE: If you change the class name "ClubcardCouponService" here, you must also update the reference to "ClubcardCouponService" in App.config.
    public class ClubcardCouponService : IClubcardCouponService
    {
        ClubcardCouponEnquiryServiceClient couponSVCClient;
        //ClubcardServiceClient svcClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseHoldID"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public bool GetAvailableCoupons(long houseHoldID, out string errorXml, out List<CouponInformation> resultArray, out int totalCoupons)
        {
            errorXml = string.Empty;
            resultArray = null;
            bool bResult = false;
            List<AvailableCoupons> availableCoupons = new List<AvailableCoupons>();
            List<CouponInformation> couponList = new List<CouponInformation>();
            totalCoupons = 0;

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardCouponService.GetAvailableCoupons HouseholdID-" + houseHoldID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardCouponService.GetAvailableCoupons HouseholdID-" + houseHoldID);

                couponSVCClient = new ClubcardCouponEnquiryServiceClient();

                //Call Clubcard coupon service to get the available coupons detail.
                availableCoupons = couponSVCClient.GetAvailableCoupons(new List<AvailableCouponRequest>() { new AvailableCouponRequest() { HouseHoldId = Convert.ToInt64(houseHoldID) } });

                //To get the Total Coupons irrespective available coupon list
                //Fix for MCA Coupon Page - Total Coupon not displayed(Coupon which sent in 6 month)
                if (availableCoupons != null)
                {
                    totalCoupons = Convert.ToInt32(availableCoupons[0].TotalCoupon);
                }

                if (availableCoupons.Count > 0 && (availableCoupons[0].CouponList != null && availableCoupons[0].CouponList.Count > 0))
                {
                    resultArray = availableCoupons[0].CouponList;
                    bResult = true;
                }

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardCouponService.GetAvailableCoupons HouseholdID-" + houseHoldID + " resultXml-" + resultArray);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardCouponService.GetAvailableCoupons HouseholdID-" + houseHoldID + " resultXml-" + resultArray);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardCouponService.GetAvailableCoupons  resultXml " + resultArray + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardCouponService.GetAvailableCoupons   resultXml " + resultArray + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardCouponService.GetAvailableCoupons ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                errorXml = ex.ToString();
            }
            finally
            {
                if (couponSVCClient != null)
                {
                    if (couponSVCClient.State == CommunicationState.Faulted)
                    {
                        couponSVCClient.Abort();
                    }
                    else if (couponSVCClient.State != CommunicationState.Closed)
                    {
                        couponSVCClient.Close();
                    }
                }
            }

            return bResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseHoldID"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public bool GetRedeemedCoupons(long houseHoldID, out string errorXml, out string couponDetail, string culture)
        {
            errorXml = string.Empty;
            List<CouponInformation> resultArray;
            bool bResult = false;
            List<RedeemedCouponResponse> redeemedCoupons = new List<RedeemedCouponResponse>();
            List<CouponInformation> couponList = new List<CouponInformation>();
            string storeNumbers = string.Empty;
            DataSet dsStoreNumbers = new DataSet();
            //Call NGC service to get store names passing store number as input parameter.
            string resultXml = string.Empty;
            DataSet dsStoreNames = null;
            XmlDocument resulDoc = null;

            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID);

                int RedemptionLength = Convert.ToInt32(ConfigurationSettings.AppSettings["RedemptionLength"].ToString());

                couponSVCClient = new ClubcardCouponEnquiryServiceClient();
                redeemedCoupons = couponSVCClient.GetRedeemedCoupons(new List<RedeemedCouponRequest>() { new RedeemedCouponRequest() { HouseHoldId = Convert.ToInt64(houseHoldID), RedemptionLength = RedemptionLength } });

                if (redeemedCoupons[0].CouponList != null && redeemedCoupons[0].CouponList.Count > 0)
                {
                    resultArray = new List<CouponInformation>();
                    resultArray = redeemedCoupons[0].CouponList;
                    DataTable dt = new DataTable();
                    dt.TableName = "UsedCouponDetail";

                    //Add data columns to the table
                    dt.Columns.Add("CouponDescription");
                    dt.Columns.Add("IssuanceStartDate");
                    dt.Columns.Add("RedemptionDate");
                    dt.Columns.Add("RedemptionStore");
                    dt.Columns.Add("RedemptionStoreName");
                    dt.Columns.Add("CouponStatus");
                    dt.Columns.Add("RedemptionCount");
                    dt.Columns.Add("ClubCardNo");
                    dt.Columns.Add("BarCodeNo");
                    dt.Columns.Add("TotalRedemption");
                    dt.Columns.Add("TotalRedeemedCoupon");


                    for (int i = 0; i < resultArray.Count; i++)
                    {
                        //if (!string.IsNullOrEmpty(storeNumbers))
                        //{
                        //    storeNumbers += ",";
                        //}

                        //int iRedemptionTxcnt = resultArray[i].ListRedemptionInfo.Count - 1;



                        //To get all the transaction of Redemption instead of single transaction                        

                        int? iRedeemNUnReedemedCnt = 0;

                        for (int iRedempTx = 0; iRedempTx < resultArray[i].ListRedemptionInfo.Count; iRedempTx++)
                        {
                            //Check whether its redeemed or not
                            if (resultArray[i].ListRedemptionInfo[iRedempTx].RedemptionType.ToString().Equals("Redeem"))
                            {
                                //if exceed max limit, then assign max limit value
                                iRedeemNUnReedemedCnt = ((iRedeemNUnReedemedCnt >= resultArray[i].MaxRedemptionLimit) ? ((resultArray[i].MaxRedemptionLimit != null) ? Convert.ToInt32(resultArray[i].MaxRedemptionLimit) : 0) : iRedeemNUnReedemedCnt + 1);
                            }
                            else if (resultArray[i].ListRedemptionInfo[iRedempTx].RedemptionType.ToString().Equals("UnRedeem"))
                            {
                                //if value reach less than zero, then assign zero
                                iRedeemNUnReedemedCnt = ((iRedeemNUnReedemedCnt <= 0) ? 0 : iRedeemNUnReedemedCnt - 1);
                            }

                            //P Kumar 23-08-2012,MKTG00007369
                            if (!string.IsNullOrEmpty(storeNumbers))
                            {
                                storeNumbers += ",";
                            }

                            //To get all the store numbers
                            storeNumbers += resultArray[i].ListRedemptionInfo[iRedempTx].StoreNumber.ToString();

                            DataRow dr = dt.NewRow();

                            dr["CouponDescription"] = resultArray[i].CouponDescription;
                            dr["IssuanceStartDate"] = resultArray[i].IssuanceStartDate;
                            dr["RedemptionDate"] = resultArray[i].ListRedemptionInfo[iRedempTx].RedemptionDateTime;
                            dr["RedemptionStore"] = resultArray[i].ListRedemptionInfo[iRedempTx].StoreNumber;
                            dr["RedemptionStoreName"] = "";
                            //Newly added Column for Mutiple Redemption
                            dr["CouponStatus"] = resultArray[i].ListRedemptionInfo[iRedempTx].RedemptionType.ToString();
                            //get the remaining count (indirectly) by subtratcting MaxRedemptionLimit-RedemptionUtilized
                            //dr["RedemptionCount"] = (resultArray[i].MaxRedemptionLimit != null) ? (resultArray[i].MaxRedemptionLimit - resultArray[i].RedemptionUtilized) : 0;
                            dr["RedemptionCount"] = iRedeemNUnReedemedCnt;
                            dr["ClubCardNo"] = ((resultArray[i].ListRedemptionInfo[iRedempTx].ClubcardNumber) != null) ? resultArray[i].ListRedemptionInfo[iRedempTx].ClubcardNumber : "";
                            dr["BarCodeNo"] = ((resultArray[i].SmartBarcode) != null) ? resultArray[i].SmartBarcode : "";
                            dr["TotalRedemption"] = ((resultArray[i].MaxRedemptionLimit) != null) ? resultArray[i].MaxRedemptionLimit : 0;
                            dr["TotalRedeemedCoupon"] = redeemedCoupons[0].TotalRedeemCoupon;
                            dt.Rows.Add(dr);

                        }

                    }

                    dsStoreNumbers = new DataSet();
                    dsStoreNumbers.Tables.Add(dt);

                    TescoStore storeObj = new TescoStore();
                    int maxRowCnt = 100;
                    int rowCount = 0;
                    //svcClient = new ClubcardServiceClient();

                    if (!string.IsNullOrEmpty(storeNumbers))
                    {
                        resultXml = storeObj.GetStoreName(storeNumbers, maxRowCnt, out rowCount, "en-GB");

                        //if (svcClient.GetStoreNames(out errorXml, out resultXml, storeNumbers, "en-GB"))
                        //{
                        if (resultXml != "" && resultXml != "<NewDataSet />")
                        {
                            resulDoc = new XmlDocument();
                            dsStoreNames = new DataSet();
                            resulDoc.LoadXml(resultXml);
                            dsStoreNames.ReadXml(new XmlNodeReader(resulDoc));

                            //Update the store names to the array
                            for (int storeNo = 0; storeNo < dsStoreNames.Tables[0].Rows.Count; storeNo++)
                            {
                                for (int rowID = 0; rowID < dsStoreNumbers.Tables["UsedCouponDetail"].Rows.Count; rowID++)
                                {
                                    if (dsStoreNumbers.Tables["UsedCouponDetail"].Rows[rowID]["RedemptionStore"].ToString() == dsStoreNames.Tables[0].Rows[storeNo]["TescoStoreID"].ToString())
                                    {
                                        dsStoreNumbers.Tables["UsedCouponDetail"].Rows[rowID]["RedemptionStoreName"] = dsStoreNames.Tables[0].Rows[storeNo]["TescoStoreName"];
                                    }
                                }
                            }
                        }
                        //}
                    }

                    bResult = true;
                }
                //NGCTrace.NGCTrace.TraceInfo("End:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID + Environment.NewLine + " Redemption Date.:" + dsStoreNumbers != null ? dsStoreNumbers.Tables[0].Rows[0]["RedemptionDate"].ToString() : "No Data");
                //NGCTrace.NGCTrace.TraceDebug("End:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID + Environment.NewLine + " Redemption Date.:" + dsStoreNumbers != null ? dsStoreNumbers.Tables[0].Rows[0]["RedemptionDate"].ToString() : "No Data");
                NGCTrace.NGCTrace.TraceInfo("End:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardCouponService.GetRedeemedCoupons HouseholdID-" + houseHoldID + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardCouponService.GetRedeemedCoupons ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                errorXml = ex.ToString();
            }
            finally
            {
                if (couponSVCClient != null)
                {
                    if (couponSVCClient.State == CommunicationState.Faulted)
                    {
                        couponSVCClient.Abort();
                    }
                    else if (couponSVCClient.State != CommunicationState.Closed)
                    {
                        couponSVCClient.Close();
                    }
                }

                //if (svcClient != null)
                //{
                //    if (svcClient.State == CommunicationState.Faulted)
                //    {
                //        svcClient.Abort();
                //    }
                //    else if (svcClient.State != CommunicationState.Closed)
                //    {
                //        svcClient.Close();
                //    }
                //}
            }

            //Convert dataset into XML.
            couponDetail = dsStoreNumbers.GetXml();
            return bResult;
        }

        /// <summary>
        /// Retreives coupon information for smartBarcode or smartAlphcode provided as input parameter.
        /// </summary>
        /// <param name="smartBarcode"></param>
        /// <param name="smartAlphacode"></param>
        /// <param name="errorXml"></param>
        /// <param name="resultArray"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool GetCouponInformation(string smartBarcode, string smartAlphacode, out string errorXml, out string couponDetail, string culture)
        {
            errorXml = string.Empty;
            CouponInformation resultArray = new CouponInformation();
            bool bResult = false;
            List<CouponInformationResponse> couponInformation = new List<CouponInformationResponse>();
            string storeNumber = string.Empty;
            string issStoreNumber = string.Empty; // MKTG00008543 Fix - Where redeemed is not displayed 
            string redemStoreNumber = string.Empty; // MKTG00008543 Fix - Where redeemed is not displayed 
            DataSet dsStoreNumbers = new DataSet();

            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:ClubcardCouponService.GetCouponInformation SmartBarcode-" + smartBarcode + " SmartAlphacode-" + smartAlphacode);
                NGCTrace.NGCTrace.TraceDebug("Start:ClubcardCouponService.GetCouponInformation SmartBarcode-" + smartBarcode + " SmartAlphacode-" + smartAlphacode);

                couponSVCClient = new ClubcardCouponEnquiryServiceClient();
                couponInformation = couponSVCClient.GetCouponInformation(new List<CouponInformationRequest>() { new CouponInformationRequest() { SmartBarcode = smartBarcode, SmartAlphaCode = smartAlphacode } });


                if (couponInformation.Count > 0 && (couponInformation[0].CouponDetails != null))
                {
                    if (couponInformation[0].ErrorStatusCode == "0")
                    {
                        resultArray = couponInformation[0].CouponDetails;
                        //**********************************************

                        DataTable dt = new DataTable();
                        dt.TableName = "CouponInformation";

                        //Add data columns to the table
                        dt.Columns.Add("SmartAlphaNumeric");
                        dt.Columns.Add("SmartBarcode");
                        dt.Columns.Add("CouponDescription");
                        dt.Columns.Add("CouponStatusId");
                        dt.Columns.Add("RedemptionEndDate");
                        dt.Columns.Add("MaxRedemptionLimit");
                        dt.Columns.Add("RedemptionUtilized");
                        dt.Columns.Add("IssuanceDate");
                        dt.Columns.Add("IssuanceTime");
                        dt.Columns.Add("IssuanceChannel");
                        dt.Columns.Add("IssuanceStore");
                        dt.Columns.Add("IssuanceStoreName");
                        dt.Columns.Add("ClubcardNumber");
                        dt.Columns.Add("RedemptionDate");
                        dt.Columns.Add("RedemptionStore");
                        dt.Columns.Add("RedemptionStoreName");
                        dt.Columns.Add("RedemptionType");

                        DataRow dr = dt.NewRow();

                        dr["SmartAlphaNumeric"] = resultArray.SmartAlphaNumeric;

                        dr["SmartBarcode"] = resultArray.SmartBarcode;

                        dr["CouponDescription"] = resultArray.CouponDescription;

                        dr["CouponStatusId"] = resultArray.CouponStatusId;

                        dr["RedemptionEndDate"] = resultArray.RedemptionEndDate;

                        dr["MaxRedemptionLimit"] = resultArray.MaxRedemptionLimit;

                        dr["RedemptionUtilized"] = resultArray.RedemptionUtilized;

                        dr["IssuanceDate"] = resultArray.IssuanceDate.ToString();

                        dr["IssuanceTime"] = resultArray.IssuanceStartTime.ToString();

                        dr["IssuanceChannel"] = resultArray.IssuanceChannel.ToString();

                        dr["IssuanceStore"] = resultArray.IssuanceStore.ToString();

                        dr["IssuanceStoreName"] = "";

                        if (resultArray.ListRedemptionInfo != null && resultArray.ListRedemptionInfo.Count > 0)
                        {
                            dr["ClubcardNumber"] = resultArray.ListRedemptionInfo[0].ClubcardNumber.ToString();

                            dr["RedemptionDate"] = resultArray.ListRedemptionInfo[0].RedemptionDateTime.ToString();

                            dr["RedemptionStore"] = resultArray.ListRedemptionInfo[0].StoreNumber.ToString();

                            dr["RedemptionStoreName"] = "";

                            dr["RedemptionType"] = resultArray.ListRedemptionInfo[0].RedemptionType.ToString();
                        }
                        else
                        {
                            dr["ClubcardNumber"] = "";

                            dr["RedemptionDate"] = "";

                            dr["RedemptionStore"] = "";

                            dr["RedemptionStoreName"] = "";

                            dr["RedemptionType"] = "";

                        }


                        dt.Rows.Add(dr);

                        dsStoreNumbers = new DataSet();
                        dsStoreNumbers.Tables.Add(dt);

                        if (resultArray.IssuanceStore != null)
                        {
                            issStoreNumber = resultArray.IssuanceStore.ToString(); // MKTG00008543 Fix - Where redeemed is not displayed 
                        }
                        //else if (resultArray.ListRedemptionInfo != null && resultArray.ListRedemptionInfo.Count > 0)
                        if (resultArray.ListRedemptionInfo != null && resultArray.ListRedemptionInfo.Count > 0) // MKTG00008543 Fix - Where redeemed is not displayed 
                        {
                            if (resultArray.ListRedemptionInfo[0].StoreNumber != null)
                            {
                                redemStoreNumber = resultArray.ListRedemptionInfo[0].StoreNumber.ToString(); // MKTG00008543 Fix - Where redeemed is not displayed 
                            }
                        }

                        //Call NGC service to get store names passing store number as input parameter.
                        //string resultXml = string.Empty;
                        //DataSet dsStoreNames = null;
                        //XmlDocument resulDoc = null;
                        TescoStore storeObj = new TescoStore();

                        int maxRowCnt = 100;
                        int rowCount = 0;
                        //svcClient = new ClubcardServiceClient();

                        //Commented for MKTG00008543 Fix - Where redeemed is not displayed starts
                        //if (!string.IsNullOrEmpty(storeNumber))
                        //{
                        //    resultXml = storeObj.GetStoreName(storeNumber, maxRowCnt, out rowCount, "en-GB");
                        //    //if (svcClient.GetStoreNames(out errorXml, out resultXml, storeNumber, "en-GB"))
                        //    //{
                        //    if (resultXml != "" && resultXml != "<NewDataSet />")
                        //    {
                        //        resulDoc = new XmlDocument();
                        //        dsStoreNames = new DataSet();
                        //        resulDoc.LoadXml(resultXml);
                        //        dsStoreNames.ReadXml(new XmlNodeReader(resulDoc));

                        //        //Update the store name into the dataset
                        //        if (resultArray.IssuanceStore != null)
                        //        {
                        //            dsStoreNumbers.Tables["CouponInformation"].Rows[0]["IssuanceStoreName"] = dsStoreNames.Tables[0].Rows[0]["TescoStoreName"];
                        //        }
                        //        else if (resultArray.ListRedemptionInfo[0].StoreNumber != null)
                        //        {
                        //            dsStoreNumbers.Tables["CouponInformation"].Rows[0]["RedemptionStoreName"] = dsStoreNames.Tables[0].Rows[0]["TescoStoreName"];
                        //        }
                        //    }
                        //}
                        //Commented for MKTG00008543 Fix - Where redeemed is not displayed ends
                        if (!string.IsNullOrEmpty(issStoreNumber))
                        {
                            string resultXml = string.Empty;
                            DataSet dsIssStoreNames = null;
                            XmlDocument resulDoc = null;
                            resultXml = storeObj.GetStoreName(issStoreNumber, maxRowCnt, out rowCount, "en-GB");
                            //if (svcClient.GetStoreNames(out errorXml, out resultXml, storeNumber, "en-GB"))
                            //{
                            if (resultXml != "" && resultXml != "<NewDataSet />")
                            {
                                resulDoc = new XmlDocument();
                                dsIssStoreNames = new DataSet();
                                resulDoc.LoadXml(resultXml);
                                dsIssStoreNames.ReadXml(new XmlNodeReader(resulDoc));

                                //Update the store name into the dataset
                                if (resultArray.IssuanceStore != null)
                                {
                                    dsStoreNumbers.Tables["CouponInformation"].Rows[0]["IssuanceStoreName"] = dsIssStoreNames.Tables[0].Rows[0]["TescoStoreName"];
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(redemStoreNumber))
                        {
                            string resultXml = string.Empty;
                            DataSet dsRedemStoreNames = null;
                            XmlDocument resulDoc = null;
                            resultXml = storeObj.GetStoreName(redemStoreNumber, maxRowCnt, out rowCount, "en-GB");
                            //if (svcClient.GetStoreNames(out errorXml, out resultXml, storeNumber, "en-GB"))
                            //{
                            if (resultXml != "" && resultXml != "<NewDataSet />")
                            {
                                resulDoc = new XmlDocument();
                                dsRedemStoreNames = new DataSet();
                                resulDoc.LoadXml(resultXml);
                                dsRedemStoreNames.ReadXml(new XmlNodeReader(resulDoc));

                                //Update the store name into the dataset
                               if (resultArray.ListRedemptionInfo[0].StoreNumber != null)
                               {
                                   dsStoreNumbers.Tables["CouponInformation"].Rows[0]["RedemptionStoreName"] = dsRedemStoreNames.Tables[0].Rows[0]["TescoStoreName"];
                               }
                            }
                        }
                        //}
                    }
                    //**********************************************

                    bResult = true;
                }
                else
                {
                    errorXml = couponInformation[0].ErrorStatusCode;
                }
                //}

                NGCTrace.NGCTrace.TraceInfo("End:ClubcardCouponService.GetCouponInformation SmartBarcode-" + smartBarcode + " SmartAlphacode-" + smartAlphacode);
                NGCTrace.NGCTrace.TraceDebug("End:ClubcardCouponService.GetRedeemedCoupons SmartBarcode-" + smartBarcode + " SmartAlphacode-" + smartAlphacode);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:ClubcardCouponService.GetCouponInformation  resultXml " + resultArray + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:ClubcardCouponService.GetCouponInformation   resultXml " + resultArray + " - Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:ClubcardCouponService.GetCouponInformation ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                //R1.5 Defect:MKTG00007200 P kumar 24-12-2012
                errorXml = ex.ToString();
            }
            finally
            {
                if (couponSVCClient != null)
                {
                    if (couponSVCClient.State == CommunicationState.Faulted)
                    {
                        couponSVCClient.Abort();
                    }
                    else if (couponSVCClient.State != CommunicationState.Closed)
                    {
                        couponSVCClient.Close();
                    }
                }

                //if (svcClient != null)
                //{
                //    if (svcClient.State == CommunicationState.Faulted)
                //    {
                //        svcClient.Abort();
                //    }
                //    else if (svcClient.State != CommunicationState.Closed)
                //    {
                //        svcClient.Close();
                //    }
                //}
            }

            couponDetail = dsStoreNumbers.GetXml();
            return bResult;
        }
    }
}
