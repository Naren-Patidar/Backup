using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Common
{
    public class SearchCustomerDetails : BaseEntity<SearchCustomerDetails>
    {
        public string CustomerID { get; set; }
        public string TitleEnglish { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string FullName { get; set; }
        public string MailingAddressLine1 { get; set; }
        public string MailingAddressLine2 { get; set; }
        public string MailingAddressLine3 { get; set; }
        public string MailingAddressLine4 { get; set; }
        public string MailingAddressLine5 { get; set; }
        public string MailingAddressPostCode { get; set; }
        public string CurrentPointsBalanceQty { get; set; }
        public string PreviousPointsBalanceQty { get; set; }
        public string JoinedDate { get; set; }
        public int CustomerUseStatusID { get; set; }
        public string ClubcardID { get; set; }
        public string HouseHoldID { get; set; }
        public string CardIssuedDate { get; set; }
        public string JoinRouteDesc { get; set; }
        public string PromotionCode { get; set; }
    }

    public class SearchCustomerDetailsList : BaseEntity<SearchCustomerDetailsList>
    {
        public SearchCustomerDetailsList()
        {
            this._list = new List<SearchCustomerDetails>();
        }

        List<SearchCustomerDetails> _list = null;

        public List<SearchCustomerDetails> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = new DateTime();
            XDocument xDoc = XDocument.Parse(xml);
            _list = (from t in xDoc.Descendants("Customer")
                     select new SearchCustomerDetails
                     {
                         CustomerID = t.Element(SearchCustomerDetailsEnum.CustomerID).GetValue<string>(),
                         ClubcardID = t.Element(SearchCustomerDetailsEnum.ClubcardID).GetValue<string>(),
                         HouseHoldID = t.Element(SearchCustomerDetailsEnum.HouseHoldID).GetValue<string>(),
                         CardIssuedDate = t.Element(SearchCustomerDetailsEnum.CardIssuedDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                         TitleEnglish = t.Element(SearchCustomerDetailsEnum.TitleEnglish).GetValue<String>(),
                         Name1 = t.Element(SearchCustomerDetailsEnum.Name1).GetValue<String>(),
                         Name2 = t.Element(SearchCustomerDetailsEnum.Name2).GetValue<String>(),
                         Name3 = t.Element(SearchCustomerDetailsEnum.Name3).GetValue<String>(),
                         MailingAddressLine1 = t.Element(SearchCustomerDetailsEnum.MailingAddressLine1).GetValue<String>(),
                         MailingAddressLine2 = t.Element(SearchCustomerDetailsEnum.MailingAddressLine2).GetValue<String>(),
                         MailingAddressLine3 = t.Element(SearchCustomerDetailsEnum.MailingAddressLine3).GetValue<String>(),
                         MailingAddressLine4 = t.Element(SearchCustomerDetailsEnum.MailingAddressLine4).GetValue<String>(),
                         MailingAddressLine5 = t.Element(SearchCustomerDetailsEnum.MailingAddressLine5).GetValue<String>(),
                         MailingAddressPostCode = t.Element(SearchCustomerDetailsEnum.MailingAddressPostCode).GetValue<String>(),
                         CurrentPointsBalanceQty = t.Element(SearchCustomerDetailsEnum.CurrentPointsBalanceQty).GetValue<String>(),
                         PreviousPointsBalanceQty = t.Element(SearchCustomerDetailsEnum.PreviousPointsBalanceQty).GetValue<String>(),
                         JoinedDate = t.Element(SearchCustomerDetailsEnum.JoinedDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                         CustomerUseStatusID = t.Element(SearchCustomerDetailsEnum.CustomerUseStatusID).GetValue<Int32>(),
                         PromotionCode = t.Element(SearchCustomerDetailsEnum.PromotionCode).GetValue<string>(),
                         JoinRouteDesc = t.Element(SearchCustomerDetailsEnum.JoinRouteDesc).GetValue<string>()
                     }).ToList();
        }

    }
}
