using System;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Points
{
    public class CustomerTransactions
    {
        public OfferDetails Offer { get; set; }
        public List<TransactionDetails> Transactions { get; set; }
    }
}
