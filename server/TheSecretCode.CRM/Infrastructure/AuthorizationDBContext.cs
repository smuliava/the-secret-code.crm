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
using System.Data.Entity.Migrations.History;
using System.ComponentModel.DataAnnotations.Schema;
using TheSecretCode.CRM.Classes;
using System.Data.Entity.Infrastructure;

namespace TheSecretCode.CRM.Infrastructure
{
    public class AuthorizationDbContext : IdentityDbContext<SystemUser>
    {
        public AuthorizationDbContext()
            : base("AuthorizationContext", throwIfV1Schema: false)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            string dbSchemaName = ConfigurationManager.AppSettings["athentificatinon:DbSchemaName"];
            string dbSystemUsersTableName = ConfigurationManager.AppSettings["athentificatinon:DbSystemUsersTableName"];
            string dbSystemUsersClaimsTableName = ConfigurationManager.AppSettings["athentificatinon:DbSystemUsersClaimsTableName"];
            string dbSystemUsersLoginsTableName = ConfigurationManager.AppSettings["athentificatinon:DbSystemUsersLoginsTableName"];
            string dbSystemUsersRolesTableName = ConfigurationManager.AppSettings["athentificatinon:DbSystemUsersRolesTableName"];
            string dbSystemUserIdFieldName = ConfigurationManager.AppSettings["athentificatinon:DbSystemUserIdFieldName"];
            string dbRolesTableName = ConfigurationManager.AppSettings["athentificatinon:DbRolesTableName"];

            modelBuilder.HasDefaultSchema(dbSchemaName);

            modelBuilder.Entity<SystemUser>().ToTable(dbSystemUsersTableName);

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable(dbSystemUsersClaimsTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable(dbSystemUsersLoginsTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserRole>()
                .ToTable(dbSystemUsersRolesTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityRole>()
                .ToTable(dbRolesTableName);
        }

        public static AuthorizationDbContext Create()
        {
            return new AuthorizationDbContext();
        }
    }
}