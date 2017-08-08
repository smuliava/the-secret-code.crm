using Microsoft.Owin;
using System;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using TheSecretCode.CRM.Models;
using Owin;
using System.Web.Http;

namespace TheSecretCode.CRM.Classes
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            //app.UseWebApi(config);
        }
    }

    public class Authentication
    {

    }

    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("AuthorizationContext")
        { }
    }

    public class AuthRepository : IDisposable
    {
        private AuthContext AuthContext = new AuthContext();
        private UserManager<IdentityUser> UserManager;

        public AuthRepository()
        {
            UserManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(AuthContext));
        }

        public async Task<IdentityResult> RegisterUser(AuthenticationModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            var result = await UserManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await UserManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            AuthContext.Dispose();
        }
    }
}