namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class EmptyProfaneTextContributor: ProfaneTextProvider
    {
        public override string ProfaneText
        {
            get { return string.Empty; }
        }
    }
}
