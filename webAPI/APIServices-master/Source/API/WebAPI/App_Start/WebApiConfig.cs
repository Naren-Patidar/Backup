using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Tesco.ClubcardProducts.MCA.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "route-1",
                routeTemplate: "{controller}",
                defaults: new { action = "APIGateway", appkey = "" }
            );

            config.Routes.MapHttpRoute(
                name: "route-2",
                routeTemplate: "{controller}/{appkey}",
                defaults: new { appkey = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "route-3",
                routeTemplate: "{controller}/{action}/{appkey}",
                defaults: new { action = RouteParameter.Optional, appkey = RouteParameter.Optional }
            );
            
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            //config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects; 
            config.Formatters.JsonFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Populate;
        }
    }
}
