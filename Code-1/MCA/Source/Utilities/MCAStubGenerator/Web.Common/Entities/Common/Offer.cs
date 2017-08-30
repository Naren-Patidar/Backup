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
            XDocument xDoc = XDocument.Parse(xml);

            _offerDetails = (from r in xDoc.Descendants("PointsInfoAllCollPrds")
                             select new Offer
                                     {
                                         Id= (Int32)r.Element(OffersEnum.OfferID.ToString()),
                                         Vouchers = (string.IsNullOrEmpty(r.Element(OffersEnum.Vouchers.ToString()).ToString())) ?
                                         (Convert.ToDecimal("0.00", CultureInfo.InvariantCulture).ToString()) : string.Format("{0:0.00}",
                                         Convert.ToDecimal(r.Element(OffersEnum.Vouchers.ToString()).Value, CultureInfo.InvariantCulture).ToString()),
                                         PointsBalanceQty = (string.IsNullOrEmpty(r.Element(OffersEnum.PointsBalanceQty.ToString()).ToString())) ? "0"
                                         : r.Element(OffersEnum.PointsBalanceQty.ToString()).Value.ToString(),
                                         StartDateTime = (r.Element(OffersEnum.StartDateTime.ToString()).Value).ToString().ToNullableDateTime(),
                                         EndDateTime = (r.Element(OffersEnum.EndDateTime.ToString()).Value).ToString().ToNullableDateTime(),
                                         Period = Convert.ToString(r.Element(OffersEnum.OfferPeriod.ToString()).Value)
                                     }).ToList();
        }
    }
}
