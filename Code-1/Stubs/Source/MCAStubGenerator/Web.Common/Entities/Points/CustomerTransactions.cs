using System;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points
{
    [Serializable] 
    public class CustomerTransactions
    {
        public OfferDetails Offer { get; set; }
        public List<TransactionDetails> Transactions { get; set; }
    }
}
