using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;
using System.Xml.Serialization;
using System.Text;

namespace SmartVoucherService
{
    public class SmartVoucherServiceProvider
    {
        Helper helper = new Helper();
        #region ServiceMethods
        public GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber)
        {
            GetRewardDtlsRsp resp = new GetRewardDtlsRsp();
            string xmlFile = string.Empty;
            string respXml = string.Empty;
             
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetRewardDtls\{0}.xml", ClubcardNumber));
            respXml = helper.LoadXMLFile(xmlFile);

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(respXml);
            MemoryStream stream = new MemoryStream(byteArray);

            XmlSerializer serializer = new XmlSerializer(typeof(GetRewardDtlsRsp));
            resp = (GetRewardDtlsRsp)serializer.Deserialize(stream);
            resp.ErrorMessage = null;
            return resp;
        }

        public GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber)
        {
            GetUnusedVoucherDtlsRsp response = new GetUnusedVoucherDtlsRsp();
            string xmlFile = string.Empty;
            string respXml = string.Empty;
          
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetUnusedVoucherDtls\{0}.xml", ClubcardNumber));
            respXml = helper.LoadXMLFile(xmlFile);

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(respXml);
            MemoryStream stream = new MemoryStream(byteArray);

            XmlSerializer serializer = new XmlSerializer(typeof(GetUnusedVoucherDtlsRsp));
            response = (GetUnusedVoucherDtlsRsp)serializer.Deserialize(stream);
            response.ErrorMessage = null;
            return response;
        }

        public GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber)
        {
            GetUsedVoucherDtlsRsp resp = new GetUsedVoucherDtlsRsp();
            string xmlFile = string.Empty;
            string respXml = string.Empty;
            
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetUsedVoucherDtls\{0}.xml", ClubcardNumber));
            respXml = helper.LoadXMLFile(xmlFile);

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(respXml);
            MemoryStream stream = new MemoryStream(byteArray);

            XmlSerializer serializer = new XmlSerializer(typeof(GetUsedVoucherDtlsRsp));
            resp = (GetUsedVoucherDtlsRsp)serializer.Deserialize(stream);
            resp.ErrorMessage = null;
            return resp;
        }
        public GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode)
        {
            GetRewardDtlsMilesRsp resp = new GetRewardDtlsMilesRsp();
            string xmlFile = string.Empty;
            string respXml = string.Empty;
            
            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetRewardDtlsMiles\{0}.xml", ClubcardNumber));
            respXml = helper.LoadXMLFile(xmlFile);
               
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(respXml);
            MemoryStream stream = new MemoryStream(byteArray);

            XmlSerializer serializer = new XmlSerializer(typeof(GetRewardDtlsMilesRsp));
            resp = (GetRewardDtlsMilesRsp)serializer.Deserialize(stream);
            resp.ErrorMessage = null;
            return resp;
        }

        public GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate)
        {
            GetVoucherValAllCPSRsp resp = new GetVoucherValAllCPSRsp();
            string xmlFile = string.Empty;
            string respXml = string.Empty;

            xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource\GetVoucherValCPS\{0}.xml", Clubcard_Number));
            respXml = helper.LoadXMLFile(xmlFile);

            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(respXml);
            MemoryStream stream = new MemoryStream(byteArray);

            XmlSerializer serializer = new XmlSerializer(typeof(GetVoucherValAllCPSRsp));
            resp = (GetVoucherValAllCPSRsp)serializer.Deserialize(stream);
            resp.ErrorMessage = null;
            return resp;
        }
        #endregion
    }
}