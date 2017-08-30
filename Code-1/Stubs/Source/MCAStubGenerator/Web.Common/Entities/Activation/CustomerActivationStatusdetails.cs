using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation
{
    public class CustomerActivationStatusdetails : BaseEntity<CustomerActivationStatusdetails>
    {
        public long CustomerId { get; set; }
        public char Activated { get; set; }
        public int CustomerUseStatus { get; set; }
        public int CustomerMailStatus { get; set; }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            CustomerId = (from t in xDoc.Descendants("ViewCheckCustomerActivated") select t.Element(CustomerActivationDetailsEnum.CustomerID.ToString()).GetValue<Int64>()).FirstOrDefault();
            Activated = (from t in xDoc.Descendants("ViewCheckCustomerActivated") select t.Element(CustomerActivationDetailsEnum.Activated.ToString()).GetValue<char>()).FirstOrDefault();
            CustomerUseStatus = (from t in xDoc.Descendants("ViewHouseholdStatusOfCustomer") select t.Element(CustomerActivationDetailsEnum.CustomerUseStatus.ToString()).GetValue<int>()).FirstOrDefault();
            CustomerMailStatus = (from t in xDoc.Descendants("ViewHouseholdStatusOfCustomer") select t.Element(CustomerActivationDetailsEnum.CustomerMailStatus.ToString()).GetValue<int>()).FirstOrDefault();
        }

    }

    [Serializable]
    public class CustomerActivationStatusdetailsList : BaseEntity<CustomerActivationStatusdetailsList>
    {
        List<CustomerActivationStatusdetails> _CustomerActivationStatusdetails;
        public List<CustomerActivationStatusdetails> CustomerActivationStatusdetailsInstance
        {
            get { return _CustomerActivationStatusdetails; }
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _CustomerActivationStatusdetails = new List<CustomerActivationStatusdetails>();
           
            _CustomerActivationStatusdetails = (from t in xDoc.Descendants("ViewCheckCustomerActivated")
                               select new CustomerActivationStatusdetails
                               {
                                   CustomerId = t.Element(CustomerActivationDetailsEnum.CustomerID.ToString()).GetValue<Int64>(),
                                   Activated = t.Element(CustomerActivationDetailsEnum.Activated.ToString()).GetValue<char>()
                                 
                               }).ToList();

            List<CustomerActivationStatusdetails> _CustomerReuslt2 = new List<CustomerActivationStatusdetails>();
            _CustomerReuslt2 = (from t in xDoc.Descendants("ViewHouseholdStatusOfCustomer")
                                                select new CustomerActivationStatusdetails
                                                {
                                                    CustomerUseStatus = t.Element(CustomerActivationDetailsEnum.CustomerUseStatus.ToString()).GetValue<Int32>(),
                                                    CustomerMailStatus = t.Element(CustomerActivationDetailsEnum.CustomerMailStatus.ToString()).GetValue<Int32>()
                                                }).ToList();


            _CustomerActivationStatusdetails[0].CustomerUseStatus = _CustomerReuslt2[0].CustomerUseStatus;
            _CustomerActivationStatusdetails[0].CustomerMailStatus = _CustomerReuslt2[0].CustomerMailStatus;



        }
       

    }
}
