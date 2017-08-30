using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon
{
    [Serializable]
    public class CouponDetails : BaseEntity<CouponDetails>
    {
        public string ExpiryDate { get; set; }
        public string BarcodeNumber { get; set; }
        public string ImageName { get; set; }
        public string PrimaryCustomerFirstName { get; set; }
        public string PrimaryCustomerMiddleName { get; set; }
        public string PrimaryCustomerLastName { get; set; }
        public long PrimaryClubcardnumber { get; set; }
        public string AssociateCustomerFirstName { get; set; }
        public string AssociateCustomerMiddleName { get; set; }
        public string AssociateCustomerLastName { get; set; }
        public long AssociateClubcardNumber { get; set; }
        public DateTime? RedemptionEndDate { get; set; }
        public string ThumbnailImageName { get; set; }
        public string FullImageName { get; set; }
        public string CouponDescription { get; set; }
        public string SmartAlphaNumeric { get; set; }
        public int MaxRedemptionLimit { get; set; }
        public int RedemptionUtilized { get; set; }
        public string TotalRedeemedCoupon { get; set; }
        public string CouponStatus { get; set; }
        public string  RedemptionCount { get; set; }
        public string TotalCoupons { get; set; }
        public string TotalRedemption { get; set; }

        public DateTime IssuanceStartDate { get; set; }
        public DateTime RedemptionDate{ get; set; }
        public string RedemptionStoreName { get; set; }
        

        public string WhereUsed { get; set; }
        
        public string AdditionalInfo { get; set; }
        public string AlphaCode { get; set; }
        public string EndDate { get; set; }
        public string StartDate { get; set; }
        public bool IsSelected { get; set; }
        public Int64 Id { get; set; }
        public bool HasImage { get; set; }

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


    [Serializable]
    public class CouponDetailsList : BaseEntity<CouponDetailsList>
    {
        List<CouponDetails> _list = new List<CouponDetails>();

        public List<CouponDetails> List
        {
            get { return _list; }
            set { _list = value; }
        }

        public override void ConvertFrom<T>(T obj)
        {
            _list = AutoMapper.Mapper.Map<T, List<CouponDetails>>(obj); 
        }

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            int index = 0;
            _list = (from t in xDoc.Descendants("UsedCouponDetail")
                               select new CouponDetails
                               {
                                   IssuanceStartDate = t.Element(CouponDetailsEnum.IssuanceStartDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                                   RedemptionDate = t.Element(CouponDetailsEnum.RedemptionDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
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
                                   RedemptionEndDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
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
                         IssuanceStartDate = t.Element(CouponDetailsEnum.IssuanceStartDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                         RedemptionDate = t.Element(CouponDetailsEnum.RedemptionDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
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
                         RedemptionEndDate = t.Element(CouponDetailsEnum.RedemptionEndDate).GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now,
                         Id = index++
                     }).ToList();
        }

    }
}

