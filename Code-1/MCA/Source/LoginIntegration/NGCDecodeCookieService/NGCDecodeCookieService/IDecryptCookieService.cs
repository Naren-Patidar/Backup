using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Tesco.com.NGCDecodeCookieService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDecryptCookieService" in both code and config file together.
    [ServiceContract]
    public interface IDecryptCookieService 
    {
         
        [OperationContract]
        CustomerIdentity GetDecodedCookie(CustomerIdentity Cust);

    }

    [Serializable]
    [DataContract]
    /// <summary>
    ///  This class is used to get the encodedcookie value and return the decodedcookie value.
    ///  we have defind the fields in the class to hold these values 
    /// </summary>
    public class CustomerIdentity
    {
        private string _Encodedvalue;
        private string _Error;
        private string _DecodedValue;

        [DataMember(IsRequired = true)]
        public string Encodedvalue
        {
            get { return _Encodedvalue; }
            set { _Encodedvalue = value; }
        }

        [DataMember]
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }

        [DataMember]
        public string DecodedValue
        {
            get { return _DecodedValue; }
            set { _DecodedValue = value; }
        }

    }

}
