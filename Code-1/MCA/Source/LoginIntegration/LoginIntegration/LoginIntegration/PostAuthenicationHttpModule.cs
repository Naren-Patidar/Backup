

using NGCTrace;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Web;
using LoginIntegration.CustomerIdentityService;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;

namespace LoginIntegration
{
    public class PostAuthenicationHttpModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        IConfigurationProvider _configProvider = null;
        
        public PostAuthenicationHttpModule()
        {
            this._configProvider = new ConfigurationProvider();
        }

        public const string ANON_COOKIE_NAME_CONFIG_KEY = "anonCookieName";
        public const string COOKIE_CONFIG_SECTION_NAME = "cookieInformation";

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += new EventHandler(this.PostAuthenticateRequest);
        }

        private void PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {

                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:PostAuthenticateRequest");
                HttpContextWrapper httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                HttpContext context = ((HttpApplication)sender).Context;
                string str1 = Path.GetExtension(httpContextWrapper.Request.Url.AbsoluteUri);

                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:PostAuthenticateRequest.str1:" + str1);

                string str3 = (sender as HttpApplication).Request.Path.ToLower();

                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:PostAuthenticateRequest.str3:" + str3);
                if (str3.Contains("join/") || str3.Contains("extlinks/"))
                {
                    NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:PostAuthenticateRequest: Value 2" + str3);
                }
                else
                {
                    string str4 = string.Empty;
                    if (string.IsNullOrEmpty(this.GetUserIdentityInfo((HttpContextBase)httpContextWrapper, true)))
                    {
                        
                        string url = _configProvider.GetStringAppSetting(AppConfigEnum.GenericLoginPage).ToString();
                        NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:PostAuthenticateRequest.Redirection to login page");
                        httpContextWrapper.Response.Redirect(url,true);
                    }
                    NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:PostAuthenticateRequest");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual string GetUserIdentityInfo(HttpContextBase httpContext, bool slidingExpiration)
        {
            DecryptCookieServiceClient cookieServiceClient = (DecryptCookieServiceClient)null;
            string str1 = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            try
            {
                

                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo");

                string valueforGivenKey = _configProvider.GetStringAppSetting(AppConfigEnum.anonCookieName);
                
                               
                if (_configProvider.GetStringAppSetting(AppConfigEnum.IsDotcomEnvironment).ToString().Equals("0"))
                {
                    string str5 = _configProvider.GetStringAppSetting(AppConfigEnum.CustomerEncodedValue).ToString();
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie(valueforGivenKey)
                    {
                        Value = str5,
                        Secure = true,
                        HttpOnly = false
                    });
                }
                HttpCookieCollection cookies = httpContext.Request.Cookies;
                if (cookies[valueforGivenKey] != null)
                {
                    NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:cookies[valueforGivenKey] is not null");
                    string str5 = cookies.Get(valueforGivenKey).Value;
                    NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:cookies[valueforGivenKey] is :" + str5);
                    if (str5 != null)
                    {
                        NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:cookies[valueforGivenKey] value is not null ");
                        cookieServiceClient = new DecryptCookieServiceClient();
                        string[] strArray = cookieServiceClient.GetDecodedCookie(new CustomerIdentity()
                        {
                            Encodedvalue = str5
                        }).DecodedValue.Split(new char[1]
                        {
                            ';'
                        });
                        NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:Decryptcookie servicecall");
                        if (strArray.Length == 2)
                        {
                            NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:strArray array length is two ");
                            string str6 = strArray[0];
                            NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:str6 " + str6);
                            HttpContext.Current.Items.Add((object)"CustomerIdentity", (object)str6);
                            NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:set customerIdentity to Context ");
                            return str6;
                        }
                        else
                        {
                            NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:strArray array length is not two ");
                        }
                    }
                    else
                    {
                        NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:cookies.Get(valueforGivenKey).Value is null ");
                    }
                }
                else
                {
                    NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo:cookies.Get(valueforGivenKey).Value is null ");
                }

                NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:GetUserIdentityInfo");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                if (cookieServiceClient != null)
                {
                    if (cookieServiceClient.State == CommunicationState.Faulted)
                        cookieServiceClient.Abort();
                    else if (cookieServiceClient.State != CommunicationState.Closed)
                        cookieServiceClient.Close();
                }
            }
            return (string)null;
        }

        public string GetConfigurationValueforGivenKey(string key, string configurationSection)
        {
            NameValueCollection nameValueCollection = ConfigurationManager.GetSection(configurationSection) as NameValueCollection;
            if (nameValueCollection == null)
                throw new ConfigurationErrorsException(string.Format("Section {0} not found in config ", (object)configurationSection));
            else
                return nameValueCollection[key];
        }

        public void OnLogRequest(object source, EventArgs e)
        {
        }
    }
}
