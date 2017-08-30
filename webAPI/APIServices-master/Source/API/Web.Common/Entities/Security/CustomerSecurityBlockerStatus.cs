using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Security
{
    public class CustomerSecurityBlockerStatus : BaseEntity<CustomerSecurityBlockerStatus>
    {
        public CustomerSecurityBlockerStatus()
        {
            this.ISBLOCKED = 'N';
        }

        public char ISBLOCKED { get; set; }

        public int ACCESSATTEMPTS { get; set; }
    }

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
                                                       ISBLOCKED = t.Element(CustomerVerificationDetailsEnum.ISBLOCKED).GetValue<Char>(),
                                                       ACCESSATTEMPTS = t.Element(CustomerVerificationDetailsEnum.ACCESSATTEMPTS).GetValue<Int32>()
                                                   }).ToList();
          
           
       }

       
   }
}
