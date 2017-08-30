using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Collections.Generic;
using System.Globalization;
namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost
{
    public class Reward : ComparableEntity<Reward>
    {
        public DateTime BookingDate { get; set; }
        public string TokenDescription { get; set; }
        public string ProductStatus { get; set; }
        public decimal TokenValue { get; set; }
        public string SupplierTokenCode { get; set; }
        public DateTime ValidUntil { get; set; }

        internal override bool AreInstancesEqual(Reward target)
        {
            return (target.BookingDate.CompareDateTimeOnly(this.BookingDate)) &&
                    (target.TokenDescription == this.TokenDescription) &&
                    (target.ProductStatus == this.ProductStatus) &&
                    (target.TokenValue == this.TokenValue) &&
                    (target.SupplierTokenCode == this.SupplierTokenCode) &&
                    (target.ValidUntil.CompareDateTimeOnly(this.ValidUntil));
        }
    }
    public class RewardList : BaseEntity<RewardList>
    {
        List<Reward> _reward;
        public List<Reward> RewardsList
        {
            get { return _reward; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = new DateTime();
            XDocument xDoc = XDocument.Parse(xml);
            _reward = (from t in xDoc.Descendants("RewardDetails")
                               select new Reward
                               {
                                    BookingDate = t.Element(RewardEnum.BookingDate.ToString()).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                                    TokenDescription = t.Element(RewardEnum.TokenDescription.ToString()).GetValue<string>(),
                                    ProductStatus = t.Element(RewardEnum.ProductStatus.ToString()).GetValue<string>(),
                                    TokenValue = Convert.ToDecimal(t.Element(RewardEnum.TokenValue.ToString()).GetValue<string>(), CultureInfo.InvariantCulture),
                                    SupplierTokenCode = t.Element(RewardEnum.SupplierTokenCode.ToString()).GetValue<string>(),
                                    ValidUntil = t.Element(RewardEnum.ValidUntil.ToString()).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now
                                 
                               }).ToList();
        }
    }
}
