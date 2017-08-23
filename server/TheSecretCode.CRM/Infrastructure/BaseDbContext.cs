using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TheSecretCode.CRM.Infrastructure
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext() 
            : base("BaseContext")
        {

        }
    }
}