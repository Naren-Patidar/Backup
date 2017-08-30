using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;

namespace RewardService
{
    public class RewardServiceProvider
    {
        Helper helper = new Helper();
        #region Service Methods
        public bool GetTokenInfo(out string serrorXml, out string sresultXml, System.Guid guid, long bookingid, long productlineid, string culture)
        {
            serrorXml = string.Empty;
            sresultXml = string.Empty;


            string fileName = string.Empty;
            XmlDocument input = new XmlDocument();

            fileName = string.Format(@"DataSource\{0}\{1}\{2}.xml", "GetTokenInfo",culture, "GetTokenInfo");
            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(fileName))
            {
                sresultXml = helper.LoadXMLFile(fileName);
            }

            return true;
        }

        public bool GetRewardDetail(out string serrorXml, out string sresultXml, long customerID, string culture)
        {
            serrorXml = string.Empty;
            sresultXml = string.Empty;


            string fileName = string.Empty;
            XmlDocument input = new XmlDocument();
         
            switch(customerID)
            {
                case 904289:
                    {
                        fileName = string.Format(@"DataSource\{0}\{1}\{2}\{3}.xml", "GetRewardDetail", culture, "GetRewardDetail", customerID);
                        fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                        break;
                    }
                default:
                    {
                        fileName = string.Format(@"DataSource\{0}\{1}\{2}\Common.xml", "GetRewardDetail", culture, "GetRewardDetail");
                        fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                        break;
                    }
        }
            if (File.Exists(fileName))
            {
                sresultXml = helper.LoadXMLFile(fileName);
            }

            return true;
        }
        #endregion
    }
}