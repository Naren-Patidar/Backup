using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Diagnostics;
using Tesco.NGC.PosBusinessLayer;
using NGCTrace;
using System.Configuration;
namespace Tesco.NGC.PosNGCWebService
{
    // NOTE: If you change the class name "PosNGCService" here, you must also update the reference to "PosNGCService" in App.config.
    public class PosNGCService : IPosNGCService
    {
        
        #region ProcessPosRequest Method

        /// <summary>
        /// Method to process Pos request
        /// </summary>
        /// <param name="requestXml">XML to process</param>
        /// <returns>xml string</returns>
        public string ProcessPosRequest(string requestXml)
        {
            PosNGCReq posNgcRequest = new PosNGCReq();
            PosNGCResult posNgcResult = new PosNGCResult();
            ProcessPosRequest processPosRequest = new ProcessPosRequest();
            MemoryStream memStream = null;
            string exceptionMsg = string.Empty;
            DataSet msgSet = null;
            try
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("Start : Tesco.NGC.PosNGCWebService.PosNGCService.ProcessPosRequest()." + System.Environment.NewLine + "Request : " + requestXml);

                if (requestXml.StartsWith("<?xml version=\"1.0\"?>"))
                {
                    requestXml = requestXml.Replace("<?xml version=\"1.0\"?>", "");
                }
                else if (requestXml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-16\"?>"))
                {
                    //remove UTF-16 encoding
                    requestXml = requestXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                }
                else if (requestXml.StartsWith("<?xml version=\"1.0\"  encoding=\"utf-8\"?>"))
                {
                    //remove UTF-8 encoding
                    requestXml = requestXml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                }

                // Deserializing the PosNGCRequest..
                XmlSerializer serializer = new XmlSerializer(typeof(PosNGCReq));

                memStream = new MemoryStream(Encoding.UTF8.GetBytes(requestXml));

                memStream.Position = 0;

                posNgcRequest = (PosNGCReq)serializer.Deserialize(memStream);


                if (posNgcRequest.TrsType.Equals("51"))
                {
                    posNgcResult = processPosRequest.ProcessPosGet(posNgcRequest);
                }
                else if (posNgcRequest.TrsType.Equals("52"))
                {
                    posNgcResult = processPosRequest.ProcessPosSet(posNgcRequest);
                }
                else
                {
                    throw new Exception("Invalid Transaction Type");
                }
               
                
            }
            catch (Exception ex)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:Tesco.NGC.PosNGCWebService.PosNGCService.ProcessPosRequest  RequestXml: " + requestXml + "-Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:Tesco.NGC.PosNGCWebService.PosNGCService.ProcessPosRequest  RequestXml: " + requestXml + "- Error Message :" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Tesco.NGC.PosNGCWebService.PosNGCService.ProcessPosRequest ");
                NGCTrace.NGCTrace.ExeptionHandling(ex);



                exceptionMsg = "Exception Occured: " + System.Environment.NewLine + ex.Message;

                posNgcResult.Status = PosNGCResultStatus.NK;

                posNgcResult.SessionId = "";

                //posNgcResult.ClubcardNo = "";

                //posNgcResult.AlternateID = "";

                if (posNgcRequest.TrsType != null && posNgcRequest.TrsType.Equals("51"))
                {
                    posNgcResult.TrsType = "51";
                }
                else if (posNgcRequest.TrsType != null && posNgcRequest.TrsType.Equals("52"))
                {
                    posNgcResult.TrsType = "52";
                    posNgcResult.StatusMsgNo = "99";
                }
            }
            finally
            {
                //Make an entry to log file.
                NGCTrace.NGCTrace.TraceInfo("End : Tesco.NGC.PosNGCWebService.PosNGCService.ProcessPosRequest()");
            }

            //returning the resultXml string to AuthGateway
            string retVal = processPosRequest.GetXmlForObject(posNgcResult);

            //add the version no at last
            retVal = retVal.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" version=\"1.0\">");


            if (posNgcResult.Status == PosNGCResultStatus.NK)
            {
                NGCTrace.NGCTrace.TraceError("Request : " + System.Environment.NewLine + requestXml + System.Environment.NewLine + "Result : " + retVal + System.Environment.NewLine + exceptionMsg);
            }
            else
            {
                NGCTrace.NGCTrace.TraceInfo("Request : " + System.Environment.NewLine + requestXml + System.Environment.NewLine + "Result : " + retVal);
            }

            return retVal;

        }

        #endregion ProcessPosRequest Method

    }
}
