using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the interface name "ISecurityService" here, you must also update the reference to "ISecurityService" in App.config.
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        bool CreateUser(string objectXml, out string UserStatus, out Int64 customerID);
    }
}
