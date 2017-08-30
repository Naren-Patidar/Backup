using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using Tesco.com.IntegrationServices.Messages;


namespace Tesco.com.IntegrationServices
{
    [ServiceContract]
    public interface ISmartVoucherServices
    {
        [OperationContract]
        ClubcardOnlineGetRewardDetailsResponse ClubcardOnlineGetRewardDetails(string ClubcardNumber);


    }
}
