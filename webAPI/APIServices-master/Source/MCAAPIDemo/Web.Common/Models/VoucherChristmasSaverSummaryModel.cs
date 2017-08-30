using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class VoucherChristmasSaverSummaryModel : ComparableEntity<VoucherChristmasSaverSummaryModel>
    {
        List<ChristmasSaverSummary> _christmasSaverSummary { get; set; }
        public bool isXmasClubMember { get; set; }

        public List<ChristmasSaverSummary> christmasSaverSummary
        {
            get { return _christmasSaverSummary; }
            set { _christmasSaverSummary = value; }
        }

        internal override bool AreInstancesEqual(VoucherChristmasSaverSummaryModel target)
        {
            return
            (
                isXmasClubMember.Equals(target.isXmasClubMember)
                && Enumerable.SequenceEqual<ChristmasSaverSummary>(this.christmasSaverSummary, target.christmasSaverSummary)
            );
        }
    }
}
