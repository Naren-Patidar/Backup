using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [Serializable]
    [DataContract]
    public class CustomerDetails
    {
        public string _title;

        public string _surName;

        public string _firstName;

        public string _emailId;

        public List<string> _preferenceList;

        public string _cardNumber;


        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [DataMember]
        public string CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        [DataMember]
        public string Surname
        {
            get { return _surName; }
            set { _surName = value; }
        }

        [DataMember]
        public string Firstname
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [DataMember]
        public string EmailId
        {
            get { return _emailId; }
            set { _emailId = value; }
        }

        [DataMember]
        public List<string> Preferencelist
        {
            get { return _preferenceList; }
            set { _preferenceList = value; }
        }
    }
}
