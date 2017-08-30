using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NGCUtilityService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class UtilityService : IUtilityService
    {
        UtilityServiceProvider provider = new UtilityServiceProvider();
        public bool ProfanityCheck(out string errorXml, out string resultXml, out int rowCount, string conditionXml)
        {
            return provider.ProfanityCheck(out  errorXml, out  resultXml, out  rowCount,  conditionXml);
        }
    }
}
