namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost
{
    public class TokenDetails
    {
        public string ExpiryDate { get; set; }

        public string Value { get; set; }

        public long TokenId { get; set; }

        public string TokenCode { get; set; }

        public string TokenValue { get; set; }

        public string QualifySpend { get; set; }

        public string Includes { get; set; }

        public string Excludes { get; set; }

        public string TermsAndCondition { get; set; }

    }
}