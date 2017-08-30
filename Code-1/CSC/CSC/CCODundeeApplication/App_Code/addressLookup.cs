using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace CCODundeeApplication.App_Code
{
    public class addressLookup
    {

    }
    public class addressList
    {
        //addressLines _addressLines = new addressLines();
        List<addressLines> _addressLines = new List<addressLines>();
        string _concatAllAddressLines = string.Empty;
        string _streetValue = string.Empty;

        public string id { get; set; }
        public string postcode { get; set; }
        public string postTown { get; set; }
        public List<addressLines> addressLines
        {
            get { return _addressLines; }
            set { _addressLines = value; }
        }
        public string concatAddressLineValue
        {
            get { return _concatAllAddressLines; }
            set { _concatAllAddressLines = concatAddressLines(addressLines); }              
        }
        public string streetValue
        {
            get { return _streetValue; }
            set { _streetValue = getStreet(addressLines); }
        }

        private string concatAddressLines(List<addressLines> addLines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in addLines)
            {
                if(sb.Length==0)
                {
                    sb.Append(obj.value);
                }
                else
                {
                    sb.Append(string.Format(", {0}", obj.value));
                }                
            }
            
            return sb.ToString();
        }

        private string getStreet(List<addressLines> addLines)
        {
            string strStreet = string.Empty;
            foreach (var obj in addLines)
            {
                if (obj.lineNumber == "1")
                {
                    strStreet = obj.value;
                    break;
                }
            }
            return strStreet;
        }
        
    }
    public class addressLines
    {
        public string lineNumber { get; set; }
        public string value { get; set; }
    }

        
}