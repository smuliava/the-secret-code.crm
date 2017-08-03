using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Infrastructure
{
    public class AuthorizationDBContext : IdentityDbContext<SystemUser>
    {
        public AuthorizationDBContext()
            : base("AuthorizationContext", throwIfV1Schema: false)
        { }

        public static AuthorizationDBContext Create()
        {
            return new AuthorizationDBContext();
        }
    }
}