using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class VoucherBasicModel
    {
        public bool ShowHoldingPage { get; set; }
        public string CIDENCR{ get; set; }
        public string CCENCR { get; set; }
        public string CLENCR { get; set; }
    }
}
