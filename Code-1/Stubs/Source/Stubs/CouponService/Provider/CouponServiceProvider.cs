using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace CouponService
{
    public class CouponServiceProvider
    {
        Helper helper = new Helper();
        #region Service Methods
        public bool GetCouponDetail(out string serrorXml, out string sresultXml, long customerID, string culture)
        {
            XmlDocument response = new XmlDocument();
            string responseXML = string.Empty;
            string fileName = string.Format(@"DataSource\{0}\{1}\{2}.xml", "GetCouponDetail", culture, customerID);
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(fileName))
            {
                responseXML = helper.LoadXMLFile(fileName);
            }
            response = helper.LoadXMLDoc(fileName);
            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
            xmlnsManager.AddNamespace("def", "http://tesco.com/clubcardonline/datacontract/2010/01");
            serrorXml = response.SelectSingleNode("//def:serrorXml", xmlnsManager).InnerText;
            sresultXml = response.SelectSingleNode("//def:sresultXml", xmlnsManager).InnerText;
            return Convert.ToBoolean(response.SelectSingleNode("//def:GetCouponDetailResult", xmlnsManager).InnerText);
        }
        #endregion
    }
}