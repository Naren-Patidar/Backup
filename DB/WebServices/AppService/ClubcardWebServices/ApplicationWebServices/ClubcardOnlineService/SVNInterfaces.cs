using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.NGC.Loyalty.EntityServiceLayer;
using System.Xml.Serialization;
using System.IO;

namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the class name "SVNInterfaces" here, you must also update the reference to "SVNInterfaces" in App.config.
    public class SVNInterfaces : ISVNInterfaces
    {
        #region ReissueRequest
        /// <summary>
        /// GetSecretQtns -- It is used to fetch secret questions from database table 
        /// </summary>
        /// <param name="string">out errorXml</param>
        /// <param name="string">out resultXml</param>
        /// <param name="string">out rowCount</param>
        /// <param name="string"> culture</param>
        public bool ReissueRequest(long ClubcardNumber, string UserName)
        {
            NGCTrace.NGCTrace.TraceInfo("start : SVN ReissueRequest");
            bool reissueRequest = false;
            Clubcard clubcardObject = null;

            try
            {
                clubcardObject = new Clubcard();
                reissueRequest = clubcardObject.ReissueRequest(ClubcardNumber, UserName);
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceError(ex.Message);
                NGCTrace.NGCTrace.ExeptionHandling(ex);
                reissueRequest = false;
            }
            finally
            {
                clubcardObject = null;

            }
            NGCTrace.NGCTrace.TraceInfo("end : SVN ReissueRequest");
            return reissueRequest;
        }
        #endregion

        public RollOverDetails RolloverRequest(RollOverDetails RolloverRequestData)
        {
            NGCTrace.NGCTrace.TraceInfo("start : SVN Rollover Request");
            RollOverDetails rolloverResponse = new RollOverDetails();
            Clubcard clubcardObject = null;
            long primaryCardNumber = 0;
            Decimal amountSpent = 0;           
            string errorReason ="";
            List<RollOverResponseDetails> rolloverResponseList = new List<RollOverResponseDetails>();
            try
            {               
                clubcardObject = new Clubcard();
                int count = RolloverRequestData.RollOverPoints.Count();
                for (int i = 0; i < count; i++)
                {
                    primaryCardNumber = RolloverRequestData.RollOverPoints[i].PrimaryClubcardID;
                    amountSpent = RolloverRequestData.RollOverPoints[i].AmountSpent;
                    errorReason = clubcardObject.RollOverRequest(primaryCardNumber, amountSpent, RolloverRequestData.CollectionPeriodNumber);
                    if (errorReason != "")
                    {
                        RollOverResponseDetails objResponseDetails = new RollOverResponseDetails();
                        objResponseDetails.PrimaryClubcardID = primaryCardNumber;                       
                        objResponseDetails.ErrorReason = errorReason;
                        objResponseDetails.AmountSpent = amountSpent;
                        objResponseDetails.VoucherBarcode = RolloverRequestData.RollOverPoints[i].VoucherBarcode;
                        rolloverResponseList.Add(objResponseDetails);
                        errorReason = "";
                    }
                    else
                    {
                        RollOverResponseDetails objResponseDetails = new RollOverResponseDetails();
                        objResponseDetails.PrimaryClubcardID = primaryCardNumber;
                        objResponseDetails.ErrorReason = "Success";
                        objResponseDetails.AmountSpent = amountSpent;
                        objResponseDetails.VoucherBarcode = RolloverRequestData.RollOverPoints[i].VoucherBarcode;
                        rolloverResponseList.Add(objResponseDetails);                        
                    }
                }              
               
            }
            catch (Exception ex)
            {

                NGCTrace.NGCTrace.TraceError(ex.Message);
                NGCTrace.NGCTrace.ExeptionHandling(ex);               
            }
            finally
            {
                clubcardObject = null;
                rolloverResponse.CollectionPeriodNumber = RolloverRequestData.CollectionPeriodNumber;
                rolloverResponse.RollOverResponse = rolloverResponseList;
            }
            NGCTrace.NGCTrace.TraceInfo("end : SVN Rollover Request");
            return rolloverResponse;
        }

    }
}
