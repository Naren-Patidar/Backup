using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Ecoupon
{
    public class CouponDetails : BaseEntity<CouponDetails>
    {
        public string ExpiryDate { get; set; }
        public string BarcodeNumber { get; set; }
        public string RedemptionEndDate { get; set; }
        public string ThumbnailImageName { get; set; }
        public string FullImageName { get; set; }
        public string CouponDescription { get; set; }
        public string SmartAlphaNumeric { get; set; }
        public int MaxRedemptionLimit { get; set; }
        public int RedemptionUtilized { get; set; }
        public string TotalRedeemedCoupon { get; set; }
        public string CouponStatus { get; set; }
        public string  RedemptionCount { get; set; }
        public string TotalRedemption { get; set; }

        public string IssuanceStartDate { get; set; }
        public string RedemptionDate{ get; set; }
        public string RedemptionStoreName { get; set; }
        
        public string AlphaCode { get; set; }
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            var couponDetail = obj as CouponDetails;
            return (string.IsNullOrEmpty(couponDetail.AlphaCode) && string.IsNullOrEmpty(this.AlphaCode)) ||  this.AlphaCode.Equals(couponDetail.AlphaCode)
                   && (string.IsNullOrEmpty(couponDetail.CouponDescription) && string.IsNullOrEmpty(this.CouponDescription)) || this.CouponDescription.Equals(couponDetail.CouponDescription)
                   && (string.IsNullOrEmpty(couponDetail.FullImageName) && string.IsNullOrEmpty(this.FullImageName)) || this.FullImageName.Equals(couponDetail.FullImageName)
                   && this.RedemptionEndDate.Equals(couponDetail.RedemptionEndDate);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class CouponDetailsList : BaseEntity<CouponDetailsList>
    {
        List<CouponDetails> _list = new List<CouponDetails>();

        public List<CouponDetails> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            int index = 0;
            _list = (from t in xDoc.Descendants("UsedCouponDetail")
                               select new CouponDetails
                               {
                                   IssuanceStartDate = t.Element(CouponDetailsEnum.IssuanceStartDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                                   RedemptionDate = t.Element(CouponDetailsEnum.RedemptionDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                                   RedemptionStoreName = t.Element(CouponDetailsEnum.RedemptionStoreName).GetValue<string>(),
                                   CouponStatus = t.Element(CouponDetailsEnum.CouponStatus).GetValue<string>(),
                                   RedemptionCount = t.Element(CouponDetailsEnum.RedemptionCount).GetValue<string>(),
                                   CouponDescription = t.Element(CouponDetailsEnum.CouponDescription).GetValue<string>(),
                                   TotalRedeemedCoupon = t.Element(CouponDetailsEnum.TotalRedeemedCoupon).GetValue<string>(),
                                   TotalRedemption = t.Element(CouponDetailsEnum.TotalRedemption).GetValue<string>(),
                                   FullImageName = t.Element(CouponDetailsEnum.FullImageName).GetValue<string>(),
                                   BarcodeNumber = t.Element(CouponDetailsEnum.SmartBarcode).GetValue<string>(),
                                   ExpiryDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>(),
                                   MaxRedemptionLimit = t.Element(CouponDetailsEnum.MaxRedemptionLimit).GetValue<Int32>(),
                                   RedemptionUtilized = t.Element(CouponDetailsEnum.RedemptionUtilized).GetValue<Int32>(),
                                   ThumbnailImageName = t.Element(CouponDetailsEnum.ThumbnailImageName).GetValue<string>(),
                                   SmartAlphaNumeric = t.Element(CouponDetailsEnum.SmartAlphaNumeric).GetValue<string>(),
                                   RedemptionEndDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                                   Id = index++
                               }).ToList();
        }

        public override void ConvertFromXml(string xml, string parent)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            int index = 0;
            _list = (from t in xDoc.Descendants(parent)
                     select new CouponDetails
                     {
                         IssuanceStartDate = t.Element(CouponDetailsEnum.IssuanceStartDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                         RedemptionDate = t.Element(CouponDetailsEnum.RedemptionDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                         RedemptionStoreName = t.Element(CouponDetailsEnum.RedemptionStoreName).GetValue<string>(),
                         CouponStatus = t.Element(CouponDetailsEnum.CouponStatus).GetValue<string>(),
                         RedemptionCount = t.Element(CouponDetailsEnum.RedemptionCount).GetValue<string>(),
                         CouponDescription = t.Element(CouponDetailsEnum.CouponDescription).GetValue<string>(),
                         TotalRedeemedCoupon = t.Element(CouponDetailsEnum.TotalRedeemedCoupon).GetValue<string>(),
                         TotalRedemption = t.Element(CouponDetailsEnum.TotalRedemption).GetValue<string>(),
                         FullImageName = t.Element(CouponDetailsEnum.FullImageName).GetValue<string>(),
                         BarcodeNumber = t.Element(CouponDetailsEnum.SmartBarcode).GetValue<string>(),
                         ExpiryDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>(),
                         MaxRedemptionLimit = t.Element(CouponDetailsEnum.MaxRedemptionLimit).GetValue<Int32>(),
                         RedemptionUtilized = t.Element(CouponDetailsEnum.RedemptionUtilized).GetValue<Int32>(),
                         ThumbnailImageName = t.Element(CouponDetailsEnum.ThumbnailImageName).GetValue<string>(),
                         SmartAlphaNumeric = t.Element(CouponDetailsEnum.SmartAlphaNumeric).GetValue<string>(),
                         RedemptionEndDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString("o") : String.Empty,
                         Id = index++
                     }).ToList();
        }

    }
}

