using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Tesco.Com.Framework.Security.Tokens;
using APITestClient.Helper;

namespace APITestClient.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Property to get the curent culture name
        /// </summary>
        public string CurrentCulture
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.Name;
            }
        }

        /// <summary>
        /// Read only property to check if the dotcom environment is enabled
        /// </summary>
        public bool IsDotcomEnvironmentEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["IsDotcomEnvironmentEnabled"] == "0" ? true : false;
            }
        }

        /// <summary>
        /// Read Only property for the Dotcom Customer Id
        /// </summary>
        public string DotcomCustomerId
        {
            get
            {
                try
                {
                    string _dotcomCustId = string.Empty;
                    if (IsDotcomEnvironmentEnabled)
                    {
                        if (ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_UK"].Equals("MARTINI"))
                        {
                            if (HttpContext.Items["ReferenceMonitorIdentificationToken"] != null)
                            {
                                if (HttpContext.Items["ReferenceMonitorIdentificationToken"] != null
                                && ((AuthorisationToken)HttpContext.Items["ReferenceMonitorIdentificationToken"]).LoggedInSuccessfully == "1")
                                {
                                    _dotcomCustId = Convert.ToString(((IdentificationToken)HttpContext.Items["ReferenceMonitorIdentificationToken"]).CustomerID);
                                }
                            }
                        }
                        else if (ConfigurationManager.AppSettings["LOGIN_SOLUTION_TYPE_GROUP"].Equals("IGHS"))
                        {
                            if (HttpContext.Items["CustomerIdentity"] != null)
                            {
                                _dotcomCustId = HttpContext.Items["CustomerIdentity"].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (TempData["DotcomID"] != null)
                        {
                            _dotcomCustId = TempData["DotcomID"].TryParse<string>();
                        }
                    }
                    return _dotcomCustId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Read only property to get the MCA Customer Id
        /// </summary>
        public string CustomerId
        {
            get
            {
                try
                {
                    string sDotComCustomerID = this.DotcomCustomerId;
                    string _CustId = string.Empty;
                    if (MCACookie.Cookie[MCACookieEnum.DotcomCustomerID] == null ||
                        (MCACookie.Cookie[MCACookieEnum.DotcomCustomerID] != null && sDotComCustomerID != MCACookie.Cookie[MCACookieEnum.DotcomCustomerID]))
                    {
                        if (!string.IsNullOrWhiteSpace(sDotComCustomerID))
                        {
                            MCACookie.Cookie.Add(MCACookieEnum.DotcomCustomerID, sDotComCustomerID);

                            APIResponseHelper apiHelper = new APIResponseHelper();
                            Dictionary<string, string> headers = apiHelper.GetHeaders();

                            string data = "{\"service\":\"ClubcardService\",\"operation\":\"IGHSCheckCustomerActivated\"," +
                                                "\"parameters\":[{\"Key\":\"dotcomCustomerID\",\"Value\":\"" + sDotComCustomerID + "\"}]," +
                                                "{\"Key\":\"culture\",\"Value\":\"en-GB\"}]}";

                            APIResponse apiResponse = apiHelper.GetAPIResponse(data, headers);

                            string customerID = "1234";
                            if (!string.IsNullOrEmpty(customerID))
                            {
                                MCACookie.Cookie.Add(MCACookieEnum.CustomerID, customerID);
                            }
                        }
                    }
                    return MCACookie.Cookie[MCACookieEnum.CustomerID];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #region GetCustomerId Code

        public long GetCustomerId()
        {
            long customerId = 0;
            try
            {

                if (!string.IsNullOrEmpty(MCACookie.Cookie[MCACookieEnum.CustomerID]))
                {
                    customerId = Convert.ToInt64(MCACookie.Cookie[MCACookieEnum.CustomerID]);
                }
                return customerId;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        #endregion

        public string oAuthToken
        {
            get
            {
                return Request.Cookies["OAuth.AccessToken"] == null ? string.Empty : Request.Cookies["OAuth.AccessToken"].Value;
            }
        }
    }
}
