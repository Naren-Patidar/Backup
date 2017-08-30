using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;

namespace JoinLoyaltyService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class JoinLoyaltyService : IJoinLoyaltyService
    {
        JoinLoyaltyServiceProvider provider = new JoinLoyaltyServiceProvider();

        public string AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture)
        {
            return provider.AccountCreate(dotcomCustomerID, objectXml, source,culture);
        }
        public bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID)
        {
            throw new NotImplementedException();
        }

        public bool AccountDuplicateCheck(out string resultXml, string inputXml)
        {
            return provider.AccountDuplicateCheck(out resultXml, inputXml);
        }
    }
}
