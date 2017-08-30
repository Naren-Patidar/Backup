using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class OfferDetails
    {
        public int OfferID { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string PointsToRewardConversionRate { get; set; }
        public string CollectionPeriodNumber { get; set; }
    }
}
