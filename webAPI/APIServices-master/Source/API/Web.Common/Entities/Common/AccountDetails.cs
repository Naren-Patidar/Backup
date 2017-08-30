﻿using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Common
{
    public class AccountDetails 
    {
        public string CustomerID { get; set; }
        public string TitleEnglish { get; set; }
        public string Name3 { get; set; }
        public string Name1 { get; set; }
        public string PointsBalanceQty { get; set; }
        public double Vouchers { get; set; }
        public string PrimaryCustomerFullName { get; set; }
        public string ClubcardID { get; set; }
        public string PrimaryCustomerName1 { get; set; }
        public string PrimaryCustomerName2 { get; set; }
        public string PrimaryCustomerName3 { get; set; }
        public string PrimaryClubcardID { get; set; }
        public string AssociateCustName1 { get; set; }
        public string AssociateCustName2 { get; set; }
        public string AssociateCustName3 { get; set; }
        public string AssociateClubcardID { get; set; }
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
                                   TitleEnglish = t.Element(AccountDetailsEnum.TitleEnglish).GetValue<string>(),
                                   Name3 = t.Element(AccountDetailsEnum.Name3).GetValue<string>(),
                                   Name1 = t.Element(AccountDetailsEnum.Name1).GetValue<string>(),
                                   PointsBalanceQty = t.Element(AccountDetailsEnum.PointsBalanceQty).GetValue<string>(),
                                   Vouchers = t.Element(AccountDetailsEnum.Vouchers).GetValue<double>(),
                                   PrimaryCustomerFullName = t.Element(AccountDetailsEnum.PrimaryCustName).GetValue<string>(),
                                   CustomerID = t.Element(AccountDetailsEnum.CustomerID).GetValue<string>(),
                                   ClubcardID = t.Element(AccountDetailsEnum.ClubcardID).GetValue<string>(),
                                   PrimaryCustomerName1 = t.Element(AccountDetailsEnum.PrimaryCustName1).GetValue<string>(),
                                   PrimaryCustomerName2 = t.Element(AccountDetailsEnum.PrimaryCustName2).GetValue<string>(),
                                   PrimaryCustomerName3 = t.Element(AccountDetailsEnum.PrimaryCustName3).GetValue<string>(),
                                   PrimaryClubcardID = t.Element(AccountDetailsEnum.PrimaryClubcardID).GetValue<string>(),
                                   AssociateCustName1 = t.Element(AccountDetailsEnum.AssociateCustName1).GetValue<string>(),
                                   AssociateCustName2 = t.Element(AccountDetailsEnum.AssociateCustName2).GetValue<string>(),
                                   AssociateCustName3 = t.Element(AccountDetailsEnum.AssociateCustName3).GetValue<string>(),
                                   AssociateClubcardID = t.Element(AccountDetailsEnum.AssociateClubcardID).GetValue<string>()
                               }).ToList();
        }

     
    }
}
