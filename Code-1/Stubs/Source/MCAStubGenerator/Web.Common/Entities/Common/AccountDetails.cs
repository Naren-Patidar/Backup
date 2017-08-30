using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    [Serializable]
    public class AccountDetails 
    {
        public long CustomerID { get; set; }
        public string TitleEnglish { get; set; }
        public string Name3 { get; set; }
        public string Name1 { get; set; }
        public long PointsBalanceQty { get; set; }
        public double Vouchers { get; set; }
        public string PrimaryCustomerFullName { get; set; }
        public long ClubcardID { get; set; }
        public string PrimaryCustomerName1 { get; set; }
        public string PrimaryCustomerName2 { get; set; }
        public string PrimaryCustomerName3 { get; set; }
        public long PrimaryClubcardID { get; set; }
        public string AssociateCustName1 { get; set; }
        public string AssociateCustName2 { get; set; }
        public string AssociateCustName3 { get; set; }
        public long AssociateClubcardID { get; set; }
        public string EmailAddress{ get; set; }
    }

    public class AccountDetailsList : BaseEntity<AccountDetailsList>
    {
        List<AccountDetails> _accountDetails;
        public List<AccountDetails> AccountDetailList
        {
            get { return _accountDetails; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            XDocument xDoc = XDocument.Parse(xml);
            _accountDetails = (from t in xDoc.Descendants("ViewMyAccountDetails")
                               select new AccountDetails
                               {
                                   TitleEnglish = t.Element(AccountDetailsEnum.TitleEnglish.ToString()).GetValue<string>(),
                                   Name3 = t.Element(AccountDetailsEnum.Name3.ToString()).GetValue<string>(),
                                   Name1 = t.Element(AccountDetailsEnum.Name1.ToString()).GetValue<string>(),
                                   PointsBalanceQty = t.Element(AccountDetailsEnum.PointsBalanceQty.ToString()).GetValue<long>(),
                                   Vouchers = t.Element(AccountDetailsEnum.Vouchers.ToString()).GetValue<double>(),
                                   PrimaryCustomerFullName = t.Element(AccountDetailsEnum.PrimaryCustName.ToString()).GetValue<string>(),
                                   CustomerID = t.Element(AccountDetailsEnum.CustomerID.ToString()).GetValue<long>(),
                                   ClubcardID = t.Element(AccountDetailsEnum.ClubcardID.ToString()).GetValue<long>(),
                                   PrimaryCustomerName1 = t.Element(AccountDetailsEnum.PrimaryCustName1.ToString()).GetValue<string>(),
                                   PrimaryCustomerName2 = t.Element(AccountDetailsEnum.PrimaryCustName2.ToString()).GetValue<string>(),
                                   PrimaryCustomerName3 = t.Element(AccountDetailsEnum.PrimaryCustName3.ToString()).GetValue<string>(),
                                   PrimaryClubcardID = t.Element(AccountDetailsEnum.PrimaryClubcardID.ToString()).GetValue<long>(),
                                   AssociateCustName1 = t.Element(AccountDetailsEnum.AssociateCustName1.ToString()).GetValue<string>(),
                                   AssociateCustName2 = t.Element(AccountDetailsEnum.AssociateCustName2.ToString()).GetValue<string>(),
                                   AssociateCustName3 = t.Element(AccountDetailsEnum.AssociateCustName3.ToString()).GetValue<string>(),
                                   AssociateClubcardID = t.Element(AccountDetailsEnum.AssociateClubcardID.ToString()).GetValue<long>()
                               }).ToList();
        }

     
    }
}
