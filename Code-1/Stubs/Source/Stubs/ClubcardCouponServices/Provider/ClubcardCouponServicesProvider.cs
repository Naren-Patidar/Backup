using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace ClubcardCouponServices
{
    public class ClubcardCouponServicesProvider
    {

        Helper helper = new Helper();
        #region ServiceMethods
        public bool GetAvailableCoupons(out string errorXml, out List<CouponInformation> resultArray, out int totalCoupons, long houseHoldID)
        {
            errorXml = null;
            string resultXml = string.Empty;
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();

            fileName = string.Format(@"DataSource\GetAvailableCoupons\{0}\{1}.xml", houseHoldID, houseHoldID);
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            response = helper.LoadXMLDoc(fileName);

            XmlNode root = response.DocumentElement;

            // create ns manager
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
            xmlnsManager.AddNamespace("b", "http://schemas.datacontract.org/2004/07/Tesco.Marketing.IT.ClubcardCoupon.DataContract");

            XmlNode xnl = response.SelectSingleNode("//def:resultArray", xmlnsManager);
            string xmlString = xnl.InnerXml.Replace("b:", "");
            xmlString = string.Format("<ArrayOfCouponInformation>{0}</ArrayOfCouponInformation>", xmlString);
            List<CouponInformation> result = helper.XMLStringToObject(typeof(List<CouponInformation>), xmlString) as List<CouponInformation>;

            resultArray = result;
            totalCoupons = Convert.ToInt32(response.SelectSingleNode("//def:totalCoupons", xmlnsManager).InnerText);
            errorXml = response.SelectSingleNode("//def:errorXml", xmlnsManager).InnerText;

            return Convert.ToBoolean(response.SelectSingleNode("//def:GetAvailableCouponsResult", xmlnsManager).InnerText);
        }

        public bool GetRedeemedCoupons(out string errorXml, out string couponDetail, long houseHoldID, string culture)
        {
            string responseXML = string.Empty;
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();

            fileName = string.Format(@"DataSource\GetRedeemedCoupons\{0}\{1}\{2}.xml", culture, houseHoldID,houseHoldID);
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(fileName))
            {
                responseXML = helper.LoadXMLFile(fileName);
            }
            response = helper.LoadXMLDoc(fileName);
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
            couponDetail = response.SelectSingleNode("//def:couponDetail", xmlnsManager).InnerText;
            errorXml = string.Empty;
            return true;
        }
        #endregion
    }
}