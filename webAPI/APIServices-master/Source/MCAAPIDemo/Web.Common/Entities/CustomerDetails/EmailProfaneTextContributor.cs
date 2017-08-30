using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class EmailProfaneTextContributor: ProfaneTextProvider
    {
        private static char[] delimiters = new char[] { '@', '.' };

        private string _emailId;
        ProfaneTextProvider _profaneTextProvider;

        public EmailProfaneTextContributor(ProfaneTextProvider profaneTextProvider, string emailId)
        {
            _profaneTextProvider = profaneTextProvider;
            _emailId = emailId;
        }

        public override string ProfaneText
        {
            get
            {
                string profaneText = _profaneTextProvider.ProfaneText;

                if (string.IsNullOrEmpty(_emailId))
                    return profaneText;

                string[] profaneTexts = _emailId.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

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
