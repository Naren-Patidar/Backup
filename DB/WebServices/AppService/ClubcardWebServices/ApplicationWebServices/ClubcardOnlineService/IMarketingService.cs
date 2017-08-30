using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Security;
using System.Xml;

namespace Tesco.com.ClubcardOnlineService
{
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IMarketingService
    {
        //[OperationContract, XmlSerializerFormat]
        //bool ViewAgency(int agencynumber, string culture, out string resultsDoc, out string resultXml);

        [OperationContract]
        bool ViewAgency(int agencynumber, string culture, out string resultsDoc, out string resultXml);

        [OperationContract]
        bool AddAgency(string updateXML, short sessionID, out long sessionCrmId, out string resultXml);


        [OperationContract]
        bool SearchAgency(string conditionXml, int maxRowCount, out int rowCount, string culture, out string resultsDoc, out string resultXml);


        #region CSD Point Limit

        [OperationContract]
        bool UpdatePointsLimit(string objectXml, string sessionUserID, out long objectID, out string resultXml);

        [OperationContract]
        bool SearchCSDPointsLimit(string conditionXml, int maxRowCount, out int rowCount, string culture, out string xmlDoc, out string resultXml);


        #endregion


        #region Collection Period

        #region Maintain Collection Period

        [OperationContract]
        bool ViewCollectionPeriod(string conditionXml, int maxRowCount, out int rowCount, string culture, out string xmlDoc, out string resultXml);
        [OperationContract]
        bool AddCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml);
        [OperationContract]
        bool UpdateCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml);
        [OperationContract]
        bool DeleteCollectionPeriod(string objectXml, string userID, out long objectId, out string resultXml);
        [OperationContract]
        bool DoesCapabilityExist(string capabilityXml, string capability);
        #endregion

        #region Reward Mailing Overview
        [OperationContract]
        bool ViewRewardMailingDetails(long offerID, string culture, string sessionID, out string xmldoc, out string resultxml);
        [OperationContract]
        bool UpdateRewardMailingDetails(string objectXml, string userID, out long objectId, out string resultXml);


        #endregion

        #region Coupon Setup Types

        [OperationContract]
        bool ViewCouponSetupTypes(string conditionXml, int maxRowCount, out int rowCount, string culture, string sessionId, out string xmldoc, out string resultxml);

        [OperationContract]
        bool DeleteCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml);

        [OperationContract]
        bool InsertCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml);

        [OperationContract]
        bool UpdateCouponSetupTypes(string objectXml, string sessionId, out long sessionCrmid, out string resultxml);


        #endregion

        #region Voucher Setup Types

        [OperationContract]
        bool ViewVoucherDetails(long offerID, string culture, string sessionId, out string xmldoc, out string resultxml);
        [OperationContract]
        bool ViewVoucherBarCodeDetails(long offerID, string culture, string sessionId, out string xmldoc, out string resultxml);

        [OperationContract]
        bool UpdateVoucherDetails(string conditionXML,string sessionId,out long SessionCrmid,out string resultxml);

        [OperationContract]
        bool AddVoucherDetails(string conditionXML, string sessionId, out long SessionCrmid, out string resultxml);

        [OperationContract]
        bool ViewPartnerOutlets(long PartnerID, string sessionID, out string resultDoc, out string resultXml);

        [OperationContract]
        bool ViewPartnerTransactions(int maxRowCount, string conditionXml, string sessionId, string culture, out int rowCount, out string resultsDoc, out string resultXml);

        [OperationContract]
        bool AddPartner(string updateXML, string sessionID, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool AddPartnerOutlets(string updateXML, string sessionID, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool UpdatePartnerOutlets(string updateXML, string sessionID, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool GetOptionalCustomerStatus(long OfferID, string sessionId, out string resultsDoc, out string resultXml);

        [OperationContract]
        bool UpdateAgency(string updateXML, string sessionID, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool UpdatePartner(string updateXML, string sessionID, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool DeletePartnerOutlets(string conditionXml, string sessionId, out long sessionCrmId, out string resultXml);

        [OperationContract]
        bool ViewReports(string conditionXml, int maxRowCount, out int rowCount, string culture, string sessionId, out string resultXml);

        

        [OperationContract]
        bool ViewAssociatedPartners(string conditionXml, short sessionID, int retrieveNumber, out string resultsDoc, out string resultXml, int maxcount, out int rowCount, string culture);

        [OperationContract]
        bool ViewPartner(long PartnerID, string sessionID, out string resultsDoc, out string resultXml);

        [OperationContract]
        bool ViewPartners(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);

        [OperationContract]
        bool ViewPartnerType(string conditionXml, int maxRowCount, string culture, out string errorXml, out string resultXml, int rowCount);

        [OperationContract]
        bool ViewMailingDetails(long OfferID, string sessionID, out string resultDoc, out string resultXml);
        #endregion
        #region Update mailing Details

        //updated by sabhareesan
        [OperationContract]
        bool UpdateMailingDetails(string objectXml, string sessionId, out long sessionCrmid, out string resultXml);

        #endregion

        #endregion
#region ConnectMtd
        [OperationContract]
        bool Connect(string userName, string password, string culture, string AppName, out string sessionId, out string capabilityXml, out string resultXml);
#endregion



    }
}