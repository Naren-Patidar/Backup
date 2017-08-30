using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    public class Offer
    {
        public string Period { get; set; }
        public int Id { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string PointsBalanceQty { get; set; }
        public string Vouchers { get; set; }
    }

    public class OfferList : BaseEntity<OfferList>
    {
        List<Offer> _offerDetails;
        public List<Offer> OfferDetailList
        {
            get { return _offerDetails; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            DataSet newDataSet = new DataSet();
            DataTable pointsInfoTable = ds.Tables[0];
            //--Add missing columns in table
            pointsInfoTable.AddMissingColumns(typeof(OffersEnum));
            ds.Tables.Remove(pointsInfoTable);
            newDataSet.Tables.Add(pointsInfoTable);
            XDocument xDoc = XDocument.Parse(newDataSet.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }
        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);

            _offerDetails = (from r in xDoc.Descendants("PointsInfoAllCollPrds")
                             select new Offer
                                {
                                    Id = r.Element(OffersEnum.OfferID).GetValue<Int32>(),
                                    Vouchers = (string.IsNullOrEmpty(r.Element(OffersEnum.Vouchers).GetValue<string>())) ?
                                                        ("0.00").TryParseDecimal().TryParse<string>() : 
                                                        string.Format("{0:0.00}", r.Element(OffersEnum.Vouchers).GetValue<string>().TryParseDecimal().TryParse<string>()),
                                    PointsBalanceQty = (string.IsNullOrEmpty(r.Element(OffersEnum.PointsBalanceQty).GetValue<string>())) ? "0"
                                                        : r.Element(OffersEnum.PointsBalanceQty).GetValue<string>(),
                                    StartDateTime = r.Element(OffersEnum.StartDateTime).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                                    EndDateTime = r.Element(OffersEnum.EndDateTime).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                                    Period = r.Element(OffersEnum.OfferPeriod).GetValue<string>()
                                }).ToList();
        }
    }
}
