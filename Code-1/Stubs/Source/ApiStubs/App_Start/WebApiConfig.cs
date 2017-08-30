using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Headers;

namespace IdentityApiStub
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v3/{controller}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional }
            );



            config.Routes.MapHttpRoute(
                name: "DefaultAddressApi",
                routeTemplate: "v4/{controller}/{id}",
                defaults: new
                {action = RouteParameter.Optional,
                  id = RouteParameter.Optional
                }
            );
            config.Routes.MapHttpRoute(
                name: "findaddressApi",
                routeTemplate: "v4/{controller}/{id}/{aid}",
                defaults: new
                {
                    action ="postcodes",
                    id = RouteParameter.Optional,
                    aid = RouteParameter.Optional
                }
            );
            
            config.Routes.MapHttpRoute(
                name: "DefaultAuthApi",
                routeTemplate: "v3/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultContactApi",
                routeTemplate: "v1/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ContactApi",
                routeTemplate: "v1/{controller}/{action}/{id:string}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultOAuthApi",
                routeTemplate: "v3/api/{controller}/{action}/{v2}/{token}",
                defaults: new { id = RouteParameter.Optional, token = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
          name: "CECContactApi",
          routeTemplate: "v1/{controller}/{action}/{id}/{aid}",
          defaults: new { id = RouteParameter.Optional, aid = RouteParameter.Optional }
      );
          


            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
