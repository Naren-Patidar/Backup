using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the interface name "IAdminService" here, you must also update the reference to "IAdminService" in App.config.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2011/11")]
    public interface IAdminService
    {
        [OperationContract]
        bool AddRole(string objectXml, int userID, out long objectId, out string resultXml);

        [OperationContract]
        bool AddRoleCapability(string objectXml, short userID, out long objectId, out string resultXml);

        [OperationContract]
        bool AddRoleMembership(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml);

        [OperationContract]
        bool DeleteRoleMembership(string objectXml, int sessionUserID, out long objectId, out string resultXml);

        [OperationContract]
        bool GetCapabilty(string insertXml, out string errorXml, out string resultXml, string culture);

        [OperationContract]
        bool RemoveCapability(string objectXml, int sessionUserID, out long objectId, out string resultXml);

        [OperationContract]
        bool IsAdminUser(string userName);

        [OperationContract]
        bool SearchRole(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, out int rowCount);

        [OperationContract]
        bool UpdateRoleDetails(string objectXml, int sessionUserID, out long objectId, out string resultXml, out string errorXml);

        [OperationContract]
        bool ViewRoleCapability(long RoleID, out string errorXml, out string resultXml, string culture);

        [OperationContract]
        bool ViewRoleMembership(long userID, out string errorXml, out string resultXml, string culture);

        [OperationContract]
        bool GetPromotionCode(out string errorXml, out string resultXml);

        [OperationContract]
        bool AddPromotionCode(string objectXml, short userID, out string resultXml);

        [OperationContract]
        bool UpdatePromotionCodePC(string objectXml, short userID, out string resultXml);


        [OperationContract]
        bool UpdatePromotionCode(string objectXml, short userID, out string resultXml);

    }
}
