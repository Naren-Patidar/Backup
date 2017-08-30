#region Using

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Tesco.NGC.DataAccessLayer;
using System.Diagnostics;
using NGCTrace;
using PosBusinessLayer.CheckOutService;
using Tesco.NGC.PosNGCWebService;


#endregion


namespace Tesco.NGC.PosBusinessLayer
{
    /// <summary>
    /// Summary description for PosNGCGet.
    /// </summary>
    public class PosNGCGet
    {


        #region Private Member Variables

        SqlParameter[] paramList = new SqlParameter[22];

        string sessionGuid = null;

        DataSet msgSet = null;


        #endregion Private Member Variables

        #region ProcessRequest Method

        /// <summary>
        /// Method to process the incoming request and get the result, called from webservice
        /// </summary>
        /// <param name="requestXml">Request</param>
        /// <returns>Result</returns>
        public PosNGCResult ProcessRequest(PosNGCReq requestObj)
        {
            PosNGCResult posNGCResult = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCGet.ProcessRequest()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + "AlternateID : " + requestObj.AlternateID + System.Environment.NewLine + "BonusPoints: " + requestObj.BonusPoints + System.Environment.NewLine + "Branch : " + requestObj.Branch + System.Environment.NewLine + "Cashier : " + requestObj.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + requestObj.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + requestObj.ClubcardNo + System.Environment.NewLine + "Country : " + requestObj.Country + System.Environment.NewLine + "Currency : " + requestObj.Currency + System.Environment.NewLine + "Date : " + requestObj.Date + System.Environment.NewLine + "GreenPoints : " + requestObj.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + requestObj.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + requestObj.PointsEarned + System.Environment.NewLine + "QualifySpend : " + requestObj.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + requestObj.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + requestObj.SequenceNo + System.Environment.NewLine + "TillNo : " + requestObj.TillNo + System.Environment.NewLine + "TillVer : " + requestObj.TillVer + System.Environment.NewLine + "Time : " + requestObj.Time + System.Environment.NewLine + "TotalSpend : " + requestObj.TotalSpend + System.Environment.NewLine + "Training : " + requestObj.Training + System.Environment.NewLine + "TrsType : " + requestObj.TrsType + System.Environment.NewLine + "version : " + requestObj.version);


                posNGCResult = new PosNGCResult();

                // Building the paramter list to be sent into SP from the request 
                BuildParameterList(requestObj);

                // Sending the parameter list to execute SP
                string connectionString = ConfigurationSettings.AppSettings["AdminConnectionString"].ToString();

                msgSet = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, "USP_AuthGateway_Pos_Get", paramList);

