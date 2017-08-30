using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using JWT;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security
{
    public class SecurityHelper
    {
        private ILoggingService _logger;

        public SecurityHelper()
        {
            this._logger = new LoggingService();
        }

        public SecurityDefinition GetTokenPayLoad()
        {
            SecurityDefinition secDef = null;
            LogData logData = new LogData(true);
            
            try
            {
                var token = System.Web.HttpContext.Current.Request.Cookies.Get(ParameterNames.JWT_TOKEN);

                if (token == null || String.IsNullOrWhiteSpace(token.Value))
                {
                    logData.RecordStep("Token empty in request. Checking response...");
                    token = System.Web.HttpContext.Current.Response.Cookies.Get(ParameterNames.JWT_TOKEN);
                    if (token == null || String.IsNullOrWhiteSpace(token.Value))
                    {
                        logData.RecordStep("Token empty. Cannot proceed.");
                        return secDef;
                    }
                }

                try
                {
                    secDef = JWTSerializerUtility.JWTDeserialize(token.Value);
                    logData.RecordStep("Token serialized");
                }
                catch (TokenExpiredException)
                {
                    logData.RecordStep("Token expired");
                }
                catch (SignatureVerificationException)
                {
                    logData.RecordStep("Token has invalid signature");
                }

                return secDef;
            }
            finally
            {
                this._logger.Submit(logData);
            }
        }

        internal string GetSecurityItem(string item)
        {
            SecurityDefinition secDef = this.GetTokenPayLoad();
            if (secDef == null)
            {
                return String.Empty;
            }
            return this.GetPropertyFromObject(item, secDef);
        }

        public string GetPropertyFromObject(string item, SecurityDefinition secDef)
        {
            try
            {
                return secDef.GetType().GetProperty(item).GetValue(secDef, null).ToString();
            }
            catch (Exception)
            {
                throw new MCASecurityException(SecurityErrors.E_401_13);
            }
        }
    }
}
