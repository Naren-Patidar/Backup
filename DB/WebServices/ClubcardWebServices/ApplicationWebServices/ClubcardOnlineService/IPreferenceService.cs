/*Created By : Sabhareesan O.K
* Reason: To split preference related function to seperate perferenceservice class to improve performance
* Created Date : 01-Feb-2012
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Tesco.com.ClubcardOnlineService
{
    // NOTE: If you change the interface name "IService1" here, you must also update the reference to "IService1" in App.config.
    [ServiceContract(Namespace = "http://tesco.com/clubcardonline/datacontract/2010/01")]
    public interface IPreferenceService
    {
        #region Customer Preferences - Sabhari

        [OperationContract]
        bool GetCustomerPreferences(Int64 CustomerID, string culture, out string errorXml, out string resultXml);

        [OperationContract]
        bool UpdateCustomerPreferences(string updateXml, string consumer, out string errorXml, out Int64 customerID, char level);

        #endregion
    }
}
