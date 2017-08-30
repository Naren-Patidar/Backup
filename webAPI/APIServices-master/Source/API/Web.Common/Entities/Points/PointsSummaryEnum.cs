namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Points
{
    public static class PointsSummaryEnum
    {
        public const string CustomerType = "CustomerType";
        public const string TotalPoints = "TotalPoints";
        public const string TotalReward = "TotalReward";
        public const string TopUpVouchers = "TopUpVouchers";
        public const string BonusVouchers = "BonusVouchers";
        public const string TotalRewardMiles = "TotalRewardMiles";
        public const string NoCoupons = "NoCoupons";
        public const string StatementVideo = "StatementVideo";
        public const string Salutation = "Salutation";
        public const string MainClubcardID = "MainClubcardID";
        public const string RewardMilesRate = "RewardMilesRate";
        public const string StatementType = "StatementType";
        public const string StartDateTime = "StartDateTime";
        public const string EndDateTime = "EndDateTime";
        public const string PointSummaryDescEnglish = "PointSummaryDescEnglish";
    }

    public enum PointsTypeEnum
    {
        //Added by keerthi on 26-Sep-2014 Code refactoring changes .
        TescoPointsChangeFromRewards,
        TescoCarriedForwardPoints,
        TescoBankPoints,
        TescoBankBroughtForwardPoints,
        TescoBankCarriedForwardPoints,
        TotalCarriedForwardPoints,
        TescoPoints,
        TescoBroughtForwardPoints,
        TescoBankPointsChangeFromRewards,
        TescoPoints1,
        TescoPoints2,
        TescoPoints3,
        TescoPoints4,
        TescoPoints5,
        TescoPoints6,
        TescoPoints7,
        TescoPoints8,
        TescoPoints9,
        TescoPoints10,
        TescoPoints11,
        TescoPoints12,
        TescoPoints13,
        TescoPoints14,
        TescoPoints15,
        TescoPoints16,
        TescoPoints17,
        TescoPoints18,
        TescoPoints19,
        TescoPoints20,
        TescoBankPoints1,
        TescoBankPoints2,
        TescoBankPoints3,
        TescoBankPoints4,
        TescoBankPoints5,
        TescoBankPoints6,
        TescoBankPoints7,
        TescoBankPoints8,
        TescoBankPoints9,
        TescoBankPoints10
    }

    public enum RewardsTypeEnum
    {
        TescoTotalReward,
        TescoBankTotalReward,
        TescoRewardMiles,
        TescoBankRewardMiles,
    }
}
