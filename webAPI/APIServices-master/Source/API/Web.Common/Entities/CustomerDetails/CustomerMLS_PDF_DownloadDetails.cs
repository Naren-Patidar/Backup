using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class CustomerMLS_PDF_DownloadDetails
    {
        public string PrimaryCustName1 { get; set; }

        public string PrimaryCustName2 { get; set; }

        public string PrimaryCustName3 { get; set; }

        public long PrimaryClubcardId { get; set; }

        public string AssociateCustName1 { get; set; }

        public string AssociateCustName2 { get; set; }

        public string AssociateCustName3 { get; set; }

        public long AssociateClubcardId { get; set; }
    }
}
