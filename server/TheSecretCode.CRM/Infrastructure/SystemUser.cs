using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TheSecretCode.CRM.Infrastructure
{
    public class SystemUser : IdentityUser
    {
        //[MaxLength(100)]
        //public string FirstName { get; set; }

        //[MaxLength(100)]
        //public string LastName { get; set; }

        //[MaxLength(100)]
        //public string MiddleName { get; set; }

        //public byte Level { get; set; }

        //[Required]
        //public DateTime RegistrationDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SystemUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }
    }
}