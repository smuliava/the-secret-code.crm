using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;

namespace TheSecretCode.CRM.Infrastructure
{
    public class AuthorizationDBContext : IdentityDbContext<SystemUser>
    {
        public AuthorizationDBContext()
            : base("AuthorizationContext", throwIfV1Schema: false)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            string dbSchemaName = ConfigurationManager.AppSettings["athentificatinon:DBSchemaName"];
            string dbSystemUsersTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersTableName"];
            modelBuilder.HasDefaultSchema(dbSchemaName);
            modelBuilder.Entity<SystemUser>().ToTable(dbSystemUsersTableName);

            modelBuilder.Entity<IdentityUserClaim>().ToTable("tblSystemUsersClaims");
            modelBuilder.Entity<IdentityUserClaim>().Property(entityProperty => entityProperty.UserId).HasColumnName("SystemUserId");

            modelBuilder.Entity<IdentityUserLogin>().ToTable("tblSystemUsersLogins");
            modelBuilder.Entity<IdentityUserLogin>().Property(entityProperty => entityProperty.UserId).HasColumnName("SystemUserId");

            modelBuilder.Entity<IdentityUserRole>().ToTable("tblSystemUsersRoles");
            modelBuilder.Entity<IdentityUserRole>().Property(entityProperty => entityProperty.UserId).HasColumnName("SystemUserId");
        }

        public static AuthorizationDBContext Create()
        {
            return new AuthorizationDBContext();
        }
    }
}