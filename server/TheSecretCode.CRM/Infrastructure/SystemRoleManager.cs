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
    public class SystemRoleManager : RoleManager<IdentityRole>
    {
        public SystemRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static SystemRoleManager Create(IdentityFactoryOptions<SystemRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new SystemRoleManager(new RoleStore<IdentityRole>(context.Get<AuthorizationDBContext>()));

            return appRoleManager;
        }
    }
}