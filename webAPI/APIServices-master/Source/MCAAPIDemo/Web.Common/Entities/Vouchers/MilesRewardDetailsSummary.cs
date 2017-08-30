namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    public class MilesRewardDetailsSummary
    {
        private int _totalRewardPoints = 0;
        private string _formattedVoucherValue = string.Empty;

        public int TotalRewardPoints
        {
            get { return _totalRewardPoints; }
            set { _totalRewardPoints = value; }
        }
        public string FormattedVoucherValue
        {
            get { return _formattedVoucherValue; }
            set { _formattedVoucherValue = value; }
        }
    }
}
