using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public class ValidateGoogleCaptcha
    {
        public static ValidateGoogleCaptcha Validate(string encodedResponse)
        {
            var client = new System.Net.WebClient();

            string sProxyURL = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                                [AppConfigEnum.ProxyForGCConnect.ToString()]
                                                                .ConfigurationValue1;

            if (!String.IsNullOrWhiteSpace(sProxyURL))
            {
                client.Proxy = new WebProxy(sProxyURL);
            }

            string sPrivateKey = DBConfigurationManager.Instance[DbConfigurationTypeEnum.AppSettings]
                                                                [AppConfigEnum.GCSecretKey.ToString()]
                                                                .ConfigurationValue1;

            sPrivateKey = CryptoUtility.DecryptTripleDES(sPrivateKey);

            var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", sPrivateKey, encodedResponse));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ValidateGoogleCaptcha>(GoogleReply);

            return captchaResponse;
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        private List<string> m_ErrorCodes;

    }
}