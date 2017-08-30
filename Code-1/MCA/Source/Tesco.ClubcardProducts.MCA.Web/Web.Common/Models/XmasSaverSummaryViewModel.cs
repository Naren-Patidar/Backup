using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class XmasSaverSummaryViewModel
    {
        public decimal totalVouchersSavedSofar { get; set; }
        public decimal clubcardVouchersSavedSofar { get; set; }
        public decimal toppedupVouchersSavedSofar { get; set; }
        public decimal bonusVouchersSavedSofar { get; set; }

        public bool displayBonusVocuherRow { get; set; }
        public bool displayCongratesMsgForMaxBonus { get; set; }
        public bool displayCongratesMsgForMinBonus { get; set; }
        


        CongratsMsgSectionValues congratsSectionValues = new CongratsMsgSectionValues();

        public CongratsMsgSectionValues CongratsSectionValues
        {
            get { return congratsSectionValues; }
            set { congratsSectionValues = value; }
        }

    }

    public class CongratsMsgSectionValues
    {
        public decimal recievedBonusVocuher { get; set; }
        public decimal bonusValueFor50 { get; set; }
        public decimal extraValueNeedToBeSaved { get; set; }
        public decimal requiredValueToMakeBonusVoucher { get; set; }
    }
}
