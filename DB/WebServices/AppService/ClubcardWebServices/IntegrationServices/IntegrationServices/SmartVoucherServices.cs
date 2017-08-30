using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Tesco.com.IntegrationServices;
using IntegrationServices.SVServiceReference;
using Tesco.com.IntegrationServices.Messages;
using System.Data;

namespace Tesco.com.IntegrationServices
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class SmartVoucherServices : ISmartVoucherServices
    {
        MessagingWebServiceSoap svClient = null;
        DataSet ds = null;

        public ClubcardOnlineGetRewardDetailsResponse ClubcardOnlineGetRewardDetails(string ClubcardNumber)
        {
            ClubcardOnlineGetRewardDetailsResponse response = null;
            try
            {
                response = new ClubcardOnlineGetRewardDetailsResponse();
                ds = new DataSet();
                svClient = new MessagingWebServiceSoapClient();
                ds = svClient.ClubcardOnlineGetRewardDetails(ClubcardNumber);
                return response = new ClubcardOnlineGetRewardDetailsResponse(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }
}
