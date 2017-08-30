using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost
{
    [Serializable]
    public class Token : ComparableEntity<Token>
    {
        public DateTime BookingDate { get; set; }
        public long TokenID { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public string TokenIDEncr { get; set; }

        public string TokenDescription { get; set; }
        public string ProductStatus { get; set; }
        public decimal TokenValue { get; set; }
        public string SupplierTokenCode { get; set; }
        public DateTime ValidUntil { get; set; }
        public string ValidUntilFormated { get; set; }
        public string ProductCode { get; set; }
        public long ProductTokenValue { get; set; }
        public string ProductTokenValuewithcurrency { get; set; }
        public string QualifyingSpend { get; set; }
        public string Title { get; set; }
        public string Includes { get; set; }
        public string Excludes { get; set; }
        public string TermsAndConditions { get; set; }
        public bool Selected { get; set; }

        internal override bool AreInstancesEqual(Token tokenTarget)
        {
            return
             (tokenTarget.BookingDate.CompareDateTimeOnly(this.BookingDate)) &&
             (tokenTarget.TokenID == this.TokenID) &&
             (tokenTarget.TokenDescription == this.TokenDescription) &&
             (tokenTarget.ProductStatus == this.ProductStatus) &&
             (tokenTarget.TokenValue == this.TokenValue) &&
             (tokenTarget.SupplierTokenCode == this.SupplierTokenCode) &&
             (tokenTarget.ValidUntil.CompareDateTimeOnly(this.ValidUntil)) &&
             (tokenTarget.ProductCode == this.ProductCode) &&
             (tokenTarget.ProductTokenValue == this.ProductTokenValue) &&
             (tokenTarget.QualifyingSpend == this.QualifyingSpend) &&
             (tokenTarget.Title == this.Title) &&
             (tokenTarget.Includes == this.Includes) &&
             (tokenTarget.Excludes == this.Excludes) &&
             (tokenTarget.TermsAndConditions == this.TermsAndConditions) &&
             (tokenTarget.Selected == this.Selected);
        }
    }

    public class TokenList : BaseEntity<TokenList>
    {
        List<Token> _token;
        public List<Token> TokensList
        {
            get { return _token; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;

            XDocument xDoc = XDocument.Parse(xml);
            _token = (from t in xDoc.Descendants("TokenDetails")
                      select new Token
                      {
                          BookingDate = t.Element(TokenEnum.BookingDate.ToString()).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                          TokenID = t.Element(TokenEnum.TokenId.ToString()).GetValue<long>(),
                          TokenIDEncr = CryptoUtility.EncryptTripleDES(t.Element(TokenEnum.TokenId.ToString()).GetValue<long>().ToString()),
                          TokenDescription = t.Element(TokenEnum.TokenDescription.ToString()).GetValue<string>(),
                          ProductStatus = t.Element(TokenEnum.ProductStatus.ToString()).GetValue<string>(),
                          TokenValue = Convert.ToDecimal(t.Element(RewardEnum.TokenValue.ToString()).GetValue<string>(), CultureInfo.InvariantCulture),
                          SupplierTokenCode = t.Element(TokenEnum.SupplierTokenCode.ToString()).GetValue<string>(),
                          ValidUntil = t.Element(TokenEnum.ValidUntil.ToString()).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                          ProductCode = t.Element(TokenEnum.ProductCode.ToString()).GetValue<string>(),
                          ProductTokenValue = t.Element(TokenEnum.ProductTokenValue.ToString()).GetValue<long>(),
                          QualifyingSpend = t.Element(TokenEnum.QualifyingSpend.ToString()).GetValue<string>(),
                          Title = t.Element(TokenEnum.Title.ToString()).GetValue<string>(),
                          Includes = t.Element(TokenEnum.Includes.ToString()).GetValue<string>(),
                          Excludes = t.Element(TokenEnum.Excludes.ToString()).GetValue<string>(),
                          TermsAndConditions = t.Element(TokenEnum.TermsAndConditions.ToString()).GetValue<string>(),
                          Selected = false

                      }).ToList();
        }

      


    }
}