                posNGCResult = BuildResultObject(requestObj);



            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCGet.ProcessRequest -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCGet.ProcessRequest- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCGet.ProcessRequest ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.

                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCGet.ProcessRequest()" + System.Environment.NewLine + "posNGCResult : " + System.Environment.NewLine + "AlternateID : " + posNGCResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNGCResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNGCResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNGCResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNGCResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNGCResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNGCResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNGCResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNGCResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNGCResult.FlexMsgNo + System.Environment.NewLine + "FlexMsg : " + posNGCResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNGCResult.SessionId + System.Environment.NewLine + "Status : " + posNGCResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNGCResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNGCResult.Surname + System.Environment.NewLine + "Title : " + posNGCResult.Title + System.Environment.NewLine + "TrsType : " + posNGCResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNGCResult.UpToDate);
            }
            return posNGCResult;
        }

        #endregion ProcessRequest Method

        #region BuildParameterList Method

        /// <summary>
        /// Method for building the parameter list for processing the request by sending them to SP
        /// </summary>
        /// <param name="req">Request Object</param>
        private void BuildParameterList(PosNGCReq requestObj)
        {
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameterList()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + "AlternateID : " + requestObj.AlternateID + System.Environment.NewLine + "BonusPoints: " + requestObj.BonusPoints + System.Environment.NewLine + "Branch : " + requestObj.Branch + System.Environment.NewLine + "Cashier : " + requestObj.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + requestObj.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + requestObj.ClubcardNo + System.Environment.NewLine + "Country : " + requestObj.Country + System.Environment.NewLine + "Currency : " + requestObj.Currency + System.Environment.NewLine + "Date : " + requestObj.Date + System.Environment.NewLine + "GreenPoints : " + requestObj.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + requestObj.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + requestObj.PointsEarned + System.Environment.NewLine + "QualifySpend : " + requestObj.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + requestObj.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + requestObj.SequenceNo + System.Environment.NewLine + "TillNo : " + requestObj.TillNo + System.Environment.NewLine + "TillVer : " + requestObj.TillVer + System.Environment.NewLine + "Time : " + requestObj.Time + System.Environment.NewLine + "TotalSpend : " + requestObj.TotalSpend + System.Environment.NewLine + "Training : " + requestObj.Training + System.Environment.NewLine + "TrsType : " + requestObj.TrsType + System.Environment.NewLine + "version : " + requestObj.version);

                //Old code To get 36 char length (includes hypen)
                //sessionGuid = Guid.NewGuid().ToString();

                //New code - To get only 32 char (without hypen from ID)
                sessionGuid = Guid.NewGuid().ToString("N");

                paramList[0] = BuildParameter("@CultureIsoCode", SqlDbType.VarChar, 7, ParameterDirection.Input, ConfigurationSettings.AppSettings["cultureInfo"].ToString());

                if (string.IsNullOrEmpty(requestObj.ClubcardNo))
                {
                    paramList[1] = BuildParameter("@ClubcardID", SqlDbType.BigInt, 19, ParameterDirection.Input, 0);
                }
                else
                {
                    paramList[1] = BuildParameter("@ClubcardID", SqlDbType.BigInt, 19, ParameterDirection.Input, long.Parse(requestObj.ClubcardNo));
                }
                paramList[2] = BuildParameter("@TescoStoreID", SqlDbType.Int, 4, ParameterDirection.Input, Int32.Parse(requestObj.Branch));

                paramList[3] = BuildParameter("@AlternateID", SqlDbType.NVarChar, 50, ParameterDirection.Input, requestObj.AlternateID);

                paramList[4] = BuildParameter("@Status", SqlDbType.VarChar, 2, ParameterDirection.Output, string.Empty);

                paramList[5] = BuildParameter("@PrimaryClubcardID", SqlDbType.BigInt, 19, ParameterDirection.Output, string.Empty);

                paramList[6] = BuildParameter("@CustomerWelcomedFlag", SqlDbType.VarChar, 1, ParameterDirection.Output, string.Empty);

                paramList[7] = BuildParameter("@Name1", SqlDbType.NVarChar, 50, ParameterDirection.Output, string.Empty);

                paramList[8] = BuildParameter("@LookupTitlePhrase", SqlDbType.NVarChar, 50, ParameterDirection.Output, string.Empty);

                paramList[9] = BuildParameter("@PostalCode", SqlDbType.NVarChar, 20, ParameterDirection.Output, string.Empty);

                paramList[10] = BuildParameter("@TotalPointsBalance", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[11] = BuildParameter("@UpToDate", SqlDbType.VarChar, 8, ParameterDirection.Output, null);

                paramList[12] = BuildParameter("@StatusMsgNo", SqlDbType.SmallInt, 2, ParameterDirection.Output, null);

                paramList[13] = BuildParameter("@UniqueNumber", SqlDbType.VarChar, 18, ParameterDirection.Output, null);

                paramList[14] = BuildParameter("@WelcomePointsBalance", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[15] = BuildParameter("@ExtraPoints1Balance", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[16] = BuildParameter("@ExtraPoints2Balance", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[17] = BuildParameter("@BonusPoints", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[18] = BuildParameter("@OperMsgCount", SqlDbType.TinyInt, 1, ParameterDirection.Output, null);

                paramList[19] = BuildParameter("@RcptMsgCount", SqlDbType.TinyInt, 1, ParameterDirection.Output, null);

                paramList[20] = BuildParameter("@GreenPoints", SqlDbType.Decimal, 15, ParameterDirection.Output, null);

                paramList[21] = BuildParameter("@HouseholdID", SqlDbType.BigInt, 15, ParameterDirection.Output, null);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameterList -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameterList- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameterList ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameterList()");
            }
        }

        #endregion BuildParameterList Method

        #region BuildResultObject Method

        /// <summary>
        /// Method for populating the response object from the updated request object
        /// </summary>
        /// <param name="req">Updated Request</param>
        /// <returns>Result Object</returns>
        /// Changes Made in this method for StorelineV54 Requirement On (17-10-2012)
        /// StorelineV54 Requirements : To include ClubCardNo in the Response and to change the ResponseXML format
        private PosNGCResult BuildResultObject(PosNGCReq requestObj)
        {
            PosNGCResult posNgcResult = new PosNGCResult();
            string sTempval = "";
            string posVersion = ConfigurationSettings.AppSettings["PosVersion"].ToString();

            try
            {               
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildResultObject()" + System.Environment.NewLine + "requestObj : " + System.Environment.NewLine + requestObj.AlternateID + System.Environment.NewLine + requestObj.BonusPoints + System.Environment.NewLine + requestObj.Branch + System.Environment.NewLine + requestObj.Cashier + System.Environment.NewLine + requestObj.CheckOutBankNo + System.Environment.NewLine + requestObj.ClubcardNo + System.Environment.NewLine + requestObj.Country + System.Environment.NewLine + requestObj.Currency + System.Environment.NewLine + requestObj.Date + System.Environment.NewLine + requestObj.GreenPoints + System.Environment.NewLine + requestObj.InterfaceVer + System.Environment.NewLine + requestObj.PointsEarned + System.Environment.NewLine + requestObj.QualifySpend + System.Environment.NewLine + requestObj.ReceiptNo + System.Environment.NewLine + requestObj.SequenceNo + System.Environment.NewLine + requestObj.TillNo + System.Environment.NewLine + requestObj.TillVer + System.Environment.NewLine + requestObj.Time + System.Environment.NewLine + requestObj.TotalSpend + System.Environment.NewLine + requestObj.Training + System.Environment.NewLine + requestObj.TrsType + System.Environment.NewLine + requestObj.version);

                #region V@T
                if (posVersion == "0")
                {
                    posNgcResult.TrsType = requestObj.TrsType;

                    if (paramList[4].Value.ToString().Equals("OK"))
                    {
                        posNgcResult.Status = (PosNGCResultStatus)0;
                    }
                    else if (paramList[4].Value.ToString().Equals("NK"))
                    {
                        posNgcResult.Status = (PosNGCResultStatus)1;
                    }
                    posNgcResult.SessionId = sessionGuid;

                    //posNgcResult.ClubcardNo = requestObj.ClubcardNo;
                    //posNgcResult.AlternateID = requestObj.AlternateID;

                    posNgcResult.ClubcardNo = null;
                    posNgcResult.AlternateID = null;

                    posNgcResult.Surname = paramList[7].Value.ToString().PadRight(BusinessConstants.iSurnameLength);


                    //Take only four Characters
                    posNgcResult.Title = (paramList[8].Value.ToString().Length > 4 ? paramList[8].Value.ToString().Substring(0, 4) : paramList[8].Value.ToString().PadRight(BusinessConstants.iTitleLength));

                    posNgcResult.Postcode = paramList[9].Value.ToString().PadRight(BusinessConstants.iPostcodeLength);

                    posNgcResult.PointsBalance = ((paramList[10].Value.ToString().Equals("")) ? "0" : paramList[10].Value.ToString());

                    posNgcResult.UpToDate = paramList[11].Value.ToString();

                    posNgcResult.StatusMsgNo = paramList[12].Value.ToString();

                    posNgcResult.Initials = sTempval.PadRight(BusinessConstants.iInitialsLength);

                    //posNgcResult.BonusPoints = paramList[17].Value.ToString();

                    posNgcResult.BonusPoints = null;

                    int noOfOpMsgs = Int32.Parse(paramList[18].Value.ToString());

                    int noOfRcptMsgs = Int32.Parse(paramList[19].Value.ToString());

                    //posNgcResult.GreenPoints = paramList[20].Value.ToString();

                    posNgcResult.GreenPoints = null;

                    posNgcResult.OperMsgRef = sTempval.PadRight(BusinessConstants.iOperMsgRefLength);


                    PosNGCResultOperMsgsMsg[] OperMsgs = null;

                    PosNGCResultRcptMsgsMsg[] rcptMsgs = null;

                    PosNGCResultRcptMsgs[] ResultRcptMsgs = null;

                    if (noOfOpMsgs != 0)
                    {
                        OperMsgs = new PosNGCResultOperMsgsMsg[noOfOpMsgs];

                        for (int c = 0; c < noOfOpMsgs; c++)
                        {
                            OperMsgs[c] = new PosNGCResultOperMsgsMsg();

                            OperMsgs[c].LineNo = msgSet.Tables[0].Rows[c].ItemArray[0].ToString();

                            OperMsgs[c].Text = msgSet.Tables[0].Rows[c].ItemArray[1].ToString().PadRight(BusinessConstants.iOperMsgs_Msg_Text_Length);
                        }
                    }
                    //need to create atleast one inner tag to Match with XSD
                    else
                    {

                        OperMsgs = new PosNGCResultOperMsgsMsg[1];
                        OperMsgs[0] = new PosNGCResultOperMsgsMsg();
                        OperMsgs[0].LineNo = "1";
                        OperMsgs[0].Text = sTempval.PadRight(BusinessConstants.iOperMsgs_Msg_Text_Length);

                    }

                    PosNGCResultOperMsgs ResultOperMsgs = new PosNGCResultOperMsgs();

                    ResultOperMsgs.Msg = OperMsgs;

                    posNgcResult.OperMsgs = ResultOperMsgs;



                    //Populating the result object with Coupon Details
                    if (posNgcResult.Status == 0)
                    {
                        //If status:31(Card is not in valid range),Status:30(card is not valid/customer is not registered) & Status:4 (customer id is nui, Shouldn't call C@T/V@T
                        if ((requestObj.ClubcardNo != null) && (posNgcResult.StatusMsgNo != "30") && (posNgcResult.StatusMsgNo != "31") && (posNgcResult.StatusMsgNo != "4") && (requestObj.AlternateID != "") && (requestObj.AlternateID == null))
                        {
                            string fetchCouponDetails = ConfigurationSettings.AppSettings["FetchCouponDetails"].ToString();
                            if (fetchCouponDetails == "1")
                            {


                                ClubcardCouponCheckOutServiceClient objCouponCheckOut = new ClubcardCouponCheckOutServiceClient();
                                //AvailableCouponRequest[] arrAvCopupons = new AvailableCouponRequest[10];
                                //AvailableCoupons[] objAvCoupons = new AvailableCoupons[100];

                                //arrAvCopupons[0] = new AvailableCouponRequest();
                                //arrAvCopupons[0].HouseHoldId = Convert.ToInt64(paramList[21].Value.ToString());

                                //objAvCoupons = objCoupon.GetAvailableCoupons(arrAvCopupons);

                                CouponAtTillRequest[] arrCouponAtTill = new CouponAtTillRequest[10];
                                CouponAtTill[] objCouponsAtTill = new CouponAtTill[100];

                                arrCouponAtTill[0] = new CouponAtTillRequest();
                                arrCouponAtTill[0].CustomerId = Convert.ToInt64(paramList[21].Value.ToString());
                                arrCouponAtTill[0].StoreNumber = Convert.ToInt16(requestObj.Branch);
                                arrCouponAtTill[0].TillId = Convert.ToInt16(requestObj.TillNo);
                                arrCouponAtTill[0].TillBankId = Convert.ToInt16(requestObj.CheckOutBankNo);
                                arrCouponAtTill[0].OperatorId = Convert.ToInt16(requestObj.Cashier);
                                //Added for V@T - Mandatory (Not for C@T)
                                arrCouponAtTill[0].ClubcardNumber = Convert.ToInt64(paramList[5].Value.ToString());

                                objCouponsAtTill = objCouponCheckOut.GetCouponsAtTill(arrCouponAtTill[0]);


                                if (objCouponsAtTill != null)
                                {
                                    if (objCouponsAtTill.Length > 0)
                                    {
                                        //check for coupon template ID
                                        if (objCouponsAtTill[0].CouponTemplateId != null)
                                        {
                                            //Receipt message reference
                                            posNgcResult.RcptMsgRef = objCouponsAtTill[0].CouponTemplateId.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                        }
                                        else
                                        {
                                            //Receipt message reference
                                            posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                        }


                                        //PosNGCResultBarcodeNumbers[] barNos = null;
                                        //barNos = new PosNGCResultBarcodeNumbers[objCouponsAtTill.Length];
                                        ResultRcptMsgs = new PosNGCResultRcptMsgs[objCouponsAtTill.Length];


                                        //Loop through the Coupon Details from Coupon service
                                        for (int iCouponCnt = 0; iCouponCnt < objCouponsAtTill.Length; iCouponCnt++)
                                        {

                                            if (objCouponsAtTill[iCouponCnt] != null)
                                            {
                                                ResultRcptMsgs[iCouponCnt] = new PosNGCResultRcptMsgs();


                                                //Get the Coupon Result Receipt information for each coupon (Max: 10 Row information)
                                                //Line Number,Line Format, Line Text

                                                noOfRcptMsgs = objCouponsAtTill[iCouponCnt].ListLineInfo.Length;
                                                rcptMsgs = new PosNGCResultRcptMsgsMsg[noOfRcptMsgs];

                                                //check for the count
                                                if (noOfRcptMsgs > 0)
                                                {
                                                    for (int i = 0; i < noOfRcptMsgs; i++)
                                                    {
                                                        //Set the Coupon Details 
                                                        rcptMsgs[i] = new PosNGCResultRcptMsgsMsg();

                                                        rcptMsgs[i].LineNo = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineNumber.ToString();

                                                        rcptMsgs[i].Frmt = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineFormat.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);

                                                        rcptMsgs[i].Text = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineText.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);

                                                    }//Item For Loop   
                                                }
                                                //create the default values
                                                else
                                                {
                                                    rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                                    rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();

                                                    rcptMsgs[0].LineNo = "1";
                                                    rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                                    rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;
                                                }


                                                ResultRcptMsgs[iCouponCnt].Msg = rcptMsgs;


                                            }
                                        }//CC for loop    

                                        posNgcResult.RcptMsgs = ResultRcptMsgs;

                                    }
                                    else
                                    {
                                        #region Create Empty Tags for RcptMsgs

                                        posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                                        ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                                        ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                                        //to create message <Msg> tag
                                        rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                        rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                                        rcptMsgs[0].LineNo = "1";
                                        rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                        rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                        ResultRcptMsgs[0].Msg = rcptMsgs;
                                        posNgcResult.RcptMsgs = ResultRcptMsgs;

                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region Create Empty Tags for RcptMsgs

                                    posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                                    ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                                    ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                                    //to create message <Msg> tag
                                    rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                    rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                                    rcptMsgs[0].LineNo = "1";
                                    rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                    rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                    ResultRcptMsgs[0].Msg = rcptMsgs;
                                    posNgcResult.RcptMsgs = ResultRcptMsgs;

                                    #endregion
                                }


                                #region Old Code - For Reference

                                //PosNGCResultBarcodeNumbers[] barNos = null;
                                //barNos = new PosNGCResultBarcodeNumbers[objCouponsAtTill[0].ListLineInfo.Length];

                                //barNos[i] = new PosNGCResultBarcodeNumbers();
                                //barNos[i].BarcodeNumber = objCouponsAtTill[0].ListLineInfo[i].LineNumber.ToString();
                                //posNgcResult.BarcodeNo = barNos;
                                //if (objAvCoupons != null)
                                //{
                                //    if (objAvCoupons[0] != null)
                                //    {
                                //        PosNGCResultBarcodeNumbers[] barNos = null;
                                //        barNos = new PosNGCResultBarcodeNumbers[objAvCoupons[0].CouponList.Length];
                                //        for (int i = 0; i < objAvCoupons[0].CouponList.Length; i++)
                                //        {
                                //            barNos[i] = new PosNGCResultBarcodeNumbers();
                                //            barNos[i].BarcodeNumber = objAvCoupons[0].CouponList[i].SmartBarcode.ToString();
                                //        }
                                //        posNgcResult.BarcodeNo = barNos;
                                //    }
                                //} 
                                #endregion
                            }
                            else
                            {
                                #region Create Empty Tags for RcptMsgs

                                posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                                ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                                ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                                //to create message <Msg> tag
                                rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                                rcptMsgs[0].LineNo = "1";
                                rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                ResultRcptMsgs[0].Msg = rcptMsgs;
                                posNgcResult.RcptMsgs = ResultRcptMsgs;

                                #endregion
                            }
                        }
                        else
                        {
                            #region Create Empty Tags for RcptMsgs

                            posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                            ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                            ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                            //to create message <Msg> tag
                            rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                            rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                            rcptMsgs[0].LineNo = "1";
                            rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                            rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                            ResultRcptMsgs[0].Msg = rcptMsgs;
                            posNgcResult.RcptMsgs = ResultRcptMsgs;

                            #endregion
                        }
                    }
                    else
                    {
                        #region Create Empty Tags for RcptMsgs

                        posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                        ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                        ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                        //to create message <Msg> tag
                        rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                        rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                        rcptMsgs[0].LineNo = "1";
                        rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                        rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                        ResultRcptMsgs[0].Msg = rcptMsgs;
                        posNgcResult.RcptMsgs = ResultRcptMsgs;

                        #endregion

                    }
                }
                #endregion

                #region Storeline V54 Changes
                else
                {
                    posNgcResult.TrsType = requestObj.TrsType;

                    if (paramList[4].Value.ToString().Equals("OK"))
                    {
                        posNgcResult.Status = (PosNGCResultStatus)0;
                    }
                    else if (paramList[4].Value.ToString().Equals("NK"))
                    {
                        posNgcResult.Status = (PosNGCResultStatus)1;
                    }
                    posNgcResult.SessionId = sessionGuid;


                    posNgcResult.ClubCardNo = paramList[5].Value.ToString();
                    //posNgcResult.AlternateID = requestObj.AlternateID;


                    posNgcResult.AlternateID = null;

                    posNgcResult.Surname = paramList[7].Value.ToString().PadRight(BusinessConstants.iSurnameLength);


                    //Take only four Characters
                    posNgcResult.Title = (paramList[8].Value.ToString().Length > 4 ? paramList[8].Value.ToString().Substring(0, 4) : paramList[8].Value.ToString().PadRight(BusinessConstants.iTitleLength));

                    posNgcResult.Postcode = paramList[9].Value.ToString().PadRight(BusinessConstants.iPostcodeLength);

                    posNgcResult.PointsBalance = ((paramList[10].Value.ToString().Equals("")) ? "0" : paramList[10].Value.ToString());

                    posNgcResult.UpToDate = paramList[11].Value.ToString();

                    posNgcResult.StatusMsgNo = paramList[12].Value.ToString();

                    posNgcResult.Initials = sTempval.PadRight(BusinessConstants.iInitialsLength);

                    //posNgcResult.BonusPoints = paramList[17].Value.ToString();

                    posNgcResult.BonusPoints = null;

                    int noOfOpMsgs = Int32.Parse(paramList[18].Value.ToString());

                    int noOfRcptMsgs = Int32.Parse(paramList[19].Value.ToString());

                    //posNgcResult.GreenPoints = paramList[20].Value.ToString();

                    posNgcResult.GreenPoints = null;

                    posNgcResult.OperMsgRef = sTempval.PadRight(BusinessConstants.iOperMsgRefLength);


                    PosNGCResultOperMsgsMsg[] OperMsgs = null;

                    PosNGCResultVoucherMsg[] flexMsgs = null;

                    PosNGCResultFlexMsg[] ResultFlexMsgs = null;

                    PosNGCResultAddFlexMsgs addflexMsgs = null;

                    PosNGCResultAddFlexMsgs ResultAddFlexMsgs = null;

                    //V54
                    PosNGCResultRcptMsgsMsg[] Msgs = null;

                    PosNGCResultRcptMsgs[] ResultRcptMsgs = null;

                    PosNGCResultRcptMsgsMsg[] rcptMsgs = null;


                    //V54

                    if (noOfOpMsgs != 0)
                    {
                        OperMsgs = new PosNGCResultOperMsgsMsg[noOfOpMsgs];

                        for (int c = 0; c < noOfOpMsgs; c++)
                        {
                            OperMsgs[c] = new PosNGCResultOperMsgsMsg();

                            OperMsgs[c].LineNo = msgSet.Tables[0].Rows[c].ItemArray[0].ToString();

                            OperMsgs[c].Text = msgSet.Tables[0].Rows[c].ItemArray[1].ToString().PadRight(BusinessConstants.iOperMsgs_Msg_Text_Length);
                        }

                    }                      
                    //need to create atleast one inner tag to Match with XSD
                    else
                    {

                        OperMsgs = new PosNGCResultOperMsgsMsg[1];
                        OperMsgs[0] = new PosNGCResultOperMsgsMsg();
                        OperMsgs[0].LineNo = "1";
                        OperMsgs[0].Text = sTempval.PadRight(BusinessConstants.iOperMsgs_Msg_Text_Length);

                    }

                    PosNGCResultOperMsgs ResultOperMsgs = new PosNGCResultOperMsgs();

                    ResultOperMsgs.Msg = OperMsgs;

                    posNgcResult.OperMsgs = ResultOperMsgs;



                    //Populating the result object with Coupon Details     
                     
                    if (posNgcResult.Status == 0)
                    {
                        if ((requestObj.ClubcardNo != null) && (requestObj.ClubcardNo != "") && (posNgcResult.StatusMsgNo != "30") && (posNgcResult.StatusMsgNo != "31") && (posNgcResult.StatusMsgNo != "4") && ((requestObj.AlternateID== null) || (requestObj.AlternateID =="" )))
                        {
                            string fetchCouponDetails = ConfigurationSettings.AppSettings["FetchCouponDetails"].ToString();
                            if (fetchCouponDetails == "1")
                            {


                                ClubcardCouponCheckOutServiceClient objCouponCheckOut = new ClubcardCouponCheckOutServiceClient();
                                //AvailableCouponRequest[] arrAvCopupons = new AvailableCouponRequest[10];
                                //AvailableCoupons[] objAvCoupons = new AvailableCoupons[100];

                                //arrAvCopupons[0] = new AvailableCouponRequest();
                                //arrAvCopupons[0].HouseHoldId = Convert.ToInt64(paramList[21].Value.ToString());

                                //objAvCoupons = objCoupon.GetAvailableCoupons(arrAvCopupons);

                                CouponAtTillRequest[] arrCouponAtTill = new CouponAtTillRequest[10];
                                CouponAtTill[] objCouponsAtTill = new CouponAtTill[100];

                                arrCouponAtTill[0] = new CouponAtTillRequest();
                                arrCouponAtTill[0].CustomerId = Convert.ToInt64(paramList[21].Value.ToString());
                                arrCouponAtTill[0].StoreNumber = Convert.ToInt16(requestObj.Branch);
                                arrCouponAtTill[0].TillId = Convert.ToInt16(requestObj.TillNo);
                                arrCouponAtTill[0].TillBankId = Convert.ToInt16(requestObj.CheckOutBankNo);
                                arrCouponAtTill[0].OperatorId = Convert.ToInt16(requestObj.Cashier);
                                //Added for V@T - Mandatory (Not for C@T)
                                arrCouponAtTill[0].ClubcardNumber = Convert.ToInt64(paramList[5].Value.ToString());

                                objCouponsAtTill = objCouponCheckOut.GetCouponsAtTill(arrCouponAtTill[0]);


                                if (objCouponsAtTill != null)
                                {
                                    if (objCouponsAtTill.Length > 0)
                                    {
                                        //PosNGCResultBarcodeNumbers[] barNos = null;
                                        //barNos = new PosNGCResultBarcodeNumbers[objCouponsAtTill.Length];

                                        ResultAddFlexMsgs = new PosNGCResultAddFlexMsgs();
                                        ResultFlexMsgs = new PosNGCResultFlexMsg[objCouponsAtTill.Length];
                                        ResultRcptMsgs = new PosNGCResultRcptMsgs[objCouponsAtTill.Length]; //V54

                                        //V54 Starts

                                        //check for coupon template ID
                                        if (objCouponsAtTill[0].CouponTemplateId != null)
                                        {
                                            //Receipt message reference
                                            posNgcResult.RcptMsgRef = objCouponsAtTill[0].CouponTemplateId.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                        }
                                        else
                                        {
                                            //Receipt message reference
                                            posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                        }

                                        //PosNGCResultBarcodeNumbers[] barNos = null;
                                        //barNos = new PosNGCResultBarcodeNumbers[objCouponsAtTill.Length];
                                        ResultRcptMsgs = new PosNGCResultRcptMsgs[objCouponsAtTill.Length];
                                        for (int iCouponCnt = 0; iCouponCnt < 1; iCouponCnt++)
                                        {
                                            if (objCouponsAtTill[iCouponCnt] != null)
                                            {
                                                ResultRcptMsgs[iCouponCnt] = new PosNGCResultRcptMsgs();


                                                //Get the Coupon Result Receipt information for each coupon (Max: 10 Row information)
                                                //Line Number,Line Format, Line Text

                                                noOfRcptMsgs = objCouponsAtTill[iCouponCnt].ListLineInfo.Length;
                                                rcptMsgs = new PosNGCResultRcptMsgsMsg[noOfRcptMsgs];


                                                //check for the count
                                                if (noOfRcptMsgs > 0)
                                                {
                                                    for (int i = 0; i < noOfRcptMsgs; i++)
                                                    {
                                                        //Set the Coupon Details 
                                                        rcptMsgs[i] = new PosNGCResultRcptMsgsMsg();

                                                        rcptMsgs[i].LineNo = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineNumber.ToString();

                                                        rcptMsgs[i].Frmt = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineFormat.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);

                                                        rcptMsgs[i].Text = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineText.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);
                                                    }//Item For Loop

                                                }

                                                //create the default values
                                                else
                                                {
                                                    rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                                    rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();
                                                    rcptMsgs[0].LineNo = "1";
                                                    rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);
                                                    rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);

                                                    addflexMsgs = new PosNGCResultAddFlexMsgs();
                                                    ResultFlexMsgs[iCouponCnt] = new PosNGCResultFlexMsg();
                                                    flexMsgs = new PosNGCResultVoucherMsg[1];
                                                    flexMsgs[0] = new PosNGCResultVoucherMsg();
                                                    flexMsgs[0].LineNo = "1";
                                                    flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);
                                                    flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);

                                                    ResultFlexMsgs[iCouponCnt].VoucherMsg = flexMsgs;
                                                    ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;
                                                }


                                                ResultRcptMsgs[iCouponCnt].Msg = rcptMsgs;


                                            }
                                        }//CC for loop    
                                        //V54 Ends

                                        //Loop through the Coupon Details from Coupon service
                                        for (int iCouponCnt = 1; iCouponCnt < objCouponsAtTill.Length; iCouponCnt++)
                                        {


                                            if (objCouponsAtTill[iCouponCnt] != null)
                                            {
                                                ResultFlexMsgs[iCouponCnt] = new PosNGCResultFlexMsg();

                                                ResultFlexMsgs[iCouponCnt].FlexMsgNo = objCouponsAtTill[iCouponCnt].CouponTemplateId;

                                                //Get the Coupon Result Receipt information for each coupon (Max: 10 Row information)
                                                //Line Number,Line Format, Line Text

                                                noOfRcptMsgs = objCouponsAtTill[iCouponCnt].ListLineInfo.Length;
                                                flexMsgs = new PosNGCResultVoucherMsg[noOfRcptMsgs];


                                                //check for the count
                                                if (noOfRcptMsgs > 0)
                                                {
                                                    for (int i = 0; i < noOfRcptMsgs; i++)
                                                    {
                                                        //Set the Coupon Details 
                                                        flexMsgs[i] = new PosNGCResultVoucherMsg();

                                                        flexMsgs[i].LineNo = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineNumber.ToString();

                                                        flexMsgs[i].Frmt = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineFormat.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);

                                                        flexMsgs[i].Text = objCouponsAtTill[iCouponCnt].ListLineInfo[i].LineText.ToString().PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);
                                                    }//Item For Loop

                                                }

                                                //create the default values
                                                else
                                                {
                                                    addflexMsgs = new PosNGCResultAddFlexMsgs();

                                                    flexMsgs = new PosNGCResultVoucherMsg[1];
                                                    flexMsgs[0] = new PosNGCResultVoucherMsg();
                                                    flexMsgs[0].LineNo = "1";
                                                    flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length);
                                                    flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length);
                                                }


                                                ResultFlexMsgs[iCouponCnt].VoucherMsg = flexMsgs;
                                                ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;


                                            }
                                        }//CC for loop    

                                        posNgcResult.RcptMsgs = ResultRcptMsgs;//V54
                                        posNgcResult.AddFlexMsgs = ResultAddFlexMsgs;

                                    }
                                    else
                                    {
                                        //V5
                                        #region Create Empty Tags for RcptMsgs

                                        posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                                        ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                                        ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                                        //to create message <Msg> tag
                                        rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                        rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                                        rcptMsgs[0].LineNo = "1";
                                        rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                        rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                        ResultRcptMsgs[0].Msg = rcptMsgs;
                                        posNgcResult.RcptMsgs = ResultRcptMsgs;

                                        #endregion
                                        //V5

                                        #region Create Empty Tags for flexMsgs


                                        ResultAddFlexMsgs = new PosNGCResultAddFlexMsgs();
                                        ResultFlexMsgs = new PosNGCResultFlexMsg[1];
                                        ResultFlexMsgs[0] = new PosNGCResultFlexMsg();


                                        //to create message <Msg> tag
                                        flexMsgs = new PosNGCResultVoucherMsg[1];
                                        ResultFlexMsgs[0].FlexMsgNo = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                        flexMsgs[0] = new PosNGCResultVoucherMsg();


                                        flexMsgs[0].LineNo = "1";
                                        flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                        flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                        ResultFlexMsgs[0].VoucherMsg = flexMsgs;
                                        ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;
                                        posNgcResult.AddFlexMsgs = ResultAddFlexMsgs;



                                        #endregion
                                    }
                                }
                                else
                                {
                                    //V5
                                    #region Create Empty Tags for RcptMsgs

                                    posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                                    ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                                    ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                                    //to create message <Msg> tag
                                    rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                                    rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                                    rcptMsgs[0].LineNo = "1";
                                    rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                    rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                    ResultRcptMsgs[0].Msg = rcptMsgs;
                                    posNgcResult.RcptMsgs = ResultRcptMsgs;

                                    #endregion
                                    //V5

                                    #region Create Empty Tags for flexMsgs

                                    ResultAddFlexMsgs = new PosNGCResultAddFlexMsgs();
                                    ResultFlexMsgs = new PosNGCResultFlexMsg[1];
                                    ResultFlexMsgs[0] = new PosNGCResultFlexMsg();


                                    //to create message <Msg> tag
                                    flexMsgs = new PosNGCResultVoucherMsg[1];
                                    ResultFlexMsgs[0].FlexMsgNo = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                                    flexMsgs[0] = new PosNGCResultVoucherMsg();


                                    flexMsgs[0].LineNo = "1";
                                    flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                                    flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                                    ResultFlexMsgs[0].VoucherMsg = flexMsgs;
                                    ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;
                                    posNgcResult.AddFlexMsgs = ResultAddFlexMsgs;




                                    #endregion
                                }


                                #region Old Code - For Reference

                                //PosNGCResultBarcodeNumbers[] barNos = null;
                                //barNos = new PosNGCResultBarcodeNumbers[objCouponsAtTill[0].ListLineInfo.Length];

                                //barNos[i] = new PosNGCResultBarcodeNumbers();
                                //barNos[i].BarcodeNumber = objCouponsAtTill[0].ListLineInfo[i].LineNumber.ToString();
                                //posNgcResult.BarcodeNo = barNos;
                                //if (objAvCoupons != null)
                                //{
                                //    if (objAvCoupons[0] != null)
                                //    {
                                //        PosNGCResultBarcodeNumbers[] barNos = null;
                                //        barNos = new PosNGCResultBarcodeNumbers[objAvCoupons[0].CouponList.Length];
                                //        for (int i = 0; i < objAvCoupons[0].CouponList.Length; i++)
                                //        {
                                //            barNos[i] = new PosNGCResultBarcodeNumbers();
                                //            barNos[i].BarcodeNumber = objAvCoupons[0].CouponList[i].SmartBarcode.ToString();
                                //        }
                                //        posNgcResult.BarcodeNo = barNos;
                                //    }
                                //} 
                                #endregion
                            }
                        }
                        else
                        {
                            //V5
                            #region Create Empty Tags for RcptMsgs

                            posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                            ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                            ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                            //to create message <Msg> tag
                            rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                            rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                            rcptMsgs[0].LineNo = "1";
                            rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                            rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                            ResultRcptMsgs[0].Msg = rcptMsgs;
                            posNgcResult.RcptMsgs = ResultRcptMsgs;

                            #endregion
                            //V5

                            #region Create Empty Tags for flexMsgs

                            ResultAddFlexMsgs = new PosNGCResultAddFlexMsgs();
                            ResultFlexMsgs = new PosNGCResultFlexMsg[1];
                            ResultFlexMsgs[0] = new PosNGCResultFlexMsg();


                            //to create message <Msg> tag
                            flexMsgs = new PosNGCResultVoucherMsg[1];
                            ResultFlexMsgs[0].FlexMsgNo = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                            flexMsgs[0] = new PosNGCResultVoucherMsg();


                            flexMsgs[0].LineNo = "1";
                            flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                            flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                            ResultFlexMsgs[0].VoucherMsg = flexMsgs;
                            ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;
                            posNgcResult.AddFlexMsgs = ResultAddFlexMsgs;


                            #endregion
                        }
                    }
                    else
                    {
                        //V5
                        #region Create Empty Tags for RcptMsgs

                        posNgcResult.RcptMsgRef = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);

                        ResultRcptMsgs = new PosNGCResultRcptMsgs[1];
                        ResultRcptMsgs[0] = new PosNGCResultRcptMsgs();

                        //to create message <Msg> tag
                        rcptMsgs = new PosNGCResultRcptMsgsMsg[1];
                        rcptMsgs[0] = new PosNGCResultRcptMsgsMsg();


                        rcptMsgs[0].LineNo = "1";
                        rcptMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                        rcptMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                        ResultRcptMsgs[0].Msg = rcptMsgs;
                        posNgcResult.RcptMsgs = ResultRcptMsgs;

                        #endregion
                        //V5

                        #region Create Empty Tags for FlexMsgs

                        ResultAddFlexMsgs = new PosNGCResultAddFlexMsgs();
                        ResultFlexMsgs = new PosNGCResultFlexMsg[1];
                        ResultFlexMsgs[0] = new PosNGCResultFlexMsg();


                        //to create message <Msg> tag
                        flexMsgs = new PosNGCResultVoucherMsg[1];
                        ResultFlexMsgs[0].FlexMsgNo = sTempval.PadRight(BusinessConstants.iRcptMsgRef_Length);
                        flexMsgs[0] = new PosNGCResultVoucherMsg();


                        flexMsgs[0].LineNo = "1";
                        flexMsgs[0].Frmt = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Frmt_Length); ;
                        flexMsgs[0].Text = sTempval.PadRight(BusinessConstants.iRcptMsgs_Msg_Text_Length); ;

                        ResultFlexMsgs[0].VoucherMsg = flexMsgs;
                        ResultAddFlexMsgs.FlexMsg = ResultFlexMsgs;
                        posNgcResult.AddFlexMsgs = ResultAddFlexMsgs;




                        #endregion

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildResultObject -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildResultObject- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildResultObject ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.

                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildResultObject()" + System.Environment.NewLine + "posNgcResult : " + System.Environment.NewLine + "AlternateID : " + posNgcResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNgcResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNgcResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNgcResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNgcResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNgcResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNgcResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNgcResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNgcResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNgcResult.FlexMsgNo + System.Environment.NewLine + "FlexMsgs : " + posNgcResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNgcResult.SessionId + System.Environment.NewLine + "Status : " + posNgcResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNgcResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNgcResult.Surname + System.Environment.NewLine + "Title : " + posNgcResult.Title + System.Environment.NewLine + "TrsType : " + posNgcResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNgcResult.UpToDate);
            }
            return posNgcResult;
        }

        #endregion BuildResultObject Method

        #region BuildParameter Method

        /// <summary>
        /// Method to Build Parameter, set its direction and value
        /// </summary>
        /// <param name="paramName">Parameter Name</param>
        /// <param name="dbType">Parameter Type</param>
        /// <param name="size">Parameter Size</param>
        /// <param name="dir">Parameter Direction</param>
        /// <param name="paramVal">Parameter Value</param>
        /// <returns></returns>
        private SqlParameter BuildParameter(string paramName, SqlDbType dbType, int size, ParameterDirection dir, object paramVal)
        {
            SqlParameter param = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameter()" + System.Environment.NewLine + "paramName : " + paramName);

                param = new SqlParameter(paramName, dbType, size);

                param.Direction = dir;

                param.Value = paramVal;
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameter -Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameter- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameter ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.PosNGCGet.BuildParameter()");
            }
            return param;
        }

        #endregion BuildParameter Method


    }
}
