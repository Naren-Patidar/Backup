using System;
using System.Web;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using LoginIntegration.NGCDecryptService;


namespace LoginIntegration
{
    public class PostAuthenicationHttpModule : IHttpModule
    {
        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        public const string ANON_COOKIE_NAME_CONFIG_KEY = "anonCookieName";
        public const string COOKIE_CONFIG_SECTION_NAME = "cookieInformation";

        #region IHttpModule Members

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += new EventHandler(PostAuthenticateRequest);
        }

        #region PostAuthenticateRequest
        /// <summary>
        /// AuthenticateRequest for the customer checking his Identity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PostAuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:PostAuthenticateRequest");
                HttpContextWrapper httpContextWrapper = new HttpContextWrapper(HttpContext.Current);
                //Get the Request handler from the Context
                HttpContext Context = ((HttpApplication)sender).Context;
                string requestUri = httpContextWrapper.Request.Url.AbsoluteUri;
                string fileType = System.IO.Path.GetExtension(requestUri).Trim('.');

                //Following code is to ensure that authentication/identity need to be generated for dynamic file requests(.aspx) 
                var DynamicFileKeyValue = ConfigurationManager.AppSettings["RoutingFilters"].ToString();
                //To avoid join page get the Cookie value from IGHS Site as it is not required here in Join case.
                var appPath = sender as HttpApplication;
                //AppTotestLoginIntegration/Join/Confirmation.aspx
                //if (app.Request.Path.Contains(@"Join/"))
                //{
                //    return;
                //}
                string strJoinfiletype = appPath.Request.Path;
                if (strJoinfiletype.Contains(@"Join/"))
                {
                    return;
                }
                string identity = string.Empty;
                if (fileType == DynamicFileKeyValue)
                {
                    identity = GetUserIdentityInfo(httpContextWrapper, true);

                    //Following code is to ensure that identity value is not null if identity value is null redirect the user to login page
                    //In System Environment if value is null for the intital request we set some value
                    if (string.IsNullOrEmpty(identity))
                    {
                        //Not Authenticated user redirect him back to login page
                        var loginUrl = ConfigurationManager.AppSettings["GenericLoginPage"].ToString();
                        httpContextWrapper.Response.Redirect(loginUrl);
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:PostAuthenticateRequest");
                NGCTrace.NGCTrace.TraceDebug("End:LoginIntegration:PostAuthenticateRequest");
            }
            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoginIntegration :PostAuthenticateRequest - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoginIntegration :PostAuthenticateRequest - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:LoginIntegration :PostAuthenticateRequest");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
            finally
            {

            }
        }

        #endregion

        #region GetUserIdentityInfo
        /// <summary>
        /// Get the customerIdentity value for the encodedcookie value what is sent from IGHS login solution 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="slidingExpiration"></param>
        /// <returns></returns>
        public virtual string GetUserIdentityInfo(HttpContextBase httpContext, bool slidingExpiration)
        {
            DecryptCookieServiceClient objDecryptClient = null;
            string sCustomerIdentity = string.Empty;
            string encodedIdentityData = string.Empty;
            string cookieName = string.Empty;
            string strGUID = string.Empty;
            try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:LoginIntegration:GetUserIdentityInfo");
                cookieName = GetConfigurationValueforGivenKey(ANON_COOKIE_NAME_CONFIG_KEY, COOKIE_CONFIG_SECTION_NAME);
                //For the system testing Environment set the value as "0" and  pass some encoded cookie value for the NGC cookie service
                //When going INT,OPS,LIVE we will get the encoded cookie value from IGHS 
                if (ConfigurationManager.AppSettings["IsDotComEnvironment"].ToString().Equals("0"))
                {
                    string sEncodevalue = ConfigurationManager.AppSettings["CustomerEncodedValue"].ToString();
                    HttpCookie GUIDCookie = new HttpCookie(cookieName)
                    {
                        Value = sEncodevalue
                    };
                    HttpContext.Current.Response.Cookies.Add(GUIDCookie);
                }
                HttpCookieCollection cookies = httpContext.Request.Cookies;
                //Check cookie does exist
                if (cookies[cookieName] != null)
                {
                    HttpCookie anonCookie = cookies.Get(cookieName);
                    //Get the encoded cookie value
                    encodedIdentityData = anonCookie.Value;
                    if (encodedIdentityData != null)
                    {
                        objDecryptClient = new DecryptCookieServiceClient();
                        CustomerIdentity objCus = new CustomerIdentity();
                        objCus.Encodedvalue = encodedIdentityData;
                        //Calling the NGCDecodeCookie service to get the customer identity value
                        objCus = objDecryptClient.GetDecodedCookie(objCus);
                        sCustomerIdentity = objCus.DecodedValue;
                        string[] identityValues = sCustomerIdentity.Split(';');
                        if (identityValues.Length == 2)
                        {
                            //Split the value and get the customer Identity value and store here
                            strGUID = identityValues[0];
                            HttpContext.Current.Items.Add("CustomerIdentity", strGUID);
                            return strGUID;
                        }
                    }
                }
                NGCTrace.NGCTrace.TraceInfo("End:LoginIntegration:GetUserIdentityInfo");
                NGCTrace.NGCTrace.TraceDebug("End:LoginIntegration:GetUserIdentityInfo");
            }

            catch (Exception exp)
            {
                NGCTrace.NGCTrace.TraceCritical("Critical:LoginIntegration:GetUserIdentityInfo - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceError("Error:LoginIntegration:GetUserIdentityInfo - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:Home.LoginIntegration:GetUserIdentityInfo");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;

            }
            finally
            {
                if (objDecryptClient != null)
                {
                    if (objDecryptClient.State == CommunicationState.Faulted)
                    {
                        objDecryptClient.Abort();
                    }
                    else if (objDecryptClient.State != CommunicationState.Closed)
                    {
                        objDecryptClient.Close();
                    }
                }
            }
            return null;
        }
        #endregion

        #region GetConfigurationValueforGivenKey
        /// <summary>
        /// Method is used to get the cookie name  from the webconfig
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configurationSection"></param>
        /// <returns></returns>
        public string GetConfigurationValueforGivenKey(string key, string configurationSection)
        {
            var cookieInformation = ConfigurationManager.GetSection(configurationSection) as System.Collections.Specialized.NameValueCollection;

            if (cookieInformation == null)
            {
                throw new ConfigurationErrorsException(string.Format("Section {0} not found in config ", configurationSection));
            }

            var configValue = cookieInformation[key];
            return configValue;
        }
        #endregion

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //custom logging logic can go here
        }
    }
}
