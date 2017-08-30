using System;
using System.Xml.Serialization;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Security
{

    /// <summary>
    /// Change Details  : CustomerSecurityAttemptAudit, this is used to get the Browser IP address and Validation details for the Audit Purpose.
    /// Changed By      : Swaraj Kumar Patra
    /// Reviewed By     : Srinivasa RaoPelluri (On 25/09/2014)
    /// Code Checked In : By Swaraj Patra (On 26/09/2014)
    /// Team Name       : 41_Digital_Clubcard_MCA (Vikings)
    /// </summary>
    
    [XmlRoot("CustomerVerification")]
    public class CustomerSecurityAttemptAudit
    {
        private long _customerId;
        private string _ipAddress;
        private string _isValidAttempt;
        private string _browserUsed;

        [XmlElement("CustomerID")]
        public long CustomerID
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        [XmlElement("IsValidAttempt")]
        public string IsValidAttempt
        {
            get { return _isValidAttempt; }
            set { _isValidAttempt = value; }
        }

        [XmlElement("Browserused")]
        public string Browserused
        {
            get { return _browserUsed; }
            set { _browserUsed = value; }
        }

        [XmlElement("IPAddress")]
        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

    }
}
