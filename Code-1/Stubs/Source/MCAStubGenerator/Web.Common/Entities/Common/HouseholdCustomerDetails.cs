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


        public string FullyQualifiedName(bool IsTitleHide, bool IsFirstNameHideinSalutation)
        {
            string name1 = string.Empty;
            string title = string.Empty;
            if (!IsTitleHide)
            {
                title = string.IsNullOrEmpty(this.TitleEnglish) ? string.Empty : this.TitleEnglish;
            }
            if (IsFirstNameHideinSalutation == true)
            {
                name1 = string.IsNullOrEmpty(this.Name1) ? string.Empty : this.Name1.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture).Substring(0, 1);
            }
            else
            {
                name1 = string.IsNullOrEmpty(this.Name1) ? string.Empty : this.Name1.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture);
            }
            string name3 = string.IsNullOrEmpty(this.Name3) ? string.Empty : this.Name3.ToTitleCase(System.Globalization.CultureInfo.CurrentCulture);
            string fullName = title + " " + name1 + " " + name3;
            return fullName.Trim();
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
                                    ClubcardID = t.Element(HouseholdCustomerDetailsEnum.ClubcardID.ToString()).GetValue<Int64>(),
                                    CustomerID=t.Element(HouseholdCustomerDetailsEnum.CustomerID.ToString()).GetValue<Int64>(),
                                    HouseHoldID = t.Element(HouseholdCustomerDetailsEnum.HouseHoldID.ToString()).GetValue<Int64>(),
                                    TitleEnglish = t.Element(HouseholdCustomerDetailsEnum.TitleEnglish.ToString()).GetValue<String>(),
                                    Name1 = t.Element(HouseholdCustomerDetailsEnum.Name1.ToString()).GetValue<String>(),
                                    Name2 = t.Element(HouseholdCustomerDetailsEnum.Name2.ToString()).GetValue<String>(),
                                    Name3 =t.Element(HouseholdCustomerDetailsEnum.Name3.ToString()).GetValue<String>(),
                                    MailingAddressLine1 = t.Element(HouseholdCustomerDetailsEnum.MailingAddressLine1.ToString()).GetValue<String>(),
                                    MailingAddressPostCode = t.Element(HouseholdCustomerDetailsEnum.MailingAddressPostCode.ToString()).GetValue<String>(),
                                    CustomerUseStatusID = t.Element(HouseholdCustomerDetailsEnum.CustomerUseStatusID.ToString()).GetValue<Int32>(), 
                                    CustomerMailStatus = t.Element(HouseholdCustomerDetailsEnum.CustomerMailStatus.ToString()).GetValue<Int32>(),
                                    PrimaryCustomerID=t.Element(HouseholdCustomerDetailsEnum.PrimaryCustomerID.ToString()).GetValue<Int32>()
                                }).ToList();
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("Customer")
                     select new HouseholdCustomerDetails
                     {
                         ClubcardID = t.Element(HouseholdCustomerDetailsEnum.ClubcardID.ToString()).GetValue<Int64>(),
                         
                         HouseHoldID = t.Element(HouseholdCustomerDetailsEnum.HouseHoldID.ToString()).GetValue<Int64>(),
                         
                     }).ToList();
        }

    }
}
