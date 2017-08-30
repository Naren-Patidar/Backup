using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    public class HouseholdCustomerDetails : BaseEntity<HouseholdCustomerDetails>
    {
        List<Clubcard> _cards = new List<Clubcard>();

        public long PrimaryCustomerID { get; set; }
        public long CustomerID { get; set; }
        public string TitleEnglish { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string FullName { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressPostCode { get; set; }
        public int CustomerUseStatusID { get; set; }
        public int CustomerMailStatus { get; set; }
        public long ClubcardID { get; set; }
        public long HouseHoldID { get; set; }
        public string CustomerType{ get; set; }
        public string MsgCardHolder { get; set; }
        public bool IsCustomerBanned { get; set; }
        

        public List<Clubcard> Cards
        {
            get { return _cards; }
            set { _cards = value; }
        }

    }

    public class HouseholdCustomerDetailsList : BaseEntity<HouseholdCustomerDetailsList>
    {
        List<HouseholdCustomerDetails> _list = new List<HouseholdCustomerDetails>();

        public List<HouseholdCustomerDetails> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public  void ConvertFromHouseholdtableXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("HouseholdCustomers")
                     select new HouseholdCustomerDetails
                                {
                                    ClubcardID = t.Element(HouseholdCustomerDetailsEnum.ClubcardID).GetValue<Int64>(),
                                    CustomerID=t.Element(HouseholdCustomerDetailsEnum.CustomerID).GetValue<Int64>(),
                                    HouseHoldID = t.Element(HouseholdCustomerDetailsEnum.HouseHoldID).GetValue<Int64>(),
                                    TitleEnglish = t.Element(HouseholdCustomerDetailsEnum.TitleEnglish).GetValue<String>(),
                                    Name1 = t.Element(HouseholdCustomerDetailsEnum.Name1).GetValue<String>(),
                                    Name2 = t.Element(HouseholdCustomerDetailsEnum.Name2).GetValue<String>(),
                                    Name3 =t.Element(HouseholdCustomerDetailsEnum.Name3).GetValue<String>(),
                                    MailingAddressLine1 = t.Element(HouseholdCustomerDetailsEnum.MailingAddressLine1).GetValue<String>(),
                                    MailingAddressPostCode = t.Element(HouseholdCustomerDetailsEnum.MailingAddressPostCode).GetValue<String>(),
                                    CustomerUseStatusID = t.Element(HouseholdCustomerDetailsEnum.CustomerUseStatusID).GetValue<Int32>(), 
                                    CustomerMailStatus = t.Element(HouseholdCustomerDetailsEnum.CustomerMailStatus).GetValue<Int32>(),
                                    PrimaryCustomerID=t.Element(HouseholdCustomerDetailsEnum.PrimaryCustomerID).GetValue<Int32>()
                                }).ToList();
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("Customer")
                     select new HouseholdCustomerDetails
                     {
                         ClubcardID = t.Element(HouseholdCustomerDetailsEnum.ClubcardID).GetValue<Int64>(),
                         HouseHoldID = t.Element(HouseholdCustomerDetailsEnum.HouseHoldID).GetValue<Int64>()
                     }).ToList();
        }

    }
}
