using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebAPIWebClient.Models;
using Newtonsoft.Json;

namespace TestWebAPIWebClient.Controllers
{
    public class HomeController : Controller
    {
        //http://www.binaryintellect.net/articles/36efdcf6-8280-4ba6-abb3-f846147c1266.aspx

        private const string cookie_uuid = "UUID";
        private const string cookie_oauthaccesstoken = "OAuth.AccessToken";
        private const string cookie_oauthrefreshtoken = "OAuth.RefreshToken";

        [HttpGet]
        public ActionResult Index()
        {
            HomeData hd = null;
            Helper hp = new Helper();
            if (hd == null)
            {
                hd = hp.LoadControlDefaults();
                hd.SelectedService = String.Empty;
                hd.SelectedOperation = String.Empty;
                Session["metadata"] = hd.gatewayMetadata;
            }
            return View(hd);
        }

        [HttpGet]
        public ActionResult GetOperations(string service)
        {
            if (String.IsNullOrWhiteSpace(service))
            {
                throw new Exception("service cannot be empty");
            }

            GatewayMetadata gmeta = Session["metadata"] as GatewayMetadata;
            if (gmeta != null)
            {
                var api = gmeta.Metadata.Where(m => m.Key.ToLower().Equals(service.ToLower())).FirstOrDefault();

                if (api.Value != null)
                {
                    return Json(api.Value.operations, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRequestBody(string service, string operation)
        {
            APIRequest req = new APIRequest();

            if (String.IsNullOrWhiteSpace(service))
            {
                throw new Exception("service cannot be empty");
            }

            if (String.IsNullOrWhiteSpace(operation))
            {
                throw new Exception("operation cannot be empty");
            }

            GatewayMetadata gmeta = Session["metadata"] as GatewayMetadata;
            if (gmeta != null)
            {
                var api = gmeta.Metadata.Where(m => m.Key.ToLower().Equals(service.ToLower())).FirstOrDefault();

                if (api.Value != null)
                {
                    var opt = api.Value.operations.Where(o => o.name.ToLower().Equals(operation.ToLower())).FirstOrDefault();

                    if (opt != null)
                    {
                        req.service = service;
                        req.operation = operation;
                        req.parameters = new List<KeyValuePair<string, object>>();

                        opt.Parameters.ForEach(p => req.parameters.Add(new KeyValuePair<string, object>(p.name, "1234")));
                    }
                    else
                    {
                        throw new Exception("No operation located.");
                    }
                }
                else
                {
                    throw new Exception("No service located.");
                }
            }

            return Json(req, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateInput(false)] 
        public ActionResult Index_Post(HomeData hd)
        {
            ModelState.Clear();
            Helper hp = new Helper();
            try
            {
                var opName = Request.Form["operationDD"];

                LoginClaim logDetails = null;
                if (hd.RequireIdentityCheck)
                {
                    logDetails = hp.LoginToIdentity(hd.UserName, hd.Password);
                }

                string accesstoken = String.Empty;
                string uuid = String.Empty;

                if (logDetails != null)
                {
                    accesstoken = logDetails.access_token;
                    uuid = logDetails.uuid;
                }
                var result = hp.InvokeAPIGateway(accesstoken, uuid, hd.RequestBody, hd.APIGatewayURL);
                hd.Response = result.Item1;
                hd.Hash = result.Item2;
                hd.gatewayMetadata = Session["metadata"] as GatewayMetadata;
            }
            catch (Exception ex)
            {
                hd.Response = ex.ToString();
            }
            return View(hd);
        }

        [HttpGet]
        public ActionResult GetGatewayHeaders(string custid)
        {
            Helper hp = new Helper();
            try
            {
                TestUser tu = hp.GetTestUser(custid);
                LoginClaim logDetails = hp.LoginToIdentity(tu.username, tu.password);

                //LoginClaim logDetails = new LoginClaim() { access_token = "test", uuid = "test1"};

                string timestamp = hp.GetEpochTime(DateTime.UtcNow).ToString();
                string nonce = Guid.NewGuid().ToString();

                string normalizedUrl = String.Empty;
                string normalizedRequestParameters = String.Empty;

                OAuthBase oauth = new OAuthBase();

                string hash = oauth.GenerateSignature(
                            new Uri(hp._apigatewayUrl),
                            hp._publicKey,
                            hp._secretKey,
                            null, // totken
                            null, //token secret
                            "POST",
                            timestamp,
                            nonce,
                            out normalizedUrl,
                            out normalizedRequestParameters

                          );
                return Json(new
                {
                    accesstoken = logDetails.access_token,
                    uuid = logDetails.uuid,
                    publickey = hp._publicKey,
                    timestamp = timestamp,
                    nonce = nonce,
                    signature = hash,
                    custid = custid
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Login(string from)
        {
            var uuidCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_uuid);
            var accessTokenCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_oauthaccesstoken);

            if (uuidCookie != null && accessTokenCookie != null)
            {
                if (!String.IsNullOrWhiteSpace(from))
                {
                    return RedirectToAction("RedirectTo", "Home", new { target = Server.UrlEncode(from) });
                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }
            }

            if (!String.IsNullOrWhiteSpace(from))
            {
                ViewBag.ReturnUrl = from;
            }
            
            return View();
        }

        [HttpPost]
        [ActionName("Login")]
        public ActionResult Login_Post(string returnUrl)
        {
            SetCookie(cookie_uuid, "e12da3b3-bc5e-42a7-a8cf-71209e5d160b", DateTime.Now.AddHours(1));
            SetCookie(cookie_oauthaccesstoken, Guid.NewGuid().ToString(), DateTime.Now.AddHours(1));
            SetCookie(cookie_oauthrefreshtoken, Guid.NewGuid().ToString(), DateTime.Now.AddHours(1));

            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("RedirectTo", "Home", new { target = Server.UrlEncode(returnUrl) });
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }
        }

        [HttpGet]
        public ActionResult RedirectTo(string target)
        {
            ViewBag.Target = Server.UrlDecode(target);
            return View();
        }
         
        [HttpGet]
        public ActionResult Home()
        {
            var uuidCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_uuid);
            var accessTokenCookie = System.Web.HttpContext.Current.Request.Cookies.Get(cookie_oauthaccesstoken);

            if (uuidCookie == null || accessTokenCookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpGet]
        public ActionResult LogOut(string from)
        {
            try
            {
                ClearCookie(cookie_oauthaccesstoken);
                ClearCookie(cookie_oauthrefreshtoken);
                ClearCookie(cookie_uuid);

                return RedirectToAction("Login", new { from = from });
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private static void SetCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = default(HttpCookie);
            cookie = new HttpCookie(key, value);
            cookie.Expires = expires;

            //cookie.HttpOnly = true;
            //cookie.Secure = true;

            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }

        private static void ClearCookie(string key)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}
