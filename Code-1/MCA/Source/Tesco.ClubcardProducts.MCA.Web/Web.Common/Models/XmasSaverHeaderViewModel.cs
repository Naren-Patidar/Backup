using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class XmasSaverHeaderViewModel
    {
        public string XmasSaverYear { get; set; }
        Decimal _totalVoucherSaver = 0;

        public Decimal TotalVoucherSaver
        {
            get { return _totalVoucherSaver; }
            set { _totalVoucherSaver = value; }
        }
    }
}
