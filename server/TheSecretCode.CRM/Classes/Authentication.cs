using Microsoft.Owin;
using System;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using TheSecretCode.CRM.Models;
using Owin;
using System.Web.Http;
using TheSecretCode.CRM.Infrastructure;

namespace TheSecretCode.CRM.Classes
{
    public class AuthRepository : IDisposable
    {
        private AuthorizationDbContext _authContext;
        private UserManager<SystemUser> _userManager;

        public AuthRepository()
        {
            _authContext = new AuthorizationDbContext();
            var userStore = new UserStore<SystemUser>(_authContext);
            _userManager = new UserManager<SystemUser>(userStore);
        }

        public async Task<IdentityResult> RegisterUser(AuthenticationModel userModel)
        {
            SystemUser user = new SystemUser
            {
                UserName = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public async Task<SystemUser> FindUser(string userName, string password)
        {
            SystemUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _authContext.Dispose();
        }
    }
}