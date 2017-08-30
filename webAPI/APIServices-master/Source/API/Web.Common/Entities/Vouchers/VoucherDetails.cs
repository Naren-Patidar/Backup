using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Vouchers
{
    public class VoucherDetails : ComparableEntity<VoucherDetails>
    {
        public VoucherDetails()
        {
            this.PrimaryCustomerCardnumber = default(long).ToString();
            this.AssociateCustomerCardnumber = default(long).ToString();
        }

        public string HouseholdId { get; set; }
        public string PeriodName { get; set; }
        public string AlphaCode { get; set; }
        public string ExpiryDate { get; set; }
        public string Value { get; set; }
        public string VoucherNumber { get; set; }
        public string VNumberEncr { get; set; }
        public string VoucherType { get; set; }
        public string VoucherNumberToPrint { get; set; }
        public string PrimaryCustomerFirstname { get; set; }
        public string PrimaryCustomerMiddlename { get; set; }
        public string PrimaryCustomerLastname { get; set; }
        public string PrimaryCustomerCardnumber { get; set; }
        public string AssociateCustomerFirstname { get; set; }
        public string AssociateCustomerMiddlename { get; set; }
        public string AssociateCustomerLastname { get; set; }
        public string AssociateCustomerCardnumber { get; set; }
        public string BarCode { get; set; }
        public bool Selected { get; set; }

        internal override bool AreInstancesEqual(VoucherDetails target)
        {
            DateTime dtSource, dtTarget;

            bool bReturn = ((target.HouseholdId == this.HouseholdId) &&
                    (GeneralUtility.StringComparison(target.PeriodName, this.PeriodName)) &&
                    (target.AlphaCode == this.AlphaCode) &&
                    (target.ExpiryDate.TryParseDate(out dtTarget)) &&
                    (this.ExpiryDate.TryParseDate(out dtSource)) &&
                    (dtTarget.CompareDateTimeOnly(dtSource)) &&
                    (target.Value == this.Value) &&
                    (target.VoucherNumber == this.VoucherNumber) &&
                    (target.VoucherType == this.VoucherType) &&
                    (target.VoucherNumberToPrint == this.VoucherNumberToPrint) &&
                    (target.PrimaryCustomerFirstname == this.PrimaryCustomerFirstname) &&
                    (target.PrimaryCustomerMiddlename == this.PrimaryCustomerMiddlename) &&
                    (target.PrimaryCustomerLastname == this.PrimaryCustomerLastname) &&
                    (target.PrimaryCustomerCardnumber == this.PrimaryCustomerCardnumber) &&
                    (target.AssociateCustomerFirstname == this.AssociateCustomerFirstname) &&
                    (target.AssociateCustomerMiddlename == this.AssociateCustomerMiddlename) &&
                    (target.AssociateCustomerLastname == this.AssociateCustomerLastname) &&
                    (target.AssociateCustomerCardnumber == this.AssociateCustomerCardnumber) &&
                    (target.BarCode == this.BarCode) &&
                    (target.Selected == this.Selected));

            return bReturn;
        }
    }


    public class VoucherDetailsList : BaseEntity<VoucherDetailsList>
    {
        public VoucherDetailsList()
        {
            this._voucherDetails = new List<VoucherDetails>();
        }

        List<VoucherDetails> _voucherDetails;
        public List<VoucherDetails> VoucherDetailsListInstance
        {
            get { return _voucherDetails; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            DateTime dtTemp = DateTime.Now;

            _voucherDetails = ds.Tables["Table"].AsEnumerable()
            .Select(dr =>
                new VoucherDetails
                {
                    HouseholdId = dr.GetValue<string>(VoucherDetailsProperties.HouseholdId),
                    PeriodName = dr.GetValue<string>(VoucherDetailsProperties.PeriodName),
                    ExpiryDate = dr.GetValue<string>(VoucherDetailsProperties.ExpiryDate).TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                    Value = dr.GetValue<string>(VoucherDetailsProperties.Value),
                    VoucherNumber = dr.GetValue<string>(VoucherDetailsProperties.VoucherNumber),
                    VNumberEncr = CryptoUtility.EncryptTripleDES(dr.GetValue<string>(VoucherDetailsProperties.VoucherNumber)),
                    VoucherType = dr.GetValue<string>(VoucherDetailsProperties.VoucherType),
                    VoucherNumberToPrint = dr.GetValue<string>(VoucherDetailsProperties.VoucherNumberToPrint),
                    AlphaCode = dr.GetValue<string>(VoucherDetailsProperties.AlphaCode),
                    BarCode = dr.GetValue<string>(VoucherDetailsProperties.TwentyTwoDigitVoucher_Number),
                    Selected = false
                }).ToList();
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;

            XDocument xDoc = XDocument.Parse(xml);
            _voucherDetails = (from t in xDoc.Descendants("Table")
                               select new VoucherDetails
                               {
                                   HouseholdId = t.Element(VoucherDetailsProperties.HouseholdId).GetValue<string>(),
                                   PeriodName = t.Element(VoucherDetailsProperties.PeriodName).GetValue<string>(),
                                   ExpiryDate = t.Element(VoucherDetailsProperties.ExpiryDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                                   Value = t.Element(VoucherDetailsProperties.Value).GetValue<string>(),
                                   VoucherNumber = t.Element(VoucherDetailsProperties.VoucherNumber).GetValue<string>(),
                                   VNumberEncr = CryptoUtility.EncryptTripleDES(t.Element(VoucherDetailsProperties.VoucherNumber).GetValue<string>()),
                                   VoucherType = t.Element(VoucherDetailsProperties.VoucherType).GetValue<string>(),
                                   VoucherNumberToPrint = t.Element(VoucherDetailsProperties.VoucherNumberToPrint).GetValue<string>(),
                                   AlphaCode = t.Element(VoucherDetailsProperties.AlphaCode).GetValue<string>(),
                                   BarCode = t.Element(VoucherDetailsProperties.TwentyTwoDigitVoucher_Number).GetValue<string>(),
                                   Selected = false
                               }).ToList();
        }
    }
}