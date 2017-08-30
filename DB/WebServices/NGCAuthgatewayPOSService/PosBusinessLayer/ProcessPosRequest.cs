using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using NGCTrace;

namespace Tesco.NGC.PosBusinessLayer
{
    public class ProcessPosRequest
    {


        #region GetXmlForObject Method

        /// <summary>
        /// Method to Serialize the object and return the xml string
        /// </summary>
        /// <param name="obj">Object to be serilaized</param>
        /// <returns>xml string</returns>
        public string GetXmlForObject(object obj)
        {
            MemoryStream stream = null;
            TextWriter writer = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.GetXmlForObject()");

                stream = new MemoryStream(); // read xml in memory

                writer = new StreamWriter(stream, Encoding.UTF8);

                // get serialise object
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                serializer.Serialize(writer, obj); // read object

                int count = (int)stream.Length; // saves object in memory stream

                byte[] arr = new byte[count];

                stream.Seek(0, SeekOrigin.Begin);

                // copy stream contents in byte array
                stream.Read(arr, 0, count);

                System.Text.UTF8Encoding utf = new UTF8Encoding();

                //UnicodeEncoding utf = new UnicodeEncoding(); // convert byte array to string
                return utf.GetString(arr).Trim().Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.GetXmlForObject-Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.GetXmlForObject- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.GetXmlForObject ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                if (stream != null) stream.Close();

                if (writer != null) writer.Close();

                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.GetXmlForObject()");
            }
        }

        #endregion GetXmlForObject Method

        #region ProcessPosGet Method

