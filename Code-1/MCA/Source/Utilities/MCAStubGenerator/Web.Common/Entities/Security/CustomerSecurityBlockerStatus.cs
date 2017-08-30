using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security
{
    public class CustomerSecurityBlockerStatus : BaseEntity<CustomerSecurityBlockerStatus>
    {
       public char ISBLOCKED { get; set; }
    }

   [Serializable]
   public class CustomerSecurityBlockerStatusList : BaseEntity<CustomerSecurityBlockerStatusList>
   {
       List<CustomerSecurityBlockerStatus> _CustomerSecurityBlockerStatus;
       public List<CustomerSecurityBlockerStatus> CustomerSecurityBlockerStatusInstance
       {
           get { return _CustomerSecurityBlockerStatus; }
       }

       public override void ConvertFromXml(string xml)
       {
             XDocument xDoc = XDocument.Parse(xml);
               _CustomerSecurityBlockerStatus = (from t in xDoc.Descendants("SecurityStatus")
                                                 select new CustomerSecurityBlockerStatus
                                                   {
                                                       ISBLOCKED = t.Element(CustomerVerificationDetailsEnum.ISBLOCKED.ToString()).GetValue<Char>()
                                                   }).ToList();
          
           
       }

       
   }
}
