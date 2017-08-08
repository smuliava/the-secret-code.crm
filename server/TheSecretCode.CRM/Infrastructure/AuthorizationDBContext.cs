using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            string dbSystemUsersClaimsTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersClaimsTableName"];
            string dbSystemUsersLoginsTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersLoginsTableName"];
            string dbSystemUsersRolesTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersRolesTableName"];
            string dbSystemUserIdFieldName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUserIdFieldName"];

            modelBuilder.HasDefaultSchema(dbSchemaName);
            modelBuilder.Entity<SystemUser>().ToTable(dbSystemUsersTableName);

            modelBuilder.Entity<IdentityUserClaim>().ToTable(dbSystemUsersClaimsTableName);
            modelBuilder.Entity<IdentityUserClaim>().Property(entityProperty => entityProperty.UserId).HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserLogin>().ToTable(dbSystemUsersLoginsTableName);
            modelBuilder.Entity<IdentityUserLogin>().Property(entityProperty => entityProperty.UserId).HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserRole>().ToTable(dbSystemUsersRolesTableName);
            modelBuilder.Entity<IdentityUserRole>().Property(entityProperty => entityProperty.UserId).HasColumnName(dbSystemUserIdFieldName);
        }

        public static AuthorizationDBContext Create()
        {
            return new AuthorizationDBContext();
        }
    }
}