using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class XmasSaverTopUpViewModel
    {
        List<ChristmasSaverSummary> christmasSaverSummary = new List<ChristmasSaverSummary>();
        public decimal sumTtlToppedUpMoney { get; set; }
        public string topUpYourAccountUpto { get; set; }
        public decimal rewardedBonus { get; set; }
        public decimal topupWithOverValue { get; set; }
        public decimal bonuVoucherinReturn { get; set; }

        public bool displayMsgForTopupBonus { get; set; }

        public List<ChristmasSaverSummary> ChristmasSaverSummaryList
        {
            get { return christmasSaverSummary; }
            set { christmasSaverSummary = value; }
        }

    }
}
