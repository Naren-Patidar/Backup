using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class CouponBoxViewModel
    {
        List<string> resources = new List<string>();
        decimal value = 0;        

        public List<string> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public decimal Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

    }
}
