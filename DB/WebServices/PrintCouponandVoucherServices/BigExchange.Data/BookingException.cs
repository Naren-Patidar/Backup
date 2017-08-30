using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;


namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: BookingException
    /// </summary>
    
    [DataContract]
    public class BookingException : Exception
    {
        [DataMember]
        public ErrorTypes ErrorType { get; set; }


        public BookingException()
        {

        }
       
    }
}
