using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.NGCJoinLoyaltyService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract]
    public interface IJoinLoyaltyService
    {
        //[OperationContract]
        //bool AccountDuplicateCheck(string conditionXml, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        String AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture);

        [OperationContract]
        bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID);

        [OperationContract]
        bool AccountDuplicateCheck(string inputXml, out string resultXml);
    }
}