        /// <summary>
        /// Method to Process the incoming request for PosGet
        /// </summary>
        /// <param name="posNGCRequest">Request Object</param>
        /// <returns>Result Object</returns>
        public PosNGCResult ProcessPosGet(PosNGCReq posNGCRequest)
        {
            PosNGCResult posNgcResult = new PosNGCResult();

            PosNGCGet posGet = new PosNGCGet();
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosGet()" + System.Environment.NewLine + "posNGCRequest : " + System.Environment.NewLine + "AlternateID : " + posNGCRequest.AlternateID + System.Environment.NewLine + "BonusPoints: " + posNGCRequest.BonusPoints + System.Environment.NewLine + "Branch : " + posNGCRequest.Branch + System.Environment.NewLine + "Cashier : " + posNGCRequest.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + posNGCRequest.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + posNGCRequest.ClubcardNo + System.Environment.NewLine + "Country : " + posNGCRequest.Country + System.Environment.NewLine + "Currency : " + posNGCRequest.Currency + System.Environment.NewLine + "Date : " + posNGCRequest.Date + System.Environment.NewLine + "GreenPoints : " + posNGCRequest.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + posNGCRequest.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + posNGCRequest.PointsEarned + System.Environment.NewLine + "QualifySpend : " + posNGCRequest.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + posNGCRequest.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + posNGCRequest.SequenceNo + System.Environment.NewLine + "TillNo : " + posNGCRequest.TillNo + System.Environment.NewLine + "TillVer : " + posNGCRequest.TillVer + System.Environment.NewLine + "Time : " + posNGCRequest.Time + System.Environment.NewLine + "TotalSpend : " + posNGCRequest.TotalSpend + System.Environment.NewLine + "Training : " + posNGCRequest.Training + System.Environment.NewLine + "TrsType : " + posNGCRequest.TrsType + System.Environment.NewLine + "version : " + posNGCRequest.version);

                // Processing the PosNGCRequest object to get the PosNGCResult
                posNgcResult = posGet.ProcessRequest(posNGCRequest);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosGet-Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosGet- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosGet ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosGet()" + System.Environment.NewLine + "posNgcResult : " + System.Environment.NewLine + "AlternateID : " + posNgcResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNgcResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNgcResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNgcResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNgcResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNgcResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNgcResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNgcResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNgcResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNgcResult.FlexMsgNo + System.Environment.NewLine + "FlexMsg : " + posNgcResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNgcResult.SessionId + System.Environment.NewLine + "Status : " + posNgcResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNgcResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNgcResult.Surname + System.Environment.NewLine + "Title : " + posNgcResult.Title + System.Environment.NewLine + "TrsType : " + posNgcResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNgcResult.UpToDate);
            }
            return posNgcResult;
        }

        #endregion ProcessPosGet Method

        #region ProcessPosSet Method

        /// <summary>
        /// Method to process incoming request for PosSet
        /// </summary>
        /// <param name="posNGCRequest">Request Object</param>
        /// <returns>Result Object</returns>
        public PosNGCResult ProcessPosSet(PosNGCReq posNGCRequest)
        {
            PosNGCResult posNgcResult = new PosNGCResult();

            PosNGCSet posSet = new PosNGCSet();

            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosSet()" + System.Environment.NewLine + "posNGCRequest : " + System.Environment.NewLine + "AlternateID : " + posNGCRequest.AlternateID + System.Environment.NewLine + "BonusPoints: " + posNGCRequest.BonusPoints + System.Environment.NewLine + "Branch : " + posNGCRequest.Branch + System.Environment.NewLine + "Cashier : " + posNGCRequest.Cashier + System.Environment.NewLine + "CheckOutBankNo : " + posNGCRequest.CheckOutBankNo + System.Environment.NewLine + "ClubcardNo : " + posNGCRequest.ClubcardNo + System.Environment.NewLine + "Country : " + posNGCRequest.Country + System.Environment.NewLine + "Currency : " + posNGCRequest.Currency + System.Environment.NewLine + "Date : " + posNGCRequest.Date + System.Environment.NewLine + "GreenPoints : " + posNGCRequest.GreenPoints + System.Environment.NewLine + "InterfaceVer : " + posNGCRequest.InterfaceVer + System.Environment.NewLine + "PointsEarned : " + posNGCRequest.PointsEarned + System.Environment.NewLine + "QualifySpend : " + posNGCRequest.QualifySpend + System.Environment.NewLine + "ReceiptNo : " + posNGCRequest.ReceiptNo + System.Environment.NewLine + "SequenceNo : " + posNGCRequest.SequenceNo + System.Environment.NewLine + "TillNo : " + posNGCRequest.TillNo + System.Environment.NewLine + "TillVer : " + posNGCRequest.TillVer + System.Environment.NewLine + "Time : " + posNGCRequest.Time + System.Environment.NewLine + "TotalSpend : " + posNGCRequest.TotalSpend + System.Environment.NewLine + "Training : " + posNGCRequest.Training + System.Environment.NewLine + "TrsType : " + posNGCRequest.TrsType + System.Environment.NewLine + "version : " + posNGCRequest.version);

                // Processing the PosNGCRequest object to get the PosNGCResult
                posNgcResult = posSet.ProcessRequest(posNGCRequest);
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosSet-Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosSet- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosSet ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);

                throw ex;
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosBusinessLayer.ProcessPosRequest.ProcessPosSet()" + System.Environment.NewLine + "posNgcResult : " + System.Environment.NewLine + "AlternateID : " + posNgcResult.AlternateID + System.Environment.NewLine + "BonusPoints  : " + posNgcResult.BonusPoints + System.Environment.NewLine + "ClubcardNo : " + posNgcResult.ClubCardNo + System.Environment.NewLine + "GreenPoints : " + posNgcResult.GreenPoints + System.Environment.NewLine + "Initials : " + posNgcResult.Initials + System.Environment.NewLine + "OperMsgRef : " + posNgcResult.OperMsgRef + System.Environment.NewLine + "OperMsgs : " + posNgcResult.OperMsgs + System.Environment.NewLine + "PointsBalance : " + posNgcResult.PointsBalance + System.Environment.NewLine + "Postcode : " + posNgcResult.Postcode + System.Environment.NewLine + "FlexMsgNo : " + posNgcResult.FlexMsgNo + System.Environment.NewLine + "FlexMsg : " + posNgcResult.FlexMsg + System.Environment.NewLine + "SessionId : " + posNgcResult.SessionId + System.Environment.NewLine + "Status : " + posNgcResult.Status + System.Environment.NewLine + "StatusMsgNo : " + posNgcResult.StatusMsgNo + System.Environment.NewLine + "Surname : " + posNgcResult.Surname + System.Environment.NewLine + "Title : " + posNgcResult.Title + System.Environment.NewLine + "TrsType : " + posNgcResult.TrsType + System.Environment.NewLine + "UpToDate : " + posNgcResult.UpToDate);
            }
            return posNgcResult;
        }

        #endregion ProcessPosSet Method

    }
}
