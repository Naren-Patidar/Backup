using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable] 
  public class OfferDetails
    {
        public int OfferID { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string PointsToRewardConversionRate { get; set; }
        public string CollectionPeriodNumber { get; set; }
        //public string OfferName { get; set; }
    }
}
