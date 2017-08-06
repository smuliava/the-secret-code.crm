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

            var daysInWork = (DateTime.Now.Date - user.RegistrationDate).TotalDays;

            if (daysInWork > 90)
            {
                claims.Add(CreateClaim("FTE", "1"));

            }
            else
            {
                claims.Add(CreateClaim("FTE", "0"));
            }

            return claims;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}