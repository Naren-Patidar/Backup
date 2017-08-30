using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RewardService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRewardService
    {

        [OperationContract]
        bool GetTokenInfo(out string serrorXml, out string sresultXml, System.Guid guid, long bookingid, long productlineid, string culture);

        [OperationContract]
        bool GetRewardDetail(out string serrorXml, out string sresultXml, long customerID, string culture);

        // TODO: Add your service operations here
    }
}