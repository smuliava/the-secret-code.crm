using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TheSecretCode.CRM.Models;

namespace TheSecretCode.CRM.Infrastructure
{
    public class DictionaryDbContext : BaseDbContext
    {
        DbSet<DictionaryName> DictionaryName { get; set; }
        DbSet<DictionaryValue> DictionaryValue { get; set; }
    }
}