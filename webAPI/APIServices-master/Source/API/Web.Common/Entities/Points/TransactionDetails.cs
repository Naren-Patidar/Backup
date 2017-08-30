using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Points
{
    public class TransactionDetails
    {
        public string ClubCardTransactionId { get; set; }
        public string ClubcardId { get; set; }
        public string ClubCardStatusDescEnglish { get; set; }
        public string CustType { get; set; }
        public string TransactionDateTime { get; set; }
        public string PartnerId { get; set; }
        public string TescoStoreId { get; set; }
        public string AmountSpent { get; set; }
        public string NormalPoints { get; set; }
        public string BonusPoints { get; set; }
        public string TotalPoints { get; set; }
        public string TransactionDescription { get; set; }
        public string PointIssuePartnerGroupId { get; set; }
        public string PointIssuePartnerGroupDesc { get; set; }
    }
}
