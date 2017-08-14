using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TheSecretCode.CRM.Infrastructure;
using TheSecretCode.CRM.Providers;

namespace TheSecretCode.CRM.Classes
{
    public static class OAuthConfiguaration
    {
        public static void Configuare(IAppBuilder application)
        {
            ConfigureOAuthTokenGeneration(application);
            ConfigureOAuthTokenConsumption(application);
        }
        private static void ConfigureOAuthTokenGeneration(IAppBuilder application)
        {
            // Configure the db context and user manager to use a single instance per request
            application.CreatePerOwinContext(AuthorizationDbContext.Create);
            application.CreatePerOwinContext<SystemUserManager>(SystemUserManager.Create);
            application.CreatePerOwinContext<SystemRoleManager>(SystemRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //TODO: remove next option after switch to HTTPS
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/authorization/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthProvider(),
                AccessTokenFormat = new SystemUserJwtFormat("http://localhost:59822")
            };

            // OAuth 2.0 Bearer Access Token Generation
            application.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private static void ConfigureOAuthTokenConsumption(IAppBuilder application)
        {

            var issuer = "http://localhost:59822";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            var jwtBearerAuthOptions = new JwtBearerAuthenticationOptions()
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audienceId },
                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                {
                    new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                }
            };
            application.UseJwtBearerAuthentication(jwtBearerAuthOptions);
        }
    }
}