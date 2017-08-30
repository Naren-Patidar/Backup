using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class XmasSaverViewModel
    {
        public bool IsOptionsAndBenefitsEnabled { get; set; }
        public string XmasSaverYear { get; set; }        
        public XmasSaverHeaderViewModel xmasSaverHeaderModel { get; set; }
        public XmasSaverSummaryViewModel xmasSaverSummaryModel { get; set; }
        public XmasSaverVoucherSavedViewModel xmasSaverVoucherSavedModel { get; set; }
        public XmasSaverTopUpViewModel xmasSaverTopUpModel { get; set; }



    }
}
