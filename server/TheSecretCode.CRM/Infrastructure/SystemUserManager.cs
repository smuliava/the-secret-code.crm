using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Infrastructure
{
    public class SystemUserManager : UserManager<SystemUser>
    {
        public SystemUserManager(IUserStore<SystemUser> store)
            : base(store)
        { }

        public static SystemUserManager Create(IdentityFactoryOptions<SystemUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<AuthorizationDBContext>();
            var appUserManager = new SystemUserManager(new UserStore<SystemUser>(appDbContext));

            // Configure validation logic for usernames
            appUserManager.UserValidator = new UserValidator<SystemUser>(appUserManager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            //appUserManager.EmailService = new AspNetIdentity.WebApi.Services.EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<SystemUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return appUserManager;
        }
    }
}