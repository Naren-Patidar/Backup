using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JoinLoyaltyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IJoinLoyaltyService
    {
        [OperationContract]
        string AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture);

        [OperationContract]
        bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID);

        [OperationContract]
        bool AccountDuplicateCheck(out string resultXml, string inputXml);
    }

}
