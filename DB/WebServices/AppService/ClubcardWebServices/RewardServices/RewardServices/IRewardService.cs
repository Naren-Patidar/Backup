using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;
using System.Data;

namespace Tesco.com.ExchangesService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface IRewardService
    {
        [OperationContract]
        bool GetRewardDetail(long customerID, string culture, out string serrorXml, out string sresultXml);

        [OperationContract]
        bool GetTokenInfo(Guid guid, long bookingid, long productlineid, string culture, out string serrorXml, out string sresultXml);

        [OperationContract]
        bool InsertInStoreTokens(DataSet dsInStoreTokens);
    }
}



