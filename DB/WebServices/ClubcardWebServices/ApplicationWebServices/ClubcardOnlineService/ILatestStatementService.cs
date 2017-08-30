using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.ClubcardOnlineService
{
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface ILatestStatementService
    {
        [OperationContract]
        bool GetLatestStatementDetails(Int64 customerID, string culture, out string errorXml, out string resultXml);
    }
}
