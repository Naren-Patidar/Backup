using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the interface name "ISVNInterfaces" here, you must also update the reference to "ISVNInterfaces" in App.config.
    [ServiceContract]
    public interface ISVNInterfaces
    {
        [OperationContract]
        bool ReissueRequest(long ClubcardNumber, string UserName);

        [OperationContract]
        RollOverDetails RolloverRequest(RollOverDetails RolloverRequestData);

    }

    [DataContract]
    public class RollOverDetails
    {
        System.Collections.Generic.List<RollOverPointsDetails> rollOverPoints;
        System.Collections.Generic.List<RollOverResponseDetails> rollOverResponse;
        int collectionPeriodNumber;

        [DataMember]
        public System.Collections.Generic.List<RollOverPointsDetails> RollOverPoints
        {
            get { return rollOverPoints; }
            set { rollOverPoints = value; }
        }

        [DataMember]
        public System.Collections.Generic.List<RollOverResponseDetails> RollOverResponse
        {
            get { return rollOverResponse; }
            set { rollOverResponse = value; }
        }

        [DataMember]
        public int CollectionPeriodNumber
        {
            get { return collectionPeriodNumber; }
            set { collectionPeriodNumber = value; }
        }
    }

    public class RollOverPointsDetails
    {
        public long PrimaryClubcardID;
        public Decimal AmountSpent;
        public string VoucherBarcode;
    }

    public class RollOverResponseDetails
    {
        public long PrimaryClubcardID;
        public string ErrorReason;
        public Decimal AmountSpent;
        public string VoucherBarcode;        
    }
    
}
