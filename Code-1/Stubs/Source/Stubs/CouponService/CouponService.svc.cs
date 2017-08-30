using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using ServiceUtility;

namespace CouponService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class CouponService : ICouponService
    {
        CouponServiceProvider provider = new CouponServiceProvider();

        public  bool GetCouponDetail(out string serrorXml, out string sresultXml, long customerID, string culture)
        {
            return provider.GetCouponDetail(out serrorXml, out sresultXml, customerID, culture);
        }

       
    }
}
