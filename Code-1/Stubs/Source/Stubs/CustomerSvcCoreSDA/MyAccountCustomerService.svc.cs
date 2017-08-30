using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Configuration;


namespace MyAccountCustomerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class MyAccountCustomerService : ICustomerSvcsCoreSDA
    {
        MyAccountCustomerServiceProvider provider = new MyAccountCustomerServiceProvider();

        #region ICustomerSvcCoreSDA Members

        public string GetPersonalDetails(long webCustomerID)
        {
            return provider.GetPersonalDetails(webCustomerID);
        }

        public long CustomerMartiniIDGet(int dxshCustomerID)
        {
            return provider.CustomerMartiniIDGet(dxshCustomerID);
        }

        public string GetHomeAddress(long webCustomerID)
        {
            return provider.GetHomeAddress(webCustomerID);
        } 


        #endregion
    }
}
