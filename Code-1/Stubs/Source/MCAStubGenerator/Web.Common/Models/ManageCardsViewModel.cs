using System;
using System.Collections.Generic;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class ManageCardsViewModel
    {

        private List<HouseholdCustomerDetails> _households = new List<HouseholdCustomerDetails>();


        public List<HouseholdCustomerDetails> Households
        {
            get { return _households; }
            set { _households = value; }
        }

        public int HouseholdCount
        {
            get
            {
                return _households.Count;
            }
        }

    }
}
