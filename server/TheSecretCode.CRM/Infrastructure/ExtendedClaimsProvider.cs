using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using TheSecretCode.CRM.Infrastructure;

namespace TheSecretCode.CRM.Providers
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(SystemUser user)
        {
            List<Claim> claims = new List<Claim>();
            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}