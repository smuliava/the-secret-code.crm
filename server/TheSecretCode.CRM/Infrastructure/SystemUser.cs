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
        [MaxLength(100)]
        public string Firstname { get; set; }

        [MaxLength(100)]
        public string Lastname { get; set; }

        [MaxLength(100)]
        public string Middlename { get; set; }

        public byte Level { get; set; }

        [Required]
        public DateTime Registrationdate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SystemUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}