using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points
{
 [Serializable] 
 public class TransactionDetails
 {
public long ClubCardTransactionId { get; set; }
public string ClubcardId { get; set; }
public string ClubCardStatusDescEnglish { get; set; }
public string CustType { get; set; }
public string TransactionDateTime { get; set; }
public long PartnerId { get; set; }
public long TescoStoreId { get; set; }
public string AmountSpent { get; set; }
public string NormalPoints { get; set; }
public string BonusPoints { get; set; }
public string TotalPoints { get; set; }
public string TransactionDescription { get; set; }
public string PointIssuePartnerGroupId { get; set; }
public string PointIssuePartnerGroupDesc { get; set; }

    }
}
