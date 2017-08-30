namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class SingleProfaneTextContributor: ProfaneTextProvider
    {
        private string _profaneTextTobeAdded;
        ProfaneTextProvider _profaneTextProvider;

        public SingleProfaneTextContributor(ProfaneTextProvider profaneTextProvider, string profaneTextAdd)
        {
            _profaneTextProvider = profaneTextProvider;
            _profaneTextTobeAdded = profaneTextAdd;
        }

        public override string ProfaneText
        {
            get 
            {
                string profaneText = _profaneTextProvider.ProfaneText;

                if (string.IsNullOrEmpty(_profaneTextTobeAdded))
                    return profaneText;


                if (profaneText != "")
                {
                    profaneText = profaneText +",";
                }

                profaneText = profaneText + _profaneTextTobeAdded;

                return profaneText;
            }
        }
    }
}
