using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TheSecretCode.CRM.Classes;
using TheSecretCode.CRM.Infrastructure;
using TheSecretCode.CRM.Providers;

[assembly: OwinStartup(typeof(TheSecretCode.CRM.WebApiApplication))]
namespace TheSecretCode.CRM
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public void Configuration(IAppBuilder application)
        {

            HttpConfiguration httpConfig = new HttpConfiguration();

            OAuthConfiguaration.Configuare(application);
            ConfigureWebApi(httpConfig);

            application.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            application.UseWebApi(httpConfig);

            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
