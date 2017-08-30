using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Common
{
    public class HouseholdDetails : BaseEntity<HouseholdDetails>
    {
        public int PrimaryCustomerID { get; set; }
        public string CustomerID { get; set; }
        public string TitleEnglish { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string FullName { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressPostCode { get; set; }
        public int CustomerUseStatusID { get; set; }
        public int CustomerMailStatus { get; set; }

        public string BusinessType { get; set; }
        public string BusinessName { get; set; }
        public string BusinessRegistrationNumber { get; set; }

        public string BusinessAddressLine1 { get; set; }
        public string BusinessAddressLine2 { get; set; }
        public string BusinessAddressLine3 { get; set; }
        public string BusinessAddressLine4 { get; set; }
        public string BusinessAddressLine5 { get; set; }
        public string BusinessAddressLine6 { get; set; }

        public string BusinessAddressPostCode { get; set; }
        
    }

    public class HouseholdDetailsList : BaseEntity<HouseholdDetailsList>
    {
        List<HouseholdDetails> _list = new List<HouseholdDetails>();

        public List<HouseholdDetails> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("HouseholdCustomers")
                     select new HouseholdDetails
                     {
                         PrimaryCustomerID = t.Element(HouseholdDetailsEnum.PrimaryCustomerID).GetValue<Int32>(),
                         CustomerID = t.Element(HouseholdDetailsEnum.CustomerID).GetValue<string>(),
                         TitleEnglish = t.Element(HouseholdDetailsEnum.TitleEnglish).GetValue<String>(),
                         Name1 = t.Element(HouseholdDetailsEnum.Name1).GetValue<String>(),
                         Name2 = t.Element(HouseholdDetailsEnum.Name2).GetValue<String>(),
                         Name3 = t.Element(HouseholdDetailsEnum.Name3).GetValue<String>(),
                         MailingAddressLine1 = t.Element(HouseholdDetailsEnum.MailingAddressLine1).GetValue<String>(),
                         MailingAddressPostCode = t.Element(HouseholdDetailsEnum.MailingAddressPostCode).GetValue<String>(),
                         CustomerUseStatusID = t.Element(HouseholdDetailsEnum.CustomerUseStatusID).GetValue<Int32>(),
                         CustomerMailStatus = t.Element(HouseholdDetailsEnum.CustomerMailStatus).GetValue<Int32>(),
                         BusinessType = t.Element(HouseholdDetailsEnum.BusinessType).GetValue<String>(),
                         BusinessName = t.Element(HouseholdDetailsEnum.BusinessName).GetValue<String>(),
                         BusinessRegistrationNumber = t.Element(HouseholdDetailsEnum.BusinessRegistrationNumber).GetValue<String>(),
                         BusinessAddressLine1 = t.Element(HouseholdDetailsEnum.BusinessAddressLine1).GetValue<String>(),
                         BusinessAddressLine2 = t.Element(HouseholdDetailsEnum.BusinessAddressLine2).GetValue<String>(),
                         BusinessAddressLine3 = t.Element(HouseholdDetailsEnum.BusinessAddressLine3).GetValue<String>(),
                         BusinessAddressLine4 = t.Element(HouseholdDetailsEnum.BusinessAddressLine4).GetValue<String>(),
                         BusinessAddressLine5 = t.Element(HouseholdDetailsEnum.BusinessAddressLine5).GetValue<String>(),
                         BusinessAddressLine6 = t.Element(HouseholdDetailsEnum.BusinessAddressLine6).GetValue<String>(),
                         BusinessAddressPostCode = t.Element(HouseholdDetailsEnum.BusinessAddressPostCode).GetValue<String>()
                     }).ToList();
        }

    }
}
