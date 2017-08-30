using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class MultipleProfaneTextContributor: ProfaneTextProvider
    {
        char[] delimiters = new char[] { ',', '-', ' ' };

        private string _profaneTextTobeAdded;
        ProfaneTextProvider _profaneTextProvider;

        public MultipleProfaneTextContributor(ProfaneTextProvider profaneTextProvider, string profaneTextAdd)
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

                string[] profaneTexts = _profaneTextTobeAdded.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < profaneTexts.Length; i++)
                {
                    if (profaneText.ToString() != "")
                    {
                        profaneText = profaneText + ",";
                    }

                    profaneText = profaneText + profaneTexts[i];
                }

                return profaneText;
            }
        }

    }
}
