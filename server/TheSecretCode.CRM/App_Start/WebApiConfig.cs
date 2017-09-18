using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace TheSecretCode.CRM
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Menu",
                routeTemplate: "api/{controller}/{menuType}/{parentId}",
                defaults: new { parentId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Dictionary",
                routeTemplate: "api/{controller}/{setOf:regexp(^names$|^values$, \"i\")/{dictionaryNameId}",
                defaults: new { dictionaryName = RouteParameter.Optional }
            );
        }
    }
}
