using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Data;
using Tesco.Framework.UITesting.Services.LocatorService;
using System.Xml;

namespace Tesco.Framework.UITesting.Services
{
    public class LocatorServiceAdaptor
    {

        public bool ValidatePostcode(string postcode)
        {

            using (LocatorSvcSDAClient client = new LocatorSvcSDAClient())
            {
                string postal = client.GetAddressListForPostcode(postcode);

                if (postal.Contains("Postcode"))
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(postal);
                    DataSet dsCustomerInfo = new DataSet();
                    dsCustomerInfo.ReadXml(new XmlNodeReader(xDoc));
                    if (dsCustomerInfo.Tables["PafAddressEntity"].Rows.Count > 0)
                        return true;
                    else 
                        return false;
                   
                }
                else
                    return false;
            }
           
        }
    }
}
