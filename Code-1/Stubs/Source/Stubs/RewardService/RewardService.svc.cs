using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace RewardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class RewardService : IRewardService
    {
        RewardServiceProvider provider = new RewardServiceProvider();
        public bool GetTokenInfo(out string serrorXml, out string sresultXml, System.Guid guid, long bookingid, long productlineid, string culture)
        {
            return provider.GetTokenInfo(out serrorXml, out sresultXml, guid, bookingid, productlineid, culture);
        }

        public bool GetRewardDetail(out string serrorXml, out string sresultXml, long customerID, string culture)
        {
            return provider.GetRewardDetail(out serrorXml, out sresultXml, customerID, culture);
        }
    }
}
